﻿using Dapper;
using MyMusic.Common.Models;
using System.Data;
using static MyMusic.Common.CommonData;

namespace MyMusic.Api.Services
{
  public sealed class DownloadService
  {
    private readonly DbLogger _logger;
    private readonly IDbConnection _connection;
    private readonly MusicService _musicService;

    public DownloadService(
        DbLogger logger,
        IDbConnection connection,
        MusicService music)
    {
      _connection = connection;
      _musicService = music;
      _logger = logger;
    }

    public async Task<Guid> CreateDownloadRequestAsync(DownloadRequest request)
    {
      using (_connection)
      {
        _connection.Open();
        using var transaction = _connection.BeginTransaction();
        try
        {
          var (serial, trackingId) = await _musicService
              .TryInsertMusicPreDownloadAsync(request.Name, transaction.Connection ?? _connection);

          await InsertDownloadAsync(request, transaction, serial);

          return trackingId;
        }
        catch (Exception e)
        {
          transaction.Rollback();
          await _logger.LogAsync(e, messagePrefix: "Failed to create download request");
          return Guid.Empty;
        }
      }
    }

    private async Task<int> InsertDownloadAsync(DownloadRequest request, IDbTransaction transaction, int musicId)
    {
      var state = (int)Mp3State.Created;
      var videoId = request.VideoId;

      var query = @"INSERT INTO download(music_serial, state, video_id)
	                    VALUES (@musicId, @state, @videoId);";

      var param = new
      {
        musicId,
        state,
        videoId,
      };

      var id = await _connection.ExecuteScalarAsync<int>(query, param, transaction);
      transaction.Commit();
      return id;
    }
  }
}