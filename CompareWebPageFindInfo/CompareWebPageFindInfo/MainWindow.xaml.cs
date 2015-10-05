using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using IdealAutomate.Core;
using IdealAutomateCore;

namespace CompareWebPageFindInfo {
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
      myActions.DebugMode = true;
      // Alternative is SqlServer2014
      string strServer = myActions.GetValueByKey("SqlServer2008", "IdealAutomateDB");
      // Alternative is QA6DB
      string strDatabaseName = myActions.GetValueByKey("ChristysDB", "IdealAutomateDB");
      string strDirectoryPath = @"c:\Data\Results";

      string strWindowTitle = myActions.PutWindowTitleInEntity();
      if (strWindowTitle.StartsWith("CompareWebPageFindInfo")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(1000);
      string strPageTitle = "";
      //comment out next line after testing
      //   goto jumpstart;
      // comment out  the following 2 lines when done testing !!!!!
      //string strPageTitle = @"http://localhost/webcash/ASPX/Reconcilement/ManualIntradayMatch.aspx";
      //goto jumpstart;

      myActions.TypeText("{UP}", 1000);
      myActions.TypeText("%(v)", 1000);
      myActions.TypeText("c", 1000);
      //myActions.TypeText("+({F10})", 1000);
      // myActions.TypeText("v", 1000);

      // Type Control A to highlight everything in notepad
      myActions.TypeText("^(a)", 1000);
      // Type Control V to paste clipboard into notepad
      myActions.TypeText("^(c)", 1000);
      myActions.TypeText("{DOWN}", 1000);

      strPageTitle = myActions.PutWindowTitleInEntity();
      strPageTitle = strPageTitle.Replace(" - Original Source", "");
      myActions.TypeText("%fc", 900); // close view source window
      // Open Notepad
      myActions.Run(@"C:\Windows\system32\notepad.exe", @"C:\Data\GLList.html");
      // Type Control A to highlight everything in notepad
      myActions.TypeText("^(a)", 1000);
      // Type Control V to paste clipboard into notepad
      myActions.TypeText("^(v)", 1000);
      //Go to Top of Notepad
      myActions.TypeText("^({HOME})", 1000);
      myActions.TypeText("{ENTER}", 1000);
      myActions.TypeText("{UP}", 1000);
      myActions.TypeText("<This is the page title>" + strPageTitle + "</", 1000);


      // save notepad alt f s
      myActions.TypeText("%fs", 900);
      // close notepad alt f x
      myActions.TypeText("%fx", 900);
      // Run UpdateATextFile
      myActions.Run(@"C:\TFS\WadeHome\Applications\UpdateATextFile\UpdateATextFile\bin\Debug\UpdateATextFile.exe", "CompareWebPageFindInfo");
      // Bring Visual Studio to the foreground
      for (int i = 0; i < 10; i++) {
        if (myActions.PutWindowTitleInEntity() == "All_Fields.txt - Notepad") {
          // close notepad alt f x
          myActions.TypeText("%fx", 900);
          break;
        }
        myActions.Sleep(500);
      }





      // myExit:


      Application.Current.Shutdown();
    }


  }
}
