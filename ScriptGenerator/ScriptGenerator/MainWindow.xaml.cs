using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Linq;
using System.Text;
using System.Windows.Media;

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

            int intWindowTop = 0;
            int intWindowLeft = 0;
            string strWindowTop = "";
            string strWindowLeft = "";
            int intRowCtr = 0;
            ControlEntity myControlEntity1 = new ControlEntity();
            List<ControlEntity> myListControlEntity1 = new List<ControlEntity>();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();
            string strmyArray = "";
            string strmyArrayToUse = "";
            string strServiceNamex = "";
            string strTimeoutMilliseconds = "";

            string strApplicationPath = System.AppDomain.CurrentDomain.BaseDirectory;

            StringBuilder sb = new StringBuilder(); // this is for creating the controls in the window

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

            intRow++;
            if (intRow > 20) {
                intRow = 1;
                intCol++;
            }


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblDeclareVariable";
            myControlEntity.Text = "Declare A Variable";
            myControlEntity.RowNumber = intRow;
            myControlEntity.ColumnNumber = intCol;
            myControlEntity.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
            myControlEntity.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRow++;

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "myButtonDeclareAVariable";
            myControlEntity.Text = "Declare Variable";
            myControlEntity.RowNumber = intRow;
            myControlEntity.ColumnNumber = intCol;
            //    myControlEntity.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
            //   myControlEntity.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
            myListControlEntity.Add(myControlEntity.CreateControlEntity());
            string strScripts = "";
            string strVariables = "";
            string strVariablesValue = "";
            string strScripts1 = "";
            string strVariables1 = "";
            string strScripts2 = "";
            string strVariables2 = "";
            string strVariables1Value = "";
            string strVariables2Value = "";
            GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
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
                case "myButtonStopService":
                    DisplayStopService:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblStopService";
                    myControlEntity1.Text = "Stop Service";
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
                    myControlEntity1.Text = "myActions.StopService([[Service Name]], [[Timeout Milliseconds]]);";
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
                    myControlEntity1.ID = "lblServiceName";
                    myControlEntity1.Text = "Service Name:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtServiceName";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorServiceNamex", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblTimeoutMilliseconds";
                    myControlEntity1.Text = "Timeout Milliseconds:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;

                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorTimeoutMilliseconds", "IdealAutomateDB");

                    myControlEntity1.ID = "txtTimeoutMilliseconds";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());



                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    strServiceNamex = myListControlEntity1.Find(x => x.ID == "txtServiceName").Text;
                    strTimeoutMilliseconds = myListControlEntity1.Find(x => x.ID == "txtTimeoutMilliseconds").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorServiceNamex", strServiceNamex, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorTimeoutMilliseconds", strTimeoutMilliseconds, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayStopService;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strServiceNamex == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter Service Name or select script variable; else press Cancel to Exit");
                            goto DisplayStopService;
                        }
                        string strServiceNameToUse = "";
                        if (strServiceNamex.Trim() == "") {
                            strServiceNameToUse = strVariables;
                        } else {
                            strServiceNameToUse = "\"" + strServiceNamex.Trim() + "\"";
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = "myActions.StopService(" + strServiceNameToUse + "," + strTimeoutMilliseconds + ");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonStartService":
                    DisplayStartService:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblStartService";
                    myControlEntity1.Text = "Start Service";
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
                    myControlEntity1.Text = "myActions.StartService([[Service Name]], [[Timeout Milliseconds]]);";
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
                    myControlEntity1.ID = "lblServiceName";
                    myControlEntity1.Text = "Service Name:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtServiceName";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorServiceNamex", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblTimeoutMilliseconds";
                    myControlEntity1.Text = "Timeout Milliseconds:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;

                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorTimeoutMilliseconds", "IdealAutomateDB");

                    myControlEntity1.ID = "txtTimeoutMilliseconds";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());



                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    strServiceNamex = myListControlEntity1.Find(x => x.ID == "txtServiceName").Text;
                    strTimeoutMilliseconds = myListControlEntity1.Find(x => x.ID == "txtTimeoutMilliseconds").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorServiceNamex", strServiceNamex, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorTimeoutMilliseconds", strTimeoutMilliseconds, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayStartService;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strServiceNamex == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter Service Name or select script variable; else press Cancel to Exit");
                            goto DisplayStartService;
                        }
                        string strServiceNameToUse = "";
                        if (strServiceNamex.Trim() == "") {
                            strServiceNameToUse = strVariables;
                        } else {
                            strServiceNameToUse = "\"" + strServiceNamex.Trim() + "\"";
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = "myActions.StartService(" + strServiceNameToUse + "," + strTimeoutMilliseconds + ");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonSleep":
                    DisplaySleepWindow:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblSleep";
                    myControlEntity1.Text = "Kill All Processes By MillisecondsToSleep";
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
                    myControlEntity1.Text = "myActions.Sleep([[MillisecondsToSleep]]);";
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
                    myControlEntity1.ID = "lblMillisecondsToSleep";
                    myControlEntity1.Text = "MillisecondsToSleep:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtMillisecondsToSleep";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorSleepMillisecondsToSleep", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }



                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    string strMillisecondsToSleep = myListControlEntity1.Find(x => x.ID == "txtMillisecondsToSleep").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorSleepMillisecondsToSleep", strMillisecondsToSleep, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplaySleepWindow;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strMillisecondsToSleep == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter MillisecondsToSleep or select script variable; else press Cancel to Exit");
                            goto DisplaySleepWindow;
                        }
                        string strMillisecondsToSleepToUse = "";
                        if (strMillisecondsToSleep.Trim() == "") {
                            strMillisecondsToSleepToUse = strVariables;
                        } else {
                            strMillisecondsToSleepToUse = strMillisecondsToSleep.Trim();
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = "myActions.Sleep(" + strMillisecondsToSleepToUse + ");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonShiftClick":
                    DisplayShiftClick:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblShiftClick";
                    myControlEntity1.Text = "ShiftClick";
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
                    myControlEntity1.Text = "myActions.ShiftClick([[myArray]]);";
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
                    myControlEntity1.ID = "lblmyArray";
                    myControlEntity1.Text = "myArray:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtmyArray";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorShiftClickmyArray", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }



                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblExample";
                    myControlEntity1.Text = "Example:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtExample";
                    myControlEntity1.Height = 250;
                    myControlEntity1.Text = "      ImageEntity myImage = new ImageEntity(); \r\n" +
" \r\n" +
"      if (boolRunningFromHome) { \r\n" +
"        myImage.ImageFile = \"Images\\\\imgSVNUpdate_Home.PNG\"; \r\n" +
"      } else { \r\n" +
"        myImage.ImageFile = \"Images\\\\imgSVNUpdate.PNG\"; \r\n" +
"      } \r\n" +
"      myImage.Sleep = 200; \r\n" +
"      myImage.Attempts = 5; \r\n" +
"      myImage.RelativeX = 10; \r\n" +
"      myImage.RelativeY = 10; \r\n" +
" \r\n" +
"      int[,] myArray = myActions.PutAll(myImage); \r\n" +
"      if (myArray.Length == 0) { \r\n" +
"        myActions.MessageBoxShow(\"I could not find image of SVN Update\"); \r\n" +
"      } \r\n" +
"      // We found output completed and now want to copy the results \r\n" +
"      // to notepad \r\n" +
" \r\n" +
"      // Highlight the output completed line \r\n" +
"      myActions.Sleep(1000); \r\n" +
"      myActions.ShiftClick(myArray); ";

                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 650, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    strmyArray = myListControlEntity1.Find(x => x.ID == "txtmyArray").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorShiftClickmyArray", strmyArray, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayShiftClick;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strmyArray == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter myArray or select script variable; else press Cancel to Exit");
                            goto DisplayShiftClick;
                        }
                        strmyArrayToUse = "";
                        if (strmyArray.Trim() == "") {
                            strmyArrayToUse = strVariables;
                        } else {
                            strmyArrayToUse = strmyArray.Trim();
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = "myActions.ShiftClick(" + strmyArrayToUse + ");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonSetValueByKey":
                    DisplaySetValueByKeyWindow:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblSetValueByKey";
                    myControlEntity1.Text = "Get Value By Key";
                    myControlEntity1.Width = 300;
                    myControlEntity1.RowNumber = intRowCtr;
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
                    myControlEntity1.Text = "myActions.SetValueByKey([[Key]], [[Value]],\"IdealAutomateDB\");";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblInputs";
                    myControlEntity1.Text = "Inputs:";
                    myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblKey";
                    myControlEntity1.Text = "Key:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtKey";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorSetValueByKeyKey", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts1";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts1DefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("Scripts1DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables1";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables1", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }


                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblValue";
                    myControlEntity1.Text = "Value:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtValue";
                    myControlEntity1.Text  = myActions.GetValueByKey("ScriptGeneratorSetValueByKeyValue", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts2";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts2";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts2 != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable2";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables2";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts2 = 0;
                        Int32.TryParse(strScripts2, out intScripts2);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts2;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }






                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts1 = myListControlEntity1.Find(x => x.ID == "Scripts1").SelectedValue;
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables1") != null) {
                        strVariables1 = myListControlEntity1.Find(x => x.ID == "Variables1").SelectedKey;
                        strVariables1Value = myListControlEntity1.Find(x => x.ID == "Variables1").SelectedValue;
                    }
                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    string strKey = myListControlEntity1.Find(x => x.ID == "txtKey").Text;
                    string strValue = myListControlEntity1.Find(x => x.ID == "txtValue").Text;
                    myActions.SetValueByKey("Scripts1DefaultValue", strScripts1, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables1", strVariables1Value, "IdealAutomateDB");
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorSetValueByKeyKey", strKey, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorSetValueByKeyValue", strValue, "IdealAutomateDB");


                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplaySetValueByKeyWindow;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strKey == "" && (strVariables1 == "--Select Item ---" || strVariables1 == "")) {
                            myActions.MessageBoxShow("Please enter Key or select script variable; else press Cancel to Exit");
                            goto DisplaySetValueByKeyWindow;
                        }
                        if (strValue == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Value or select script variable; else press Cancel to Exit");
                            goto DisplaySetValueByKeyWindow;
                        }
                        string strKeyToUse = "";
                        if (strKey.Trim() == "") {
                            strKeyToUse = strVariables1;
                        } else {
                            strKeyToUse = "\"" + strKey.Trim() + "\"";
                        }

                        string strValueToUse = "";
                        if (strValue.Trim() == "") {
                            strValueToUse = strVariables2;
                        } else {
                            strValueToUse = "\"" + strValue.Trim() + "\"";
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = "myActions.SetValueByKey(" + strKeyToUse + ", " + strValueToUse + ", \"IdealAutomateDB\");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonRunSync":
                    DisplayRunSync:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblRunSync";
                    myControlEntity1.Text = "RunSync";
                    myControlEntity1.Width = 300;
                    myControlEntity1.RowNumber = intRowCtr;
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
                    myControlEntity1.Text = "myActions.RunSync([[Executable]], [[Content]]);";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblInputs";
                    myControlEntity1.Text = "Inputs:";
                    myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblKey";
                    myControlEntity1.Text = "Executable:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtExecutable";
                    myControlEntity1.Text  = myActions.GetValueByKey("ScriptGeneratorRunSyncExecutable", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts1";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts1DefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("Scripts1DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables1";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables1", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblValue";
                    myControlEntity1.Text = "Content:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtContent";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorRunSyncContent", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts2";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts2";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts2 != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable2";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables2";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts2 = 0;
                        Int32.TryParse(strScripts2, out intScripts2);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts2;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }







                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts1 = myListControlEntity1.Find(x => x.ID == "Scripts1").SelectedValue;
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables1") != null) {
                        strVariables1 = myListControlEntity1.Find(x => x.ID == "Variables1").SelectedKey;
                        strVariables1Value = myListControlEntity1.Find(x => x.ID == "Variables1").SelectedValue;
                    }
                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    string strExecutable = myListControlEntity1.Find(x => x.ID == "txtExecutable").Text;
                    string strContent = myListControlEntity1.Find(x => x.ID == "txtContent").Text;
                    myActions.SetValueByKey("Scripts1DefaultValue", strScripts1, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables1", strVariables1Value, "IdealAutomateDB");
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorRunSyncExecutable", strExecutable, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorRunSyncContent", strContent, "IdealAutomateDB");
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayRunSync;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strExecutable == "" && (strVariables1 == "--Select Item ---" || strVariables1 == "")) {
                            myActions.MessageBoxShow("Please enter Executable or select script variable; else press Cancel to Exit");
                            goto DisplayRunSync;
                        }
                        if (strContent == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Content or select script variable; else press Cancel to Exit");
                            goto DisplayRunSync;
                        }
                        string strExecutableToUse = "";
                        if (strExecutable.Trim() == "") {
                            strExecutableToUse = strVariables1;
                        } else {
                            strExecutableToUse = "\"" + strExecutable.Trim() + "\"";
                        }

                        string strContentToUse = "";
                        if (strContent.Trim() == "") {
                            strContentToUse = strVariables2;
                        } else {
                            strContentToUse = "\"" + strContent.Trim() + "\"";
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = "myActions.RunSync(" + strExecutableToUse + ", " + strContentToUse + ");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonRun":
                    DisplayRun:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblRun";
                    myControlEntity1.Text = "Run";
                    myControlEntity1.Width = 300;
                    myControlEntity1.RowNumber = intRowCtr;
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
                    myControlEntity1.Text = "myActions.Run([[Executable]], [[Content]]);";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblInputs";
                    myControlEntity1.Text = "Inputs:";
                    myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblKey";
                    myControlEntity1.Text = "Executable:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtExecutable";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorRunExecutable", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts1";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts1DefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("Scripts1DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables1";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables1", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblValue";
                    myControlEntity1.Text = "Content:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtContent";
                    myControlEntity1.Text  = myActions.GetValueByKey("ScriptGeneratorRunContent", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts2";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts2";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts2 != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable2";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables2";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts2 = 0;
                        Int32.TryParse(strScripts2, out intScripts2);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts2;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }


  




                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts1 = myListControlEntity1.Find(x => x.ID == "Scripts1").SelectedValue;
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables1") != null) {
                        strVariables1 = myListControlEntity1.Find(x => x.ID == "Variables1").SelectedKey;
                        strVariables1Value = myListControlEntity1.Find(x => x.ID == "Variables1").SelectedValue;
                    }
                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    strExecutable = myListControlEntity1.Find(x => x.ID == "txtExecutable").Text;
                    strContent = myListControlEntity1.Find(x => x.ID == "txtContent").Text;
                    myActions.SetValueByKey("Scripts1DefaultValue", strScripts1, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables1", strVariables1Value, "IdealAutomateDB");
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorRunExecutable", strExecutable, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorRunContent", strContent, "IdealAutomateDB");
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayRun;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strExecutable == "" && (strVariables1 == "--Select Item ---" || strVariables1 == "")) {
                            myActions.MessageBoxShow("Please enter Executable or select script variable; else press Cancel to Exit");
                            goto DisplayRun;
                        }
                        if (strContent == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Content or select script variable; else press Cancel to Exit");
                            goto DisplayRun;
                        }
                        string strExecutableToUse = "";
                        if (strExecutable.Trim() == "") {
                            strExecutableToUse = strVariables1;
                        } else {
                            strExecutableToUse = "\"" + strExecutable.Trim() + "\"";
                        }

                        string strContentToUse = "";
                        if (strContent.Trim() == "") {
                            strContentToUse = strVariables2;
                        } else {
                            strContentToUse = "\"" + strContent.Trim() + "\"";
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = "myActions.Run(" + strExecutableToUse + ", " + strContentToUse + ");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonRightClick":
                    DisplayRightClick:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblRightClick";
                    myControlEntity1.Text = "RightClick";
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
                    myControlEntity1.Text = "myActions.RightClick([[myArray]]);";
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
                    myControlEntity1.ID = "lblmyArray";
                    myControlEntity1.Text = "myArray:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtmyArray";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorRightClickmyArray", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }



                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblExample";
                    myControlEntity1.Text = "Example:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtExample";
                    myControlEntity1.Height = 250;
                    myControlEntity1.Text = "      ImageEntity myImage = new ImageEntity(); \r\n" +
" \r\n" +
"      if (boolRunningFromHome) { \r\n" +
"        myImage.ImageFile = \"Images\\\\imgSVNUpdate_Home.PNG\"; \r\n" +
"      } else { \r\n" +
"        myImage.ImageFile = \"Images\\\\imgSVNUpdate.PNG\"; \r\n" +
"      } \r\n" +
"      myImage.Sleep = 200; \r\n" +
"      myImage.Attempts = 5; \r\n" +
"      myImage.RelativeX = 10; \r\n" +
"      myImage.RelativeY = 10; \r\n" +
" \r\n" +
"      int[,] myArray = myActions.PutAll(myImage); \r\n" +
"      if (myArray.Length == 0) { \r\n" +
"        myActions.MessageBoxShow(\"I could not find image of SVN Update\"); \r\n" +
"      } \r\n" +
"      // We found output completed and now want to copy the results \r\n" +
"      // to notepad \r\n" +
" \r\n" +
"      // Highlight the output completed line \r\n" +
"      myActions.Sleep(1000); \r\n" +
"      myActions.RightClick(myArray); ";

                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 650, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    strmyArray = myListControlEntity1.Find(x => x.ID == "txtmyArray").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorRightClickmyArray", strmyArray, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayRightClick;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strmyArray == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter myArray or select script variable; else press Cancel to Exit");
                            goto DisplayRightClick;
                        }
                        strmyArrayToUse = "";
                        if (strmyArray.Trim() == "") {
                            strmyArrayToUse = strVariables;
                        } else {
                            strmyArrayToUse = strmyArray.Trim();
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = "myActions.RightClick(" + strmyArrayToUse + ");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonRestartService":
                    DisplayRestartService:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblRestartService";
                    myControlEntity1.Text = "Restart Service";
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
                    myControlEntity1.Text = "myActions.RestartService([[Service Name]], [[Timeout Milliseconds]]);";
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
                    myControlEntity1.ID = "lblServiceName";
                    myControlEntity1.Text = "Service Name:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtServiceName";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorServiceNamex", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblTimeoutMilliseconds";
                    myControlEntity1.Text = "Timeout Milliseconds:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;

                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorTimeoutMilliseconds", "IdealAutomateDB");

                    myControlEntity1.ID = "txtTimeoutMilliseconds";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());



                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    strServiceNamex = myListControlEntity1.Find(x => x.ID == "txtServiceName").Text;
                    strTimeoutMilliseconds = myListControlEntity1.Find(x => x.ID == "txtTimeoutMilliseconds").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorServiceNamex", strServiceNamex, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorTimeoutMilliseconds", strTimeoutMilliseconds, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayRestartService;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strServiceNamex == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter Service Name or select script variable; else press Cancel to Exit");
                            goto DisplayRestartService;
                        }
                        string strServiceNameToUse = "";
                        if (strServiceNamex.Trim() == "") {
                            strServiceNameToUse = strVariables;
                        } else {
                            strServiceNameToUse = "\"" + strServiceNamex.Trim() + "\"";
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = "myActions.RestartService(" + strServiceNameToUse + "," + strTimeoutMilliseconds + ");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonPutWindowTitleInEntity":
                    DisplayPutWindowTitleInEntity:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    List<ComboBoxPair> cbp1 = new List<ComboBoxPair>();

                    intRowCtr = 0;
                    myListControlEntity1 = new List<ControlEntity>();
                    myControlEntity = new ControlEntity();
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.Text = "Get Active Window Title";
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
                    myControlEntity1.Text = "[[ResultValue]] = myActions.PutWindowTitleInEntity();";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblOutput";
                    myControlEntity1.Text = "Output:";
                    myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblResultValue";
                    myControlEntity1.Text = "ResultValue:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtResultValue";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorPutWindowTitleInEntityResultValue", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts2";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts2";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts2 != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable2";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables2";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts2 = 0;
                        Int32.TryParse(strScripts2, out intScripts2);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts2;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    string strResultValue = myListControlEntity1.Find(x => x.ID == "txtResultValue").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorPutWindowTitleInEntityResultValue", strResultValue, "IdealAutomateDB");
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayPutWindowTitleInEntity;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strResultValue == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Value or select script variable; else press Cancel to Exit");
                            goto DisplayPutWindowTitleInEntity;
                        }

                        string strResultValueToUse = "";
                        if (strResultValue.Trim() == "") {
                            strResultValueToUse = strVariables2;
                        } else {
                            strResultValueToUse = strResultValue.Trim();
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = strResultValueToUse + " = myActions.PutWindowTitleInEntity();";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;

                    break;
                case "myButtonPutInternetExplorerTabURLContainingStringInEntity":
                    DisplayPutInternetExplorerTabURLContainingStringInEntity:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblGetValueByKey";
                    myControlEntity1.Text = "PutInternetExplorerTabURLContainingStringInEntity";
                    myControlEntity1.Width = 300;
                    myControlEntity1.RowNumber = intRowCtr;
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
                    myControlEntity1.Text = "string [[ResultURL]] = myActions.PutInternetExplorerTabURLContainingStringInEntity([[SearchString]]);";
                    myControlEntity1.ColumnSpan = 5;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblOutput";
                    myControlEntity1.Text = "Output:";
                    myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblResultURL";
                    myControlEntity1.Text = "ResultURL:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtResultURL";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorPutInternetExplorerTabURLContainingStringInEntityResultURL", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts2";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts2";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts2 != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable2";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables2";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts2 = 0;
                        Int32.TryParse(strScripts2, out intScripts2);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts2;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }
                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblEmptyRow1";
                    myControlEntity1.Text = "";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
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
                    myControlEntity1.ID = "lblKey";
                    myControlEntity1.Text = "SearchString:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtSearchString";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorPutInternetExplorerTabURLContainingStringInEntitySearchString", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts1";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts1DefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("Scripts1DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables1";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables1", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }




                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts1 = myListControlEntity1.Find(x => x.ID == "Scripts1").SelectedValue;
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables1") != null) {
                        strVariables1 = myListControlEntity1.Find(x => x.ID == "Variables1").SelectedKey;
                        strVariables1Value = myListControlEntity1.Find(x => x.ID == "Variables1").SelectedValue;
                    }
                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    string strSearchString = myListControlEntity1.Find(x => x.ID == "txtSearchString").Text;
                    string strResultURL = myListControlEntity1.Find(x => x.ID == "txtResultURL").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts1DefaultValue", strScripts1, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables1", strVariables1Value, "IdealAutomateDB");
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorPutInternetExplorerTabURLContainingStringInEntitySearchString", strSearchString, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorPutInternetExplorerTabURLContainingStringInEntityResultURL", strResultURL, "IdealAutomateDB");
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayPutInternetExplorerTabURLContainingStringInEntity;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strSearchString == "" && (strVariables1 == "--Select Item ---" || strVariables1 == "")) {
                            myActions.MessageBoxShow("Please enter SearchString or select script variable; else press Cancel to Exit");
                            goto DisplayPutInternetExplorerTabURLContainingStringInEntity;
                        }
                        if (strResultURL == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter ResultURL or select script variable; else press Cancel to Exit");
                            goto DisplayPutInternetExplorerTabURLContainingStringInEntity;
                        }
                        string strSearchStringToUse = "";
                        if (strSearchString.Trim() == "") {
                            strSearchStringToUse = strVariables1;
                        } else {
                            strSearchStringToUse = "\"" + strSearchString.Trim() + "\"";
                        }

                        string strResultURLToUse = "";
                        if (strResultURL.Trim() == "") {
                            strResultURLToUse = strVariables2;
                        } else {
                            strResultURLToUse = "string " + strResultURL.Trim();
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = strResultURLToUse + " = myActions.PutInternetExplorerTabURLContainingStringInEntity(" + strSearchStringToUse + ");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonPutInternetExplorerTabTitleInEntity":
                    DisplayPutInternetExplorerTabTitleInEntity:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp1 = new List<ComboBoxPair>();

                    intRowCtr = 0;
                    myListControlEntity1 = new List<ControlEntity>();
                    myControlEntity = new ControlEntity();
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.Text = "Get Active Window Title";
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
                    myControlEntity1.Text = "string [[ResultValue]] = myActions.PutInternetExplorerTabTitleInEntity();";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblOutput";
                    myControlEntity1.Text = "Output:";
                    myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblResultValue";
                    myControlEntity1.Text = "ResultValue:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtResultValue";
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorPutInternetExplorerTabTitleInEntityResultValue", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts2";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts2";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts2 != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable2";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables2";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts2 = 0;
                        Int32.TryParse(strScripts2, out intScripts2);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts2;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    strResultValue = myListControlEntity1.Find(x => x.ID == "txtResultValue").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorPutInternetExplorerTabTitleInEntityResultValue", strResultValue, "IdealAutomateDB");
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayPutInternetExplorerTabTitleInEntity;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strResultValue == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Value or select script variable; else press Cancel to Exit");
                            goto DisplayPutInternetExplorerTabTitleInEntity;
                        }

                        string strResultValueToUse = "";
                        if (strResultValue.Trim() == "") {
                            strResultValueToUse = strVariables2;
                        } else {
                            strResultValueToUse = "string " + strResultValue.Trim();
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = strResultValueToUse + " = myActions.PutInternetExplorerTabTitleInEntity();";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;

                    break;
                case "myButtonPutEntityInClipboard":
                    DisplayPutEntityInClipboard:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblPutEntityInClipboard";
                    myControlEntity1.Text = "PutEntityInClipboard";
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
                    myControlEntity1.Text = "myActions.PutEntityInClipboard([[myEntity]]);";
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
                    myControlEntity1.ID = "lblmyEntity";
                    myControlEntity1.Text = "myEntity:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtmyEntity";
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorPutEntityInClipboardmyEntity", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }



                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    string strMyEntity = myListControlEntity1.Find(x => x.ID == "txtmyEntity").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorPutEntityInClipboardmyEntity", strMyEntity, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayPutEntityInClipboard;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strMyEntity == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter myEntity or select script variable; else press Cancel to Exit");
                            goto DisplayPutEntityInClipboard;
                        }
                        string strmyEntityToUse = "";
                        if (strMyEntity.Trim() == "") {
                            strmyEntityToUse = strVariables;
                        } else {
                            strmyEntityToUse = "\"" + strMyEntity.Trim() + "\"";
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = "myActions.PutEntityInClipboard(" + strmyEntityToUse + ");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonPutCursorPosition":
                    DisplayPutCursorPosition:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp1 = new List<ComboBoxPair>();

                    intRowCtr = 0;
                    myListControlEntity1 = new List<ControlEntity>();
                    myControlEntity = new ControlEntity();
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.Text = "PutCursorPosition";
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
                    myControlEntity1.Text = "int[,] [[ResultValue]] = myActions.PutCursorPosition();";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblOutput";
                    myControlEntity1.Text = "Output:";
                    myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblResultValue";
                    myControlEntity1.Text = "ResultValue:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtResultValue";
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorPutCursorPositionResultValue", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts2";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts2";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts2 != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable2";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables2";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts2 = 0;
                        Int32.TryParse(strScripts2, out intScripts2);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts2;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    strResultValue = myListControlEntity1.Find(x => x.ID == "txtResultValue").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorPutCursorPositionResultValue", strResultValue, "IdealAutomateDB");
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayPutCursorPosition;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strResultValue == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Value or select script variable; else press Cancel to Exit");
                            goto DisplayPutCursorPosition;
                        }

                        string strResultValueToUse = "";
                        if (strResultValue.Trim() == "") {
                            strResultValueToUse = strVariables2;
                        } else {
                            strResultValueToUse = "int[,] " + strResultValue.Trim();
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = strResultValueToUse + " = myActions.PutCursorPosition();";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;

                    break;
                case "myButtonPutClipboardInEntity":
                    DisplayPutClipboardInEntity:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp1 = new List<ComboBoxPair>();

                    intRowCtr = 0;
                    myListControlEntity1 = new List<ControlEntity>();
                    myControlEntity = new ControlEntity();
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.Text = "PutClipboardInEntity";
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
                    myControlEntity1.Text = "[[ResultValue]] = myActions.PutClipboardInEntity();";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblOutput";
                    myControlEntity1.Text = "Output:";
                    myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblResultValue";
                    myControlEntity1.Text = "ResultValue:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtResultValue";
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorPutClipboardInEntityResultValue", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts2";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts2";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts2 != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable2";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables2";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts2 = 0;
                        Int32.TryParse(strScripts2, out intScripts2);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts2;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    strResultValue = myListControlEntity1.Find(x => x.ID == "txtResultValue").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorPutClipboardInEntityResultValue", strResultValue, "IdealAutomateDB");
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayPutClipboardInEntity;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strResultValue == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Value or select script variable; else press Cancel to Exit");
                            goto DisplayPutClipboardInEntity;
                        }

                        string strResultValueToUse = "";
                        if (strResultValue.Trim() == "") {
                            strResultValueToUse = strVariables2;
                        } else {
                            strResultValueToUse = strResultValue.Trim();
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = strResultValueToUse + " = myActions.PutClipboardInEntity();";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;

                    break;

                case "myButtonPutCaretPositionInArray":
                    DisplayPutCaretPositionInArray:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp1 = new List<ComboBoxPair>();

                    intRowCtr = 0;
                    myListControlEntity1 = new List<ControlEntity>();
                    myControlEntity = new ControlEntity();
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.Text = "PutCaretPositionInArray";
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
                    myControlEntity1.Text = "int[,] [[ResultValue]] = myActions.PutCaretPositionInArray();";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblOutput";
                    myControlEntity1.Text = "Output:";
                    myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblResultValue";
                    myControlEntity1.Text = "ResultValue:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtResultValue";
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorPutCaretPositionInArrayResultValue", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts2";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts2";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts2 != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable2";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables2";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts2 = 0;
                        Int32.TryParse(strScripts2, out intScripts2);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts2;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    strResultValue = myListControlEntity1.Find(x => x.ID == "txtResultValue").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorPutCaretPositionInArrayResultValue", strResultValue, "IdealAutomateDB");
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayPutCaretPositionInArray;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strResultValue == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Value or select script variable; else press Cancel to Exit");
                            goto DisplayPutCaretPositionInArray;
                        }

                        string strResultValueToUse = "";
                        if (strResultValue.Trim() == "") {
                            strResultValueToUse = strVariables2;
                        } else {
                            strResultValueToUse = strResultValue.Trim();
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = "int[,] " + strResultValueToUse + " = myActions.PutCaretPositionInArray();";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;

                    break;                   
                case "myButtonPutAll":
                    DisplayPutAll:
                    intRowCtr = 0;
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblPutAll";
                    myControlEntity1.Text = "PutAll";
                    myControlEntity1.RowNumber = intRowCtr;
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
                    myControlEntity1.Text = "      myImage = new ImageEntity();\r\n " +
" \r\n " +
"      if (boolRunningFromHome) { \r\n " +
"        myImage.ImageFile = \"Images\\\\\" + \"[[homeimage]]\";  \r\n " +
"      } else { \r\n " +
"        myImage.ImageFile = \"Images\\\\\" + \"[[workimage]]\"; \r\n " +
"      } \r\n " +
"      myImage.Sleep = [[Sleep]];  \r\n " +
"      myImage.Attempts = [[Attempts]];  \r\n " +
"      myImage.RelativeX = [[RelativeX]];  \r\n " +
"      myImage.RelativeY = [[RelativeY]]; \r\n " +
" \r\n " +
"      int[,] [[ResultMyArray]] = myActions.PutAll(myImage); ";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.Height = 225;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblHomeImage";
                    myControlEntity1.Text = "HomeImage";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtHomeImage";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorHomeImage", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblWorkImage";
                    myControlEntity1.Text = "WorkImage";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtWorkImage";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorWorkImage", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblSleep";
                    myControlEntity1.Text = "Sleep";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtSleep";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorSleep", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblAttempts";
                    myControlEntity1.Text = "Attempts";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtAttempts";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorAttempts", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblRelativeX";
                    myControlEntity1.Text = "RelativeX";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtRelativeX";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorRelativeX", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblRelativeY";
                    myControlEntity1.Text = "RelativeY";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtRelativeY";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorRelativeY", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblEmptyRow7";
                    myControlEntity1.Text = "";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblOccurrence";
                    myControlEntity1.Text = "Occurrence";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtOccurrence";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorOccurrence", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblTolerance";
                    myControlEntity1.Text = "Tolerance";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtTolerance";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorTolerance", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblUseGrayScale";
                    myControlEntity1.Text = "UseGrayScale";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    cbp.Clear();
                    cbp.Add(new ComboBoxPair("True", "True"));
                    cbp.Add(new ComboBoxPair("False", "False"));
                    myControlEntity1.ListOfKeyValuePairs = cbp;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorUseGrayScale", "IdealAutomateDB");
                    myControlEntity1.ID = "cbxUseGrayScale";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblOutput";
                    myControlEntity1.Text = "Output:";
                    myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblValue";
                    myControlEntity1.Text = "ResultMyArray:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtResultMyArray";
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorPutAllResultMyArray", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts2";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts2";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts2 != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable2";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables2";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts2 = 0;
                        Int32.TryParse(strScripts2, out intScripts2);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts2;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 700, intWindowTop, intWindowLeft);
                    string strHomeImage = myListControlEntity1.Find(x => x.ID == "txtHomeImage").Text;
                    string strWorkImage = myListControlEntity1.Find(x => x.ID == "txtWorkImage").Text;
                    string strSleep = myListControlEntity1.Find(x => x.ID == "txtSleep").Text;
                    string strAttempts = myListControlEntity1.Find(x => x.ID == "txtAttempts").Text;
                    string strRelativeX = myListControlEntity1.Find(x => x.ID == "txtRelativeX").Text;
                    string strRelativeY = myListControlEntity1.Find(x => x.ID == "txtRelativeY").Text;
                    string strOccurrence = myListControlEntity1.Find(x => x.ID == "txtOccurrence").Text;
                    string strTolerance = myListControlEntity1.Find(x => x.ID == "txtTolerance").Text;
                    string strUseGrayScale = myListControlEntity1.Find(x => x.ID == "cbxUseGrayScale").SelectedValue;
                    myActions.SetValueByKey("ScriptGeneratorHomeImage", strHomeImage, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorWorkImage", strWorkImage, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorSleep", strSleep, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorAttempts", strAttempts, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorRelativeX", strRelativeX, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorRelativeY", strRelativeY, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorOccurrence", strOccurrence, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorTolerance", strTolerance, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorUseGrayScale", strUseGrayScale, "IdealAutomateDB");
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    string strResultMyArray = myListControlEntity1.Find(x => x.ID == "txtResultMyArray").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorPutAllResultMyArray", strResultMyArray, "IdealAutomateDB");
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayPutAll;
                    }
                    string strResultMyArrayToUse = "";
                    if (strButtonPressed == "btnOkay") {
                        if (strResultMyArray == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter ResultMyArray or select script variable; else press Cancel to Exit");
                            goto DisplayPutAll;
                        }

                        strResultMyArrayToUse = "";
                        if (strResultMyArray.Trim() == "") {
                            strResultMyArrayToUse = strVariables2;
                        } else {
                            strResultMyArrayToUse = strResultMyArray.Trim();
                        }
                        
                    }
                    string strInFile = strApplicationPath + "Templates\\TemplatePutAll.txt";
                    // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";

                    List<string> listOfSolvedProblems = new List<string>();
                    List<string> listofRecs = new List<string>();
                    string[] lineszz = System.IO.File.ReadAllLines(strInFile);

                    sb.Length = 0;

                    int intLineCount = lineszz.Count();
                    int intCtr = 0;
                    for (int i = 0; i < intLineCount; i++) {
                        string line = lineszz[i];
                        line = line.Replace("&&HomeImage", strHomeImage.Trim());
                        line = line.Replace("&&WorkImage", strWorkImage.Trim());
                        line = line.Replace("&&ResultMyArray", strResultMyArrayToUse.Trim());
                        if (strSleep != "") {
                            line = line.Replace("&&Sleep", strSleep);
                        }
                        if (strAttempts != "") {
                            line = line.Replace("&&Attempts", strAttempts);
                        }
                        if (strRelativeX != "") {
                            line = line.Replace("&&RelativeX", strRelativeX);
                        }
                        if (strRelativeY != "") {
                            line = line.Replace("&&RelativeY", strRelativeY);
                        }
                        if (strOccurrence != "") {
                            line = line.Replace("&&Occurrence", strOccurrence);
                        }
                        if (strTolerance != "") {
                            line = line.Replace("&&Tolerance", strTolerance);
                        }
                        if (strUseGrayScale != "False") {
                            line = line.Replace("&&UseGrayScale", strUseGrayScale);
                        }

                        if (!line.Contains("&&")) {
                            sb.AppendLine(line);
                        }
                    }
                    if (strButtonPressed == "btnOkay") {


                        myActions.PutEntityInClipboard(sb.ToString());
                        myActions.MessageBoxShow(sb.ToString());
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonPositionCursor":
                    DisplayPositionCursor:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblPositionCursor";
                    myControlEntity1.Text = "PositionCursor";
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
                    myControlEntity1.Text = "myActions.PositionCursor([[myArray]]);";
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
                    myControlEntity1.ID = "lblmyArray";
                    myControlEntity1.Text = "myArray:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtmyArray";
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorPositionCursormyArray", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }



                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblExample";
                    myControlEntity1.Text = "Example:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtExample";
                    myControlEntity1.Height = 250;
                    myControlEntity1.Text = "      ImageEntity myImage = new ImageEntity(); \r\n" +
" \r\n" +
"      if (boolRunningFromHome) { \r\n" +
"        myImage.ImageFile = \"Images\\\\imgSVNUpdate_Home.PNG\"; \r\n" +
"      } else { \r\n" +
"        myImage.ImageFile = \"Images\\\\imgSVNUpdate.PNG\"; \r\n" +
"      } \r\n" +
"      myImage.Sleep = 200; \r\n" +
"      myImage.Attempts = 5; \r\n" +
"      myImage.RelativeX = 10; \r\n" +
"      myImage.RelativeY = 10; \r\n" +
" \r\n" +
"      int[,] myArray = myActions.PutAll(myImage); \r\n" +
"      if (myArray.Length == 0) { \r\n" +
"        myActions.MessageBoxShow(\"I could not find image of SVN Update\"); \r\n" +
"      } \r\n" +
"      // We found output completed and now want to copy the results \r\n" +
"      // to notepad \r\n" +
" \r\n" +
"      // Highlight the output completed line \r\n" +
"      myActions.Sleep(1000); \r\n" +
"      myActions.PositionCursor(myArray); ";

                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 650, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    strmyArray = myListControlEntity1.Find(x => x.ID == "txtmyArray").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorPositionCursormyArray", strmyArray, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayPositionCursor;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strmyArray == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter myArray or select script variable; else press Cancel to Exit");
                            goto DisplayPositionCursor;
                        }
                        strmyArrayToUse = "";
                        if (strmyArray.Trim() == "") {
                            strmyArrayToUse = strVariables;
                        } else {
                            strmyArrayToUse = strmyArray.Trim();
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = "myActions.PositionCursor(" + strmyArrayToUse + ");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonMessageBoxShowWithYesNo":
                    DisplayMessageBoxShowWithYesNoWindow:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblMessageBoxShowWithYesNo";
                    myControlEntity1.Text = "MessageBoxShowWithYesNo";
                    myControlEntity1.Width = 300;
                    myControlEntity1.RowNumber = intRowCtr;
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
                    myControlEntity1.Text = "System.Windows.Forms.DialogResult [[ResultYesNo]] = myActions.MessageBoxShowWithYesNo([[Message]]);";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblOutput";
                    myControlEntity1.Text = "Output:";
                    myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblResultYesNo";
                    myControlEntity1.Text = "ResultYesNo:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtResultYesNo";
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorMessageBoxShowWithYesNoResultYesNo", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts2";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts2";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts2 != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable2";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables2";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts2 = 0;
                        Int32.TryParse(strScripts2, out intScripts2);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts2;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }
                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblEmptyRow1";
                    myControlEntity1.Text = "";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
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
                    myControlEntity1.ID = "lblMessage";
                    myControlEntity1.Text = "Message:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtMessage";
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorMessageBoxShowWithYesNoMessage", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts1";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts1DefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("Scripts1DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables1";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables1", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }




                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblExample";
                    myControlEntity1.Text = "Example:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtExample";
                    myControlEntity1.Height = 250;
                    myControlEntity1.Text = "        System.Windows.Forms.DialogResult myResult = myActions.MessageBoxShowWithYesNo(\"I could not find \" + myImage.ImageFile + \"Do you want me to try again ? \"); \r\n " +
   "        if (myResult == System.Windows.Forms.DialogResult.Yes) { \r\n " +
   "          goto TryAgain; \r\n " +
   "        } else { \r\n " +
   "          goto myExit; \r\n " +
   "        } ";

                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 650, 700, intWindowTop, intWindowLeft);
                    strScripts1 = myListControlEntity1.Find(x => x.ID == "Scripts1").SelectedValue;
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables1") != null) {
                        strVariables1 = myListControlEntity1.Find(x => x.ID == "Variables1").SelectedKey;
                        strVariables1Value = myListControlEntity1.Find(x => x.ID == "Variables1").SelectedValue;
                    }
                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    string strMessage = myListControlEntity1.Find(x => x.ID == "txtMessage").Text;
                    string strResultYesNo = myListControlEntity1.Find(x => x.ID == "txtResultYesNo").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts1DefaultValue", strScripts1, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables1", strVariables1Value, "IdealAutomateDB");
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorMessageBoxShowWithYesNoMessage", strMessage, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorMessageBoxShowWithYesNoResultYesNo", strResultYesNo, "IdealAutomateDB");
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayMessageBoxShowWithYesNoWindow;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strMessage == "" && (strVariables1 == "--Select Item ---" || strVariables1 == "")) {
                            myActions.MessageBoxShow("Please enter Message or select script variable; else press Cancel to Exit");
                            goto DisplayMessageBoxShowWithYesNoWindow;
                        }
                        if (strResultYesNo == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter ResultYesNo or select script variable; else press Cancel to Exit");
                            goto DisplayMessageBoxShowWithYesNoWindow;
                        }
                        string strMessageToUse = "";
                        if (strMessage.Trim() == "") {
                            strMessageToUse = strVariables1;
                        } else {
                            strMessageToUse = "\"" + strMessage.Trim() + "\"";
                        }

                        string strResultYesNoToUse = "";
                        if (strResultYesNo.Trim() == "") {
                            strResultYesNoToUse = strVariables2;
                        } else {
                            strResultYesNoToUse = strResultYesNo.Trim();
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = strResultYesNoToUse + " = myActions.GetValueByKey(" + strMessageToUse + ", \"IdealAutomateDB\");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonMessageBoxShow":
                    DisplayMessageBoxShow:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblMessageBoxShow";
                    myControlEntity1.Text = "MessageBoxShow";
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
                    myControlEntity1.Text = "myActions.MessageBoxShow([[Message]]);";
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
                    myControlEntity1.ID = "lblMessage";
                    myControlEntity1.Text = "Message:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtMessage";
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorMessageBoxShowMessage", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }



                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    strMessage = myListControlEntity1.Find(x => x.ID == "txtMessage").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorMessageBoxShowMessage", strMessage, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayMessageBoxShow;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strMessage == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter Message or select script variable; else press Cancel to Exit");
                            goto DisplayMessageBoxShow;
                        }
                        string strMessageToUse = "";
                        if (strMessage.Trim() == "") {
                            strMessageToUse = strVariables;
                        } else {
                            strMessageToUse = "\"" + strMessage.Trim() + "\"";
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = "myActions.MessageBoxShow(" + strMessageToUse + ");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonLeftClick":
                    DisplayLeftClick:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblLeftClick";
                    myControlEntity1.Text = "LeftClick";
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
                    myControlEntity1.Text = "myActions.LeftClick([[myArray]]);";
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
                    myControlEntity1.ID = "lblmyArray";
                    myControlEntity1.Text = "myArray:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtmyArray";
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorLeftClickmyArray", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }



                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblExample";
                    myControlEntity1.Text = "Example:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtExample";
                    myControlEntity1.Height = 250;
                    myControlEntity1.Text = "      ImageEntity myImage = new ImageEntity(); \r\n" +
" \r\n" +
"      if (boolRunningFromHome) { \r\n" +
"        myImage.ImageFile = \"Images\\\\imgSVNUpdate_Home.PNG\"; \r\n" +
"      } else { \r\n" +
"        myImage.ImageFile = \"Images\\\\imgSVNUpdate.PNG\"; \r\n" +
"      } \r\n" +
"      myImage.Sleep = 200; \r\n" +
"      myImage.Attempts = 5; \r\n" +
"      myImage.RelativeX = 10; \r\n" +
"      myImage.RelativeY = 10; \r\n" +
" \r\n" +
"      int[,] myArray = myActions.PutAll(myImage); \r\n" +
"      if (myArray.Length == 0) { \r\n" +
"        myActions.MessageBoxShow(\"I could not find image of SVN Update\"); \r\n" +
"      } \r\n" +
"      // We found output completed and now want to copy the results \r\n" +
"      // to notepad \r\n" +
" \r\n" +
"      // Highlight the output completed line \r\n" +
"      myActions.Sleep(1000); \r\n" +
"      myActions.LeftClick(myArray); ";

                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 650, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    strmyArray = myListControlEntity1.Find(x => x.ID == "txtmyArray").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorLeftClickmyArray", strmyArray, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayLeftClick;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strmyArray == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter myArray or select script variable; else press Cancel to Exit");
                            goto DisplayLeftClick;
                        }
                        strmyArrayToUse = "";
                        if (strmyArray.Trim() == "") {
                            strmyArrayToUse = strVariables;
                        } else {
                            strmyArrayToUse =  strmyArray.Trim();
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = "myActions.LeftClick(" + strmyArrayToUse + ");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonKillAllProcessesByProcessName":
                    DisplayKillAllProcessesByProcessNameWindow:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblKillAllProcessesByProcessName";
                    myControlEntity1.Text = "Kill All Processes By Process Name";
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
                    myControlEntity1.Text = "myActions.KillAllProcessesByProcessName([[Process Name]]);";
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
                    myControlEntity1.ID = "lblProcessName";
                    myControlEntity1.Text = "Process Name:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtProcessName";
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorKillAllProcessesByProcessNameProcessName", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }

                   

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    string strProcessName = myListControlEntity1.Find(x => x.ID == "txtProcessName").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorKillAllProcessesByProcessNameProcessName", strProcessName, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayKillAllProcessesByProcessNameWindow;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strProcessName == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter Process Name or select script variable; else press Cancel to Exit");
                            goto DisplayKillAllProcessesByProcessNameWindow;
                        }
                        string strProcessNameToUse = "";
                        if (strProcessName.Trim() == "") {
                            strProcessNameToUse = strVariables;
                        } else {
                            strProcessNameToUse = "\"" + strProcessName.Trim() + "\"";
                        }
                        string strGeneratedLinex = "";
                        
                            strGeneratedLinex = "myActions.KillAllProcessesByProcessName(" + strProcessNameToUse + ");";
                        
                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonGetWindowTitlesByProcessName":
                    DisplayGetWindowTitlesByProcessName:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblGetValueByKey";
                    myControlEntity1.Text = "Get Window Titles By Process Name";
                    myControlEntity1.Width = 300;
                    myControlEntity1.RowNumber = intRowCtr;
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
                    myControlEntity1.Text = "List<string> [[WindowTitle]] = myActions.GetWindowTitlesByProcessName([[ProcessName]]);";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblOutput";
                    myControlEntity1.Text = "Output:";
                    myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblValue";
                    myControlEntity1.Text = "WindowTitles:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtValue";
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorGetWindowTitlesByProcessNameWindowTitles", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts2";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts2";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts2 != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable2";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables2";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts2 = 0;
                        Int32.TryParse(strScripts2, out intScripts2);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts2;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }
                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblEmptyRow1";
                    myControlEntity1.Text = "";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
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
                    myControlEntity1.ID = "lblKey";
                    myControlEntity1.Text = "ProcessName:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtKey";
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorGetWindowTitlesByProcessNameProcessName", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts1";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts1DefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("Scripts1DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables1";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables1", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }




                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts1 = myListControlEntity1.Find(x => x.ID == "Scripts1").SelectedValue;
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables1") != null) {
                        strVariables1 = myListControlEntity1.Find(x => x.ID == "Variables1").SelectedKey;
                        strVariables1Value = myListControlEntity1.Find(x => x.ID == "Variables1").SelectedValue;
                    }
                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    strKey = myListControlEntity1.Find(x => x.ID == "txtKey").Text;
                    strValue = myListControlEntity1.Find(x => x.ID == "txtValue").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts1DefaultValue", strScripts1, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables1", strVariables1Value, "IdealAutomateDB");
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorGetWindowTitlesByProcessNameProcessName", strKey, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorGetWindowTitlesByProcessNameWindowTitles", strValue, "IdealAutomateDB");
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayGetWindowTitlesByProcessName;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strKey == "" && (strVariables1 == "--Select Item ---" || strVariables1 == "")) {
                            myActions.MessageBoxShow("Please enter Key or select script variable; else press Cancel to Exit");
                            goto DisplayGetWindowTitlesByProcessName;
                        }
                        if (strValue == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Value or select script variable; else press Cancel to Exit");
                            goto DisplayGetWindowTitlesByProcessName;
                        }
                        string strKeyToUse = "";
                        if (strKey.Trim() == "") {
                            strKeyToUse = strVariables1;
                        } else {
                            strKeyToUse = "\"" + strKey.Trim() + "\"";
                        }

                        string strValueToUse = "";
                        if (strValue.Trim() == "") {
                            strValueToUse = strVariables2;
                        } else {
                            strValueToUse = "List<string> " + strValue.Trim();
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = strValueToUse + " = myActions.GetWindowTitlesByProcessName(" + strKeyToUse + ");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonGetValueByKey":
                    DisplayGetValueByKeyWindow:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblGetValueByKey";
                    myControlEntity1.Text = "Get Value By Key";
                    myControlEntity1.Width = 300;
                    myControlEntity1.RowNumber = intRowCtr;
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
                    myControlEntity1.Text = "[[Value]] = myActions.GetValueByKey([[Key]],\"IdealAutomateDB\");";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblOutput";
                    myControlEntity1.Text = "Output:";
                    myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblValue";
                    myControlEntity1.Text = "Value:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtValue";
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorGetValueByKeyValue", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts2";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts2";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts2 != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable2";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables2";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts2 = 0;
                        Int32.TryParse(strScripts2, out intScripts2);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts2;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }
                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblEmptyRow1";
                    myControlEntity1.Text = "";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
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
                    myControlEntity1.ID = "lblKey";
                    myControlEntity1.Text = "Key:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtKey";
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorGetValueByKeyKey", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts1";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts1DefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("Scripts1DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables1";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables1", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }




                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts1 = myListControlEntity1.Find(x => x.ID == "Scripts1").SelectedValue;
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables1") != null) {
                        strVariables1 = myListControlEntity1.Find(x => x.ID == "Variables1").SelectedKey;
                        strVariables1Value = myListControlEntity1.Find(x => x.ID == "Variables1").SelectedValue;
                    }
                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                     strKey = myListControlEntity1.Find(x => x.ID == "txtKey").Text;
                     strValue = myListControlEntity1.Find(x => x.ID == "txtValue").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts1DefaultValue", strScripts1, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables1", strVariables1Value, "IdealAutomateDB");
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorGetValueByKeyKey", strKey, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorGetValueByKeyValue", strValue, "IdealAutomateDB");
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayGetValueByKeyWindow;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strKey == "" && (strVariables1 == "--Select Item ---" || strVariables1 == "")) {
                            myActions.MessageBoxShow("Please enter Key or select script variable; else press Cancel to Exit");
                            goto DisplayGetValueByKeyWindow;
                        }
                        if (strValue == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Value or select script variable; else press Cancel to Exit");
                            goto DisplayGetValueByKeyWindow;
                        }
                        string strKeyToUse = "";
                        if (strKey.Trim() == "") {
                            strKeyToUse = strVariables1;
                        } else {
                            strKeyToUse = "\"" + strKey.Trim() + "\"";
                        }

                        string strValueToUse = "";
                        if (strValue.Trim() == "") {
                            strValueToUse = strVariables2;
                        } else {
                            strValueToUse = strValue.Trim();
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = strValueToUse + " = myActions.GetValueByKey(" + strKeyToUse + ", \"IdealAutomateDB\");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonGetActiveWindowTitle":
                    DisplayGetActiveWindowTitle:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp1 = new List<ComboBoxPair>();

                    intRowCtr = 0;
                    myListControlEntity1 = new List<ControlEntity>();                   
                    myControlEntity = new ControlEntity();
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.Text = "Get Active Window Title";
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
                    myControlEntity1.Text = "[[ResultValue]] = myActions.GetActiveWindowTitle();";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblOutput";
                    myControlEntity1.Text = "Output:";
                    myControlEntity1.FontFamilyx = new FontFamily("Segoe UI Bold");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblResultValue";
                    myControlEntity1.Text = "ResultValue:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtResultValue";
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorGetActiveWindowTitleResultValue", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts2";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts2";
                    myControlEntity1.DDLName = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts2 != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable2";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables2";
                        myControlEntity1.DDLName = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts2 = 0;
                        Int32.TryParse(strScripts2, out intScripts2);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts2;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                   if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    strResultValue = myListControlEntity1.Find(x => x.ID == "txtResultValue").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorGetActiveWindowTitleResultValue", strResultValue, "IdealAutomateDB");
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayGetActiveWindowTitle;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strResultValue == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Value or select script variable; else press Cancel to Exit");
                            goto DisplayGetActiveWindowTitle;
                        }
            
                        string strResultValueToUse = "";
                        if (strResultValue.Trim() == "") {
                            strResultValueToUse = strVariables2;
                        } else {
                            strResultValueToUse = strResultValue.Trim();
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = strResultValueToUse + " = myActions.GetActiveWindowTitle();";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;

                    break;
                case "myButtonFindDelimitedText":
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp1 = new List<ComboBoxPair>();
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblFindDelimitedText";
                    myControlEntity1.Text = "Find Delimited Text";
                    myControlEntity1.RowNumber = 1;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblInputs";
                    myControlEntity1.Text = "Inputs:";
                    myControlEntity1.ToolTipx = "";
                    myControlEntity1.RowNumber = 2;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblNotImplemented";
                    myControlEntity1.Text = "Code Behind Not Implemented";
                    myControlEntity1.ToolTipx = "";
                    myControlEntity1.RowNumber = 2;
                    myControlEntity1.ColumnNumber = 1;
                    myControlEntity1.BackgroundColor = Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
                    myControlEntity1.ForegroundColor = Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lbllines";
                    myControlEntity1.Text = "lines";
                    myControlEntity1.RowNumber = 3;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtlines";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorlines", "IdealAutomateDB"); ;
                    myControlEntity1.ToolTipx = "string[]";
                    myControlEntity1.RowNumber = 3;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblStartingColumn";
                    myControlEntity1.Text = "StartingColumn";
                    myControlEntity1.RowNumber = 4;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtStartingColumn";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorStartingColumn", "IdealAutomateDB"); ;
                    myControlEntity1.ToolTipx = "int";
                    myControlEntity1.RowNumber = 4;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblListBeginDelim";
                    myControlEntity1.Text = "ListBeginDelim";
                    myControlEntity1.RowNumber = 6;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtListBeginDelim";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorListBeginDelim", "IdealAutomateDB"); ;
                    myControlEntity1.ToolTipx = "List<string>";
                    myControlEntity1.RowNumber = 6;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblListEndDelim";
                    myControlEntity1.Text = "ListEndDelim";
                    myControlEntity1.RowNumber = 7;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtListEndDelim";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorListEndDelim", "IdealAutomateDB"); ;
                    myControlEntity1.ToolTipx = "List<string>";
                    myControlEntity1.RowNumber = 7;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblEmptyRow9";
                    myControlEntity1.Text = "";
                    myControlEntity1.ToolTipx = "&&TOOLTIP";
                    myControlEntity1.RowNumber = 9;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblLineCtrInputandOut";
                    myControlEntity1.Text = "LineCtr Input and Out:";
                    myControlEntity1.ToolTipx = "";
                    myControlEntity1.RowNumber = 10;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblLineCtr";
                    myControlEntity1.Text = "LineCtr";
                    myControlEntity1.RowNumber = 11;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtLineCtr";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorLineCtr", "IdealAutomateDB"); ;
                    myControlEntity1.ToolTipx = "int";
                    myControlEntity1.RowNumber = 11;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblEmptyRow12";
                    myControlEntity1.Text = "";
                    myControlEntity1.ToolTipx = "&&TOOLTIP";
                    myControlEntity1.RowNumber = 12;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblOutputparmsfollow";
                    myControlEntity1.Text = "Output parms follow:";
                    myControlEntity1.ToolTipx = "";
                    myControlEntity1.RowNumber = 13;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblstrDelimitedTextFound";
                    myControlEntity1.Text = "strDelimitedTextFound";
                    myControlEntity1.RowNumber = 14;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtstrDelimitedTextFound";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorstrDelimitedTextFound", "IdealAutomateDB"); ;
                    myControlEntity1.ToolTipx = "string";
                    myControlEntity1.RowNumber = 14;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblintDelimFound";
                    myControlEntity1.Text = "intDelimFound";
                    myControlEntity1.RowNumber = 15;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtintDelimFound";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorintDelimFound", "IdealAutomateDB"); ;
                    myControlEntity1.ToolTipx = "int";
                    myControlEntity1.RowNumber = 15;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblstrResultTypeFound";
                    myControlEntity1.Text = "strResultTypeFound";
                    myControlEntity1.RowNumber = 16;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtstrResultTypeFound";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorstrResultTypeFound", "IdealAutomateDB"); ;
                    myControlEntity1.ToolTipx = "string";
                    myControlEntity1.RowNumber = 16;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblintEndDelimColPosFound";
                    myControlEntity1.Text = "intEndDelimColPosFound";
                    myControlEntity1.RowNumber = 17;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtintEndDelimColPosFound";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorintEndDelimColPosFound", "IdealAutomateDB"); ;
                    myControlEntity1.ToolTipx = "int";
                    myControlEntity1.RowNumber = 17;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    string strlines = myListControlEntity1.Find(x => x.ID == "txtlines").Text;
                    myActions.SetValueByKey("ScriptGeneratorlines", strlines, "IdealAutomateDB");
                    string strStartingColumn = myListControlEntity1.Find(x => x.ID == "txtStartingColumn").Text;
                    myActions.SetValueByKey("ScriptGeneratorStartingColumn", strStartingColumn, "IdealAutomateDB");
                    string strListBeginDelim = myListControlEntity1.Find(x => x.ID == "txtListBeginDelim").Text;
                    myActions.SetValueByKey("ScriptGeneratorListBeginDelim", strListBeginDelim, "IdealAutomateDB");
                    string strListEndDelim = myListControlEntity1.Find(x => x.ID == "txtListEndDelim").Text;
                    myActions.SetValueByKey("ScriptGeneratorListEndDelim", strListEndDelim, "IdealAutomateDB");
                    string strLineCtr = myListControlEntity1.Find(x => x.ID == "txtLineCtr").Text;
                    myActions.SetValueByKey("ScriptGeneratorLineCtr", strLineCtr, "IdealAutomateDB");
                    string strstrDelimitedTextFound = myListControlEntity1.Find(x => x.ID == "txtstrDelimitedTextFound").Text;
                    myActions.SetValueByKey("ScriptGeneratorstrDelimitedTextFound", strstrDelimitedTextFound, "IdealAutomateDB");
                    string strintDelimFound = myListControlEntity1.Find(x => x.ID == "txtintDelimFound").Text;
                    myActions.SetValueByKey("ScriptGeneratorintDelimFound", strintDelimFound, "IdealAutomateDB");
                    string strstrResultTypeFound = myListControlEntity1.Find(x => x.ID == "txtstrResultTypeFound").Text;
                    myActions.SetValueByKey("ScriptGeneratorstrResultTypeFound", strstrResultTypeFound, "IdealAutomateDB");
                    string strintEndDelimColPosFound = myListControlEntity1.Find(x => x.ID == "txtintEndDelimColPosFound").Text;
                    myActions.SetValueByKey("ScriptGeneratorintEndDelimColPosFound", strintEndDelimColPosFound, "IdealAutomateDB");
                    sb.Length = 0;
                    sb.Append("Have not implemented code gen yet");
                    if (strButtonPressed == "btnOkay") {


                        myActions.PutEntityInClipboard(sb.ToString());
                        myActions.MessageBoxShow(sb.ToString());
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;


                    break;
                case "myButtonClickImageIfExists":
                    intRowCtr = 0;
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblClickImageIfExists";
                    myControlEntity1.Text = "Click Image If Exists";
                    myControlEntity1.RowNumber = intRowCtr; 
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
                    myControlEntity1.Text = "      myImage = new ImageEntity();\r\n " +
" \r\n " +
"      if (boolRunningFromHome) { \r\n " +
"        myImage.ImageFile = \"Images\\\\\" + \"[[homeimage]]\";  \r\n " +
"      } else { \r\n " +
"        myImage.ImageFile = \"Images\\\\\" + \"[[workimage]]\"; \r\n " +
"      } \r\n " +
"      myImage.Sleep = [[Sleep]];  \r\n " +
"      myImage.Attempts = [[Attempts]];  \r\n " +
"      myImage.RelativeX = [[RelativeX]];  \r\n " +
"      myImage.RelativeY = [[RelativeY]]; \r\n " +
" \r\n " +
"      myActions.ClickImageIfExists(myImage); ";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.Height = 250;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblHomeImage";
                    myControlEntity1.Text = "HomeImage";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtHomeImage";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorHomeImage", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblWorkImage";
                    myControlEntity1.Text = "WorkImage";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtWorkImage";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorWorkImage", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblSleep";
                    myControlEntity1.Text = "Sleep";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtSleep";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorSleep", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblAttempts";
                    myControlEntity1.Text = "Attempts";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtAttempts";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorAttempts", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblRelativeX";
                    myControlEntity1.Text = "RelativeX";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtRelativeX";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorRelativeX", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblRelativeY";
                    myControlEntity1.Text = "RelativeY";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtRelativeY";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorRelativeY", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblEmptyRow7";
                    myControlEntity1.Text = "";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblOccurrence";
                    myControlEntity1.Text = "Occurrence";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtOccurrence";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorOccurrence", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblTolerance";
                    myControlEntity1.Text = "Tolerance";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtTolerance";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorTolerance", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblUseGrayScale";
                    myControlEntity1.Text = "UseGrayScale";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    cbp.Clear();
                    cbp.Add(new ComboBoxPair("True", "True"));
                    cbp.Add(new ComboBoxPair("False", "False"));
                    myControlEntity1.ListOfKeyValuePairs = cbp;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorUseGrayScale", "IdealAutomateDB");
                    myControlEntity1.ID = "cbxUseGrayScale";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 650, 500, intWindowTop, intWindowLeft);
                    strHomeImage = myListControlEntity1.Find(x => x.ID == "txtHomeImage").Text;
                    strWorkImage = myListControlEntity1.Find(x => x.ID == "txtWorkImage").Text;
                    strSleep = myListControlEntity1.Find(x => x.ID == "txtSleep").Text;
                    strAttempts = myListControlEntity1.Find(x => x.ID == "txtAttempts").Text;
                    strRelativeX = myListControlEntity1.Find(x => x.ID == "txtRelativeX").Text;
                    strRelativeY = myListControlEntity1.Find(x => x.ID == "txtRelativeY").Text;
                    strOccurrence = myListControlEntity1.Find(x => x.ID == "txtOccurrence").Text;
                    strTolerance = myListControlEntity1.Find(x => x.ID == "txtTolerance").Text;
                    strUseGrayScale = myListControlEntity1.Find(x => x.ID == "cbxUseGrayScale").SelectedValue;
                    myActions.SetValueByKey("ScriptGeneratorHomeImage", strHomeImage, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorWorkImage", strWorkImage, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorSleep", strSleep, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorAttempts", strAttempts, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorRelativeX", strRelativeX, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorRelativeY", strRelativeY, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorOccurrence", strOccurrence, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorTolerance", strTolerance, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorUseGrayScale", strUseGrayScale, "IdealAutomateDB");

                    strInFile = strApplicationPath + "Templates\\TemplateClickImageIfExists.txt";
                    // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";

                   listOfSolvedProblems = new List<string>();
                   listofRecs = new List<string>();
                   lineszz = System.IO.File.ReadAllLines(strInFile);

                    sb.Length = 0;

                    intLineCount = lineszz.Count();
                    intCtr = 0;
                    for (int i = 0; i < intLineCount; i++) {
                        string line = lineszz[i];
                        line = line.Replace("&&HomeImage", strHomeImage.Trim());
                        line = line.Replace("&&WorkImage", strWorkImage.Trim());
                        if (strSleep != "") {
                            line = line.Replace("&&Sleep", strSleep);
                        }
                        if (strAttempts != "") {
                            line = line.Replace("&&Attempts", strAttempts);
                        }
                        if (strRelativeX != "") {
                            line = line.Replace("&&RelativeX", strRelativeX);
                        }
                        if (strRelativeY != "") {
                            line = line.Replace("&&RelativeY", strRelativeY);
                        }
                        if (strOccurrence != "") {
                            line = line.Replace("&&Occurrence", strOccurrence);
                        }
                        if (strTolerance != "") {
                            line = line.Replace("&&Tolerance", strTolerance);
                        }
                        if (strUseGrayScale != "False") {
                            line = line.Replace("&&UseGrayScale", strUseGrayScale);
                        }

                        if (!line.Contains("&&")) {
                            sb.AppendLine(line);
                        }
                    }
                    if (strButtonPressed == "btnOkay") {


                        myActions.PutEntityInClipboard(sb.ToString());
                        myActions.MessageBoxShow(sb.ToString());
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonActivateWindowByTitle":
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
                    myControlEntity1.Text =  myActions.GetValueByKey("ScriptGeneratorWindowTitlex", "IdealAutomateDB");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 3;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 4;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = intRowCtr;
                        myControlEntity1.ColumnNumber = 5;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables", "IdealAutomateDB");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }

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
                    myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorShowOption", "IdealAutomateDB");
                    if (myControlEntity1.SelectedValue == null) {
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

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 400, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    string strWindowTitlex = myListControlEntity1.Find(x => x.ID == "txtWindowTitle").Text;
                    string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorWindowTitlex", strWindowTitlex, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption, "IdealAutomateDB");

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayActivateWindowByTitleWindow;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strWindowTitlex == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter Window Title or select script variable; else press Cancel to Exit");
                            goto DisplayActivateWindowByTitleWindow;
                        }
                        string strWindowTitleToUse = "";
                        if (strWindowTitlex.Trim() == "") {
                            strWindowTitleToUse = strVariables;
                        } else {
                            strWindowTitleToUse = strWindowTitlex.Trim();
                        }
                        string strGeneratedLinex = "";
                        if (strShowOption == "--Select Item ---") {
                            strGeneratedLinex = "myActions.ActivateWindowByTitle(\"" + strWindowTitleToUse + "\");";
                        } else {
                            strGeneratedLinex = "myActions.ActivateWindowByTitle(\"" + strWindowTitleToUse + "\"," + strShowOption + ");";
                        }
                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;

                case "myButtonDeclareAVariable":

                    myActions.RunSync(@"C:\SVNIA\trunk\DDLMaint\DDLMaint\bin\debug\DDLMaint.exe", "");
                    break;
                case "myButtonTypeText":
                    myListControlEntity1 = new List<ControlEntity>();

                    myControlEntity1 = new ControlEntity();
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
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblAppendComment";
                    myControlEntity1.Text = "Append Comment (no slashes needed):";
                    myControlEntity1.RowNumber = 2;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtAppendComment";
                    myControlEntity1.Text = "";
                    myControlEntity1.RowNumber = 2;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());


                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.CheckBox;
                    myControlEntity1.ID = "chkVariable";
                    myControlEntity1.Text = "Is this a variable?";
                    myControlEntity1.RowNumber = 3;
                    myControlEntity1.ColumnNumber = 0;
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
                    myControlEntity1.RowNumber = 4;
                    myControlEntity1.ColumnNumber = 0;
                    if (myActions.GetValueByKey("ScriptGeneratorCtrlKey", "IdealAutomateDB") == "True") {
                        myControlEntity1.Checked = true;
                    } else {
                        myControlEntity1.Checked = false;
                    }
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.CheckBox;
                    myControlEntity1.ID = "chkAltKey";
                    myControlEntity1.Text = "Is Alt Key Pressed?";
                    myControlEntity1.RowNumber = 5;
                    myControlEntity1.ColumnNumber = 0;
                    if (myActions.GetValueByKey("ScriptGeneratorAltKey", "IdealAutomateDB") == "True") {
                        myControlEntity1.Checked = true;
                    } else {
                        myControlEntity1.Checked = false;
                    }
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.CheckBox;
                    myControlEntity1.ID = "chkShiftKey";
                    myControlEntity1.Text = "Is Shift Key Pressed?";
                    myControlEntity1.RowNumber = 6;
                    myControlEntity1.ColumnNumber = 0;
                    if (myActions.GetValueByKey("ScriptGeneratorShiftKey", "IdealAutomateDB") == "True") {
                        myControlEntity1.Checked = true;
                    } else {
                        myControlEntity1.Checked = false;
                    }
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblVSShortCutKeys";
                    myControlEntity1.Text = "Visual Studio Shortcut Keys:";
                    myControlEntity1.RowNumber = 7;
                    myControlEntity1.ColumnNumber = 0;
                    myControlEntity1.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
                    myControlEntity1.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);

                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnMinimizeVisualStudio";
                    myControlEntity1.Text = "Minimize Visual Studio";
                    myControlEntity1.RowNumber = 8;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnMaximizeVisualStudio";
                    myControlEntity1.Text = "Maximize Visual Studio";
                    myControlEntity1.RowNumber = 9;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblIEShortCutKeys";
                    myControlEntity1.Text = "Internet Explorer Shortcut Keys:";
                    myControlEntity1.RowNumber = 10;
                    myControlEntity1.ColumnNumber = 0;
                    myControlEntity1.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
                    myControlEntity1.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnIEAltD";
                    myControlEntity1.Text = "Go To Address Bar and select it Alt-D";
                    myControlEntity1.RowNumber = 11;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnIEAltEnter";
                    myControlEntity1.Text = "Alt enter while in address bar opens new tab";
                    myControlEntity1.RowNumber = 12;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnIEF6";
                    myControlEntity1.Text = "F6 selects address bar in IE";
                    myControlEntity1.RowNumber = 13;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnIEMax";
                    myControlEntity1.Text = "Maximize IE";
                    myControlEntity1.RowNumber = 14;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnIECloseCurrentTab";
                    myControlEntity1.Text = "Close Current Tab";
                    myControlEntity1.RowNumber = 15;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnIEGoToTopOfPage";
                    myControlEntity1.Text = "HOME goes to top of page";
                    myControlEntity1.RowNumber = 16;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnIEClose";
                    myControlEntity1.Text = "Close IE";
                    myControlEntity1.RowNumber = 17;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());


                    //          internet explorer
                    //myActions.TypeText("%(d)", 2500); // go to address bar in internet explorer
                    //          myActions.TypeText("%({ENTER})", 2500);  // Alt enter while in address bar opens new tab
                    //          myActions.TypeText("{F6}", 2500); // selects address bar in internet explorer
                    //          myActions.TypeText("%(\" \")", 500); // maximize internet explorer
                    //          myActions.TypeText("x", 500);
                    //          myActions.TypeText("^(w)", 500); // close the current tab
                    //          myActions.TypeText("{HOME}", 500); // go to top of web page
                    //          myActions.TypeText("%(f)", 1000);  // close internet explorer
                    //          myActions.TypeText("x", 1000);  // close internet explorer

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblEditingShortCutKeys";
                    myControlEntity1.Text = "Editing Shortcut Keys:";
                    myControlEntity1.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
                    myControlEntity1.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
                    myControlEntity1.RowNumber = 3;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnCopy";
                    myControlEntity1.Text = "Ctrl-C Copy";
                    myControlEntity1.RowNumber = 4;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnCut";
                    myControlEntity1.Text = "Ctrl-x Cut";
                    myControlEntity1.RowNumber = 5;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnSelectAll";
                    myControlEntity1.Text = "Ctrl-a Select All";
                    myControlEntity1.RowNumber = 6;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnPaste";
                    myControlEntity1.Text = "Ctrl-v Paste";
                    myControlEntity1.RowNumber = 7;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblSpecialKeys";
                    myControlEntity1.Text = "Special Keys:";
                    myControlEntity1.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
                    myControlEntity1.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
                    myControlEntity1.RowNumber = 8;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDelete";
                    myControlEntity1.Text = "{DELETE}";
                    myControlEntity1.RowNumber = 9;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDown";
                    myControlEntity1.Text = "{DOWN}";
                    myControlEntity1.RowNumber = 10;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnEnd";
                    myControlEntity1.Text = "{END}";
                    myControlEntity1.RowNumber = 11;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnEnter";
                    myControlEntity1.Text = "{ENTER}";
                    myControlEntity1.RowNumber = 12;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnEscape";
                    myControlEntity1.Text = "{ESCAPE}";
                    myControlEntity1.RowNumber = 13;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnFxx";
                    myControlEntity1.Text = "{Fxx}";
                    myControlEntity1.RowNumber = 14;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnHome";
                    myControlEntity1.Text = "{HOME}";
                    myControlEntity1.RowNumber = 15;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnLeft";
                    myControlEntity1.Text = "{LEFT}";
                    myControlEntity1.RowNumber = 16;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnPGDN";
                    myControlEntity1.Text = "{PGDN}";
                    myControlEntity1.RowNumber = 17;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnPGUP";
                    myControlEntity1.Text = "{PGUP}";
                    myControlEntity1.RowNumber = 18;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnRight";
                    myControlEntity1.Text = "{RIGHT}";
                    myControlEntity1.RowNumber = 19;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnSpace";
                    myControlEntity1.Text = "{SPACE}";
                    myControlEntity1.RowNumber = 20;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnTAB";
                    myControlEntity1.Text = "{TAB}";
                    myControlEntity1.RowNumber = 21;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnUP";
                    myControlEntity1.Text = "{UP}";
                    myControlEntity1.RowNumber = 22;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblSpecialKeysModifier";
                    myControlEntity1.Text = "Special Keys Repeat Count/Func Modifier:";
                    myControlEntity1.RowNumber = 23;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtSpecialKeysModifier";
                    myControlEntity1.Text = "";
                    myControlEntity1.RowNumber = 23;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);

                    if (strButtonPressed == "btnCancel") {
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                        goto DisplayWindowAgain;
                    }

                    if (strButtonPressed == "btnMinimizeVisualStudio") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%(\" \"n)";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "Minimize Visual Studio";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnMaximizeVisualStudio") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%(f)x";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "Maximize Visual Studio";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnIEAltD") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%(d)";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "Go to IE address bar and select it";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }


                    if (strButtonPressed == "btnIEAltEnter") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%({ENTER})";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "Alt enter while in address bar opens new tab";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnIEF6") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{F6}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "F6 is another way to highlight address bar in IE";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnIEMax") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%(\" \")";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "maximize internet explorer";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnIECloseCurrentTab") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "^(w)";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "close the current tab";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnIEGoToTopOfPage") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{HOME}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "go to top of web page";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnIEClose") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%(f)x";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "close internet explorer";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnCopy") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "^(c)";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "copy";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnCut") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "^(x)";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "cut";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnSelectAll") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "^(a)";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "select all";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnPaste") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "^(v)";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "paste";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    string txtSpecialKeysModifier = myListControlEntity1.Find(x => x.ID == "txtSpecialKeysModifier").Text;

                    if (strButtonPressed == "btnFxx") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{F" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (txtSpecialKeysModifier.Length > 0) {
                        txtSpecialKeysModifier = " " + txtSpecialKeysModifier;
                    }

                    if (strButtonPressed == "btnDelete") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{DELETE" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "delete";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnDown") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{DOWN" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "down";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnEnd") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{END" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "end";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnEnter") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{ENTER" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "enter";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnEscape") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{ESCAPE" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "escape";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnHome") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{HOME" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "home";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnLeft") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{LEFT" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "left";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnPGDN") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{PGDN" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "page down";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnPGUP") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{PGUP" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "page up";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnRight") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{RIGHT" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "right";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnSpace") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{SPACE" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "space";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnTAB") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{TAB" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "tab";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnUP") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{UP" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "up";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, intWindowTop, intWindowLeft);
                    }





                    //if (strButtonPressed == "btnOkay") {
                    //  strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 100, 850);
                    //  goto DisplayWindowAgain;
                    //}
                    string strTextToType = myListControlEntity1.Find(x => x.ID == "txtTextToType").Text;
                    string strAppendComment = myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text;
                    string strMillisecondsToWait = myListControlEntity1.Find(x => x.ID == "txtMillisecondsToWait").Text;
                    bool boolVariable = myListControlEntity1.Find(x => x.ID == "chkVariable").Checked;
                    bool boolCtrlKey = myListControlEntity1.Find(x => x.ID == "chkCtrlKey").Checked;
                    bool boolAltKey = myListControlEntity1.Find(x => x.ID == "chkAltKey").Checked;
                    bool boolShiftKey = myListControlEntity1.Find(x => x.ID == "chkShiftKey").Checked;
                    myActions.SetValueByKey("ScriptGeneratorDefaultMilliseconds", strMillisecondsToWait, "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorVariable", boolVariable.ToString(), "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorCtrlKey", boolCtrlKey.ToString(), "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorAltKey", boolAltKey.ToString(), "IdealAutomateDB");
                    myActions.SetValueByKey("ScriptGeneratorShiftKey", boolShiftKey.ToString(), "IdealAutomateDB");
                    if (strAppendComment.Length > 0) {
                        strAppendComment = " // " + strAppendComment;
                    }
                    string strGeneratedLine = "";
                    //   string strType = myListControlEntity1.Find(x => x.ID == "cbxType").SelectedValue;
                    if (!boolVariable && !boolCtrlKey && !boolAltKey && !boolShiftKey) {
                        if (strAppendComment == " // Maximize Visual Studio" && strTextToType == "%(f)x") {
                            strGeneratedLine = "myActions.TypeText(\"%(f)\"," + strMillisecondsToWait + ");" + strAppendComment;
                            strGeneratedLine += System.Environment.NewLine + "myActions.TypeText(\"x\"," + strMillisecondsToWait + ");";
                            myActions.PutEntityInClipboard(strGeneratedLine);
                            myActions.MessageBoxShow(strGeneratedLine);
                        } else if (strAppendComment == " // maximize internet explorer" && strTextToType == "%(\" \")") {
                            strGeneratedLine = "myActions.TypeText(\"%(\\\" \\\")," + strMillisecondsToWait + ");" + strAppendComment;
                            strGeneratedLine += System.Environment.NewLine + "myActions.TypeText(\"x\"," + strMillisecondsToWait + ");";
                            myActions.PutEntityInClipboard(strGeneratedLine);
                            myActions.MessageBoxShow(strGeneratedLine);
                        } else if (strAppendComment == " // close internet explorer" && strTextToType == "%(f)x") {
                            strGeneratedLine = "myActions.TypeText(\"%(f)," + strMillisecondsToWait + ");" + strAppendComment;
                            strGeneratedLine += System.Environment.NewLine + "myActions.TypeText(\"x\"," + strMillisecondsToWait + ");";
                            myActions.PutEntityInClipboard(strGeneratedLine);
                            myActions.MessageBoxShow(strGeneratedLine);
                        } else {
                            strGeneratedLine = "myActions.TypeText(\"" + strTextToType + "\"," + strMillisecondsToWait + ");" + strAppendComment;
                            myActions.PutEntityInClipboard(strGeneratedLine);
                            myActions.MessageBoxShow(strGeneratedLine);
                        }

                    }
                    if (boolVariable && !boolCtrlKey && !boolAltKey && !boolShiftKey) {
                        strGeneratedLine = "myActions.TypeText(" + strTextToType + "," + strMillisecondsToWait + ");" + strAppendComment;
                        myActions.PutEntityInClipboard(strGeneratedLine);
                        myActions.MessageBoxShow(strGeneratedLine);
                    }
                    if (boolCtrlKey && !boolVariable) {
                        strGeneratedLine = "myActions.TypeText(\"^(" + strTextToType + ")\"," + strMillisecondsToWait + ");" + strAppendComment;
                        myActions.PutEntityInClipboard(strGeneratedLine);
                        myActions.MessageBoxShow(strGeneratedLine);
                    }
                    if (boolCtrlKey && boolVariable) {
                        myActions.MessageBoxShow("Control Key and Variable is not valid");
                    }
                    if (boolAltKey && !boolVariable) {
                        strGeneratedLine = "myActions.TypeText(\"%(" + strTextToType + ")\"," + strMillisecondsToWait + ");" + strAppendComment;
                        myActions.PutEntityInClipboard(strGeneratedLine);
                        myActions.MessageBoxShow(strGeneratedLine);
                    }
                    if (boolAltKey && boolVariable) {
                        myActions.MessageBoxShow("Alt Key and Variable is not valid");
                    }

                    if (boolShiftKey && !boolVariable) {
                        strGeneratedLine = "myActions.TypeText(\"+(" + strTextToType + ")\"," + strMillisecondsToWait + ");" + strAppendComment;
                        myActions.PutEntityInClipboard(strGeneratedLine);
                        myActions.MessageBoxShow(strGeneratedLine);
                    }
                    if (boolShiftKey && boolVariable) {
                        myActions.MessageBoxShow("Shift Key and Variable is not valid");
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;

                default:
                    strFilePath = "FT/Trn/FtrnList.aspx";
                    break;
            }


            GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, intWindowTop, intWindowLeft);
            goto DisplayWindowAgain;

            myExit:
            Application.Current.Shutdown();
        }

        private static void GetSavedWindowPosition(Methods myActions, out int intWindowTop, out int intWindowLeft, out string strWindowTop, out string strWindowLeft) {
            strWindowLeft = myActions.GetValueByKey("WindowLeft", "IdealAutomateDB");
            strWindowTop = myActions.GetValueByKey("WindowTop", "IdealAutomateDB");
            Int32.TryParse(strWindowLeft, out intWindowLeft);
            Int32.TryParse(strWindowTop, out intWindowTop);
        }
    }
}