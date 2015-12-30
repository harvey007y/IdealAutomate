using System.Windows;
using IdealAutomate.Core;
using System.IO;
using System.Collections.Generic;

namespace CopyVSExecutableToIdealAutomate {
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
      InitializeComponent();
      this.Hide();
      IdealAutomate.Core.Methods myActions = new Methods();
      string strRunningFromHome = myActions.GetValueByKey("RunningFromHome", "IdealAutomateDB");
      if (strRunningFromHome == "True") {
        boolRunningFromHome = true;
      }

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
      // activate solution explorer
      myActions.TypeText("^%(l)", 1500);
      // go to top of solution explorer so that bin is not highlighted
      myActions.TypeText("{UP 20}", 500);
      myActions.TypeText("{DOWN 2}", 500);
      myImage = new ImageEntity();
      myImage.ImageFile = "Images\\Show_All_Files.PNG";
      myImage.Sleep = 500;
      myImage.Attempts = 2;
      myImage.RelativeX = 10;
      // click show all files to make sure bin folder is visible
      myActions.ClickImageIfExists(myImage);

      myImage = new ImageEntity();
      if (boolRunningFromHome) {
        myImage.ImageFile = "Images\\BinHome.PNG";
      } else {
        myImage.ImageFile = "Images\\Bin.PNG";
      }
      myImage.Sleep = 500;
      myImage.Attempts = 3;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;
      myImage.Tolerance = 99;

     
      int[,] myArray3 = myActions.PutAll(myImage);
      if (myArray3.Length == 0) {
        myImage = new ImageEntity();
        if (boolRunningFromHome) {
          myImage.ImageFile = "Images\\Bin2Home.PNG";
        } else {
          myImage.ImageFile = "Images\\Bin2.PNG";
        }
        myImage.Sleep = 500;
        myImage.Attempts = 1;
        myImage.RelativeX = 10;

         myArray3 = myActions.PutAll(myImage);
        if (myArray3.Length == 0) {
          myActions.MessageBoxShow("I could not find " + myImage.ImageFile);
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
    TryToFindFile:
      string strWindowsLoginName = myActions.GetValueByKey("WindowsLoginName", "IdealAutomateDB");
 
      string strFileName = @"C:\Users\" + strWindowsLoginName + @"\Desktop\Ideal Automate - 1 .appref-ms";
   
      if (!File.Exists(strFileName)) {
        List<ControlEntity> myListControlEntity = new List<ControlEntity>();

        ControlEntity myControlEntity = new ControlEntity();
        myControlEntity.ControlEntitySetDefaults();
        myControlEntity.ControlType = ControlType.Heading;
        myControlEntity.Text = "Wrong WindowsLoginName";
        myListControlEntity.Add(myControlEntity.CreateControlEntity());


        myControlEntity.ControlEntitySetDefaults();
        myControlEntity.ControlType = ControlType.Label;
        myControlEntity.ID = "myLabel";
        myControlEntity.Text = "Enter Correct WindowsLoginName ";
        myControlEntity.RowNumber = 0;
        myControlEntity.ColumnNumber = 0;
        myListControlEntity.Add(myControlEntity.CreateControlEntity());


        myControlEntity.ControlEntitySetDefaults();
        myControlEntity.ControlType = ControlType.TextBox;
        myControlEntity.ID = "myTextBox";
        myControlEntity.Text = strWindowsLoginName;
        myControlEntity.RowNumber = 0;
        myControlEntity.ColumnNumber = 1;
        myListControlEntity.Add(myControlEntity.CreateControlEntity());
        myActions.WindowMultipleControls(ref myListControlEntity, 700, 900, 0, 0);
        string strNewWindowsLoginName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
        if (strNewWindowsLoginName == "") {
          myActions.MessageBoxShow("Script cancelled");
          goto myExit;
        } else {
          myActions.SetValueByKey("WindowsLoginName", strNewWindowsLoginName, "IdealAutomateDB");
          goto TryToFindFile;
        }
      }
      myActions.Run(strFileName, "");

      myImage = new ImageEntity();
      if (boolRunningFromHome) {
        myImage.ImageFile = "Images\\ScriptNameHome.PNG";
      } else {
        myImage.ImageFile = "Images\\ScriptName.PNG";
      }
      TryAgain:
      myImage.Sleep = 500;
      myImage.Attempts = 10;
      myImage.RelativeX = 10;
      myImage.RelativeY = 40;   
  
      myArray3 = myActions.PutAll(myImage);
      if (myArray3.Length == 0) {
        System.Windows.Forms.DialogResult myResult = myActions.MessageBoxShowWithYesNo("I could not find " + myImage.ImageFile + "Do you want me to try again?");
        if (myResult == System.Windows.Forms.DialogResult.Yes) {
          goto TryAgain;
        } else {
          goto myExit;
        }
      }
      myActions.LeftClick(myArray3);

      myActions.TypeText("^{END}", 500);
      myActions.TypeText("{HOME}", 500);
      myActions.TypeText(strScriptName, 500);
      myActions.TypeText("{TAB 2}", 500);
      myActions.TypeText("IdealAutomateScript",500);

      myImage = new ImageEntity();

      if (boolRunningFromHome) {
        myImage.ImageFile = "Images\\IdealAutomateSave_Home.PNG";
      } else {
        myImage.ImageFile = "Images\\IdealAutomateSave.PNG";
      }
     
      
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
      if (boolRunningFromHome) {
        myImage.ImageFile = "Images\\IdealAutomateOkay_Home.PNG";
      } else {
        myImage.ImageFile = "Images\\IdealAutomateOkay.PNG";
      }
      myImage.Sleep = 500;
      myImage.Attempts = 5;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;
      myArray3 = myActions.PutAll(myImage);
      if (myArray3.Length == 0) {
        myActions.MessageBoxShow("I could not find IdealAutomateOkay.PNG");
      }
      myActions.LeftClick(myArray3);

      myActions.TypeText("%(d)", 500);
      myActions.Sleep(1000);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText(strPathForBin,500);
      myActions.TypeText("\\", 500);
      myActions.TypeText(strScriptName, 500);
      myActions.TypeText(".exe", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText("%(e)", 500);
     

      myImage = new ImageEntity();
      if (boolRunningFromHome) {
        myImage.ImageFile = "Images\\IdealAutomateOkay_Home.PNG";
      } else {
        myImage.ImageFile = "Images\\IdealAutomateOkay.PNG";
      }

      myImage.Sleep = 500;
      myImage.Attempts = 5;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;
      myArray3 = myActions.PutAll(myImage);
      if (myArray3.Length == 0) {
        myActions.MessageBoxShow("I could not find " + myImage.ImageFile);
      }
      myActions.LeftClick(myArray3);

     
      myExit:

      myActions.MessageBoxShow("Script completed");
      Application.Current.Shutdown();
    }
  }
}
