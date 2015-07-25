using System.Windows;
using IdealAutomate.Core;

namespace TestAgentRansack {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
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
      // Register Pause Key as globalhotkey to allow user to stop
      // script from executing. Pause key will kill all processes with
      // assembly description of IdealAutomateScript. It does this
      // allow you to stop any child scripts that were launched from
      // a parent script
      myActions.RegisterHotKey(window); 
      InitializeComponent();
      this.Hide();
      myActions.Run(@"c:\Program Files\Mythicsoft\Agent Ransack\AgentRansack.exe","");
      Application.Current.Shutdown();
    }
  }
}
