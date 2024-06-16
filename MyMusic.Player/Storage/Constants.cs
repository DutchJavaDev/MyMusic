using SQLite;

namespace MyMusic.Player.Storage
{
	public static class Constants
  {
    private const string DatabaseFileName = "mymusic.db";

    public const SQLiteOpenFlags DatabaseFlags =
        SQLiteOpenFlags.Create | // create if not exists
        SQLiteOpenFlags.ReadWrite |
        SQLiteOpenFlags.SharedCache | // multi threaded access to database
        SQLiteOpenFlags.FullMutex | 
        SQLiteOpenFlags.ProtectionComplete;

		public const CreateFlags TableCreateFlags = CreateFlags.AutoIncPK;

    public static string DatabasePath =
        Path.Combine(FileSystem.Current.AppDataDirectory, DatabaseFileName);
  }
}