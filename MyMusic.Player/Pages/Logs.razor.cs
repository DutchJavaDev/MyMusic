using Microsoft.AspNetCore.Components;
using MyMusic.Player.Blazor.Components;
using MyMusic.Player.Services;
using MyMusic.Player.Services.Read;
using MyMusic.Player.Storage.Models;
using Radzen;

namespace MyMusic.Player.Pages
{
	public partial class Logs : ComponentBase
	{
		[Inject]
		private LogReaderService LogReaderService { get; set; }

		[Inject]
		private DialogService DialogService { get; set; }

		private IEnumerable<Log> SelectedLogs;
		private IEnumerable<string> _logDropdowns;
		private string _logDropDownValue;

		protected override async Task OnInitializedAsync()
		{
			_logDropdowns = ["All", "Info", "Error"];
			_logDropDownValue = _logDropdowns.FirstOrDefault();

			SelectedLogs = await LogReaderService.GetLogsAsync();
		}

		public async void FetchLogsAsync(string type)
		{
			if (type.Equals("All"))
			{
				SelectedLogs = await LogReaderService.GetLogsAsync();
				StateHasChanged();
				return;
			}

			if (type.Equals("Info"))
			{
				SelectedLogs = await LogReaderService.GetLogsAsync(Util.INFO);
				StateHasChanged();
				return;
			}

			if (type.Equals("Error"))
			{
				SelectedLogs = await LogReaderService.GetLogsAsync(Util.ERROR);
				StateHasChanged();
				return;
			}
		}

		public async Task OpenLogAsync(Log log)
		{
			await DialogService.OpenAsync<LogDialog>("Log Popup",
				new Dictionary<string, object> { {"Log", log } },
				new DialogOptions() { Width = "700px", Height = "512px", Resizable = true, Draggable = false });
		}
	}
}
