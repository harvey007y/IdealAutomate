using System.Windows;
using IdealAutomate.Core;

namespace CreateNewVSProjectForScript {
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
      string strRunningFromHome = myActions.GetValueByKey("RunningFromHome", "IdealAutomateDB");
      if (strRunningFromHome == "True") {
        boolRunningFromHome = true;
      } 
      InitializeComponent();
      this.Hide();

      string strWindowTitle = myActions.PutWindowTitleInEntity();
      if (strWindowTitle.StartsWith("CreateNewVSProjectForScript")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Run(myActions.GetValueByKey("VS2013Path","IdealAutomateDB"), "");
      ImageEntity myImage = new ImageEntity();

      if (boolRunningFromHome) {
        myImage.ImageFile = "Images\\imgSolutionExplorer_Home.PNG";
      } else {
        myImage.ImageFile = "Images\\imgSolutionExplorer_Home.PNG";
      }
      myImage.Sleep = 200;
      myImage.Attempts = 5;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;

      int[,] myArray = myActions.PutAll(myImage);
      if (myArray.Length == 0) {
        myActions.MessageBoxShow("I could not find image of " + myImage.ImageFile);
      }
      myActions.TypeText("%(f)", 500);
      myActions.TypeText("n", 500);
      myActions.TypeText("p", 500);
      myImage = new ImageEntity();

      if (boolRunningFromHome) {
        myImage.ImageFile = "Images\\imgStartPage_Home.PNG";
      } else {
        myImage.ImageFile = "Images\\imgStartPage_Home.PNG";
      }
      myImage.Sleep = 200;
      myImage.Attempts = 5;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;

      myArray = myActions.PutAll(myImage);
      if (myArray.Length == 0) {
        myActions.MessageBoxShow("I could not find image of " + myImage.ImageFile);
      }
      myActions.TypeText("^(e)", 500);
      myActions.PutEntityInClipboard("IdealAutomateCore");
      myActions.TypeText("^(v)", 1500);
    //  myActions.TypeText("{ENTER}", 500);
      myActions.TypeText("%(n)", 1500);
      myActions.Sleep(1000);
      
      string strProjectName = myActions.WindowTextBox("Please Enter Project Name");
      if (strProjectName == "") {
        myActions.MessageBoxShow("Script Cancelled");
        goto myExit;
      }
      myActions.Sleep(1000);
      myActions.PutEntityInClipboard(strProjectName);
      myActions.TypeText("^(v)", 500);
      myActions.TypeText("{TAB}", 500);
      myActions.PutEntityInClipboard(myActions.GetValueByKey("SVNPath","IdealAutomateDB"));
      myActions.TypeText("^(v)", 500);
      myActions.TypeText("{TAB 5}", 500);

      myActions.TypeText("{ENTER}", 500);
      myImage = new ImageEntity();

      if (boolRunningFromHome) {
        myImage.ImageFile = "Images\\imgSolutionExplorer_Home.PNG";
      } else {
        myImage.ImageFile = "Images\\imgSolutionExplorer_Home.PNG";
      }
      myImage.Sleep = 200;
      myImage.Attempts = 20;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;

      myArray = myActions.PutAll(myImage);
      if (myArray.Length == 0) {
        myActions.MessageBoxShow("I could not find image of " + myImage.ImageFile);
      }
      myActions.TypeText("{ENTER}", 500);
      myActions.TypeText("^(;)", 500);
      //myActions.TypeText("{TAB}", 500);
      myActions.PutEntityInClipboard("MainWindow.xaml.cs");
      myActions.TypeText("^(v)", 1500);
      myActions.TypeText("{ENTER}", 500);
      myActions.TypeText("{ENTER}", 500);
      goto myExit;
      myActions.TypeText("^(\" \")", 500);
      myActions.TypeText("+({F10})", 500);
     
      // We found output completed and now want to copy the results
      // to notepad

      // Highlight the output completed line

      myImage = new ImageEntity();
      if (boolRunningFromHome) {
        myImage.ImageFile = "Images\\imgUpdateLogOK_Home.PNG";
      } else {
        myImage.ImageFile = "Images\\imgUpdateLogOK.PNG";
      }
      myImage.Sleep = 200;
      myImage.Attempts = 200;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;
      myArray = myActions.PutAll(myImage);
      if (myArray.Length == 0) {
        myActions.MessageBoxShow("I could not find image of OK button for update log");
      }
      myActions.Sleep(1000);
      myActions.LeftClick(myArray);
      myActions.TypeText("%(f)", 200);
      myActions.TypeText("{UP}", 500);
      myActions.TypeText("{ENTER}", 500);
      myActions.Sleep(1000);
      myActions.RunSync(@"C:\Windows\Explorer.EXE", @"C:\SVN\GTreasury\branches");
      myImage = new ImageEntity();
      if (boolRunningFromHome) {
        myImage.ImageFile = "Images\\imgPatch2015_08_Home.PNG";
      } else {
        myImage.ImageFile = "Images\\imgPatch2015_08.PNG";
      }
      myImage.Sleep = 200;
      myImage.Attempts = 200;
      myImage.RelativeX = 30;
      myImage.RelativeY = 10;


      myArray = myActions.PutAll(myImage);
      if (myArray.Length == 0) {
        myActions.MessageBoxShow("I could not find image of " + myImage.ImageFile);
      }
      // We found output completed and now want to copy the results
      // to notepad

      // Highlight the output completed line
      myActions.RightClick(myArray);

      myImage = new ImageEntity();

      if (boolRunningFromHome) {
        myImage.ImageFile = "Images\\imgSVNUpdate_Home.PNG";
      } else {
        myImage.ImageFile = "Images\\imgSVNUpdate.PNG";
      }
      myImage.Sleep = 200;
      myImage.Attempts = 5;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;

      myArray = myActions.PutAll(myImage);
      if (myArray.Length == 0) {
        myActions.MessageBoxShow("I could not find image of SVN Update");
      }
      // We found output completed and now want to copy the results
      // to notepad

      // Highlight the output completed line
      myActions.Sleep(1000);
      myActions.LeftClick(myArray);
      myImage = new ImageEntity();
      if (boolRunningFromHome) {
        myImage.ImageFile = "Images\\imgUpdateLogOK_Home.PNG";
      } else {
        myImage.ImageFile = "Images\\imgUpdateLogOK.PNG";
      }
      myImage.Sleep = 200;
      myImage.Attempts = 200;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;
      myArray = myActions.PutAll(myImage);
      if (myArray.Length == 0) {
        myActions.MessageBoxShow("I could not find image of OK button for update log");
      }
      // We found output completed and now want to copy the results
      // to notepad

      // Highlight the output completed line
      myActions.Sleep(1000);
      myActions.LeftClick(myArray);
      myActions.TypeText("%(f)", 200);
      myActions.TypeText("{UP}", 500);
      myActions.TypeText("{ENTER}", 500);
      myActions.Sleep(1000);
      myActions.Run(@"C:\SVNStats.bat", "");
      myActions.Run(@"C:\Program Files\Microsoft Office\Office15\EXCEL.EXE", @"C:\SVNStats\SVNStats.xlsx");
     myExit:
      Application.Current.Shutdown();
    }
  }
}
