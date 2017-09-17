using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Collections;

namespace CSharpSyntax {
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
            List<ComboBoxPair> cbp1 = new List<ComboBoxPair>();
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
            myControlEntity.Text = "C# Syntax";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            //myControlEntity.ControlEntitySetDefaults();
            //myControlEntity.ControlType = ControlType.Label;
            //myControlEntity.ID = "myLabelMethods";
            //myControlEntity.Text = "IdealAutomateCore Methods";
            //myControlEntity.RowNumber = 0;
            //myControlEntity.ColumnNumber = 0;
            //myListControlEntity.Add(myControlEntity.CreateControlEntity());

            //myControlEntity.ControlEntitySetDefaults();
            //myControlEntity.ControlType = ControlType.Label;
            //myControlEntity.ID = "myLabelCashManagement";
            //myControlEntity.Text = "More IdealAutomateCore Methods";
            //myControlEntity.RowNumber = 0;
            //myControlEntity.ColumnNumber = 1;
            //myListControlEntity.Add(myControlEntity.CreateControlEntity());
            ArrayList syntaxCategories = new ArrayList();
            syntaxCategories.Add("Program Structure");
            syntaxCategories.Add("Comments");
            syntaxCategories.Add("Data Types");
            syntaxCategories.Add("Constants");
            syntaxCategories.Add("Enumerations");
            syntaxCategories.Add("Operators");
            syntaxCategories.Add("Choices");
            syntaxCategories.Add("Loops");
            syntaxCategories.Add("Arrays");
            syntaxCategories.Add("Functions");
            syntaxCategories.Add("Strings");
            syntaxCategories.Add("Regular Expressions");
            syntaxCategories.Add("Exception Handling");
            syntaxCategories.Add("Namespaces");
            syntaxCategories.Add("Classes & Interfaces");
            syntaxCategories.Add("Constructors & Destructors");
            syntaxCategories.Add("Using Objects");
            syntaxCategories.Add("Structs");
            syntaxCategories.Add("Properties");
            syntaxCategories.Add("Generics");
            syntaxCategories.Add("Delegates & Lambda Expressions");
            syntaxCategories.Add("Extension Methods");
            syntaxCategories.Add("Events");
            syntaxCategories.Add("LINQ");
            syntaxCategories.Add("Collections");
            syntaxCategories.Add("Attributes");
            syntaxCategories.Add("Console I / O");
            syntaxCategories.Add("File I/ O");
            syntaxCategories.Sort();
            // myActions.WriteArrayListToAppDirectoryKey("SyntaxCategories", syntaxCategories);
            int intCol = 0;
            int intRow = 0;
            string strPreviousCategory = "";
            foreach (var item in syntaxCategories) {
                intRow++;
                if (intRow > 20) {
                    intRow = 1;
                    intCol++;
                }
                string strMethodName = item.ToString();
                string strCategory = item.ToString();

                if (strCategory != strPreviousCategory) {
                    if (intRow > 18) {
                        intRow = 1;
                        intCol++;
                    }
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "lbl" + strCategory.Replace(" ", "").Replace("&", "and").Replace("/", "");
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
                myControlEntity.ID = "myButton" + strMethodName.Replace(" ","").Replace("&", "and").Replace("/","");
                myControlEntity.Text = strMethodName;
                myControlEntity.RowNumber = intRow;
                myControlEntity.ColumnNumber = intCol;
                //    myControlEntity.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
                //   myControlEntity.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
                myListControlEntity.Add(myControlEntity.CreateControlEntity());
            }


            

            intRow++;
            if (intRow > 20) {
                intRow = 1;
                intCol++;
            }


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblCreateVSProject";
            myControlEntity.Text = "Create New VS Project";
            myControlEntity.RowNumber = intRow;
            myControlEntity.ColumnNumber = intCol;
            myControlEntity.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
            myControlEntity.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRow++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "myButtonCreateVSProject";
            myControlEntity.Text = "CreateNewVSProject";
            myControlEntity.RowNumber = intRow;
            myControlEntity.ColumnNumber = intCol;
            //    myControlEntity.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
            //   myControlEntity.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRow++;
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

            intRow++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblCopyVSProjectToIA";
            myControlEntity.Text = "Copy VS Project to IA";
            myControlEntity.RowNumber = intRow;
            myControlEntity.ColumnNumber = intCol;
            myControlEntity.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
            myControlEntity.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRow++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "myButtonCopyVSProjectToIA";
            myControlEntity.Text = "Copy VS Project to IA";
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
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
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
                case "myButtonArrays":
                    DisplayArraysWindow:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblArrays";
                    myControlEntity1.Text = "Arrays";
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
                    myControlEntity1.Text = "int[] nums = {1, 2, 3}; \r\n" +
"for (int i = 0; i < nums.Length; i++) \r\n" +
" Console.WriteLine(nums[i]); \r\n" +
" \r\n" +
" \r\n" +
"// 5 is the size of the array \r\n" +
"string[] names = new string[5]; \r\n" +
"names[0] = \"David\"; \r\n" +
"names[5] = \"Bobby\"; // Throws System.IndexOutOfRangeException  \r\n" +
" \r\n" +
"// Add two elements, keeping the existing values \r\n" +
"Array.Resize(ref names, 7); \r\n" +
"float[,] twoD = new float[rows, cols]; \r\n" +
"twoD[2,0] = 4.5f;  \r\n" +
"int[][] jagged = new int[3][] {  \r\n" +
" new int[5], new int[2], new int[3] }; \r\n" +
"jagged[0][4] = 5;  \r\n";

                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.Height = 250;
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }

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
                    if (myControlEntity1.SelectedValue == null) {
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

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    string strWebsiteURLx = myListControlEntity1.Find(x => x.ID == "txtWebsiteURL").Text;
                    string strUseNewTab = myListControlEntity1.Find(x => x.ID == "cbxUseNewTab").SelectedValue;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts);
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue);
                    myActions.SetValueByKey("ScriptGeneratorWebsiteURLx", strWebsiteURLx);
                    myActions.SetValueByKey("ScriptGeneratorUseNewTab", strUseNewTab);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayArraysWindow;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strWebsiteURLx == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter Website URL or select script variable; else press Cancel to Exit");
                            goto DisplayArraysWindow;
                        }
                        string strWebsiteURLToUse = "";
                        if (strWebsiteURLx.Trim() == "") {
                            strWebsiteURLToUse = strVariables;
                        } else {
                            strWebsiteURLToUse = "\"" + strWebsiteURLx.Trim() + "\"";
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = "myActions.IEGoToURL(myActions, " + strWebsiteURLToUse + ", " + strUseNewTab + ");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonWindowMultipleControls":
                    myActions.RunSync(myActions.GetValueByKey("SVNPath") + @"CodeGenTemplateParms\CodeGenTemplateParms\bin\debug\CodeGenTemplateParms.exe", "");
                    break;
                case "myButtonWindowMultipleControlsMinimized":
                    myActions.RunSync(myActions.GetValueByKey("SVNPath") + @"CodeGenTemplateParms\CodeGenTemplateParms\bin\debug\CodeGenTemplateParms.exe", "Minimized");
                    break;
                case "myButtonAttributes":
                    DisplayWindowAttributes:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    cbp1 = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblWindowShape";
                    myControlEntity1.Text = "Attributes";
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
                    myControlEntity1.Text = "// Attribute can be applied to anything \r\n" +
"public class IsTestedAttribute : Attribute \r\n" +
"{ \r\n" +
"} \r\n" +
" \r\n" +
"// Attribute can only be applied to classes or structs \r\n" +
"[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)] \r\n" +
"public class AuthorAttribute : Attribute { \r\n" +
" \r\n" +
"  public string Name { get; set; } \r\n" +
"  public int Version { get; set; }  \r\n" +
" \r\n" +
"  public AuthorAttribute(string name) { \r\n" +
"    Name = name; \r\n" +
"    Version = 0; \r\n" +
"  } \r\n" +
"} \r\n" +
" \r\n" +
"[Author(\"Sue\", Version = 3)] \r\n" +
"class Shape { \r\n" +
" \r\n" +
"  [IsTested] \r\n" +
"  void Move() { \r\n" +
"    // Do something... \r\n" +
"  } \r\n" +
"} \r\n";

                    myControlEntity1.ColumnSpan = 5;
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.ID = "lblShape";
                    myControlEntity1.Text = "Shape:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    cbp.Clear();
                    cbp.Add(new ComboBoxPair("Box", "Box"));
                    cbp.Add(new ComboBoxPair("Arrow", "Arrow"));
                    myControlEntity1.ListOfKeyValuePairs = cbp;
                    myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorShape");
                    if (myControlEntity1.SelectedValue == null) {
                        myControlEntity1.SelectedValue = "--Select Item ---";
                    }
                    myControlEntity1.ID = "cbxShape";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblOrientation";
                    myControlEntity1.Text = "Orientation:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    cbp1.Clear();
                    cbp1.Add(new ComboBoxPair("Left", "Left"));
                    cbp1.Add(new ComboBoxPair("Right", "Right"));
                    cbp1.Add(new ComboBoxPair("Up", "Up"));
                    cbp1.Add(new ComboBoxPair("Down", "Down"));

                    myControlEntity1.ListOfKeyValuePairs = cbp1;
                    myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorOrientation");
                    if (myControlEntity1.SelectedValue == null) {
                        myControlEntity1.SelectedValue = "--Select Item ---";
                    }
                    myControlEntity1.ID = "cbxOrientation";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblTitle";
                    myControlEntity1.Text = "Title:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtTitle";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorTitlex");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myControlEntity1.ColumnSpan = 5;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());









                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblContent";
                    myControlEntity1.Text = "Content:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtContent";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorContentx");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.Height = 100;
                    myControlEntity1.Multiline = true;
                    myControlEntity1.ColumnNumber = 1;
                    myControlEntity1.ColumnSpan = 5;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblTop";
                    myControlEntity1.Text = "Top:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtTop";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorTopx");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    intRowCtr++;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblLeft";
                    myControlEntity1.Text = "Left:";
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 0;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtLeft";
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorLeftx");
                    myControlEntity1.RowNumber = intRowCtr;
                    myControlEntity1.ColumnNumber = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);

                    string strTitlex = myListControlEntity1.Find(x => x.ID == "txtTitle").Text;
                    string strContentx = myListControlEntity1.Find(x => x.ID == "txtContent").Text;
                    string strTopx = myListControlEntity1.Find(x => x.ID == "txtTop").Text;
                    string strLeftx = myListControlEntity1.Find(x => x.ID == "txtLeft").Text;
                    string strShape = myListControlEntity1.Find(x => x.ID == "cbxShape").SelectedValue;
                    string strOrientation = myListControlEntity1.Find(x => x.ID == "cbxOrientation").SelectedValue;
                    myActions.SetValueByKey("ScriptGeneratorTitlex", strTitlex);
                    myActions.SetValueByKey("ScriptGeneratorContentx", strContentx);
                    myActions.SetValueByKey("ScriptGeneratorTopx", strTopx);
                    myActions.SetValueByKey("ScriptGeneratorLeftx", strLeftx);
                    myActions.SetValueByKey("ScriptGeneratorShape", strShape);
                    myActions.SetValueByKey("ScriptGeneratorOrientation", strOrientation);


                    if (strButtonPressed == "btnOkay") {

                        string strGeneratedLinex = "";

                        strGeneratedLinex = "myActions.WindowShape(\"" + strShape + "\", \"" + strOrientation + "\", \"" + strTitlex + "\", \"" + strContentx + "\"," + strTopx + ", " + strLeftx + ");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonChoices":
                    DisplayChoices:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblStopService";
                    myControlEntity1.Text = "Choices";
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
                    myControlEntity1.Text = "// Null-coalescing operator \r\n" +
"x = y ?? 5;  // if y != null then x = y, else x = 5 \r\n" +
"// Ternary/Conditional operator \r\n" +
"greeting = age < 20 ? \"What's up?\" : \"Hello\"; \r\n" +
"if (age < 20) \r\n" +
" greeting = \"What's up?\"; \r\n" +
"else \r\n" +
" greeting = \"Hello\";  \r\n" +
"// Multiple statements must be enclosed in {} \r\n" +
"if (x != 100 && y < 5) { \r\n" +
" x *= 5; \r\n" +
" y *= 2; \r\n" +
"} \r\n" +
" \r\n" +
" \r\n" +
"No need for _ or : since ; is used to terminate each statement. \r\n" +
" \r\n" +
" \r\n" +
"if (x > 5)  \r\n" +
" x *= y;  \r\n" +
"else if (x == 5 || y % 2 == 0)  \r\n" +
" x += y;  \r\n" +
"else if (x < 10)  \r\n" +
" x -= y;  \r\n" +
"else  \r\n" +
" x /= y; \r\n" +
" \r\n" +
" \r\n" +
"// Every case must end with break or goto case  \r\n" +
"switch (color){ // Must be integer or string \r\n" +
" case \"pink\": \r\n" +
" case \"red\": r++; break;  \r\n" +
" case \"blue\": b++; break; \r\n" +
" case \"green\": g++; break; \r\n" +
" default: other++; break;  // break necessary on default \r\n" +
"}  \r\n";

                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorServiceNamex");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables");
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

                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorTimeoutMilliseconds");

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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    strServiceNamex = myListControlEntity1.Find(x => x.ID == "txtServiceName").Text;
                    strTimeoutMilliseconds = myListControlEntity1.Find(x => x.ID == "txtTimeoutMilliseconds").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts);
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue);
                    myActions.SetValueByKey("ScriptGeneratorServiceNamex", strServiceNamex);
                    myActions.SetValueByKey("ScriptGeneratorTimeoutMilliseconds", strTimeoutMilliseconds);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayChoices;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strServiceNamex == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter Service Name or select script variable; else press Cancel to Exit");
                            goto DisplayChoices;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonClassesandInterfaces":
                    DisplayClassesandInterfaces:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblStartService";
                    myControlEntity1.Text = "Classes and Interfaces";
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
                    myControlEntity1.Text = "Access Modifiers  \r\n" +
