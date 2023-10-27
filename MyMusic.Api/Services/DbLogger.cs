using Dapper;
using MyMusic.Api.Models.DbLogger;
using System.Data;

namespace MyMusic.Api.Services
{
    public sealed class DbLogger
    {
        private readonly IDbConnection _dbConnection;
        public DbLogger(IDbConnection dbConnection) 
        {
            _dbConnection = dbConnection;
        }

        public async Task LogAsync(DbLog dbLog)
        {
            var sql = @"INSERT INTO mymusic.exception(message, stacktrace)
	                        VALUES (@Message, @StackTrace);";
            var param = new 
            {
                dbLog.Message,
                dbLog.StackTrace
            };

            using(_dbConnection)
            {
                await _dbConnection.ExecuteAsync(sql, param);
            }
        }
    }
}
