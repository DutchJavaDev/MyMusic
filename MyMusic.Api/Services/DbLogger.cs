using Dapper;
using System.Data;

namespace MyMusic.Api.Services
{
    public sealed class DbLogger : IDisposable
    {
        private readonly IDbConnection _dbConnection;
        public DbLogger(IDbConnection dbConnection) 
        {
            _dbConnection = dbConnection;
        }

        public async Task LogAsync(Exception e, string messagePrefix = "")
        {
            var sql = @"INSERT INTO mymusic.exception(message, stacktrace)
	                        VALUES (@Message, @StackTrace);";
            var param = new 
            {
                Message = $"{messagePrefix} {e.Message}",
                e.StackTrace
            };

            using(_dbConnection)
            {
                await _dbConnection.ExecuteAsync(sql, param);
            }
        }

        public async Task LogAsync(string message, string stacktrace = "")
        {
            var sql = @"INSERT INTO mymusic.exception(message, stacktrace)
	                        VALUES (@Message, @StackTrace);";
            var param = new
            {
                Message = message,
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
