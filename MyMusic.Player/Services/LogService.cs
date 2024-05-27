using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;

namespace MyMusic.Player.Services
{
  public sealed class LogService(LocalDatabase database)
  {
		private readonly int ERROR = 0;
		private readonly int INFO = 1;

    private async Task WriteLogAsync(Log logEntry)
    {
			await database.InsertAsync(logEntry);
    }

		public async Task LogInfo(string message)
		{
			await WriteLogAsync(new Log
			{
				Message = message,
				DateTime = DateTime.Now.ToString(),
				Type = INFO
			});
		}

		public async Task LogError<T>(Exception exception, T @class) where T : class
    {
      // Get type name
      var type = @class.GetType();
      var name = type.Name;

      // Format message as type name : exception message
      var message = $"{name} : {exception.Message}";

      await WriteLogAsync(new Log
      {
        Message = message,
        StackTrace = exception.StackTrace,
        DateTime = DateTime.Now.ToString(),
				Type = ERROR
      });
    }
  }
}