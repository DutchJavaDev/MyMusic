using MyMusic.Player.Storage;
using MyMusic.Player.Storage.Models;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

		public async Task Error(string message, string stacktrace, NotificationService notificationService = null)
		{
			await WriteLogAsync(new Log
			{
				Message = message,
				DateTime = DateTime.Now,
				LogType = Util.ERROR,
				StackTrace = stacktrace
			});
			LogWriterService.Notify(notificationService, message);
		}

		public async Task Error<T>(Exception exception, T @class, NotificationService notificationService = null) where T : class
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

			LogWriterService.Notify(notificationService, message);
		}

		private static void Notify(NotificationService notificationService, string detail)
		{
			if (notificationService is null) return;
			
			notificationService.Notify(new() 
			{
				Duration = 5000, // 5 seconds
				Severity = NotificationSeverity.Error,
				Summary = "An error accourd \r\n",
				Detail = detail
			});
		}
	}
}
