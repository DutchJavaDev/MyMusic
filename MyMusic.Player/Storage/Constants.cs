using SQLite;

namespace MyMusic.Player.Storage
{
    public static class Constants
    {
        private static readonly string DatabaseFileName = "mymusic.db3";

        public const SQLiteOpenFlags Flags =
            SQLiteOpenFlags.Create | // create if not exists
            SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.SharedCache | // multi threaded access to database
            SQLiteOpenFlags.FullMutex;

        public static string DatabasePath =
            Path.Combine(FileSystem.Current.AppDataDirectory, DatabaseFileName);
    }
}
