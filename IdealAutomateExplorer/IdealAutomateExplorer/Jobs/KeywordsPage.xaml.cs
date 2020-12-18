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
    /// Interaction logic for JobApplicationkeywords.xaml
    /// </summary>
    public partial class KewordsPage : Page
    {
        KeywordManager keywordManager = new KeywordManager();
        public KewordsPage()
        {
            InitializeComponent();
            GridView2.ItemsSource = keywordManager.GetKeywords();
        }

        private void dataGrid_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            keywordManager.Upsert((JobApplicationKeywords)((DataGrid)sender).CurrentItem);

        }




        private void dataGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            if (((DataGrid)sender).SelectedItem != null || ((DataGrid)sender).CurrentItem == null)
            {
                return;
            }
            JobApplicationKeywords myJobApplicationkeywords = (JobApplicationKeywords)((DataGrid)sender).CurrentItem;
            keywordManager.Delete(myJobApplicationkeywords.Id);
        }
    }
}
