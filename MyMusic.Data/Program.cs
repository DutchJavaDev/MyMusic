using MyMusic.Common;
using MyMusic.Data;

var connectionString = EnviromentProvider.GetDatabaseConnectionString();

try
{
  DatabaseMigration.EnsureDatabaseCreation(connectionString);
}
catch (Exception e)
{
  Console.WriteLine(e);
}