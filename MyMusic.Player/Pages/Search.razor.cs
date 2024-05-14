using Microsoft.AspNetCore.Components;
using MyMusic.Player.Services;

namespace MyMusic.Player.Pages
{
  public partial class Search : ComponentBase
  {
    [Inject]
    public SearchService SearchService { get; set; }

    [Parameter]
    public string SearchQuery { get; set; } = string.Empty;

    public bool HasSearchQuery { get; set; }

    public string SearchResponse { get; set; } = string.Empty;

    protected override void OnParametersSet()
    {
      HasSearchQuery = !string.IsNullOrEmpty(SearchQuery);
    }

    protected override async Task OnInitializedAsync()
    {
      if (HasSearchQuery || !string.IsNullOrEmpty(SearchQuery))
      {
        SearchResponse = await SearchService.SearchRawResults(SearchQuery);
        StateHasChanged();
      }
    }
  }
}
