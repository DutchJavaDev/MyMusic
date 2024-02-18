using MongoDB.Driver;
using MyMusic.Common.Models;

namespace MyMusic.Api.Services
{
  public sealed class MyMusicCollectionService
  {
    public static string DatabaseName = "music";
    public static string CollectionName = "upload";

    private readonly MongoClient _client;

    public MyMusicCollectionService(MongoClient mongoClient) 
    {
      _client = mongoClient;
    }

    public async Task UploadAsync(MyMusicObject myMusicObject)
    {
      var collection = GetCollection();

      await collection.InsertOneAsync(myMusicObject);
    }

    public async Task<MyMusicObject?> GetByTrackId(Guid trackId)
    {
      var collection = GetCollection();

      var trackFilter = Builders<MyMusicObject>.Filter.Eq(nameof(MyMusicObject.TrackId), trackId);

      return await (await collection.FindAsync(trackFilter)).FirstOrDefaultAsync();
    }

    private IMongoCollection<MyMusicObject> GetCollection()
    {
      var database = _client.GetDatabase(DatabaseName);
      return database.GetCollection<MyMusicObject>(CollectionName);
    }
  }
}
