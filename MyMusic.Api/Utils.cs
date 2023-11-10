using VideoLibrary;

namespace MyMusic.Api
{
    public static class Utils
    {
        private readonly static string DownloadFolderName = "mymusic_downloads";
        private static string? FolderPath = string.Empty;

        static Utils()
        {
            var localFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            FolderPath = Path.Combine(localFolderPath, DownloadFolderName);

            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }
        }

        public static string WriteToDownloads(this YouTubeVideo? video)
        {
            if (string.IsNullOrEmpty(FolderPath))
            {
                throw new ArgumentNullException(nameof(FolderPath));
            }

            var path = Path.Combine(FolderPath, video.FullName);
            
            File.WriteAllBytes(path, video.GetBytes());
            
            return path;
        }

        public enum Mp3State : int
        {
            Null,
            Created,
            Downloaded,
            Converted,
            Uploading,
            Done,
            Failed
        }
    }
}
