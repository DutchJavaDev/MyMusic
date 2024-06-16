using Microsoft.AspNetCore.Components;

namespace MyMusic.Player.Services
{
	public static class PageNotificationService
	{
		private static readonly Dictionary<Type, Action<object>> _callbackDictionary;
		private static readonly Dictionary<string, Action<object>> _namedCallbackDictionary;

		static PageNotificationService()
		{
			_callbackDictionary = [];
			_namedCallbackDictionary = [];
		}

		public static void AddActionCallBack(Type type, Action<object> callback)
		{
			_callbackDictionary.TryAdd(type, callback);
		}
		public static void AddNamedCallBack(string name, Action<object> callback)
		{
			_namedCallbackDictionary.TryAdd(name, callback);
		}

		public static void RemoveActionCallBack(Type type)
		{
			_callbackDictionary.Remove(type);
		}
		public static void RemoveNamedCallBack(string name)
		{
			_namedCallbackDictionary.Remove(name);
		}

		public static void InvokeActionCallBackFor(Type type, object data)
		{
			if (_callbackDictionary.TryGetValue(type, out var action))
			{
				action.Invoke(data);
			}
		}
		public static void InvokeNamedCallback(string name, object data)
		{
			if (_namedCallbackDictionary.TryGetValue(name, out var action))
			{
				action.Invoke(data);
			}
		}
	}
}
