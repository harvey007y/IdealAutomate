using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;

namespace OpenCurrentPageInVS {
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
      if (strWindowTitle.StartsWith("OpenCurrentPageInVS")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(1000);
      myActions.TypeText("%(d)", 500);
      myActions.TypeText("^(c)", 500);
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
      delimParms.strDelimitedTextFound = myOrigEditPlusLine;


     // myActions.FindDelimitedText(delimParms);
      int intLastSlash = delimParms.strDelimitedTextFound.LastIndexOf('/');
      if (intLastSlash < 1) {
        myActions.MessageBoxShow("Could not find last slash in in EditPlusLine - aborting");
        goto myExit;
      }
      string strPathOnly = delimParms.strDelimitedTextFound.SubstringBetweenIndexes(0, intLastSlash);
      string strFileNameOnly = delimParms.strDelimitedTextFound.Substring(intLastSlash + 1);
      //myBeginDelim.Clear();
      //myEndDelim.Clear();
      //myBeginDelim.Add("(");
      //myEndDelim.Add(",");
      //delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);
      //delimParms.lines[0] = myOrigEditPlusLine;
      //myActions.FindDelimitedText(delimParms);
      //string strLineNumber = delimParms.strDelimitedTextFound;


      myWindowTitles.RemoveAll(item => item == "");
      if (myWindowTitles.Count > 0) {


        if (strFileNameOnly.ToLower().EndsWith("aspx")) {
          myWebSite = myWindowTitles.Find(x => x.StartsWith("WEB Source"));
          strFileNameOnly += ".cs";
        }

        if (strFileNameOnly.ToLower().EndsWith("asp")) {
          myWebSite = myWindowTitles.Find(x => x.StartsWith("WebApp"));
        }

       

        if (myWebSite == "" || myWebSite == null) {
          myActions.MessageBoxShow("Could not find an open visual studio for this type of file");
        } else {
         
          myActions.ActivateWindowByTitle(myWebSite, 3);

          // myActions.MessageBoxShow("just activated vs");
          myActions.TypeText("{ESC}", 2000);
          myActions.TypeText("^(;)", 1000);
          myActions.TypeText(strFileNameOnly, 1500);
          myActions.TypeText("{ENTER}", 1500);
          //myActions.TypeText("^(g)", 1000);
          //myActions.TypeText(strLineNumber, 500);
          //myActions.TypeText("{ENTER}", 500);
        }
      } else {
        myActions.MessageBoxShow("Could not find an open visual studio for this type of file");
      }
      goto myExit;


      myExit:

      //myActions.MessageBoxShow("Script completed");
      Application.Current.Shutdown();
    }
  }
}