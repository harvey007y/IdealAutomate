using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Text;
using System;
using System.Windows.Media;

namespace IEGoToURL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
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
            if (strWindowTitle.StartsWith("IEGoToURL"))
            {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            myActions.Sleep(1000);
            int intWindowTop = 0;
            int intWindowLeft = 0;
            int intRowCtr = 0;
            ControlEntity myControlEntity1 = new ControlEntity();
            List<ControlEntity> myListControlEntity1 = new List<ControlEntity>();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();
            List<ComboBoxPair> cbp1 = new List<ComboBoxPair>();

            string strApplicationPath = System.AppDomain.CurrentDomain.BaseDirectory;

            StringBuilder sb = new StringBuilder(); // this is for creating the controls in the window

            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Script Generator";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            // get project folder
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            directory = directory.Replace("\\bin\\Debug\\", "");
            int intLastSlashIndex = directory.LastIndexOf("\\");
        DisplayIEGoToURLWindow:
            myControlEntity1 = new ControlEntity();
            myListControlEntity1 = new List<ControlEntity>();
            cbp = new List<ComboBoxPair>();

            // Row 0 is heading that says:
            // IE Go To URL
            intRowCtr = 0;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Heading;
            myControlEntity1.ID = "lblIEGoToURL";
            myControlEntity1.Text = "IE Go To URL";
            myControlEntity1.Width = 300;
            myControlEntity1.RowNumber = 0;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            // Row 1 has label Syntax and textbox that contains syntax
            // The syntax is hard-coded inline
            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblSyntax";
            myControlEntity1.Text = "Syntax:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtSyntax2";
            myControlEntity1.Text = "myActions.IEGoToURL([[Website URL]], [[Use New Tab]]);";
            myControlEntity1.ColumnSpan = 4;
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            // Row 2 has label Input
            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblInput";
            myControlEntity1.Text = "Input:";
            myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            // Row 3 has label Website URL 
            // and textbox that contains Website URL
            // The value for Website URL comes from roaming folder for script
            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblWebsiteURL";
            myControlEntity1.Text = "Website URL:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtWebsiteURL";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorWebsiteURLx");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());


            // Row 4 has label Use New Tab 
            // and textbox that contains UseNewTab
            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblUseNewTab";
            myControlEntity1.Text = "Use New Tab:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.ComboBox;
            cbp.Clear();
            cbp.Add(new ComboBoxPair("true", "true"));
            cbp.Add(new ComboBoxPair("false", "false"));
            myControlEntity1.ListOfKeyValuePairs = cbp;
            myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorUseNewTab");
            if (myControlEntity1.SelectedValue == null)
            {
                myControlEntity1.SelectedValue = "--Select Item ---";
            }
            myControlEntity1.ID = "cbxUseNewTab";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblUseNewTab";
            myControlEntity1.Text = "(Optional)";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 2;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
            // Display input dialog
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
            // Get Values from input dialog and save to roaming

            string strWebsiteURLx = myListControlEntity1.Find(x => x.ID == "txtWebsiteURL").Text;
            string strUseNewTab = myListControlEntity1.Find(x => x.ID == "cbxUseNewTab").SelectedValue;

            myActions.SetValueByKey("ScriptGeneratorWebsiteURLx", strWebsiteURLx);
            myActions.SetValueByKey("ScriptGeneratorUseNewTab", strUseNewTab);



            // if okay button pressed, validate inputs; place inputs into syntax; put generated 
            // code into clipboard and display generated code
            if (strButtonPressed == "btnOkay")
            {

                string strWebsiteURLToUse = "";

                strWebsiteURLToUse = "\"" + strWebsiteURLx.Trim() + "\"";

                string strGeneratedLinex = "";

                strGeneratedLinex = "myActions.IEGoToURL(myActions, " + strWebsiteURLToUse + ", " + strUseNewTab + ");";

                myActions.PutEntityInClipboard(strGeneratedLinex);
                myActions.MessageBoxShow(strGeneratedLinex + Environment.NewLine + Environment.NewLine + "The generated text has been put into your clipboard");
            }
        myExit:
            myActions.ScriptEndedSuccessfullyUpdateStats();
            Application.Current.Shutdown();
        }
    }
}
