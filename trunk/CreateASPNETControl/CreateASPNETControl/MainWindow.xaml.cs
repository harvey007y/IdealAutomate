using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateASPNETControl {
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
      if (strWindowTitle.StartsWith("Ideal Automate")) {
        myActions.TypeText("%(\" \"n)", 500); // minimize Ideal Automate
      }

      List<ComboBoxPair> cbp = new List<ComboBoxPair>();
      cbp.Add(new ComboBoxPair("LabelCtrl", ""));
      cbp.Add(new ComboBoxPair("TextBoxCtrl", ""));
      cbp.Add(new ComboBoxPair("AmountCtrl", ""));
      cbp.Add(new ComboBoxPair("CheckBox", ""));
      cbp.Add(new ComboBoxPair("DateCtrl", ""));
      cbp.Add(new ComboBoxPair("SearchFieldCtrl", ""));
      cbp.Add(new ComboBoxPair("SearchButtonCtrl", ""));
      cbp.Add(new ComboBoxPair("CountryStateCtrl", ""));
      cbp.Add(new ComboBoxPair("ReferenceEditSectionFW", ""));
      cbp.Add(new ComboBoxPair("ButtonGroupCtrl", ""));
      cbp.Add(new ComboBoxPair("PageSectionManagerCtrl", ""));
      cbp.Add(new ComboBoxPair("PageSectionCtrl", ""));
      cbp.Add(new ComboBoxPair("SimpleListCtrl", ""));
      cbp.Add(new ComboBoxPair("Filter", ""));
      cbp.Add(new ComboBoxPair("DropDownListCtrl", ""));
      cbp.Add(new ComboBoxPair("SearchGLButtonCtrl", ""));
      cbp.Add(new ComboBoxPair("AddressCtrl", ""));
      cbp.Add(new ComboBoxPair("DocumentCtrl", ""));
      cbp.Add(new ComboBoxPair("ReferenceValueEditor", ""));
      cbp.Add(new ComboBoxPair("ReferenceEditSection", ""));
      cbp.Add(new ComboBoxPair("HistoryCtrl", ""));
      cbp.Add(new ComboBoxPair("TreeCtrl", ""));
      cbp.Add(new ComboBoxPair("GLDetailCtrl", ""));
      cbp.Add(new ComboBoxPair("BalModelGLReferenceEditSection", ""));
      cbp.Add(new ComboBoxPair("ClientWindowCtrl", ""));
      cbp.Add(new ComboBoxPair("CallbackCtrl", ""));
      cbp.Add(new ComboBoxPair("CommentCtrl", ""));
      cbp.Add(new ComboBoxPair("DashboardAcctCtrl", ""));
      cbp.Add(new ComboBoxPair("ErrorCtrl", ""));
      cbp.Add(new ComboBoxPair("FTGLReferenceEditSection", ""));
      cbp.Add(new ComboBoxPair("GLReferenceEditSection", ""));
      cbp.Add(new ComboBoxPair("HiddenCtrl", ""));
      cbp.Add(new ComboBoxPair("ImageCtrl", ""));
      cbp.Add(new ComboBoxPair("ModelReferenceEditSection", ""));
      cbp = cbp.OrderBy(x => x._Key).ToList();

      List<ComboBoxPair> cbp1 = new List<ComboBoxPair>();
      cbp1.Add(new ComboBoxPair("false", ""));
      cbp1.Add(new ComboBoxPair("true", ""));

      string myLabel = "Please select a GwControl for which you want to generate code";
      ComboBoxPair myResult = myActions.WindowComboBox(cbp, myLabel);
      string strMyParm = "myParm " + myResult._Key;
      if (myResult._Key == "") {
        myActions.MessageBoxShow("Cancel pressed - script cancelled");
        goto myExit;
      }
      StringBuilder sb = new StringBuilder();
      string myID = "";
      string myAssociatedControlID = "";
      string myText = "";
      string myTextNoSpaces = "";
      string myControl = "";
      string myCodeBehind = "";
      string myWidth = "";
      string myDatabaseField = "";
      bool myRequired = false;
      bool myEnabled = false;
      bool myAllowBlankAmount = false;
      bool myShowTextBox = true;
      bool myShowFormattedAmount = true;
      string myMaxLength = "";
      string myAmount = "";
      List<ControlEntity> myListControlEntity = new List<ControlEntity>();
      ControlEntity myControlEntity = new ControlEntity();
      switch (myResult._Key) {
        case "LabelCtrl":
          myActions.WindowShape("RedBox", "", "Attention", "Put your cursor in the markup in visual studio where you want to insert the control ", 200, 200);
          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.Heading;
          myControlEntity.Text = "Create Label Control";
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.Label;
          myControlEntity.ID = "lblID";
          myControlEntity.Text = "ID:";
          myControlEntity.RowNumber = 0;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.TextBox;
          myControlEntity.ID = "txtID";
          myControlEntity.Text = "lbl???";
          myControlEntity.RowNumber = 0;
          myControlEntity.ColumnNumber = 1;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.Label;
          myControlEntity.ID = "lblAssociatedID";
          myControlEntity.Text = "AssociatedID:";
          myControlEntity.RowNumber = 1;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.TextBox;
          myControlEntity.ID = "txtAssociatedID";
          myControlEntity.Text = "";
          myControlEntity.RowNumber = 1;
          myControlEntity.ColumnNumber = 1;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.Label;
          myControlEntity.ID = "lblText";
          myControlEntity.Text = "Text";
          myControlEntity.RowNumber = 2;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.TextBox;
          myControlEntity.ID = "txtText";
          myControlEntity.Text = "";
          myControlEntity.RowNumber = 2;
          myControlEntity.ColumnNumber = 1;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.CheckBox;
          myControlEntity.ID = "chkAppendColon";
          myControlEntity.Text = "Append Colon to Label";
          myControlEntity.Checked = false;
          myControlEntity.RowNumber = 3;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myActions.WindowMultipleControls(ref myListControlEntity, 700, 900);

          myID = myListControlEntity.Find(x => x.ID == "txtID").Text;
          myAssociatedControlID = myListControlEntity.Find(x => x.ID == "txtAssociatedID").Text;
          myText = myListControlEntity.Find(x => x.ID == "txtText").Text;
          bool myBoolAppendColon = myListControlEntity.Find(x => x.ID == "chkAppendColon").Checked;

          myTextNoSpaces = myText.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("#", "PoundSign").Replace(":", "Colon").Replace("-", "Negative").Replace("&nbsp;", "Space").Replace("!", "Exclamation").Replace("-", "Negative").Replace("?", "QuestionMark").Replace("/", "ForwardSlash").Replace("*", "Asterisk");

          sb.Append("<GwCtrl:GwLabelCtrl ID=\"");
          sb.Append(myID);
          sb.Append("\" runat=\"server\" CssClass=\"Control Label\" ");
          if (myAssociatedControlID != null && myAssociatedControlID.Trim() != "") {
            sb.Append("AssociatedControlID=\"");
            sb.Append(myAssociatedControlID);
            sb.Append("\" ");
          }
          sb.Append("ClientIDMode=\"Static\" />");
          myControl = sb.ToString();
          if (myBoolAppendColon) {
            myCodeBehind = myID + ".Text = Master.GetPageLabel(\"" + myTextNoSpaces + "\", \"" + myText + "\") + \":\"; ";
          } else {
            myCodeBehind = myID + ".Text = Master.GetPageLabel(\"" + myTextNoSpaces + "\", \"" + myText + "\"); ";
          }
          myActions.PutEntityInClipboard(myControl);
          myActions.ActivateWindowByTitle("Web Source - Microsoft Visual Studio (Administrator)");
          myActions.TypeText("%(\" \")", 500); // maximize internet explorer
          myActions.TypeText("x", 500);
          myActions.TypeText("^(v)", 1500); // paste the control into the markup
          myActions.TypeText("{F7}", 1000); // go to code behind
          myActions.TypeText("^(f)", 1000); // open find control
          myActions.PutEntityInClipboard("base.GwLoad(e)");

          myActions.TypeText("^(v)", 1000); // paste search string in find control
          myActions.TypeText("{TAB}", 1000);
          myActions.TypeText("{ENTER}", 1000);
          myActions.TypeText("{ESC}", 1000);
          myActions.TypeText("{ESC}", 1000);
          myActions.TypeText("{LEFT}", 1000);
          myActions.TypeText("{ENTER}", 1000);
          myActions.PutEntityInClipboard(myCodeBehind);
          myActions.TypeText("^(v)", 500);
          myActions.TypeText("{ENTER}", 1000);
          myActions.TypeText("^(+{F6})", 1000); // return to markup by going to previous screen in visual studio

          myActions.Sleep(5000);
          break;
        // ************************************** TextBox Control ***********************************************
        case "TextBoxCtrl":
          // ToDo: put all of these properties on single popup screen so the user just has to fill out one screen
          //  MaxLength="16" Required="true" Width="600" TextType="Custom" RangeMinValue="-9999999999.9999" RangeMaxValue="9999999999.999999" 
          // Enabled="false" style="width:305px"TextMode="MultiLine" onblur="ValidateTime(this, this.value, 'Hours');" 
          // CssClass="DisabledBG" onChange="javascript:AmountValidate(this);"  UseSetWidth="true" UpperCase="true" TextMode="Password"
          myActions.WindowShape("RedBox", "", "Attention", "Put your cursor in the markup in visual studio where you want to insert the control ", 500, 500);
          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.Heading;
          myControlEntity.Text = "Create GwTextBox Control";
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.Label;
          myControlEntity.ID = "lblID";
          myControlEntity.Text = "ID:";
          myControlEntity.RowNumber = 0;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.TextBox;
          myControlEntity.ID = "txtID";
          myControlEntity.Text = "txt???";
          myControlEntity.RowNumber = 0;
          myControlEntity.ColumnNumber = 1;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.Label;
          myControlEntity.ID = "lblWidth";
          myControlEntity.Text = "Width:";
          myControlEntity.RowNumber = 1;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.TextBox;
          myControlEntity.ID = "txtWidth";
          myControlEntity.Text = "";
          myControlEntity.RowNumber = 1;
          myControlEntity.ColumnNumber = 1;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.Label;
          myControlEntity.ID = "lblMaxLength";
          myControlEntity.Text = "MaxLength";
          myControlEntity.RowNumber = 2;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.TextBox;
          myControlEntity.ID = "txtMaxLength";
          myControlEntity.Text = "";
          myControlEntity.RowNumber = 2;
          myControlEntity.ColumnNumber = 1;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.Label;
          myControlEntity.ID = "lblText";
          myControlEntity.Text = "Text";
          myControlEntity.RowNumber = 3;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.TextBox;
          myControlEntity.ID = "txtText";
          myControlEntity.Text = "";
          myControlEntity.RowNumber = 3;
          myControlEntity.ColumnNumber = 1;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.CheckBox;
          myControlEntity.ID = "chkFieldRequired";
          myControlEntity.Text = "Field is Required";
          myControlEntity.Checked = false;
          myControlEntity.RowNumber = 4;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.CheckBox;
          myControlEntity.ID = "chkFieldEnabled";
          myControlEntity.Text = "Field is Enabled";
          myControlEntity.Checked = true;
          myControlEntity.RowNumber = 5;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.Label;
          myControlEntity.ID = "lblDBField";
          myControlEntity.Text = "DBField";
          myControlEntity.RowNumber = 6;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.TextBox;
          myControlEntity.ID = "txtDBField";
          myControlEntity.Text = "x";
          myControlEntity.RowNumber = 6;
          myControlEntity.ColumnNumber = 1;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myActions.WindowMultipleControls(ref myListControlEntity, 700, 900);
          myID = myListControlEntity.Find(x => x.ID == "txtID").Text;
          myWidth = myListControlEntity.Find(x => x.ID == "txtWidth").Text;
          myMaxLength = myListControlEntity.Find(x => x.ID == "txtMaxLength").Text;
          myText = myListControlEntity.Find(x => x.ID == "txtText").Text;
          myRequired = myListControlEntity.Find(x => x.ID == "chkFieldRequired").Checked;
          myEnabled = myListControlEntity.Find(x => x.ID == "chkFieldEnabled").Checked;
          myDatabaseField = myListControlEntity.Find(x => x.ID == "txtDBField").Text;


          // <GwCtrl:GwTextBoxCtrl ID="txtFxRateTrnAmount" runat="server" ClientIDMode="Static" />
          sb.Append("<GwCtrl:GwTextBoxCtrl ID=\"");
          sb.Append(myID);
          sb.Append("\" runat=\"server\" ");
          if (myWidth != null && myWidth.Trim() != "") {
            sb.Append("Width=\"");
            sb.Append(myWidth);
            sb.Append("\" ");
          }

          if (myMaxLength != null && myMaxLength.Trim() != "") {
            sb.Append("MaxLength=\"");
            sb.Append(myMaxLength);
            sb.Append("\" ");
          }

          if (myRequired == true) {
            sb.Append("Required=\"");
            sb.Append("True");
            sb.Append("\" ");
          }

          sb.Append("ClientIDMode=\"Static\" />");
          myControl = sb.ToString();
          myCodeBehind = myID + ".Text = Master.GetPageLabel(\"" + myTextNoSpaces + "\", \"" + myText + "\"); ";

          myActions.PutEntityInClipboard(myControl);
          myActions.ActivateWindowByTitle("Web Source - Microsoft Visual Studio (Administrator)");
          myActions.TypeText("%(\" \")", 500); // maximize internet explorer
          myActions.TypeText("x", 500);
          myActions.TypeText("^(v)", 1500); // paste the control into the markup
          myActions.TypeText("{F7}", 1000); // go to code behind
          if (myText.Trim() != "") {
            myActions.TypeText("^(f)", 1000); // open find control
            myActions.PutEntityInClipboard("base.GwLoad(e)");

            myActions.TypeText("^(v)", 1000); // paste search string in find control
            myActions.TypeText("{TAB}", 1000);
            myActions.TypeText("{ENTER}", 1000);
            myActions.TypeText("{ESC}", 1000);
            myActions.TypeText("{ESC}", 1000);
            myActions.TypeText("{LEFT}", 1000);
            myActions.TypeText("{ENTER}", 1000);
            myActions.PutEntityInClipboard(myCodeBehind);
            myActions.TypeText("^(v)", 500);
            myActions.TypeText("{ENTER}", 1000);
          }
          myActions.TypeText("^(f)", 1000); // open find control
          myActions.PutEntityInClipboard("void InitJS");


          myActions.TypeText("^(v)", 1000); // paste search string in find control
          myActions.TypeText("{TAB}", 1000);
          myActions.TypeText("{ENTER}", 1000);
          myActions.TypeText("{ESC}", 1000);
          myActions.TypeText("{ESC}", 1000);
          GoToEndOfFunction(myActions);

          myCodeBehind = "AddControlToJSList(\"" + myID + "\"," + myID + ".ClientID);";
          myActions.PutEntityInClipboard(myCodeBehind);
          myActions.MessageBoxShow(myCodeBehind);
          myActions.MessageBoxShow("Code Behind for adding Control to JS is in clipboard - paste it where you want it or do nothing and press okay to continue");

          myActions.TypeText("^(f)", 1000); // open find control
          myActions.PutEntityInClipboard("void BindToControls");
          myActions.TypeText("^(v)", 1000); // paste search string in find control
          myActions.TypeText("{TAB}", 1000);
          myActions.TypeText("{ENTER}", 1000);
          myActions.TypeText("{ESC}", 1000);
          myActions.TypeText("{ESC}", 1000);
          GoToEndOfFunction(myActions);
          myCodeBehind = myID + ".Text = " + myDatabaseField + ";";
          myActions.PutEntityInClipboard(myCodeBehind);
          myActions.MessageBoxShow(myCodeBehind);
          myActions.MessageBoxShow("Code Behind for BindToControls is in clipboard - paste it where you want it or do nothing and press okay to continue");

          myActions.TypeText("^(f)", 1000); // open find control
          myActions.PutEntityInClipboard("void BindToBO");
          myActions.TypeText("^(v)", 1000); // paste search string in find control
          myActions.TypeText("{TAB}", 1000);
          myActions.TypeText("{ENTER}", 1000);
          myActions.TypeText("{ESC}", 1000);
          myActions.TypeText("{ESC}", 1000);
          GoToEndOfFunction(myActions);
          myCodeBehind = myDatabaseField + " = " + myID + ".Text;";
          myActions.PutEntityInClipboard(myCodeBehind);
          myActions.MessageBoxShow(myCodeBehind);
          myActions.MessageBoxShow("Code Behind for BindToBO is in clipboard - paste it where you want it or do nothing and press okay to continue");

          if (myEnabled == false) {
            myActions.TypeText("^(f)", 1000); // open find control
            myActions.PutEntityInClipboard("OnPreRender");
            myActions.TypeText("^(v)", 1000); // paste search string in find control
            myActions.TypeText("{TAB}", 1000);
            myActions.TypeText("{ENTER}", 1000);
            myActions.TypeText("{ESC}", 1000);
            myActions.TypeText("{ESC}", 1000);
            GoToEndOfFunction(myActions);
            myCodeBehind = "SetToReadOnly(" + myID + ", true);";
            myActions.PutEntityInClipboard(myCodeBehind);
            myActions.MessageBoxShow(myCodeBehind);
            myActions.MessageBoxShow("Code Behind for BindToBO is in clipboard - paste it where you want it or do nothing and press okay to continue");
          }


          myActions.TypeText("^(+{F6})", 1000); // return to markup by going to previous screen in visual studio

          myActions.Sleep(5000);
          break;
        // ************************************** Amount Control ***********************************************
        case "AmountCtrl":
          // ToDo: put all of these properties on single popup screen so the user just has to fill out one screen
          //  MaxLength="16" Required="true" Width="600" TextType="Custom" RangeMinValue="-9999999999.9999" RangeMaxValue="9999999999.999999" 
          // Enabled="false" style="width:305px"TextMode="MultiLine" onblur="ValidateTime(this, this.value, 'Hours');" 
          // CssClass="DisabledBG" onChange="javascript:AmountValidate(this);"  UseSetWidth="true" UpperCase="true" TextMode="Password"
          myActions.WindowShape("RedBox", "", "Attention", "Put your cursor in the markup in visual studio where you want to insert the control ", 500, 500);
          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.Heading;
          myControlEntity.Text = "Create GwAmount Control";
          myListControlEntity.Add(myControlEntity.CreateControlEntity());
          // ID ***
          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.Label;
          myControlEntity.ID = "lblID";
          myControlEntity.Text = "ID:";
          myControlEntity.RowNumber = 0;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.TextBox;
          myControlEntity.ID = "txtID";
          myControlEntity.Text = "amt???";
          myControlEntity.RowNumber = 0;
          myControlEntity.ColumnNumber = 1;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());
          // Width ***
          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.Label;
          myControlEntity.ID = "lblWidth";
          myControlEntity.Text = "Width:";
          myControlEntity.RowNumber = 1;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.TextBox;
          myControlEntity.ID = "txtWidth";
          myControlEntity.Text = "";
          myControlEntity.RowNumber = 1;
          myControlEntity.ColumnNumber = 1;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());
          // Amount ***

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.Label;
          myControlEntity.ID = "lblAmount";
          myControlEntity.Text = "Amount";
          myControlEntity.RowNumber = 2;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.TextBox;
          myControlEntity.ID = "txtAmount";
          myControlEntity.Text = "";
          myControlEntity.RowNumber = 2;
          myControlEntity.ColumnNumber = 1;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          // AllowBlankAmount ***
          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.CheckBox;
          myControlEntity.ID = "chkAllowBlankAmount";
          myControlEntity.Text = "Allow Blank Amount";
          myControlEntity.Checked = false;
          myControlEntity.RowNumber = 3;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());
          // Enabled ***
          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.CheckBox;
          myControlEntity.ID = "chkFieldEnabled";
          myControlEntity.Text = "Field is Enabled";
          myControlEntity.Checked = true;
          myControlEntity.RowNumber = 4;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.CheckBox;
          myControlEntity.ID = "chkShowTextBox";
          myControlEntity.Text = "Show TextBox";
          myControlEntity.Checked = true;
          myControlEntity.RowNumber = 5;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.CheckBox;
          myControlEntity.ID = "chkShowFormattedAmount";
          myControlEntity.Text = "Show Formatted Amount";
          myControlEntity.Checked = true;
          myControlEntity.RowNumber = 6;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.Label;
          myControlEntity.ID = "lblDBField";
          myControlEntity.Text = "DBField";
          myControlEntity.RowNumber = 7;
          myControlEntity.ColumnNumber = 0;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myControlEntity.ControlEntitySetDefaults();
          myControlEntity.ControlType = ControlType.TextBox;
          myControlEntity.ID = "txtDBField";
          myControlEntity.Text = "x";
          myControlEntity.RowNumber = 7;
          myControlEntity.ColumnNumber = 1;
          myListControlEntity.Add(myControlEntity.CreateControlEntity());

          myActions.WindowMultipleControls(ref myListControlEntity, 700, 900);

          myID = myListControlEntity.Find(x => x.ID == "txtID").Text;
          myWidth = myListControlEntity.Find(x => x.ID == "txtWidth").Text;
          myAmount = myListControlEntity.Find(x => x.ID == "txtAmount").Text;
          myAllowBlankAmount = myListControlEntity.Find(x => x.ID == "chkAllowBlankAmount").Checked;
          myEnabled = myListControlEntity.Find(x => x.ID == "chkFieldEnabled").Checked;
          myShowTextBox = myListControlEntity.Find(x => x.ID == "chkShowTextBox").Checked;
          myShowFormattedAmount = myListControlEntity.Find(x => x.ID == "chkShowFormattedAmount").Checked;
          myDatabaseField = myListControlEntity.Find(x => x.ID == "txtDBField").Text;


          // <GwCtrl:GwTextBoxCtrl ID="txtFxRateTrnAmount" runat="server" ClientIDMode="Static" />
          sb.Append("<GwCtrl:GwAmountCtrl ID=\"");
          sb.Append(myID);
          sb.Append("\" runat=\"server\" ");

          if (myWidth != null && myWidth.Trim() != "") {
            sb.Append("Width=\"");
            sb.Append(myWidth);
            sb.Append("\" ");
          }

          if (myAmount != null && myAmount.Trim() != "") {
            sb.Append("Amount=\"");
            sb.Append(myAmount);
            sb.Append("\" ");
          }

          if (myAllowBlankAmount == true) {
            sb.Append("AllowBlankAmount=\"");
            sb.Append("true");
            sb.Append("\" ");
          }


          if (myEnabled == false) {
            sb.Append("Enabled=\"");
            sb.Append("false");
            sb.Append("\" ");
          }

          if (myShowTextBox == false) {
            sb.Append("ShowTextBox=\"");
            sb.Append("false");
            sb.Append("\" ");
          }

          if (myShowFormattedAmount == false) {
            sb.Append("ShowFormattedAmount=\"");
            sb.Append("false");
            sb.Append("\" ");
          }

          sb.Append("ClientIDMode=\"Static\" />");
          myControl = sb.ToString();
          myCodeBehind = myID + ".Text = Master.GetPageLabel(\"" + myTextNoSpaces + "\", \"" + myText + "\"); ";

          myActions.PutEntityInClipboard(myControl);
          myActions.ActivateWindowByTitle("Web Source - Microsoft Visual Studio (Administrator)");
          myActions.TypeText("%(\" \")", 500); // maximize internet explorer
          myActions.TypeText("x", 500);
          myActions.TypeText("^(v)", 1500); // paste the control into the markup
          myActions.TypeText("{F7}", 1000); // go to code behind

          myActions.TypeText("^(f)", 1000); // open find control
          myActions.PutEntityInClipboard("InitJS");


          myActions.TypeText("^(v)", 1000); // paste search string in find control
          myActions.TypeText("{TAB}", 1000);
          myActions.TypeText("{ENTER}", 1000);
          myActions.TypeText("{ESC}", 1000);
          myActions.TypeText("{ESC}", 1000);
          GoToEndOfFunction(myActions);

          myCodeBehind = "AddControlToJSList(\"" + myID + "\"," + myID + ".ClientID);";
          myActions.PutEntityInClipboard(myCodeBehind);
          myActions.MessageBoxShow(myCodeBehind);
          myActions.MessageBoxShow("Code Behind for adding Control to JS is in clipboard - paste it where you want it or do nothing and press okay to continue");

          myActions.TypeText("^(f)", 1000); // open find control
          myActions.PutEntityInClipboard("void BindToControls");
          myActions.TypeText("^(v)", 1000); // paste search string in find control
          myActions.TypeText("{TAB}", 1000);
          myActions.TypeText("{ENTER}", 1000);
          myActions.TypeText("{ESC}", 1000);
          myActions.TypeText("{ESC}", 1000);
          GoToEndOfFunction(myActions);
          myCodeBehind = myID + ".Amount = " + myDatabaseField + ";";
          myActions.PutEntityInClipboard(myCodeBehind);
          myActions.MessageBoxShow(myCodeBehind);
          myActions.MessageBoxShow("Code Behind for BindToControls is in clipboard - paste it where you want it or do nothing and press okay to continue");

          myActions.TypeText("^(f)", 1000); // open find control
          myActions.PutEntityInClipboard("void BindToBO");
          myActions.TypeText("^(v)", 1000); // paste search string in find control
          myActions.TypeText("{TAB}", 1000);
          myActions.TypeText("{ENTER}", 1000);
          myActions.TypeText("{ESC}", 1000);
          myActions.TypeText("{ESC}", 1000);
          GoToEndOfFunction(myActions);
          myCodeBehind = myDatabaseField + " = " + myID + ".Amount;";
          myActions.PutEntityInClipboard(myCodeBehind);
          myActions.MessageBoxShow(myCodeBehind);
          myActions.MessageBoxShow("Code Behind for BindToBO is in clipboard - paste it where you want it or do nothing and press okay to continue");

          if (myEnabled == false) {
            myActions.TypeText("^(f)", 1000); // open find control
            myActions.PutEntityInClipboard("void OnPreRender");
            myActions.TypeText("^(v)", 1000); // paste search string in find control
            myActions.TypeText("{TAB}", 1000);
            myActions.TypeText("{ENTER}", 1000);
            myActions.TypeText("{ESC}", 1000);
            myActions.TypeText("{ESC}", 1000);
            GoToEndOfFunction(myActions);
            myCodeBehind = "SetToReadOnly(" + myID + ", true);";
            myActions.PutEntityInClipboard(myCodeBehind);
            myActions.MessageBoxShow(myCodeBehind);
            myActions.MessageBoxShow("Code Behind for BindToBO is in clipboard - paste it where you want it or do nothing and press okay to continue");
          }


          myActions.TypeText("^(+{F6})", 1000); // return to markup by going to previous screen in visual studio

          myActions.Sleep(5000);
          break;
        default:
          myActions.MessageBoxShow("The code generator for this control has not been implemented yet");
          break;
      }
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
      myActions.RunSync(@"C:\Windows\Explorer.EXE", @"C:\SVN\GTreasury\branches");
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
      myActions.Run(@"C:\Program Files\Microsoft Office\Office15\EXCEL.EXE", @"C:\SVNStats\SVNStats.xlsx");
    myExit:
      Application.Current.Shutdown();
    }

    private static void GoToEndOfFunction(IdealAutomate.Core.Methods myActions) {
      myActions.TypeText("^(m)m", 1000); // collapse current function
      myActions.TypeText("{DOWN}", 1000);
      myActions.TypeText("^(m)l", 1000); // expand all collapsed functions
      myActions.TypeText("{UP 2}", 1000);
      myActions.TypeText("{ENTER}", 1000);
    }
  }
}

