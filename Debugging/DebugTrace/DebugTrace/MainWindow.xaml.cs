using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Diagnostics;
using System.Collections;

namespace DebugTrace {
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
      if (strWindowTitle.StartsWith("DebugTrace")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(1000);
      string directory = AppDomain.CurrentDomain.BaseDirectory;
      directory = directory.Replace("\\bin\\Debug\\", "");
      int intLastSlashIndex = directory.LastIndexOf("\\");
      directory = directory.Substring(0, intLastSlashIndex);
      //  string strScriptName = directory.Substring(intLastSlashIndex + 1);
      // string strScriptName = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
      string settingsDirectory =
Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\" + myActions.ConvertFullFileNameToScriptPath(directory);
      if (!Directory.Exists(settingsDirectory)) {
        Directory.CreateDirectory(settingsDirectory);
      }
      string filePath = Path.Combine(settingsDirectory, "TraceLog.txt");
      string filePath2 = Path.Combine(settingsDirectory, "TempCode.txt");
      string strOutFile = filePath;
      string strOutFile2 = filePath2;
      int intCurrentLine = 0;
      if (File.Exists(strOutFile)) {
        File.Delete(strOutFile);
      }

      List<ControlEntity> myListControlEntity = new List<ControlEntity>();

      ControlEntity myControlEntity = new ControlEntity();
      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.Heading;
      myControlEntity.Text = "Debug Trace";
      myListControlEntity.Add(myControlEntity.CreateControlEntity());


      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.Label;
      myControlEntity.ID = "myLabel";
      myControlEntity.Text = "Variable of Interest";
      myControlEntity.RowNumber = 0;
      myControlEntity.ColumnNumber = 0;
      myListControlEntity.Add(myControlEntity.CreateControlEntity());


      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.TextBox;
      myControlEntity.ID = "txtVariableOfInterest";
      myControlEntity.Text = "Hello World";
      myControlEntity.RowNumber = 0;
      myControlEntity.ColumnNumber = 1;
      myListControlEntity.Add(myControlEntity.CreateControlEntity());

      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.Label;
      myControlEntity.ID = "myLabel2";
      myControlEntity.Text = "Select F10/F11";
      myControlEntity.RowNumber = 1;
      myControlEntity.ColumnNumber = 0;
      myListControlEntity.Add(myControlEntity.CreateControlEntity());

      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.ComboBox;
      myControlEntity.ID = "cbxDepth";
      myControlEntity.Text = "Hello World";
      List<ComboBoxPair> cbp = new List<ComboBoxPair>();
      cbp.Add(new ComboBoxPair("F10 - Step Over", "F10"));
      cbp.Add(new ComboBoxPair("F11 - Step Into", "F11"));
      myControlEntity.ListOfKeyValuePairs = cbp;
      myControlEntity.SelectedValue = "F11";
      myControlEntity.RowNumber = 1;
      myControlEntity.ColumnNumber = 1;
      myListControlEntity.Add(myControlEntity.CreateControlEntity());

      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.CheckBox;
      myControlEntity.ID = "myCheckBox";
      myControlEntity.Text = "Stop at variable";
      myControlEntity.RowNumber = 2;
      myControlEntity.ColumnNumber = 0;
      myListControlEntity.Add(myControlEntity.CreateControlEntity());

      myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

      string myVariableOfInterest = myListControlEntity.Find(x => x.ID == "txtVariableOfInterest").Text;
      string myDepth = myListControlEntity.Find(x => x.ID == "cbxDepth").SelectedValue;

      bool boolStopAtVariable = myListControlEntity.Find(x => x.ID == "myCheckBox").Checked;
      

      myActions.Sleep(1000);
      string strCurrentLine = "";
      string[] myCodeArray = new string[10000];
      string strPrevLine1 = "";
      string strPrevLine2 = "";
      string strPrevLine3 = "";
      string strCurrentFile = "";
      string strCurrentFileText = "";
      string strPrevFile = "";
      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < 9000; i++) {
        myActions.TypeText("{F11}", 500);
        myActions.TypeText("^(c)", 500);
        strCurrentLine = myActions.PutClipboardInEntity();
        goto skipLineNumberProcessing;
        myActions.TypeText("^(w)", 100);
        myActions.TypeText("^(s)", 100);
        myActions.TypeText("^(w)", 100);
        myActions.TypeText("^(p)", 100);
        myActions.TypeText("{DOWN 5}", 200);
        myActions.TypeText("^(c)", 100);
        strCurrentFile = myActions.PutClipboardInEntity();
        if (strCurrentFile != strPrevFile) {
          strPrevFile = strCurrentFile;
          myActions.TypeText("^({F6})", 100);
          myActions.TypeText("^(a)", 100);
          myActions.TypeText("^(c)", 100);
          strCurrentFileText = myActions.PutClipboardInEntity();
          //myActions.MessageBoxShow(strCurrentFileText);
          if (File.Exists(strOutFile2)) {
            File.Delete(strOutFile2);
          }
          WriteTheFile(strOutFile2, strCurrentFileText);
          myCodeArray = File.ReadAllLines(strOutFile2);

        }



        
        for (int j = 0; j < myCodeArray.Length; j++) {
          if (myCodeArray[j] == strCurrentLine.Replace("\r\n", "")) {
            intCurrentLine = j;
            break;
          }
        }
        skipLineNumberProcessing:
        strCurrentLine = strCurrentFile + " " + intCurrentLine.ToString() + " " + strCurrentLine;
        if (strCurrentLine == strPrevLine1 && strPrevLine1 == strPrevLine2 && strPrevLine2 == strPrevLine3) {
          break;
        }
        strPrevLine3 = strPrevLine2;
        strPrevLine2 = strPrevLine1;
        strPrevLine1 = strCurrentLine;

        if (strCurrentLine != "") {
          WriteALine(strCurrentLine);
        }
        if (strCurrentLine.Contains("tmpParamValue= HTMLEncode(Request.Form(strParamName))")) {
          myActions.TypeText("^(d)", 100);
          myActions.TypeText("^(l)", 100);
          myActions.TypeText("^(a)", 100);
          myActions.TypeText("^(c)", 100);
          string strLocals = myActions.PutClipboardInEntity();
          WriteALine(strLocals);
        }
      }
     
      




