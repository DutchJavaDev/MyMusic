using Microsoft.AspNetCore.Components;
using MyMusic.Player.Services.Read;
using MyMusic.Player.Storage.Models;

namespace MyMusic.Player.Blazor.Components
{
  public partial class SongsComponent : ComponentBase
  {
    private List<Song> songModels = [];
		[Inject]
		public SongReaderService SongReaderService { get; set; }

    protected override async Task OnInitializedAsync()
    {
      songModels = await SongReaderService.GetSongAsync();
    }
  }
}
