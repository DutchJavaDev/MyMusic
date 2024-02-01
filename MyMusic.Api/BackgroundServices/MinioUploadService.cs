namespace MyMusic.Api.BackgroundServices
{
    public sealed class MinioUploadService(IServiceProvider _serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
        }

         private Task<IEnumerable<MusicDownload>> GetNextUploadsAsync(IDbConnection connection)
        {
            var query = @"select d.serial as DownloadId, m.name as Name, d.state as State, 
                                    d.video_id as VideoId, mm.file_path as FilePath
                                    from download as d
                                    inner join music as m ON m.serial = d.music_serial
                                    inner join mp3media as mm ON mm.download_serial = d.serial
                                    where d.state = @state
                                    order by m.created_utc asc
                                    limit @limit;".Trim();

            var param = new { state = (int)Mp3State.Converted, limit = MaxConcurrentDownloads };

            return connection.QueryAsync<MusicDownload>(query, param);
        }
    }
}