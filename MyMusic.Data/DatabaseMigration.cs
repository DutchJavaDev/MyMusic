﻿using DbUp;

namespace MyMusic.Data
{
  public static class DatabaseMigration
  {
    public static void EnsureDatabaseCreation(string connectionString)
    {
      EnsureDatabase.For.PostgresqlDatabase(connectionString);

      var upgrader =
       DeployChanges.To
      .PostgresqlDatabase(connectionString)
      .WithScriptsEmbeddedInAssembly(typeof(DatabaseMigration).Assembly)
      .LogToConsole()
      .Build();

      if (upgrader.IsUpgradeRequired())
      {
        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine(result.Error);
          Console.ResetColor();
        }
        else
        {
          Console.ForegroundColor = ConsoleColor.Green;
          Console.WriteLine("Success!");
          Console.ResetColor();
        }
      }
    }
  }
}