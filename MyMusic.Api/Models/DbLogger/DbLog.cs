using System.ComponentModel.DataAnnotations.Schema;

namespace MyMusic.Api.Models.DbLogger
{
    public sealed class DbLog
    {
        [Column("serial")]
        public int? Serial { get; set; }

        [Column("message")] 
        public string? Message { get; set; }

        [Column("stacktrace")]
        public string? StackTrace { get; set; }  
    }
}
