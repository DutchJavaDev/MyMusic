using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;

namespace MyMusic.Player.Services.Write
{
	public sealed class ArtistWriterService(LocalDatabase database)
	{
		public async Task AddArtistAsync(Artists artists)
		{
			await database.InsertAsync(artists).ConfigureAwait(false);
		}

		public async Task UpdateArtistAsync(Artists artists)
		{
			await database.UpdateAsync(artists).ConfigureAwait(false);
		}
	}
}