"public \r\n" +
"private \r\n" +
"internal \r\n" +
"protected \r\n" +
"protected internal \r\n" +
"Class Modifiers  \r\n" +
"abstract \r\n" +
"sealed \r\n" +
"static \r\n" +
"Method Modifiers  \r\n" +
"abstract \r\n" +
"sealed \r\n" +
"static \r\n" +
"virtual \r\n" +
"No Module equivalent - just use static class \r\n" +
" \r\n" +
"// Partial classes \r\n" +
"partial class Team { \r\n" +
" ... \r\n" +
" protected string name; \r\n" +
" public virtual void DislpayName() { \r\n" +
"  Console.WriteLine(name); \r\n" +
"}  \r\n" +
" \r\n" +
"// Inheritance \r\n" +
"class FootballTeam : Team { \r\n" +
" ... \r\n" +
" public override void DislpayName() { \r\n" +
"  Console.WriteLine(\"** \" + name + \" **\"); \r\n" +
" } \r\n" +
"}  \r\n" +
" \r\n" +
"// Interface definition \r\n" +
"interface IAlarmClock { \r\n" +
" void Ring();  \r\n" +
" DateTime CurrentDateTime { get; set; } \r\n" +
"}  \r\n" +
"// Extending an interface \r\n" +
"interface IAlarmClock : IClock { \r\n" +
" ... \r\n" +
"}  \r\n" +
" \r\n" +
"// Interface implementation \r\n" +
"class WristWatch : IAlarmClock, ITimer { \r\n" +
" \r\n" +
" public void Ring() { \r\n" +
"  Console.WriteLine(\"Wake up!\"); \r\n" +
" } \r\n" +
" \r\n" +
" public DateTime TriggerDateTime { get; set; } \r\n" +
" ... \r\n" +
"}  \r\n";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorServiceNamex");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables");
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

                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorTimeoutMilliseconds");

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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    strServiceNamex = myListControlEntity1.Find(x => x.ID == "txtServiceName").Text;
                    strTimeoutMilliseconds = myListControlEntity1.Find(x => x.ID == "txtTimeoutMilliseconds").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts);
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue);
                    myActions.SetValueByKey("ScriptGeneratorServiceNamex", strServiceNamex);
                    myActions.SetValueByKey("ScriptGeneratorTimeoutMilliseconds", strTimeoutMilliseconds);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayClassesandInterfaces;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strServiceNamex == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter Service Name or select script variable; else press Cancel to Exit");
                            goto DisplayClassesandInterfaces;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonCollections":
                    DisplayCollectionsWindow:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblSleep";
                    myControlEntity1.Text = "Collections";
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
                    myControlEntity1.Text = "Popular classes in System.Collections (stored as Object) \r\n" +
"ArrayList \r\n" +
"Hashtable \r\n" +
"Queue \r\n" +
"Stack \r\n" +
"Popular classes in System.Collections.Generic (stored as type T) \r\n" +
"List<T> \r\n" +
"SortedList<TKey, TValue> \r\n" +
"Dictionary<TKey, TValue> \r\n" +
"Queue<T> \r\n" +
"Stack<T> \r\n" +
"Popular classes in System.Collections.Concurrent (thread safe) \r\n" +
"BlockingCollection<T> \r\n" +
"ConcurrentDictionary<TKey, TValue> \r\n" +
"ConcurrentQueue<T> \r\n" +
"ConcurrentStack<T>  \r\n" +
"No equivalent to Microsoft.VisualBasic.Collection  \r\n" +
" \r\n" +
"// Store ID and name \r\n" +
"var students = new Dictionary<int, string>  \r\n" +
"{ \r\n" +
" { 123, \"Bob\" }, \r\n" +
" { 444, \"Sue\" }, \r\n" +
" { 555, \"Jane\" } \r\n" +
"}; \r\n" +
" \r\n" +
"students.Add(987, \"Gary\"); \r\n" +
"Console.WriteLine(students[444]);  // Sue \r\n" +
" \r\n" +
"// Display all \r\n" +
"foreach (var stu in students) { \r\n" +
" Console.WriteLine(stu.Key + \" = \" + stu.Value); \r\n" +
"} \r\n" +
" \r\n" +
"// Method iterator for custom iteration over a collection \r\n" +
"static System.Collections.Generic.IEnumerable<int> OddNumbers(int lastNum) \r\n" +
"{ \r\n" +
" for (var num = 1; num <= lastNum; num++) \r\n" +
"  if (num % 2 == 1) \r\n" +
"   yield return num; \r\n" +
"} \r\n" +
" \r\n" +
"// 1 3 5 7 \r\n" +
"foreach (double num in OddNumbers(7)) \r\n" +
"{ \r\n" +
" Console.Write(num + \" \"); \r\n" +
"} \r\n";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorSleepMillisecondsToSleep");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables");
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    string strMillisecondsToSleep = myListControlEntity1.Find(x => x.ID == "txtMillisecondsToSleep").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts);
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue);
                    myActions.SetValueByKey("ScriptGeneratorSleepMillisecondsToSleep", strMillisecondsToSleep);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayCollectionsWindow;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strMillisecondsToSleep == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter MillisecondsToSleep or select script variable; else press Cancel to Exit");
                            goto DisplayCollectionsWindow;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonComments":
                    DisplayComments:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblShiftClick";
                    myControlEntity1.Text = "Comments";
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
                    myControlEntity1.Text = " \r\n" +
" \r\n" +
"REMSingle line only \r\n" +
" \r\n" +
"''' <summary>XML comments</summary>  \r\n" +
"// Single line \r\n" +
"/* Multiple \r\n" +
" line */ \r\n" +
"/// <summary>XML comments on single line</summary> \r\n" +
"/** <summary>XML comments on multiple lines</summary> */ \r\n";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorShiftClickmyArray");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables");
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
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts);
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue);
                    myActions.SetValueByKey("ScriptGeneratorShiftClickmyArray", strmyArray);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayComments;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strmyArray == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter myArray or select script variable; else press Cancel to Exit");
                            goto DisplayComments;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonConsoleIO":
                    DisplaySetValueByKeyWindow:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblSetValueByKey";
                    myControlEntity1.Text = "Console I/O";
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
                    myControlEntity1.Text = "Console.Write(\"What's your name? \"); \r\n" +
"string name = Console.ReadLine(); \r\n" +
"Console.Write(\"How old are you? \"); \r\n" +
"int age = Convert.ToInt32(Console.ReadLine()); \r\n" +
"Console.WriteLine(\"{0} is {1} years old.\", name, age); \r\n" +
"// or \r\n" +
"Console.WriteLine(name + \" is \" + age + \" years old.\");  \r\n" +
" \r\n" +
"int c = Console.Read(); // Read single char \r\n" +
"Console.WriteLine(c); // Prints 65 if user enters \"A\" \r\n";
                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorSetValueByKeyKey");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts1DefaultValue");
                    strScripts = myActions.GetValueByKey("Scripts1DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables1");
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorSetValueByKeyValue");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2");
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
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
                    myActions.SetValueByKey("Scripts1DefaultValue", strScripts1);
                    myActions.SetValueByKey("ScriptGeneratorVariables1", strVariables1Value);
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2);
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value);
                    myActions.SetValueByKey("ScriptGeneratorSetValueByKeyKey", strKey);
                    myActions.SetValueByKey("ScriptGeneratorSetValueByKeyValue", strValue);


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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonConstants":
                    DisplayConstants:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblRunSync";
                    myControlEntity1.Text = "Constants";
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
                    myControlEntity1.Text = "const int MAX_STUDENTS = 25;  \r\n" +
