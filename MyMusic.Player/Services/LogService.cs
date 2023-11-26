using MyMusic.Player.Blazor.Models.Logging;
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

        public Task Log<T>(Exception exception, T t) where T : class
        {
            // Get type name
            var type = t.GetType();
            var name = type.Name;

            // Format message as type name : exception message
            var message = string.Concat(name, " ", exception.Message);

            _ = _connection.InsertAsync(new LogEntry 
            {
                Message = message,
                StackTrace = exception.StackTrace,
                Created = DateTime.Now,
            });
            
            return Task.CompletedTask;
        }

        public async Task<List<LogEntry>> GetLogsAsync()
        {
            return await _connection.QueryAsync<LogEntry>("select * from logs order by created desc");
        }
    }
}
