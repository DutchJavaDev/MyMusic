using System.Reflection;

namespace MyMusic.Player
{
  public partial class App : Application
  {
    public App()
    {
      InitializeComponent();

      MainPage = new MainPage();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
      var window =  base.CreateWindow(activationState);

      if(DeviceInfo.Current.Platform == DevicePlatform.WinUI)
      {
        var version = Assembly.GetExecutingAssembly().GetName().Version;

        window.Title = $"MyMusic build: {version}";
      }

      return window;
    }
  }
}