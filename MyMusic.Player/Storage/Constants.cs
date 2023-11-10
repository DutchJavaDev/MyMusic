﻿using SQLite;

namespace MyMusic.Player.Storage
{
    public static class Constants
    {
        public static readonly string DatabaseFileName = "mymusic.db3";

        public const SQLiteOpenFlags Flags =
            SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create | // create if not exists
            SQLiteOpenFlags.SharedCache; // multi threaded access to database

        public static string DatabasePath = 
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFileName);
    }
}