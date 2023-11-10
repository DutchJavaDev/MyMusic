using Microsoft.AspNetCore.Mvc;
using MyMusic.Api.Services;
using MyMusic.Common.Models;

namespace MyMusic.Api.Controllers
{
    public class DownloadController : BaseApiController
    {
        [HttpPost("start")]
        public async Task<IActionResult> Start(
            [FromBody] DownloadRequest request,
            [FromServices] DownloadService pipeline
            )
        {
            var id = await pipeline.CreateDownloadRequestAsync(request);

            if (id != -1)
            {
                return Ok(id);
            }

            return BadRequest();
        }
    }
}
