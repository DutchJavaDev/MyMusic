using Microsoft.AspNetCore.Components;
using MyMusic.Player.Storage.Models;

namespace MyMusic.Player.Pages
{
  public partial class Settings : ComponentBase
  {
    public Configuration CurrentConfiguration { get; set; }

    private IEnumerable<Configuration> _configurations;
    private IEnumerable<string> _configurationsNames;
    private string _selectedConfig;

    protected override void OnInitialized()
    {
      _configurations = Enumerable.Range(2, 6).Select(i => new Configuration
			{ 
        Serial = i,
        Name = $"Config_{i}",
        CloudPassword = Guid.NewGuid().ToString(),
        CloudUrl = $"localhost:{i*i}",
      });

      CurrentConfiguration = _configurations.First();

      _configurationsNames = _configurations.Select(i => i.Name);

      _selectedConfig = _configurationsNames.First();
    }

    private void Submit(Configuration configuration)
    {
      // TODO
    }

    private void NewConfig()
    {
      CurrentConfiguration = new();
      _selectedConfig = "New Configuration";
    }

    private void DeleteConfig()
    {
      // Does not work, reason temp data lol
      if(CurrentConfiguration != null)
      {
        _configurations = _configurations.Where(i => i != CurrentConfiguration);
        CurrentConfiguration = _configurations.FirstOrDefault();
        _selectedConfig = CurrentConfiguration.Name;
      }
    }

    private void ChangeConfig(string name)
    {
      CurrentConfiguration = _configurations.Where(i => i.Name == name).FirstOrDefault();
    }
  }
}