"// Can set to a const or var; may be initialized in a constructor  \r\n" +
"readonly float MIN_DIAMETER = 4.93f;  \r\n";

                    myControlEntity1.ColumnSpan = 4;
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorRunSyncExecutable");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts1DefaultValue");
                    strScripts = myActions.GetValueByKey("Scripts1DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables1");
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorRunSyncContent");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2");
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
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
                    myActions.SetValueByKey("Scripts1DefaultValue", strScripts1);
                    myActions.SetValueByKey("ScriptGeneratorVariables1", strVariables1Value);
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2);
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value);
                    myActions.SetValueByKey("ScriptGeneratorRunSyncExecutable", strExecutable);
                    myActions.SetValueByKey("ScriptGeneratorRunSyncContent", strContent);
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayConstants;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strExecutable == "" && (strVariables1 == "--Select Item ---" || strVariables1 == "")) {
                            myActions.MessageBoxShow("Please enter Executable or select script variable; else press Cancel to Exit");
                            goto DisplayConstants;
                        }
                        if (strContent == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Content or select script variable; else press Cancel to Exit");
                            goto DisplayConstants;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonConstructorsandDestructors":
                    DisplayConstructorsandDestructors:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblRun";
                    myControlEntity1.Text = "Constructors and Destructors";
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
                    myControlEntity1.Text = "class SuperHero : Person { \r\n" +
" \r\n" +
" private int powerLevel; \r\n" +
" private string name; \r\n" +
" \r\n" +
" \r\n" +
" // Default constructor \r\n" +
" public SuperHero() { \r\n" +
"  powerLevel = 0; \r\n" +
"  name = \"Super Bison\"; \r\n" +
" } \r\n" +
" \r\n" +
" public SuperHero(int powerLevel)  \r\n" +
"  : this(\"Super Bison\") {  // Call other constructor \r\n" +
" this.powerLevel = powerLevel; \r\n" +
" } \r\n" +
" \r\n" +
" public SuperHero(string name) \r\n" +
"  : base(name) {  // Call base classes' constructor \r\n" +
"  this.name = name; \r\n" +
" } \r\n" +
" \r\n" +
" static SuperHero() { \r\n" +
"  // Static constructor invoked before 1st instance is created \r\n" +
" } \r\n" +
" \r\n" +
" ~SuperHero() { \r\n" +
" // Destructor implicitly creates a Finalize method \r\n" +
" } \r\n" +
" \r\n" +
"} \r\n";

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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorRunExecutable");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts1DefaultValue");
                    strScripts = myActions.GetValueByKey("Scripts1DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables1");
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorRunContent");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2");
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
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
                    myActions.SetValueByKey("Scripts1DefaultValue", strScripts1);
                    myActions.SetValueByKey("ScriptGeneratorVariables1", strVariables1Value);
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2);
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value);
                    myActions.SetValueByKey("ScriptGeneratorRunExecutable", strExecutable);
                    myActions.SetValueByKey("ScriptGeneratorRunContent", strContent);
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayConstructorsandDestructors;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strExecutable == "" && (strVariables1 == "--Select Item ---" || strVariables1 == "")) {
                            myActions.MessageBoxShow("Please enter Executable or select script variable; else press Cancel to Exit");
                            goto DisplayConstructorsandDestructors;
                        }
                        if (strContent == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Content or select script variable; else press Cancel to Exit");
                            goto DisplayConstructorsandDestructors;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonDataTypes":
                    DisplayDataTypes:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblRightClick";
                    myControlEntity1.Text = "Data Types";
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
                    myControlEntity1.Text = "Value Types \r\n" +
"bool \r\n" +
"byte, sbyte \r\n" +
"char \r\n" +
"short, ushort, int, uint, long, ulong \r\n" +
"float,double \r\n" +
"decimal \r\n" +
"DateTime (not a built-in C#type) \r\n" +
"structs \r\n" +
"enumerations  \r\n" +
"Reference Types \r\n" +
"objects \r\n" +
"string \r\n" +
"arrays \r\n" +
"delegates  \r\n" +
"Initializing \r\n" +
"bool correct = true; \r\n" +
"byte b = 0x2A;  // hex \r\n" +
"object person = null; \r\n" +
"string name = \"Dwight\"; \r\n" +
"char grade = 'B'; \r\n" +
"DateTime today = DateTime.Parse(\"12/31/2010 12:15:00 PM\"); \r\n" +
"decimal amount = 35.99m; \r\n" +
"float gpa = 2.9f; \r\n" +
"double pi = 3.14159265;  // or 3.14159265D \r\n" +
"long lTotal = 123456L; \r\n" +
"short sTotal = 123; \r\n" +
"ushort usTotal = 123; \r\n" +
"uint uiTotal = 123;  // or 123U \r\n" +
"ulong ulTotal = 123;  // or 123UL \r\n" +
"Nullable Types \r\n" +
"int? x = null;  \r\n" +
"Anonymous Types \r\n" +
"var stu = new {Name = \"Sue\", Gpa = 3.5}; \r\n" +
"var stu2 = new {Name = \"Bob\", Gpa = 2.9};  // no Key equivalent  \r\n" +
" \r\n" +
"Implicitly Typed Local Variables \r\n" +
"var s = \"Hello!\"; \r\n" +
"var nums = new int[] { 1, 2, 3 }; \r\n" +
"var hero = new SuperHero() { Name = \"Batman\" };  \r\n" +
"Type Information \r\n" +
"int x; \r\n" +
"Console.WriteLine(x.GetType());      // System.Int32 \r\n" +
"Console.WriteLine(typeof(int));       // System.Int32  \r\n" +
"Console.WriteLine(x.GetType().Name);  // Int32  \r\n" +
" \r\n" +
"Circle c = new Circle(); \r\n" +
"isShape = c is Shape;  // true if c is a Shape  \r\n" +
" \r\n" +
"isSame = Object.ReferenceEquals(o1, o2)  // true if o1 and o2 reference same object  \r\n" +
"Type Conversion / Casting  \r\n" +
"float d = 3.5f;  \r\n" +
"i = Convert.ToInt32(d);   // Set to 4 (rounds)  \r\n" +
"int i = (int)d;   // set to 3 (truncates decimal)  \r\n" +
" \r\n" +
" \r\n" +
"Shape s = new Shape(); \r\n" +
"Circle c = s as Circle;  // Returns null if type cast fails \r\n" +
"c = (Circle)s;  // Throws InvalidCastException if type cast fails \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorRightClickmyArray");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables");
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
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts);
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue);
                    myActions.SetValueByKey("ScriptGeneratorRightClickmyArray", strmyArray);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayDataTypes;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strmyArray == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter myArray or select script variable; else press Cancel to Exit");
                            goto DisplayDataTypes;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonDelegatesandLambdaExpressions":
                    DisplayDelegatesandLambdaExpressions:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblRestartService";
                    myControlEntity1.Text = "Delegates and Lambda Expressions";
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
                    myControlEntity1.Text = "delegate void HelloDelegate(string s);  \r\n" +
"void SayHello(string s) { \r\n" +
" Console.WriteLine(\"Hello, \" + s); \r\n" +
"}  \r\n" +
"// C# 1.0 delegate syntax with named method \r\n" +
"HelloDelegate hello = new HelloDelegate(SayHello); \r\n" +
"hello(\"World\");  // Or hello.Invoke(\"World\"); \r\n" +
"// C# 2.0 delegate syntax with anonymous method \r\n" +
"HelloDelegate hello2 = delegate(string s) {  \r\n" +
" Console.WriteLine(\"Hello, \" + s);  \r\n" +
"}; \r\n" +
"hello2(\"World\");  \r\n" +
"// C# 3.0 delegate syntax with lambda expression \r\n" +
"HelloDelegate hello3 = s => { Console.WriteLine(\"Hello, \" + s + );\r\n" +
 "           }; \r\n" +
"hello3(\"World\");  \r\n" +
"// Use Func<in T, out TResult> delegate to call Uppercase \r\n" +
"Func<string, string> convert = Uppercase; \r\n" +
"Console.WriteLine(convert(\"test\"));  \r\n" +
"string Uppercase(string s) { \r\n" +
" return s.ToUpper(); \r\n" +
"}  \r\n" +
"// Declare and invoke Func using a lambda expression \r\n" +
"Console.WriteLine(new Func<int, int>(num => num + 1)(2)); \r\n" +
"// Pass lamba expression as an argument \r\n" +
"TestValues((x, y) => x % y == 0);  \r\n" +
"void TestValues(Func<int, int, bool> f) { \r\n" +
" if (f(8, 4)) \r\n" +
"  Console.WriteLine(\"true\"); \r\n" +
" else \r\n" +
"  Console.WriteLine(\"false\"); \r\n" +
"}  \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorServiceNamex");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables");
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

                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorTimeoutMilliseconds");

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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    strServiceNamex = myListControlEntity1.Find(x => x.ID == "txtServiceName").Text;
                    strTimeoutMilliseconds = myListControlEntity1.Find(x => x.ID == "txtTimeoutMilliseconds").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts);
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue);
                    myActions.SetValueByKey("ScriptGeneratorServiceNamex", strServiceNamex);
                    myActions.SetValueByKey("ScriptGeneratorTimeoutMilliseconds", strTimeoutMilliseconds);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayDelegatesandLambdaExpressions;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strServiceNamex == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter Service Name or select script variable; else press Cancel to Exit");
                            goto DisplayDelegatesandLambdaExpressions;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonEnumerations":
                    DisplayEnumerations:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp1 = new List<ComboBoxPair>();

                    intRowCtr = 0;
                    myListControlEntity1 = new List<ControlEntity>();
                    myControlEntity = new ControlEntity();
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.Text = "Enumerations";
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
                    myControlEntity1.Text = "enum Action {Start, Stop, Rewind, Forward}; \r\n" +
