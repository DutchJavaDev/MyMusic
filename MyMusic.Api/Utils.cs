using MyMusic.Api.Services;
using VideoLibrary;

namespace MyMusic.Api
{
  public static class Utils
  {
    public static readonly string MinioReadonlyPolicy = "readonly";
    public static readonly string MinioWriteOnlyPolicy = "writeonly";

    private static readonly string DownloadFolderName = "mymusic_downloads";
    private static readonly string AudioSourceFolder = "mymusic_source";
    private static readonly string DownloadFolderPath = string.Empty;
    private static readonly string AudioSourcePath = string.Empty;

    static Utils()
    {
      var localFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

      DownloadFolderPath = Path.Combine(localFolderPath, DownloadFolderName);
      if (!Directory.Exists(DownloadFolderPath))
      {
        Directory.CreateDirectory(DownloadFolderPath);
      }
      else
      {
        // Lets not do this
        // Files in this folder will be deleted on application startup fyi
        // Clear downloads
        //var files = Directory.GetFiles(DownloadFolderPath);
        //foreach (var file in files)
        //{
        //    File.Delete(file);
        //}
      }

      AudioSourcePath = Path.Combine(localFolderPath, AudioSourceFolder);
      if (!Directory.Exists(AudioSourcePath))
      {
        Directory.CreateDirectory(AudioSourcePath);
      }
    }

    public static string GenerateDownloadPath()
    {
      var guid = Guid.NewGuid();

      var fileName = guid.ToString().Replace("-", "");

      // dowloading the audio part of a video stream, with .mp3 its not converted right
      return Path.Combine(DownloadFolderPath, string.Concat(fileName, ".mp4"));
    }

    public static async Task DeleteDownloadAsync(string path, DbLogger dbLogger)
    {
      try
      {
        File.Delete(path);
      }
      catch (Exception e)
      {
        await dbLogger.LogAsync(e);
      }
    }

    public static string GenerateSourePath()
    {
      var guid = Guid.NewGuid();

      var fileName = guid.ToString().Replace("-", string.Empty);

      return Path.Combine(AudioSourcePath, string.Concat(fileName, ".mp3"));
    }

    public static string WriteToDisk(this YouTubeVideo? video)
    {
      if (video is null)
      {
        throw new ArgumentNullException(nameof(video));
      }

      var path = Path.Combine(DownloadFolderPath, video.FullName);

      File.WriteAllBytes(path, video.GetBytes());

      return path;
    }
  }
}