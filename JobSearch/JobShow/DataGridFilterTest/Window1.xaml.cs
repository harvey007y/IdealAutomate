using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.ComponentModel;
using System.Collections.ObjectModel;
using DataGridExtensionsSample;
using DataGridFilterTest.TestData;
using DataGridFilterLibrary.Querying;
using DataGridFilterLibrary;

namespace DataGridFilterTest
{
    public partial class Window1 : Window,INotifyPropertyChanged

    {       
        string strConnectionString = null;
        //private ObservableCollection<DataItem> jobApplicationList = new ObservableCollection<DataItem>();

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public Window1()
        {
           
            InitializeComponent();
            if (MyObjectDataProvider != null) MyObjectDataProvider.ObjectInstance = TestData.TestDataGenerator.Instance;

            this.DataContext = MyObjectDataProvider;

            //this.DataContext = TestData.TestDataGenerator.Instance;

            //TestData.TestDataGenerator.Instance.GenerateTestData(null);

            TestData.TestDataGenerator.Instance.GenerateTestData(args =>
            {
                SetDefaultValues();
            });


        }

        private void SetDefaultValues()
        {
            QueryController queryController = DataGridExtensions.GetDataGridFilterQueryController(this.SampleGrid);

            var filters = queryController.GetFiltersForColumns();

            //var keywordFilter = filters.FirstOrDefault(w => w.Key == "str_jobapplications_Keyword");
            //keywordFilter.Value.QueryString = "c";

            var dateAddedFilter = filters.FirstOrDefault(w => w.Key == "dt_jobapplications_DateAdded");
            dateAddedFilter.Value.QueryString = System.DateTime.Today.AddDays(-2).ToShortDateString();
            dateAddedFilter.Value.Operator = DataGridFilterLibrary.Support.FilterOperator.GreaterThanOrEqual;

            //var positionAge = filters.FirstOrDefault(w => w.Key == "Position.Age");
            //positionAge.Value.QueryString = "10";
            //positionAge.Value.QueryStringTo = "20";

            queryController.SetFiltersForColumns(filters);
        }

        private ObjectDataProvider MyObjectDataProvider
        {
            get
            {
                return this.TryFindResource("EmployeeData") as ObjectDataProvider;
            }
        }

     

        private void DataGrid_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = e.OriginalSource as Hyperlink;
            if (hyperlink != null)
            {
                Process.Start(hyperlink.NavigateUri.ToString());
            }
        }





        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            SampleGrid.SelectAllCells();
        }

        private void UnselectAll_Click(object sender, RoutedEventArgs e)
        {
            SampleGrid.UnselectAllCells();
        }

        private void ApplicationStatusSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            DataItem selectedItem = (DataItem)this.SampleGrid.CurrentItem;
            if (selectedItem.str_jobapplications_ApplicationStatus == ((EmployeeStatus)comboBox.SelectedItem).Name)
            {
                return;
            }
            //Create Connection
            SqlConnection thisConnection = new SqlConnection(@"Data Source=.\SQLEXPRESS02;Initial Catalog=IdealAutomateDB;Integrated Security=SSPI");
            OpenConnection(thisConnection);
            try
            {
                // 1. Create Command
                // Sql Update Statement
                string updateSql = "UPDATE JobApplications " + "SET ApplicationStatus = @ApplicationStatus, DateApplied = @DateApplied, DateLastModified = @DateLastModified " + "WHERE JobUrl = @JobUrl";
                SqlCommand UpdateCmd = new SqlCommand(updateSql, thisConnection);

                // 2. Map Parameters

                UpdateCmd.Parameters.Add("@ApplicationStatus", SqlDbType.NVarChar, 50, "ApplicationStatus");

                UpdateCmd.Parameters.Add("@JobUrl", SqlDbType.NVarChar, 4000, "JobUrl");
                UpdateCmd.Parameters.Add("@DateApplied", SqlDbType.DateTime);
                UpdateCmd.Parameters.Add("@DateLastModified", SqlDbType.DateTime);

                UpdateCmd.Parameters["@ApplicationStatus"].Value = ((EmployeeStatus)comboBox.SelectedItem).Name;
                UpdateCmd.Parameters["@JobUrl"].Value = selectedItem.str_jobapplications_JobUrl;
                if (comboBox.SelectedItem.ToString() == "Applied")
                {
                    UpdateCmd.Parameters["@DateApplied"].Value = System.DateTime.Now;
                }
                else
                {
                    UpdateCmd.Parameters["@DateApplied"].Value = DBNull.Value;
                }
                UpdateCmd.Parameters["@DateLastModified"].Value = System.DateTime.Now;
                UpdateCmd.ExecuteNonQuery();
            }

            catch (SqlException ex)
            {
                // Display error
                Console.WriteLine("Error: " + ex.ToString());
            }

            // Close Connection
            thisConnection.Close();
         //   SetUpPlanets();

        }
        void OpenConnection(SqlConnection con)
        {
            try
            {
                // Open Connection
                con.Open();
                Console.WriteLine("Connection Opened");
            }
            catch (SqlException ex)
            {
                // Display error
                Console.WriteLine("Error: " + ex.ToString());
            }
        }
        private void Comments_LostFocus(object sender, RoutedEventArgs e)
        {
            DataGridCell myCell = sender as DataGridCell;
            DataItem selectedItem = (DataItem)this.SampleGrid.CurrentItem;



            SqlConnection thisConnection = new SqlConnection(@"Data Source=.\SQLEXPRESS02;Initial Catalog=IdealAutomateDB;Integrated Security=SSPI");
            OpenConnection(thisConnection);
            try
            {
                // 1. Create Command
                // Sql Update Statement
                string updateSql = "UPDATE JobApplications " + "SET Comments = @Comments, DateLastModified = @DateLastModified " + "WHERE JobUrl = @JobUrl";
                SqlCommand UpdateCmd = new SqlCommand(updateSql, thisConnection);

                // 2. Map Parameters



                UpdateCmd.Parameters.Add("@JobUrl", SqlDbType.NVarChar, 4000, "JobUrl");
                UpdateCmd.Parameters.Add("@Comments", SqlDbType.VarChar, 2000, "Comments");
                UpdateCmd.Parameters.Add("@DateLastModified", SqlDbType.DateTime);

                UpdateCmd.Parameters["@Comments"].Value = ((TextBox)sender).Text;
                UpdateCmd.Parameters["@JobUrl"].Value = selectedItem.str_jobapplications_JobUrl;

                UpdateCmd.Parameters["@DateLastModified"].Value = System.DateTime.Now;
                UpdateCmd.ExecuteNonQuery();
            }

            catch (SqlException ex)
            {
                // Display error
                Console.WriteLine("Error: " + ex.ToString());
            }

            // Close Connection
            thisConnection.Close();
       //     SetUpPlanets();

        }
    }
}
