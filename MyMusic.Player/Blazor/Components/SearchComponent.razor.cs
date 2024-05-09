using Microsoft.AspNetCore.Components;

namespace MyMusic.Player.Blazor.Components
{
  public partial class SearchComponent : ComponentBase
  {
    [Inject]
    private NavigationManager NavigationManager { get; set; }
    public string Query { get; set; } = string.Empty;

    private bool _ready = false;

    protected override void OnParametersSet()
    {
      _ready = true;
    }

    public void NavigateToSearchPage()
    {
      if (_ready && !string.IsNullOrEmpty(Query))
      {
        NavigationManager.NavigateTo($"/search/{Query}");

        Query = string.Empty; // ?
      }
    }
  }
}
