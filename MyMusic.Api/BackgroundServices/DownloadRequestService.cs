using System.Data;
using Dapper;
using static MyMusic.Api.Utils;
using VideoLibrary;
using MyMusic.Api.Services;

namespace MyMusic.Api.BackgroundServices
{
    public sealed class DownloadRequestService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DownloadRequestService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            using var logger = _serviceProvider.GetRequiredService<DbLogger> ();
            using var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
            
            while(!stoppingToken.IsCancellationRequested)
            {
                var download = (await GetNextDownloadAsync(connection))
                    .FirstOrDefault();

                if(download != null)
                {
                    try
                    {
                        // Download
                        var video = await Downloadvideo(download);

                        // Write to disk
                        var path = video.WriteToDownloads();

                        // Create mp3 entry
                        await InsertMp3EntryAsync(download.MusicId, path, connection);

                        // Update state
                        await UpdateStatusAsync(download.MusicId, Mp3State.Downloaded, connection);
                        // Done
                    }
                    catch (Exception e)
                    {
                        await logger.LogAsync(e);
                    }
                }
            }
        }

        private static Task<IEnumerable<MusicDownload>> GetNextDownloadAsync(IDbConnection connection)
        {
            var query = @"select d.serial as MusicId, m.name as Name, d.state as State, 
                                    d.download_id as Id
                                    from mymusic.download as d
                                    inner join mymusic.music as m ON m.serial = d.music_serial
                                    order by d.created_utc asc
                                    limit 1;".Trim();

            var param = new { state = (int)Mp3State.Created };

            return connection.QueryAsync<MusicDownload>(query, param);
        }

        private static Task<YouTubeVideo> Downloadvideo(MusicDownload download)
        {
            var downloadUrl = string.Concat("https://youtube.com/watch?v=", download.Id);

            using var downloader = Client.For(YouTube.Default);

            return downloader.GetVideoAsync(downloadUrl);
        }

        private static Task UpdateStatusAsync(int? id, Mp3State nstate, IDbConnection connection)
        {
            var query = @"update mymusic.download as d set d.status = @nstate where serial = @serial;";
            var param = new { 
                nstate = (int)nstate,
                serial = id
            };

            return connection.QueryAsync(query, param); 
        }

        private static Task InsertMp3EntryAsync(int? id, string path, IDbConnection connection)
        {
            var query = @"insert into mymusic.mp3media(download_serial, file_path, created_utc)
                          values(@id, @path, now())";

            var param = new 
            {
                id,
                path,
            };

            return connection.ExecuteAsync(query, param);
        }
    }

    internal class MusicDownload 
    {
        public int? MusicId { get; set; }
        public string? Name { get; set; }
        public Mp3State? State { get; set; }
        public string? Id { get; set; }
    }
}
