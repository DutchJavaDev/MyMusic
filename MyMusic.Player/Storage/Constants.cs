using SQLite;

namespace MyMusic.Player.Storage
{
  public static class Constants
  {
    private const string DatabaseFileName = "mymusic.db3";

    public const SQLiteOpenFlags Flags =
        SQLiteOpenFlags.Create | // create if not exists
        SQLiteOpenFlags.ReadWrite |
        SQLiteOpenFlags.SharedCache | // multi threaded access to database
        SQLiteOpenFlags.FullMutex | 
        SQLiteOpenFlags.ProtectionComplete;

    public static string DatabasePath =
        Path.Combine(FileSystem.Current.AppDataDirectory, DatabaseFileName);
  }
}