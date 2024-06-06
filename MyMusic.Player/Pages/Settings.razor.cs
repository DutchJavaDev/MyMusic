using Microsoft.AspNetCore.Components;
using MyMusic.Player.Services.Read;
using MyMusic.Player.Services.Write;
using MyMusic.Player.Storage.Models;
using Radzen;

namespace MyMusic.Player.Pages
{
	public enum ConfigurationState
	{
		Null,
		Create,
		Update
	}
  public partial class Settings : ComponentBase
  {
    public Configuration CurrentConfiguration { get; set; } = new();

		[Inject]
		public ConfigurationReaderService ConfigurationReaderService { get; set; }
		[Inject]
		public ConfigurationWriterService ConfigurationWriterService { get; set; }
		[Inject]
		public NotificationService NotificationService { get; set; }

    private IList<Configuration> _configurations;
    private IEnumerable<string> _configurationsNames = [];
    private string _selectedConfig = string.Empty;
		private ConfigurationState _configurationState;

    protected override async void OnInitialized()
    {
			_configurations = await ConfigurationReaderService.GetConfigurationsAsync()
				.ConfigureAwait(false);

			LoadConfig();

			await InvokeAsync(StateHasChanged);
		}

    private async void Submit(Configuration configuration)
    {
			switch(_configurationState)
			{
				case ConfigurationState.Create:

					if (_configurations.Any(i => i.Name.Equals(configuration.Name)))
					{
						NotificationService.Notify(new()
						{
							Severity = NotificationSeverity.Warning,
							Detail = $"Configuration with name {configuration.Name} already exists.",
							Duration = 5000,
						});
						return;
					}

					await ConfigurationWriterService.AddConfigurationAsync(configuration)
						.ConfigureAwait(false);

					_configurations.Add(configuration);

					NotificationService.Notify(new()
					{
						Severity = NotificationSeverity.Success,
						Detail = "Configuration has been added.",
						Duration = 2500
					});

					_configurationState = ConfigurationState.Null;
					break;

					case ConfigurationState.Update:
					await ConfigurationWriterService.UpdateConfigurationAsync(configuration)
						.ConfigureAwait(false);

					NotificationService.Notify(new() 
					{ 
						Severity = NotificationSeverity.Success,
						Detail = "Configuration has been updated.",
						Duration = 2500
					});

					_configurationState = ConfigurationState.Null;
					break;

					default:break;
			}

			LoadConfig();

			await InvokeAsync(StateHasChanged);
		}

    private void NewConfig()
    {
      CurrentConfiguration = new();
      _selectedConfig = "New Configuration";
			_configurationState = ConfigurationState.Create;
    }

    private async void DeleteConfig()
    {
      if(CurrentConfiguration != null)
      {
				await ConfigurationWriterService.DeleteConfigurationAsync(CurrentConfiguration)
					.ConfigureAwait(false);

				_configurations.Remove(CurrentConfiguration);

				NewConfig();
				LoadConfig();

				await InvokeAsync(StateHasChanged);

				NotificationService.Notify(new()
				{
					Severity = NotificationSeverity.Success,
					Detail = "Configuration has been been deleted.",
					Duration = 2500
				});
			}
    }

		private void LoadConfig()
		{
			if (_configurations is null || !_configurations.Any())
			{
				_configurations = [];
			}
			else
			{
				CurrentConfiguration = _configurations.FirstOrDefault();
				_configurationsNames = _configurations.Select(i => i.Name);
				_selectedConfig = _configurationsNames.FirstOrDefault();
				_configurationState = ConfigurationState.Update;
			}
		}

    private void ChangeConfig(string name)
    {
      CurrentConfiguration = _configurations.Where(i => i.Name == name).FirstOrDefault();
			_configurationState = ConfigurationState.Update;
    }
  }
}
