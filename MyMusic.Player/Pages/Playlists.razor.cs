using Microsoft.AspNetCore.Components;

namespace MyMusic.Player.Pages
{
  public partial class Playlists : ComponentBase, IDisposable
  {
    [Parameter]
    public int? PlaylistSerial { get; set; }

    public bool HasPlaylistId { get; set; }

    protected override void OnParametersSet()
    {
      HasPlaylistId = PlaylistSerial.HasValue;
    }

    public void Dispose()
    {

    }
  }
}
