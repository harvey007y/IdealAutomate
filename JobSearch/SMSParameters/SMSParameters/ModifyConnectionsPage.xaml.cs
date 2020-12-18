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
using IdealAutomate.Core;

namespace SMSParameters { 
    public partial class ModifyConnectionsPage: Page 
    {
        StringBuilder sb1 = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();
        StringBuilder sb3 = new StringBuilder();
        DataTable dt;
        Methods myActions = new Methods();
        string strTable = "";
        bool boolFirstTime = true;
        bool boolDeleteNeeded = false;
       
        public ModifyConnectionsPage()
        {
            InitializeComponent();
        }
        void OnLoad(object sender, RoutedEventArgs e)
        {
            var startListenerWorker = new BackgroundWorker();
            startListenerWorker.DoWork += new DoWorkEventHandler(this.StartListenerDoWork);
            startListenerWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.WorkCompleted);
            this.cbConnectionStrings.IsEnabled = false;
            startListenerWorker.RunWorkerAsync();
            this.cbConnectionStrings.IsEnabled = false;

        }

        private void WorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.cbConnectionStrings.ItemsSource = ((IListSource)dt).GetList();
            this.cbConnectionStrings.DisplayMemberPath = "ConnectionString";
            this.cbConnectionStrings.SelectedValuePath = "tID";
            this.cbConnectionStrings.IsEnabled = true;
            if (this.cbConnectionStrings.HasItems)
            {
 
                string savedConnectionString = myActions.GetValueByKey("JobsConnectionString");
                foreach (var item in this.cbConnectionStrings.Items)
                {
                    if (((DataRowView)item)[1].ToString() == savedConnectionString)
                    {
                        this.cbConnectionStrings.SelectedItem = item;
                    }
                }
            }
          
        }

        private void StartListenerDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            
            try
            {
                DataSet ds = getData();
 
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                else
                {
                    dt = new DataTable();
                }


            }
            catch (SqlException ex)
            {
                // Display error
                //   MessageBox.Show("Error Code 014 [exception=" + ex.Message + "]");
            }
            
        }
        public DataSet getData()
        {
            try
            {
                SqlConnection thisConnection;
                if (String.IsNullOrEmpty(myActions.GetValueByKey("JobsConnectionString")))
                          
                        {
                            DataSet ds = new DataSet();
                            return ds;
                        }
                else
                {
                    thisConnection = new SqlConnection(ConnectionString1.SqlConnString);
                    string sql = "SELECT * From ConnectionStrings";
                    SqlDataAdapter da = new SqlDataAdapter(sql, thisConnection);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "ConnectionStrings");
                    return ds;
                }
            }
            catch (SqlException ex)
            {
                // Display error
             //   MessageBox.Show("Error Code 013 [exception=" + ex.Message + "]");
                DataSet ds = new DataSet();
                
                return ds;
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            myActions.SetValueByKey("JobsConnectionString", txtConnectionString.Text);
            ConnectionString1.GetConnectionString(myActions);

            SqlConnection thisConnection = new SqlConnection(txtConnectionString.Text);
            //Create Command object
            SqlCommand nonqueryCommand = thisConnection.CreateCommand();

            try
            {
                // Open Connection
                thisConnection.Open();
                //Console.WriteLine("Connection Opened");

                // Create INSERT statement with named parameters
                nonqueryCommand.CommandText = "IF  NOT EXISTS (SELECT * FROM sys.objects  " +
"WHERE object_id = OBJECT_ID(N'[dbo].[ConnectionStrings]') AND type in (N'U')) " +
"BEGIN " +
"CREATE TABLE [dbo].[ConnectionStrings]( " +
"	[tID] [int] IDENTITY(1,1) NOT NULL, " +
"	[ConnectionString] [varchar](500) NULL, " +
" CONSTRAINT [PK_ConnectionStrings] PRIMARY KEY CLUSTERED  " +
"( " +
"	[tID] ASC " +
")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY] " +
") ON [PRIMARY] " +
"END " +
" " +
"IF  NOT EXISTS (SELECT * FROM sys.objects  " +
"WHERE object_id = OBJECT_ID(N'[dbo].[JobApplicationJobBoards]') AND type in (N'U')) " +
"BEGIN " +
"CREATE TABLE [dbo].[JobApplicationJobBoards]( " +
"	[Id] [int] IDENTITY(1,1) NOT NULL, " +
"	[Name] [varchar](50) NULL, " +
"	[Enabled] [bit] NOT NULL, " +
" CONSTRAINT [PK_JobApplicationJobBoards] PRIMARY KEY CLUSTERED  " +
"( " +
"	[Id] ASC " +
")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY] " +
") ON [PRIMARY] " +
" " +
"INSERT INTO [dbo].[JobApplicationJobBoards] " +
"           ([Name] " +
"           ,[Enabled]) " +
"     VALUES " +
"           ('Dice',0), " +
"           ('Glassdoor',0), " +
"		   ('Indeed',0), " +
"		   ('LinkedIn',1), " +
"	       ('Monster',0), " +
"		   ('CareerBuilder',0), " +
"		   ('StackOverflow',0), " +
"		   ('ZipRecruiter',0) " +
"END " +
" " +
"IF  NOT EXISTS (SELECT * FROM sys.objects  " +
"WHERE object_id = OBJECT_ID(N'[dbo].[JobApplicationKeywords]') AND type in (N'U')) " +
"BEGIN " +
"CREATE TABLE [dbo].[JobApplicationKeywords]( " +
"	[Id] [int] IDENTITY(1,1) NOT NULL, " +
"	[Keyword] [varchar](50) NOT NULL, " +
"	[Enabled] [bit] NOT NULL, " +
" CONSTRAINT [PK_JobApplicationKeywords] PRIMARY KEY CLUSTERED  " +
"( " +
"	[Id] ASC " +
")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY] " +
") ON [PRIMARY] " +
"END " +
" " +
"IF  NOT EXISTS (SELECT * FROM sys.objects  " +
"WHERE object_id = OBJECT_ID(N'[dbo].[JobApplicationLocations]') AND type in (N'U')) " +
"BEGIN " +
"CREATE TABLE [dbo].[JobApplicationLocations]( " +
"	[Id] [int] IDENTITY(1,1) NOT NULL, " +
"	[LocationName] [varchar](500) NOT NULL, " +
"	[LinkedInGeoId] [varchar](100) NULL, " +
"	[GlassdoorLocation] [varchar](100) NULL, " +
"	[DiceLatitude] [varchar](100) NULL, " +
"	[DiceLongitude] [varchar](100) NULL, " +
"	[Enabled] [bit] NOT NULL, " +
" CONSTRAINT [PK_JobApplicationLocations] PRIMARY KEY CLUSTERED  " +
"( " +
"	[Id] ASC " +
")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY] " +
") ON [PRIMARY] " +
"END " +
" " +
"IF  NOT EXISTS (SELECT * FROM sys.objects  " +
"WHERE object_id = OBJECT_ID(N'[dbo].[JobApplications]') AND type in (N'U')) " +
"BEGIN " +
"CREATE TABLE [dbo].[JobApplications]( " +
"	[JobUrl] [nvarchar](max) NOT NULL, " +
"	[JobBoard] [nvarchar](500) NULL, " +
"	[JobTitle] [nvarchar](500) NULL, " +
"	[CompanyTitle] [nvarchar](500) NULL, " +
"	[DateAdded] [datetime] NULL, " +
"	[DateLastModified] [datetime] NULL, " +
"	[DateApplied] [datetime] NULL, " +
"	[ApplicationStatus] [nvarchar](50) NULL, " +
"	[Keyword] [nvarchar](500) NULL, " +
"	[Location] [nvarchar](500) NULL, " +
"	[Comments] [varchar](2000) NULL " +
") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] " +
"END " +
" " +
"IF  NOT EXISTS (SELECT * FROM sys.objects  " +
"WHERE object_id = OBJECT_ID(N'[dbo].[JobApplicationStatus]') AND type in (N'U')) " +
"BEGIN " +
"CREATE TABLE [dbo].[JobApplicationStatus]( " +
"	[Id] [int] NOT NULL, " +
"	[Status] [nvarchar](50) NULL, " +
" CONSTRAINT [PK_JobApplicationStatus] PRIMARY KEY CLUSTERED  " +
"( " +
"	[Id] ASC " +
")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY] " +
") ON [PRIMARY] " +
"END ";




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

            DataSet ds = getData();

            DataTable dt = ds.Tables[0];

            this.cbConnectionStrings.ItemsSource = ((IListSource)dt).GetList();
            this.cbConnectionStrings.DisplayMemberPath = "ConnectionString";
            this.cbConnectionStrings.SelectedValuePath = "tID";    

            foreach (var item in this.cbConnectionStrings.Items)
            {
                if (((DataRowView)item)[1].ToString() == txtConnectionString.Text)
                {
                    this.cbConnectionStrings.SelectedItem = item;
                    MessageBox.Show("Connection String Added");
                    return;
                }
            }



            thisConnection = new SqlConnection(txtConnectionString.Text);
            //Create Command object
            nonqueryCommand = thisConnection.CreateCommand();

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

          
            ds = getData();

            dt = ds.Tables[0];

            this.cbConnectionStrings.ItemsSource = ((IListSource)dt).GetList();
            this.cbConnectionStrings.DisplayMemberPath = "ConnectionString";
            this.cbConnectionStrings.SelectedValuePath = "tID";
            MessageBox.Show("Connection String Added");
           
            foreach (var item in this.cbConnectionStrings.Items)
            {
               if (((DataRowView)item)[1].ToString() == txtConnectionString.Text) {
                    this.cbConnectionStrings.SelectedItem = item;
                }
            }
            
        }
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(ConnectionString1.SqlConnString))
            {

                SqlConnection thisConnection = new SqlConnection(ConnectionString1.SqlConnString);
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
            }

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
                MessageBox.Show("Error Code 009 [exception=" + ex.Message + "]");
            }

        }
        void cbConnectionStrings_SelectionChanged(object sender, EventArgs e)
        {
            int selectedIndex = 0;
            selectedIndex = cbConnectionStrings.SelectedIndex;
            object selectedItem = null;
            string selection = "";
            if (selectedIndex != -1)
            {

                selectedItem = cbConnectionStrings.SelectedItem;
                selection = Convert.ToString(((DataRowView)cbConnectionStrings.SelectedItem).Row["ConnectionString"]);
            }
            myActions.SetValueByKey("JobsConnectionString", selection);
        }


        private void ShowHelpDialog(object sender, RoutedEventArgs e)
        {
            //NavWindowFindColumns dlg = new NavWindowFindColumns();
            //dlg.Owner = (Window)this.Parent;
            //Shadow.Visibility = Visibility.Visible;
            //dlg.Show();
            //Shadow.Visibility = Visibility.Collapsed;
        }
    }
}
