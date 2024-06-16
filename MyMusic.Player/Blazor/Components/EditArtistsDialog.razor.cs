using Microsoft.AspNetCore.Components;
using MyMusic.Player.Services.Read;
using MyMusic.Player.Services.Write;
using MyMusic.Player.Storage.Models;
using Radzen;

namespace MyMusic.Player.Blazor.Components
{
	public partial class EditArtistsDialog : ComponentBase
	{
		[Inject]
		public ArtistWriterService ArtistWriterService { get; set; }
		[Inject]
		public DialogService DialogService { get; set; }	
		[Parameter]
		public Artists Model { get; set; }

		public async void Submit()
		{ 
			await ArtistWriterService.UpdateArtistAsync(Model);
			DialogService.Close();	
		}
	}
}
