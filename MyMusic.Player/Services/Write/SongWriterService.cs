using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;

namespace MyMusic.Player.Services.Write
{
	public sealed class SongWriterService(LocalDatabase database)
	{
		public async Task InsertSongAsync(Song song)
		{
			await database.InsertAsync(song).ConfigureAwait(false);
		}
	}
}
