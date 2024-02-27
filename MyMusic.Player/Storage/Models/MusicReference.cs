using SQLite;

namespace MyMusic.Player.Storage.Models
{
  [Table("music_reference")]
  public sealed class MusicReference
  {
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; }
    public string CoverUrl { get; set; }

    // GUID --- lol
    public string TrackingId { get; set; }

    [Ignore]
    public TimeSpan Durration { get; set; }

    [Ignore]
    public string VideoId { get; set; }
  }
}