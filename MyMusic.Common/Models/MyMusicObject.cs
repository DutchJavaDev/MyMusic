using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyMusic.Common.Models
{
  public sealed class MyMusicObject
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public Guid TrackId { get; set; }
    public string? Name { get; set; }
    public BsonBinaryData? BinaryData { get; set; }
    public DateTime? Uploaded { get; set; }
  }
}