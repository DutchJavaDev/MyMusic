using Microsoft.AspNetCore.Components;
using MyMusic.Common.Models;
using MyMusic.Player.Services;
using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;

namespace MyMusic.Player.Blazor.Components
{
  public partial class DownloadComponent : ComponentBase
  {
    [Parameter]
    public MusicReference Model { get; set; }
    [Inject]
    private LocalDatabase LocalDb { get; set; } 
    [Inject]
    private ApiService ApiService { get; set; }
    public async Task SendDownLoadAsync()
    {
      var request = new DownloadRequest
      {
        Name = Model.Name,
        VideoId = Model.VideoId,
        
      };

      var trackingId = await ApiService.DownloadAsync(request);

      await LocalDb.InsertAsync(new MusicReference
      {
        TrackingId = trackingId,
        CoverUrl = Model.CoverUrl,
        Name = Model.Name,
      });
    }
  }
}