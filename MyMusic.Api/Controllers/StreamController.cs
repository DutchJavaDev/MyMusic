﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMusic.Api.Services;

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

      if(track == null)
      {
        return NotFound();
      }

      return CreateFileEnabledRangeProcessing(track.BinaryData.AsByteArray);
    }

    private FileContentResult CreateFileEnabledRangeProcessing(byte[] data)
    {
      //file.EnableRangeProcessing = true; // Allow for ajusting the playback time: https://stackoverflow.com/questions/48711209/partial-content-in-net-core-mvc-for-video-audio-streaming
      return File(data, "audio/mpeg", true);
    }
  }
}