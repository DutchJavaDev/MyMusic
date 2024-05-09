
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
        window.Title = "MyMusic";
      }

      return window;
    }
  }
}