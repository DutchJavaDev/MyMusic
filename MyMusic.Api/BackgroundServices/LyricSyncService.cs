
using Dapper;
using MyMusic.Api.Services;
using MyMusic.Common.Models;
using SharpCompress.Common;
using System.Data;
using static MyMusic.Common.CommonData;

namespace MyMusic.Api.BackgroundServices
{

  // Use Speech service to conver the audi to text with time-stamps
  // Then update the lyrics database by adding a new entry for the music
  // If that all works with timestamps and all then the frontend can attempt to sync the text while playing
  // the audio
  public sealed class LyricSyncService(IServiceProvider _serviceProvider) : BackgroundService
  {
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      // query lyrics table for ids
      // query music table for music_ids
      var sql = @"select m.serial Serial, mm.file_path as FilePath from music m
                  left join lyric_snippet l on l.music_serial = m.serial
                  inner join download d on d.music_serial = m.serial
                  inner join mp3media as mm ON mm.download_serial = d.serial
                  where d.state = @state
                  and l.serial is null
                  order by m.created_utc desc
                  limit 1".Trim();

      var parameters = new { state = (int)Mp3State.Done };

      using IServiceScope scope = _serviceProvider.CreateScope();

      try
      {
        using var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();

        var model = (await connection.QueryAsync<LyricModel>(sql, parameters)).FirstOrDefault();

        var httpFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();

        var client = httpFactory.CreateClient();

        // Can take long before its done
        //client.Timeout = TimeSpan.FromSeconds(120); // 2 minutes should be enough :)

        // hardcoded url
        const string url = @"";

        using var formData = new MultipartFormDataContent();
        await using var fileStream = new FileStream(model.FilePath, FileMode.Open);
        formData.Add(new StreamContent(fileStream), "file", Path.GetFileName(model.FilePath));
        formData.Add(new StringContent("whisper-1"), "model");

        // Send
        var response = await client.PostAsync(url, formData);

        if (response.IsSuccessStatusCode)
        {
          var content = await response.Content.ReadAsStringAsync();

          Console.WriteLine(content);
        }
        else
        {
          Console.WriteLine(response);
        }
      }
      catch (Exception e)
      {
        var logger = scope.ServiceProvider.GetRequiredService<DbLogger>();
        await logger.LogAsync(e);
      }
    }
  }
}
