using Dapper;
using MyMusic.Api.Services;
using System.Data;
using MediaToolkit;
using static MyMusic.Api.Utils;
using MediaToolkit.Model;

namespace MyMusic.Api.BackgroundServices
{
    public sealed class Mp3ConvertService : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;

        public Mp3ConvertService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            using var logger = _serviceProvider.GetRequiredService<DbLogger>();
            using var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();

            while (!stoppingToken.IsCancellationRequested) 
            {
                var next = (await GetNextConversionAsync(connection))
                    .FirstOrDefault();

                if (next != null) 
                {
                    try
                    {
                        var inputFile = new MediaFile { Filename = next.FilePath };
                        var outputFile = new MediaFile { Filename = CreateMp3Extension(next.FilePath) };
                        
                        // Convert
                        Convert(inputFile, outputFile);

                        // Update file path
                        await UpdateMp3FilePath(next.DownloadId, outputFile.Filename, connection);

                        // Update state
                        await UpdateDownloadStatusAsync(next.DownloadId, Mp3State.Converted, connection);
                        // Done
                    }
                    catch (Exception e)
                    {
                        await logger.LogAsync(e);
                    }
                }
            }
        }

        private static Task UpdateDownloadStatusAsync(int? id, Mp3State nstate, IDbConnection connection)
        {
            var query = @"update mymusic.download as d set d.status = @nstate where serial = @serial;";
            var param = new { nstate = (int)nstate, serial = id };
            return connection.QueryAsync(query, param);
        }

        private static Task UpdateMp3FilePath(int? id, string path, IDbConnection connection)
        {
            var query = @"update mymusic.mp3media as m set m.file_path = @npath where download_serial = @id";
            var param = new { path, id };
            return connection.ExecuteAsync(query, param);
        }

        private static void Convert(MediaFile input, MediaFile output)
        {
            using var engine = new Engine();
            // engine.ConvertProgressEvent track progress
            // create seperate table to update the download progress
            engine.GetMetadata(input);
            engine.Convert(input, output);
        }

        private static Task<IEnumerable<AudioConversion>> GetNextConversionAsync(IDbConnection connection)
        {
            var query = @"select m.download_serial as DownloadId, m.file_path as FilePath
                          from mymusic.mp3media m
                          inner join mymusic.download as d on m.download_serial = d.serial
                          and d.state = @state
                          order by m.created_utc desc
                          limit 1;";

            var param = new
            {
                state = (int)Mp3State.Downloaded
            };

            return connection.QueryAsync<AudioConversion>(query, param); 
        }

        private static string CreateMp3Extension(string originalName)
        {
            var nName = originalName.Split('.')[0];
            return string.Concat(nName, ".mp3");
        }
    }

    internal class AudioConversion
    {
        public string? FilePath { get; set; }
        public int? DownloadId { get; set; }
    }
}
