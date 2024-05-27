using SQLite;

namespace MyMusic.Player.Storage.Models
{
	public sealed class Log
	{
		[PrimaryKey]
		public int Serial { get; set; }
		public byte Type { get; set; }
		public string Message {  get; set; }
		public string StackTrace { get; set; }	
		// Convert
		public string DateTime { get; set; }
	}
}
