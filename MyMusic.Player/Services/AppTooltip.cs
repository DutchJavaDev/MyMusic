using Microsoft.AspNetCore.Components;
using Radzen;

namespace MyMusic.Player.Services
{
	public static class AppTooltip
	{
		private static TooltipService _tooltipService;
		private static readonly int _tooltipDurration = 2000; // 2 seconds

		public static void SetTooltipService(TooltipService tooltipService)
		{
			_tooltipService = tooltipService;
		}

		public static void ShowTooltip(ElementReference element, string message, TooltipOptions options = null)
		{
			options ??= new()
			{
				Duration = _tooltipDurration,
				Position = TooltipPosition.Bottom
			};

			_tooltipService.Open(element, message, options);
		}
	}
}
