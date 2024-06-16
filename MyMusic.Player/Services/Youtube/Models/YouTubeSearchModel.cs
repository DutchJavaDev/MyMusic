namespace MyMusic.Player.Services.Youtube.Models
{
  // Base model
	public sealed class YouTubeSearchModel
	{
    public string kind { get; set; }
    public string etag { get; set; }
    public string nextPageToken { get; set; }
    public List<Item> items { get; set; }
  }

  public class ChannelThumbnail
  {
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
  }

  public class DetailedMetadataSnippet
  {
    public string text { get; set; }
    public bool? bold { get; set; }
  }

  public class Id
  {
    public string kind { get; set; }
    public string videoId { get; set; }
  }

  public class Item
  {
    public string kind { get; set; }
    public string etag { get; set; }
    public Id id { get; set; }
    public Snippet snippet { get; set; }
  }
  public class Snippet
  {
    public string channelId { get; set; }
    public string title { get; set; }
    public List<Thumbnail> thumbnails { get; set; }
    public string channelTitle { get; set; }
    public string channelHandle { get; set; }
    public string timestamp { get; set; }
    public int duration { get; set; }
    public int views { get; set; }
    public List<string> badges { get; set; }
    public string channelApproval { get; set; }
    public List<ChannelThumbnail> channelThumbnails { get; set; }
    public List<DetailedMetadataSnippet> detailedMetadataSnippet { get; set; }
    public List<object> chapters { get; set; }
  }

  public class Thumbnail
  {
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
  }

}
