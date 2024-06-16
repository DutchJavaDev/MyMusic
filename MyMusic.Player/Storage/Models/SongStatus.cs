using SQLite;

namespace MyMusic.Player.Storage.Models
{
	public sealed class SongStatus
	{
		[PrimaryKey]
		public int Serial {  get; set; }
		public int SongSerial { get; set; }
		public int Status { get; set; }	
		// Guid
		public string TrackingId { get; set; }
	}
}
