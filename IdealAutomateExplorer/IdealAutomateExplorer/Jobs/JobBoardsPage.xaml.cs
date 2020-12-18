using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data.Sql;
using System.IO;
using System.Data;



namespace SMSParameters
{
    /// <summary>
    /// Interaction logic for JobApplicationJobBoards.xaml
    /// </summary>
    public partial class JobBoardsPage : Page
    {
        JobBoardManager jobBoardManager = new JobBoardManager();
        public JobBoardsPage()
        {
            InitializeComponent();
            GridView2.ItemsSource = jobBoardManager.GetJobBoards();
        }

        private void dataGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            jobBoardManager.Upsert((JobApplicationJobBoards)((DataGrid)sender).CurrentItem);
            
        }

        private void dataGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            if (((DataGrid)sender).SelectedItem != null || ((DataGrid)sender).CurrentItem == null)
            {
                return;
            }
            JobApplicationJobBoards myJobApplicationJobBoards = (JobApplicationJobBoards)((DataGrid)sender).CurrentItem;
            jobBoardManager.Delete(myJobApplicationJobBoards.Id);
    
        }
    }
}
