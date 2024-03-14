using Dapper;
using System.Data;
using System.Reflection;

namespace MyMusic.Common.Services
{
  public sealed class DatabaseLogger(IDbConnection connection) : IDisposable
  {
    private readonly string? AppName = Assembly.GetExecutingAssembly().FullName;
    private const string query = "INSERT INTO exception (message,app,stacktrace) VALUES(@message,@app,@stacktrace)";

    public async Task LogExceptionAsync(Exception exception)
    {
      var parameters = CreateExceptionParameters(exception.Message, exception.StackTrace ?? string.Empty);

      await ExecuteAsync(query, parameters);
    }

    public async Task LogAsync(string message, string stacktrace)
    {
      var parameters = CreateParameters(message, stacktrace);

      await ExecuteAsync(query, parameters);
    }

    private object CreateExceptionParameters(string message, string stacktrace = "No stacktrace")
    {
      return new
      {
        app = AppName,
        message = $"Exception: {message}",
        stacktrace
      };
    }

    private object CreateParameters(string message, string stacktrace = "No stacktrace")
    {
      return new
      {
        app = AppName,
        message = $"Message: {message}",
        stacktrace
      };
    }

    private async Task ExecuteAsync(string query, object parameters)
    {
      if(connection.State != ConnectionState.Open)
      {
        connection.Open();
      }

      await connection.ExecuteAsync(query, parameters);
    }

    public void Dispose()
    {
      if(connection.State.Equals(ConnectionState.Open))
      {
        connection.Close();
        connection.Dispose();
      }
    }
  }
}
