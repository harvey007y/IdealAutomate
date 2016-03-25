using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;

namespace WindowsExplorerGetFullFileName {
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

      InitializeComponent();
      this.Hide();

      string strWindowTitle = myActions.PutWindowTitleInEntity();
      if (strWindowTitle.StartsWith("WindowsExplorerGetFullFileName")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(1000);
      myActions.TypeText("+({F10})", 500); // right-click filename in windows explorer
      myActions.TypeText("{UP}", 500);  // highlight properties
      myActions.TypeText("+({ENTER})", 500); // select properties
      myActions.TypeText("+({TAB 2})", 500); // focus filename
      myActions.SelectAllCopy(500); // Copy filename
      myActions.TypeText("+({TAB 2})", 500); // focus cancel button
      myActions.TypeText("{ENTER}", 500); // close properties window
      ImageEntity myImage = new ImageEntity();
      myImage.ImageFile = "Images\\imgName.PNG";
      myImage.Sleep = 700;
      myImage.Attempts = 2;
      myImage.RelativeX = 300;
      myImage.RelativeY = -25;
      myActions.ClickImageIfExists(myImage);
      myActions.TypeText("%(d)", 500); // highlight windows explorer address bar
      myActions.TypeText("{END}", 500); // close properties window
      myActions.TypeText("\\", 500); // type slash
      myActions.TypeText("^(v)", 500); // paste filename
      myActions.SelectAllCopy(500); // Copy full filename


      goto myExit;
     
    myExit:
      Application.Current.Shutdown();
    }
  }
}