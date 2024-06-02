using SQLite;

namespace MyMusic.Player.Storage.Models
{
	public sealed class Artists
	{
		[AutoIncrement]
		[PrimaryKey]
		public int Serial { get; set; }
		public string Name { get; set; }
	}
}
