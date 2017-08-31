using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Diagnostics;

namespace NotepadOpenFileGoToLine {
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
      if (strWindowTitle.StartsWith("NotepadOpenFileGoToLine")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(500);
      myActions.TypeText("{RIGHT}", 500);
      myActions.TypeText("{HOME}", 500);
      myActions.TypeText("+({END})", 500);
      myActions.TypeText("^(c)", 500);
      string strCurrentLine = myActions.PutClipboardInEntity();
      List<string> myBeginDelim = new List<string>();
      List<string> myEndDelim = new List<string>();
      myBeginDelim.Add("\"");
      myEndDelim.Add("\"");
      FindDelimitedTextParms delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);

      string myQuote = "\"";
      delimParms.lines[0] = strCurrentLine;


      myActions.FindDelimitedText(delimParms);
      int intLastSlash = delimParms.strDelimitedTextFound.LastIndexOf('\\');
      if (intLastSlash < 1) {
        myActions.MessageBoxShow("Could not find last slash in in EditPlusLine - aborting");
        goto myExit;
      }
      string strPathOnly = delimParms.strDelimitedTextFound.SubstringBetweenIndexes(0, intLastSlash);
      string strFileNameOnly = delimParms.strDelimitedTextFound.Substring(intLastSlash + 1);
      string strFullFileName = delimParms.strDelimitedTextFound;
      myBeginDelim.Clear();
      myEndDelim.Clear();
      myBeginDelim.Add("(");
      myEndDelim.Add(",");
      delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);
      delimParms.lines[0] = strCurrentLine;
      myActions.FindDelimitedText(delimParms);
      string strLineNumber = delimParms.strDelimitedTextFound;
      string strExecutable = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
      string strContent = strFullFileName;
      Process.Start(strExecutable, string.Concat("", strContent, ""));
      myActions.TypeText("^(g)", 2500);
      myActions.TypeText(strLineNumber, 1000);
      myActions.TypeText("{ENTER}", 500);
      myActions.TypeText("^(f)", 1500);

      string strFindWhatToUse = myActions.GetValueByKeyForNonCurrentScript("FindWhatToUse","FindTextInFiles");
      string blockText = strFindWhatToUse;
      strFindWhatToUse = "";
      char[] specialChars = { '{', '}', '(', ')', '+', '^' };

      foreach (char letter in blockText) {
        bool _specialCharFound = false;

        for (int i = 0; i < specialChars.Length; i++) {
          if (letter == specialChars[i]) {
            _specialCharFound = true;
            break;
          }
        }

        if (_specialCharFound)
          strFindWhatToUse += "{" + letter.ToString() + "}";
        else
          strFindWhatToUse += letter.ToString();
      }
     
      myActions.TypeText(strFindWhatToUse, 500);
      myActions.TypeText("{ENTER}", 500);
      myActions.TypeText("{ESC}", 500);
    
      goto myExit;
      
      myExit:
      myActions.ScriptEndedSuccessfullyUpdateStats();
      Application.Current.Shutdown();
    }
  }
}