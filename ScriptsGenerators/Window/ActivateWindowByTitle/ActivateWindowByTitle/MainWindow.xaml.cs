using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Collections;

namespace ActivateWindowByTitle
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
            if (strWindowTitle.StartsWith("ActivateWindowByTitle"))
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


        DisplayActivateWindowByTitleWindow:
            myControlEntity1 = new ControlEntity();
            myListControlEntity1 = new List<ControlEntity>();
            cbp = new List<ComboBoxPair>();
            intRowCtr = 0;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Heading;
            myControlEntity1.ID = "lblActivateWindowByTitle";
            myControlEntity1.Text = "Activate Window By Title";
            myControlEntity1.Width = 300;
            myControlEntity1.RowNumber = 0;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

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
            myControlEntity1.Text = "myActions.ActivateWindowByTitle([[Window Title]], [[Show Option]]);";
            myControlEntity1.ColumnSpan = 4;
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblInput";
            myControlEntity1.Text = "Input:";
            myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblWindowTitle";
            myControlEntity1.Text = "Window Title:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.TextBox;
            myControlEntity1.ID = "txtWindowTitle";
            myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorWindowTitlex");
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());



            intRowCtr++;
            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblShowOption";
            myControlEntity1.Text = "Show Option:";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 0;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.ComboBox;
            cbp.Clear();
            cbp.Add(new ComboBoxPair("--Select Item ---", "--Select Item ---"));
            cbp.Add(new ComboBoxPair("SW_HIDE", "0"));
            cbp.Add(new ComboBoxPair("SW_SHOWNORMAL", "1"));
            cbp.Add(new ComboBoxPair("SW_SHOWMINIMIZED", "2"));
            cbp.Add(new ComboBoxPair("SW_SHOWMAXIMIZED", "3"));
            cbp.Add(new ComboBoxPair("SW_SHOWNOACTIVATE", "4"));
            cbp.Add(new ComboBoxPair("SW_RESTORE", "9"));
            cbp.Add(new ComboBoxPair("SW_SHOWDEFAULT", "10"));

            myControlEntity1.ListOfKeyValuePairs = cbp;
            myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorShowOption");
            if (myControlEntity1.SelectedValue == null)
            {
                myControlEntity1.SelectedValue = "--Select Item ---";
            }
            myControlEntity1.ID = "cbxShowOption";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 1;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            myControlEntity1.ControlEntitySetDefaults();
            myControlEntity1.ControlType = ControlType.Label;
            myControlEntity1.ID = "lblShowOption";
            myControlEntity1.Text = "(Optional)";
            myControlEntity1.RowNumber = intRowCtr;
            myControlEntity1.ColumnNumber = 2;
            myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);

            string strWindowTitlex = myListControlEntity1.Find(x => x.ID == "txtWindowTitle").Text;
            string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;

            myActions.SetValueByKey("ScriptGeneratorWindowTitlex", strWindowTitlex);
            myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

            if (strButtonPressed == "btnOkay")
            {

                string strWindowTitleToUse = "";

                strWindowTitleToUse = "\"" + strWindowTitlex.Trim() + "\"";

                string strGeneratedLinex = "";
                if (strShowOption == "--Select Item ---")
                {
                    strGeneratedLinex = "myActions.ActivateWindowByTitle(" + strWindowTitleToUse + ");";
                }
                else
                {
                    strGeneratedLinex = "myActions.ActivateWindowByTitle(" + strWindowTitleToUse + "," + strShowOption + ");";
                }
                myActions.PutEntityInClipboard(strGeneratedLinex);
                myActions.MessageBoxShow(strGeneratedLinex + Environment.NewLine + Environment.NewLine + "The generated text has been put into your clipboard");
            }
        myExit:
            myActions.ScriptEndedSuccessfullyUpdateStats();
            Application.Current.Shutdown();
        }
    }
}
