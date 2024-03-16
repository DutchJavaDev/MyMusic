using MyMusic.Api.Services;
using VideoLibrary;

namespace MyMusic.Api
{
  public static class Utils
  {
    private const string DownloadFolderName = "mymusic_downloads";
    private const string AudioSourceFolder = "mymusic_source";
    private static readonly string DownloadFolderPath;
    private static readonly string AudioSourcePath;

    static Utils()
    {
      var localFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

      DownloadFolderPath = Path.Combine(localFolderPath, DownloadFolderName);
      CreateFolderIfNotExists(DownloadFolderPath);

      AudioSourcePath = Path.Combine(localFolderPath, AudioSourceFolder);
      CreateFolderIfNotExists(AudioSourcePath);
    }

    public static string GenerateSourePath()
    {
      return GenerateFilePath(AudioSourcePath, ".mp3");
    }

    public static string GenerateDownloadPath()
    {
      return GenerateFilePath(DownloadFolderPath, ".mp4");
    }

    private static string GenerateFilePath(string rootPath, string fileExtension)
    {
      var guid = Guid.NewGuid();

      var fileName = guid.ToString().Replace("-", "");

      return Path.Combine(rootPath, string.Concat(fileName, fileExtension));
    }

    private static void CreateFolderIfNotExists(string Path)
    {
      if (!Directory.Exists(Path))
      {
        Directory.CreateDirectory(Path);
      }
    }

    public static async Task DeleteFileAsync(string path, DbLogger dbLogger)
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

    public static string WriteToDisk(this YouTubeVideo video)
    {
      var path = Path.Combine(DownloadFolderPath, video.FullName);

      File.WriteAllBytes(path, video.GetBytes());

      return path;
    }
  }
}