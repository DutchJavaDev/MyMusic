using SQLite;

namespace MyMusic.Player.Storage.Models
{
	public sealed class Song
	{
		[AutoIncrement]
		[PrimaryKey]
		public int Serial { get; set; }
		
		public int ArtistsSerial { get; set; }

		public string Title { get; set; }

		public int Duration { get; set; }

		public string Description { get; set; }
	}
}