      string strExecutable = @"C:\Windows\system32\notepad.exe";
      string strContent = strOutFile;
      Process.Start(strExecutable, string.Concat("", strContent, ""));
      myExit:
      myActions.ScriptEndedSuccessfullyUpdateStats();
      Application.Current.Shutdown();
    }

    private void WriteTheFile(string strOutFile2, string strCurrentFileText) {
      Methods myActions = new Methods();
      try {

        //Pass the filepath and filename to the StreamWriter Constructor
        StreamWriter sw = new StreamWriter(strOutFile2);

        //Write a line of text
        sw.Write(strCurrentFileText);

        //Close the file
        sw.Close();
      } catch (Exception e) {
        myActions.MessageBoxShow("Exception: " + e.Message);
      } 
    }

    private void WriteALine(string strPrevLine3) {
      Methods myActions = new Methods();
      string directory = AppDomain.CurrentDomain.BaseDirectory;
      directory = directory.Replace("\\bin\\Debug\\", "");
      int intLastSlashIndex = directory.LastIndexOf("\\");
      directory = directory.Substring(0, intLastSlashIndex);
      //  string strScriptName = directory.Substring(intLastSlashIndex + 1);
      // string strScriptName = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
      string settingsDirectory =
Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\" + myActions.ConvertFullFileNameToScriptPath(directory);
      if (!Directory.Exists(settingsDirectory)) {
        Directory.CreateDirectory(settingsDirectory);
      }
      string filePath = Path.Combine(settingsDirectory, "TraceLog.txt");
      //System.Web.HttpContext.Current.Server.MapPath("~//Trace.html")
      StreamWriter sw = null;

      if (File.Exists(filePath) == false) {
        // Create a file to write to.
        sw = File.CreateText(filePath);

        sw.WriteLine(" ");

        sw.Flush();
        sw.Close();
      }

      try {
        sw = File.AppendText(filePath);
        sw.WriteLine(strPrevLine3);
        sw.Flush();

        sw.Close();
      } catch (Exception Ex) {
      }
    }
  }
}
