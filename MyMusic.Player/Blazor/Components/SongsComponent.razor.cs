using Microsoft.AspNetCore.Components;
using MyMusic.Player.Storage.Models;

namespace MyMusic.Player.Blazor.Components
{
  public partial class SongsComponent : ComponentBase
  {
    private IEnumerable<Song> songDemos;

    protected override void OnInitialized()
    {
      songDemos = Enumerable.Range(0,49).Select(i => new Song
      {
        Serial = i,
        Description = $"Description_{i}",
        Duration = i*60,
        Title = $"Song_{i}"
      });
    }
  }
}
