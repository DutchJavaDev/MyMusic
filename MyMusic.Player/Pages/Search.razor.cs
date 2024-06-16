using Microsoft.AspNetCore.Components;
using MyMusic.Player.Services;
using MyMusic.Player.Services.Read;
using MyMusic.Player.Services.Write;
using MyMusic.Player.Services.Youtube;
using MyMusic.Player.Services.Youtube.Models;

namespace MyMusic.Player.Pages
{
	public partial class Search : ComponentBase, IDisposable
  {
		private bool disposedValue;

		public bool _searching = false;

		private CancellationTokenSource cancellationTokenSource;
		[Inject]
		private LogWriterService LogWriterService { get; set; }

		[Inject]
    private YoutubeSearchService YoutubeSearchService { get; set; }
		[Inject]
		private ArtistWriterService ArtistWriterService { get; set; }
		[Inject]
		private ArtistReaderService ArtistReaderService { get; set; }
		[Inject]
		private SongWriterService SongWriterService { get; set; }
		[Inject]
		private SongStatusWriterService SongStatusWriterService { get; set;}

    [Parameter]
    public string SearchQuery { get; set; } = string.Empty;

    private YouTubeSearchModel Model { get; set; } 

		private string NextPageToken = string.Empty;


		protected override async void OnInitialized()
    {
			// Callback to update page from other components
			PageNotificationService.AddActionCallBack(GetType(), async (data) => await RemoteSearchAsync(data));
			PageNotificationService.AddNamedCallBack(nameof(ClearSearchToken), async (data) => await ClearSearchToken(null));
			cancellationTokenSource = new();

			if (string.IsNullOrEmpty(SearchQuery))
			{
				SearchQuery = "latest, music";
				NextPageToken = string.Empty;
				await SearchAsync(false);
			}
			else
			{
				await SearchAsync();
			}

    }

		public async Task ClearSearchToken(object data = null)
		{
			if (string.IsNullOrEmpty(NextPageToken)) return;

			SearchQuery = "latest, music";
			NextPageToken = string.Empty;
			await SearchAsync(false);
		}

		// This will be called from other components if this is the main page
		private async Task RemoteSearchAsync(object data )
		{
			// Stop any search that was going on so this will be the main search 
			if (cancellationTokenSource is not null)
			{
				cancellationTokenSource.Cancel();
				cancellationTokenSource.Dispose();
				cancellationTokenSource = null;
			}

			SearchQuery = data.ToString();
			Model = null;
			StateHasChanged();
			
			await SearchAsync();
		}

		private async Task SearchAsync(bool saveToken = true) 
		{
			cancellationTokenSource ??= new();

			if (!string.IsNullOrEmpty(SearchQuery))
			{
				_searching = true;
				StateHasChanged();

				Model = await YoutubeSearchService.SearchAsync(SearchQuery, cancellationTokenSource.Token, NextPageToken);

				if(saveToken)
				{
					NextPageToken = Model.nextPageToken;
				}
				
				_searching = false;
				StateHasChanged();
			}
		}

		private async Task SendDownloadRequest(Item item)
		{
			// try and grab artist
			var artistInfoResult = await YoutubeSearchService.TryFindArtist(item.id.videoId, cancellationTokenSource.Token);

			if (artistInfoResult == null)
			{
				await LogWriterService.Error($"{nameof(artistInfoResult)} is null", $"Could not find artist for {item.snippet.title}, using channel instead.");
				return;
			}

			var itemCollection = artistInfoResult.items.FirstOrDefault();
			
			if (itemCollection == null)
			{
				await LogWriterService.Error($"{nameof(itemCollection)} is null", $"Could not find artist for {item.snippet.title}, using channel instead.");
				return;
			}

			var artistInfo = itemCollection.musics.FirstOrDefault();

			if (artistInfo == null)
			{
				await LogWriterService.Error($"{nameof(artistInfo)} is null", $"Could not find artist for {item.snippet.title}, using channel instead.");
				return;
			}

			if(string.IsNullOrEmpty(artistInfo.artist)) 
			{
				await LogWriterService.Error($"{nameof(artistInfo.artist)} is null or empty", $"Could not find artist for {item.snippet.title}, using channel instead.");
				return;
			}
			AppNotification.Success("Found artist", $"Found artist for song {item.snippet.title} || {artistInfo.artist}");
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					cancellationTokenSource.Cancel();
					cancellationTokenSource.Dispose();
					PageNotificationService.RemoveActionCallBack(GetType());
					PageNotificationService.RemoveNamedCallBack(nameof(ClearSearchToken));
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
