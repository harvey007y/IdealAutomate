using System.Windows;
using IdealAutomate.Core;
using System.Data.SqlClient;
using System;

namespace CompareWebPages {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    Methods myActions;
    ImageEntity myImage;
    bool boolRunningFromHome = false;
    int[,] myArray2;
    public MainWindow() {



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
      myActions = new Methods();

      myActions.DebugMode = true;
      myImage = new ImageEntity();

      string strWindowTitle = myActions.PutWindowTitleInEntity();
      if (strWindowTitle.StartsWith("CompareWebPages")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(1000);
      //* TODO: Instead of logging into website, I should be looping thru scripts that login website and take me to specific page
      // that I want to compare - I could have a table of login scripts that I could loop thru to go to specific pages and specific
      // websites. When I get to the page, I run CompareWebPageFindInfo.



      // we have made it to login page
      SqlConnection con = new SqlConnection("Server=(local)\\SQLEXPRESS;Initial Catalog=WadeTest2;Integrated Security=SSPI");
      SqlConnection con2 = new SqlConnection("Server=(local)\\SQLEXPRESS;Initial Catalog=IdealAutomateDB;Integrated Security=SSPI");
      SqlCommand cmd = new SqlCommand();      
      cmd.CommandText = "SELECT ScriptPath FROM [dbo].[LoginScriptPaths] where Enabled = 1";
      cmd.Connection = con;
      SqlCommand cmd2 = new SqlCommand();
      cmd2.CommandText = "SELECT Count(*) FROM [IdealAutomateDB].[dbo].[CompareWebPagesPageNames] ";
      cmd2.Connection = con2;
      SqlCommand cmd3 = new SqlCommand();
      cmd3.CommandText = "SELECT PageName FROM [IdealAutomateDB].[dbo].[CompareWebPagesPageNames] where IdealActual = 'Ideal' ";
      cmd3.Connection = con2;
      SqlCommand cmd4 = new SqlCommand();
      cmd4.CommandText = "SELECT PageName FROM [IdealAutomateDB].[dbo].[CompareWebPagesPageNames] where IdealActual = 'Actual' ";
      cmd4.Connection = con2;
      string strFilePath = "";
      try {
        con.Open();
        con2.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        //(CommandBehavior.SingleRow)
        while (reader.Read()) {
          Console.WriteLine(reader.GetString(0));
          strFilePath = reader.GetString(0);
          myActions.RunSync(strFilePath, "");
          if (boolRunningFromHome == true) {
            myImage.ImageFile = "Images\\Home.PNG";
          } else {
            myImage.ImageFile = "Images\\Home_Work.PNG";
          }
          myImage.Sleep = 500;
          myImage.Attempts = 10;
          myImage.RelativeX = 10;
          myImage.Tolerance = 74;
          myArray2 = myActions.PutAll(myImage);
          if (myArray2.Length == 0) {
            myActions.MessageBoxShow("I could not find Home hyperlink (CompareWebPages)");
          }
          myActions.RunSync(@"C:\SVNIA\trunk\CompareWebPageFindInfo\CompareWebPageFindInfo\bin\Debug\CompareWebPageFindInfo.exe", "");
          int intRecordCount = (int)cmd2.ExecuteScalar();
          if (intRecordCount == 2) {
            string strIdealPageName = (string)cmd3.ExecuteScalar();
            string strActualPageName = (string)cmd4.ExecuteScalar();
            UseSSMSToRunQueryToCompareWebPages(strIdealPageName, strActualPageName);
          }
        }
        reader.Close();
      } finally {
        con.Close();
        con2.Close();
      }







      myActions.MessageBoxShow("Script completed");
      myActions.Sleep(3000);
      Application.Current.Shutdown();
    }

    private void UseSSMSToRunQueryToCompareWebPages(string strIdealPageName, string strActualPageName) {
      // Alternatives are: SqlServer2008, SqlServer2014
      string strServer = ".\\SQLEXPRESS";
      // Alternatives are: ChristysDB, QA6DB
      string strDatabaseName = "IdealAutomateDB";
      bool boolWindowFound = false;
      string myBigSqlString =
        "declare @myIdealPage varchar(500) " +
"declare @myActualPage varchar(500) " +
"declare @myQuery varchar (500) " +
"set @myIdealPage = '" + strIdealPageName + "' " +
"set @myActualPage = '" + strActualPageName + "' " +
"select 'In ideal but not actual in ' + @myIdealPage + ' that are not in ' + @myActualPage " +
"set @myQuery = 'select UntranslatedText from dbo.UntranslatedPageText where PageName =  '''  + @myIdealPage + '''' + '  " +
"and unTranslatedText not in (select UntranslatedText from dbo.UntranslatedPageText where PageName =  '''  + @myActualPage + '''' + ')' + 'order by UntranslatedText' " +
"exec(@myQuery) " +
"select 'In actual but not ideal in ' + @myActualPage + ' that are not in ' + @myIdealPage " +
"set @myQuery = 'select UntranslatedText from dbo.UntranslatedPageText where PageName =  '''  + @myActualPage + '''' + ' " +
"and unTranslatedText not in (select UntranslatedText from dbo.UntranslatedPageText where PageName =  '''  + @myIdealPage + '''' + ')' + 'order by UntranslatedText' " +
"exec(@myQuery) ";

      boolWindowFound = myActions.ActivateWindowByTitle("Microsoft SQL Server Management Studio");
      // If Sql Server Profiler not running, we have a problem
      if (boolWindowFound == false) {
        myActions.Run(@"C:\Program Files (x86)\Microsoft SQL Server\100\Tools\Binn\VSShell\Common7\IDE\SSMS.exe", "-S " + strServer + " -D " + strDatabaseName);
        myActions.Sleep(15000);
      }
      for (int i = 0; i < 10; i++) {
        if (myActions.PutWindowTitleInEntity() == "Microsoft SQL Server Management Studio") {
          // close notepad alt f x
          break;
        }
        myActions.Sleep(500);
      }
      myActions.Sleep(2000);
      myActions.TypeText("%(\" \")x", 1000); // maximize SSMS
      myActions.TypeText("%(f)e", 1000); // file connection explorer
      myActions.TypeText("{DEL}", 500); // file connection explorer
      myActions.TypeText(strServer, 500); // file connection explorer
      myActions.TypeText("{ENTER}", 1000); // select the server
      myActions.TypeText("{DOWN}", 500); // select databases
      // {NUMPADMULT}
      myActions.TypeText("{NUMPADADD}", 1000); // expand databases
      myActions.TypeText("%({F8})", 1000); // open server in object explorer
      myActions.TypeText(strDatabaseName, 1000); // select the database
      myActions.TypeText("{ENTER}", 1000); // open new query window
      myActions.TypeText("^(n)", 1000); // open new query window
      myActions.PutEntityInClipboard(myBigSqlString);
      myActions.TypeText("^(v)", 1000); // put query in window
      //  myActions.TypeText("{F5}", 1000); // run the query
      myActions.TypeText("^(k)", 1000); // put query in window
      myActions.TypeText("^(f)", 1000); // put query in window
    }

    private void ProcessAPage(string strFilePath) {
      string strCurrentWindowTitle = "";
      string strJavascriptGoURL = "";
      myActions.Sleep(1000);
      myActions.TypeText("%(d)", 500); // select the address bar
      myActions.SelectAllCopy(500);
      string strCurrentURL = myActions.PutClipboardInEntity();
      if (strCurrentURL.EndsWith("asp")) {
        // first GoUrl has lower case "rl" because we are on classic asp page
        strJavascriptGoURL = "xjavascript:GoUrl('http://localhost:80/gt/" + strFilePath + "');";
      } else {
        strJavascriptGoURL = "xjavascript:GoURL('http://localhost:80/gt/" + strFilePath + "');";
      }
      myActions.PutEntityInClipboard(strJavascriptGoURL);
      myActions.SelectAllPaste(1500);
      myActions.TypeText("{HOME}", 500);
      myActions.TypeText("{DELETE}", 500);
      myActions.TypeText("{ENTER}", 500);
      myActions.Sleep(5000);
      // myActions.MessageBoxShow("Script completed");
      strCurrentWindowTitle = myActions.PutWindowTitleInEntity();
      //     myActions.RunSync(@"C:\SVNIA\trunk\FindUntranslated\FindUntranslated\bin\Debug\FindUntranslated.exe", "");

      // do create page **********************
      myActions.ActivateWindowByTitle(strCurrentWindowTitle);
      myActions.Sleep(1500);
      myActions.TypeText("%(\" \")", 500); // maximize internet explorer
      myActions.TypeText("x", 500);
      myActions.Sleep(1000);
      if (strFilePath.Contains("List") == false) {
        return;
      }
      myImage = new ImageEntity();
      if (boolRunningFromHome == true) {
        myImage.ImageFile = "Images\\CreateNewHome.PNG";
      } else {
        myImage.ImageFile = "Images\\CreateNew.PNG";
      }
      myImage.Sleep = 500;
      myImage.Attempts = 1;
      myImage.RelativeX = 10;
      myImage.Tolerance = 55;
      myImage.UseGrayScale = true;
      myArray2 = myActions.PutAll(myImage);
      if (myArray2.Length == 0) {
        System.Windows.Forms.DialogResult _dialogResult = myActions.MessageBoxShowWithYesNo("I could not find CreateNew.PNG (CompareWebPages); If you can find it, click it and then click yes; else just click no");
        if (_dialogResult == System.Windows.Forms.DialogResult.Yes) {
          myActions.Sleep(2000);
          // myActions.MessageBoxShow("Script completed");
          strCurrentWindowTitle = myActions.PutWindowTitleInEntity();
          //  myActions.RunSync(@"C:\SVNIA\trunk\FindUntranslated\FindUntranslated\bin\Debug\FindUntranslated.exe", "");
          myActions.ActivateWindowByTitle(strCurrentWindowTitle);
          return;
        } else {
          return; // get next page
        }
      }
      myActions.LeftClick(myArray2);
      myActions.Sleep(2000);
      // myActions.MessageBoxShow("Script completed");
      strCurrentWindowTitle = myActions.PutWindowTitleInEntity();
      //  myActions.RunSync(@"C:\SVNIA\trunk\FindUntranslated\FindUntranslated\bin\Debug\FindUntranslated.exe", "");
      myActions.ActivateWindowByTitle(strCurrentWindowTitle);
      // end do create page ******************
    }
  }
}
