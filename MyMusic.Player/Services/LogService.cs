using MyMusic.Player.Blazor.Models.Logging;
using MyMusic.Player.Storage.Models;
using SQLite;

namespace MyMusic.Player.Services
{
    public sealed class LogService(SQLiteAsyncConnection connection)
    {
        private readonly SQLiteAsyncConnection _connection = connection;

        public Task WriteLogAsync(LogEntry logEntry)
        {
            // This should be fire and forget, don't want to hang the ui
            _ = _connection.InsertAsync(logEntry)
                .ConfigureAwait(false);

            return Task.CompletedTask;
        }

        public async Task<List<LogEntry>> GetLogsAsync()
        {
            return await _connection.QueryAsync<LogEntry>("select * from logs order by created desc");
        }
    }
}
