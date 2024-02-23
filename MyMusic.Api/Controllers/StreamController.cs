using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMusic.Api.Services;
using MyMusic.Common.Models;
using System.Net;

namespace MyMusic.Api.Controllers
{
    [AllowAnonymous]
    public sealed class StreamController(MyMusicCollectionService collectionService) : BaseApiController
    {
        [HttpGet("apg7/{id}")]
        public async Task<IActionResult> Stream(string id)
        {
            var trackId = Guid.Parse(id);

            var track = await collectionService.GetByTrackId(trackId);

            return File(track.BinaryData.AsByteArray, "audio/wav");
        }
    }
}
