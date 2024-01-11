using Dapper;
using System.Data;

namespace MyMusic.Api.Services
{
    public sealed class DbLogger : IDisposable
    {
        private readonly IDbConnection _dbConnection;
        private readonly string AppName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
        public DbLogger(IDbConnection dbConnection) 
        {
            _dbConnection = dbConnection;
        }

        public async Task LogAsync(Exception e, string messagePrefix = "")
        {
            var sql = @"INSERT INTO mymusic.exception(message, app, stacktrace)
	                        VALUES (@Message, @AppName @StackTrace);";
            var param = new 
            {
                Message = $"{messagePrefix} {e.Message}",
                AppName,
                e.StackTrace
            };

            using(_dbConnection)
            {
                await _dbConnection.ExecuteAsync(sql, param);
            }
        }

        public async Task LogAsync(string message, string stacktrace = "")
        {
            var sql = @"INSERT INTO mymusic.exception(message, app, stacktrace)
	                        VALUES (@Message, @AppName @StackTrace);";
            var param = new
            {
                Message = message,
                AppName,
                StackTrace = stacktrace
            };

            using (_dbConnection)
            {
                await _dbConnection.ExecuteAsync(sql, param);
            }
        }

        public void Dispose()
        {
            _dbConnection?.Dispose();
        }
    }
}
