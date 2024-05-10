using Microsoft.AspNetCore.Components;
using MyMusic.Player.Blazor.Models.Temp;

namespace MyMusic.Player.Pages
{
  public partial class Settings : ComponentBase
  {
    public ConfigurationDemo CurrentConfiguration { get; set; }

    private IEnumerable<ConfigurationDemo> _configurations;
    private IEnumerable<string> _configurationsNames;
    private string _selectedConfig;

    protected override void OnInitialized()
    {
      _configurations = Enumerable.Range(2, 6).Select(i => new ConfigurationDemo
      { 
        Serial = i,
        Name = $"Config_{i}",
        CLoudPassword = Guid.NewGuid().ToString(),
        CloudUrl = $"localhost:{i*i}",
        DataApiKey = Guid.NewGuid().ToString(),
      });

      CurrentConfiguration = _configurations.First();

      _configurationsNames = _configurations.Select(i => i.Name);

      _selectedConfig = _configurationsNames.First();
    }

    private void Submit()
    {

    }

    private void ChangeConfig(string name)
    {
      CurrentConfiguration = _configurations.Where(i => i.Name == name).FirstOrDefault();
    }
  }
}
