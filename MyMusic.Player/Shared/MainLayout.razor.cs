using Microsoft.AspNetCore.Components;

namespace MyMusic.Player.Shared
{
  public partial class MainLayout : LayoutComponentBase
  {
    // Load playlists id's
    private readonly Dictionary<string, int> TemplPlaylist = [];

    public bool SidebarExpanded { get; set; } = true;

    protected override void OnInitialized()
    {
      for (int i = 0; i < 5; i++)
      {
        TemplPlaylist.Add($"Playlist_{i}",i);
      }
    }
  }
}
