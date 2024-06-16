using SQLite;

namespace MyMusic.Player.Storage.Models
{
	public sealed class Artists
	{
		[PrimaryKey]
		public int Serial { get; set; }
		[Unique]
		public string Name { get; set; }
		public string ImageUrl { get; set; }
	}
}
