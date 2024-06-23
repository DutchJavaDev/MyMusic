using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;
using static MyMusic.Common.CommonData;

namespace MyMusic.Player.Services.Write
{
	public sealed class SongStatusWriterService(LocalDatabase localDatabase)
	{
		public async Task InsertStatus(SongStatus songStatus)
		{
			await localDatabase.InsertAsync(songStatus).ConfigureAwait(false);
		}
		
		public async Task UpdateStatus(SongStatus songStatus)
		{
			await localDatabase.UpdateAsync(songStatus).ConfigureAwait(false);
		}

		public async Task UpdateStatusToDone(List<Guid> ids)
		{
			var query = $"update SongStatus set Status=? where TrackingId=?";
			foreach (var id in ids)
			{
				await localDatabase.ExecuteAsync(query,(int)Mp3State.Done,id.ToString()).ConfigureAwait(false);
			}
		}
	}
}
