using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyMusic.Player.Storage.Models;

namespace MyMusic.Player.Blazor.Components
{
  public partial class MusicReferenceComponent : ComponentBase
  {
    [Parameter]
    public MusicReference Model { get; set; }

    [Inject]
    public IJSRuntime JsRuntime { get; set; }
  }
}