using Microsoft.AspNetCore.Mvc;
using MyMusic.Api.Services;

namespace MyMusic.Api.Controllers
{
  public sealed class MusicController : BaseApiController
  {
    [HttpGet]
    public async Task<IActionResult> Index([FromServices] MusicService musicService)
    {
      return Ok(await musicService.GetDownloadedMusic());
    }
  }
}
