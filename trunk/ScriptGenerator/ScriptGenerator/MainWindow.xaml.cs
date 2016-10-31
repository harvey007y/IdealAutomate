﻿using System.Windows;
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

          strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700,500, 50, 850);

          if (strButtonPressed == "btnCancel") {
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 100, 850);
            goto DisplayWindowAgain;
          }

          if (strButtonPressed == "btnMinimizeVisualStudio") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%(\" \"n)";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "Minimize Visual Studio";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700,500, 50, 850);
          }

          if (strButtonPressed == "btnMaximizeVisualStudio") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%(f)x";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "Maximize Visual Studio";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700,500, 50, 850);
          }

          if (strButtonPressed == "btnIEAltD") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%(d)";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "Go to IE address bar and select it";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }


          if (strButtonPressed == "btnIEAltEnter") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%({ENTER})";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "Alt enter while in address bar opens new tab";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnIEF6") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{F6}";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "F6 is another way to highlight address bar in IE";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnIEMax") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%(\" \")";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "maximize internet explorer";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnIECloseCurrentTab") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "^(w)";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "close the current tab";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnIEGoToTopOfPage") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{HOME}";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "go to top of web page";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnIEClose") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "%(f)x";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "close internet explorer";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnCopy") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "^(c)";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "copy";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnCut") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "^(x)";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "cut";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnSelectAll") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "^(a)";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "select all";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnPaste") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "^(v)";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "paste";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          string txtSpecialKeysModifier = myListControlEntity1.Find(x => x.ID == "txtSpecialKeysModifier").Text;

          if (strButtonPressed == "btnFxx") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{F" + txtSpecialKeysModifier + "}";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (txtSpecialKeysModifier.Length > 0) {
            txtSpecialKeysModifier = " " + txtSpecialKeysModifier;
          }

          if (strButtonPressed == "btnDelete") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{DELETE" + txtSpecialKeysModifier + "}";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "delete";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnDown") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{DOWN" + txtSpecialKeysModifier + "}";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "down";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnEnd") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{END" + txtSpecialKeysModifier + "}";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "end";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnEnter") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{ENTER" + txtSpecialKeysModifier + "}";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "enter";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnEscape") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{ESCAPE" + txtSpecialKeysModifier + "}";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "escape";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnHome") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{HOME" + txtSpecialKeysModifier + "}";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "home";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnLeft") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{LEFT" + txtSpecialKeysModifier + "}";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "left";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnPGDN") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{PGDN" + txtSpecialKeysModifier + "}";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "page down";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnPGUP") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{PGUP" + txtSpecialKeysModifier + "}";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "page up";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnRight") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{RIGHT" + txtSpecialKeysModifier + "}";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "right";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnSpace") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{SPACE" + txtSpecialKeysModifier + "}";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "space";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnTAB") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{TAB" + txtSpecialKeysModifier + "}";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "tab";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
          }

          if (strButtonPressed == "btnUP") {
            myListControlEntity1.Find(x => x.ID == "txtTextToType").Text = "{UP" + txtSpecialKeysModifier + "}";
            myListControlEntity1.Find(x => x.ID == "txtAppendComment").Text = "up";
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity1, 700, 500, 50, 850);
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