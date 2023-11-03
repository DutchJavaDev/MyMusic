using Microsoft.AspNetCore.Mvc;
using MyMusic.Api.Models.PipelineService;
using MyMusic.Api.Services;

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
            await pipeline.CreateDownloadRequest(request);
            return Ok();
        }
    }
}
