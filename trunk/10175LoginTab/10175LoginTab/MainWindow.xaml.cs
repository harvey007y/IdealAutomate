using System.Windows;
using IdealAutomate.Core;


namespace _10175LoginTab {
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
      myActions.DebugMode = true;
     // myActions.MessageBoxShow("Minimize VS if running in debug");

      string strWindowTitle = myActions.PutWindowTitleInEntity();
      if (strWindowTitle.StartsWith("10175Login")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(1000);
      myActions.RunSync(@"C:\SVNIA\trunk\DeleteLockedRecords\DeleteLockedRecords\bin\Debug\DeleteLockedRecords.exe", "");
      myActions.TypeText("^(t)", 2500);
      myActions.TypeText("{F6}", 2500);
      myActions.TypeText("http://localhost/gt/aspx/main/login.aspx", 2500);
      myActions.TypeText("{ENTER}", 2500);

      ImageEntity myImage = new ImageEntity();
      myImage.ImageFile = "Images\\imgLoadedGTreasury.PNG";
      myImage.Sleep = 700;
      myImage.Attempts = 10;
      myImage.RelativeX = 10;

      int[,] myArray = myActions.PutAll(myImage);
      if (myArray.Length == 0) {
        myActions.MessageBoxShow("I could not find GTreasury Logo");
      }
      myActions.TypeText("demo", 200);
      myActions.TypeText("{TAB}", 200);
      myActions.TypeText(myActions.GetValueByKey("myUsualUserName", "IdealAutomateDB"), 200);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText(myActions.GetValueByKey("myUsualPassword", "IdealAutomateDB"), 500);
      myActions.TypeText("{TAB}", 200);
      myActions.TypeText("{TAB}", 200);
      myActions.TypeText("{ENTER}", 200);
      myActions.Sleep(1000);
      myActions.TypeText("%(\" \")", 200);
      myActions.TypeText("x", 200);
      myActions.Sleep(500);
      myImage = new ImageEntity();
      if (boolRunningFromHome == true) {
        myImage.ImageFile = "Images\\Home.PNG";
      } else {
        myImage.ImageFile = "Images\\Home_Work.PNG";
      }
      myImage.Sleep = 200;
      myImage.Attempts = 10;
      myImage.RelativeX = 10;
      myImage.Tolerance = 69;
      int[,] myArray2 = myActions.PutAll(myImage);
      if (myArray2.Length == 0) {
        myActions.MessageBoxShow("I could not find Home hyperlink (10175Login)");
      }
      myActions.TypeText("{F6}", 200);

      myActions.Sleep(500);

      //  myActions.MessageBoxShow("Script completed");








      // myActions.MessageBoxShow(boolWindowFound.ToString());
      // myExit:


      Application.Current.Shutdown();

    }
  }
}
