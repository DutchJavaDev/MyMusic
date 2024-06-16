using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;

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
	}
}