"enum Status {Flunk = 50, Pass = 70, Excel = 90}; \r\n" +
" \r\n" +
"Action a = Action.Stop; \r\n" +
"if (a != Action.Start) \r\n" +
" Console.WriteLine(a + \" is \" + (int)a);// \"Stop is 1\" \r\n" +
                    " \r\n" +
                    "Console.WriteLine((int) Status.Pass); // 70  \r\n" +
                    "Console.WriteLine(Status.Pass);   // Pass \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorPutWindowTitleInEntityResultValue");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2");
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    string strResultValue = myListControlEntity1.Find(x => x.ID == "txtResultValue").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2);
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value);
                    myActions.SetValueByKey("ScriptGeneratorPutWindowTitleInEntityResultValue", strResultValue);
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayEnumerations;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strResultValue == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Value or select script variable; else press Cancel to Exit");
                            goto DisplayEnumerations;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;

                    break;
                case "myButtonEvents":
                    DisplayEvents:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblGetValueByKey";
                    myControlEntity1.Text = "Events";
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
                    myControlEntity1.Text = "delegate void MsgArrivedEventHandler(string message);  \r\n" +
"event MsgArrivedEventHandler MsgArrivedEvent; \r\n" +
"// Delegates must be used with events in C# \r\n" +
" \r\n" +
" \r\n" +
"MsgArrivedEvent += new MsgArrivedEventHandler(My_MsgArrivedEventCallback); \r\n" +
"MsgArrivedEvent(\"Test message\"); // Throws exception if obj is null \r\n" +
"MsgArrivedEvent -= new MsgArrivedEventHandler(My_MsgArrivedEventCallback); \r\n" +
" \r\n" +
" \r\n" +
"using System.Windows.Forms; \r\n" +
"Button MyButton = new Button(); \r\n" +
"MyButton.Click += new System.EventHandler(MyButton_Click); \r\n" +
"void MyButton_Click(object sender, System.EventArgs e) {  \r\n" +
" MessageBox.Show(this, \"Button was clicked\", \"Info\",  \r\n" +
" MessageBoxButtons.OK, MessageBoxIcon.Information);  \r\n" +
"} \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorPutInternetExplorerTabURLContainingStringInEntityResultURL");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2");
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorPutInternetExplorerTabURLContainingStringInEntitySearchString");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts1DefaultValue");
                    strScripts = myActions.GetValueByKey("Scripts1DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables1");
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
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
                    myActions.SetValueByKey("Scripts1DefaultValue", strScripts1);
                    myActions.SetValueByKey("ScriptGeneratorVariables1", strVariables1Value);
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2);
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value);
                    myActions.SetValueByKey("ScriptGeneratorPutInternetExplorerTabURLContainingStringInEntitySearchString", strSearchString);
                    myActions.SetValueByKey("ScriptGeneratorPutInternetExplorerTabURLContainingStringInEntityResultURL", strResultURL);
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayEvents;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strSearchString == "" && (strVariables1 == "--Select Item ---" || strVariables1 == "")) {
                            myActions.MessageBoxShow("Please enter SearchString or select script variable; else press Cancel to Exit");
                            goto DisplayEvents;
                        }
                        if (strResultURL == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter ResultURL or select script variable; else press Cancel to Exit");
                            goto DisplayEvents;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonExceptionHandling":
                    DisplayExceptionHandling:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp1 = new List<ComboBoxPair>();

                    intRowCtr = 0;
                    myListControlEntity1 = new List<ControlEntity>();
                    myControlEntity = new ControlEntity();
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.Text = "Exception Handling";
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
                    myControlEntity1.Text = "// Throw an exception \r\n" +
"Exception up = new Exception(\"Something is really wrong.\");  \r\n" +
"throw up; // ha ha  \r\n" +
"// Catch an exception \r\n" +
"try { \r\n" +
" y = 0;  \r\n" +
" x = 10 / y;  \r\n" +
"}  \r\n" +
"catch (Exception ex) { // Argument is optional, no \"When\" keyword \r\n" +
" Console.WriteLine(ex.Message);  \r\n" +
"}  \r\n" +
"finally {  \r\n" +
" Microsoft.VisualBasic.Interaction.Beep();  \r\n" +
"}  \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorPutInternetExplorerTabTitleInEntityResultValue");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2");
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    strResultValue = myListControlEntity1.Find(x => x.ID == "txtResultValue").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2);
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value);
                    myActions.SetValueByKey("ScriptGeneratorPutInternetExplorerTabTitleInEntityResultValue", strResultValue);
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayExceptionHandling;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strResultValue == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Value or select script variable; else press Cancel to Exit");
                            goto DisplayExceptionHandling;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;

                    break;
                case "myButtonExtensionMethods":
                    DisplayExtensionMethods:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblPutEntityInClipboard";
                    myControlEntity1.Text = "Extension Methods";
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
                    myControlEntity1.Text = " \r\n" +
" \r\n" +
"Module StringExtensions \r\n" +
" <Extension()> \r\n" +
" Public Function VowelCount(ByVal s As String) As Integer \r\n" +
"  Return s.Count(Function(c) \"aeiou\".Contains(Char.ToLower(c))) \r\n" +
" End Function \r\n" +
"End Module \r\n" +
"' Using the extension method \r\n" +
"Console.WriteLine(\"This is a test\".VowelCount)  \r\n" +
"public static class StringExtensions { \r\n" +
" public static int VowelCount(this string s) { \r\n" +
"  return s.Count(c => \"aeiou\".Contains(Char.ToLower(c))); \r\n" +
" } \r\n" +
"}  \r\n" +
" \r\n" +
" \r\n" +
" \r\n" +
"// Using the extension method \r\n" +
"Console.WriteLine(\"This is a test\".VowelCount());  \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorPutEntityInClipboardmyEntity");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables");
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    string strMyEntity = myListControlEntity1.Find(x => x.ID == "txtmyEntity").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts);
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue);
                    myActions.SetValueByKey("ScriptGeneratorPutEntityInClipboardmyEntity", strMyEntity);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayExtensionMethods;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strMyEntity == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter myEntity or select script variable; else press Cancel to Exit");
                            goto DisplayExtensionMethods;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonFileIO":
                    DisplayFileIO:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp1 = new List<ComboBoxPair>();

                    intRowCtr = 0;
                    myListControlEntity1 = new List<ControlEntity>();
                    myControlEntity = new ControlEntity();
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.Text = "File I\\O";
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
                    myControlEntity1.Text = "using System.IO; \r\n" +
"// Write out to text file \r\n" +
"StreamWriter writer = File.CreateText(\"c:\\\\myfile.txt\");  \r\n" +
"writer.WriteLine(\"Out to file.\");  \r\n" +
"writer.Close();  \r\n" +
"// Read all lines from text file \r\n" +
"StreamReader reader = File.OpenText(\"c:\\\\myfile.txt\");  \r\n" +
"string line = reader.ReadLine();  \r\n" +
"while (line != null) { \r\n" +
" Console.WriteLine(line);  \r\n" +
" line = reader.ReadLine();  \r\n" +
"}  \r\n" +
"reader.Close();  \r\n" +
"// Write out to binary file \r\n" +
"string str = \"Text data\";  \r\n" +
"int num = 123;  \r\n" +
"BinaryWriter binWriter = new BinaryWriter(File.OpenWrite(\"c:\\\\myfile.dat\"));  \r\n" +
"binWriter.Write(str);  \r\n" +
"binWriter.Write(num);  \r\n" +
"binWriter.Close();  \r\n" +
"// Read from binary file \r\n" +
"BinaryReader binReader = new BinaryReader(File.OpenRead(\"c:\\\\myfile.dat\"));  \r\n" +
"str = binReader.ReadString();  \r\n" +
"num = binReader.ReadInt32();  \r\n" +
"binReader.Close(); \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorPutCursorPositionResultValue");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2");
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    strResultValue = myListControlEntity1.Find(x => x.ID == "txtResultValue").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2);
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value);
                    myActions.SetValueByKey("ScriptGeneratorPutCursorPositionResultValue", strResultValue);
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayFileIO;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strResultValue == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Value or select script variable; else press Cancel to Exit");
                            goto DisplayFileIO;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;

                    break;
                case "myButtonFunctions":
                    DisplayFunctions:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp1 = new List<ComboBoxPair>();

                    intRowCtr = 0;
                    myListControlEntity1 = new List<ControlEntity>();
                    myControlEntity = new ControlEntity();
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.Text = "Functions";
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
                    myControlEntity1.Text = "// Pass by value (in, default), reference (in/out), andreference (out) \r\n" +
"void TestFunc(int x, ref int y, out int z) { \r\n" +
" x++; \r\n" +
" y++; \r\n" +
" z = 5; \r\n" +
"}  \r\n" +
"int a = 1, b = 1, c; // c doesn't need initializing \r\n" +
"TestFunc(a, ref b, out c); \r\n" +
"Console.WriteLine(\"{0} {1} {2}\", a, b, c); // 1 2 5 \r\n" +
"// Accept variable number of arguments \r\n" +
"int Sum(params int[] nums) { \r\n" +
" int sum = 0; \r\n" +
" foreach (int i in nums) \r\n" +
" sum += i; \r\n" +
" return sum; \r\n" +
"} \r\n" +
"int total = Sum(4, 3, 2, 1); // returns 10 \r\n" +
"/* C# 4.0 supports optional parameters. Previous versions required function overloading. */  \r\n" +
"void SayHello(string name, string prefix = \"\") { \r\n" +
" Console.WriteLine(\"Greetings, \" + prefix + \" \" + name); \r\n" +
"}  \r\n" +
"SayHello(\"Strangelove\", \"Dr.\"); \r\n" +
"SayHello(\"Mom\");  \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorPutClipboardInEntityResultValue");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2");
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    strResultValue = myListControlEntity1.Find(x => x.ID == "txtResultValue").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2);
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value);
                    myActions.SetValueByKey("ScriptGeneratorPutClipboardInEntityResultValue", strResultValue);
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayFunctions;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strResultValue == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Value or select script variable; else press Cancel to Exit");
                            goto DisplayFunctions;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;

                    break;

                case "myButtonGenerics":
                    DisplayGenerics:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp1 = new List<ComboBoxPair>();

                    intRowCtr = 0;
                    myListControlEntity1 = new List<ControlEntity>();
                    myControlEntity = new ControlEntity();
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.Text = "Generics";
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
                    myControlEntity1.Text = "// Enforce accepted data type at compile-time  \r\n" +
