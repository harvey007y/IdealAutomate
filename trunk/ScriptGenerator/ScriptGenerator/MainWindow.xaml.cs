using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ScriptGenerator {
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
      if (strWindowTitle.StartsWith("ScriptGenerator")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(1000);
      List<ControlEntity> myListControlEntity = new List<ControlEntity>();

      ControlEntity myControlEntity = new ControlEntity();
      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.Heading;
      myControlEntity.Text = "Script Generator";
      myListControlEntity.Add(myControlEntity.CreateControlEntity());


      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.Label;
      myControlEntity.ID = "myLabelMethods";
      myControlEntity.Text = "Methods";
      myControlEntity.RowNumber = 0;
      myControlEntity.ColumnNumber = 0;
      myListControlEntity.Add(myControlEntity.CreateControlEntity());

      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.Label;
      myControlEntity.ID = "myLabelCashManagement";
      myControlEntity.Text = "More Methods";
      myControlEntity.RowNumber = 0;
      myControlEntity.ColumnNumber = 1;
      myListControlEntity.Add(myControlEntity.CreateControlEntity());

    

      SqlConnection con = new SqlConnection("Server=(local)\\SQLEXPRESS;Initial Catalog=IdealAutomateDB;Integrated Security=SSPI");
     
      SqlCommand cmd = new SqlCommand();

      cmd.CommandText = "SELECT Method FROM Methods order by Method";
      cmd.Connection = con;
      int intCol = 0;
      int intRow = 0;

      try {
        con.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        //(CommandBehavior.SingleRow)
        while (reader.Read()) {
          intRow++;
          if (intRow > 20) {
            intRow = 1;
            intCol++;
          }
          string strMethodName = reader.GetString(0);
          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.Button;
          myControlEntity.ID = "myButton" + strMethodName;
          myControlEntity.Text = strMethodName;
          myControlEntity.RowNumber = intRow;
          myControlEntity.ColumnNumber = intCol;
      //    myControlEntity.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
       //   myControlEntity.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
          myListControlEntity.Add(myControlEntity.CreateControlEntity());
        }
        reader.Close();
      } finally {
        con.Close();
      }

     

     
      string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 100, 850);
      DisplayWindowAgain:

      if (strButtonPressed == "btnCancel") {
        // myActions.MessageBoxShow(strButtonPressed);
        goto myExit;
      }
      if (strButtonPressed == "btnOkay") {
        //  myActions.MessageBoxShow(strButtonPressed);
        goto myExit;
      }


        //myActions.TypeText("%(d)", 1500); // select address bar
        //myActions.TypeText("{ESC}", 1500);
        //myActions.TypeText("%({ENTER})", 1500); // Alt enter while in address bar opens new tab

        string strFilePath = "";
        switch (strButtonPressed) {
          case "myButtonTypeText":
            List<ControlEntity> myListControlEntity1 = new List<ControlEntity>();

            ControlEntity myControlEntity1 = new ControlEntity();
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Heading;
            myControlEntity1.Text = "Script Generator";
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblTextToType";
            myControlEntity1.Text = "Text to Type:";
            myControlEntity1.RowNumber = 0;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

          myControlEntity1.ControlEntitySetDefaults();
          myControlEntity1.ControlType = ControlType.TextBox;
          myControlEntity1.ID = "txtTextToType";
          myControlEntity1.Text = "";
          myControlEntity1.RowNumber = 0;
          myControlEntity1.ColumnNumber = 1;
          myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

          myControlEntity1.ControlEntitySetDefaults();
          myControlEntity1.ControlType = ControlType.Label;
          myControlEntity1.ID = "lblMilliSecondsToWait";
          myControlEntity1.Text = "Milliseconds to Wait:";
          myControlEntity1.RowNumber = 1;
          myControlEntity1.ColumnNumber = 0;
          myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

          string strDefaultMilliseconds = myActions.GetValueByKey("ScriptGeneratorDefaultMilliseconds", "IdealAutomateDB");
          myControlEntity1.ControlEntitySetDefaults();
          myControlEntity1.ControlType = ControlType.TextBox;
          myControlEntity1.ID = "txtMillisecondsToWait";
          myControlEntity1.Text = strDefaultMilliseconds;
          myControlEntity1.RowNumber = 1;
          myControlEntity1.ColumnNumber = 1;
          myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

          
          myControlEntity1.ControlEntitySetDefaults();
          myControlEntity1.ControlType = ControlType.CheckBox;
          myControlEntity1.ID = "chkVariable";
          myControlEntity1.Text = "Is this a variable?";
          myControlEntity1.RowNumber = 2;
          myControlEntity1.ColumnNumber = 1;
          if (myActions.GetValueByKey("ScriptGeneratorVariable", "IdealAutomateDB") == "True") {
            myControlEntity1.Checked = true;
          } else {
            myControlEntity1.Checked = false;
          }
          myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

          myControlEntity1.ControlEntitySetDefaults();
          myControlEntity1.ControlType = ControlType.CheckBox;
          myControlEntity1.ID = "chkCtrlKey";
          myControlEntity1.Text = "Is Control Key Pressed?";
          myControlEntity1.RowNumber = 3;
          myControlEntity1.ColumnNumber = 1;
          if (myActions.GetValueByKey("ScriptGeneratorCtrlKey", "IdealAutomateDB") == "True") {
            myControlEntity1.Checked = true;
          } else {
            myControlEntity1.Checked = false;
          }
          myListControlEntity1.Add(myControlEntity1.CreateControlEntity());



          strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 500, 100, 850);         

            if (strButtonPressed == "btnCancel") {
              strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 100, 850);
              goto DisplayWindowAgain;
            }
            //if (strButtonPressed == "btnOkay") {
            //  strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 100, 850);
            //  goto DisplayWindowAgain;
            //}
          string strTextToType = myListControlEntity1.Find(x => x.ID == "txtTextToType").Text;
          string strMillisecondsToWait = myListControlEntity1.Find(x => x.ID == "txtMillisecondsToWait").Text;
          bool boolVariable = myListControlEntity1.Find(x => x.ID == "chkVariable").Checked;
          bool boolCtrlKey = myListControlEntity1.Find(x => x.ID == "chkCtrlKey").Checked;
          myActions.SetValueByKey("ScriptGeneratorDefaultMilliseconds", strMillisecondsToWait, "IdealAutomateDB");
          myActions.SetValueByKey("ScriptGeneratorVariable", boolVariable.ToString(), "IdealAutomateDB");
          myActions.SetValueByKey("ScriptGeneratorCtrlKey", boolCtrlKey.ToString(), "IdealAutomateDB");
          string strGeneratedLine = "";
          //   string strType = myListControlEntity1.Find(x => x.ID == "cbxType").SelectedValue;
          if (!boolVariable && !boolCtrlKey) {
            strGeneratedLine = "myActions.TypeText(\"" + strTextToType + "\"," + strMillisecondsToWait + ");";
            myActions.PutEntityInClipboard(strGeneratedLine);
            myActions.MessageBoxShow(strGeneratedLine);
            
          }
          if (boolVariable && !boolCtrlKey) {
            strGeneratedLine = "myActions.TypeText(" + strTextToType + "," + strMillisecondsToWait + ");";
            myActions.PutEntityInClipboard(strGeneratedLine);
            myActions.MessageBoxShow(strGeneratedLine);
          } 
          if (boolCtrlKey && !boolVariable) {
            strGeneratedLine = "myActions.TypeText(\"^(" + strTextToType + ")\"," + strMillisecondsToWait + ");";
            myActions.PutEntityInClipboard(strGeneratedLine);
            myActions.MessageBoxShow(strGeneratedLine);
          }
          if (boolCtrlKey && boolVariable) {
            myActions.MessageBoxShow("Control Key and Variable is not valid");
          }
          strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 100, 850);
          goto DisplayWindowAgain;
          break;
          case "myButtonWorksheetList":
            strFilePath = "ASPX/Treasury/WorksheetList.aspx";
            break;
          case "myButtonForecastRulesList":
            strFilePath = "ASPX/Treasury/ForecastRulesList.aspx";
            break;
          case "myButtonBalanceExplorer":
            strFilePath = "bal/balSearch.asp";
            break;
          case "myButtonIntradayMatching":
            strFilePath = "ASPX/Reconcilement/IntradayMatching.aspx";
            break;
          case "myButtonTARuleList":
            strFilePath = "ta/TaRuleAcctList.asp";
            break;
          case "myButtonFTWorkflow":
            strFilePath = "ft/ftexplorer.asp";
            break;
          case "myButtonFTSearch":
            strFilePath = "ft/ftfindftrn.asp";
            break;
          case "myButtonTemplateModelWorkflow":
            strFilePath = "models/mdstatuscontrol.asp";
            break;
          case "myButtonFTListNew":
            strFilePath = "ASPX/FT/Trn/FtrnList.aspx";
            break;
          case "myButtonFTTestTransactionList":
            strFilePath = "ASPX/FT/Trn/FtrnTestList.aspx";
            break;
          case "myButtonSetupFundsTransferTypesInstrType":
            strFilePath = "ASPX/FT/Maint/FTInstTypeList.aspx";
            break;
          case "myButtonCreateTemplateModel":
            strFilePath = "models/mdcreate.asp";
            break;
          case "myButtonTemplateModelSearch":
            strFilePath = "models/mdexplorer.asp";
            break;
          case "myButtonJournalEntryList":
            strFilePath = "ASPX/gl/glJournalList.aspx";
            break;
          case "myButtonChartofAccounts":
            strFilePath = "ASPX/GL/GLLedgerList.aspx";
            break;
          case "myButtonGLTransactionList":
            strFilePath = "ASPX/GL/GLTransactionList.aspx";
            break;
          case "myButtonTARulesList2":
            strFilePath = "ta/TaRuleAcctList.asp";
            break;
          case "myButtonBankExplorer":
            strFilePath = "ASPX/AdvMaint/Bank/BankExplorer.aspx";
            break;
          case "myButtonBankList":
            strFilePath = "ASPX/AdvMaint/Bank/bankList.aspx";
            break;
          case "myButtonSystemBankList":
            strFilePath = "ASPX/AdvMaint/SystemBank/systemBankList.aspx";
            break;
          case "myButtonAccountWorkflow":
            strFilePath = "ASPX/AdvMaint/Account/AccountStatus.aspx";
            break;
          case "myButtonAccountExplorer":
            strFilePath = "ASPX/AdvMaint/Account/maintAccountExplorer.aspx";
            break;
          case "myButtonAccountList":
            strFilePath = "ASPX/AdvMaint/Account/accountList.aspx";
            break;
          case "myButtonUserCodeExplorer":
            strFilePath = "ASPX/Maintenance/UserCode/maintUserCodeExplorer.aspx";
            break;
          case "myButtonUserCodeList":
            strFilePath = "ASPX/Maintenance/UserCode/maintUserCodeList.aspx";
            break;
          case "myButtonSystemBankCodes":
            strFilePath = "ASPX/Maintenance/SystemBankCode/maintSysBankCodeList.aspx";
            break;
          case "myButtonJobsPluginOptions":
            strFilePath = "ASPX/Ix/PluginOption/maintPluginOptionList.aspx";
            break;
          case "myButtonEnotificationEventTypes":
            strFilePath = "ASPX/eNotify/eNotifyEventMainList.aspx";
            break;
          case "myButtonCommunicationTemplates":
            strFilePath = "ASPX/Documents/CommunicationTemplateList.aspx";
            break;
          case "myButtonGeneralReferenceTypes":
            strFilePath = "ASPX/AdvMaint/ReferenceType/maintReferenceTypeList.aspx";
            break;
          case "myButtonAccessGroupExplorer":
            strFilePath = "ASPX/AdvMaint/Oper/operAccessGroupExplorer.aspx";
            break;
          case "myButtonOperatorList":
            strFilePath = "ASPX/AdvMaint/Oper/operList.aspx";
            break;
          case "myButtonChangePassword":
            strFilePath = "ASPX/Utilities/ChangePassword.aspx";
            break;
          case "myButtonSystemOptions":
            strFilePath = "ASPX/Maintenance/SystemOptions/maintSystemOptionsList.aspx";
            break;
          case "myButtonSystemLocks":
            strFilePath = "ASPX/admin/maintSignOutList.aspx";
            break;

          default:
            strFilePath = "FT/Trn/FtrnList.aspx";
            break;
        }

        string strCurrentURL = myActions.PutClipboardInEntity();
        // http://localhost/gt/aspx/main/login.aspx          
        string strUrlBase = "http://localhost/gt/ASPX/";
        if (strCurrentURL.Contains("://localhost/gt/")) {
          strUrlBase = "http://localhost/gt/";
        }
        if (strCurrentURL.Contains("://devserver11/gtreasury")) {
          strUrlBase = "http://devserver11/gtreasury/";
        }
        if (strCurrentURL.Contains("://devserver12/gtreasury")) {
          strUrlBase = "http://devserver12/gtreasury/";
        }
        if (strCurrentURL.Contains("://qaserver6/webcash/")) {
          strUrlBase = "http://qaserver6/webcash/";
        }
        if (strCurrentURL.Contains("://qaserver6a/webcash/")) {
          strUrlBase = "http://qaserver6a/webcash/";
        }
        if (strCurrentURL.Contains("://qaserver6b/webcash/")) {
          strUrlBase = "http://qaserver6b/webcash/";
        }
        if (strCurrentURL.Contains("://bbgqa/gt/")) {
          strUrlBase = "http://bbgqa/gt/";
        }
        if (strCurrentURL.Contains("://bbgqa/gt/")) {
          strUrlBase = "http://bbgqa/gt/";
        }
        if (strCurrentURL.Contains("https://qaserver6.gtreasuryss.net/webcash/")) {
          strUrlBase = "https://qaserver6.gtreasuryss.net/webcash/";
        }
        if (strCurrentURL.Contains("https://qaserver6a.gtreasuryss.net/webcash/")) {
          strUrlBase = "https://qaserver6a.gtreasuryss.net/webcash/";
        }
        if (strCurrentURL.Contains("https://qaserver6.gtreasuryss.net/gtreasury/")) {
          strUrlBase = "https://qaserver6.gtreasuryss.net/gtreasury/";
        }
        if (strCurrentURL.Contains("https://qaserver6a.gtreasuryss.net/gtreasury/")) {
          strUrlBase = "https://qaserver6a.gtreasuryss.net/gtreasury/";
        }
        if (strCurrentURL.Contains("https://prodcopy.gtreasuryss.net/gtreasury/")) {
          strUrlBase = "https://prodcopy.gtreasuryss.net/gtreasury/";
        }

        string strJavascriptGoURL = "";

        if (strCurrentURL.EndsWith("asp")) {
          strFilePath = strUrlBase + strFilePath;
          // first GoUrl has lower case "rl" because we are on classic asp page
          strJavascriptGoURL = "xjavascript:GoUrl('" + strFilePath + "');";
        } else {
          strFilePath = strUrlBase + strFilePath;
          strJavascriptGoURL = "xjavascript:GoURL('" + strFilePath + "');";
        }
        myActions.PutEntityInClipboard(strJavascriptGoURL);
        myActions.SelectAllPaste(500);
        myActions.TypeText("{HOME}", 500);
        myActions.TypeText("{DELETE}", 500);
        myActions.TypeText("{ENTER}", 500);

        myActions.Sleep(500);
    
      strButtonPressed = myActions.WindowMultipleControlsMinimized(ref myListControlEntity, 500, 1600, 500, 0);
      goto DisplayWindowAgain;

      myExit:
      Application.Current.Shutdown();
    }
  }
}