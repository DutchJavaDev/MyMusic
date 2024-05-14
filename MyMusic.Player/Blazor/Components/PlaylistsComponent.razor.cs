using Microsoft.AspNetCore.Components;

namespace MyMusic.Player.Blazor.Components
{
  public partial class PlaylistsComponent : ComponentBase
  {
    private readonly Dictionary<string, int> Playlists = [];

    protected override void OnInitialized()
    {
      for (int i = 0; i < 10; i++)
      {
        Playlists.Add($"Playlist_{i}", i);
      }
    }
  }
}
