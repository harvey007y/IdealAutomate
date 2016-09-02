using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.IO;

namespace OpenEditPlusInVS {
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

      myActions.DebugMode = true;
      ImageEntity myImage = new ImageEntity();

      string strWindowTitle = myActions.PutWindowTitleInEntity();
      if (strWindowTitle.StartsWith("OpenEditPlusInVS")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      //myImage.ImageFile = "Images\\Ready.PNG";
      //myImage.Sleep = 3500;
      //myImage.Attempts = 2;
      //myImage.RelativeX = 10;
      //myActions.ClickImageIfExists(myImage);
      List<string> myWindowTitles = myActions.GetWindowTitlesByProcessName("devenv");
      string myWebSite = "";
      TryAgainClip:
      string myOrigEditPlusLine = myActions.PutClipboardInEntity();
      if (myOrigEditPlusLine.Length == 0) {
        System.Windows.Forms.DialogResult myResult = myActions.MessageBoxShowWithYesNo("You forgot to put line in clipboard - Put line in clipboard and click yes to continue");
        if (myResult == System.Windows.Forms.DialogResult.Yes) {
          goto TryAgainClip;
        } else {
          goto myExit;
        }
      }
      List<string> myBeginDelim = new List<string>();
      List<string> myEndDelim = new List<string>();
      myBeginDelim.Add("\"");
      myEndDelim.Add("\"");
      FindDelimitedTextParms delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);

      string myQuote = "\"";
      delimParms.lines[0] = myOrigEditPlusLine;


      myActions.FindDelimitedText(delimParms);
      int intLastSlash = delimParms.strDelimitedTextFound.LastIndexOf('\\');
      if (intLastSlash < 1) {
        myActions.MessageBoxShow("Could not find last slash in in EditPlusLine - aborting");
        goto myExit;    
      }
      string strPathOnly = delimParms.strDelimitedTextFound.SubstringBetweenIndexes(0, intLastSlash);
      string strFileNameOnly = delimParms.strDelimitedTextFound.Substring(intLastSlash + 1);
      myBeginDelim.Clear();
      myEndDelim.Clear();
      myBeginDelim.Add("(");
      myEndDelim.Add(",");
      delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);
      delimParms.lines[0] = myOrigEditPlusLine;
      myActions.FindDelimitedText(delimParms);
      string strLineNumber = delimParms.strDelimitedTextFound;


      myWindowTitles.RemoveAll(item => item == "");
      if (myWindowTitles.Count > 0) {


        if (strPathOnly.Contains("\\ASPX\\")) {
          myWebSite = myWindowTitles.Find(x => x.StartsWith("Web Source"));
        }

        if (strPathOnly.Contains("\\ASP\\")) {
          myWebSite = myWindowTitles.Find(x => x.StartsWith("WebApp"));
        }

        if (myWebSite == "") {
          myActions.MessageBoxShow("Could not find an open visual studio for this type of file");
        } else {
          myActions.ActivateWindowByTitle(myWebSite, 3);
          myActions.TypeText("^(o)", 1000);
          myActions.TypeText("%(d)", 500);
          myActions.TypeText(strPathOnly, 500);
          myActions.TypeText("{ENTER}", 500);
          myActions.TypeText("%(n)", 500);
          myActions.TypeText(strFileNameOnly, 500);
          myActions.TypeText("{ENTER}", 500);
          myActions.TypeText("^(g)", 1000);
          myActions.TypeText(strLineNumber, 500);
          myActions.TypeText("{ENTER}", 500);
        }
      } else {
        myActions.MessageBoxShow("Could not find an open visual studio for this type of file");
      }
      goto myExit;
      myActions.Sleep(3000);
      string strScriptName = myActions.PutWindowTitleInEntity();
      int intIndex = strScriptName.IndexOf(" - Microsoft");
      if (intIndex < 0) {
        myActions.MessageBoxShow("Could not find - Microsoft in window title for VS; \nPlease click on visual studio for the project you want to copy and click okay \nHere is the title I did find: " + strScriptName + "\nHere is the title that I was looking for: " + myWebSite);

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
      myActions.TypeText("+({F10})", 500);      // open context menu
      myActions.TypeText("x", 200);             // open folder in windows explorer
                                                // myActions.TypeText("{DOWN}", 1000);
                                                // myActions.TypeText("{ENTER}", 1000);
      myActions.TypeText("{F4}", 1000);         // goto address bar in windows explorer
      myActions.TypeText("{ESC}", 200);         // clear the dropdown for the address bar   
      myActions.SelectAllCopy(500);             // copy the path to the clipboard
      string strPathForBin = myActions.PutClipboardInEntity() + @"\bin\debug";
      myActions.CloseApplicationAltFc(200);     // close Windows Explorer
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
        bool boolOkayPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 500, -1, 0);

        if (boolOkayPressed == false) {
          myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
          goto myExit;
        }
        string strNewWindowsLoginName = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
        if (strNewWindowsLoginName == "") {
          myActions.MessageBoxShow("Script cancelled");
          goto myExit;
        } else {
          myActions.SetValueByKey("WindowsLoginName", strNewWindowsLoginName, "IdealAutomateDB");
          goto TryToFindFile;
        }
      }
      List<string> myWindowTitlesx = myActions.GetWindowTitlesByProcessName("Ideal Automate");
      myWindowTitlesx.RemoveAll(item => item == "");
      if (myWindowTitles.Count > 0) {
        myActions.ActivateWindowByTitle(myWindowTitlesx[0]);
      } else {
        myActions.Run(strFileName, "");
      }
      myActions.TypeText("{ESCAPE}", 1000);


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

      int[,] myArray3 = myActions.PutAll(myImage);
      if (myArray3.Length == 0) {
        System.Windows.Forms.DialogResult myResult = myActions.MessageBoxShowWithYesNo("I could not find " + myImage.ImageFile + " You can manually left-click first script name and click yes button or click no button to cancel");
        if (myResult == System.Windows.Forms.DialogResult.No) {
          goto myExit;
        }
      } else {
        myActions.LeftClick(myArray3);
        myActions.LeftClick(myArray3);
      }

      myActions.TypeText("^{END}", 500);
      myActions.TypeText("{HOME}", 500);
      myActions.TypeText(strScriptName, 500);
      myActions.TypeText("{TAB 2}", 500);
      myActions.TypeText("IdealAutomateScript", 500);

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
        myActions.MessageBoxShow("I could not find IdealAutomateSave.PNG - Please manually click save button");
      } else {
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
      if (myArray3.Length == 0) {
        myActions.MessageBoxShow("I could not find IdealAutomateOkay.PNG - Please manually click okay button");
      } else {
        myActions.LeftClick(myArray3);
      }

      myActions.TypeText("%(d)", 500);
      myActions.Sleep(1000);
      myActions.TypeText("{TAB}", 500);
      myActions.TypeText(strPathForBin, 500);
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
        myActions.MessageBoxShow("I could not find " + myImage.ImageFile + "Please manually click it");
      } else {
        myActions.LeftClick(myArray3);
      }


      myExit:

      myActions.MessageBoxShow("Script completed");
      Application.Current.Shutdown();
    }
  }
}
