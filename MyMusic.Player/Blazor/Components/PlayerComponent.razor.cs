using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MyMusic.Player.Blazor.Components
{
  public partial class PlayerComponent : ComponentBase, IDisposable
  {
    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    protected DotNetObjectReference<PlayerComponent> _componentReference;

    public float DurationInSeconds { get; set; }
    public string DurationString { get; set; }
    public float CurrentTimeInSeconds { get; set; }
    public string CurrentTimeString { get; set; }

    [JSInvokable]
    public void SetDurations(float seconds)
    {
      DurationInSeconds = seconds;
      TimeSpan timeSpan = TimeSpan.FromSeconds(DurationInSeconds);
      DurationString = $"{(int)timeSpan.TotalMinutes}:{timeSpan.Seconds:D2}";
      StateHasChanged();
    }

    [JSInvokable]
    public void SetCurrentTime(float time)
    {
      CurrentTimeInSeconds = time;
      TimeSpan timeSpan = TimeSpan.FromSeconds(CurrentTimeInSeconds);
      CurrentTimeString = $"{(int)timeSpan.TotalMinutes}:{timeSpan.Seconds:D2}";
      StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
      await base.OnAfterRenderAsync(firstRender);

      if (firstRender)
      {
        // warning about memory
        _componentReference = DotNetObjectReference.Create(this);
        await JSRuntime.InvokeVoidAsync("window.setPlayerComponentReference", _componentReference);
      }
    }

    // This method will be called when the component is disposed
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        _componentReference.Dispose();
      }
    }
    public void Dispose()
    {
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }
  }
}
