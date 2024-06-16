using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;

namespace MyMusic.Player.Services.Read
{
	public sealed class ArtistReaderService(LocalDatabase database)
	{
		public async Task<Artists> GetArtistsByNameAsync(string name)
		{
			var query = "select * from artists where name=?";
			return (await database.QueryAsync<Artists>(query, name).
				ConfigureAwait(false)).
				FirstOrDefault() ?? null;
		}

		public async Task<IEnumerable<ArtistsSongsModel>> GetArtistsAsync()
		{
			var query = @"SELECT 
    a.Serial, 
    a.Name, 
    a.ImageUrl, 
    IFNULL(song_count.SongCount, 0) AS SongCount
FROM 
    artists a
LEFT JOIN 
    (SELECT 
         s.ArtistsSerial, 
         COUNT(*) AS SongCount 
     FROM 
         song s 
     GROUP BY 
         s.ArtistsSerial) AS song_count
ON 
    a.Serial = song_count.ArtistsSerial;";

			return (await database.QueryAsync<ArtistsSongsModel>(query));
		}
	}
}