"List<int> numbers = new List<int>(); \r\n" +
"numbers.Add(2); \r\n" +
"numbers.Add(4); \r\n" +
"DisplayList<int>(numbers); \r\n" +
"// Function can display any type of List  \r\n" +
"void DisplayList<T>(List<T> list) { \r\n" +
"  foreach (T item in list) \r\n" +
"    Console.WriteLine(item); \r\n" +
"} \r\n" +
" \r\n" +
"// Class works on any data type  \r\n" +
"class SillyList<T> { \r\n" +
"  private T[] list = new T[10]; \r\n" +
"  private Random rand = new Random(); \r\n" +
" \r\n" +
"  public void Add(T item) { \r\n" +
"    list[rand.Next(10)] = item; \r\n" +
"  } \r\n" +
" \r\n" +
"  public T GetItem() { \r\n" +
"    return list[rand.Next(10)]; \r\n" +
"  } \r\n" +
"} \r\n" +
"// Limit T to only types that implement IComparable \r\n" +
"T Maximum<T>(params T[] items) where T : IComparable<T> { \r\n" +
"  T max = items[0]; \r\n" +
"  foreach (T item in items) \r\n" +
"    if (item.CompareTo(max) > 0) \r\n" +
"      max = item; \r\n" +
"  return max; \r\n" +
"} \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorPutCaretPositionInArrayResultValue");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2");
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    strResultValue = myListControlEntity1.Find(x => x.ID == "txtResultValue").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2);
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value);
                    myActions.SetValueByKey("ScriptGeneratorPutCaretPositionInArrayResultValue", strResultValue);
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayGenerics;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strResultValue == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Value or select script variable; else press Cancel to Exit");
                            goto DisplayGenerics;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;

                    break;
                case "myButtonLINQ":
                    DisplayLINQ:
                    intRowCtr = 0;
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblPutAll";
                    myControlEntity1.Text = "LINQ";
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
                    myControlEntity1.Text = "int[] nums = { 5, 8, 2, 1, 6 }; \r\n" +
" \r\n" +
"// Get all numbers in the array above 4 \r\n" +
"var results = from n in nums \r\n" +
"        where n > 4 \r\n" +
"        select n; \r\n" +
" \r\n" +
"// Same thing using lamba expression \r\n" +
"results = nums.Where(n => n > 4); \r\n" +
" \r\n" +
"// Displays 5 8 6  \r\n" +
"foreach (int n in results) \r\n" +
"  Console.Write(n + \" \"); \r\n" +
" \r\n" +
" \r\n" +
"Console.WriteLine(results.Count());   // 3  \r\n" +
"Console.WriteLine(results.First());   // 5  \r\n" +
"Console.WriteLine(results.Last());   // 6  \r\n" +
"Console.WriteLine(results.Average());   // 6.33333  \r\n" +
" \r\n" +
"results = results.Intersect(new[] {5, 6, 7});   // 5 6  \r\n" +
"results = results.Concat(new[] {5, 1, 5});   // 5 6 5 1 5  \r\n" +
"results = results.Distinct();   // 5 6 1 \r\n" +
" \r\n" +
"Student[] Students = { \r\n" +
"  new Student{ Name = \"Bob\", Gpa = 3.5 }, \r\n" +
"  new Student{ Name = \"Sue\", Gpa = 4.0 }, \r\n" +
"  new Student{ Name = \"Joe\", Gpa = 1.9 } \r\n" +
"}; \r\n" +
" \r\n" +
"// Get a list of students ordered by Gpa with Gpa >= 3.0 \r\n" +
"var goodStudents = from s in Students \r\n" +
"      where s.Gpa >= 3.0 \r\n" +
"      orderby s.Gpa descending \r\n" +
"      select s; \r\n" +
" \r\n" +
"Console.WriteLine(goodStudents.First().Name);   // Sue \r\n";

                    myControlEntity1.Height = 500;
                    myControlEntity1.ColumnSpan = 4;
                    
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorHomeImage");
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorWorkImage");
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorSleep");
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorAttempts");
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorRelativeX");
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorRelativeY");
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorOccurrence");
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorTolerance");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorUseGrayScale");
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorPutAllResultMyArray");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2");
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
                    myActions.SetValueByKey("ScriptGeneratorHomeImage", strHomeImage);
                    myActions.SetValueByKey("ScriptGeneratorWorkImage", strWorkImage);
                    myActions.SetValueByKey("ScriptGeneratorSleep", strSleep);
                    myActions.SetValueByKey("ScriptGeneratorAttempts", strAttempts);
                    myActions.SetValueByKey("ScriptGeneratorRelativeX", strRelativeX);
                    myActions.SetValueByKey("ScriptGeneratorRelativeY", strRelativeY);
                    myActions.SetValueByKey("ScriptGeneratorOccurrence", strOccurrence);
                    myActions.SetValueByKey("ScriptGeneratorTolerance", strTolerance);
                    myActions.SetValueByKey("ScriptGeneratorUseGrayScale", strUseGrayScale);
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    string strResultMyArray = myListControlEntity1.Find(x => x.ID == "txtResultMyArray").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2);
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value);
                    myActions.SetValueByKey("ScriptGeneratorPutAllResultMyArray", strResultMyArray);
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayLINQ;
                    }
                    string strResultMyArrayToUse = "";
                    if (strButtonPressed == "btnOkay") {
                        if (strResultMyArray == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter ResultMyArray or select script variable; else press Cancel to Exit");
                            goto DisplayLINQ;
                        }

                        strResultMyArrayToUse = "";
                        if (strResultMyArray.Trim() == "") {
                            strResultMyArrayToUse = strVariables2;
                        } else {
                            strResultMyArrayToUse = strResultMyArray.Trim();
                        }

                    }
                    
                    if (strButtonPressed == "btnOkay") {


                        myActions.PutEntityInClipboard(sb.ToString());
                        myActions.MessageBoxShow(sb.ToString());
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonLoops":
                    DisplayLoops:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblPositionCursor";
                    myControlEntity1.Text = "Loops";
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
                    myControlEntity1.Text = "Pre-test Loops:   \r\n" +
"// no \"until\" keyword \r\n" +
"while (c < 10)  \r\n" +
" c++; \r\n" +
" \r\n" +
" \r\n" +
"for (c = 2; c <= 10; c += 2)  \r\n" +
" Console.WriteLine(c); \r\n" +
" \r\n" +
"Post-test Loop: \r\n" +
" \r\n" +
"do  \r\n" +
" c++;  \r\n" +
"while (c < 10); \r\n" +
" \r\n" +
" \r\n" +
"// Array or collection looping \r\n" +
"string[] names = {\"Fred\", \"Sue\", \"Barney\"}; \r\n" +
"foreach (string s in names) \r\n" +
" Console.WriteLine(s);  \r\n" +
" \r\n" +
"// Breaking out of loops \r\n" +
"int i = 0; \r\n" +
"while (true) { \r\n" +
" if (i == 5) \r\n" +
"  break; \r\n" +
" i++; \r\n" +
"}  \r\n" +
"// Continue to next iteration \r\n" +
"for (i = 0; i <= 4; i++) { \r\n" +
" if (i < 4) \r\n" +
"  continue; \r\n" +
" Console.WriteLine(i);  // Only prints 4 \r\n" +
"}  \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorPositionCursormyArray");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables");
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
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts);
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue);
                    myActions.SetValueByKey("ScriptGeneratorPositionCursormyArray", strmyArray);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayLoops;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strmyArray == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter myArray or select script variable; else press Cancel to Exit");
                            goto DisplayLoops;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonNamespaces":
                    DisplayNamespaces:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblMessageBoxShowWithYesNo";
                    myControlEntity1.Text = "Namespaces";
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
                    myControlEntity1.Text = "namespace Harding.Compsci.Graphics { \r\n" +
" ... \r\n" +
"}  \r\n" +
"// or  \r\n" +
"namespace Harding { \r\n" +
" namespace Compsci { \r\n" +
" namespace Graphics { \r\n" +
" ... \r\n" +
" } \r\n" +
" } \r\n" +
"}  \r\n" +
"using Harding.Compsci.Graphics;  \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorMessageBoxShowWithYesNoResultYesNo");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2");
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorMessageBoxShowWithYesNoMessage");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts1DefaultValue");
                    strScripts = myActions.GetValueByKey("Scripts1DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables1");
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
                    myActions.SetValueByKey("Scripts1DefaultValue", strScripts1);
                    myActions.SetValueByKey("ScriptGeneratorVariables1", strVariables1Value);
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2);
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value);
                    myActions.SetValueByKey("ScriptGeneratorMessageBoxShowWithYesNoMessage", strMessage);
                    myActions.SetValueByKey("ScriptGeneratorMessageBoxShowWithYesNoResultYesNo", strResultYesNo);
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayNamespaces;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strMessage == "" && (strVariables1 == "--Select Item ---" || strVariables1 == "")) {
                            myActions.MessageBoxShow("Please enter Message or select script variable; else press Cancel to Exit");
                            goto DisplayNamespaces;
                        }
                        if (strResultYesNo == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter ResultYesNo or select script variable; else press Cancel to Exit");
                            goto DisplayNamespaces;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonOperators":
                    DisplayOperators:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblMessageBoxShow";
                    myControlEntity1.Text = "Operators";
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
                    myControlEntity1.Text = "Comparison \r\n" +
"== < > <= >= != \r\n" +
"Arithmetic \r\n" +
"+ - * / \r\n" +
"% (mod) \r\n" +
"/ (integer division if both operands are ints) \r\n" +
"Math.Pow(x, y) \r\n" +
"Assignment \r\n" +
"= += -= *= /=  %= &= |= ^= <<= >>= ++ -- \r\n" +
"Bitwise \r\n" +
"&  |  ^  ~  <<  >> \r\n" +
"Logical \r\n" +
"&&  ||  &  |  ^  ! \r\n" +
"Note: && and||perform short-circuit logical evaluations \r\n" +
"String Concatenation \r\n" +
"+ \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorMessageBoxShowMessage");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables");
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    strMessage = myListControlEntity1.Find(x => x.ID == "txtMessage").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts);
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue);
                    myActions.SetValueByKey("ScriptGeneratorMessageBoxShowMessage", strMessage);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayOperators;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strMessage == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter Message or select script variable; else press Cancel to Exit");
                            goto DisplayOperators;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonProgramStructure":
                    DisplayProgramStructure:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblLeftClick";
                    myControlEntity1.Text = "Program Structure";
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
                    myControlEntity1.Text = "using System;  \r\n" +
" \r\n" +
"namespace Hello { \r\n" +
" public class HelloWorld { \r\n" +
" public static void Main(string[] args) { \r\n" +
" string name = \"C#\"; \r\n" +
" \r\n" +
"  // See if an argument was passed from the command line \r\n" +
"  if (args.Length == 1) \r\n" +
"  name = args[0]; \r\n" +
" \r\n" +
"  Console.WriteLine(\"Hello, \" + name + \"!\"); \r\n" +
" } \r\n" +
" } \r\n" +
"}  \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorLeftClickmyArray");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables");
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
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts);
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue);
                    myActions.SetValueByKey("ScriptGeneratorLeftClickmyArray", strmyArray);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayProgramStructure;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strmyArray == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter myArray or select script variable; else press Cancel to Exit");
                            goto DisplayProgramStructure;
                        }
                        strmyArrayToUse = "";
                        if (strmyArray.Trim() == "") {
                            strmyArrayToUse = strVariables;
                        } else {
                            strmyArrayToUse = strmyArray.Trim();
                        }
                        string strGeneratedLinex = "";

                        strGeneratedLinex = "myActions.LeftClick(" + strmyArrayToUse + ");";

                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonProperties":
                    DisplayProperties:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblKillAllProcessesByProcessName";
                    myControlEntity1.Text = "Properties";
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
                    myControlEntity1.Text = "// Auto-implemented properties \r\n" +
