using MyMusic.Api.Services;
using VideoLibrary;

namespace MyMusic.Api
{
  public static class Utils
  {
    private static readonly string LocalFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private const string DownloadFolderName = "mymusic_downloads";
    private const string AudioSourceFolder = "mymusic_source";
    private static readonly string DownloadFolderPath = Path.Combine(LocalFolderPath, DownloadFolderName);
    private static readonly string AudioSourcePath = Path.Combine(LocalFolderPath, AudioSourceFolder);

    static Utils()
    {
      CreateFolderIfNotExists(DownloadFolderPath);
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