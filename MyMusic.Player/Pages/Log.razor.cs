using Microsoft.AspNetCore.Components;
using MyMusic.Player.Blazor.Models.Logging;
using MyMusic.Player.Services;

namespace MyMusic.Player.Pages
{
  public partial class Log : ComponentBase, IDisposable
  {
    [Inject]
    public LogService LogService { get; set; }

    [Inject]
    public UpdaterService UpdaterService { get; set; }

    private readonly Guid _updaterId = new("4a79b5ec-0b42-4c47-994e-282cf63c8adf");

    public List<LogEntry> Logs { get; set; }

    protected override async Task OnInitializedAsync()
    {
      Logs = await LogService.GetLogsAsync();

      UpdaterService.AddUpdateCallBack(_updaterId, 500, FetchLogs);
    }

    private async Task FetchLogs()
    {
      Logs = await LogService.GetLogsAsync();
      await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
      UpdaterService.RemoveUpdateCallBack(_updaterId);
    }
  }
}