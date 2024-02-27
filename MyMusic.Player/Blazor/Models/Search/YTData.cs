using MyMusic.Player.Storage.Models;

namespace MyMusic.Player.Blazor.Models.Search
{
  public sealed class Id
  {
    public string kind { get; set; }
    public string videoId { get; set; }
  }

  public sealed class Item
  {
    //public string kind { get; set; }
    //public string etag { get; set; }
    public Id id { get; set; }

    public Snippet snippet { get; set; }
  }

  public sealed class PageInfo
  {
    public int totalResults { get; set; }
    public int resultsPerPage { get; set; }
  }

  public sealed class SearchViewModel
  {
    public string Title { get; set; }
    public string VideoId { get; set; }
    public string Description { get; set; }
    public string CoverUrl { get; set; }
    public DateTime Published { get; set; }

    // Fecth
    public TimeSpan Durration { get; set; }

    public MusicReference Dto()
    {
      return new()
      {
        CoverUrl = CoverUrl,
        Id = 0,
        Name = Title,
        TrackingId = Guid.NewGuid().ToString(),
        Durration = Durration,
        VideoId = VideoId,
      };
    }
  }

  public sealed class SearchResult
  {
    //public string kind { get; set; }
    //public string etag { get; set; }
    //public string nextPageToken { get; set; }
    //public string regionCode { get; set; }
    public PageInfo pageInfo { get; set; }

    public List<Item>? items { get; set; }
  }

  public sealed class Snippet
  {
    public DateTime publishedAt { get; set; }

    //public string channelId { get; set; }
    public string title { get; set; }

    public string description { get; set; }
    public Thumbnails thumbnails { get; set; }
    public string channelTitle { get; set; }
    //public string liveBroadcastContent { get; set; }
    //public DateTime publishTime { get; set; }
  }

  public sealed class Thumbnails
  {
    public Default @default { get; set; }
    public Medium medium { get; set; }
    public High high { get; set; }
  }

  public class Default
  {
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
  }

  public class Medium
  {
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
  }

  public class High
  {
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
  }
}