using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.IO;

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
      string strFileName = myActions.GetValueByKey("VS2013Path", "IdealAutomateDB");
      TryToFindFile:
      if (!File.Exists(strFileName)) {
        List<ControlEntity> myListControlEntity = new List<ControlEntity>();

        ControlEntity myControlEntity = new ControlEntity();
        myControlEntity.ControlEntitySetDefaults();
        myControlEntity.ControlType = ControlType.Heading;
        myControlEntity.Text = "Wrong File Name";
        myListControlEntity.Add(myControlEntity.CreateControlEntity());


        myControlEntity.ControlEntitySetDefaults();
        myControlEntity.ControlType = ControlType.Label;
        myControlEntity.ID = "myLabel";
        myControlEntity.Text = "Enter Correct File for Visual Studio devenv.exe ";
        myControlEntity.RowNumber = 0;
        myControlEntity.ColumnNumber = 0;
        myListControlEntity.Add(myControlEntity.CreateControlEntity());


        myControlEntity.ControlEntitySetDefaults();
        myControlEntity.ControlType = ControlType.TextBox;
        myControlEntity.ID = "myTextBox";
        myControlEntity.Text = strFileName;
        myControlEntity.RowNumber = 0;
        myControlEntity.ColumnNumber = 1;
        myListControlEntity.Add(myControlEntity.CreateControlEntity());
        bool boolOkayPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 500, -1, 0);

        if (boolOkayPressed == false) {
          myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
          goto myExit;
        }
        string strNewFile = myListControlEntity.Find(x => x.ID == "myTextBox").Text;     
        if (strNewFile == "") {
          myActions.MessageBoxShow("Script cancelled");
          goto myExit;
        } else {
          myActions.SetValueByKey("VS2013Path", strNewFile, "IdealAutomateDB");
          strFileName = strNewFile;
          goto TryToFindFile;
        }
      }
      myActions.Run(strFileName, "");
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
      bool boolStartPageFound = false;
      int intCtr = 0;
      while (boolStartPageFound == false && intCtr < 20) {
        List<string> myVisualStudioTitles = myActions.GetWindowTitlesByProcessName("devenv");
        foreach (var item in myVisualStudioTitles) {
          if (item.StartsWith("Start Page")) {
            boolStartPageFound = true;
            myActions.ActivateWindowByTitle(item,3);


          }
        }
        myActions.Sleep(1000);
      }

      myActions.TypeText("^(+(n))", 2500);
      myActions.Sleep(2000);


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


      myActions.TypeText("^(e)", 2500);
      string myEntityx = "IdealAutomateCoreTemplate";
      myActions.PutEntityInClipboard(myEntityx);

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
      myActions.PutEntityInClipboard(myActions.GetValueByKey("SVNPath", "IdealAutomateDB"));
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
     
    myExit:
      Application.Current.Shutdown();
    }
  }
}
