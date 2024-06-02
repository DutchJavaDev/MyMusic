using Microsoft.AspNetCore.Components;
using MyMusic.Player.Storage.Models;
using Radzen;

namespace MyMusic.Player.Blazor.Components
{
	public partial class LogDialog : ComponentBase
	{

		[Inject]
		private DialogService DialogService {  get; set; }

		[Parameter]
	  public Log Log { get; set; }
	}
}
