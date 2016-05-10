using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;

namespace CodeGenTemplateParms
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

            InitializeComponent();
            this.Hide();

            string strWindowTitle = myActions.PutWindowTitleInEntity();
            if (strWindowTitle.StartsWith("CodeGenTemplateParms"))
            {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            myActions.Sleep(1000);
            string strIterations = "";
            string strIterationsID = "";
            string strIteratorID = "";
            string strApplicationPath = System.AppDomain.CurrentDomain.BaseDirectory;
            int intRowCtr = -1;
            string strOutFile = @"C:\Data\BlogPost.txt";
            StringBuilder sb = new StringBuilder(); // this is for creating the controls in the window
            StringBuilder sb2 = new StringBuilder(); // this is for retrieving stuff from window
            StringBuilder sb3 = new StringBuilder(); // this is for defining the template
            StringBuilder sb4 = new StringBuilder(); // this is for doing replacements to template
            sb.AppendLine("List<ControlEntity> myListControlEntity = new List<ControlEntity>();");
            sb.AppendLine("List<ComboBoxPair> cbp = new List<ComboBoxPair>();");

         AddControl:
            intRowCtr++;
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Code Gen Template Parms";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lblSelectControl";
            myControlEntity.Text = "Select Control To Add";
            myControlEntity.RowNumber = 1;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.ComboBox;
            myControlEntity.ID = "cbxSelectControl";
            myControlEntity.Text = "Hello World";
            //    Label = 0,
            //TextBox = 1,
            //ComboBox = 2,
            //Heading = 3,
            //CheckBox = 4
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();
            cbp.Add(new ComboBoxPair("Done Adding", "-1"));
            cbp.Add(new ComboBoxPair("Label", "0"));
            cbp.Add(new ComboBoxPair("TextBox", "1"));
            cbp.Add(new ComboBoxPair("ComboBox", "2"));
            cbp.Add(new ComboBoxPair("Heading", "3"));
            cbp.Add(new ComboBoxPair("Iterator", "4"));
            cbp.Add(new ComboBoxPair("Number of Iterations", "5"));
            cbp.Add(new ComboBoxPair("Template", "6"));
            myControlEntity.ListOfKeyValuePairs = cbp;
            myControlEntity.SelectedValue = "-1";
            myControlEntity.RowNumber = 1;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

            //string mySearchTerm = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
            string strSelectControl = myListControlEntity.Find(x => x.ID == "cbxSelectControl").SelectedValue;
            // label
            if (strSelectControl == "0")
            {
                myListControlEntity.Clear();
                myListControlEntity = new List<ControlEntity>();

                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Add Label Control";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblText";
                myControlEntity.Text = "Text";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtText";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblWidth";
                myControlEntity.Text = "Width";
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
                myControlEntity.ID = "lblID";
                myControlEntity.Text = "ID";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtID";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                string strText = myListControlEntity.Find(x => x.ID == "txtText").Text;
                string strWidth = myListControlEntity.Find(x => x.ID == "txtWidth").Text;
                string strID = myListControlEntity.Find(x => x.ID == "txtID").Text;
                string strInFile = strApplicationPath + "TemplateLabel.txt";
                // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";
               
                List<string> listOfSolvedProblems = new List<string>();
                List<string> listofRecs = new List<string>();
                string[] lineszz = System.IO.File.ReadAllLines(strInFile);



                    int intLineCount = lineszz.Count();
                    int intCtr = 0;
                    for (int i = 0; i < intLineCount; i++)
                    {
                        string line = lineszz[i];
                        line = line.Replace("&&ID", strID.Trim());
                        line = line.Replace("&&TEXT", strText.Trim());
                        line = line.Replace("&&ROW", intRowCtr.ToString());
                        if (strWidth != "") {
                          line = line.Replace("&&WIDTH", strWidth);
                        }

                        if (!line.Contains("&&WIDTH")) {
                          sb.AppendLine(line);
                        }
                    }

                goto AddControl;
            }

            // TextBox ----------------------------------------
            if (strSelectControl == "1")
            {
                myListControlEntity.Clear();
                myListControlEntity = new List<ControlEntity>();

                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Add TextBox Control";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());


                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblText";
                myControlEntity.Text = "Text";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtText";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblWidth";
                myControlEntity.Text = "Width";
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
                myControlEntity.ID = "lblID";
                myControlEntity.Text = "ID";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtID";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblHeight";
                myControlEntity.Text = "Height";
                myControlEntity.RowNumber = 3;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtHeight";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 3;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.CheckBox;
                myControlEntity.ID = "chkMultiline";
                myControlEntity.Text = "Multiline";
                myControlEntity.RowNumber = 4;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                string strText = myListControlEntity.Find(x => x.ID == "txtText").Text;
                string strWidth = myListControlEntity.Find(x => x.ID == "txtWidth").Text;
                string strID = myListControlEntity.Find(x => x.ID == "txtID").Text;
                string strHeight = myListControlEntity.Find(x => x.ID == "txtHeight").Text;
                bool boolMultiline = myListControlEntity.Find(x => x.ID == "chkMultiline").Checked;
                string strInFile = strApplicationPath + "TemplateTextBox.txt";
                // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";

                string[] lineszz = System.IO.File.ReadAllLines(strInFile);

                    int intLineCount = lineszz.Count();
                    int intCtr = 0;
                    for (int i = 0; i < intLineCount; i++)
                    {
                      string line = lineszz[i];
                      line = line.Replace("&&ID", strID.Trim());
                      line = line.Replace("&&TEXT", strText.Trim().Replace("\\r\\n","\" + System.Environment.NewLine + \""));
                      line = line.Replace("&&ROW", intRowCtr.ToString());
                      if (strWidth != "") {
                        line = line.Replace("&&WIDTH", strWidth);
                      }
                      if (strHeight != "") {
                        line = line.Replace("&&HEIGHT", strHeight);
                      }
                      if (boolMultiline) {
                        line = line.Replace("&&MULTILINE", boolMultiline.ToString());
                      }

                      if (!line.Contains("&&WIDTH") && !line.Contains("&&HEIGHT") && !line.Contains("&&MULTILINE")) {
                        sb.AppendLine(line);
                      }
                    }
                    sb2.AppendLine("string str" + strID + " = myListControlEntity.Find(x => x.ID == \"txt"+ strID +"\").Text;");
                    sb4.AppendLine("txtTemplateOut = txtTemplateOut.Replace(\"&&" + strID + "\",str" + strID + ");");

                goto AddControl;
            }

            // ComboBox ----------------------------------------
            if (strSelectControl == "2")
            {
                myListControlEntity.Clear();
                myListControlEntity = new List<ControlEntity>();

                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Add ComboBox Control";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblSelectedValue";
                myControlEntity.Text = "Default Selected Value";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtSelectedValue";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblWidth";
                myControlEntity.Text = "Width";
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
                myControlEntity.ID = "lblID";
                myControlEntity.Text = "ID";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtID";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblKeys";
                myControlEntity.Text = "Keys";
                myControlEntity.RowNumber = 3;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblValues";
                myControlEntity.Text = "Values";
                myControlEntity.RowNumber = 3;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtKey1";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 4;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtValue1";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 4;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtKey2";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 5;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtValue2";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 5;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtKey3";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 6;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtValue3";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 6;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtKey4";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 7;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtValue4";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 7;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtKey5";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 8;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtValue5";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 8;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                string strSelectedValue = myListControlEntity.Find(x => x.ID == "txtSelectedValue").Text;
                string strWidth = myListControlEntity.Find(x => x.ID == "txtWidth").Text;
                string strID = myListControlEntity.Find(x => x.ID == "txtID").Text;
                string strKey1 = myListControlEntity.Find(x => x.ID == "txtKey1").Text;
                string strValue1 = myListControlEntity.Find(x => x.ID == "txtValue1").Text;

                string strKey2 = myListControlEntity.Find(x => x.ID == "txtKey2").Text;
                string strValue2 = myListControlEntity.Find(x => x.ID == "txtValue2").Text;

                string strKey3 = myListControlEntity.Find(x => x.ID == "txtKey3").Text;
                string strValue3 = myListControlEntity.Find(x => x.ID == "txtValue3").Text;

                string strKey4 = myListControlEntity.Find(x => x.ID == "txtKey4").Text;
                string strValue4 = myListControlEntity.Find(x => x.ID == "txtValue4").Text;

                string strKey5 = myListControlEntity.Find(x => x.ID == "txtKey5").Text;
                string strValue5 = myListControlEntity.Find(x => x.ID == "txtValue5").Text;

                string strInFile = strApplicationPath + "TemplateComboBox.txt";
                // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";                
                string[] lineszz = System.IO.File.ReadAllLines(strInFile);
                    int intLineCount = lineszz.Count();
                    int intCtr = 0;
                    for (int i = 0; i < intLineCount; i++)
                    {
                      string line = lineszz[i];
                      line = line.Replace("&&ID", strID.Trim());
                      line = line.Replace("&&SELECTEDVALUE", strSelectedValue.Trim());
                      if (strKey1.Trim() != "") {
                        line = line.Replace("&&KEY1", strKey1);                      
                      }
                      if (strValue1.Trim() != "") {
                        line = line.Replace("&&VALUE1", strValue1);
                      }

                      if (strKey2.Trim() != "") {
                        line = line.Replace("&&KEY2", strKey2);
                      }
                      if (strValue1.Trim() != "") {
                        line = line.Replace("&&VALUE2", strValue2);
                      }

                      if (strKey3.Trim() != "") {
                        line = line.Replace("&&KEY3", strKey3);
                      }
                      if (strValue3.Trim() != "") {
                        line = line.Replace("&&VALUE3", strValue3);
                      }

                      if (strKey4.Trim() != "") {
                        line = line.Replace("&&KEY4", strKey4);
                      }
                      if (strValue4.Trim() != "") {
                        line = line.Replace("&&VALUE4", strValue4);
                      }

                      if (strKey5.Trim() != "") {
                        line = line.Replace("&&KEY5", strKey5);
                      }
                      if (strValue5.Trim() != "") {
                        line = line.Replace("&&VALUE5", strValue5);
                      }

                      line = line.Replace("&&ROW", intRowCtr.ToString());
                      if (strWidth != "") {
                        line = line.Replace("&&WIDTH", strWidth);
                      }

                      if (!line.Contains("&&WIDTH") && !line.Contains("&&SELECTEDVALUE") &&
                        !line.Contains("&&KEY1") && !line.Contains("&&VALUE1") &&
                        !line.Contains("&&KEY2") && !line.Contains("&&VALUE2") &&
                        !line.Contains("&&KEY3") && !line.Contains("&&VALUE3") &&
                        !line.Contains("&&KEY4") && !line.Contains("&&VALUE4") &&
                        !line.Contains("&&KEY5") && !line.Contains("&&VALUE5") 
                        ) {
                        sb.AppendLine(line);
                      }
                    }

                sb2.AppendLine("string str" + strID + " = myListControlEntity.Find(x => x.ID == \"cbx" + strID + "\").SelectedValue;");
                sb4.AppendLine("txtTemplateOut = txtTemplateOut.Replace(\"&&" + strID + "\",str" + strID + ");");

                goto AddControl;
            }

            // Heading -------------------------------------------------
            if (strSelectControl == "3")
            {
              myListControlEntity.Clear();
              myListControlEntity = new List<ControlEntity>();

              myControlEntity = new ControlEntity();
              myControlEntity.ControlEntitySetDefaults();
              myControlEntity.ControlType = ControlType.Heading;
              myControlEntity.Text = "Add Heading Control";
              myListControlEntity.Add(myControlEntity.CreateControlEntity());

              myControlEntity.ControlEntitySetDefaults();
              myControlEntity.ControlType = ControlType.Label;
              myControlEntity.ID = "lblText";
              myControlEntity.Text = "Text";
              myControlEntity.RowNumber = 0;
              myControlEntity.ColumnNumber = 0;
              myListControlEntity.Add(myControlEntity.CreateControlEntity());

              myControlEntity.ControlEntitySetDefaults();
              myControlEntity.ControlType = ControlType.TextBox;
              myControlEntity.ID = "txtText";
              myControlEntity.Text = "";
              myControlEntity.RowNumber = 0;
              myControlEntity.ColumnNumber = 1;
              myListControlEntity.Add(myControlEntity.CreateControlEntity());

              myControlEntity.ControlEntitySetDefaults();
              myControlEntity.ControlType = ControlType.Label;
              myControlEntity.ID = "lblWidth";
              myControlEntity.Text = "Width";
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
              myControlEntity.ID = "lblID";
              myControlEntity.Text = "ID";
              myControlEntity.RowNumber = 2;
              myControlEntity.ColumnNumber = 0;
              myListControlEntity.Add(myControlEntity.CreateControlEntity());

              myControlEntity.ControlEntitySetDefaults();
              myControlEntity.ControlType = ControlType.TextBox;
              myControlEntity.ID = "txtID";
              myControlEntity.Text = "";
              myControlEntity.RowNumber = 2;
              myControlEntity.ColumnNumber = 1;
              myListControlEntity.Add(myControlEntity.CreateControlEntity());

              myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

              string strText = myListControlEntity.Find(x => x.ID == "txtText").Text;
              string strWidth = myListControlEntity.Find(x => x.ID == "txtWidth").Text;
              string strID = myListControlEntity.Find(x => x.ID == "txtID").Text;
              string strInFile = strApplicationPath + "TemplateHeading.txt";
              // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";
              string[] lineszz = System.IO.File.ReadAllLines(strInFile);
              int intLineCount = lineszz.Count();
              int intCtr = 0;
              for (int i = 0; i < intLineCount; i++) {
                string line = lineszz[i];
                line = line.Replace("&&ID", strID.Trim());
                line = line.Replace("&&TEXT", strText.Trim());
                line = line.Replace("&&ROW", intRowCtr.ToString());
                if (strWidth != "") {
                  line = line.Replace("&&WIDTH", strWidth);
                }

                if (!line.Contains("&&WIDTH")) {
                  sb.AppendLine(line);
                }
              }

              goto AddControl;
            }

            // Iterator ----------------------------------------
            if (strSelectControl == "4")
            {
                myListControlEntity.Clear();
                myListControlEntity = new List<ControlEntity>();

                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Add Iterator Control";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());


                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblStart";
                myControlEntity.Text = "Start";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtStart";
                myControlEntity.Text = "0";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblIncrementBy";
                myControlEntity.Text = "Increment By";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtIncrementBy";
                myControlEntity.Text = "1";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblID";
                myControlEntity.Text = "ID";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtID";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                

                myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                string strStart = myListControlEntity.Find(x => x.ID == "txtStart").Text;
                string strIncrementBy = myListControlEntity.Find(x => x.ID == "txtIncrementBy").Text;
                strIteratorID = myListControlEntity.Find(x => x.ID == "txtID").Text;
                string strInFile = strApplicationPath + "TemplateIterator.txt";
            
                

                string[] lineszz = System.IO.File.ReadAllLines(strInFile);



                    int intLineCount = lineszz.Count();
                    int intCtr = 0;
                    for (int i = 0; i < intLineCount; i++)
                    {
                        string line = lineszz[i];
                    if (line.Contains("&&STARTROW"))
                    {
                        line = line.Replace("&&STARTROW", intRowCtr.ToString());
                        intRowCtr++;
                    }
                    if (line.Contains("&&INCREMENTBYROW"))
                    {
                        line = line.Replace("&&INCREMENTBYROW", intRowCtr.ToString());
                        intRowCtr++;
                    }
                    line = line.Replace("&&ROW", intRowCtr.ToString());
                    line = line.Replace("&&ID", strIteratorID.Trim());
                    line = line.Replace("&&START", strStart.Trim());
                    line = line.Replace("&&INCREMENTBY", strIncrementBy.Trim());
                    line = line.Replace("&&TEXT", strStart.Trim());

                    sb.AppendLine(line);
                    }

                sb2.AppendLine("string strStart" + strIteratorID.Trim() + " = myListControlEntity.Find(x => x.ID == \"txtStart" + strIteratorID.Trim() + "\").Text;");
                sb2.AppendLine("int intStart" + strIteratorID.Trim() + " = 0;");
                sb2.AppendLine("Int32.TryParse(strStart" + strIteratorID.Trim() + ",out intStart" + strIteratorID.Trim() + ");");
                sb2.AppendLine("string strIncrementBy"  + strIteratorID.Trim() + " = myListControlEntity.Find(x => x.ID == \"txtIncrementBy" + strIteratorID.Trim() + "\").Text;");
                sb2.AppendLine("int intIncrementBy" + strIteratorID.Trim() + " = 0;");
                sb2.AppendLine("Int32.TryParse(strIncrementBy" + strIteratorID.Trim() + ",out intIncrementBy" + strIteratorID.Trim() + ");");
                sb2.AppendLine("string strIterator" + strIteratorID.Trim() + " = myListControlEntity.Find(x => x.ID == \"txtIterator" + strIteratorID.Trim() + "\").Text;");
                sb4.AppendLine("txtTemplateOut = txtTemplateOut.Replace(\"&&" + strIteratorID + "\",intIterator.ToString());");
                goto AddControl;
            }

            // Number of Iterations ----------------------------------------
            if (strSelectControl == "5")
            {
                myListControlEntity.Clear();
                myListControlEntity = new List<ControlEntity>();

                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Add Number of Iterations Control";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());


                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblIterations";
                myControlEntity.Text = "Number of Iterations";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtIterations";
                myControlEntity.Text = "1";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblID";
                myControlEntity.Text = "ID";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtID";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());





                myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                strIterations = myListControlEntity.Find(x => x.ID == "txtIterations").Text;
                strIterationsID = myListControlEntity.Find(x => x.ID == "txtID").Text;
                string strInFile = strApplicationPath + "TemplateIterations.txt";
                // private string strInFile = @"C:\Data\LanguageXMLInput3.txt";
               
                string[] lineszz = System.IO.File.ReadAllLines(strInFile);



                    int intLineCount = lineszz.Count();
                    int intCtr = 0;
                    for (int i = 0; i < intLineCount; i++)
                    {
                        string line = lineszz[i];
                    line = line.Replace("&&ROW", intRowCtr.ToString());
                    line = line.Replace("&&ID", strIterationsID.Trim());                    
                    line = line.Replace("&&ITERATIONS", strIterations.Trim());
                    sb.AppendLine(line);
                    }

                sb2.AppendLine("string strIterations" + strIterationsID.Trim() + " = myListControlEntity.Find(x => x.ID == \"txtIterations" + strIterationsID.Trim() + "\").Text;");
                sb2.AppendLine("int intIterations" + strIterationsID.Trim() + " = 0;");
                sb2.AppendLine("Int32.TryParse(strIterations" + strIterationsID.Trim() + ",out intIterations" + strIterationsID.Trim() + ");");
                goto AddControl;
            }

            // Template ----------------------------------------
            if (strSelectControl == "6")
            {
                myListControlEntity.Clear();
                myListControlEntity = new List<ControlEntity>();

                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Add Template Control";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());


                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblTemplate";
                myControlEntity.Text = "Template";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtTemplate";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 0;
                myControlEntity.Multiline = true;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblID";
                myControlEntity.Text = "ID";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtID";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());





                myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                string strTemplate = myListControlEntity.Find(x => x.ID == "txtTemplate").Text;
                string strID = myListControlEntity.Find(x => x.ID == "txtID").Text;
                string strInFile = strApplicationPath + "TemplateTemplate.txt";

                string[] lineszz = System.IO.File.ReadAllLines(strInFile);


                    int intLineCount = lineszz.Count();
                    int intCtr = 0;
                    for (int i = 0; i < intLineCount; i++)
                    {
                        string line = lineszz[i];
                    line = line.Replace("&&ROW", intRowCtr.ToString());
                    line = line.Replace("&&ID", strID.Trim());
                    line = line.Replace("&&TEMPLATE", strTemplate.Trim());
                    sb.AppendLine(line);
                    }

                sb2.AppendLine("string strTemplate" + strID.Trim() + " = myListControlEntity.Find(x => x.ID == \"txtTemplate" + strID.Trim() + "\").Text;");
                sb3.AppendLine("string txtTemplateOut =  strTemplate" + strID.Trim() + ";");
                goto AddControl;
            }

            System.Windows.Forms.DialogResult myResult = myActions.MessageBoxShowWithYesNo("Click Yes to Confirm or No to Continue");
            if (myResult == System.Windows.Forms.DialogResult.No) {
              goto AddControl;
            }
          // Done --------------------
        
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutFile)) {
              file.WriteLine(" ControlEntity myControlEntity = new ControlEntity();");
              file.Write(sb.ToString());
              file.WriteLine("myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);");
              file.Write(sb2.ToString());
   
                // TODO: I have the template in txtTemplateOut and I need to
                // loop iteration times over the template starting at start iterator
                // incrementing iterator incrementby 
              file.WriteLine("int intIterator = intStart" + strIteratorID.Trim() + ";");
              file.WriteLine(" string strOutFile = @\"C:\\Data\\TemplateOut.txt\";");
              file.WriteLine("using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutFile)) {");
                file.WriteLine(" for (int i = 0; i < intIterations" + strIterationsID.Trim() + "; i++)");
                file.WriteLine("{");
                file.Write(sb3.ToString());
              file.Write(sb4.ToString());
              file.WriteLine("intIterator += intIncrementBy" + strIteratorID.Trim() + ";");
              file.WriteLine("file.WriteLine(\"\");");
              file.WriteLine("file.Write(txtTemplateOut);");
                file.WriteLine("}");
                file.WriteLine("}");
                file.WriteLine("string strExecutable = @\"C:\\Windows\\system32\\notepad.exe\";");
              file.WriteLine("string strContent = strOutFile;");
              file.WriteLine("Process.Start(strExecutable, string.Concat(\"\", strContent, \"\"));");
            }
             string strExecutable = @"C:\Windows\system32\notepad.exe";
            string strContent = strOutFile;
            Process.Start(strExecutable, string.Concat("", strContent, ""));

            //bool boolUseNewTab = myListControlEntity.Find(x => x.ID == "myCheckBox").Checked;
            //if (boolUseNewTab == true)
            //{
            //    List<string> myWindowTitles = myActions.GetWindowTitlesByProcessName("iexplore");
            //    myWindowTitles.RemoveAll(item => item == "");
            //    if (myWindowTitles.Count > 0)
            //    {
            //        myActions.ActivateWindowByTitle(myWindowTitles[0], (int)WindowShowEnum.SW_SHOWMAXIMIZED);
            //        myActions.TypeText("%(d)", 1500); // select address bar
            //        myActions.TypeText("{ESC}", 1500);
            //        myActions.TypeText("%({ENTER})", 1500); // Alt enter while in address bar opens new tab
            //        myActions.TypeText("%(d)", 1500);
            //        myActions.TypeText(myWebSite, 1500);
            //        myActions.TypeText("{ENTER}", 1500);
            //        myActions.TypeText("{ESC}", 1500);

            //    }
            //    else {
            //        myActions.Run("iexplore", myWebSite);

            //    }
            //}
            //else {
            //    myActions.Run("iexplore", myWebSite);
            //}

            //myActions.Sleep(1000);
            //myActions.TypeText(mySearchTerm, 500);
            //myActions.TypeText("{ENTER}", 500);


            goto myExit;
            myActions.RunSync(@"C:\Windows\Explorer.EXE", @"C:\SVN");
            myActions.TypeText("%(e)", 500);
            myActions.TypeText("a", 500);
            myActions.TypeText("^({UP 10})", 500);
            myActions.TypeText("^(\" \")", 500);
            myActions.TypeText("+({F10})", 500);
            ImageEntity myImage = new ImageEntity();

            if (boolRunningFromHome)
            {
                myImage.ImageFile = "Images\\imgSVNUpdate_Home.PNG";
            }
            else {
                myImage.ImageFile = "Images\\imgSVNUpdate.PNG";
            }
            myImage.Sleep = 200;
            myImage.Attempts = 5;
            myImage.RelativeX = 10;
            myImage.RelativeY = 10;

            int[,] myArray = myActions.PutAll(myImage);
            if (myArray.Length == 0)
            {
                myActions.MessageBoxShow("I could not find image of SVN Update");
            }
            // We found output completed and now want to copy the results
            // to notepad

            // Highlight the output completed line
            myActions.Sleep(1000);
            myActions.LeftClick(myArray);
            myImage = new ImageEntity();
            if (boolRunningFromHome)
            {
                myImage.ImageFile = "Images\\imgUpdateLogOK_Home.PNG";
            }
            else {
                myImage.ImageFile = "Images\\imgUpdateLogOK.PNG";
            }
            myImage.Sleep = 200;
            myImage.Attempts = 200;
            myImage.RelativeX = 10;
            myImage.RelativeY = 10;
            myArray = myActions.PutAll(myImage);
            if (myArray.Length == 0)
            {
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
            if (boolRunningFromHome)
            {
                myImage.ImageFile = "Images\\imgPatch2015_08_Home.PNG";
            }
            else {
                myImage.ImageFile = "Images\\imgPatch2015_08.PNG";
            }
            myImage.Sleep = 200;
            myImage.Attempts = 200;
            myImage.RelativeX = 30;
            myImage.RelativeY = 10;


            myArray = myActions.PutAll(myImage);
            if (myArray.Length == 0)
            {
                myActions.MessageBoxShow("I could not find image of " + myImage.ImageFile);
            }
            // We found output completed and now want to copy the results
            // to notepad

            // Highlight the output completed line
            myActions.RightClick(myArray);

            myImage = new ImageEntity();

            if (boolRunningFromHome)
            {
                myImage.ImageFile = "Images\\imgSVNUpdate_Home.PNG";
            }
            else {
                myImage.ImageFile = "Images\\imgSVNUpdate.PNG";
            }
            myImage.Sleep = 200;
            myImage.Attempts = 5;
            myImage.RelativeX = 10;
            myImage.RelativeY = 10;

            myArray = myActions.PutAll(myImage);
            if (myArray.Length == 0)
            {
                myActions.MessageBoxShow("I could not find image of SVN Update");
            }
            // We found output completed and now want to copy the results
            // to notepad

            // Highlight the output completed line
            myActions.Sleep(1000);
            myActions.LeftClick(myArray);
            myImage = new ImageEntity();
            if (boolRunningFromHome)
            {
                myImage.ImageFile = "Images\\imgUpdateLogOK_Home.PNG";
            }
            else {
                myImage.ImageFile = "Images\\imgUpdateLogOK.PNG";
            }
            myImage.Sleep = 200;
            myImage.Attempts = 200;
            myImage.RelativeX = 10;
            myImage.RelativeY = 10;
            myArray = myActions.PutAll(myImage);
            if (myArray.Length == 0)
            {
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
    }
}