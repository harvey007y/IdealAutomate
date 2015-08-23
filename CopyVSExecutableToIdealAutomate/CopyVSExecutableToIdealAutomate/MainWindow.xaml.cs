using System.Windows;
using IdealAutomate.Core;

namespace CopyVSExecutableToIdealAutomate {
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
      if (strWindowTitle.StartsWith("CopyVSExecutableToIdealAutomate")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      } else {
        myActions.MessageBoxShow("I could not find VS - please minimize it if you are running in debug mode");
      }
    
      myImage.ImageFile = "Images\\Ready.PNG";
      myImage.Sleep = 3500;
      myImage.Attempts = 2;
      myImage.RelativeX = 10;
      myActions.ClickImageIfExists(myImage);

      string strScriptName = myActions.PutWindowTitleInEntity();
      int intIndex = strScriptName.IndexOf(" - Microsoft");
      if (intIndex < 0) {
        myActions.MessageBoxShow("Could not find - Microsoft in window title for VS");
      }
      strScriptName = strScriptName.Substring(0, intIndex);
      //myImage.ImageFile = "Images\\Solution_Explorer.PNG";
      //myImage.Sleep = 500;
      //myImage.Attempts = 1;
      //myImage.RelativeX = 10;
      //myActions.ClickImageIfExists(myImage);
      myActions.TypeText("^%(l)", 500);
      myActions.TypeText("{UP 20}", 500);
      myImage = new ImageEntity();
      myImage.ImageFile = "Images\\Show_All_Files.PNG";
      myImage.Sleep = 500;
      myImage.Attempts = 1;
      myImage.RelativeX = 10;
      myActions.ClickImageIfExists(myImage);

      myImage = new ImageEntity();
      myImage.ImageFile = "Images\\BinHome.PNG";
      myImage.Sleep = 500;
      myImage.Attempts = 3;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;
      myImage.Tolerance = 99;

     
      int[,] myArray3 = myActions.PutAll(myImage);
      if (myArray3.Length == 0) {
        myImage = new ImageEntity();
        myImage.ImageFile = "Images\\Bin2.PNG";
        myImage.Sleep = 500;
        myImage.Attempts = 1;
        myImage.RelativeX = 10;

         myArray3 = myActions.PutAll(myImage);
        if (myArray3.Length == 0) {
          myActions.MessageBoxShow("I could not find Bin2Home.PNG");
        }
      }
      myActions.RightClick(myArray3);
      myActions.TypeText("x", 200);
      myActions.TypeText("{DOWN}", 1000);
      myActions.TypeText("{ENTER}", 1000);
      myActions.TypeText("{F4}", 1000);
      myActions.SelectAllCopy(1000);
      string strPathForBin = myActions.PutClipboardInEntity();
      myActions.CloseApplicationAltFc(200);

      myActions.Run(@"C:\Users\wharvey\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\IdealAutomate.com\Ideal Automate.appref-ms","");

      myImage = new ImageEntity();
      myImage.ImageFile = "Images\\ScriptNameHome.PNG";
      myImage.Sleep = 500;
      myImage.Attempts = 500;
      myImage.RelativeX = 10;
      myImage.RelativeY = 40;   
  
      myArray3 = myActions.PutAll(myImage);
      if (myArray3.Length == 0) {
        myActions.MessageBoxShow("I could not find ScriptName.PNG");
      }
      myActions.LeftClick(myArray3);

