using Microsoft.AspNetCore.Components;
using MyMusic.Player.Services;
using MyMusic.Player.Services.Youtube;
using MyMusic.Player.Services.Youtube.Models;

namespace MyMusic.Player.Pages
{
	public partial class Search : ComponentBase, IDisposable
  {
		private bool disposedValue;

		private CancellationTokenSource cancellationTokenSource;

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
			
			Model = null;
			StateHasChanged();

			// Stop any search that was going on so this will be the main search 
			// This still a bit buggy
			cancellationTokenSource.Cancel();
			cancellationTokenSource.Dispose();

			cancellationTokenSource = new();

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
