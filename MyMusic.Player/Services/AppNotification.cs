using Radzen;

namespace MyMusic.Player.Services
{
	public static class AppNotification
	{
		private static NotificationService _notificationService;
		private static readonly int _notificationDurration = 3000; // 3 seconds

		public static void SetNotificationService(NotificationService notificationService)
		{
			_notificationService = notificationService;
		}

		public static void Success(string title, string message = "")
		{
			ShowNotification(NotificationSeverity.Info, title, message);
		}

		public static void Info(string title, string message = "")
		{
			ShowNotification(NotificationSeverity.Info, title, message);
		}

		public static void Warning(string title, string message = "") 
		{ 
			ShowNotification(NotificationSeverity.Warning, title, message);
		}

		public static void Error(string title, string message = "")
		{ 
			ShowNotification(NotificationSeverity.Error ,title, message);
		}

		private static void ShowNotification(NotificationSeverity notificationSeverity, string title, string message)
		{
			if (_notificationService != null)
			{
				var notificationMessage = new NotificationMessage 
				{
					Severity = notificationSeverity,
					Summary = title,
					Detail = message,
					Duration = _notificationDurration,
				};

				_notificationService.Notify(notificationMessage);
			}
		}
	}
}
