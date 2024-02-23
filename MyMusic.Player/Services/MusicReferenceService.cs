using MyMusic.Player.Storage.Models;
using SQLite;

namespace MyMusic.Player.Services
{
    public sealed class MusicReferenceService(SQLiteAsyncConnection connection)
    {
        public async Task InsertAsync(MusicReference reference)
        {
            // fireforget
            _ = await connection.InsertAsync(reference).ConfigureAwait(false);
        }

        public async Task<MusicReference> GetMusicReferenceByIdAsync(Guid trackingId)
        {
            return await connection.GetAsync((MusicReference mr) => mr.TrackingId == trackingId.ToString()).ConfigureAwait(false);
        }

        public async Task<IEnumerable<MusicReference>> GetAllMusicsAsync()
        {
            return await connection.QueryAsync<MusicReference>("select * from music_reference").ConfigureAwait(false);
        }

        public async Task UpdateAsync(MusicReference musicReference)
        {
            await connection.UpdateAsync(musicReference).ConfigureAwait(false);
        }
    }
}
