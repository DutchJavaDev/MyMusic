using Microsoft.AspNetCore.Components;
using MyMusic.Player.Services;
using Radzen;

namespace MyMusic.Player.Shared
{
  public partial class MainLayout : LayoutComponentBase
  {
    // Load playlists id's
    private readonly Dictionary<string, int> TemplPlaylist = [];

    public bool SidebarExpanded { get; set; } = true;

		[Inject]
		private NotificationService NotificationService { get; set; }
		[Inject]
		private TooltipService TooltipService { get; set; }

    protected override void OnInitialized()
    {
			AppNotification.SetNotificationService(NotificationService);
			AppTooltip.SetTooltipService(TooltipService);

      for (int i = 0; i < 5; i++)
      {
        TemplPlaylist.Add($"Playlist_{i}",i);
      }
    }
  }
}
