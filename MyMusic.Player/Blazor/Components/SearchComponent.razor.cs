using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using MyMusic.Player.Pages;
using MyMusic.Player.Services;
using Radzen.Blazor;

namespace MyMusic.Player.Blazor.Components
{
  public partial class SearchComponent : ComponentBase
  {
    [Inject]
    private NavigationManager NavigationManager { get; set; }
    public string Query { get; set; } = string.Empty;

		InputText inputTextreference;

		public void CheckKeyPressed(KeyboardEventArgs keyboardEventArgs) 
		{
			if (keyboardEventArgs.Code == "Enter")
			{
				NavigateToSearchPage();
			}
		}

		public void ClearCurrentSearch()
		{
			if (string.IsNullOrEmpty(Query)) return;
			
			Query = string.Empty;	
			PageNotificationService.InvokeNamedCallback(nameof(Search.ClearSearchToken), "");
		}

    public void NavigateToSearchPage()
    {
			if (string.IsNullOrEmpty(Query))
			{
				AppTooltip.ShowTooltip(inputTextreference.Element.Value, "Search cannot be empty");
				return;
			}

			// If the main page is not search then navigate to it, pass in the search query
      if (!NavigationManager.Uri.Contains("search"))
      {
        NavigationManager.NavigateTo($"/search/{Query}");
      }
			else
			{
				// Notify the search page instead of navigating to it
				PageNotificationService.InvokeActionCallBackFor(typeof(Search), Query);
			}
    }
  }
}
