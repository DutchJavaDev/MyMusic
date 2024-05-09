using Microsoft.AspNetCore.Components;

namespace MyMusic.Player.Pages
{
  public partial class Search : ComponentBase
  {
    [Parameter]
    public string SearchQuery { get; set; } = string.Empty;

    public bool HasSearchQuery { get; set; }

    protected override void OnParametersSet()
    {
      HasSearchQuery = !string.IsNullOrEmpty(SearchQuery);
    }
  }
}
