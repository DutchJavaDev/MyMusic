using Microsoft.AspNetCore.Components;
using MyMusic.Player.Blazor.Components;
using MyMusic.Player.Services.Read;
using MyMusic.Player.Storage.Models;
using Radzen;

namespace MyMusic.Player.Pages
{
	public partial class Artists : ComponentBase
	{

		[Inject]
		public ArtistReaderService artistReaderService { get; set; }
		[Inject]
		public DialogService DialogService { get; set; }

		private IEnumerable<ArtistsSongsModel> Models { get; set; }

		protected override async Task OnInitializedAsync()
		{
			Models = await artistReaderService.GetArtistsAsync();
		}
		public async Task OpenEditDialog(ArtistsSongsModel _model)
		{
			var model = new MyMusic.Player.Storage.Models.Artists
			{
				Serial = _model.Serial,
				Name = _model.Name,
				ImageUrl = _model.ImageUrl,
			};

			await DialogService.OpenAsync<EditArtistsDialog>("Edit artists",
				new Dictionary<string, object> { { "Model",model} },
				new DialogOptions() { Width = "700px", Height = "512px", Resizable = true, Draggable = false });

			Models = await artistReaderService.GetArtistsAsync();
			StateHasChanged();
		}
		public async Task OpenCreateDialog()
		{
			await DialogService.OpenAsync<CreateArtistsDialog>("Create new artists", [],
				new DialogOptions() { Width = "700px", Height = "512px", Resizable = true, Draggable = false });

			Models = await artistReaderService.GetArtistsAsync();
			StateHasChanged();
		}
	}
}
