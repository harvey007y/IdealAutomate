using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace DDLMaint {
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
            if (strWindowTitle.StartsWith("DDLMaint")) {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            myActions.Sleep(1000);
            DisplayMainMenu:
            ControlEntity myControlEntity = new ControlEntity();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.ID = "lblDDLMainMainMenu";
            myControlEntity.Text = "Drop Down Lists Maint Main Menu";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnDropDownLists";
            myControlEntity.Text = "Drop Down Lists";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 1;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnDropDownItems";
            myControlEntity.Text = "Drop Down Items";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 2;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnDropDownListHierarchy";
            myControlEntity.Text = "Drop Down List Hierarchy";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 3;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

            if (strButtonPressed == "btnDropDownLists") {
                myControlEntity = new ControlEntity();
                myListControlEntity = new List<ControlEntity>();
                cbp = new List<ComboBoxPair>();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.ID = "lblMaintainDDL";
                myControlEntity.Text = "Maintain DropDownLists";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
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
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblWidth";
                myControlEntity.Text = "Width";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtWidth";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblDefaultSelectedValue";
                myControlEntity.Text = "Default Selected Value";
                myControlEntity.RowNumber = 3;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtDefaultSelectedValue";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 3;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Button;
                myControlEntity.ID = "btnAddDDL";
                myControlEntity.Text = "Add DDL";
                myControlEntity.RowNumber = 4;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblBlankLine";
                myControlEntity.Text = " ";
                myControlEntity.RowNumber = 5;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());


                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblddlIds";
                myControlEntity.Text = "DDL IDs";
                myControlEntity.RowNumber = 6;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select DDL to Delete ---", "--Select DDL to Delete ---"));
                SqlConnection con = new SqlConnection("Server=(local)\\SQLEXPRESS;Initial Catalog=IdealAutomateDB;Integrated Security=SSPI");

                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "SELECT * FROM DDLNames order by ID";
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
                        int intInc = reader.GetInt32(0);
                        string strIDx = reader.GetString(1);
                        cbp.Add(new ComboBoxPair(strIDx, intInc.ToString()));
                    }
                    reader.Close();
                } finally {
                    con.Close();
                }
                myControlEntity.ListOfKeyValuePairs = cbp;
                myControlEntity.SelectedValue = "";
                myControlEntity.ID = "cbxddlIds";
                myControlEntity.RowNumber = 6;
                myControlEntity.ColumnNumber = 1;
                myControlEntity.SelectedValue = "--Select DDL to Delete ---";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Button;
                myControlEntity.ID = "btnDeleteDDL";
                myControlEntity.Text = "Delete DDL";
                myControlEntity.RowNumber = 7;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);
                string strID = myListControlEntity.Find(x => x.ID == "txtID").Text;
                string strWidth = myListControlEntity.Find(x => x.ID == "txtWidth").Text;
                string strDefaultSelectedValue = myListControlEntity.Find(x => x.ID == "txtDefaultSelectedValue").Text;
                string strddlIds = myListControlEntity.Find(x => x.ID == "cbxddlIds").SelectedValue;
                if (strButtonPressed == "btnAddDDL") {
                    SqlConnection thisConnection = new SqlConnection("server=.\\SQLEXPRESS;" + "integrated security=sspi;database=IdealAutomateDB");
                    //Create Command object        
                    SqlCommand nonqueryCommand = thisConnection.CreateCommand();
                    SqlCommand nonqueryCommand_delete = thisConnection.CreateCommand();
                    SqlCommand nonqueryCommand_insert_page_name = thisConnection.CreateCommand();
                    try {
                        // Open Connection
                        thisConnection.Open();
                        // Console.WriteLine("Connection Opened");


                        // Create INSERT statement with named parameters
                        nonqueryCommand.CommandText = "IF NOT EXISTS (" +
                        " SELECT * " +
                        " FROM DDLNames " +
                        " WHERE ID = @ID " +
                        " ) " +
                " BEGIN " +
              " INSERT  INTO DDLNames (ID, Width, DefaultValue) VALUES (@ID, @Width, @DefaultValue)" +
              " END ";

                        // Add Parameters to Command Parameters collection
                        nonqueryCommand.Parameters.Add("@ID", SqlDbType.VarChar, 500);
                        nonqueryCommand.Parameters["@ID"].Value = strID;
                        nonqueryCommand.Parameters.Add("@Width", SqlDbType.Int);
                        if (strWidth.Trim() == "") {
                            strWidth = "0";
                        }
                        nonqueryCommand.Parameters["@Width"].Value = strWidth;
                        nonqueryCommand.Parameters.Add("@DefaultValue", SqlDbType.VarChar, -1);
                        nonqueryCommand.Parameters["@DefaultValue"].Value = strDefaultSelectedValue;

                        // Prepare command for repeated execution
                        nonqueryCommand.Prepare();
                        int intRowsModified = nonqueryCommand.ExecuteNonQuery();
                        if (intRowsModified < 1) {
                            myActions.MessageBoxShow("Record not added - ID probably already exists; delete before adding to modify");
                        }


                    } catch (SqlException ex) {
                        // Display error
                        //Console.WriteLine("Error: " + ex.ToString());
                        myActions.MessageBoxShow("Error: " + ex.ToString());
                    }
                    goto DisplayMainMenu;
                }
                if (strButtonPressed == "btnDeleteDDL") {
                    SqlConnection thisConnection = new SqlConnection("server=.\\SQLEXPRESS;" + "integrated security=sspi;database=IdealAutomateDB");
                    //Create Command object        
                    SqlCommand nonqueryCommand = thisConnection.CreateCommand();
                    SqlCommand nonqueryCommand_delete = thisConnection.CreateCommand();
                    SqlCommand nonqueryCommand_insert_page_name = thisConnection.CreateCommand();
                    try {
                        // Open Connection
                        thisConnection.Open();
                        // Console.WriteLine("Connection Opened");


                        // Create INSERT statement with named parameters
                        nonqueryCommand.CommandText = "IF EXISTS (" +
                        " SELECT * " +
                        " FROM DDLNames " +
                        " WHERE INC = @INC " +
                        " ) " +
                " BEGIN " +
              " DELETE FROM DDLNames WHERE INC = @INC" +
              " END ";

                        // Add Parameters to Command Parameters collection

                        nonqueryCommand.Parameters.Add("@INC", SqlDbType.Int);
                        if (strWidth.Trim() == "") {
                            strWidth = "0";
                        }
                        int intDeleteInc = 0;
                        Int32.TryParse(strddlIds, out intDeleteInc);
                        nonqueryCommand.Parameters["@INC"].Value = intDeleteInc;


                        // Prepare command for repeated execution
                        nonqueryCommand.Prepare();
                        int intRowsModified = nonqueryCommand.ExecuteNonQuery();
                        if (intRowsModified < 1) {
                            myActions.MessageBoxShow("Record not deleted");
                        }


                    } catch (SqlException ex) {
                        // Display error
                        //Console.WriteLine("Error: " + ex.ToString());
                        myActions.MessageBoxShow("Error: " + ex.ToString());
                    }
                    goto DisplayMainMenu;
                }
            }

            if (strButtonPressed == "btnDropDownItems") {
                myControlEntity = new ControlEntity();
                myListControlEntity = new List<ControlEntity>();
                cbp = new List<ComboBoxPair>();
                List<ComboBoxPair> cbp1 = new List<ComboBoxPair>();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.ID = "lblMaintainDDLItems";
                myControlEntity.Text = "Maintain DropDownList Items";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());



                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblddlIds";
                myControlEntity.Text = "DDL IDs";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select DDL to Maintain ---", "--Select DDL to Maintain ---"));
                SqlConnection con = new SqlConnection("Server=(local)\\SQLEXPRESS;Initial Catalog=IdealAutomateDB;Integrated Security=SSPI");

                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "SELECT * FROM DDLNames order by ID";
                cmd.Connection = con;
                int intRow = 0;

                try {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    //(CommandBehavior.SingleRow)
                    while (reader.Read()) {
                        intRow++;
                        int intInc = reader.GetInt32(0);
                        string strIDx = reader.GetString(1);
                        cbp.Add(new ComboBoxPair(strIDx, intInc.ToString()));
                    }
                    reader.Close();
                } finally {
                    con.Close();
                }
                myControlEntity.ListOfKeyValuePairs = cbp;
                myControlEntity.SelectedValue = "";
                myControlEntity.ID = "cbxddlIds";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 1;
                myControlEntity.SelectedValue = "--Select DDL to Maintain ---";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Button;
                myControlEntity.ID = "btnAddItem";
                myControlEntity.Text = "Add Item";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Button;
                myControlEntity.ID = "btnDeleteItem";
                myControlEntity.Text = "Delete Item";
                myControlEntity.RowNumber = 3;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);
                string strddlIds = myListControlEntity.Find(x => x.ID == "cbxddlIds").SelectedValue;

                if (strButtonPressed == "btnAddItem") {
                    myControlEntity = new ControlEntity();
                    myListControlEntity = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "lblKey";
                    myControlEntity.Text = "Key";
                    myControlEntity.RowNumber = 2;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.TextBox;
                    myControlEntity.ID = "txtKey";
                    myControlEntity.Text = "";
                    myControlEntity.RowNumber = 2;
                    myControlEntity.ColumnNumber = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());


                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "lblValue";
                    myControlEntity.Text = "Value";
                    myControlEntity.RowNumber = 3;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.TextBox;
                    myControlEntity.ID = "txtValue";
                    myControlEntity.Text = "";
                    myControlEntity.RowNumber = 3;
                    myControlEntity.ColumnNumber = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Button;
                    myControlEntity.ID = "btnAddItem";
                    myControlEntity.Text = "Add Item";
                    myControlEntity.RowNumber = 4;
                    myControlEntity.ColumnNumber = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());
                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);
                    string strKey = myListControlEntity.Find(x => x.ID == "txtKey").Text;
                    string strValue = myListControlEntity.Find(x => x.ID == "txtValue").Text;

                }

                if (strButtonPressed == "btnDeleteItem") {
                    myControlEntity = new ControlEntity();
                    myListControlEntity = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "lblddlItems";
                    myControlEntity.Text = "DDL Items";
                    myControlEntity.RowNumber = 6;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.ComboBox;
                    cbp1.Clear();
                    cbp1.Add(new ComboBoxPair("--Select Item to Delete ---", "--Select Item to Delete ---"));
                    con = new SqlConnection("Server=(local)\\SQLEXPRESS;Initial Catalog=IdealAutomateDB;Integrated Security=SSPI");

                    cmd = new SqlCommand();

                    cmd.CommandText = "SELECT * FROM LkDDLNamesItems where DDLNamesInc = @DDLNamesInc";
                    cmd.Parameters.Add("@INC", SqlDbType.Int);
                    int intDeleteInc = 0;
                    Int32.TryParse(strddlIds, out intDeleteInc);
                    cmd.Parameters["@INC"].Value = intDeleteInc;
                    cmd.Connection = con;

                    intRow = 0;

                    try {
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        //(CommandBehavior.SingleRow)
                        while (reader.Read()) {
                            intRow++;
                            int intInc = reader.GetInt32(0);
                            string strIDx = reader.GetString(1);
                            cbp1.Add(new ComboBoxPair(strIDx, intInc.ToString()));
                        }
                        reader.Close();
                    } finally {
                        con.Close();
                    }
                    myControlEntity.ListOfKeyValuePairs = cbp1;
                    myControlEntity.SelectedValue = "";
                    myControlEntity.ID = "cbxddlItems";
                    myControlEntity.RowNumber = 6;
                    myControlEntity.ColumnNumber = 1;
                    myControlEntity.SelectedValue = "--Select Item to Delete ---";
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Button;
                    myControlEntity.ID = "btnDeleteItem";
                    myControlEntity.Text = "Delete Item";
                    myControlEntity.RowNumber = 7;
                    myControlEntity.ColumnNumber = 1;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);
                    string strddlItems = myListControlEntity.Find(x => x.ID == "cbxddlItems").SelectedValue;


                    SqlConnection thisConnection = new SqlConnection("server=.\\SQLEXPRESS;" + "integrated security=sspi;database=IdealAutomateDB");
                    //Create Command object        
                    SqlCommand nonqueryCommand = thisConnection.CreateCommand();
                    SqlCommand nonqueryCommand_delete = thisConnection.CreateCommand();
                    SqlCommand nonqueryCommand_insert_page_name = thisConnection.CreateCommand();
                    try {
                        // Open Connection
                        thisConnection.Open();
                        // Console.WriteLine("Connection Opened");


                        // Create INSERT statement with named parameters
                        nonqueryCommand.CommandText = "IF EXISTS (" +
                        " SELECT * " +
                        " FROM DDLItems " +
                        " WHERE INC = @INC " +
                        " ) " +
                " BEGIN " +
              " DELETE FROM DDLItems WHERE INC = @INC" +
              " END ";

                        // Add Parameters to Command Parameters collection

                        nonqueryCommand.Parameters.Add("@INC", SqlDbType.Int);
                         intDeleteInc = 0;
                        Int32.TryParse(strddlItems, out intDeleteInc);
                        nonqueryCommand.Parameters["@INC"].Value = intDeleteInc;


                        // Prepare command for repeated execution
                        nonqueryCommand.Prepare();
                        int intRowsModified = nonqueryCommand.ExecuteNonQuery();
                        if (intRowsModified < 1) {
                            myActions.MessageBoxShow("Record not deleted");
                        }


                    } catch (SqlException ex) {
                        // Display error
                        //Console.WriteLine("Error: " + ex.ToString());
                        myActions.MessageBoxShow("Error: " + ex.ToString());
                    }
                    goto DisplayMainMenu;
                }

            }

            if (strButtonPressed == "btnDropDownListHierarchy") {
                myControlEntity = new ControlEntity();
                myListControlEntity = new List<ControlEntity>();
                cbp = new List<ComboBoxPair>();
                List<ComboBoxPair> cbp1 = new List<ComboBoxPair>();
                List<ComboBoxPair> cbp2 = new List<ComboBoxPair>();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.ID = "lblMaintainDDLItems";
                myControlEntity.Text = "Maintain DropDownList Hierarchies";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());



                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblddlIds";
                myControlEntity.Text = "Parent DDL IDs";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select DDL for Parent ---", "--Select DDL for Parent ---"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                myControlEntity.SelectedValue = "";
                myControlEntity.ID = "cbxParentDdlIds";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 1;
                myControlEntity.SelectedValue = "--Select DDL for Parent ---";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblddlIds";
                myControlEntity.Text = "Child DDL IDs";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1.Clear();
                cbp1.Add(new ComboBoxPair("--Select DDL for Child ---", "--Select DDL for Child ---"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                myControlEntity.SelectedValue = "";
                myControlEntity.ID = "cbxChildDdlIds";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 1;
                myControlEntity.SelectedValue = "--Select DDL for Child ---";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());



                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Button;
                myControlEntity.ID = "btnAddHierarchy";
                myControlEntity.Text = "Add Hierarchy";
                myControlEntity.RowNumber = 4;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblBlankLine";
                myControlEntity.Text = " ";
                myControlEntity.RowNumber = 5;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());


                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblddlHierarchies";
                myControlEntity.Text = "DDL Hierarchies";
                myControlEntity.RowNumber = 6;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2.Clear();
                cbp2.Add(new ComboBoxPair("--Select Hierarchy to Delete ---", "--Select Hierarchy to Delete ---"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                myControlEntity.SelectedValue = "";
                myControlEntity.ID = "cbxHierarchyDll";
                myControlEntity.RowNumber = 6;
                myControlEntity.ColumnNumber = 1;
                myControlEntity.SelectedValue = "--Select Hierarchy to Delete ---";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Button;
                myControlEntity.ID = "btnDeleteHierarchy";
                myControlEntity.Text = "Delete Hierarchy";
                myControlEntity.RowNumber = 7;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                string strParentDdlIds = myListControlEntity.Find(x => x.ID == "cbxParentDdlIds").SelectedValue;
                string strChildDdlIds = myListControlEntity.Find(x => x.ID == "cbxChildDdlIds").SelectedValue;
                string strHierarchyDll = myListControlEntity.Find(x => x.ID == "cbxHierarchyDll").SelectedValue;

            }


            goto myExit;

            myExit:
            Application.Current.Shutdown();
        }
    }
}