using System.Windows;
using IdealAutomate.Core;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace _10175_LoginFTTestPay {
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
      ImageEntity myImage = new ImageEntity();
      int[,] myArray;

      string strWindowTitle = myActions.PutWindowTitleInEntity();
      //      if (strWindowTitle.StartsWith("localhostTimesheets")) {
      //        myActions.TypeText("%(f)", 1000); // minimize visual studio
      //        myActions.TypeText("x", 1000); // minimize visual studio
      //      }
      List<string> myWindowTitles = myActions.GetWindowTitlesByProcessName("iexplore");
      bool boolLocalhostFound = false;
      bool boolInternetExplorerFound = false;
      foreach (var myWindowTitle in myWindowTitles) {
        boolInternetExplorerFound = true;
        if (myWindowTitle != "") {
          myActions.ActivateWindowByTitle(myWindowTitle);
          // go to last tab;
          // myActions.TypeText("^(9)",2500);
          myActions.Sleep(1500);
          myActions.TypeText("{F6}", 1500);
          string strFirstTabURL = myActions.SelectAllCopyIntoEntity(1500);

          if (strFirstTabURL.ToLower().Contains("localhost")) {
            boolLocalhostFound = true;
            break;
          }
          string strCurrentTabURL = "";
          // go to next tab
          while (strCurrentTabURL != strFirstTabURL) {
            myActions.TypeText("^({TAB})", 1500);
            // window title does not contain tab title - looks like I need methods
            // to: get title from ie page and get string between two delimiters -
            // I think I did the second one in update a text file - can I 
            // make it a string extension method? yes
            myActions.TypeText("{F6}", 1500);
            strCurrentTabURL = myActions.SelectAllCopyIntoEntity(1500);
            if (strCurrentTabURL.ToLower().Contains("localhost")) {
              boolLocalhostFound = true;
              break;
            }
          }
          if (boolLocalhostFound) {
            break;
          }

        }
      }
      if (boolLocalhostFound == false) {
        // if lstWindowTitles.Count == 0, run ie and login to localhost, else open
        // localhost in new tab
        if (boolInternetExplorerFound) {
          myActions.RunSync(@"C:\SVNIA\trunk\10175LoginTab\10175LoginTab\bin\Debug\10175LoginTab.exe", "");

        } else {
          myActions.RunSync(@"C:\SVNIA\trunk\10175Login\10175Login\bin\Debug\10175Login.exe", "");
        }
      } else {
        myActions.TypeText("%(\" \")", 500);
        myActions.TypeText("x", 500);
      }
      myActions.Sleep(1000);
      // Run 10175Login.exe
     
      if (boolRunningFromHome == true) {
        myImage.ImageFile = "Images\\Home.PNG";
      } else {
        myImage.ImageFile = "Images\\Home_Work.PNG";
      }
      myImage.Sleep = 500;
      myImage.Attempts = 10;
      myImage.RelativeX = 10;
      myImage.Tolerance = 79;
      myArray2 = myActions.PutAll(myImage);
      if (myArray2.Length == 0) {
        myActions.MessageBoxShow("I could not find Home hyperlink (10175_LoginFTTestPay)");
      }
      //// we have made it to login page
      //SqlConnection con = new SqlConnection("Server=(local)\\SQLEXPRESS;Initial Catalog=WadeTest2;Integrated Security=SSPI");
      //SqlCommand cmd = new SqlCommand();

      //cmd.CommandText = "SELECT FilePath FROM [dbo].[Menu_Paths] where filepath is not null and filepath not like 'javascript%'";
      //cmd.Connection = con;
      //string strFilePath = "";
      //try {
      //  con.Open();
      //  SqlDataReader reader = cmd.ExecuteReader();
      //  //(CommandBehavior.SingleRow)
      //  while (reader.Read()) {
      //    Console.WriteLine(reader.GetString(0));
      //    strFilePath = reader.GetString(0);
      ProcessAPage("ASPX/FT/Trn/FtrnTestList.aspx");
      //  }
      //  reader.Close();
      //} finally {
      //  con.Close();
      //}







      myActions.MessageBoxShow("Script completed");
      myActions.Sleep(3000);
      Application.Current.Shutdown();
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
      myActions.SelectAllPaste(500);
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
     
      myActions.Sleep(2000);
      myActions.TypeText("+({TAB 4})", 200);
      myActions.TypeText("01/01/2014", 200);
      myActions.TypeText("{TAB 6}", 200);
      myActions.TypeText("{ENTER}", 200);
      // myActions.MessageBoxShow("Script completed");
    //  strCurrentWindowTitle = myActions.PutWindowTitleInEntity();
      //  myActions.RunSync(@"C:\SVNIA\trunk\FindUntranslated\FindUntranslated\bin\Debug\FindUntranslated.exe", "");
   //   myActions.ActivateWindowByTitle(strCurrentWindowTitle);
      // end do create page ******************
    }
    private string GetTabTitle(IdealAutomate.Core.Methods myActions) {
      myActions.TypeText("{UP}", 1000);
      myActions.TypeText("%(v)", 1000);
      myActions.TypeText("c", 1000);
      //myActions.TypeText("+({F10})", 1000);
      // myActions.TypeText("v", 1000);



      string strPageTitle = myActions.PutWindowTitleInEntity();
      strPageTitle = strPageTitle.Replace(" - Original Source", "");
      myActions.CloseApplicationAltFc(500);
      return strPageTitle;
    }
  }
}
