using SQLite;

namespace MyMusic.Player.Blazor.Models.Logging
{
    [Table("logs")]
    public sealed class LogEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime Created { get; set; }

        public static LogEntry FromException(Exception exception)
        {
            return new LogEntry 
            {
                Message = exception.Message,
                StackTrace = exception.StackTrace.Trim(),
                Created = DateTime.Now
            };
        }
    }
}
