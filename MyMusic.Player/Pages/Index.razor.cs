using Microsoft.AspNetCore.Components;
using MyMusic.Common.Models;
using MyMusic.Player.Blazor.Models.Logging;
using MyMusic.Player.Blazor.Models.Search;
using MyMusic.Player.Services;
using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;

namespace MyMusic.Player.Pages
{
  public partial class Index : ComponentBase
  {
    [Inject]
    public LocalDatabase LocalDb { get; set; }

    [Inject]
    public SearchService SearchService { get; set; }

    [Inject]
    public ApiService ApiService { get; set; }

    [Inject]
    public LogService LogService { get; set; }

    public List<SearchViewModel> Models { get; set; }

    public bool _searching = false;

    public async Task Search(string query)
    {
      _searching = true;

      // Scroll back to the top
      // Im lazy
      Models = Enumerable.Empty<SearchViewModel>().ToList();

      Models = await SearchService.SearchAsync(query);
      _searching = false;
    }

    public async Task SendDownLoadAsync(SearchViewModel model)
    {
      var trackingId = await ApiService.DownloadAsync(new DownloadRequest
      {
        VideoId = model.VideoId,
        Name = model.Title,
        Release = model.Published
      });

      // Valid Guid
      if (!Guid.TryParse(trackingId, out var _))
      {
        await LogService.WriteLogAsync(new LogEntry
        {
          Message = $"Failed to parse GUID for {model.VideoId} | {model.Title}"
        });
        return;
      }

      // Keep track in local db
      await LocalDb.InsertAsync(new MusicReference
      {
        TrackingId = trackingId,
        CoverUrl = model.CoverUrl,
        Name = model.Title
      });
    }
  }
}