using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.IO;

namespace ExecuteWithBreakpointTrace {
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
      myActions.ScriptStartedUpdateStats();

      InitializeComponent();
      this.Hide();

      string strWindowTitle = myActions.PutWindowTitleInEntity();
      if (strWindowTitle.StartsWith("ExecuteWithBreakpointTrace")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(4000);
      // TODO: display dialog asking them which visual studio to activate
      // TODO: In IdealAutomateCore, create another PutAll override that does not ask if you want an alternative image when it cannot find image
      string firstLine = "";
      string currentLine = "";
      string currentLineNumber = "";
      string firstFileName = "";
      string currFileName = "";
      string firstLineNumber = "";
      List<LineOfCode> listExecutedCode = new List<LineOfCode>();
      myActions.TypeText("{F11}", 1000); // compile program and go to first breakpoint
      int intCtr = 0;
      TryToFindYellowArrow:
      intCtr++;
      ImageEntity myImage = new ImageEntity();

      if (boolRunningFromHome) {
        myImage.ImageFile = "Images\\imgYellowArrow.PNG";
      } else {
        myImage.ImageFile = "Images\\imgYellowArrow.PNG";
      }
      myImage.Sleep = 200;
      myImage.Attempts = 1;
      myImage.RelativeX = 10;
      myImage.RelativeY = 10;
      myImage.Tolerance = 60;

      int[,] myArray = myActions.PutAllDoNotCheckForAlternative(myImage);
      if (myArray.Length == 0 && intCtr < 50) {
        goto TryToFindYellowArrow;
      }
      if (myArray.Length == 0) {
        myActions.MessageBoxShow("I could not find image of YellowArrow");
      } else {
       // myActions.MessageBoxShow("Found Yellow Arrow");
      }
      myActions.TypeText("^(c)", 200);
      currentLine = myActions.PutClipboardInEntity();
      firstLine = currentLine;
      LineOfCode myLine = new LineOfCode();
      myLine.TextOfCode = currentLine;

      // get line number
      myActions.TypeText("^(g)", 200);
      myActions.TypeText("^(a)", 200);
      myActions.TypeText("^(c)", 200);
      currentLineNumber = myActions.PutClipboardInEntity();
      firstLineNumber = currentLineNumber;
      myLine.LineNumber = currentLineNumber;
      myActions.TypeText("{ESCAPE}", 200);

      // get filename
      myActions.TypeText("%(f)", 200);
      myActions.TypeText("a", 200);
      myActions.TypeText("^(c)", 200);
      currFileName = myActions.PutClipboardInEntity();
      myLine.FileName = currFileName;
      firstFileName = currFileName;
      myActions.TypeText("{ESCAPE}", 200);

      // add the line to list
      listExecutedCode.Add(myLine);

      GetNextLine:
      // get next line
      myActions.TypeText("{F11}", 200); // next breakpoint

      // get line number
      myActions.TypeText("^(g)", 200);
      myActions.TypeText("^(a)", 200);
      myActions.TypeText("^(c)", 200);
      currentLineNumber = myActions.PutClipboardInEntity();
      firstLineNumber = currentLineNumber;
      myLine.LineNumber = currentLineNumber;
      myActions.TypeText("{ESCAPE}", 200);

      // get filename
      myActions.TypeText("%(f)", 200);
      myActions.TypeText("a", 200);
      myActions.TypeText("^(c)", 200);
      currFileName = myActions.PutClipboardInEntity();
      myLine.FileName = currFileName;
      firstFileName = currFileName;
      myActions.TypeText("{ESCAPE}", 200);

      // add the line to list
      listExecutedCode.Add(myLine);

      if (myLine.LineNumber != firstLineNumber) {
        goto GetNextLine;
      }

      myActions.MessageBoxShow("Successfully Reached End of Execution");

      string strOutFile = @"C:\Data\ExecutedCode.txt";
      if (File.Exists(strOutFile)) {
        File.Delete(strOutFile);
      }
      using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutFile)) {
        // Write list to text file so I can look at it
        foreach (LineOfCode item in listExecutedCode) {
          file.WriteLine(item.FileName + " " + item.LineNumber + " " + item.TextOfCode);
        }
        
      }


      string strExecutable = @"C:\Windows\system32\notepad.exe";
      myActions.RunSync(strExecutable, strOutFile);


      // We found output completed and now want to copy the results
      // to notepad

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

      myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

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
      if (myWebSite == "http://www.google.com") {
        myActions.TypeText("%(d)", 500);
        myActions.TypeText("{ESC}", 500);
        myActions.TypeText("{F6}", 500);
        myActions.TypeText("{TAB}", 500);
        myActions.TypeText("{TAB 2}", 500);
        myActions.TypeText("{ESC}", 500);
      }
      myActions.TypeText(mySearchTerm, 500);
      myActions.TypeText("{ENTER}", 500);


      goto myExit;
      myActions.RunSync(@"C:\Windows\Explorer.EXE", @"C:\SVN");
      myActions.TypeText("%(e)", 500);
      myActions.TypeText("a", 500);
      myActions.TypeText("^({UP 10})", 500);
      myActions.TypeText("^(\" \")", 500);
      myActions.TypeText("+({F10})", 500);
   
      myActions.TypeText("%(f)", 200);
      myActions.TypeText("{UP}", 500);
      myActions.TypeText("{ENTER}", 500);
      myActions.Sleep(1000);
      myActions.Run(@"C:\SVNStats.bat", "");
      myActions.Run(@"C:\Program Files\Microsoft Office\Office15\EXCEL.EXE", @"C:\SVNStats\SVNStats.xlsx");
      myExit:
      myActions.ScriptEndedSuccessfullyUpdateStats();
      Application.Current.Shutdown();
    }
  }
}
