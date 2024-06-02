using SQLite;

namespace MyMusic.Player.Storage.Models
{
	public sealed class SongStatus
	{
		[AutoIncrement]
		[PrimaryKey]
		public int Serial {  get; set; }
		public int SongSerial { get; set; }
		public int Status { get; set; }	
	}
}
