using System.Windows;
using IdealAutomate.Core;

namespace ClearCache {
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
      InitializeComponent();
      this.Hide();
      IdealAutomate.Core.Methods myActions = new Methods();

      myActions.DebugMode = true;
      ImageEntity myImage = new ImageEntity();

      string strWindowTitle = myActions.PutWindowTitleInEntity();
      if (strWindowTitle.StartsWith("ClearCache")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(1000);
      // Open Windows Explorer
      myActions.Run(@"C:\Windows\Explorer.EXE", @"C:\GTreasury\WebCache\Clients\DEMO");
      myActions.TypeText("^(a)", 1000);
      myActions.TypeText("{DELETE}", 1000);
      myActions.TypeText("{ENTER}", 1000);  // confirm delete
      myActions.TypeText("+({F4})", 1000);
      myActions.TypeText("^(a)", 1000);
      myActions.TypeText("{DELETE}", 1000);
      myActions.PutEntityInClipboard(@"C:\Users\wharvey\AppData\Roaming\Microsoft\Windows\Cookies");
      myActions.TypeText("^(v)", 1000);
      myActions.TypeText("{ENTER}", 1000);
      myActions.TypeText("^(a)", 1000);
      myActions.TypeText("{DELETE}", 1000);
      myActions.TypeText("{ENTER}", 1000);  // confirm delete
      myActions.TypeText("%(f)", 1000);  // close windows explorer
      myActions.TypeText("c", 1000);
      myActions.Run(@"c:\Program Files\Internet Explorer\iexplore.exe", "");
      myActions.TypeText("%(t)", 1000);  // tools
      myActions.TypeText("o", 1000);  // options
      myActions.TypeText("%(d)", 1000);  // delete
      myActions.TypeText("%(d)", 1000);  // delete
      myActions.TypeText("{ESC}", 1000);  // escape
      myActions.TypeText("%(fx)", 1000);  // close internet explorer


      goto myExit;
      myActions.Run(@"C:\SVNIA\trunk\10175Login\10175Login\bin\Debug\10175Login.exe", "");

      myImage.ImageFile = "Images\\Home.PNG";
      myImage.ImageSleep = 500;
      myImage.ImageAttempts = 10;
      myImage.ImageRelativeX = 10;
      int[,] myArray2 = myActions.PutAll(myImage);
      if (myArray2.Length == 0) {
        myActions.MessageBoxShow("I could not find Home hyperlink (FindUntranslatedAll)");
      }

      myActions.Sleep(2000);
      myActions.TypeText("javascript:GoUrl{(}'http://localhost:80/webcash/ASPX/gl/glJournalList.aspx'{)};", 200);
      myActions.TypeText("{ENTER}", 500);
      myActions.Sleep(5000);
      // myActions.MessageBoxShow("Script completed");
      myActions.Run(@"C:\SVNIA\trunk\FindUntranslated\FindUntranslated\bin\Debug\FindUntranslated.exe", "");

      // todo look for picture script completed and click on it before opening next screen
      myImage = new ImageEntity();
      myImage.ImageFile = "Images\\Script_Completed_OK_Button.PNG";
      myImage.ImageSleep = 5000;
      myImage.ImageAttempts = 500;
      myImage.ImageRelativeX = 10;
      int[,] myArray3 = myActions.PutAll(myImage);
      if (myArray3.Length == 0) {
        myActions.MessageBoxShow("I could not find Script_Completed_OK_Button.PNG");
      }
      myExit:
      Application.Current.Shutdown();

    }
  }
}
