using System.Windows;
using IdealAutomate.Core;

namespace TestAgentRansack {
  /// <summary>
  /// Interaction logic for MainWindow.xaml....
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      var window = new Window() //make sure the window is invisible
{
  Width = 0,
  Height = 0,
  Left = -2000,
  WindowStyle = WindowStyle.None,
  ShowInTaskbar = false,
  ShowActivated = false,
};
      window.Show();
      IdealAutomate.Core.Methods myActions = new Methods();

      InitializeComponent();
      this.Hide();
      myActions.Run(@"c:\Program Files\Mythicsoft\Agent Ransack\AgentRansack.exe","");
      Application.Current.Shutdown();
    }
  }
}
