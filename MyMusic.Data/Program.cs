using Npgsql;
using Dapper;
using System.Reflection;
using MyMusic.Common;

var connectionString = EnviromentProvider.GetDatabaseConnectionString();
using var connection = new NpgsqlConnection(connectionString);
NpgsqlTransaction? transaction = null;
try
{
    await connection.OpenAsync();
    transaction = await connection.BeginTransactionAsync();

    // run scripts
    var assembly = Assembly.GetExecutingAssembly();
    var scripts = assembly.GetManifestResourceNames().Where(i => i.EndsWith(".sql"));
    
    foreach (var script in scripts.OrderBy(i => i[0]))
    {
        Console.WriteLine(script);
        using var stream = assembly.GetManifestResourceStream(script);
        using var reader = new StreamReader(stream);

        var command = await reader.ReadToEndAsync();

        await connection.ExecuteAsync(command);
    }

    await transaction.CommitAsync();

}
catch (Exception e)
{
    if (transaction is not null)
    {
        await transaction.RollbackAsync();
    }

    Console.WriteLine(e);
}
