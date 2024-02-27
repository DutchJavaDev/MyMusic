using Dapper;
using MyMusic.Common.Models;
using System.Data;
using static MyMusic.Common.CommonData;

namespace MyMusic.Api.Services
{
  public sealed class MusicService(IDbConnection _connection, DbLogger _dbLogger)
  {
    /// <summary>
    /// Purepose is to have a placeholder inplace until conversion is done and music is stored in storage
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<(int serial, Guid trakingId)> TryInsertMusicPreDownloadAsync(string name, IDbConnection connection)
    {
      const string query = @"INSERT INTO music(name, release_date)
	                    VALUES (@name, @date) returning (serial, tracking_id);";

      var param = new
      {
        name,
        date = DateTime.Now
      };

      var inserts = await connection.ExecuteScalarAsync<object[]>(query, param);

      var serial = (int)inserts[0];

      var guid = (Guid)inserts[1];

      return (serial, guid);
    }

    public async Task<object> GetDownloadedMusic()
    {
      try
      {
        var query = @"select m.name Name, m.tracking_id TrackingId from
                              music as m
                              left join download as d ON d.music_serial = m.serial
                              where d.state = @state".Trim();

        var param = new { state = (int)Mp3State.Uploaded };

        return await _connection.QueryAsync<MusicDto>(query, param);
      }
      catch (Exception e)
      {
        _dbLogger.LogAsync(e);
        return null;
      }
    }
  }
}