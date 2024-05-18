using Microsoft.AspNetCore.Components;

namespace MyMusic.Player.Services
{
	public static class PageNotificationService
	{
		private static readonly Dictionary<Type, Action<object>> _callbackDictionary;

		static PageNotificationService() 
		{
			_callbackDictionary = [];
		}

		public static void AddActionCallBack(Type type, Action<object> callback)
		{
			_callbackDictionary.TryAdd(type, callback);
		}

		public static void RemoveActionCallBack(Type type) 
		{
			_callbackDictionary.Remove(type);
		}

		public static void InvokeActionCallBackFor(Type type, object data)
		{
			if( _callbackDictionary.TryGetValue(type, out var action) ) 
			{
				action.Invoke(data);
			}
		}
	}
}
