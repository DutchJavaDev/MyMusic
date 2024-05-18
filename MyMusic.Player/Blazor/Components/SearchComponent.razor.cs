using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MyMusic.Player.Pages;
using MyMusic.Player.Services;

namespace MyMusic.Player.Blazor.Components
{
  public partial class SearchComponent : ComponentBase
  {
    [Inject]
    private NavigationManager NavigationManager { get; set; }
    public string Query { get; set; } = string.Empty;

		public void CheckKeyPressed(KeyboardEventArgs keyboardEventArgs) 
		{
			if (keyboardEventArgs.Code == "Enter")
			{
				NavigateToSearchPage();
			}
		}

    public void NavigateToSearchPage()
    {
      if (!string.IsNullOrEmpty(Query) && !NavigationManager.Uri.Contains("search"))
      {
        NavigationManager.NavigateTo($"/search/{Query}");
      }
			else
			{
				// Need to notify search page when we are already on the search page
				PageNotificationService.InvokeActionCallBackFor(typeof(Search), Query);
			}
    }
  }
}
