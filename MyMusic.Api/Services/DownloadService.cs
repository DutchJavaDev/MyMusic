using Dapper;
using MyMusic.Api.Models.PipelineService;
using System.Data;
using static MyMusic.Api.Utils;

namespace MyMusic.Api.Services
{
    public sealed class DownloadService
    {
        private readonly DbLogger _logger;
        private readonly IDbConnection _connection;
        private readonly MusicService _musicService;

        public DownloadService(
            DbLogger logger,
            IDbConnection connection,
            MusicService music) 
        {
            _connection = connection;
            _musicService = music;
            _logger = logger;
        }

        public async Task CreateDownloadRequest(DownloadRequest request)
        {
            using (_connection) 
            {
                _connection.Open();
                using var transaction = _connection.BeginTransaction();
                try
                {
                    var music_id = await _musicService
                        .TryInsertMusicPreDownloadAsync(request.Name, transaction.Connection ?? _connection);

                    await InserDownload(request, transaction, music_id);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    await _logger.LogAsync(e, messagePrefix: "Failed to create download request");
                }
            }
        }

        private async Task InserDownload(DownloadRequest request, IDbTransaction transaction, int music_id)
        {
            var state = (int)Mp3State.Created;
            var download_id = request.DownloadId;

            var query = @"INSERT INTO mymusic.download(music_serial, state, download_id)
	                    VALUES (@music_id, @state, @download_id);";

            var param = new
            {
                music_id,
                state,
                download_id,
            };

            await _connection.ExecuteAsync(query, param,transaction);
            transaction.Commit();
        }
    }
}
