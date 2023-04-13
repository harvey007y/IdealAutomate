using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Data.SqlClient;
using System.Data.Sql;
using System.IO;
using System.Configuration;
using Hardcodet.Wpf.Samples;



using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Hardcodet.Wpf.Samples.Pages
{
    public partial class SQLToGrid
    {
        
        private StringBuilder sb3 = new StringBuilder();
        private string strTable = "";
        private bool boolFirstTime = true;
        private bool boolDeleteNeeded;

        public SQLToGrid()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection thisConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 
            //Create Command object
            SqlCommand nonqueryCommand = thisConnection.CreateCommand();

            try
            {
                // Open Connection
                thisConnection.Open();
                //Console.WriteLine("Connection Opened");

                // Create INSERT statement with named parameters
                nonqueryCommand.CommandText = "INSERT  INTO ConnectionStrings (ConnectionString) VALUES (@ConnectionString)";

                // Add Parameters to Command Parameters collection
                nonqueryCommand.Parameters.Add("@ConnectionString", SqlDbType.VarChar, 500);


                // Prepare command for repeated execution
                nonqueryCommand.Prepare();

                // Data to be inserted

                nonqueryCommand.Parameters["@ConnectionString"].Value = txtConnectionString.Text;

                nonqueryCommand.ExecuteNonQuery();



            }
            catch (SqlException ex)
            {
                // Display error
                MessageBox.Show("Error Code 010 [exception=" + ex.Message + "]");
            }
            finally
            {
                // Close Connection
                thisConnection.Close();
                Console.WriteLine("Connection Closed");

            }



            //ConnectionStringsTableAdapter.Insert(TextBox1.Text);
            //ConnectionStringsTableAdapter.Update(IdealAutomaterDataSet);
            //ConnectionStringsTableAdapter.Fill(IdealAutomaterDataSet.ConnectionStrings);
            //cbConnectionStrings.Refresh()


            //BindingSource1.Insert(BindingSource1.Count, TextBox1.Text)
            DataSet ds = getData();

            DataTable dt = ds.Tables[0];

            this.cbConnectionStrings.ItemsSource = ((IListSource)dt).GetList();
            this.cbConnectionStrings.DisplayMemberPath = "ConnectionString";
            this.cbConnectionStrings.SelectedValuePath = "tID";
            MessageBox.Show("Connection String Added");
            //lblErrMsg.Content = "Connection String Added Successfully";
            //lblErrMsg.Visibility = System.Windows.Visibility.Visible;
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            DataSet ds = getData();

            DataTable dt = ds.Tables[0];

            this.cbConnectionStrings.ItemsSource = ((IListSource)dt).GetList();
            this.cbConnectionStrings.DisplayMemberPath = "ConnectionString";
            this.cbConnectionStrings.SelectedValuePath = "tID";

        }

        public DataSet getData()
        {
            SqlConnection thisConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 
            string sql = "SELECT * From ConnectionStrings order by connectionstring";
            SqlDataAdapter da = new SqlDataAdapter(sql, thisConnection);
            DataSet ds = new DataSet();
            da.Fill(ds, "ConnectionStrings");
            return ds;
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection thisConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 
            //Create Command object
            SqlCommand nonqueryCommand = thisConnection.CreateCommand();

            try
            {
                // Open Connection
                thisConnection.Open();
                //Console.WriteLine("Connection Opened");

                // Create INSERT statement with named parameters
                nonqueryCommand.CommandText = "DELETE From ConnectionStrings WHERE tID = @tID";

                // Add Parameters to Command Parameters collection
                nonqueryCommand.Parameters.Add("@tID", SqlDbType.Int);


                // Prepare command for repeated execution
                nonqueryCommand.Prepare();

                nonqueryCommand.Parameters["@tID"].Value = cbConnectionStrings.SelectedValue;

                nonqueryCommand.ExecuteNonQuery();



            }
            catch (SqlException ex)
            {
                // Display error
                MessageBox.Show("Error Code 011 [exception=" + ex.Message + "]");
            }
            finally
            {
                // Close Connection
                thisConnection.Close();
                Console.WriteLine("Connection Closed");

            }



            //ConnectionStringsTableAdapter.Insert(TextBox1.Text);
            //ConnectionStringsTableAdapter.Update(IdealAutomaterDataSet);
            //ConnectionStringsTableAdapter.Fill(IdealAutomaterDataSet.ConnectionStrings);
            //cbConnectionStrings.Refresh()


            //BindingSource1.Insert(BindingSource1.Count, TextBox1.Text)
            DataSet ds = getData();

            DataTable dt = ds.Tables[0];

            this.cbConnectionStrings.ItemsSource = ((IListSource)dt).GetList();
            this.cbConnectionStrings.DisplayMemberPath = "ConnectionString";
            this.cbConnectionStrings.SelectedValuePath = "tID";
        }

        private void btnGenerateCode_Click(object sender, RoutedEventArgs e)
        {
            sb3.Length = 0;
            txtGeneratedCode.Text = "";
            txtGeneratedASPX.Text = "";
            // STEPS:
            // 1. Validation
            // 2. Get Selected connection string; set up command and parms; read into sqldatareader
            // 3. Create datatable called dtSchemax to hold schema for selected columns only
            // 4. Get the schema for all columns in query including join fields and put in dtSchema
            // 5. Insert the info into ColumnProperties 
            // 
            // ************************************************************************************
            // 1. Validation
            // ************************************************************************************
            if (cbLanguage.Text == "")
            {
                MessageBox.Show("Please select Code-Behind Language");
                return;
            }
            if (txtSQL.Text.Trim() == "")
            {
                MessageBox.Show("Please enter SQL Statement or Stored Procedure Name");
                return;
            }
            if (txtParm1Name.Text.Trim() != "" && cbParm1Type.Text == "")
            {
                MessageBox.Show("Please select Type for Parm1 or remove Parm1");
                return;
            }
            if (txtParm2Name.Text.Trim() != "" && cbParm2Type.Text == "")
            {
                MessageBox.Show("Please select Type for Parm2 or remove Parm2");
                return;
            }
            if (txtParm3Name.Text.Trim() != "" && cbParm3Type.Text == "")
            {
                MessageBox.Show("Please select Type for Parm3 or remove Parm3");
                return;
            }
            if (txtParm4Name.Text.Trim() != "" && cbParm4Type.Text == "")
            {
                MessageBox.Show("Please select Type for Parm4 or remove Parm4");
                return;
            }
            if (txtParm5Name.Text.Trim() != "" && cbParm5Type.Text == "")
            {
                MessageBox.Show("Please select Type for Parm5 or remove Parm5");
                return;
            }

            boolDeleteNeeded = true;

            // ************************************************************************************
            // 2. Get Selected connection string; set up command and parms; read into sqldatareader
            // ************************************************************************************

            string queryString = txtSQL.Text;
            try
            {


                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = cbConnectionStrings.Text;
                    SqlCommand command = new SqlCommand(queryString, connection);
                    if (chkStoredProcedure.IsChecked == true)
                    {
                        command.CommandType = CommandType.StoredProcedure;
                    }
                    if (txtParm1Name.Text != "")
                    {
                        command.Parameters.Add(txtParm1Name.Text, "SqlDbType." + cbParm1Type.Text);
                        command.Parameters[txtParm1Name.Text].Value = txtParm1Value.Text.Trim();
                    }
                    if (txtParm2Name.Text != "")
                    {
                        command.Parameters.Add(txtParm2Name.Text, "SqlDbType." + cbParm2Type.Text);
                        command.Parameters[txtParm2Name.Text].Value = txtParm2Value.Text.Trim();
                    }
                    if (txtParm3Name.Text != "")
                    {
                        command.Parameters.Add(txtParm3Name.Text, "SqlDbType." + cbParm3Type.Text);
                        command.Parameters[txtParm3Name.Text].Value = txtParm3Value.Text.Trim();
                    }
                    if (txtParm4Name.Text != "")
                    {
                        command.Parameters.Add(txtParm4Name.Text, "SqlDbType." + cbParm4Type.Text);
                        command.Parameters[txtParm4Name.Text].Value = txtParm4Value.Text.Trim();
                    }
                    if (txtParm5Name.Text != "")
                    {
                        command.Parameters.Add(txtParm5Name.Text, "SqlDbType." + cbParm5Type.Text);
                        command.Parameters[txtParm5Name.Text].Value = txtParm5Value.Text.Trim();
                    }

                    // ************************************************************************************
                    // 3. Create datatable called dtSchemax to hold schema for selected columns only
                    // this table does not include columns for fields used in joins, etc.
                    // ************************************************************************************
                    connection.Open();
                    SqlDataReader readerx = command.ExecuteReader();

                    DataTable dtSchemax = readerx.GetSchemaTable();

                    int intNumOfColumns = dtSchemax.Rows.Count;
                    intNumOfColumns = intNumOfColumns - 1;
                    readerx.Close();

                    // ************************************************************************************
                    // 4. Get the schema for all columns in query including join fields and put in dtSchema
                    // ************************************************************************************

                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.KeyInfo);

                    DataTable dtSchema = reader.GetSchemaTable();



                    // You can also use an ArrayList instead of List<>")

                    List<DataColumn> listCols = new List<DataColumn>();
                    if (dtSchema != null)
                    {

                        foreach (DataRow drow in dtSchema.Rows)
                        {


                            string columnName = System.Convert.ToString(drow["ColumnName"]); //Alias if used

                            DataColumn column = new DataColumn(columnName, (Type)(drow["DataType"]));
                            string strDBName = System.Convert.ToString(drow["BaseCatalogName"]);
                            string strSchemaName = System.Convert.ToString(drow["BaseSchemaName"]);
                            string strTableViewName = System.Convert.ToString(drow["BaseTableName"]);
                            strTableViewName = strTableViewName.Replace("]", "");
                            string strSQLColumnName = System.Convert.ToString(drow["ColumnName"]); //Alias if used
                            strSQLColumnName = strSQLColumnName.Replace("]", "");
                            int intColumnOrdinal = (int)drow["ColumnOrdinal"];
                            if (intColumnOrdinal.CompareTo(intNumOfColumns) > 0)
                            {
                                continue;
                            }
                            string strSQLDataType = System.Convert.ToString(drow["DataType"]);
                            int strLength = (int)drow["ColumnSize"];
                            string strNETDataType = System.Convert.ToString(drow["ColumnName"]); //Alias if used
                            string strGetData = "GetString";
                            string strNETColumnName = "";
                            int intIsNullable = 0;
                            int intOrdinalNumberDataType = 0;
                            int intOrdinalNumberLength = 0;
                            int intOrdinalNumberIsNullable = 0;
                            //intOrdinalNumberDataType = dr.GetOrdinal("datatype")
                            //intOrdinalNumberLength = dr.GetOrdinal("length")
                            //intOrdinalNumberIsNullable = dr.GetOrdinal("IsNullable")
                            //strSQLDataType = dr.GetString(intOrdinalNumberDataType)
                            //strLength = dr.GetInt16(intOrdinalNumberLength)
                            //intIsNullable = dr.GetInt32(intOrdinalNumberIsNullable)
                            // insert into columnproperties
                            SqlConnection thisConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 

                            //Create Command object
                            SqlCommand nonqueryCommand = thisConnection.CreateCommand();

                            try
                            {
                                // Open Connection
                                thisConnection.Open();
                                if (boolDeleteNeeded == true)
                                {
                                    boolDeleteNeeded = false;
                                    nonqueryCommand.CommandText = "DELETE FROM [dbo].[ColumnProperties]";
                                    nonqueryCommand.ExecuteNonQuery();
                                    nonqueryCommand.CommandText = "DBCC CHECKIDENT('ColumnProperties', RESEED, 0)";
                                    nonqueryCommand.ExecuteNonQuery();
                                }
                                // ************************************************************************************
                                // 5. Insert the info into ColumnProperties 
                                // ************************************************************************************
                                // Create INSERT statement with named parameters
                                nonqueryCommand.CommandText = "INSERT INTO [dbo].[ColumnProperties] " + " ([FullyQualifiedColumnName] " + " ,[DBName] " + " ,[SchemaName] " + " ,[TableViewName] " + " ,[SQLColumnName] " + " ,[ColumnOrdinal] " + " ,[SQLDataType] " + " ,[NETDataType] " + " ,[NETGetData] " + " ,[NETColumnName] " + " ,[Length] " + " ,[IsNullable]) " + " VALUES  " + "  (@FullyQualifiedColumnName  " + "  ,@DBName " + "  ,@SchemaName " + "  ,@TableViewName " + "  ,@SQLColumnName " + "  ,@ColumnOrdinal " + "  ,@SQLDataType " + "  ,@NETDataType " + "  ,@NETGetData " + "  ,@NETColumnName " + "  ,@Length " + "  ,@IsNullable) ";

                                // Add Parameters to Command Parameters collection
                                nonqueryCommand.Parameters.Add("@FullyQualifiedColumnName", SqlDbType.VarChar, 500);
                                nonqueryCommand.Parameters.Add("@DBName", SqlDbType.VarChar, 500);
                                nonqueryCommand.Parameters.Add("@SchemaName", SqlDbType.VarChar, 500);
                                nonqueryCommand.Parameters.Add("@TableViewName", SqlDbType.VarChar, 500);
                                nonqueryCommand.Parameters.Add("@SQLColumnName", SqlDbType.VarChar, 500);
                                nonqueryCommand.Parameters.Add("@ColumnOrdinal", SqlDbType.Int);
                                nonqueryCommand.Parameters.Add("@SQLDataType", SqlDbType.VarChar, 500);
                                nonqueryCommand.Parameters.Add("@NETDataType", SqlDbType.VarChar, 500);
                                nonqueryCommand.Parameters.Add("@NETGetData", SqlDbType.VarChar, 500);
                                nonqueryCommand.Parameters.Add("@NETColumnName", SqlDbType.VarChar, 500);
                                nonqueryCommand.Parameters.Add("@Length", SqlDbType.VarChar, 500);
                                nonqueryCommand.Parameters.Add("@IsNullable", SqlDbType.VarChar, 500);
                                // Prepare command for repeated execution
                                nonqueryCommand.Prepare();

                                // Data to be inserted

                                nonqueryCommand.Parameters["@FullyQualifiedColumnName"].Value = strDBName + "." + strSchemaName + "." + strTableViewName + "." + strSQLColumnName;
                                nonqueryCommand.Parameters["@DBName"].Value = strDBName;
                                nonqueryCommand.Parameters["@SchemaName"].Value = strSchemaName;
                                nonqueryCommand.Parameters["@TableViewName"].Value = strTableViewName.Replace(" ", "_");
                                nonqueryCommand.Parameters["@SQLColumnName"].Value = strSQLColumnName.Replace(" ", "_");
                                nonqueryCommand.Parameters["@ColumnOrdinal"].Value = intColumnOrdinal;
                                nonqueryCommand.Parameters["@SQLDataType"].Value = strSQLDataType;
                                nonqueryCommand.Parameters["@Length"].Value = strLength;
                                nonqueryCommand.Parameters["@IsNullable"].Value = intIsNullable;

                                strSQLDataType = strSQLDataType.Replace("System.", "");
                                if (strSQLDataType == "Int64")
                                {
                                    strNETDataType = "Int64";
                                    strGetData = "GetInt64";
                                    strNETColumnName = "int_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "Byte()")
                                {
                                    strNETDataType = "Byte()";
                                    strGetData = "GetBytes";
                                    strNETColumnName = "byt_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "Boolean")
                                {
                                    strNETDataType = "Boolean";
                                    strGetData = "GetBoolean";
                                    strNETColumnName = "bool_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "DateTime")
                                {
                                    strNETDataType = "DateTime";
                                    strGetData = "GetDateTime";
                                    strNETColumnName = "dt_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "Decimal")
                                {
                                    strNETDataType = "Decimal";
                                    strGetData = "GetDecimal";
                                    strNETColumnName = "dec_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "Double")
                                {
                                    strNETDataType = "Double";
                                    strGetData = "GetDouble";
                                    strNETColumnName = "dbl_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "Byte()")
                                {
                                    strNETDataType = "Byte()";
                                    strGetData = "GetBytes";
                                    strNETColumnName = "byt_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "Int32")
                                {
                                    strNETDataType = "Int32";
                                    strGetData = "GetInt32";
                                    strNETColumnName = "int_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "Decimal")
                                {
                                    strNETDataType = "Decimal";
                                    strGetData = "GetDecimal";
                                    strNETColumnName = "dec_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "String")
                                {
                                    strNETDataType = "String";
                                    strGetData = "GetString";
                                    strNETColumnName = "str_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "String")
                                {
                                    strNETDataType = "String";
                                    strGetData = "GetString";
                                    strNETColumnName = "str_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "Decimal")
                                {
                                    strNETDataType = "Decimal";
                                    strGetData = "GetDecimal";
                                    strNETColumnName = "dec_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "String")
                                {
                                    strNETDataType = "String";
                                    strGetData = "GetString";
                                    strNETColumnName = "str_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "Single")
                                {
                                    strNETDataType = "Single";
                                    strGetData = "GetFloat";
                                    strNETColumnName = "sin_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "DateTime")
                                {
                                    strNETDataType = "DateTime";
                                    strGetData = "GetDateTime";
                                    strNETColumnName = "dt_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "Int16")
                                {
                                    strNETDataType = "Int16";
                                    strGetData = "GetInt16";
                                    strNETColumnName = "int_" + strTableViewName + "_" + strSQLColumnName;
                                }
                                else if (strSQLDataType == "Decimal")
                                {
                                    strNETDataType = "Decimal";
                                    strGetData = "GetDecimal";
                                    strNETColumnName = "dec_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "Object")
                                {
                                    strNETDataType = "Object";
                                    strGetData = "GetValue";
                                    strNETColumnName = "obj_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "String")
                                {
                                    strNETDataType = "String";
                                    strGetData = "GetString";
                                    strNETColumnName = "str_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "Byte()")
                                {
                                    strNETDataType = "Byte()";
                                    strGetData = "GetBytes";
                                    strNETColumnName = "byt_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "Byte")
                                {
                                    strNETDataType = "Byte";
                                    strGetData = "GetByte";
                                    strNETColumnName = "byt_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "Guid")
                                {
                                    strNETDataType = "Guid";
                                    strGetData = "GetGuid";
                                    strNETColumnName = "guid_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "Byte[]" || strSQLDataType == "Byte()")
                                {
                                    strNETDataType = "Byte()";
                                    strGetData = "GetBytes";
                                    strNETColumnName = "byt_" + strTableViewName + "_" + strSQLColumnName;

                                }
                                else if (strSQLDataType == "String")
                                {
                                    strNETDataType = "String";
                                    strGetData = "GetString";
                                    strNETColumnName = "str_" + strTableViewName + "_" + strSQLColumnName;
                                }
                                else
                                {
                                    MessageBox.Show("Unable to handle " + strSQLDataType + " for " + strSQLColumnName);

                                    continue;
                                }
                                nonqueryCommand.Parameters["@NETDataType"].Value = strNETDataType;
                                nonqueryCommand.Parameters["@NETGetData"].Value = strGetData;
                                nonqueryCommand.Parameters["@NETColumnName"].Value = strNETColumnName.Replace(" ", "_");

                                nonqueryCommand.ExecuteNonQuery();



                            }
                            catch (SqlException ex)
                            {
                                // Display error
                                MessageBox.Show("Error Code 012 [exception=" + ex.Message + "]");
                            }
                            finally
                            {
                                // Close Connection
                                thisConnection.Close();
                                Console.WriteLine("Connection Closed");

                            }


                        }

                    } // end for if schema != null

                } // end using

            } // end try
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

                return;
            }
            // ********************************************************************
            // 6. Generate Code-Behind for Imports, Sub ReadSQL(), Dim queryString
            // ********************************************************************
            if (cbLanguage.Text == "VisualBasic.NET")
            {
                sb3.AppendLine("'Imports System.Data.SqlClient ");
                sb3.AppendLine("'Imports System.Data.Sql ");
                sb3.AppendLine("'Imports System.IO ");
                sb3.AppendLine("'Imports System.Data");
                sb3.AppendLine(" ");
                sb3.AppendLine("Sub ReadSQL()");
                sb3.AppendLine(" ");
                sb3.AppendLine("    ' ********************************************************************");
                sb3.AppendLine("    ' Code Generated by Ideal Tools Organizer at http://idealautomate.com");
                sb3.AppendLine("    ' ********************************************************************");
                sb3.AppendLine("    ' Define Query String");
                sb3.AppendLine("    Dim queryString as String = _");
            }
            else
            {
                sb3.AppendLine("//using System.Data.SqlClient; ");
                sb3.AppendLine("//using System.Data.Sql; ");
                sb3.AppendLine("//using System.IO; ");
                sb3.AppendLine("//using System.Data;");
                sb3.AppendLine(" ");
                sb3.AppendLine("public void ReadSQL()");
                sb3.AppendLine("{ ");
                sb3.AppendLine("    // ********************************************************************");
                sb3.AppendLine("    // Code Generated by Ideal Tools Organizer at http://idealautomate.com");
                sb3.AppendLine("    // ********************************************************************");
                sb3.AppendLine("    // Define Query String");
                sb3.AppendLine("    string queryString = ");
            }
            // ******************************************************************
            // 7. Generate Code-Behind for SQL Query
            // ******************************************************************
            Array arrayQuery = txtSQL.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var line in arrayQuery)
            {
                if (chkStoredProcedure.IsChecked == true)
                {
                    if (cbLanguage.Text == "VisualBasic.NET")
                    {
                        sb3.AppendLine("        \"" + line.ToString().Replace(Environment.NewLine, "").Trim() + "\" & _");
                    }
                    else
                    {
                        sb3.AppendLine("        \"" + line.ToString().Replace(Environment.NewLine, "").Trim() + "\" + ");
                    }
                }
                else
                
                {
                    if (cbLanguage.Text == "VisualBasic.NET")
                    {
                        sb3.AppendLine("        \"" + line.ToString().Replace(Environment.NewLine, "").Trim() + " \" & _");
                    }
                    else
                    {
                        sb3.AppendLine("        \"" + line.ToString().Replace(Environment.NewLine, "").Trim() + " \" + ");
                    }
                }
            }
            if (cbLanguage.Text == "VisualBasic.NET")
            {
                sb3.AppendLine("       \"\" ");
            }
            else
            {
                sb3.AppendLine("       \"\"; ");
            }


            // ****************************************************************
            // 8. Write code-behind for defining connection string
            // ****************************************************************
            queryString = "Select * from ColumnProperties order by insertseq";

            if (cbLanguage.Text == "VisualBasic.NET")
            {
                sb3.AppendLine("    ' Define Connection String");
                sb3.AppendLine("    Dim strConnectionString As String");
                sb3.AppendLine("    strConnectionString = \"" + ((DataRowView)cbConnectionStrings.SelectedItem).Row["ConnectionString"].ToString() + "\"");
            }
            else
            {
                sb3.AppendLine("    // Define Connection String");
                sb3.AppendLine("    string strConnectionString = null;");
                sb3.AppendLine("    strConnectionString = \"" + ((DataRowView)cbConnectionStrings.SelectedItem).Row["ConnectionString"].ToString() + "\";");
            }

            // ****************************************************************
            // 9. Get records from ColumnProperties ordered by insertseq
            // ****************************************************************
            // This is where we define the net columns
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)))) 
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();


                SqlCommand command2 = new SqlCommand(queryString, connection);
                SqlDataReader reader = command2.ExecuteReader();
                int intOrdinalNumberNETColumnName = 0;
                intOrdinalNumberNETColumnName = reader.GetOrdinal("NETColumnName");
                int intOrdinalNumberNETDataType = 0;
                intOrdinalNumberNETDataType = reader.GetOrdinal("NETDataType");

                string strColumns = null;
                string strNETDataType = null;
                if (cbLanguage.Text == "VisualBasic.NET")
                {
                    sb3.AppendLine("         ' Define .net fields to hold each column selected in query");
                }
                else
                {
                    sb3.AppendLine("         // Define .net fields to hold each column selected in query");
                }
                while (reader.Read())
                {
                    if (!(reader.IsDBNull(intOrdinalNumberNETColumnName)))
                    {
                        // Dim intOrdinalNumberSqlColumnName As Integer
                        // intOrdinalNumberSqlColumnName = reader.GetOrdinal("SQLColumnName")
                        strColumns = reader.GetString(intOrdinalNumberNETColumnName);
                        strNETDataType = reader.GetString(intOrdinalNumberNETDataType);
                        if (cbLanguage.Text == "VisualBasic.NET")
                        {
                            sb3.AppendLine("         Dim " + strColumns + " As " + strNETDataType);
                        }
                        else
                        {
                            sb3.AppendLine("         " + strNETDataType + " " + strColumns + ";");
                        }
                    }
                }
            }
            // ****************************************************************
            // 10. Define a table
            // ****************************************************************
            if (cbLanguage.Text == "VisualBasic.NET")
            {
                sb3.AppendLine("         ' Define a datatable that we will define columns in to match the columns");
                sb3.AppendLine("         ' selected in the query. We will use sqldatareader to read the results");
                sb3.AppendLine("         ' from the sql query one row at a time. Then we will add each of those");
                sb3.AppendLine("         ' rows to the datatable - this is where you can modify the information");
                sb3.AppendLine("         ' returned from the sql query one row at a time. Finally, we will");
                sb3.AppendLine("         ' bind the table to the gridview.");
                sb3.AppendLine("         Dim dt As DataTable = New DataTable()");
                sb3.AppendLine(" ");
                sb3.AppendLine("    Using connection As New SqlConnection(strConnectionString" + strTable + ")");
                sb3.AppendLine("        Dim command As New SqlCommand(queryString, connection) ");
            }
            else
            {
                sb3.AppendLine("         // Define a datatable that we will define columns in to match the columns");
                sb3.AppendLine("         // selected in the query. We will use sqldatareader to read the results");
                sb3.AppendLine("         // from the sql query one row at a time. Then we will add each of those");
                sb3.AppendLine("         // rows to the datatable - this is where you can modify the information");
                sb3.AppendLine("         // returned from the sql query one row at a time. Finally, we will");
                sb3.AppendLine("         // bind the table to the gridview.");
                sb3.AppendLine("         DataTable dt = new DataTable();");
                sb3.AppendLine(" ");
                sb3.AppendLine("    using (SqlConnection connection = new SqlConnection(strConnectionString" + strTable + "))");
                sb3.AppendLine("{");
                sb3.AppendLine("        SqlCommand command = new SqlCommand(queryString, connection); ");
            }

            if (chkStoredProcedure.IsChecked == true)
            {
                if (cbLanguage.Text == "VisualBasic.NET")
                {
                    sb3.AppendLine("        Command.CommandType = CommandType.StoredProcedure");
                }
                else
                {
                    sb3.AppendLine("        command.CommandType = CommandType.StoredProcedure;");
                }
            }

            // ****************************************************************
            // 11. Add parameters to command
            // ****************************************************************
            if (txtParm1Name.Text != "")
            {
                if (cbLanguage.Text == "VisualBasic.NET")
                {
                    sb3.AppendLine("         ' Define parameters used in query and assign values to them");
                    sb3.AppendLine("        Command.Parameters.Add(\"" + txtParm1Name.Text + "\", SqlDbType." + cbParm1Type.Text + ")");
                    sb3.AppendLine("        Command.Parameters(\"" + txtParm1Name.Text + "\").Value = " + txtParm1Value.Text.Trim());
                }
                else
                {
                    sb3.AppendLine("        // Define parameters used in query and assign values to them");
                    sb3.AppendLine("        command.Parameters.Add(\"" + txtParm1Name.Text + "\", SqlDbType." + cbParm1Type.Text + ");");
                    sb3.AppendLine("        command.Parameters[\"" + txtParm1Name.Text + "\"].Value = " + txtParm1Value.Text.Trim() + ";");
                }

            }
            if (txtParm2Name.Text != "")
            {
                if (cbLanguage.Text == "VisualBasic.NET")
                {
                    sb3.AppendLine("        Command.Parameters.Add(\"" + txtParm2Name.Text + "\", SqlDbType." + cbParm2Type.Text + ")");
                    sb3.AppendLine("        Command.Parameters(\"" + txtParm2Name.Text + "\").Value = " + txtParm2Value.Text.Trim());
                }
                else
                {
                    sb3.AppendLine("        command.Parameters.Add(\"" + txtParm2Name.Text + "\", SqlDbType." + cbParm2Type.Text + ");");
                    sb3.AppendLine("        command.Parameters[\"" + txtParm2Name.Text + "\"].Value = " + txtParm2Value.Text.Trim() + ";");
                }
            }
            if (txtParm3Name.Text != "")
            {
                if (cbLanguage.Text == "VisualBasic.NET")
                {
                    sb3.AppendLine("        Command.Parameters.Add(\"" + txtParm3Name.Text + "\", SqlDbType." + cbParm3Type.Text + ")");
                    sb3.AppendLine("        Command.Parameters(\"" + txtParm3Name.Text + "\").Value = " + txtParm3Value.Text.Trim());
                }
                else
                {
                    sb3.AppendLine("        command.Parameters.Add(\"" + txtParm3Name.Text + "\", SqlDbType." + cbParm3Type.Text + ");");
                    sb3.AppendLine("        command.Parameters[\"" + txtParm3Name.Text + "\"].Value = " + txtParm3Value.Text.Trim() + ";");
                }
            }
            if (txtParm4Name.Text != "")
            {
                if (cbLanguage.Text == "VisualBasic.NET")
                {
                    sb3.AppendLine("        Command.Parameters.Add(\"" + txtParm4Name.Text + "\", SqlDbType." + cbParm4Type.Text + ")");
                    sb3.AppendLine("        Command.Parameters(\"" + txtParm4Name.Text + "\").Value = " + txtParm4Value.Text.Trim());
                }
                else
                {
                    sb3.AppendLine("        command.Parameters.Add(\"" + txtParm4Name.Text + "\", SqlDbType." + cbParm4Type.Text + ");");
                    sb3.AppendLine("        command.Parameters[\"" + txtParm4Name.Text + "\"].Value = " + txtParm4Value.Text.Trim() + ";");
                }
            }
            if (txtParm5Name.Text != "")
            {
                if (cbLanguage.Text == "VisualBasic.NET")
                {
                    sb3.AppendLine("        Command.Parameters.Add(\"" + txtParm5Name.Text + "\", SqlDbType." + cbParm5Type.Text + ")");
                    sb3.AppendLine("        Command.Parameters(\"" + txtParm5Name.Text + "\").Value = " + txtParm5Value.Text.Trim());
                }
                else
                {
                    sb3.AppendLine("        command.Parameters.Add(\"" + txtParm5Name.Text + "\", SqlDbType." + cbParm5Type.Text + ");");
                    sb3.AppendLine("        command.Parameters[\"" + txtParm5Name.Text + "\"].Value = " + txtParm5Value.Text.Trim() + ";");
                }
            }

            if (cbLanguage.Text == "VisualBasic.NET")
            {
                sb3.AppendLine(" ");
                sb3.AppendLine("        connection.Open() ");
                sb3.AppendLine(" ");
                sb3.AppendLine("        Dim reader As SqlDataReader = command.ExecuteReader() ");
                sb3.AppendLine("        ' Define a column in the table for each column that was selected in the sql query ");
                sb3.AppendLine("        ' We do this before the sqldatareader loop because the columns only need to be  ");
                sb3.AppendLine("        ' defined once. ");
                sb3.AppendLine(" ");
                sb3.AppendLine("         Dim column As DataColumn ");
            }
            else
            {
                sb3.AppendLine(" ");
                sb3.AppendLine("        connection.Open(); ");
                sb3.AppendLine(" ");
                sb3.AppendLine("        SqlDataReader reader = command.ExecuteReader(); ");
                sb3.AppendLine("        // Define a column in the table for each column that was selected in the sql query ");
                sb3.AppendLine("        // We do this before the sqldatareader loop because the columns only need to be  ");
                sb3.AppendLine("        // defined once. ");
                sb3.AppendLine(" ");
                sb3.AppendLine("         DataColumn column = null; ");
            }
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)))) 
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();


                SqlCommand command2 = new SqlCommand(queryString, connection);
                SqlDataReader reader = command2.ExecuteReader();
                int intOrdinalNumberSqlColumnName = 0;
                intOrdinalNumberSqlColumnName = reader.GetOrdinal("SQLColumnName");
                int intOrdinalNumberTableViewName = 0;
                intOrdinalNumberTableViewName = reader.GetOrdinal("TableViewName");

                int intOrdinalNumberNETDataType = reader.GetOrdinal("NETDataType");


                string strTableViewName = null;

                while (reader.Read())
                {
                    if (!(reader.IsDBNull(intOrdinalNumberSqlColumnName)) & !(reader.IsDBNull(intOrdinalNumberSqlColumnName)))
                    {
                        // Dim intOrdinalNumberSqlColumnName As Integer
                        // intOrdinalNumberSqlColumnName = reader.GetOrdinal("SQLColumnName")
                        string strColumns = reader.GetString(intOrdinalNumberSqlColumnName);
                        strTableViewName = reader.GetString(intOrdinalNumberTableViewName);
                        string strNETDataType = reader.GetString(intOrdinalNumberNETDataType);

                        //sb3.AppendLine("         Dim intOrdinalNumber" & strTableViewName & "_" & strColumns & " As Integer")
                        //sb3.AppendLine("         intOrdinalNumber" & strTableViewName & "_" & strColumns & " = reader.GetOrdinal(""" & strColumns & """) ")
                        if (cbLanguage.Text == "VisualBasic.NET")
                        {
                            sb3.AppendLine("         column = New DataColumn(\"" + strTableViewName + "_" + strColumns + "\", Type.GetType(\"System." + strNETDataType.Replace("()", "") + "\")) ");
                            sb3.AppendLine("         dt.Columns.Add(column) ");
                        }
                        else
                        {
                            sb3.AppendLine("         column = new DataColumn(\"" + strTableViewName + "_" + strColumns + "\", Type.GetType(\"System." + strNETDataType.Replace("()", "") + "\")); ");
                            sb3.AppendLine("         dt.Columns.Add(column); ");
                        }


                    }
                }
            }


            if (cbLanguage.Text == "VisualBasic.NET")
            {
                sb3.AppendLine("       ' Read the results from the sql query one row at a time ");
                sb3.AppendLine("        While reader.Read() ");
                sb3.AppendLine("            ' define a new datatable row to hold the row read from the sql query ");
                sb3.AppendLine("            Dim dataRow As DataRow = dt.NewRow() ");
                sb3.AppendLine("            ' Move each field from the reader to a holding field in .net ");
                sb3.AppendLine("            ' ******************************************************************** ");
                sb3.AppendLine("            ' The holding field in .net is where you can alter the contents of the ");
                sb3.AppendLine("            ' field ");
                sb3.AppendLine("            ' ******************************************************************** ");
                sb3.AppendLine("            ' Then, you move the contents of the holding .net field to the column in ");
                sb3.AppendLine("            ' the datarow that you defined above ");                
            }
            else
            {
                sb3.AppendLine("       // Read the results from the sql query one row at a time ");
                sb3.AppendLine("        while (reader.Read()) ");
                sb3.AppendLine("         {");
                sb3.AppendLine("            // define a new datatable row to hold the row read from the sql query ");
                sb3.AppendLine("            DataRow dataRow = dt.NewRow(); ");
                sb3.AppendLine("            // Move each field from the reader to a holding field in .net ");
                sb3.AppendLine("            // ******************************************************************** ");
                sb3.AppendLine("            // The holding field in .net is where you can alter the contents of the ");
                sb3.AppendLine("            // field ");
                sb3.AppendLine("            // ******************************************************************** ");
                sb3.AppendLine("            // Then, you move the contents of the holding .net field to the column in ");
                sb3.AppendLine("            // the datarow that you defined above "); 
            }

            // we need to do this for each column
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)))) 
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();


                SqlCommand command2 = new SqlCommand(queryString, connection);
                SqlDataReader reader = command2.ExecuteReader();

                int intOrdinalNumberNETColumnName = reader.GetOrdinal("NETColumnName");
                int intOrdinalNumberSQLColumnName = 0;
                intOrdinalNumberSQLColumnName = reader.GetOrdinal("SQLColumnName");
                int intOrdinalNumberNETGetData = 0;
                intOrdinalNumberNETGetData = reader.GetOrdinal("NETGetData");
                int intOrdinalNumberTableViewName = 0;
                intOrdinalNumberTableViewName = reader.GetOrdinal("TableViewName");
                int intOrdinalNumberLength = 0;
                intOrdinalNumberLength = reader.GetOrdinal("Length");
                int intOrdinalNumberColumnOrdinal = 0;
                intOrdinalNumberColumnOrdinal = reader.GetOrdinal("ColumnOrdinal");

                string strSQLColumnName = null;
                string strNETColumnName = null;
                string strNETGetData = null;
                string strTableViewName = null;
                int intLength = 0;
                int intColumnOrdinal = 0;
                while (reader.Read())
                {
                    if (!(reader.IsDBNull(intOrdinalNumberNETColumnName)))
                    {
                        // Dim intOrdinalNumberSqlColumnName As Integer
                        // intOrdinalNumberSqlColumnName = reader.GetOrdinal("SQLColumnName")
                        strSQLColumnName = reader.GetString(intOrdinalNumberSQLColumnName);
                        strTableViewName = reader.GetString(intOrdinalNumberTableViewName);
                        strNETColumnName = reader.GetString(intOrdinalNumberNETColumnName);
                        strNETGetData = reader.GetString(intOrdinalNumberNETGetData);
                        intColumnOrdinal = reader.GetInt32(intOrdinalNumberColumnOrdinal);
                        // Need to generate code for this:
                        // str_ConnectionStrings_ConnectionString = reader.GetString(intOrdinalNumberConnectionString)
                        if (cbLanguage.Text == "VisualBasic.NET")
                        {
                            sb3.AppendLine("    If NOT reader.IsDBNull(" + intColumnOrdinal + ") Then");
                        }
                        else
                        {
                            sb3.AppendLine("    if (! (reader.IsDBNull(" + intColumnOrdinal + ")))");
                            sb3.AppendLine("        {");
                        }

                        if (strNETGetData == "GetBytes")
                        {
                            if (cbLanguage.Text == "VisualBasic.NET")
                            {
                                sb3.AppendLine("             'reader.GetBytes(" + intColumnOrdinal + ",0," + strNETColumnName + ",0," + intLength + ")");
                                sb3.AppendLine("             'dataRow(\"" + strTableViewName + "_" + strSQLColumnName + "\") = " + strNETColumnName);
                            }
                            else
                            {
                                sb3.AppendLine("             //reader.GetBytes(" + intColumnOrdinal + ",0," + strNETColumnName + ",0," + intLength + ");");
                                sb3.AppendLine("             //dataRow[\"" + strTableViewName + "_" + strSQLColumnName + "\"] = " + strNETColumnName + ";");
                            }
                        }
                        else
                        {
                            if (cbLanguage.Text == "VisualBasic.NET")
                            {
                                sb3.AppendLine("         " + strNETColumnName + " = reader." + strNETGetData + "(" + intColumnOrdinal + ")");
                                sb3.AppendLine("         dataRow(\"" + strTableViewName + "_" + strSQLColumnName + "\") = " + strNETColumnName);
                            }
                            else
                            {
                                sb3.AppendLine("         " + strNETColumnName + " = reader." + strNETGetData + "(" + intColumnOrdinal + ");");
                                sb3.AppendLine("         dataRow[\"" + strTableViewName + "_" + strSQLColumnName + "\"] = " + strNETColumnName + ";");
                            }

                        }
                        if (cbLanguage.Text == "VisualBasic.NET")
                        {
                            sb3.AppendLine("    End If ");
                        }
                        else
                        {
                            sb3.AppendLine("    } ");
                        }

                    }
                }
            }
            if (cbLanguage.Text == "VisualBasic.NET")
            {
                sb3.AppendLine("            ' Add the row to the datatable ");
                sb3.AppendLine("            dt.Rows.Add(dataRow) ");
                sb3.AppendLine("        End While");
                sb3.AppendLine(" ");
                sb3.AppendLine("        ' Call Close when done reading. ");
                sb3.AppendLine("        reader.Close() ");
                sb3.AppendLine("    End Using ");
                sb3.AppendLine(" ");
                sb3.AppendLine("    ' assign the datatable as the datasource for the gridview and bind the gridview      ");
                sb3.AppendLine("    GridView2.DataSource = dt ");
                sb3.AppendLine("    GridView2.DataBind() ");
                sb3.AppendLine(" ");

                sb3.AppendLine("End Sub ");
                sb3.AppendLine(" ");
            }
            else
            {
                sb3.AppendLine("            // Add the row to the datatable ");
                sb3.AppendLine("            dt.Rows.Add(dataRow); ");
                sb3.AppendLine("        }");
                sb3.AppendLine(" ");
                sb3.AppendLine("        // Call Close when done reading. ");
                sb3.AppendLine("        reader.Close(); ");
                sb3.AppendLine("    } ");
                sb3.AppendLine(" ");
                sb3.AppendLine("    // assign the datatable as the datasource for the gridview and bind the gridview      ");
                sb3.AppendLine("    GridView2.DataSource = dt; ");
                sb3.AppendLine("    GridView2.DataBind(); ");
                sb3.AppendLine(" ");

                sb3.AppendLine("} ");
                sb3.AppendLine(" ");
            }


            txtGeneratedCode.Text = sb3.ToString();

            StringBuilder sb4 = new StringBuilder();
            sb4.AppendLine("<asp:GridView ID=\"GridView2\" runat=\"server\" EnableModelValidation=\"True\" ");
            sb4.AppendLine("  AutoGenerateColumns=\"false\" > ");
            sb4.AppendLine("      ");
            sb4.AppendLine("  <Columns>");
            // Need to loop through for each column
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)))) 
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();


                SqlCommand command2 = new SqlCommand(queryString, connection);
                SqlDataReader reader = command2.ExecuteReader();
                int intOrdinalNumberSqlColumnName = 0;
                intOrdinalNumberSqlColumnName = reader.GetOrdinal("SQLColumnName");
                int intOrdinalNumberTableViewName = 0;
                intOrdinalNumberTableViewName = reader.GetOrdinal("TableViewName");

                string strSqlColumnName = null;
                string strTableViewName = null;
                while (reader.Read())
                {
                    if (!(reader.IsDBNull(intOrdinalNumberSqlColumnName)))
                    {
                        // Dim intOrdinalNumberSqlColumnName As Integer
                        // intOrdinalNumberSqlColumnName = reader.GetOrdinal("SQLColumnName")
                        strSqlColumnName = reader.GetString(intOrdinalNumberSqlColumnName);
                        strTableViewName = reader.GetString(intOrdinalNumberTableViewName);

                        sb4.AppendLine("         <asp:TemplateField HeaderText=\"" + strSqlColumnName + "\">");
                        sb4.AppendLine("            <ItemTemplate>");
                        sb4.AppendLine("               <asp:Label ID=\"lbl" + strSqlColumnName + "\" runat=\"server\" ");
                        sb4.AppendLine("                             Text='<%# Bind(\"" + strTableViewName + "_" + strSqlColumnName + "\") %>'></asp:Label>");
                        sb4.AppendLine("             </ItemTemplate>");
                        sb4.AppendLine("          </asp:TemplateField>");



                    }
                }
            }
            sb4.AppendLine("  </Columns>");
            sb4.AppendLine("</asp:GridView>");
            //<asp:GridView ID="GridView2" runat="server" EnableModelValidation="True"
            //    AutoGenerateColumns="false" AllowSorting="True" AutoGenerateDeleteButton="True"
            //    AutoGenerateEditButton="True" AutoGenerateSelectButton="True" >
            //    <Columns>
            //        <asp:TemplateField HeaderText="ConnectionString_ID">
            //            <ItemTemplate>
            //                <asp:Label ID="lblConnectionStringID" runat="server"
            //                    Text='<%# Bind("ConnectionString_ID") %>'></asp:Label>
            //            </ItemTemplate>
            //        </asp:TemplateField>
            //        <asp:TemplateField HeaderText="Connection String">
            //            <ItemTemplate>
            //                <asp:Label ID="lblConnection_String" runat="server"
            //                    Text='<%# Bind("[Connection String]") %>'></asp:Label>
            //            </ItemTemplate>
            //        </asp:TemplateField>
            //    </Columns>
            //</asp:GridView>
            txtGeneratedASPX.Text = sb4.ToString();
            //MessageBox.Show("Code Generated Successfully");

        }


        private void btnSelectCodeBehind_Click(object sender, RoutedEventArgs e)
        {
            txtGeneratedCode.Focus();
            txtGeneratedCode.SelectAll();
        }

        private void btnSelectASPX_Click(object sender, RoutedEventArgs e)
        {
            txtGeneratedASPX.Focus();
            txtGeneratedASPX.SelectAll();
        }
        private void ShowHelpDialog(object sender, RoutedEventArgs e)
        {
            NavWindow dlg = new NavWindow();
            dlg.Owner = (Window)this.Parent;
           // Shadow.Visibility = Visibility.Visible;
             dlg.Show();
            //Shadow.Visibility = Visibility.Collapsed;
        }
        private void txtSQL_LostFocus(object sender, RoutedEventArgs e)
        {
            string strSQL = txtSQL.Text;

            strSQL = strSQL.Replace("(", " ");
            strSQL = strSQL.Replace(")", " ");
            strSQL = strSQL.Replace(",", " ");
            strSQL = strSQL.Replace(";", " ");


            Array aryWords = strSQL.Split(' ');
            string[] aryParam = new string[6];
            int intParam = -1;

            foreach (var element in aryWords)
            {
                if (element.ToString().StartsWith("@") & !(element.ToString().StartsWith("@@")))
                {
                    intParam = intParam + 1;
                    aryParam[intParam] = (string)element;
                }
            }

            if (intParam > -1)
            {
                txtParm1Name.Text = aryParam[0];
            }

            if (intParam > 0)
            {
                txtParm2Name.Text = aryParam[1];
            }

            if (intParam > 1)
            {
                txtParm3Name.Text = aryParam[2];
            }

            if (intParam > 2)
            {
                txtParm4Name.Text = aryParam[3];
            }

            if (intParam > 3)
            {
                txtParm5Name.Text = aryParam[4];
            }

            if (intParam > 4)
            {
                MessageBox.Show("Cannot handle more than 5 parameters");

            }

        }


    }
}