"public string Name { get; set; } \r\n" +
"public int Size { get; protected set; }   // Set default value in constructor  \r\n" +
"// Traditional property implementation \r\n" +
"private string name; \r\n" +
"public string Name { \r\n" +
" get { \r\n" +
"  return name; \r\n" +
" } \r\n" +
" set { \r\n" +
"  name = value; \r\n" +
" } \r\n" +
"}  \r\n" +
"// Read-only property \r\n" +
"private int powerLevel; \r\n" +
"public int PowerLevel { \r\n" +
" get { \r\n" +
"  return powerLevel; \r\n" +
" } \r\n" +
"}  \r\n" +
"// Write-only property \r\n" +
"private double height; \r\n" +
"public double Height { \r\n" +
" set { \r\n" +
"  height = value < 0 ? 0 : value; \r\n" +
" } \r\n" +
"}  \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorKillAllProcessesByProcessNameProcessName");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables");
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    string strProcessName = myListControlEntity1.Find(x => x.ID == "txtProcessName").Text;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts);
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue);
                    myActions.SetValueByKey("ScriptGeneratorKillAllProcessesByProcessNameProcessName", strProcessName);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayProperties;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strProcessName == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter Process Name or select script variable; else press Cancel to Exit");
                            goto DisplayProperties;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonRegularExpressions":
                    DisplayRegularExpressions:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblGetValueByKey";
                    myControlEntity1.Text = "Regular Expressions";
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
                    myControlEntity1.Text = "using System.Text.RegularExpressions; \r\n" +
