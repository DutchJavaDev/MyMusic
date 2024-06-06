using SQLite;

namespace MyMusic.Player.Storage.Models
{
	public sealed class Log
	{
		[PrimaryKey]
		public int Serial { get; set; }
		public byte LogType { get; set; }
		public string Message {  get; set; }
		public string StackTrace { get; set; }	
		public DateTime DateTime { get; set; }
	}
}
