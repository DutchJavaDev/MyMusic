using Dapper;
using System.Data;

namespace MyMusic.Api.Services
{
  public sealed class MusicService
  {
    /// <summary>
    /// Purepose is to have a placeholder inplace until conversion is done and music is stored in storage
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<int> TryInsertMusicPreDownloadAsync(string name, IDbConnection connection)
    {
      const string query = @"INSERT INTO music(name, release_date)
	                    VALUES (@name, @date) returning serial;";

      var param = new
      {
        name,
        date = DateTime.Now
      };

      var result = await connection.ExecuteScalarAsync<string?>(query, param);

      return int.Parse(result?.ToString() ?? string.Empty);
    }

  }
}
