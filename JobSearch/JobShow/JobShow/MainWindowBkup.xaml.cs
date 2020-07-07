using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace JobShow {
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
      if (strWindowTitle.StartsWith("JobShow")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(1000);
            string myUrl = "url";
                string myKeyword = "keyword"; 
                string myLocation = "location";
            SqlConnection con = new SqlConnection("Server=(local)\\SQLEXPRESS02;Initial Catalog=IdealAutomateDB;Integrated Security=SSPI");
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT JobUrl, Keyword, Location FROM JobApplications";


            cmd.Connection = con;
            string buttonPressed = "";
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                //(CommandBehavior.SingleRow)
                while (reader.Read())
                {
                    myUrl = reader.GetString(0);
                    if (!reader.IsDBNull(1))
                    {
                        myKeyword = reader.GetString(1);
                    } else
                    {
                        myKeyword = "";
                    }
                    if (!reader.IsDBNull(2))
                    {
                        myLocation = reader.GetString(2);
                    }
                    else
                    {
                        myLocation = "";
                    }
                    DisplayAUrl(myActions, myUrl, myKeyword, myLocation, ref buttonPressed);
                    if (buttonPressed == "Cancel")
                    {
                        break;
                    }
                    myActions.Run(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", myUrl);
                    myActions.Sleep(1000);
                }
                reader.Close();
            }
            finally
            {
                con.Close();
            }

           



     

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
      string officePath = myActions.GetValueByKeyGlobalRespondWithDialogIfEmpty("OfficePath");
      myActions.Run(System.IO.Path.Combine(officePath, "EXCEL.EXE"), @"C:\SVNStats\SVNStats.xlsx");
      myExit:
      myActions.ScriptEndedSuccessfullyUpdateStats();
      Application.Current.Shutdown();
    }
        void DisplayAUrl(Methods myActions, string myUrl, string myKeyword, string myLocation, ref string buttonPressed)
        {
            int intRowCtr = 0;
            ControlEntity myControlEntity = new ControlEntity();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.ID = "lbl";
            myControlEntity.Text = "Job Found";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblUrl";
            myControlEntity.Text = "Url";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtUrl";
            myControlEntity.Text = myUrl;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblKeyword";
            myControlEntity.Text = "Keyword";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtKeyword";
            myControlEntity.Text = myKeyword;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblLocation";
            myControlEntity.Text = "Location";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtLocation";
            string[] locationParts;
            if (myLocation.Contains("|"))
            {
                locationParts = myLocation.Split('|');
                myControlEntity.Text = locationParts[0];
            }
            else
            {
                myControlEntity.Text = myLocation;
            }
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnSkip";
            myControlEntity.Text = "Skip";
            myControlEntity.ColumnSpan = 0;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 800, 0, 0);
            if (strButtonPressed == "btnCancel")
            {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                buttonPressed = "Cancel";
                return;
            }

            string strUrl = myListControlEntity.Find(x => x.ID == "txtUrl").Text;
            string strKeyword = myListControlEntity.Find(x => x.ID == "txtKeyword").Text;
            string strLocation = myListControlEntity.Find(x => x.ID == "txtLocation").Text;
        }
  }
}
