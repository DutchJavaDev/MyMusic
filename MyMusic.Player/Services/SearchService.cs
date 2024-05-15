using MyMusic.Player.Blazor;
using MyMusic.Player.Blazor.Models.Search;
using MyMusic.Player.Storage;
using Newtonsoft.Json;
using System.Text;

namespace MyMusic.Player.Services
{
  public sealed class SearchService
  {
    private readonly string SearchV3Url = "https://www.googleapis.com/youtube/v3/search?part=snippet&order=viewCount&maxResults=30&type=video&";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly LocalDatabase _database;
    private readonly VideoDurationService _videoDurationService;
    private readonly LogService _logService;

    public SearchService(IHttpClientFactory httpClientFactory,
        LocalDatabase database,
        VideoDurationService videoDurationService,
        LogService log)
    {
      _httpClientFactory = httpClientFactory;
      _database = database;
      _videoDurationService = videoDurationService;
      _logService = log;
    }

  }
}