using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using DataGridFilterTest;
using System.Windows.Controls;

namespace SMSParameters {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class SMSPage : Page {
    public SMSPage() {
      bool boolRunningFromHome = false;
//      var window = new Window() //make sure the window is invisible
//{
//        Width = 0,
//        Height = 0,
//        Left = -2000,
//        WindowStyle = WindowStyle.None,
//        ShowInTaskbar = false,
//        ShowActivated = false,
//      };
//      window.Show();
      IdealAutomate.Core.Methods myActions = new Methods();
      myActions.ScriptStartedUpdateStats();

      InitializeComponent();
  //    this.Hide();

      string strWindowTitle = myActions.PutWindowTitleInEntity();
      if (strWindowTitle.StartsWith("SMSParameters")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(1000);
            int intRowCtr = 0;
            ControlEntity myControlEntity = new ControlEntity();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.ID = "lbl";
            myControlEntity.Text = "SMS Parameters";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblUserId";
            myControlEntity.Text = "UserId";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtUserId";
            myControlEntity.Text = myActions.GetValueByKey("UserId"); ;
            myControlEntity.ToolTipx = "UserId or email address for email acct";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblPassword";
            myControlEntity.Text = "Password";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtPassword";
            myControlEntity.Text = myActions.GetValueByKey("Password"); ;
            myControlEntity.ToolTipx = "Password for email acct";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblClientHost";
            myControlEntity.Text = "ClientHost";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtClientHost";
            myControlEntity.Text = myActions.GetValueByKey("ClientHost"); ;
            myControlEntity.ToolTipx = "ClientHost for email acct - example smtp.gmail.com";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblcarriersmsemailsuffix";
            myControlEntity.Text = "carrier sms email suffix";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.ComboBox;
            cbp.Clear();
            cbp.Add(new ComboBoxPair("@messaging.sprintpcs.com", "@messaging.sprintpcs.com"));
            cbp.Add(new ComboBoxPair("@itelemigcelular.com.br", "@itelemigcelular.com.br"));
            cbp.Add(new ComboBoxPair("@message.alltel.com", "@message.alltel.com"));
            cbp.Add(new ComboBoxPair("@message.pioneerenidcellular.com", "@message.pioneerenidcellular.com"));
            cbp.Add(new ComboBoxPair("@messaging.cellone-sf.com", "@messaging.cellone-sf.com"));
            cbp.Add(new ComboBoxPair("@messaging.centurytel.net", "@messaging.centurytel.net"));
            cbp.Add(new ComboBoxPair("@messaging.sprintpcs.com", "@messaging.sprintpcs.com"));
            cbp.Add(new ComboBoxPair("@mobile.att.net", "@mobile.att.net"));
            cbp.Add(new ComboBoxPair("@mobile.cell1se.com", "@mobile.cell1se.com"));
            cbp.Add(new ComboBoxPair("@mobile.celloneusa.com", "@mobile.celloneusa.com"));
            cbp.Add(new ComboBoxPair("@mobile.dobson.net", "@mobile.dobson.net"));
            cbp.Add(new ComboBoxPair("@mobile.mycingular.com", "@mobile.mycingular.com"));
            cbp.Add(new ComboBoxPair("@mobile.mycingular.net", "@mobile.mycingular.net"));
            cbp.Add(new ComboBoxPair("@mobile.surewest.com", "@mobile.surewest.com"));
            cbp.Add(new ComboBoxPair("@msg.acsalaska.com", "@msg.acsalaska.com"));
            cbp.Add(new ComboBoxPair("@msg.clearnet.com", "@msg.clearnet.com"));
            cbp.Add(new ComboBoxPair("@msg.mactel.com", "@msg.mactel.com"));
            cbp.Add(new ComboBoxPair("@msg.myvzw.com", "@msg.myvzw.com"));
            cbp.Add(new ComboBoxPair("@msg.telus.com", "@msg.telus.com"));
            cbp.Add(new ComboBoxPair("@mycellular.com", "@mycellular.com"));
            cbp.Add(new ComboBoxPair("@mycingular.com", "@mycingular.com"));
            cbp.Add(new ComboBoxPair("@mycingular.net", "@mycingular.net"));
            cbp.Add(new ComboBoxPair("@mycingular.textmsg.com", "@mycingular.textmsg.com"));
            cbp.Add(new ComboBoxPair("@o2.net.br", "@o2.net.br"));
            cbp.Add(new ComboBoxPair("@ondefor.com", "@ondefor.com"));
            cbp.Add(new ComboBoxPair("@pcs.rogers.com", "@pcs.rogers.com"));
            cbp.Add(new ComboBoxPair("@personal-net.com.ar", "@personal-net.com.ar"));
            cbp.Add(new ComboBoxPair("@personal.net.py", "@personal.net.py"));
            cbp.Add(new ComboBoxPair("@portafree.com", "@portafree.com"));
            cbp.Add(new ComboBoxPair("@qwest.com", "@qwest.com"));
            cbp.Add(new ComboBoxPair("@qwestmp.com", "@qwestmp.com"));
            cbp.Add(new ComboBoxPair("@sbcemail.com", "@sbcemail.com"));
            cbp.Add(new ComboBoxPair("@sms.bluecell.com", "@sms.bluecell.com"));
            cbp.Add(new ComboBoxPair("@sms.cwjamaica.com", "@sms.cwjamaica.com"));
            cbp.Add(new ComboBoxPair("@sms.edgewireless.com", "@sms.edgewireless.com"));
            cbp.Add(new ComboBoxPair("@sms.hickorytech.com", "@sms.hickorytech.com"));
            cbp.Add(new ComboBoxPair("@sms.net.nz", "@sms.net.nz"));
            cbp.Add(new ComboBoxPair("@sms.pscel.com", "@sms.pscel.com"));
            cbp.Add(new ComboBoxPair("@smsc.vzpacifica.net", "@smsc.vzpacifica.net"));
            cbp.Add(new ComboBoxPair("@speedmemo.com", "@speedmemo.com"));
            cbp.Add(new ComboBoxPair("@suncom1.com", "@suncom1.com"));
            cbp.Add(new ComboBoxPair("@sungram.com", "@sungram.com"));
            cbp.Add(new ComboBoxPair("@telesurf.com.py", "@telesurf.com.py"));
            cbp.Add(new ComboBoxPair("@teletexto.rcp.net.pe", "@teletexto.rcp.net.pe"));
            cbp.Add(new ComboBoxPair("@text.houstoncellular.net", "@text.houstoncellular.net"));
            cbp.Add(new ComboBoxPair("@text.telus.com", "@text.telus.com"));
            cbp.Add(new ComboBoxPair("@timnet.com", "@timnet.com"));
            cbp.Add(new ComboBoxPair("@timnet.com.br", "@timnet.com.br"));
            cbp.Add(new ComboBoxPair("@tms.suncom.com", "@tms.suncom.com"));
            cbp.Add(new ComboBoxPair("@tmomail.net", "@tmomail.net"));
            cbp.Add(new ComboBoxPair("@tsttmobile.co.tt", "@tsttmobile.co.tt"));
            cbp.Add(new ComboBoxPair("@txt.bellmobility.ca", "@txt.bellmobility.ca"));
            cbp.Add(new ComboBoxPair("@typetalk.ruralcellular.com", "@typetalk.ruralcellular.com"));
            cbp.Add(new ComboBoxPair("@unistar.unifon.com.ar", "@unistar.unifon.com.ar"));
            cbp.Add(new ComboBoxPair("@uscc.textmsg.com", "@uscc.textmsg.com"));
            cbp.Add(new ComboBoxPair("@voicestream.net", "@voicestream.net"));
            cbp.Add(new ComboBoxPair("@vtext.com", "@vtext.com"));
            cbp.Add(new ComboBoxPair("@wireless.bellsouth.com", "@wireless.bellsouth.com"));
            myControlEntity.ListOfKeyValuePairs = cbp;
            myControlEntity.SelectedValue = myActions.GetValueByKey("cbxcarriersmsemailsuffix");
            myControlEntity.ID = "cbxcarriersmsemailsuffix";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ToolTipx = "";
            myControlEntity.DDLName = "";
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblPhoneComplete";
            myControlEntity.Text = "Phone Completed";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtPhoneComplete";
            myControlEntity.Text = myActions.GetValueByKey("PhoneComplete"); ;
            myControlEntity.ToolTipx = "Enter phone number to send sms text to; do not use hyphens";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblSubjectComplete";
            myControlEntity.Text = "Subject Complete";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtSubjectComplete";
            myControlEntity.Text = myActions.GetValueByKey("SubjectComplete");
            myControlEntity.ToolTipx = "Enter Subject number to send sms text to; do not use hyphens";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblMessageBodyComplete";
            myControlEntity.Text = "MessageBody Complete";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtMessageBodyComplete";
            myControlEntity.Text = myActions.GetValueByKey("MessageBodyComplete"); ;
            myControlEntity.ToolTipx = "Enter MessageBody number to send sms text to; do not use hyphens";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblPhoneError";
            myControlEntity.Text = "Phone Error";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtPhoneError";
            myControlEntity.Text = myActions.GetValueByKey("PhoneError"); ;
            myControlEntity.ToolTipx = "Enter phone number to send sms text to; do not use hyphens";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblSubjectError";
            myControlEntity.Text = "Subject Error";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtSubjectError";
            myControlEntity.Text = myActions.GetValueByKey("SubjectError"); ;
            myControlEntity.ToolTipx = "Enter Subject number to send sms text to; do not use hyphens";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblMessageBodyError";
            myControlEntity.Text = "MessageBody Error";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "txtMessageBodyError";
            myControlEntity.Text = myActions.GetValueByKey("MessageBodyError"); ;
            myControlEntity.ToolTipx = "Enter MessageBody number to send sms text to; do not use hyphens";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 1;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);
            if (strButtonPressed == "btnCancel")
            {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                goto myExit;
            }


            string strUserId = myListControlEntity.Find(x => x.ID == "txtUserId").Text;
            myActions.SetValueByKey("UserId", strUserId);
            string strPassword = myListControlEntity.Find(x => x.ID == "txtPassword").Text;
            myActions.SetValueByKey("Password", strPassword);
            string strClientHost = myListControlEntity.Find(x => x.ID == "txtClientHost").Text;
            myActions.SetValueByKey("ClientHost", strClientHost);
            string strcarriersmsemailsuffix = myListControlEntity.Find(x => x.ID == "cbxcarriersmsemailsuffix").SelectedValue;
            myActions.SetValueByKey("cbxcarriersmsemailsuffix", strcarriersmsemailsuffix);
            string strPhoneComplete = myListControlEntity.Find(x => x.ID == "txtPhoneComplete").Text;
            myActions.SetValueByKey("PhoneComplete", strPhoneComplete);

            string strSubjectComplete = myListControlEntity.Find(x => x.ID == "txtSubjectComplete").Text;
            myActions.SetValueByKey("SubjectComplete", strSubjectComplete);

            string strMessageBodyComplete = myListControlEntity.Find(x => x.ID == "txtMessageBodyComplete").Text;
            myActions.SetValueByKey("MessageBodyComplete", strMessageBodyComplete);

            string strPhoneError = myListControlEntity.Find(x => x.ID == "txtPhoneError").Text;
            myActions.SetValueByKey("PhoneError", strPhoneError);

            string strSubjectError = myListControlEntity.Find(x => x.ID == "txtSubjectError").Text;
            myActions.SetValueByKey("SubjectError", strSubjectError);

            string strMessageBodyError = myListControlEntity.Find(x => x.ID == "txtMessageBodyError").Text;
            myActions.SetValueByKey("MessageBodyError", strMessageBodyError);


        // goto myExit;
        myExit:
            string mystring = "";
      //myActions.ScriptEndedSuccessfullyUpdateStats();
      //Application.Current.Shutdown();
    }
  }
}
