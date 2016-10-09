using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;

namespace SortDeleteDupsCaseInsensitive {
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
      if (strWindowTitle.StartsWith("SortDeleteDupsCaseInsensitive")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(1000);

      myActions.Run(@"C:\Windows\system32\notepad.exe", @"C:\Data\listsorting.txt");

      myActions.MessageBoxShow("1. Paste the list that you want to sort and remove dups into notepad that opened. \r\n \r\n 2. SAVE IT \r\n \r\n 3. Hit Okay");

      string strApplicationPath = System.AppDomain.CurrentDomain.BaseDirectory;

      myActions.RunSync(strApplicationPath + "sortdeletedupscaseinsensitive.bat", @"C:\Data\listsorting.txt");

      goto myExit;
      List<ControlEntity> myListControlEntity = new List<ControlEntity>();

      ControlEntity myControlEntity = new ControlEntity();
      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.Heading;
      myControlEntity.Text = "Multiple Controls";
      myListControlEntity.Add(myControlEntity.CreateControlEntity());


      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.Label;
      myControlEntity.ID = "myLabel";
      myControlEntity.Text = "Enter Search Term";
      myControlEntity.RowNumber = 0;
      myControlEntity.ColumnNumber = 0;
      myListControlEntity.Add(myControlEntity.CreateControlEntity());


      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.TextBox;
      myControlEntity.ID = "myTextBox";
      myControlEntity.Text = "Hello World";
      myControlEntity.RowNumber = 0;
      myControlEntity.ColumnNumber = 1;
      myListControlEntity.Add(myControlEntity.CreateControlEntity());

      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.Label;
      myControlEntity.ID = "myLabel2";
      myControlEntity.Text = "Select Website";
      myControlEntity.RowNumber = 1;
      myControlEntity.ColumnNumber = 0;
      myListControlEntity.Add(myControlEntity.CreateControlEntity());

      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.ComboBox;
      myControlEntity.ID = "myComboBox";
      myControlEntity.Text = "Hello World";
      List<ComboBoxPair> cbp = new List<ComboBoxPair>();
      cbp.Add(new ComboBoxPair("google", "http://www.google.com"));
      cbp.Add(new ComboBoxPair("yahoo", "http://www.yahoo.com"));
      myControlEntity.ListOfKeyValuePairs = cbp;
      myControlEntity.SelectedValue = "http://www.yahoo.com";
      myControlEntity.RowNumber = 1;
      myControlEntity.ColumnNumber = 1;
      myListControlEntity.Add(myControlEntity.CreateControlEntity());

      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.CheckBox;
      myControlEntity.ID = "myCheckBox";
      myControlEntity.Text = "Use new tab";
      myControlEntity.RowNumber = 2;
      myControlEntity.ColumnNumber = 0;
      myListControlEntity.Add(myControlEntity.CreateControlEntity());

      string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 500, -1, 0);

      if (strButtonPressed == "btnCancel") {
        myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
        goto myExit;
      }

      string mySearchTerm = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
      string myWebSite = myListControlEntity.Find(x => x.ID == "myComboBox").SelectedValue;

      bool boolUseNewTab = myListControlEntity.Find(x => x.ID == "myCheckBox").Checked;
      if (boolUseNewTab == true) {
        List<string> myWindowTitles = myActions.GetWindowTitlesByProcessName("iexplore");
        myWindowTitles.RemoveAll(item => item == "");
        if (myWindowTitles.Count > 0) {
          myActions.ActivateWindowByTitle(myWindowTitles[0], (int)WindowShowEnum.SW_SHOWMAXIMIZED);
          myActions.TypeText("%(d)", 1500); // select address bar
          myActions.TypeText("{ESC}", 1500);
          myActions.TypeText("%({ENTER})", 1500); // Alt enter while in address bar opens new tab
          myActions.TypeText("%(d)", 1500);
          myActions.TypeText(myWebSite, 1500);
          myActions.TypeText("{ENTER}", 1500);
          myActions.TypeText("{ESC}", 1500);

        } else {
          myActions.Run("iexplore", myWebSite);

        }
      } else {
        myActions.Run("iexplore", myWebSite);
      }

      myActions.Sleep(1000);
      myActions.TypeText(mySearchTerm, 500);
      myActions.TypeText("{ENTER}", 500);


      goto myExit;
      myActions.RunSync(@"C:\Windows\Explorer.EXE", @"C:\SVN");
      myActions.TypeText("%(e)", 500);
      myActions.TypeText("a", 500);
      myActions.TypeText("^({UP 10})", 500);
      myActions.TypeText("^(\" \")", 500);
      myActions.TypeText("+({F10})", 500);
      ImageEntity myImage = new ImageEntity();

      if (boolRunningFromHome) {
        myImage.ImageFile = "Images\\imgSVNUpdate_Home.PNG";
      } else {
        myImage.ImageFile = "Images\\imgSVNUpdate.PNG";
      }
      myImage.Sleep = 200;
      myImage.Attempts = 5;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;

      int[,] myArray = myActions.PutAll(myImage);
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
      myActions.Sleep(1000);
      myActions.LeftClick(myArray);
      myActions.TypeText("%(f)", 200);
      myActions.TypeText("{UP}", 500);
      myActions.TypeText("{ENTER}", 500);
      myActions.Sleep(1000);
      myActions.RunSync(@"C:\Windows\Explorer.EXE", @"");
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