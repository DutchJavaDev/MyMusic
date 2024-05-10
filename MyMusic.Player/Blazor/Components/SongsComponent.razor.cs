using Microsoft.AspNetCore.Components;
using MyMusic.Player.Blazor.Models.Temp;

namespace MyMusic.Player.Blazor.Components
{
  public partial class SongsComponent : ComponentBase
  {
    private IEnumerable<SongDemo> songDemos;

    protected override void OnInitialized()
    {
      songDemos = Enumerable.Range(0,49).Select(i => new SongDemo 
      {
        Serial = i,
        Description = $"Description_{i}",
        Durration = TimeSpan.FromMinutes(i),
        Name = $"Song_{i}"
      });
    }
  }
}
