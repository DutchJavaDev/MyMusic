using Microsoft.AspNetCore.Components;
using MyMusic.Common.Models;
using MyMusic.Player.Services;
using MyMusic.Player.Services.Read;
using MyMusic.Player.Services.Write;
using System.Threading;
using static MyMusic.Common.CommonData;

namespace MyMusic.Player.Pages
{
  public partial class Cloud : ComponentBase, IDisposable
  {
    private readonly int _updateInterval = 5000; // 5 seconds
		private Timer _timer;

		private IEnumerable<StatusModel> _statusModels = [];

		[Inject]
		public ApiService ApiService { get; set; }
		[Inject]
		public SongStatusReaderService SongStatusReaderService { get; set; }
		[Inject]
		public SongStatusWriterService SongStatusWriterService { get; set; }

    protected override async Task OnInitializedAsync()
    {
			// Run every 5 seconds
			_timer = new Timer((_) => UpdateUI(),null,0,_updateInterval);
    }

		private async void UpdateUI()
		{
			var ids = await SongStatusReaderService.GetTrackingIdsAsync();

			if (ids.Count == 0) return;

			var requets = new StatusRequest 
			{
				TrackingIds = ids
			};

			_statusModels = await ApiService.GetStatusModelsAsync(requets);

			await InvokeAsync(StateHasChanged);
			
			// TODO move this to main page since it will be updating it while the app is on
			// Now only when you are on this page
			var doneIds = _statusModels.Where(i => i.State == (int)Mp3State.Done)
				.Select(i => i.TrackingId).ToList();

			await SongStatusWriterService.UpdateStatusToDone(doneIds);

		}

    public void Dispose()
    {
      _timer.Dispose();
    }
  }
}