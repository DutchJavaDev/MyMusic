using Blazored.LocalStorage;

namespace MyMusic.BlazorWasm.Services
{
    public sealed class DefaultStorage : IStorageService
    {
        private readonly string msulrKey = "_4nhys6";
        private readonly string ytdakKey = "_5s8d7g";
        private readonly string spKey = "_sd86a09as";

        private readonly ISyncLocalStorageService _syncLocalStorageService;

        public DefaultStorage(ISyncLocalStorageService syncLocalStorageService)
        {
            _syncLocalStorageService = syncLocalStorageService;
        }

        public string GetMyMusicServerURL()
        {
            return _syncLocalStorageService.GetItemAsString(msulrKey);
        }

        public string GetServerPassword()
        {
            return _syncLocalStorageService.GetItemAsString(spKey);
        }

        public string GetYouTubeDataApiKey()
        {
            return _syncLocalStorageService.GetItemAsString(ytdakKey);
        }

        public void SetMyMusicServerURl(string value)
        {
            // Don't ask why
            if(!value.EndsWith("/"))
            {
                value += '/';
            }

            Set(msulrKey, value);
        }

        public void SetServerPassword(string value)
        {
            Set(spKey, value);
        }

        public void SetYouTubeDataApiKey(string value)
        {
            Set(ytdakKey, value);
        }

        private void Set(string key, string value)
        {
            _syncLocalStorageService.SetItemAsString(key, value);
        }
    }
}
