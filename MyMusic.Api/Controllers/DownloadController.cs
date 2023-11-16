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
            var id = await downloadService.CreateDownloadRequestAsync(downloadRequest);

            if (id > -1)
            {
                return Ok(id);
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
