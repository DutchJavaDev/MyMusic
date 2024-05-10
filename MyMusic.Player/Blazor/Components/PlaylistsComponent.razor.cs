using Microsoft.AspNetCore.Components;

namespace MyMusic.Player.Blazor.Components
{
  public partial class PlaylistsComponent : ComponentBase
  {
    private readonly List<string> Playlists = [];

    protected override void OnInitialized()
    {
      for (int i = 0; i < 30; i++)
      {
        Playlists.Add($"Playlist_{i}");
      }
    }
  }
}
