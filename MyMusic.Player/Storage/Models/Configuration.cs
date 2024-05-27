using SQLite;

namespace MyMusic.Player.Storage.Models
{
  public sealed class Configuration
  {
    [PrimaryKey]
    public int Id { get; set; }

    public string CloudUrl { get; set; }
    public string CloudPassword { get; set; }
  }
}