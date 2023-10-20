using Npgsql;
using Dapper;
using System.Reflection;

var connectionString = "Host=localhost;Username=postgres;Password=Kwende1995!;Database=postgres";

try
{
    using var connection = new NpgsqlConnection(connectionString);
    await connection.OpenAsync();

    // run scripts
    var assembly = Assembly.GetExecutingAssembly();
    var scripts = assembly.GetManifestResourceNames().Where(i => i.EndsWith(".sql"));

    foreach (var script in scripts.OrderBy(i => i[0]))
    {
        using var stream = assembly.GetManifestResourceStream(script);
        using var reader = new StreamReader(stream);

        var command = await reader.ReadToEndAsync();

        await connection.ExecuteAsync(command);
    }

}
catch (Exception e)
{
    Console.WriteLine(e);
}
