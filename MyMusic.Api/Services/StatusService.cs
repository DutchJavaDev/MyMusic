using Dapper;
using MyMusic.Common.Models;
using System.Data;
using static MyMusic.Common.CommonData;

namespace MyMusic.Api.Services
{
  public sealed class StatusService
  {
    private DbLogger _logger;
    private IDbConnection _connection;

    public StatusService(
        DbLogger logger,
        IDbConnection connection)
    {
      _connection = connection;
      _logger = logger;
    }

		public async Task<IEnumerable<StatusModel>> GetStatusModelsAsync(List<Guid> guids)
		{
			try
			{
				var strIds = guids.Select(i => $"'{i}'").ToList();

				var ids = string.Join(',', strIds);
				
				var query = @$"select m.name as Name, m.tracking_id as TrackingId, d.state as State from
                              music as m
                              left join download as d ON d.music_serial = m.serial
                              left join mp3media as mp ON mp.download_serial = d.serial
                              where m.tracking_id in ({ids}) and d.state <= @state".Trim();

				using (_connection)
				{
					_connection.Open();
					return await _connection.QueryAsync<StatusModel>(query, new { state = (int)Mp3State.Done });
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

    public async Task<IEnumerable<StatusModel>> GetStatusModelsAsync()
    {
      try
      {
        var query = @"select m.name as Name,d.state as State,mp.file_path as Path from
                              music as m
                              left join download as d ON d.music_serial = m.serial
                              left join mp3media as mp ON mp.download_serial = d.serial
                              where d.state < @state".Trim();

        using (_connection)
        {
          _connection.Open();
          return await _connection.QueryAsync<StatusModel>(query, new { state = (int)Mp3State.Done });
        }
      }
      catch (Exception e)
      {
        await _logger.LogAsync(e, messagePrefix: "Failed to get statuses");
        return [new() { Name = "Server Error" }];
      }
    }
  }
}