"// Match a string pattern  \r\n" +
"Regex r = new Regex(@\"j[aeiou]h?. \\d:*\", RegexOptions.IgnoreCase |  \r\n" +
"    RegexOptions.Compiled); \r\n" +
"if (r.Match(\"John 3:16\").Success)  // true \r\n" +
"  Console.WriteLine(\"Match\");  \r\n" +
" \r\n" +
"// Find and remember all matching patterns \r\n" +
"string s = \"My number is 305-1881, not 305-1818.\"; \r\n" +
"Regex r = new Regex(\"(\\\\d+-\\\\d+)\"); \r\n" +
"// Matches 305-1881 and 305-1818 \r\n" +
"for (Match m = r.Match(s); m.Success; m = m.NextMatch())  \r\n" +
"  Console.WriteLine(\"Found number: \" + m.Groups[1] + \" at position \" +  \r\n" +
"    m.Groups[1].Index);  \r\n" +
" \r\n" +
" \r\n" +
"// Remeber multiple parts of matched pattern \r\n" +
"Regex r = new Regex(\"@(\\d\\d):(\\d\\d) (am|pm)\"); \r\n" +
"Match m = r.Match(\"We left at 03:15 pm.\"); \r\n" +
"if (m.Success) { \r\n" +
"  Console.WriteLine(\"Hour: \" + m.Groups[1]);    // 03  \r\n" +
                    "  Console.WriteLine(\"Min: \" + m.Groups[2]);     // 15  \r\n" +
                    "  Console.WriteLine(\"Ending: \" + m.Groups[3]);  // pm  \r\n" +
                    "}  \r\n" +
                    "// Replace all occurrances of a pattern \r\n" +
                    "Regex r = new Regex(\"h\\\\w+?d\", RegexOptions.IgnoreCase); \r\n" +
                    "string s = r.Replace(\"I heard this was HARD!\", \"easy\"));  // I easy this was easy!  \r\n" +
                    "// Replace matched patterns \r\n" +
                    "string s = Regex.Replace(\"123 < 456\", @\"(\\d+) . (\\d+)\", \"$2 > $1\");  // 456 > 123  \r\n" +
                    "// Split a string based on a pattern \r\n" +
                    "string names = \"Michael, Dwight, Jim, Pam\"; \r\n" +
                    "Regex r = new Regex(@\",\\s*\"); \r\n" +
                    "string[] parts = r.Split(names);  // One name in each slot  \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorGetWindowTitlesByProcessNameWindowTitles");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2");
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorGetWindowTitlesByProcessNameProcessName");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts1DefaultValue");
                    strScripts = myActions.GetValueByKey("Scripts1DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables1");
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
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
                    myActions.SetValueByKey("Scripts1DefaultValue", strScripts1);
                    myActions.SetValueByKey("ScriptGeneratorVariables1", strVariables1Value);
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2);
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value);
                    myActions.SetValueByKey("ScriptGeneratorGetWindowTitlesByProcessNameProcessName", strKey);
                    myActions.SetValueByKey("ScriptGeneratorGetWindowTitlesByProcessNameWindowTitles", strValue);
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayRegularExpressions;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strKey == "" && (strVariables1 == "--Select Item ---" || strVariables1 == "")) {
                            myActions.MessageBoxShow("Please enter Key or select script variable; else press Cancel to Exit");
                            goto DisplayRegularExpressions;
                        }
                        if (strValue == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Value or select script variable; else press Cancel to Exit");
                            goto DisplayRegularExpressions;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonStrings":
                    DisplayStrings:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblGetValueByKey";
                    myControlEntity1.Text = "Strings";
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
                    myControlEntity1.Text = "Escape sequences \r\n" +
"\\r  // carriage-return \r\n" +
"\\n  // line-feed \r\n" +
"\\t  // tab \r\n" +
"\\\\  // backslash \r\n" +
"\\\"  // quote  \r\n" +
" \r\n" +
" \r\n" +
"// String concatenation \r\n" +
"string school = \"Harding\\t\"; \r\n" +
"school = school + \"University\"; // school is \"Harding (tab) University\"  \r\n" +
"school += \"University\";  // Same thing  \r\n" +
"// Chars \r\n" +
"char letter = school[0];  // letter is H  \r\n" +
"letter = 'Z';                // letter is Z  \r\n" +
"letter = Convert.ToChar(65);   // letter is A  \r\n" +
"letter = (char)65; // same thing  \r\n" +
"char[] word = school.ToCharArray(); // word holds Harding  \r\n" +
"// String literal  \r\n" +
"string filename = @\"c:\\temp\\x.dat\";  // Same as \"c:\\\\temp\\\\x.dat\"  \r\n" +
"// String comparison \r\n" +
"stringmascot = \"Bisons\";  \r\n" +
"if (mascot == \"Bisons\")  // true \r\n" +
"if (mascot.Equals(\"Bisons\")) // true \r\n" +
"if (mascot.ToUpper().Equals(\"BISONS\")) // true \r\n" +
"if (mascot.CompareTo(\"Bisons\") == 0) // true \r\n" +
"// String matching - No Like equivalent, use Regex  \r\n" +
" \r\n" +
"// Substring \r\n" +
"s = mascot.Substring(2, 3))   // son \r\n" +
"s = \"testing\".Substring(1, 3);  // est (no Mid)  \r\n" +
"// Replacement \r\n" +
"s = mascot.Replace(\"sons\", \"nomial\"))   // Binomial  \r\n" +
"// Split \r\n" +
"string names = \"Michael,Dwight,Jim,Pam\"; \r\n" +
"string[] parts = names.Split(\",\".ToCharArray());  // One name in each slot  \r\n" +
"// Date to string \r\n" +
"DateTime dt = new DateTime(1973, 10, 12); \r\n" +
"string s = dt.ToString(\"MMM dd, yyyy\");   // Oct 12, 1973  \r\n" +
"// int to string \r\n" +
"int x = 2; \r\n" +
"string y = x.ToString();   // y is \"2\"  \r\n" +
"// string to int \r\n" +
"int x = Convert.ToInt32(\"-5\");   // x is -5  \r\n" +
" \r\n" +
"// Mutable string  \r\n" +
"System.Text.StringBuilder buffer = new System.Text.StringBuilder(\"two \");  \r\n" +
"buffer.Append(\"three \");  \r\n" +
"buffer.Insert(0, \"one \");  \r\n" +
"buffer.Replace(\"two\", \"TWO\");  \r\n" +
"Console.WriteLine(buffer); // Prints \"one TWO three\" \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorGetValueByKeyValue");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2");
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorGetValueByKeyKey");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts1DefaultValue");
                    strScripts = myActions.GetValueByKey("Scripts1DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables1");
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
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
                    myActions.SetValueByKey("Scripts1DefaultValue", strScripts1);
                    myActions.SetValueByKey("ScriptGeneratorVariables1", strVariables1Value);
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2);
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value);
                    myActions.SetValueByKey("ScriptGeneratorGetValueByKeyKey", strKey);
                    myActions.SetValueByKey("ScriptGeneratorGetValueByKeyValue", strValue);
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayStrings;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strKey == "" && (strVariables1 == "--Select Item ---" || strVariables1 == "")) {
                            myActions.MessageBoxShow("Please enter Key or select script variable; else press Cancel to Exit");
                            goto DisplayStrings;
                        }
                        if (strValue == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Value or select script variable; else press Cancel to Exit");
                            goto DisplayStrings;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;
                case "myButtonStructs":
                    DisplayStructs:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp1 = new List<ComboBoxPair>();

                    intRowCtr = 0;
                    myListControlEntity1 = new List<ControlEntity>();
                    myControlEntity = new ControlEntity();
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.Text = "Structs";
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
                    myControlEntity1.Text = "struct Student { \r\n" +
" public string name; \r\n" +
" public float gpa; \r\n" +
" \r\n" +
" public Student(string name, float gpa) { \r\n" +
" this.name = name; \r\n" +
" this.gpa = gpa; \r\n" +
" } \r\n" +
"} \r\n" +
"Student stu = new Student(\"Bob\", 3.5f); \r\n" +
"Student stu2 = stu; \r\n" +
" \r\n" +
"stu2.name = \"Sue\"; \r\n" +
"Console.WriteLine(stu.name);  // Prints Bob \r\n" +
"Console.WriteLine(stu2.name);  // Prints Sue \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.Text = myActions.GetValueByKey("ScriptGeneratorGetActiveWindowTitleResultValue");
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("Scripts2DefaultValue");
                    strScripts2 = myActions.GetValueByKey("Scripts2DefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables2");
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
                    strScripts2 = myListControlEntity1.Find(x => x.ID == "Scripts2").SelectedValue;


                    if (myListControlEntity1.Find(x => x.ID == "Variables2") != null) {
                        strVariables2 = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedKey;
                        strVariables2Value = myListControlEntity1.Find(x => x.ID == "Variables2").SelectedValue;
                    }
                    strResultValue = myListControlEntity1.Find(x => x.ID == "txtResultValue").Text;
                    // string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("Scripts2DefaultValue", strScripts2);
                    myActions.SetValueByKey("ScriptGeneratorVariables2", strVariables2Value);
                    myActions.SetValueByKey("ScriptGeneratorGetActiveWindowTitleResultValue", strResultValue);
                    //   myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayStructs;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strResultValue == "" && (strVariables2 == "--Select Item ---" || strVariables2 == "")) {
                            myActions.MessageBoxShow("Please enter Value or select script variable; else press Cancel to Exit");
                            goto DisplayStructs;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;

                    break;
               
                case "myButtonUsingObjects":
                    DisplayUsingObjects:
                    myControlEntity1 = new ControlEntity();
                    myListControlEntity1 = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();
                    intRowCtr = 0;
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Heading;
                    myControlEntity1.ID = "lblActivateWindowByTitle";
                    myControlEntity1.Text = "Using Objects";
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
                    myControlEntity1.Text = "SuperHero hero = new SuperHero();  \r\n" +
" \r\n" +
" \r\n" +
" \r\n" +
"// No \"With\" but can use object initializers \r\n" +
"SuperHero hero = new SuperHero() { Name = \"SpamMan\", PowerLevel = 3 };  \r\n" +
" \r\n" +
"hero.Defend(\"Laura Jones\"); \r\n" +
"SuperHero.Rest(); // Calling static method \r\n" +
" \r\n" +
" \r\n" +
"SuperHero hero2 = hero; // Both reference the same object  \r\n" +
"hero2.Name = \"WormWoman\";  \r\n" +
"Console.WriteLine(hero.Name); // Prints WormWoman  \r\n" +
"hero = null ; // Free the object  \r\n" +
"if (hero == null) \r\n" +
" hero = new SuperHero(); \r\n" +
"Object obj = new SuperHero(); \r\n" +
"if (obj is SuperHero)  \r\n" +
" Console.WriteLine(\"Is a SuperHero object.\");  \r\n" +
"// Mark object for quick disposal \r\n" +
"using (StreamReader reader = File.OpenText(\"test.txt\")) { \r\n" +
" string line; \r\n" +
" while ((line = reader.ReadLine()) != null) \r\n" +
"  Console.WriteLine(line); \r\n" +
"}  \r\n";
                    myControlEntity1.Height = 500;
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
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue");
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
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables");
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
                    myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorShowOption");
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 600, 700, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    string strWindowTitlex = myListControlEntity1.Find(x => x.ID == "txtWindowTitle").Text;
                    string strShowOption = myListControlEntity1.Find(x => x.ID == "cbxShowOption").SelectedValue;
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts);
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue);
                    myActions.SetValueByKey("ScriptGeneratorWindowTitlex", strWindowTitlex);
                    myActions.SetValueByKey("ScriptGeneratorShowOption", strShowOption);

                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayUsingObjects;
                    }

                    if (strButtonPressed == "btnOkay") {
                        if (strWindowTitlex == "" && strVariables == "--Select Item ---") {
                            myActions.MessageBoxShow("Please enter Window Title or select script variable; else press Cancel to Exit");
                            goto DisplayUsingObjects;
                        }
                        string strWindowTitleToUse = "";
                        if (strWindowTitlex.Trim() == "") {
                            strWindowTitleToUse = strVariables;
                        } else {
                            strWindowTitleToUse = "\"" + strWindowTitlex.Trim() + "\"";
                        }
                        string strGeneratedLinex = "";
                        if (strShowOption == "--Select Item ---") {
                            strGeneratedLinex = "myActions.ActivateWindowByTitle(" + strWindowTitleToUse + ");";
                        } else {
                            strGeneratedLinex = "myActions.ActivateWindowByTitle(" + strWindowTitleToUse + "," + strShowOption + ");";
                        }
                        myActions.PutEntityInClipboard(strGeneratedLinex);
                        myActions.MessageBoxShow(strGeneratedLinex);
                    }
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;

                case "myButtonCreateVSProject":

                    myActions.RunSync(myActions.GetValueByKey("SVNPath") + @"CreateNewVSProjectForScript\CreateNewVSProjectForScript\bin\debug\CreateNewVSProjectForScript.exe", "");
                    break;

                case "myButtonDeclareAVariable":

                    myActions.RunSync(myActions.GetValueByKey("SVNPath") + @"DDLMaint\DDLMaint\bin\debug\DDLMaint.exe", "");
                    break;

                case "myButtonCopyVSProjectToIA":

                    myActions.RunSync(myActions.GetValueByKey("SVNPath") + @"CopyVSExecutableToIdealAutomate\CopyVSExecutableToIdealAutomate\bin\Debug\CopyVSExecutableToIdealAutomate.exe", "");
                    break;
                case "myButtonTypeText":
                    DisplayTypeText:
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
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblScripts";
                    myControlEntity1.Text = "Script:";
                    myControlEntity1.Width = 150;
                    myControlEntity1.RowNumber = 0;
                    myControlEntity1.ColumnNumber = 3;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.ComboBox;
                    myControlEntity1.ID = "Scripts";
                    myControlEntity1.Text = "Drop Down Items";
                    myControlEntity1.Width = 300;
                    myControlEntity1.RowNumber = 0;
                    myControlEntity1.ColumnNumber = 4;
                    myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue");
                    strScripts = myActions.GetValueByKey("ScriptsDefaultValue");
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    if (strScripts != "--Select Item ---") {
                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.Label;
                        myControlEntity1.ID = "lblVariable";
                        myControlEntity1.Text = "Variable:";
                        myControlEntity1.Width = 150;
                        myControlEntity1.RowNumber = 0;
                        myControlEntity1.ColumnNumber = 5;
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                        myControlEntity1.ControlEntitySetDefaults();
                        myControlEntity1.ControlType = ControlType.ComboBox;
                        myControlEntity1.ID = "Variables";
                        myControlEntity1.Text = "Drop Down Items";
                        myControlEntity1.Width = 300;
                        myControlEntity1.RowNumber = 0;
                        myControlEntity1.ColumnNumber = 6;
                        int intScripts = 0;
                        Int32.TryParse(strScripts, out intScripts);
                        myControlEntity1.ParentLkDDLNamesItemsInc = intScripts;
                        myControlEntity1.SelectedValue = myControlEntity1.SelectedValue = myActions.GetValueByKey("ScriptGeneratorVariables");
                        myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    }



                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblMilliSecondsToWait";
                    myControlEntity1.Text = "Milliseconds to Wait:";
                    myControlEntity1.RowNumber = 1;
                    myControlEntity1.ColumnNumber = 0;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    string strDefaultMilliseconds = myActions.GetValueByKey("ScriptGeneratorDefaultMilliseconds");
                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtMillisecondsToWait";
                    myControlEntity1.Text = strDefaultMilliseconds;
                    myControlEntity1.RowNumber = 1;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 1;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblAppendComment";
                    myControlEntity1.Text = "Append Comment (no slashes needed):";
                    myControlEntity1.RowNumber = 2;
                    myControlEntity1.ColumnNumber = 0;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtAppendComment";
                    myControlEntity1.Text = "";
                    myControlEntity1.RowNumber = 2;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());


                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDDLRefresh";
                    myControlEntity1.Text = "ComboBox Refresh";
                    myControlEntity1.RowNumber = 3;
                    myControlEntity1.ColumnNumber = 0;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.CheckBox;
                    myControlEntity1.ID = "chkCtrlKey";
                    myControlEntity1.Text = "Is Control Key Pressed?";
                    myControlEntity1.RowNumber = 4;
                    myControlEntity1.ColumnNumber = 0;
                    myControlEntity1.ColumnSpan = 2;
                    if (myActions.GetValueByKey("ScriptGeneratorCtrlKey") == "True") {
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
                    myControlEntity1.ColumnSpan = 2;
                    if (myActions.GetValueByKey("ScriptGeneratorAltKey") == "True") {
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
                    myControlEntity1.ColumnSpan = 2;
                    if (myActions.GetValueByKey("ScriptGeneratorShiftKey") == "True") {
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
                    myControlEntity1.ColumnSpan = 2;
                    myControlEntity1.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
                    myControlEntity1.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);

                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnMinimizeVisualStudio";
                    myControlEntity1.Text = "Minimize Visual Studio";
                    myControlEntity1.RowNumber = 8;
                    myControlEntity1.ColumnNumber = 0;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnMaximizeVisualStudio";
                    myControlEntity1.Text = "Maximize Visual Studio";
                    myControlEntity1.RowNumber = 9;
                    myControlEntity1.ColumnNumber = 0;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblIEShortCutKeys";
                    myControlEntity1.Text = "Internet Explorer Shortcut Keys:";
                    myControlEntity1.RowNumber = 10;
                    myControlEntity1.ColumnNumber = 0;
                    myControlEntity1.ColumnSpan = 2;
                    myControlEntity1.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
                    myControlEntity1.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnIEAltD";
                    myControlEntity1.Text = "Go To Address Bar and select it Alt-D";
                    myControlEntity1.RowNumber = 11;
                    myControlEntity1.ColumnNumber = 0;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnIEAltEnter";
                    myControlEntity1.Text = "Alt enter while in address bar opens new tab";
                    myControlEntity1.RowNumber = 12;
                    myControlEntity1.ColumnNumber = 0;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnIEF6";
                    myControlEntity1.Text = "F6 selects address bar in IE";
                    myControlEntity1.RowNumber = 13;
                    myControlEntity1.ColumnNumber = 0;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnIEMax";
                    myControlEntity1.Text = "Maximize IE";
                    myControlEntity1.RowNumber = 14;
                    myControlEntity1.ColumnNumber = 0;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnIECloseCurrentTab";
                    myControlEntity1.Text = "Close Current Tab";
                    myControlEntity1.RowNumber = 15;
                    myControlEntity1.ColumnNumber = 0;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnIEGoToTopOfPage";
                    myControlEntity1.Text = "HOME goes to top of page";
                    myControlEntity1.RowNumber = 16;
                    myControlEntity1.ColumnNumber = 0;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnIEClose";
                    myControlEntity1.Text = "Close IE";
                    myControlEntity1.RowNumber = 17;
                    myControlEntity1.ColumnNumber = 0;
                    myControlEntity1.ColumnSpan = 2;
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
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnCopy";
                    myControlEntity1.Text = "Ctrl-C Copy";
                    myControlEntity1.RowNumber = 4;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnCut";
                    myControlEntity1.Text = "Ctrl-x Cut";
                    myControlEntity1.RowNumber = 5;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnSelectAll";
                    myControlEntity1.Text = "Ctrl-a Select All";
                    myControlEntity1.RowNumber = 6;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnPaste";
                    myControlEntity1.Text = "Ctrl-v Paste";
                    myControlEntity1.RowNumber = 7;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblSpecialKeys";
                    myControlEntity1.Text = "Special Keys:";
                    myControlEntity1.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);
                    myControlEntity1.ForegroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.White.R, System.Drawing.Color.White.G, System.Drawing.Color.White.B);
                    myControlEntity1.RowNumber = 8;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDelete";
                    myControlEntity1.Text = "{DELETE}";
                    myControlEntity1.RowNumber = 9;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnDown";
                    myControlEntity1.Text = "{DOWN}";
                    myControlEntity1.RowNumber = 10;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnEnd";
                    myControlEntity1.Text = "{END}";
                    myControlEntity1.RowNumber = 11;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnEnter";
                    myControlEntity1.Text = "{ENTER}";
                    myControlEntity1.RowNumber = 12;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnEscape";
                    myControlEntity1.Text = "{ESCAPE}";
                    myControlEntity1.RowNumber = 13;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnFxx";
                    myControlEntity1.Text = "{Fxx}";
                    myControlEntity1.RowNumber = 14;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnHome";
                    myControlEntity1.Text = "{HOME}";
                    myControlEntity1.RowNumber = 15;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnLeft";
                    myControlEntity1.Text = "{LEFT}";
                    myControlEntity1.RowNumber = 16;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnPGDN";
                    myControlEntity1.Text = "{PGDN}";
                    myControlEntity1.RowNumber = 17;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnPGUP";
                    myControlEntity1.Text = "{PGUP}";
                    myControlEntity1.RowNumber = 18;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnRight";
                    myControlEntity1.Text = "{RIGHT}";
                    myControlEntity1.RowNumber = 19;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnSpace";
                    myControlEntity1.Text = "{SPACE}";
                    myControlEntity1.RowNumber = 20;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnTAB";
                    myControlEntity1.Text = "{TAB}";
                    myControlEntity1.RowNumber = 21;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Button;
                    myControlEntity1.ID = "btnUP";
                    myControlEntity1.Text = "{UP}";
                    myControlEntity1.RowNumber = 22;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.Label;
                    myControlEntity1.ID = "lblSpecialKeysModifier";
                    myControlEntity1.Text = "Special Keys Repeat Count/Func Modifier:";
                    myControlEntity1.RowNumber = 23;
                    myControlEntity1.ColumnNumber = 0;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());

                    myControlEntity1.ControlEntitySetDefaults();
                    myControlEntity1.ControlType = ControlType.TextBox;
                    myControlEntity1.ID = "txtSpecialKeysModifier";
                    myControlEntity1.Text = "";
                    myControlEntity1.RowNumber = 23;
                    myControlEntity1.ColumnNumber = 2;
                    myControlEntity1.ColumnSpan = 2;
                    myListControlEntity1.Add(myControlEntity1.CreateControlEntity());
                    GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    strScripts = myListControlEntity1.Find(x => x.ID == "Scripts").SelectedValue;
                    if (myListControlEntity1.Find(x => x.ID == "Variables") != null) {
                        strVariables = myListControlEntity1.Find(x => x.ID == "Variables").SelectedKey;
                        strVariablesValue = myListControlEntity1.Find(x => x.ID == "Variables").SelectedValue;
                    }
                    myActions.SetValueByKey("ScriptsDefaultValue", strScripts);
                    myActions.SetValueByKey("ScriptGeneratorVariables", strVariablesValue);
                    if (strButtonPressed == "btnDDLRefresh") {
                        goto DisplayTypeText;
                    }
                    if (strButtonPressed == "btnCancel") {
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                        goto DisplayWindowAgain;
                    }

                    if (strButtonPressed == "btnMinimizeVisualStudio") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%(\" \"n)";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "Minimize Visual Studio";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnMaximizeVisualStudio") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%(f)x";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "Maximize Visual Studio";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnIEAltD") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%(d)";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "Go to IE address bar and select it";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }


                    if (strButtonPressed == "btnIEAltEnter") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%({ENTER})";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "Alt enter while in address bar opens new tab";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnIEF6") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{F6}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "F6 is another way to highlight address bar in IE";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnIEMax") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%(\" \")";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "maximize internet explorer";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnIECloseCurrentTab") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "^(w)";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "close the current tab";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnIEGoToTopOfPage") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{HOME}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "go to top of web page";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnIEClose") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%(f)x";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "close internet explorer";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnCopy") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "^(c)";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "copy";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnCut") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "^(x)";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "cut";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnSelectAll") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "^(a)";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "select all";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnPaste") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "^(v)";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "paste";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    string txtSpecialKeysModifier = myListControlEntity1.Find(x => x.ID == "txtSpecialKeysModifier").Text;

                    if (strButtonPressed == "btnFxx") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{F" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (txtSpecialKeysModifier.Length > 0) {
                        txtSpecialKeysModifier = " " + txtSpecialKeysModifier;
                    }

                    if (strButtonPressed == "btnDelete") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{DELETE" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "delete";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnDown") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{DOWN" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "down";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnEnd") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{END" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "end";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnEnter") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{ENTER" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "enter";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnEscape") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{ESCAPE" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "escape";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnHome") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{HOME" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "home";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnLeft") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{LEFT" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "left";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnPGDN") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{PGDN" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "page down";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnPGUP") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{PGUP" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "page up";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnRight") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{RIGHT" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "right";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnSpace") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{SPACE" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "space";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnTAB") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{TAB" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "tab";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }

                    if (strButtonPressed == "btnUP") {
                        myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{UP" + txtSpecialKeysModifier + "}";
                        myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "up";
                        GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
                        strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 800, 900, intWindowTop, intWindowLeft);
                    }





                    //if (strButtonPressed == "btnOkay") {
                    //  strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, 100, 850);
                    //  goto DisplayWindowAgain;
                    //}
                    string strTextToType = myListControlEntity1.Find(x => x.ID == "txtTextToType").Text;
                    string strAppendComment = myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text;
                    string strMillisecondsToWait = myListControlEntity1.Find(x => x.ID == "txtMillisecondsToWait").Text;
                    bool boolCtrlKey = myListControlEntity1.Find(x => x.ID == "chkCtrlKey").Checked;
                    bool boolAltKey = myListControlEntity1.Find(x => x.ID == "chkAltKey").Checked;
                    bool boolShiftKey = myListControlEntity1.Find(x => x.ID == "chkShiftKey").Checked;
                    myActions.SetValueByKey("ScriptGeneratorDefaultMilliseconds", strMillisecondsToWait);
                    myActions.SetValueByKey("ScriptGeneratorCtrlKey", boolCtrlKey.ToString());
                    myActions.SetValueByKey("ScriptGeneratorAltKey", boolAltKey.ToString());
                    myActions.SetValueByKey("ScriptGeneratorShiftKey", boolShiftKey.ToString());
                    if (strAppendComment.Length > 0) {
                        strAppendComment = " // " + strAppendComment;
                    }
                    string strGeneratedLine = "";
                    //   string strType = myListControlEntity1.Find(x => x.ID == "cbxType").SelectedValue;
                    bool boolVariable = false;

                    if (strTextToType.Trim() == "") {
                        boolVariable = true;
                    }
                    string strTextToTypeToUse = "";
                    if (strTextToType.Trim() == "") {
                        strTextToTypeToUse = strVariables;
                    } else {
                        strTextToTypeToUse = "\"" + strTextToType.Trim() + "\"";
                    }
                    if (!boolVariable && !boolCtrlKey && !boolAltKey && !boolShiftKey) {
                        if (strAppendComment == " // Maximize Visual Studio" && strTextToType == "%(f)x") {
                            strGeneratedLine = "myActions.TypeText(\"%(f)\"," + strMillisecondsToWait + ");" + strAppendComment;
                            strGeneratedLine += System.Environment.NewLine + "myActions.TypeText(\"x\"," + strMillisecondsToWait + ");";
                            myActions.PutEntityInClipboard(strGeneratedLine);
                            myActions.MessageBoxShow(strGeneratedLine);
                        } else if (strAppendComment == " // maximize internet explorer" && strTextToType == "%(\" \")") {
                            strGeneratedLine = "myActions.TypeText(\"%(\\\" \\\")\"," + strMillisecondsToWait + ");" + strAppendComment;
                            strGeneratedLine += System.Environment.NewLine + "myActions.TypeText(\"x\"," + strMillisecondsToWait + ");";
                            myActions.PutEntityInClipboard(strGeneratedLine);
                            myActions.MessageBoxShow(strGeneratedLine);
                        } else if (strAppendComment == " // close internet explorer" && strTextToType == "%(f)x") {
                            strGeneratedLine = "myActions.TypeText(\"%(f)," + strMillisecondsToWait + ");" + strAppendComment;
                            strGeneratedLine += System.Environment.NewLine + "myActions.TypeText(\"x\"," + strMillisecondsToWait + ");";
                            myActions.PutEntityInClipboard(strGeneratedLine);
                            myActions.MessageBoxShow(strGeneratedLine);
                        } else {
                            strGeneratedLine = "myActions.TypeText(" + strTextToTypeToUse + "," + strMillisecondsToWait + ");" + strAppendComment;
                            myActions.PutEntityInClipboard(strGeneratedLine);
                            myActions.MessageBoxShow(strGeneratedLine);
                        }

                    }
                    if (boolVariable && !boolCtrlKey && !boolAltKey && !boolShiftKey) {
                        strGeneratedLine = "myActions.TypeText(" + strTextToTypeToUse + "," + strMillisecondsToWait + ");" + strAppendComment;
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
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
                    goto DisplayWindowAgain;
                    break;

                default:
                    strFilePath = "FT/Trn/FtrnList.aspx";
                    break;
            }


            GetSavedWindowPosition(myActions, out intWindowTop, out intWindowLeft, out strWindowTop, out strWindowLeft);
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 650, 800, intWindowTop, intWindowLeft);
            goto DisplayWindowAgain;

            myExit:
            myActions.ScriptEndedSuccessfullyUpdateStats();
            Application.Current.Shutdown();
        }

        private static void GetSavedWindowPosition(Methods myActions, out int intWindowTop, out int intWindowLeft, out string strWindowTop, out string strWindowLeft) {
            strWindowLeft = myActions.GetValueByKey("WindowLeft");
            strWindowTop = myActions.GetValueByKey("WindowTop");
            Int32.TryParse(strWindowLeft, out intWindowLeft);
            Int32.TryParse(strWindowTop, out intWindowTop);
        }
    }
}
