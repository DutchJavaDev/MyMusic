using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;

namespace MyMusic.Player.Services.Write
{
	public sealed class LogWriterService(LocalDatabase localDatabase)
	{
		private async Task WriteLogAsync(Log logEntry)
		{
			await localDatabase.InsertAsync(logEntry);
		}

		public async Task Info(string message)
		{
			await WriteLogAsync(new Log
			{
				Message = message,
				DateTime = DateTime.Now,
				LogType = Util.INFO
			});
		}

		public async Task Error(string message, string stacktrace)
		{
			await WriteLogAsync(new Log
			{
				Message = message,
				DateTime = DateTime.Now,
				LogType = Util.ERROR,
				StackTrace = stacktrace
			});
			AppNotification.Error("Error", message);
		}

		public async Task Error<T>(Exception exception, T @class) where T : class
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
				DateTime = DateTime.Now,
				LogType = Util.ERROR
			});

			AppNotification.Error("Error", message);
		}
	}
}
