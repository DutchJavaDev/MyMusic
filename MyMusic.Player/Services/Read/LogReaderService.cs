using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;

namespace MyMusic.Player.Services.Read
{
	public sealed class LogReaderService(LocalDatabase localDatabase)
	{
		public async Task<IEnumerable<Log>> GetLogsAsync()
		{
			var query = "select * from Log order by Serial desc";
			return await localDatabase.QueryAsync<Log>(query);
		}

		public async Task<IEnumerable<Log>> GetLogsAsync(byte logType)
		{
			var query = "select * from Log where LogType=? order by Serial desc";
			return await localDatabase.QueryAsync<Log>(query,	logType);
		}
	}
}
