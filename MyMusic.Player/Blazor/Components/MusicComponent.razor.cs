using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyMusic.Player.Services;
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
    public ApiService ApiService { get; set; }

    [Inject]
    public ConfigurationService ConfigurationService { get; set; }

    [Inject]
    public MusicReferenceService MusicReferenceService { get; set; }

    [Inject]
    public IJSRuntime jSRuntime { get; set; }

    async Task PlayAsync()
    {
      var url = $"{BaseUrl}stream/apg7/{Model.TrackingId}";

      await jSRuntime.InvokeVoidAsync("window.play", url);
    }
  }
}