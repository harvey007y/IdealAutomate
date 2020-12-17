using IdealAutomate.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SMSParameters
{
    public static class ConnectionString1
    {

        private static string sqlConnString;

        public static string SqlConnString
        {
            get
            {
                if (String.IsNullOrEmpty(sqlConnString))
                {
                    MessageBox.Show("The connection string is empty - please use Connections menu item to set Connection String", "No Connection String", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                }
                return sqlConnString;
            }
            set { sqlConnString = value; }
        }


        public static bool GetConnectionString(Methods myAction)
        {
            bool connectionStringFound = true;
            string connectionString = myAction.GetValueByKey("JobsConnectionString");
            if (String.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("The connection string is empty - please use Connections menu item to set Connection String", "No Connection String", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                connectionStringFound = false;
            }
            else
            {
                SqlConnString = connectionString;
            }
            return connectionStringFound;
        }

        public static bool GetConnectionStringWithNoMsgIfNotFound(Methods myAction)
        {
            bool connectionStringFound = true;
            string connectionString = myAction.GetValueByKey("JobsConnectionString");
            if (String.IsNullOrEmpty(connectionString))
            {
                connectionStringFound = false;
            }
            else
            {
                SqlConnString = connectionString;
            }
            return connectionStringFound;
        }
    }
}
