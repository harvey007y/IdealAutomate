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

namespace Hardcodet.Wpf.Samples.Pages {
    public partial class FindColumns 
    {
        StringBuilder sb1 = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();
        StringBuilder sb3 = new StringBuilder();
        string strTable = "";
        bool boolFirstTime = true;
        bool boolDeleteNeeded = false;
        public FindColumns()
        {
            InitializeComponent();
        }
        void OnLoad(object sender, RoutedEventArgs e)
        {
            try
            {
                DataSet ds = getData();

                DataTable dt = ds.Tables[0];

                this.cbConnectionStrings.ItemsSource = ((IListSource)dt).GetList();
                this.cbConnectionStrings.DisplayMemberPath = "ConnectionString";
                this.cbConnectionStrings.SelectedValuePath = "tID";
            }
                 catch (SqlException ex)
            {
                // Display error
                MessageBox.Show("Error Code 014 [exception=" + ex.Message + "]");
            }

        }
        public DataSet getData()
        {
            try
            {
                SqlConnection thisConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)));
                string sql = "SELECT * From ConnectionStrings";
                SqlDataAdapter da = new SqlDataAdapter(sql, thisConnection);
                DataSet ds = new DataSet();
                da.Fill(ds, "ConnectionStrings");
                return ds;
            }
            catch (SqlException ex)
            {
                // Display error
                MessageBox.Show("Error Code 013 [exception=" + ex.Message + "]");
                DataSet ds = new DataSet();
                
                return ds;
            }
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
                MessageBox.Show("Error Code 008 [exception=" + ex.Message + "]");
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
                MessageBox.Show("Error Code 009 [exception=" + ex.Message + "]");
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
            try
            {
                DataSet ds = getData();

                DataTable dt = ds.Tables[0];

                this.cbConnectionStrings.ItemsSource = ((IListSource)dt).GetList();
                this.cbConnectionStrings.DisplayMemberPath = "ConnectionString";
                this.cbConnectionStrings.SelectedValuePath = "tID";
            }
            catch (SqlException ex)
            {
                // Display error
                MessageBox.Show("Error Code 009 [exception=" + ex.Message + "]");
            }

        }
        void cbConnectionStrings_SelectionChanged(object sender, EventArgs e)
        {
            
        }

       
        private void ShowHelpDialog(object sender, RoutedEventArgs e)
        {
            NavWindowFindColumns dlg = new NavWindowFindColumns();
            dlg.Owner = (Window)this.Parent;
            //Shadow.Visibility = Visibility.Visible;
           dlg.Show();
            //Shadow.Visibility = Visibility.Collapsed;
        }

        private void FilterButton(object sender, RoutedEventArgs e)
        {
            int selectedIndex = 0;
            selectedIndex = cbConnectionStrings.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("Please select connection string");
                return;
            }
            object selectedItem = null;
            selectedItem = cbConnectionStrings.SelectedItem;
            string selection = ((DataRowView)cbConnectionStrings.SelectedItem).Row["ConnectionString"].ToString();

            SqlConnection con = new SqlConnection(selection);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT table_name=sysobjects.name, sysobjects.xtype,        column_name=syscolumns.name,         datatype=systypes.name,         length=syscolumns.length, syscolumns.isnullable    FROM sysobjects     JOIN syscolumns ON sysobjects.id = syscolumns.id    JOIN systypes ON syscolumns.xtype=systypes.xtype   WHERE systypes.name <>  'sysname' and sysobjects.name like @name and sysobjects.xtype like @xtype and syscolumns.name like @columnname and systypes.name like @datatype --  and syscolumns.name like '%contactid%'     and sysobjects.name = 'Contact'ORDER BY sysobjects.name,syscolumns.name";
            cmd.Parameters.Add("@name", SqlDbType.VarChar);
            cmd.Parameters["@name"].Value = txtName.Text.Trim();
            cmd.Parameters.Add("@xtype", SqlDbType.VarChar);
            cmd.Parameters["@xtype"].Value = txtXtype.Text.Trim();
            cmd.Parameters.Add("@columnname", SqlDbType.VarChar);
            cmd.Parameters["@columnname"].Value = txtColumnName.Text.Trim();
            cmd.Parameters.Add("@datatype", SqlDbType.VarChar);
            cmd.Parameters["@datatype"].Value = txtDataType.Text.Trim();
            if (txtName.Text.Trim() == "")
            {
                cmd.Parameters["@name"].Value = "%";
            }
            if (txtXtype.Text.Trim() == "")
            {
                cmd.Parameters["@xtype"].Value = "%";
            }
            if (txtColumnName.Text.Trim() == "")
            {
                cmd.Parameters["@columnname"].Value = "%";
            }
            if (txtDataType.Text.Trim() == "")
            {
                cmd.Parameters["@datatype"].Value = "%";
            }


            try
            {


                con.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                this.DataContext = "";
                this.DataContext = dt;
                // DataGridView1.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = dt }); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }
    }
}
