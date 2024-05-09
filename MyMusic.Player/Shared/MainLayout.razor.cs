using Microsoft.AspNetCore.Components;

namespace MyMusic.Player.Shared
{
  public partial class MainLayout : LayoutComponentBase
  {
    // Load playlists id's
    public Dictionary<string, int> TemplPlaylist = new(); 

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
