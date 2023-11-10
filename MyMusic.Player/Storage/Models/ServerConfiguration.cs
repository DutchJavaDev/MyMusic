using SQLite;

namespace MyMusic.Player.Storage.Models
{
    public sealed class ServerConfiguration
    {
        [PrimaryKey]
        public int? Id { get; set; }
        public string ServerUrl { get; set; }
        public string ServerPassword { get; set; }
        public string ApiKey { get; set; }
    }
}
