using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;
using static MyMusic.Common.CommonData;

namespace MyMusic.Player.Services.Read
{
	public sealed class SongStatusReaderService(LocalDatabase localDatabase)
	{
		public async Task<List<Guid>> GetTrackingIdsAsync()
		{
			var query = "select * from SongStatus where Status<?";
			// This is dumb, sqlite does not support List<string>....
			var result = await localDatabase.QueryAsync<SongStatus>(query, (int)Mp3State.Done);
			return result.Where(i => !string.IsNullOrEmpty(i.TrackingId))
				.Select(i => Guid.Parse(i.TrackingId)).ToList() ?? [];
		}
	}
}
