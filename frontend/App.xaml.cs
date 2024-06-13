using System.Windows;

namespace frontend;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
  public static string ApiUrl { get; set; }

    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      ApiUrl = "http://localhost:5043/api/";
    }
}

