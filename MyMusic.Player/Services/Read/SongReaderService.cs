using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyMusic.Common.CommonData;

namespace MyMusic.Player.Services.Read
{
	public sealed class SongReaderService(LocalDatabase localDatabase)
	{
		public async Task<List<Song>> GetSongAsync()
		{
			var query = "select * from Song s inner join SongStatus ss on ss.SongSerial = s.Serial where ss.Status=?";
			return await localDatabase.QueryAsync<Song>(query,(int)Mp3State.Done).ConfigureAwait(false);
		}
	}
}
