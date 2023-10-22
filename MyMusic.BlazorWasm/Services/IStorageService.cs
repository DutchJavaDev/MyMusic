namespace MyMusic.BlazorWasm.Services
{
    public interface IStorageService
    {
        string GetServerPassword();
        void SetServerPassword(string value);
        string GetYouTubeDataApiKey();
        void SetYouTubeDataApiKey(string value);

        string GetMyMusicServerURl();
        void SetMyMusicServerURl(string value);
    }
}