      myActions.TypeText("^{END}", 500);
      myActions.TypeText("{HOME}", 500);
      myActions.TypeText(strScriptName, 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("IdealAutomateScript",500);

      myImage = new ImageEntity();
      myImage.ImageFile = "Images\\IdealAutomateSave.PNG";
      myImage.Sleep = 500;
      myImage.Attempts = 5;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;
      myArray3 = myActions.PutAll(myImage);
      if (myArray3.Length == 0) {
        myActions.MessageBoxShow("I could not find IdealAutomateSave.PNG");
      }
      myActions.LeftClick(myArray3);
  
      myImage = new ImageEntity();
      myImage.ImageFile = "Images\\IdealAutomateOkay.PNG";
      myImage.Sleep = 500;
      myImage.Attempts = 5;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;
      myArray3 = myActions.PutAll(myImage);
      if (myArray3.Length == 0) {
        myActions.MessageBoxShow("I could not find IdealAutomateOkay.PNG");
      }
      myActions.LeftClick(myArray3);

      myImage = new ImageEntity();
      myImage.ImageFile = "Images\\IdealAutomatePrimitivesHome.PNG";
      myImage.Sleep = 500;
      myImage.Attempts = 5;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;
      myArray3 = myActions.PutAll(myImage);
      if (myArray3.Length == 0) {
        myActions.MessageBoxShow("I could not find IdealAutomatePrimitives.PNG");
      }
      myActions.LeftClick(myArray3);
      myActions.Sleep(1000);
      myActions.TypeText("txtExe", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("s", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText(strPathForBin,500);
      myActions.TypeText("\\", 500);
      myActions.TypeText(strScriptName, 500);
      myActions.TypeText(".exe", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("{UP}", 500);

      myActions.TypeText("Alt-Space-N", 500);
      string strAltSpaceN = "%(\" \"n)";
      myActions.TypeText("{TAB}", 500);
     
      myActions.TypeText("s", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.PutEntityInClipboard(strAltSpaceN);
      myActions.TypeText("^(v)", 500);

      myImage = new ImageEntity();
      myImage.ImageFile = "Images\\IdealAutomateSave.PNG";
      myImage.Sleep = 500;
      myImage.Attempts = 5;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;
      myImage.Tolerance = 55;
      myArray3 = myActions.PutAll(myImage);
      if (myArray3.Length == 0) {
        myActions.MessageBoxShow("I could not find IdealAutomateSave.PNG");
      }
      myActions.LeftClick(myArray3);

      myImage = new ImageEntity();
      myImage.ImageFile = "Images\\IdealAutomateOkay.PNG";
      myImage.Sleep = 500;
      myImage.Attempts = 5;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;
      myArray3 = myActions.PutAll(myImage);
      if (myArray3.Length == 0) {
        myActions.MessageBoxShow("I could not find IdealAutomateOkay.PNG");
      }
      myActions.LeftClick(myArray3);

      myImage = new ImageEntity();
      myImage.ImageFile = "Images\\IdealAutomateLogicHome.PNG";
      myImage.Sleep = 500;
      myImage.Attempts = 5;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;
      myArray3 = myActions.PutAll(myImage);
      if (myArray3.Length == 0) {
        myActions.MessageBoxShow("I could not find IdealAutomateLogic.PNG");
      }
      myActions.LeftClick(myArray3);

      myActions.TypeText("1", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("t", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("a", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("{UP}", 500);
      myActions.TypeText("2", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("ru", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("t", 500);

      myImage = new ImageEntity();
      myImage.ImageFile = "Images\\IdealAutomateSave.PNG";
      myImage.Sleep = 500;
      myImage.Attempts = 5;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;
      myImage.Tolerance = 55;
      myArray3 = myActions.PutAll(myImage);
      if (myArray3.Length == 0) {
        myActions.MessageBoxShow("I could not find IdealAutomateSave.PNG");
      }
      myActions.LeftClick(myArray3);

      myImage = new ImageEntity();
      myImage.ImageFile = "Images\\IdealAutomateOkay.PNG";
      myImage.Sleep = 500;
      myImage.Attempts = 5;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;
      myArray3 = myActions.PutAll(myImage);
      if (myArray3.Length == 0) {
        myActions.MessageBoxShow("I could not find IdealAutomateOkay.PNG");
      }
      myActions.LeftClick(myArray3);


      myActions.MessageBoxShow("Script completed");
      Application.Current.Shutdown();
    }
  }
}
