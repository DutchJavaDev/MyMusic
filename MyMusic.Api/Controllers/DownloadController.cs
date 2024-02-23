using Microsoft.AspNetCore.Mvc;
using MyMusic.Api.Services;
using MyMusic.Common.Models;

namespace MyMusic.Api.Controllers
{
  public class DownloadController : BaseApiController
  {
    [HttpPost("start")]
    public async Task<IActionResult> Start(
        [FromBody] DownloadRequest downloadRequest,
        [FromServices] DownloadService downloadService
        )
    {
      var trackingId = await downloadService.CreateDownloadRequestAsync(downloadRequest);

      if (trackingId != Guid.Empty)
      {
        return Ok(trackingId.ToString());
      }

      return BadRequest();
    }

    [HttpGet("status")]
    public async Task<IActionResult> Status(
        [FromServices] StatusService statusService)
    {
      return Ok(await statusService.GetStatusModelsAsync());
    }
  }
}
