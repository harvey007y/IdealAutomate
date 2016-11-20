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
            string strCountries = "";
            string strScriptsKey = "";
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
            myControlEntity.ControlType = ControlType.ComboBox;
            myControlEntity.ID = "Scripts";
            myControlEntity.Text = "Drop Down Items";
            myControlEntity.Width = 150;
            myControlEntity.RowNumber = 3;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.SelectedValue = myActions.GetValueByKey("ScriptsDefaultValue", "IdealAutomateDB");
            myControlEntity.SelectedKey = myActions.GetValueByKey("ScriptsDefaultKey", "IdealAutomateDB");
            strCountries = myControlEntity.SelectedValue;
            strScriptsKey = myControlEntity.SelectedKey;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            if (strCountries != "--Select Item ---") {
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                myControlEntity.ID = "Variables";
                myControlEntity.Text = "Drop Down Items";
                myControlEntity.Width = 150;
                myControlEntity.RowNumber = 4;
                myControlEntity.ColumnNumber = 0;
                int intCountries = 0;
                Int32.TryParse(strCountries, out intCountries);
                myControlEntity.ParentLkDDLNamesItemsInc = intCountries;
                myControlEntity.SelectedValue = "--Select Item ---";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());
            }

            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);
            strCountries = myListControlEntity.Find(x => x.ID == "Scripts").SelectedValue;
            strScriptsKey = myListControlEntity.Find(x => x.ID == "Scripts").SelectedKey;
            myActions.SetValueByKey("ScriptsDefaultValue", strCountries, "IdealAutomateDB");
            myActions.SetValueByKey("ScriptsDefaultKey", strScriptsKey, "IdealAutomateDB");
            if (strCountries != "--Select Item ---" && strButtonPressed == "btnOkay") {
                goto DisplayMainMenu;
            }

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
                myControlEntity.ID = "lblParentddlIds";
                myControlEntity.Text = "Optional Parent DDL IDs";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select Parent DDL to Maintain ---", "-1"));
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
                myControlEntity.ID = "cbxParentddlIds";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 1;
                myControlEntity.SelectedValue = "-1";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblddlIds";
                myControlEntity.Text = "DDL IDs";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1.Clear();
                cbp1.Add(new ComboBoxPair("--Select DDL to Maintain ---", "--Select DDL to Maintain ---"));
                con = new SqlConnection("Server=(local)\\SQLEXPRESS;Initial Catalog=IdealAutomateDB;Integrated Security=SSPI");

                cmd = new SqlCommand();

                cmd.CommandText = "SELECT * FROM DDLNames order by ID";
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
                myControlEntity.ID = "cbxddlIds";
                myControlEntity.RowNumber = 2;
                myControlEntity.ColumnNumber = 1;
                myControlEntity.SelectedValue = "--Select DDL to Maintain ---";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Button;
                myControlEntity.ID = "btnAddItem";
                myControlEntity.Text = "Add Item";
                myControlEntity.RowNumber = 3;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Button;
                myControlEntity.ID = "btnDeleteItem";
                myControlEntity.Text = "Delete Item";
                myControlEntity.RowNumber = 4;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());
                DisplayMenu:
                strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);
                string strddlIds = myListControlEntity.Find(x => x.ID == "cbxddlIds").SelectedValue;
                string strParentddlIds = myListControlEntity.Find(x => x.ID == "cbxParentddlIds").SelectedValue;
                if (strddlIds == "--Select DDL to Maintain ---" && (strButtonPressed == "btnAddItem" || strButtonPressed == "btnDeleteItem")) {
                    myActions.MessageBoxShow("Please select DDL to maintain");
                    goto DisplayMenu;
                }
                if (strButtonPressed == "btnAddItem") {
                    myControlEntity = new ControlEntity();
                    myListControlEntity = new List<ControlEntity>();
                    cbp = new List<ComboBoxPair>();

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "lblddlItems";
                    myControlEntity.Text = "Optional Parent DDL Items";
                    myControlEntity.RowNumber = 1;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    if (strParentddlIds != "-1") {

                        myControlEntity.ControlEntitySetDefaults();
                        myControlEntity.ControlType = ControlType.ComboBox;
                        cbp.Clear();
                        cbp.Add(new ComboBoxPair("--Select Optional Parent Item ---", "-1"));
                        con = new SqlConnection("Server=(local)\\SQLEXPRESS;Initial Catalog=IdealAutomateDB;Integrated Security=SSPI");

                        cmd = new SqlCommand();

                        cmd.CommandText = "SELECT lk.inc, i.listItemKey, i.ListItemValue FROM LkDDLNamesItems lk " +
    "join DDLNames n on n.inc = lk.DDLNamesInc " +
    "join DDLItems i on i.inc = lk.ddlItemsInc " +
    "where DDLNamesInc = @DDLNamesInc ";
                        cmd.Parameters.Add("@DDLNamesInc", SqlDbType.Int);
                        int intDeleteInc = 0;
                        Int32.TryParse(strParentddlIds, out intDeleteInc);
                        cmd.Parameters["@DDLNamesInc"].Value = intDeleteInc;
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
                                cbp.Add(new ComboBoxPair(strIDx, intInc.ToString()));
                            }
                            reader.Close();
                        } finally {
                            con.Close();
                        }
                        myControlEntity.ListOfKeyValuePairs = cbp;
                        myControlEntity.SelectedValue = "";
                        myControlEntity.ID = "cbxParentddlItems";
                        myControlEntity.RowNumber = 1;
                        myControlEntity.ColumnNumber = 1;
                        myControlEntity.SelectedValue = "-1";
                        myListControlEntity.Add(myControlEntity.CreateControlEntity());
                    }

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
                    string strParentddlItems = "-1";
                    if (strParentddlIds != "-1") {
                       strParentddlItems = myListControlEntity.Find(x => x.ID == "cbxParentddlItems").SelectedValue;
                    }
                    // TODO: 1. See if key value pair exists in DDLItems
                    // if it exists, save the inc
                    // if it does not exist, add it to DDLItems and get the inc
                    // add record to LK table with DDLNameInc and DDLItemInc if it does 
                    // not already exist
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
                        " FROM DDLItems " +
                        " WHERE ListItemKey = @ListItemKey " +
                         " AND ListItemValue = @ListItemValue " +
                        " ) " +
                " BEGIN " +
               " INSERT  INTO DDLItems (ListItemKey, ListItemValue) VALUES (@ListItemKey, @ListItemValue)" +
                  " END " +
               " DECLARE @DDLItemsInc int;" +
               " set @DDLItemsInc =   (SELECT Top 1 Inc " +
                        " FROM DDLItems " +
                        " WHERE ListItemKey = @ListItemKey " +
                         " AND ListItemValue = @ListItemValue); " +
                         "IF NOT EXISTS (" +
                        " SELECT * " +
                        " FROM LKDdlNamesItems " +
                        " WHERE DDLNamesInc = @DDLNamesInc " +
                         " AND DDLItemsInc = @DDLItemsInc " +
                        " ) " +
                         " BEGIN " +
               " INSERT  INTO LKDdlNamesItems (ParentLkDDLNamesItemsInc,DDLNamesInc, DDLItemsInc) VALUES (@ParentLkDDLNamesItemsInc, @DDLNamesInc, @DDLItemsInc)" +
             
                        " END ";

                        // Add Parameters to Command Parameters collection

                        nonqueryCommand.Parameters.Add("@ListItemKey", SqlDbType.VarChar,-1);
                        nonqueryCommand.Parameters["@ListItemKey"].Value = strKey;
                        nonqueryCommand.Parameters.Add("@ListItemValue", SqlDbType.VarChar, -1);
                        nonqueryCommand.Parameters["@ListItemValue"].Value = strValue;
                        nonqueryCommand.Parameters.Add("@DDLNamesInc", SqlDbType.Int);
                        int intDDLNamesInc = 0;
                        Int32.TryParse(strddlIds, out intDDLNamesInc);
                        nonqueryCommand.Parameters["@DDLNamesInc"].Value = intDDLNamesInc;
                        nonqueryCommand.Parameters.Add("@ParentLkDDLNamesItemsInc", SqlDbType.Int);
                        int intParentLkDDLNamesItemsInc = 0;
                        Int32.TryParse(strParentddlItems, out intParentLkDDLNamesItemsInc);
                        nonqueryCommand.Parameters["@ParentLkDDLNamesItemsInc"].Value = intParentLkDDLNamesItemsInc;


                        // Prepare command for repeated execution
                        nonqueryCommand.Prepare();
                        int intRowsModified = nonqueryCommand.ExecuteNonQuery();
                        if (intRowsModified < 1) {
                            myActions.MessageBoxShow("Record not added");
                        }


                    } catch (SqlException ex) {
                        // Display error
                        //Console.WriteLine("Error: " + ex.ToString());
                        myActions.MessageBoxShow("Error: " + ex.ToString());
                    }
                    goto DisplayMainMenu;
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

                    cmd.CommandText = "SELECT lk.inc, i.listItemKey, i.ListItemValue FROM LkDDLNamesItems lk " +
                    "join DDLNames n on n.inc = lk.DDLNamesInc " +
                    "join DDLItems i on i.inc = lk.ddlItemsInc " +
                    "where DDLNamesInc = @DDLNamesInc ";
                    cmd.Parameters.Add("@DDLNamesInc", SqlDbType.Int);
                    int intDeleteInc = 0;
                    Int32.TryParse(strddlIds, out intDeleteInc);
                    cmd.Parameters["@DDLNamesInc"].Value = intDeleteInc;
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
                        " FROM LkDDLNamesItems " +
                        " WHERE INC = @INC " +
                        " ) " +
                " BEGIN " +
                " DECLARE @DDLItemsInc int;" +
                " SET @DDLItemsInc = (  SELECT DDLItemsInc " +
                        " FROM LkDDLNamesItems " +
                        " WHERE INC = @INC); " +
              " DELETE FROM LKDDLNamesItems WHERE INC = @INC;" +
              "IF NOT EXISTS (" +
                        " SELECT * " +
                        " FROM LkDDLNamesItems " +
                        " WHERE DDLItemsInc = @DDLItemsInc " +
                        " ) " +
                " BEGIN " +
                " DELETE FROM DDLItems WHERE INC = @DDLItemsInc;" +
                 " END " +
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




            goto myExit;

            myExit:
            Application.Current.Shutdown();
        }
    }
}