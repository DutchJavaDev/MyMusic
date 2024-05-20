using Microsoft.AspNetCore.Components;
using MyMusic.Player.Services;
using MyMusic.Player.Services.Youtube;
using MyMusic.Player.Services.Youtube.Models;
using Radzen;

namespace MyMusic.Player.Pages
{
	public partial class Search : ComponentBase, IDisposable
  {
		private bool disposedValue;

		private CancellationTokenSource cancellationTokenSource;

		[Inject]
		private NotificationService NotificationService { get; set; }

		[Inject]
    private YoutubeSearchService YoutubeSearchService { get; set; }

    [Parameter]
    public string SearchQuery { get; set; } = string.Empty;

    private YouTubeSearchModel Model { get; set; } 

    protected override async Task OnInitializedAsync()
    {
			// Callback to update page from other components
			PageNotificationService.AddActionCallBack(GetType(), async (data) => { await RemoteSearchAsync(data); });

			cancellationTokenSource = new CancellationTokenSource();

			await SearchAsync();
    }

		// This will be called from other components if this is the main page
		private async Task RemoteSearchAsync(object data )
		{
			SearchQuery = data.ToString();

			// Stop any search that was going on so this will be the main search 
			// This still a bit buggy
			//cancellationTokenSource.Cancel();
			//cancellationTokenSource.Dispose();

			//cancellationTokenSource = new();

			Model = null;
			StateHasChanged();

			await SearchAsync();
			StateHasChanged();
		}

		private async Task SearchAsync() 
		{
			if (!string.IsNullOrEmpty(SearchQuery))
			{
				Model = await YoutubeSearchService.SearchAsync(SearchQuery, cancellationTokenSource.Token);
			}
		}

		private async Task SendDownloadRequest(Item item)
		{
			// try and grab artist
			var artistInfoResult = await YoutubeSearchService.TryFindArtist(item.id.videoId);

			var erroNotification = new NotificationMessage
			{
				Severity = NotificationSeverity.Error,
				Duration = 5000, // 5 seconds
				Summary = "Error finding artist \r\n",
				Detail = $"Could not find artist for {item.snippet.title}, using channel instead."
			};

			if (artistInfoResult == null)
			{
				Notify(erroNotification);
				return;
			}

			var itemCollection = artistInfoResult.items.FirstOrDefault();
			
			if (itemCollection == null)
			{
				Notify(erroNotification);
				return;
			}

			var artistInfo = itemCollection.musics.FirstOrDefault();

			if (artistInfo == null)
			{
				Notify(erroNotification);
				return;
			}

			if(string.IsNullOrEmpty(artistInfo.artist)) 
			{
				Notify(erroNotification);
				return;
			}

			var foundNotification = new NotificationMessage { 
				Severity = NotificationSeverity.Success,
				Duration = 5000, // 5 seconds
				Summary = "Found artist \r\n",
				Detail = $"Found artist for song {item.snippet.title} || {artistInfo.artist}"
			};

			Notify(foundNotification);

			// Rest to db etc...
		}

		private void Notify(NotificationMessage notificationMessage)
		{
			NotificationService.Notify(notificationMessage);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					PageNotificationService.RemoveActionCallBack(GetType());
				}

				disposedValue = true;
			}
		}

		// TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		~Search()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
