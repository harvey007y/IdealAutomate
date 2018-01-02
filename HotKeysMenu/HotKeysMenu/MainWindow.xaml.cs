using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System;

namespace HotKeysMenu {
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
            if (strWindowTitle.StartsWith("HotKeysMenu")) {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            myActions.Sleep(1000);
            ResourceDictionary dictExecutables = new ResourceDictionary();
            int intRowCtr = 0;
            ControlEntity myControlEntity = new ControlEntity();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.ID = "lblHotKeysMenu";
            myControlEntity.Text = "Hot Keys Menu";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            ArrayList arrayListScriptInfo = myActions.ReadAppDirectoryKeyToArrayListGlobal("ScriptInfo");
            foreach (var item in arrayListScriptInfo) {
                string[] myCols = item.ToString().Split('^');
                // myCols[0] has scriptname after last hyphen
                // myCols[1] has shortcut key
                // myCols[5] has executable
                myActions.MessageBoxShow(myCols[5]);
            }

            string directory = AppDomain.CurrentDomain.BaseDirectory;
            directory = directory.Replace("\\bin\\Debug\\", "");
            int intLastSlashIndex = directory.LastIndexOf("\\");
            //string strScriptName = directory.Substring(intLastSlashIndex + 1);
            // string strScriptName = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
            ArrayList myArrayList = myActions.ReadPublicKeyToArrayList("Methods", directory);
            int intCol = 0;
            int intRow = 0;
            string strPreviousCategory = "";

            foreach (var item in myArrayList) {
                string[] myArrayFields = item.ToString().Split('^');
                {
                    intRow++;
                    if (intRow > 20) {
                        intRow = 1;
                        intCol++;
                    }
                    string strMethodName = myArrayFields[0];
                    string strCategory = myArrayFields[1];

                    if (strCategory != strPreviousCategory) {
                        if (intRow > 18) {
                            intRow = 1;
                            intCol++;
                        }
                        myControlEntity.ControlEntitySetDefaults();
                        myControlEntity.ControlType = ControlType.Label;
                        myControlEntity.ID = "lbl" + strCategory.Replace(" ", "");
                        myControlEntity.Text = strCategory;
                        myControlEntity.RowNumber = intRow;
                        myControlEntity.ColumnNumber = intCol;
                        myControlEntity.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
                        myControlEntity.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
                        myListControlEntity.Add(myControlEntity.CreateControlEntity());
                        strPreviousCategory = strCategory;
                        intRow++;
                    }
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
            }



            intRow++;
            if (intRow > 20) {
                intRow = 1;
                intCol++;
            }
            SqlConnection con = new SqlConnection("Server=(local)\\SQLEXPRESS;Initial Catalog=IdealAutomateDB;Integrated Security=SSPI");

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "SELECT ScriptName, Category1, Executable FROM Scripts where Category1 is not null and Executable is not null order by Category1, ScriptName ";
            cmd.Connection = con;
 

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
                    string strScriptName = reader.GetString(0).Replace(" ","");
                    string strCategory = reader.GetString(1);
                    string strExecutable = reader.GetString(2);

                    if (strCategory != strPreviousCategory) {
                        if (intRow > 18) {
                            intRow = 1;
                            intCol++;
                        }
                        myControlEntity.ControlEntitySetDefaults();
                        myControlEntity.ControlType = ControlType.Label;
                        myControlEntity.ID = "lbl" + strCategory.Replace(" ", "");
                        myControlEntity.Text = strCategory;
                        myControlEntity.RowNumber = intRow;
                        myControlEntity.ColumnNumber = intCol;
                        myControlEntity.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
                        myControlEntity.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
                        myListControlEntity.Add(myControlEntity.CreateControlEntity());
                        strPreviousCategory = strCategory;
                        intRow++;
                    }
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Button;
                    myControlEntity.ID = "btn" + strScriptName;
                    myControlEntity.Text = strScriptName;
                    dictExecutables.Add(myControlEntity.ID, strExecutable);
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





            DisplayWindow:
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 700, 700, 0, 0);
            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                goto myExit;
            }
            string strExecutable1 = dictExecutables[strButtonPressed].ToString();
            myActions.Run(strExecutable1, "");

            myActions.Sleep(1000);
            myExit:
            myActions.ScriptStartedUpdateStats();
            Application.Current.Shutdown();
        }
    }
}