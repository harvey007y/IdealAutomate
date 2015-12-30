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
      myActions.TypeText("+({F10})", 500);      
      myActions.TypeText("x", 200);
      myActions.TypeText("{DOWN}", 1000);
     // myActions.TypeText("{ENTER}", 1000);
      myActions.TypeText("{F4}", 1000);
      myActions.TypeText("{ESC}", 200);
      myActions.SelectAllCopy(500);
      string strPathForBin = myActions.PutClipboardInEntity() + @"\bin\debug"; 
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
  
      int[,]myArray3 = myActions.PutAll(myImage);
            if (myArray3.Length == 0)
            {
                System.Windows.Forms.DialogResult myResult = myActions.MessageBoxShowWithYesNo("I could not find " + myImage.ImageFile + " You can manually left-click first script name and click yes button or click no button to cancel");
                if (myResult == System.Windows.Forms.DialogResult.No)
                {
                    goto myExit;
                }
            }
            else {
                myActions.LeftClick(myArray3);
            }

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
            if (myArray3.Length == 0)
            {
                myActions.MessageBoxShow("I could not find IdealAutomateSave.PNG - Please manually click save button");
            }
            else {
                myActions.LeftClick(myArray3);
            }
  
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
            if (myArray3.Length == 0)
            {
                myActions.MessageBoxShow("I could not find IdealAutomateOkay.PNG - Please manually click okay button");
            }
            else {
                myActions.LeftClick(myArray3);
            }

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
            if (myArray3.Length == 0)
            {
                myActions.MessageBoxShow("I could not find " + myImage.ImageFile + "Please manually click it");
            }
            else {
                myActions.LeftClick(myArray3);
            }

     
      myExit:

      myActions.MessageBoxShow("Script completed");
      Application.Current.Shutdown();
    }
  }
}
