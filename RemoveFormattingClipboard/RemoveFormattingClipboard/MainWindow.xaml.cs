using System.Windows;
using IdealAutomate.Core;

namespace RemoveFormattingClipboard {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      bool boolRunningFromHome = false;
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
            myActions.ScriptStartedUpdateStats();

            InitializeComponent();
      this.Hide();

      string strWindowTitle = myActions.PutWindowTitleInEntity();
      if (strWindowTitle.StartsWith("RemoveFormattingClipboard")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      string myString = myActions.PutClipboardInEntity();
      myActions.PutEntityInClipboard(myString);
      goto myExit;
      
    myExit:
      myActions.MessageBoxShow("Formatting has been removed");
            myActions.ScriptEndedSuccessfullyUpdateStats();
            Application.Current.Shutdown();
    }
  }
}
