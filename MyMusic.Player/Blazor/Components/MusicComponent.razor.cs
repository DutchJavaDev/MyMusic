using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyMusic.Player.Storage.Models;


namespace MyMusic.Player.Blazor.Components
{
  public partial class MusicComponent : ComponentBase
  {
    [Parameter]
    public MusicReference Model { get; set; }
    
    [Parameter]
    public string BaseUrl { get; set; } = string.Empty;

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    async Task PlayAsync()
    {
      var url = $"{BaseUrl}stream/apg7/{Model.TrackingId}";

      await JSRuntime.InvokeVoidAsync("window.setCoverUrl",Model.CoverUrl);
      await JSRuntime.InvokeVoidAsync("window.play", url);
    }
  }
}