using Microsoft.AspNetCore.Components;
using MyMusic.Common.Models;
using MyMusic.Player.Services;

namespace MyMusic.Player.Pages
{
  public partial class Cloud : ComponentBase, IDisposable
  {
    [Inject]
    private UpdaterService UpdaterService { get; set; }

    private readonly Guid _updaterId = new("4c0cfd19-d204-40d2-8ad1-39f576439790");
    private readonly double _updateInterval = 2000; // 2 seconds

    protected override async Task OnInitializedAsync()
    {
      UpdaterService.AddUpdateCallBack(_updaterId, _updateInterval, CallBack);
    }

    public async Task CallBack()
    {
      //await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
      UpdaterService.RemoveUpdateCallBack(_updaterId);
    }
  }
}