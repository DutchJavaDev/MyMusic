using Dapper;
using MongoDB.Driver;
using MyMusic.Api.Services;
using MyMusic.Common.Models;
using System.Data;
using static MyMusic.Common.CommonData;

namespace MyMusic.Api.BackgroundServices
{
  public sealed class MongoDbUploadService(IServiceProvider _serviceProvider) : BackgroundService
  {
    private readonly int MaxConcurrentDownloads = 2; // read from db

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      using IServiceScope scope = _serviceProvider.CreateScope();
      await EnsureCollectionCreation(scope);

      while (!stoppingToken.IsCancellationRequested)
      {
        try
        {
          using var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();

          var uploads = await GetNextUploadsAsync(connection);

          var uploadTasks = uploads.Select(upload => UploadTask(upload, scope));

          await Task.WhenAll(uploadTasks);
        }
        catch (Exception e)
        {
          var logger = scope.ServiceProvider.GetRequiredService<DbLogger>();
          await logger.LogAsync(e);
        }
      }
    }

    private static async Task UploadTask(DownloadObject upload, IServiceScope scope)
    {
      using var logger = scope.ServiceProvider.GetRequiredService<DbLogger>();
      using var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
      var client = scope.ServiceProvider.GetRequiredService<MyMusicCollectionService>();

      try
      {
        await logger.LogAsync($"Start Upload: {upload.TrackId}");

        var bytes = File.ReadAllBytes(upload.FilePath ?? "");

        var trackToUpload = new MyMusicObject
        {
          Name = upload.Name,
          BinaryData = new(bytes),
          TrackId = upload.TrackId ?? throw new ArgumentNullException(nameof(upload.TrackId), "TrackId has not been set."), // satisfy idea
          Uploaded = DateTime.UtcNow
        };

        await client.UploadAsync(trackToUpload);

        await UpdateStatusAsync(upload.DownloadId, Mp3State.Uploaded, connection);

        // Delete filestream from disk since its now in mongo
        await Utils.DeleteFileAsync(upload.FilePath ?? "", logger);

        // Then ---> its done
        await UpdateStatusAsync(upload.DownloadId, Mp3State.Done, connection);

        // remove file path from mp3media table?
      }
      catch (Exception e)
      {
        await UpdateStatusAsync(upload.DownloadId, Mp3State.Failed, connection);
        await logger.LogAsync(e);
      }
    }

    private static async Task EnsureCollectionCreation(IServiceScope scope)
    {
      var client = scope.ServiceProvider.GetRequiredService<MongoClient>();
      var databases = await client.ListDatabaseNamesAsync();
      var exists = (await databases.ToListAsync())
        .Where(i => i == MyMusicCollectionService.DatabaseName)
        != null;

      if (!exists)
      {
        // Create db
        var db = client.GetDatabase(MyMusicCollectionService.DatabaseName);

        // Create collection
        await db.CreateCollectionAsync(MyMusicCollectionService.CollectionName);
      }
    }

    private async Task<IEnumerable<DownloadObject>> GetNextUploadsAsync(IDbConnection connection)
    {
      var query = @"select d.serial as DownloadId, m.name as Name, d.state as State,
                                    d.video_id as VideoId, mm.file_path as FilePath,
                                    m.tracking_id as TrackId
                                    from download as d
                                    inner join music as m ON m.serial = d.music_serial
                                    inner join mp3media as mm ON mm.download_serial = d.serial
                                    where d.state = @state
                                    order by m.created_utc asc
                                    limit @limit;".Trim();

      var param = new { state = (int)Mp3State.Converted, limit = MaxConcurrentDownloads };

      return await connection.QueryAsync<DownloadObject>(query, param);
    }

    private static Task<int> UpdateStatusAsync(int? id, Mp3State nstate, IDbConnection connection)
    {
      const string query = "update download set state = @nstate where serial = @serial;";
      var param = new
      {
        nstate = (int)nstate,
        serial = id
      };

      return connection.ExecuteAsync(query, param);
    }
  }
}