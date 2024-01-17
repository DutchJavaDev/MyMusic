using Dapper;
using MediaToolkit;
using MediaToolkit.Model;
using MyMusic.Api.Services;
using System.Data;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using static MyMusic.Common.CommonData;

namespace MyMusic.Api.BackgroundServices
{
    public sealed class DownloadRequestService(IServiceProvider _serviceProvider) : BackgroundService
    {
        public readonly int MaxConcurrentDownloads = 2; // read from db

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            while (!stoppingToken.IsCancellationRequested)
            {
                using var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();

                var downloads = await GetNextDownloadsAsync(connection);

                var youtubeClient = new YoutubeClient();

                var downloadTasks = downloads.Select(download => DownloadTask(download, youtubeClient, scope));

                await Task.WhenAll(downloadTasks);
            }
        }

        private static async Task DownloadTask(
            MusicDownload download,
            YoutubeClient client,
            IServiceScope scope)
        {
            using var logger = scope.ServiceProvider.GetRequiredService<DbLogger>();
            using var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();

            try
            {
                var videoUrl = string.Concat("https://youtube.com/watch?v=", download.VideoId);

                // Get stream manifest and get the moest important stream..... audio
                var streamManifest = await client.Videos.Streams.GetManifestAsync(videoUrl);

                var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

                // TODO: Pre create file path instead of here
                var filePath = Utils.GenerateDownloadPath();

                // Download the stream to a file
                await client.Videos.Streams.DownloadAsync(streamInfo, filePath);

                // Update state
                await UpdateStatusAsync(download.DownloadId, Mp3State.Downloaded, connection);

                // Convert to mp3
                // Changes extension to mp3..
                filePath = ConvertToMp3(filePath);

                // Create mp3 entry
                await InsertMp3EntryAsync(download.DownloadId, filePath, connection);

                // Update state
                await UpdateStatusAsync(download.DownloadId, Mp3State.Converted, connection);

            }
            catch (Exception e)
            {
                // Update state
                // add retry-service for this to work without breaking stuf
                //await UpdateStatusAsync(download.MusicId, Mp3State.Failed, connection);

                await logger.LogAsync(e, messagePrefix: $"Download failed: {download.Name}");
            }
        }

        private Task<IEnumerable<MusicDownload>> GetNextDownloadsAsync(IDbConnection connection)
        {
            var query = @"select d.serial as DownloadId, m.name as Name, d.state as State, 
                                    d.video_id as VideoId
                                    from download as d
                                    inner join music as m ON m.serial = d.music_serial
                                    where d.state = @state
                                    order by m.created_utc asc
                                    limit @limit;".Trim();

            var param = new { state = (int)Mp3State.Created, limit = MaxConcurrentDownloads };

            return connection.QueryAsync<MusicDownload>(query, param);
        }

        private static Task<int> UpdateStatusAsync(int? id, Mp3State nstate, IDbConnection connection)
        {
            var query = @"update download set state = @nstate where serial = @serial;";
            var param = new
            {
                nstate = (int)nstate,
                serial = id
            };

            return connection.ExecuteAsync(query, param);
        }

        private static Task<int> InsertMp3EntryAsync(int? id, string path, IDbConnection connection)
        {
            var query = @"insert into mp3media(download_serial, file_path, created_utc)
                          values(@id, @path, now())";

            var param = new
            {
                id,
                path,
            };

            return connection.ExecuteAsync(query, param);
        }

        private static string ConvertToMp3(string mp4Path)
        {
            var input = new MediaFile { Filename = mp4Path };
            var output = new MediaFile { Filename = Utils.GenerateSourePath() };

            using var engine = new Engine();
            // engine.ConvertProgressEvent track progress
            // create seperate table to update the download progress
            engine.GetMetadata(input);
            engine.Convert(input, output);

            // Cleanup
            File.Delete(mp4Path);

            return output.Filename;
        }
    }
}
