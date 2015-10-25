using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Hardcodet.Wpf.GenericTreeView;
using Hardcodet.Wpf.Samples.ViewModel;
using SampleShop.Products;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;

using System.Runtime.InteropServices;
using System.Reflection;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Management;
using System.Collections;
using System.Linq;
using System.Configuration;




namespace Hardcodet.Wpf.Samples.Pages
{
    /// <summary>
    /// Interaction logic for IdealLauncher.xaml
    /// </summary>
    public partial class IdealLauncher : UserControl
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer;
        public IntPtr myhWnd;
        public int intKeyCtr = 0;
        public int intOverallAverageKeysPerMinute = 0;
        public int intPrevKeyCtr = 0;
        public int intPrevElapsedSeconds = 0;
        public int intStartShowElapsedSeconds = 0;
        public int intReminderNum = 0;
        public bool boolKeepCounting = true;
        public bool boolReminderDisplayed = true;
        public bool boolFirstTime = true;

        public ArrayList list = new ArrayList();
        public StringBuilder sbg = new StringBuilder();




        public DateTime dtStartDateTime = System.DateTime.Now;
        private const int SW_SHOWNOACTIVATE = 4;
        private const int SW_SHOWMINNOACTIVE = 6;
        private const uint SWP_NOACTIVATE = 0X10;




        private const Int32 WM_GETTEXT = 0XD;
        private const Int32 WM_GETTEXTLENGTH = 0XE;
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SendMessageA", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        public static extern Int32 SendMessage(IntPtr hwnd, Int32 wMsg, Int32 wParam, Int32 lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SendMessageA", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        public static extern Int32 SendMessage(IntPtr hwnd, Int32 wMsg, Int32 wParam, string lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetForegroundWindow", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, ref int lpdwProcessID);
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetWindowTextA", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, string WinTitle, int MaxLength);
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetWindowTextLengthA", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern int GetWindowTextLength(int hwnd);
        [DllImport("user32.dll")]
        private extern static bool ShowWindow(IntPtr hWnd, int nCmdShow);
        bool boolContentHasFocus = true;
        bool boolExecutablesHasFocus = false;
        string strRootToNode = "";
        public IdealLauncher()
        {
            string myServiceName = "MSSQL$SQLEXPRESS"; //service name of SQL Server Express
            string status; //service status (For example, Running or Stopped)

            Console.WriteLine("Service: " + myServiceName);

            //display service status: For example, Running, Stopped, or Paused
            ServiceController mySC = new ServiceController(myServiceName);

            try
            {
                status = mySC.Status.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("SQL Server Express Service not found. It is probably not installed. [exception=" + ex.Message + "]");
                

                return;

            }

            //display service status: For example, Running, Stopped, or Paused
            Console.WriteLine("Service status : " + status);

            //if service is Stopped or StopPending, you can run it with the following code.
            if (mySC.Status.Equals(ServiceControllerStatus.Stopped) | mySC.Status.Equals(ServiceControllerStatus.StopPending))
            {
                try
                {
                    MessageBox.Show("Starting the service...");
                    mySC.Start();
                    mySC.WaitForStatus(ServiceControllerStatus.Running);
                    MessageBox.Show("The service is now " + mySC.Status.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in starting the SQL SERVER Express service: " + ex.Message);

                }

            }

        


            InitializeComponent();
            CategoryTree.RootNode = null;
            lbExecutables.Items.Clear();
            lbContent.Items.Clear();
            
            // MessageBox.Show("This free version of Ideal Tools Organizer will expire on April 4, 2012 - Please become LifeTime member of http://IdealAutomate.com to continue using");
            
        }
        void OnLoad(object sender, RoutedEventArgs e)
        {
            string CName = System.Environment.MachineName;
            SqlConnection thisConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ClubSiteDB"].ConnectionString);

            //Create Command object
            SqlCommand nonqueryCommand = thisConnection.CreateCommand();

            try
            {
                // Open Connection
                thisConnection.Open();

                // Create INSERT statement with named parameters
                nonqueryCommand.CommandText = "INSERT  INTO IdealToolsOrganizerLog (ComputerName, CreateDate) VALUES (@ComputerName, @CreateDate)";

                // Add Parameters to Command Parameters collection
                nonqueryCommand.Parameters.Add("@ComputerName", SqlDbType.VarChar, 500);
                nonqueryCommand.Parameters.Add("@CreateDate", SqlDbType.DateTime);


                nonqueryCommand.Parameters["@ComputerName"].Value = CName;
                nonqueryCommand.Parameters["@CreateDate"].Value = System.DateTime.Now;

                nonqueryCommand.ExecuteNonQuery();
            }

            catch (SqlException ex)
            {
                // Display error
               
            }

            finally
            {
                // Close Connection
                thisConnection.Close();

            }

            string settingsDirectory =
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealToolsOrganizer";
            string fileName;
            string settingsPath;
            string trialDate;
            if (!Directory.Exists(settingsDirectory))
            {
                Directory.CreateDirectory(settingsDirectory);
            }

            fileName = "tasks.txt";

            settingsPath = Path.Combine(settingsDirectory, fileName);

            if (!File.Exists(settingsPath))
            {
                trialDate = "01/01/2000 12:00:00";
                // Hook a write to the text file.
                StreamWriter writer = new StreamWriter(settingsPath);
                // Rewrite the entire value of s to the file
                writer.Write(trialDate);
                writer.Close();
            }
            //if it's not already there, 
            //copy the file from the deployment location to the folder
            string sourceFilePath = Path.Combine(
              System.AppDomain.CurrentDomain.BaseDirectory, "IdealLauncher.mdf");
            string destFilePath = Path.Combine(settingsDirectory, "IdealLauncher.mdf");
            if (!File.Exists(destFilePath))
            {
                File.Copy(sourceFilePath, destFilePath);
                sourceFilePath = Path.Combine(
                 System.AppDomain.CurrentDomain.BaseDirectory, "IdealLauncher_log.ldf");
                destFilePath = Path.Combine(settingsDirectory, "IdealLauncher_log.ldf");
            }
            if (!File.Exists(destFilePath))
            {
                File.Copy(sourceFilePath, destFilePath);
            }

            fileName = "tasks.txt";

            settingsPath = Path.Combine(settingsDirectory, fileName);
            StreamReader file = File.OpenText(settingsPath);
            trialDate = file.ReadToEnd();
            file.Close();
            //if trialDate > -1, then trial date is today or in the past
            //TimeSpan span = System.DateTime.Today - DateTime.Parse(trialDate);
            //if (span.TotalDays > -1)
            //{

            //    ExpireDialog dlg = new ExpireDialog();
            //    dlg.Owner = this;
            //    Shadow.Visibility = Visibility.Visible;
            //    dlg.ShowDialog();
            //    Shadow.Visibility = Visibility.Collapsed;
            //    if (dlg.DialogResult == false)
            //    {
            //        Application.Current.Shutdown();


            //    }
            //}
           

        }
        private void ShowProfileDialog(object sender, RoutedEventArgs e)
        {
            UserInfo dlg = new UserInfo();
         //   dlg.Owner = this;
            // Shadow.Visibility = Visibility.Visible;
          //  dlg.Show();
            //  Shadow.Visibility = Visibility.Collapsed;
        }

        private void ShowAboutDialog(object sender, RoutedEventArgs e)
        {
            AboutDialog dlg = new AboutDialog();
             dlg.Owner = (Window)this.Parent;
            // Shadow.Visibility = Visibility.Visible;
              dlg.Show();
            //  Shadow.Visibility = Visibility.Collapsed;
        }

        public void btnAddContent_Click(object sender, RoutedEventArgs e)
        {
           
            string name = ShowInputDialog(null);

            if (name != "")
            {
                System.Windows.Controls.ListBoxItem myItem = new ListBoxItem();
                myItem.Content = name;
                lbContent.Items.Add(myItem);
                myItem.IsSelected = true;
                SqlConnection con;

                con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 
                SqlCommand cmd = new SqlCommand("USP_INSERT_MYFILES", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RootToNodeKey", SqlDbType.VarChar, 500);
                cmd.Parameters.Add("@boolContent", SqlDbType.Bit);
                cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 1000);
                //cmd.Parameters.Add("@itemType", SqlDbType.Int);


                cmd.Parameters["@RootToNodeKey"].Value = strRootToNode;
                cmd.Parameters["@boolContent"].Value = true;
                cmd.Parameters["@FileName"].Value = name;
                //  cmd.Parameters["@itemType"].Value = 0;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                //string myString = myIdealLauncher.txtNewItem.Text;
                lbContent.Items.SortDescriptions.Add(

                            new System.ComponentModel.SortDescription("Content",

                               System.ComponentModel.ListSortDirection.Ascending));
            }
        }
        public void btnAddContentBrowsing_Click(object sender, RoutedEventArgs e)
        {
            
            Microsoft.Win32.OpenFileDialog myFile = new Microsoft.Win32.OpenFileDialog();
            myFile.ShowDialog();
           

            if (myFile.FileName != "")
            {
                System.Windows.Controls.ListBoxItem myItem = new ListBoxItem();
                myItem.Content = myFile.FileName;
                lbContent.Items.Add(myItem);
                myItem.IsSelected = true;
                SqlConnection con;

                con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 
                SqlCommand cmd = new SqlCommand("USP_INSERT_MYFILES", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RootToNodeKey", SqlDbType.VarChar, 500);
                cmd.Parameters.Add("@boolContent", SqlDbType.Bit);
                cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 1000);
                //cmd.Parameters.Add("@itemType", SqlDbType.Int);


                cmd.Parameters["@RootToNodeKey"].Value = strRootToNode;
                cmd.Parameters["@boolContent"].Value = true;
                cmd.Parameters["@FileName"].Value = myFile.FileName;
                //  cmd.Parameters["@itemType"].Value = 0;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                //string myString = myIdealLauncher.txtNewItem.Text;
                lbContent.Items.SortDescriptions.Add(

                            new System.ComponentModel.SortDescription("Content",

                               System.ComponentModel.ListSortDirection.Ascending));
            }
        }
        public void btnAddBrowsing_Click(object sender, RoutedEventArgs e)
        {
           
            Microsoft.Win32.OpenFileDialog myFile = new Microsoft.Win32.OpenFileDialog();
            myFile.ShowDialog();
            

            if (myFile.FileName != "")
            {
                System.Windows.Controls.ListBoxItem myItem = new ListBoxItem();
                myItem.Content = myFile.FileName;
                lbExecutables.Items.Add(myItem);
                myItem.IsSelected = true;
                SqlConnection con;

                con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 
                SqlCommand cmd = new SqlCommand("USP_INSERT_MYFILES", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RootToNodeKey", SqlDbType.VarChar, 500);
                cmd.Parameters.Add("@boolContent", SqlDbType.Bit);
                cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 1000);
                //cmd.Parameters.Add("@itemType", SqlDbType.Int);


                cmd.Parameters["@RootToNodeKey"].Value = strRootToNode;
                cmd.Parameters["@boolContent"].Value = false;
                cmd.Parameters["@FileName"].Value = myFile.FileName;
                //  cmd.Parameters["@itemType"].Value = 0;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                //string myString = myIdealLauncher.txtNewItem.Text;
                lbExecutables.Items.SortDescriptions.Add(

                            new System.ComponentModel.SortDescription("Content",

                               System.ComponentModel.ListSortDirection.Ascending));
            }
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // code goes here
            IntPtr hWnd = GetForegroundWindow();
            if (hWnd == IntPtr.Zero)
            {
                return;
            }

            //----- Find the Length of the Window's Title ----- 

            int TitleLength = 0;

            TitleLength = GetWindowTextLength(hWnd.ToInt32());

            //----- Find the Window's Title ----- 

            string WindowTitle = new String('*', TitleLength + 1);

            GetWindowText(hWnd, WindowTitle, TitleLength + 1);

            //----- Find the PID of the Application that Owns the Window ----- 

            int pid = 0;

            GetWindowThreadProcessId(hWnd, ref pid);

            if (pid == 0)
            {
                return;
            }
            if (myhWnd != hWnd)
            {
                string myFileName = Keyboard.GetMainModuleFilepath(pid);
                System.Windows.Controls.ListBoxItem myItem = new ListBoxItem();
                myItem.Content = myFileName;
                lbExecutables.Items.Add(myItem);
                myItem.IsSelected = true;
                SqlConnection con;

                con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)));
                SqlCommand cmd = new SqlCommand("USP_INSERT_MYFILES", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RootToNodeKey", SqlDbType.VarChar, 500);
                cmd.Parameters.Add("@boolContent", SqlDbType.Bit);
                cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 1000);
                //cmd.Parameters.Add("@itemType", SqlDbType.Int);


                cmd.Parameters["@RootToNodeKey"].Value = strRootToNode;
                cmd.Parameters["@boolContent"].Value = false;
                cmd.Parameters["@FileName"].Value = myFileName;
                //  cmd.Parameters["@itemType"].Value = 0;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                lbExecutables.Items.SortDescriptions.Add(

                 new System.ComponentModel.SortDescription("Content",

                    System.ComponentModel.ListSortDirection.Ascending));
                dispatcherTimer.Stop();
            }
        }
        public void btnAddClicking_Click(object sender, RoutedEventArgs e)
        {

           
           // SetWindowPos(myhWnd.ToInt32(), HWND_TOPMOST, this.Left, this.Top, this.Width, this.Height, SWP_NOACTIVATE);
            IntPtr hWnd = GetForegroundWindow();
            myhWnd = hWnd;
            MakeTopMost(hWnd.ToInt32());
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

          //  ShowWindow(myhWnd, SW_SHOWNOACTIVATE);
         //   SetWindowPos(myhWnd.ToInt32(), HWND_TOPMOST, this.Left, this.Top, this.Width, this.Height, SWP_NOACTIVATE);
            //if (myFile.FileName != "")
            //{
            //    System.Windows.Controls.ListBoxItem myItem = new ListBoxItem();
            //    myItem.Content = myFile.FileName;
            //    lbExecutables.Items.Add(myItem);
            //    myItem.IsSelected = true;
            //    SqlConnection con;

            //    con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)));
            //    SqlCommand cmd = new SqlCommand("USP_INSERT_MYFILES", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add("@RootToNodeKey", SqlDbType.VarChar, 500);
            //    cmd.Parameters.Add("@boolContent", SqlDbType.Bit);
            //    cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 1000);
            //    //cmd.Parameters.Add("@itemType", SqlDbType.Int);


            //    cmd.Parameters["@RootToNodeKey"].Value = strRootToNode;
            //    cmd.Parameters["@boolContent"].Value = false;
            //    cmd.Parameters["@FileName"].Value = myFile.FileName;
            //    //  cmd.Parameters["@itemType"].Value = 0;
            //    con.Open();
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //    //string myString = myIdealLauncher.txtNewItem.Text;
            //    lbExecutables.Items.SortDescriptions.Add(

            //                new System.ComponentModel.SortDescription("Content",

            //                   System.ComponentModel.ListSortDirection.Ascending));
            //}
        }
        public void btnDeleteContent_Click(object sender, RoutedEventArgs e)
        {
           
            System.Windows.Controls.ListBoxItem myItem = new ListBoxItem();

            for (int i = lbContent.Items.Count - 1; i > -1; i--)
            {

                myItem = (ListBoxItem)lbContent.Items[i];
                if (myItem.IsSelected)
                {
                    SqlConnection con;

                    con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 
                    SqlCommand cmd = new SqlCommand("USP_DELETE_MYFILES", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RootToNodeKey", SqlDbType.VarChar, 500);
                    cmd.Parameters.Add("@boolContent", SqlDbType.Bit);
                    cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 1000);
                    //cmd.Parameters.Add("@itemType", SqlDbType.Int);


                    cmd.Parameters["@RootToNodeKey"].Value = strRootToNode;
                    cmd.Parameters["@boolContent"].Value = true;
                    cmd.Parameters["@FileName"].Value = myItem.Content;
                    //  cmd.Parameters["@itemType"].Value = 0;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    lbContent.Items.Remove(myItem);
                }
            }
            if (lbContent.Items.Count > 0)
            {
                ListBoxItem li = (ListBoxItem)lbContent.Items[0];
                li.IsSelected = true;

            }
            if (lbExecutables.Items.Count > 0)
            {
                ListBoxItem li = (ListBoxItem)lbExecutables.Items[0];
                li.IsSelected = true;

            }
        }
        public void btnLaunchContent_Click(object sender, RoutedEventArgs e)
        {
            
            if (lbExecutables.SelectedIndex > -1)
            {
                ListBoxItem myExecutable = (ListBoxItem)lbExecutables.SelectedItem;
                string vsPath = myExecutable.Content.ToString();
                if (File.Exists(vsPath) == false)
                {
                    MessageBox.Show("Executable not found on your computer");
                    return;
                }

                if (lbContent.SelectedIndex == -1)
                {
                    Process.Start(vsPath);
                }
                foreach (ListBoxItem item in lbContent.SelectedItems)
                    
                {
                    if (myExecutable.Content.ToString().ToUpper().Contains("SSMS.EXE"))
                    {
                        try
                        {
                            Process.Start(vsPath, string.Concat("", item.Content, ""));
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.ToString());
                        }
                        

                    }
                    else
                    {
                        try
                        {
                            Process.Start(vsPath, string.Concat("\"", item.Content, "\""));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            //throw;
                        }
                    
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select executable before launching");
            }

        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            
            string name = ShowInputDialog(null);

            if (name != "")
            {
                System.Windows.Controls.ListBoxItem myItem = new ListBoxItem();
                myItem.Content = name;
                lbExecutables.Items.Add(myItem);
                myItem.IsSelected = true;
                SqlConnection con;

                con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 
                SqlCommand cmd = new SqlCommand("USP_INSERT_MYFILES", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RootToNodeKey", SqlDbType.VarChar, 500);
                cmd.Parameters.Add("@boolContent", SqlDbType.Bit);
                cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 1000);
                //cmd.Parameters.Add("@itemType", SqlDbType.Int);


                cmd.Parameters["@RootToNodeKey"].Value = strRootToNode;
                cmd.Parameters["@boolContent"].Value = false;
                cmd.Parameters["@FileName"].Value = name;
                //  cmd.Parameters["@itemType"].Value = 0;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                lbExecutables.Items.SortDescriptions.Add(

                 new System.ComponentModel.SortDescription("Content",

                    System.ComponentModel.ListSortDirection.Ascending));
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {


            System.Windows.Controls.ListBoxItem myItem = new ListBoxItem();

            for (int i = lbExecutables.Items.Count - 1; i > -1; i--)
            {

                myItem = (ListBoxItem)lbExecutables.Items[i];
                if (myItem.IsSelected)
                {
                    SqlConnection con;

                    con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 
                    SqlCommand cmd = new SqlCommand("USP_DELETE_MYFILES", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RootToNodeKey", SqlDbType.VarChar, 500);
                    cmd.Parameters.Add("@boolContent", SqlDbType.Bit);
                    cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 1000);
                    //cmd.Parameters.Add("@itemType", SqlDbType.Int);


                    cmd.Parameters["@RootToNodeKey"].Value = strRootToNode;
                    cmd.Parameters["@boolContent"].Value = false;
                    cmd.Parameters["@FileName"].Value = myItem.Content;
                    //  cmd.Parameters["@itemType"].Value = 0;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    lbExecutables.Items.Remove(myItem);
                }
            }
            if (lbContent.Items.Count > 0)
            {
                ListBoxItem li = (ListBoxItem)lbContent.Items[0];
                li.IsSelected = true;

            }
            if (lbExecutables.Items.Count > 0)
            {
                ListBoxItem li = (ListBoxItem)lbExecutables.Items[0];
                li.IsSelected = true;

            }
        }
        private void btnLaunch_Click(object sender, RoutedEventArgs e)
        {
          
            if (lbExecutables.SelectedIndex > -1)
            {
                ListBoxItem myExecutable = (ListBoxItem)lbExecutables.SelectedItem;
                string vsPath = myExecutable.Content.ToString();
                if (File.Exists(vsPath) == false)
                {
                    MessageBox.Show("Executable not found on your computer");
                    return;
                }
                if (lbContent.SelectedIndex == -1)
                {
                    Process.Start(vsPath);
                }
                foreach (ListBoxItem item in lbContent.SelectedItems)
                {
                    if (myExecutable.Content.ToString().ToUpper().Contains("SSMS.EXE"))
                    {
                        Process.Start(vsPath, string.Concat("", item.Content, ""));

                    }
                    else
                    {
                        Process.Start(vsPath, string.Concat("\"", item.Content, "\""));
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select content before launching");
            }
        }
        private void lbExecutables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

        }


        #region context menu command handling

        /// <summary>
        /// Creates a sub category for the clicked item
        /// and refreshes the tree.
        /// </summary>
        /// 

        private void AddCategory(object sender, ExecutedRoutedEventArgs e)
        {
           
            //get the processed item
            ShopCategory parent = GetCommandItem();
            if (parent.ParentCategory != null)
            {
                if (parent.ParentCategory.ParentCategory != null)
                {
                    if (parent.ParentCategory.ParentCategory.ParentCategory == null)
                    {
                        MessageBox.Show("Sorry, you can only go 3 levels deep in this tree structure");
                        return;
                    }
                }
            }
           
            //create a sub category
            string name = ShowInputDialog(null);




            if (name != "")
            {


                ShopCategory subCategory = new ShopCategory(name, parent);
                parent.SubCategories.Add(subCategory);

                // this is where we can add to database???????????????????????????????????
                SqlConnection con;

                con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 

                SqlCommand cmd = new SqlCommand("USP_INSERT_CATEGORY", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@userCategoryID", SqlDbType.Int, 4);
                cmd.Parameters.Add("@paretCategoryID", SqlDbType.Int, 4);
                cmd.Parameters.Add("@categoryTitle", SqlDbType.VarChar, 200);
                cmd.Parameters.Add("@categoryDesc", SqlDbType.VarChar, 5000);
                //cmd.Parameters.Add("@itemType", SqlDbType.Int);

                if (parent.ParentCategory == null)
                {
                    cmd.Parameters["@userCategoryID"].Value = 0;
                    cmd.Parameters["@paretCategoryID"].Value = 0;
                    cmd.Parameters["@categoryTitle"].Value = name;
                    cmd.Parameters["@categoryDesc"].Value = "";
                    //  cmd.Parameters["@itemType"].Value = 0;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                else if (parent.ParentCategory.ParentCategory == null)
                {
                    // we need to find the category id for the category folder so we can use it as the parent id
                    // when we do the insert
                    con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 

                    cmd = new SqlCommand("USP_GET_CATEGORY_BY_TITLE", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@paretCategoryID", SqlDbType.Int, 4);
                    cmd.Parameters.Add("@categoryTitle", SqlDbType.VarChar, 200);
                    Int32 intCategoryID = 0;
                    try
                    {
                        con.Open();
                        cmd.Parameters["@paretCategoryID"].Value = 0;
                        cmd.Parameters["@categoryTitle"].Value = parent.CategoryName.Trim();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            intCategoryID = reader.GetInt32(0);

                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Code 001 [exception=" + ex.Message + "]");
                    }
                    finally
                    {
                        con.Close();
                    }

                    // we now have the category id for the category folder and we can use that for the parent id when we insert
                    con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 

                    cmd = new SqlCommand("USP_INSERT_CATEGORY", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@userCategoryID", SqlDbType.Int, 4);
                    cmd.Parameters.Add("@paretCategoryID", SqlDbType.Int, 4);
                    cmd.Parameters.Add("@categoryTitle", SqlDbType.VarChar, 200);
                    cmd.Parameters.Add("@categoryDesc", SqlDbType.VarChar, 5000);
                    //cmd.Parameters.Add("@itemType", SqlDbType.Int);


                    cmd.Parameters["@userCategoryID"].Value = 0;
                    cmd.Parameters["@paretCategoryID"].Value = intCategoryID;
                    cmd.Parameters["@categoryTitle"].Value = name;
                    cmd.Parameters["@categoryDesc"].Value = "";
                    //  cmd.Parameters["@itemType"].Value = 0;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                }
            

            }

            //make sure the parent is expanded
            CategoryTree.TryFindNode(parent).IsExpanded = true;

            //NOTE this would be an alternative to force layout preservation
            //even if the PreserveLayoutOnRefresh property was false:
            //TreeLayout layout = CategoryTree.GetTreeLayout();
            //CategoryTree.Refresh(layout);

            //Important - mark the event as handled
            e.Handled = true;
        }


        /// <summary>
        /// Checks whether it is allowed to delete a category, which is only
        /// allowed for nested categories, but not the root items.
        /// </summary>
        private void EvaluateCanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            //get the processed item
            ShopCategory item = GetCommandItem();

            e.CanExecute = item.ParentCategory != null;
            e.Handled = true;
        }


        /// <summary>
        /// Deletes the currently processed item. This can be a right-clicked
        /// item (context menu) or the currently selected item, if the user
        /// pressed delete.
        /// </summary>
        private void DeleteCategory(object sender, ExecutedRoutedEventArgs e)
        {
           
            //get item
            ShopCategory item = GetCommandItem();

            //remove from parent
            item.ParentCategory.SubCategories.Remove(item);
            // this is where we can delete category????????????????????????????????
            SqlConnection con;
            con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 

            SqlCommand cmd = new SqlCommand("USP_GET_CATEGORY_BY_TITLE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@paretCategoryID", SqlDbType.Int, 4);
            cmd.Parameters.Add("@categoryTitle", SqlDbType.VarChar, 200);
            Int32 intCategoryID = 0;
            if (item.ParentCategory.ParentCategory == null)
            {
                try
                {
                    con.Open();
                    cmd.Parameters["@paretCategoryID"].Value = 0;
                    cmd.Parameters["@categoryTitle"].Value = item.CategoryName.Trim();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        intCategoryID = reader.GetInt32(0);

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Code 002 [exception=" + ex.Message + "]");
                }
                finally
                {
                    con.Close();
                }
                SqlConnection con1;
                con1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 

                SqlCommand cmd1 = new SqlCommand("USP_GET_DELETE_CATEGORY", con1);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add("@CategoryID", SqlDbType.Int, 4);

                con1.Open();
                cmd1.Parameters["@CategoryID"].Value = intCategoryID;

                try
                {
                    cmd1.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    
                    throw;
                }
               
                con1.Close();
            }
            else if (item.ParentCategory.ParentCategory.ParentCategory == null)
            {
                try
                {
                    con.Open();
                    cmd.Parameters["@paretCategoryID"].Value = 0;
                    cmd.Parameters["@categoryTitle"].Value = item.ParentCategory.CategoryName.Trim();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows == false)
                    {
                        return;
                    }
                    while (reader.Read())
                    {
                        intCategoryID = reader.GetInt32(0);

                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Code 003 [exception=" + ex.Message + "]");
                }
                finally
                {
                    
                    con.Close();
                }
                try
                {
                    con.Open();
                    SqlCommand cmd2 = new SqlCommand("USP_GET_CATEGORY_BY_TITLE", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.Add("@paretCategoryID", SqlDbType.Int, 4);
                    cmd2.Parameters.Add("@categoryTitle", SqlDbType.VarChar, 200);
                    cmd2.Parameters["@paretCategoryID"].Value = intCategoryID;
                    cmd2.Parameters["@categoryTitle"].Value = item.CategoryName.Trim();
                    SqlDataReader reader1 = cmd2.ExecuteReader();
                    if (reader1.HasRows == false)
                    {
                        return;
                    }
                    while (reader1.Read())
                    {
                        intCategoryID = reader1.GetInt32(0);

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Code 004 [exception=" + ex.Message + "]");
                }
                finally
                {
                    con.Close();
                }
                SqlConnection con1;
                con1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 

                SqlCommand cmd1 = new SqlCommand("USP_GET_DELETE_CATEGORY", con1);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add("@CategoryID", SqlDbType.Int, 4);

                con1.Open();
                cmd1.Parameters["@CategoryID"].Value = intCategoryID;

                try
                {
                    cmd1.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Display error
                    MessageBox.Show("Error Code 015 [exception=" + ex.Message + "]");
                    throw;
                }

                con1.Close();
            }

            //mark event as handled
            e.Handled = true;
        }


        /// <summary>
        /// Determines the item that is the source of a given command.
        /// As a command event can be routed from a context menu click
        /// or a short-cut, we have to evaluate both possibilities.
        /// </summary>
        /// <returns></returns>
        private ShopCategory GetCommandItem()
        {
            //get the processed item
            ContextMenu menu = CategoryTree.NodeContextMenu;
            if (menu.IsVisible)
            {
                //a context menu was clicked
                TreeViewItem treeNode = (TreeViewItem)menu.PlacementTarget;
                return (ShopCategory)treeNode.Header;
            }
            else
            {
                //the context menu is closed - the user has pressed a shortcut
                return CategoryTree.SelectedItem;
            }
        }

        #endregion


        #region tree modification

        /// <summary>
        /// Sets or removes a custom root node for the bound
        /// <see cref="ShopCategory"/> items.
        /// </summary>
        private void ToggleRootNode(object sender, RoutedEventArgs e)
        {
            if (CategoryTree.RootNode == null)
            {
                //create a dummy root node
                TreeViewItem rootNode = (TreeViewItem)FindResource("CustomRootNode");
                CategoryTree.RootNode = rootNode;
            }
            else
            {
                //disable artificial root node
                CategoryTree.RootNode = null;
            }
        }


        /// <summary>
        /// Enables / disables the node's context menu.
        /// </summary>
        private void ToggleContextMenu(object sender, RoutedEventArgs e)
        {
            
           
                if (CategoryTree.NodeContextMenu == null)
                {
                    //the menu is declared as a resource of the window
                    ContextMenu menu = (ContextMenu)FindResource("CategoryMenu");
                    CategoryTree.NodeContextMenu = menu;
                }
                else
                {
                    CategoryTree.NodeContextMenu = null;
                }
           
        }


        /// <summary>
        /// Sets or resets the style to be applied on the tree's
        /// nodes.
        /// </summary>
        private void ToggleNodeStyle(object sender, RoutedEventArgs e)
        {
            if (CategoryTree.TreeNodeStyle == null)
            {
                Style style = (Style)FindResource("SimpleFolders");
                CategoryTree.TreeNodeStyle = style;
            }
            else
            {
                //setting the style to null does not clear the existing
                //styles (in order to preserve default layout)
                //-> refresh tree
                CategoryTree.TreeNodeStyle = null;
                CategoryTree.Refresh(CategoryTree.GetTreeLayout());
            }
        }


        /// <summary>
        /// Just triggers a refresh of the view models data. The
        /// resulting <see cref="ShopModel.PropertyChanged"/>
        /// event is enough to trigger a refresh of the tree's
        /// items.
        /// </summary>
        private void ReloadData(object sender, RoutedEventArgs e)
        {
            //the shop instance is declared as a resource of the window
            ShopModel model = GetShop();
            model.RefreshData();

        }


        /// <summary>
        /// Copies the layout of one tree to the other.
        /// </summary>
        private void CopyTreeLayout(object sender, RoutedEventArgs e)
        {
            TreeLayout layout = CategoryTree.GetTreeLayout();
            //SynchronizedTree.Refresh(layout);
        }

        #endregion


        #region expand / collapse

        private void ExpandAll(object sender, RoutedEventArgs e)
        {
            CategoryTree.ExpandAll();
        }


        private void CollapseAll(object sender, RoutedEventArgs e)
        {
            CategoryTree.CollapseAll();
        }

        #endregion


        #region util

        /// <summary>
        /// Gets the view model to which the trees are bound.
        /// </summary>
        /// <returns>View model.</returns>
        private ShopModel GetShop()
        {
            return (ShopModel)FindResource("Shop");
        }

        /// <summary>
        /// Displays an input dialog and returns the entered
        /// value.
        /// </summary>
        private string ShowInputDialog(string defaultValue)
        {
            InputDialog dlg = new InputDialog();
            dlg.CategoryName = defaultValue;
         //   dlg.Owner = this;
            dlg.ShowDialog();

            return dlg.CategoryName.Trim();
        }


        //private void ShowAboutDialog(object sender, RoutedEventArgs e)
        //{
        //    AboutDialog dlg = new AboutDialog();
        //  //  dlg.Owner = this;
        //    Shadow.Visibility = Visibility.Visible;
        //  //  dlg.ShowDialog();
        //    Shadow.Visibility = Visibility.Collapsed;
        //}

        #endregion


        #region SelectedItemChanged event

        /// <summary>
        /// Handles the tree's <see cref="TreeViewBase{T}.SelectedItemChanged"/>
        /// event and updates the status bar.
        /// </summary>
        public void OnSelectedItemChanged(object sender, RoutedTreeItemEventArgs<ShopCategory> e)
        {
            
          //  txtOldItem.Text = String.Format("'{0}'", e.OldItem);
          //  txtNewItem.Text = String.Format("'{0}'", e.NewItem);
            //Introduction.mySelectedItem = txtNewItem.Text;
            // USP_GET_MYFILES_BY_TYPE_NODE

            System.Windows.Controls.ListBoxItem myItem = new ListBoxItem();
            System.Windows.Controls.ListBoxItem myItemExecutables = new ListBoxItem();

            lbContent.Items.Clear();
            lbExecutables.Items.Clear();
            SqlConnection con;

            con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 

            SqlCommand cmd;

            //cmd.Parameters.Add("@itemType", SqlDbType.Int);

            if (CategoryTree.SelectedItem.ParentCategory == null)
            {
                cmd = new SqlCommand("USP_GET_MYFILES_BY_TYPE_NODE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@rootToNodeKey", SqlDbType.VarChar, 500);
                cmd.Parameters.Add("@boolContent", SqlDbType.Bit);
                cmd.Parameters["@rootToNodeKey"].Value = CategoryTree.SelectedItem.CategoryName.Trim();
                cmd.Parameters["@boolContent"].Value = true;
                //  cmd.Parameters["@itemType"].Value = 0;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    myItem.Content = reader.GetString(0);
                    lbContent.Items.Add(myItem);

                }
                con.Close();
                cmd = new SqlCommand("USP_GET_MYFILES_BY_TYPE_NODE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@rootToNodeKey", SqlDbType.VarChar, 500);
                cmd.Parameters.Add("@boolContent", SqlDbType.Bit);
                cmd.Parameters["@rootToNodeKey"].Value = CategoryTree.SelectedItem.CategoryName.Trim();
                cmd.Parameters["@boolContent"].Value = false;
                //  cmd.Parameters["@itemType"].Value = 0;
                con.Open();
                SqlDataReader reader3 = cmd.ExecuteReader();
                while (reader3.Read())
                {

                    myItem.Content = reader3.GetString(0);
                    lbExecutables.Items.Add(myItem);

                }
                con.Close();
            }
            else if (CategoryTree.SelectedItem.ParentCategory.ParentCategory == null)
            {
                // we need to find the category id for the category folder so we can use it as the parent id
                // when we do the insert
                con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 

                cmd = new SqlCommand("USP_GET_CATEGORY_BY_TITLE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@paretCategoryID", SqlDbType.Int, 4);
                cmd.Parameters.Add("@categoryTitle", SqlDbType.VarChar, 200);
                Int32 intCategoryID = 0;
                try
                {
                    con.Open();
                    cmd.Parameters["@paretCategoryID"].Value = 0;
                    cmd.Parameters["@categoryTitle"].Value = CategoryTree.SelectedItem.CategoryName.Trim();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        intCategoryID = reader.GetInt32(0);

                    }
                    reader.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Code 005 [exception=" + ex.Message + "]");
                }
                finally
                {

                    con.Close();
                }

                // we now have the category id for the category folder and we can use that for the parent id when we insert
                con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 

                cmd = new SqlCommand("USP_GET_CATEGORY_DETAILS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int, 4);

                //cmd.Parameters.Add("@itemType", SqlDbType.Int);


                cmd.Parameters["@CategoryID"].Value = intCategoryID;

                //  cmd.Parameters["@itemType"].Value = 0;
                con.Open();
                SqlDataReader reader1;
                try
                {
                    reader1 = cmd.ExecuteReader();
                }
                catch (Exception ex)
                {
                    // Display error
                    MessageBox.Show("Error Code 016 [exception=" + ex.Message + "]");
                    throw;
                }

                while (reader1.Read())
                {

                    strRootToNode = reader1.GetString(3);


                }
                reader1.Close();
                con.Close();
                cmd = new SqlCommand("USP_GET_MYFILES_BY_TYPE_NODE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@rootToNodeKey", SqlDbType.VarChar, 500);
                cmd.Parameters.Add("@boolContent", SqlDbType.Bit);
                cmd.Parameters["@rootToNodeKey"].Value = strRootToNode;
                cmd.Parameters["@boolContent"].Value = true;
                //  cmd.Parameters["@itemType"].Value = 0;
                con.Open();
                SqlDataReader reader2;
                try
                {
                    reader2 = cmd.ExecuteReader();
                }
                catch (Exception ex)
                {
                    // Display error
                    MessageBox.Show("Error Code 017 [exception=" + ex.Message + "]");
                    throw;
                }

                while (reader2.Read())
                {
                    try
                    {
                        System.Windows.Controls.ListBoxItem myItemC = new ListBoxItem();
                        myItemC.Content = reader2.GetString(0);
                        lbContent.Items.Add(myItemC);
                    }
                    catch (Exception ex)
                    {
                        // Display error
                        MessageBox.Show("Error Code 018 [exception=" + ex.Message + "]");
                        throw;
                    }


                }
                con.Close();

                cmd = new SqlCommand("USP_GET_MYFILES_BY_TYPE_NODE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@rootToNodeKey", SqlDbType.VarChar, 500);
                cmd.Parameters.Add("@boolContent", SqlDbType.Bit);
                cmd.Parameters["@rootToNodeKey"].Value = strRootToNode;
                cmd.Parameters["@boolContent"].Value = false;
                //  cmd.Parameters["@itemType"].Value = 0;
                con.Open();
                SqlDataReader reader4 = cmd.ExecuteReader();
                while (reader4.Read())
                {
                    try
                    {
                        System.Windows.Controls.ListBoxItem myItemExecutablesx = new ListBoxItem();
                        myItemExecutablesx.Content = reader4.GetString(0);
                        lbExecutables.Items.Add(myItemExecutablesx);
                    }
                    catch (Exception ex)
                    {
                        // Display error
                        MessageBox.Show("Error Code 019 [exception=" + ex.Message + "]");
                        throw;
                    }


                }
                con.Close();
                if (lbContent.Items.Count > 0)
                {
                    ListBoxItem li = (ListBoxItem)lbContent.Items[0];
                    li.IsSelected = true;

                }
                if (lbExecutables.Items.Count > 0)
                {
                    ListBoxItem li = (ListBoxItem)lbExecutables.Items[0];
                    li.IsSelected = true;

                }

            }

            else if (CategoryTree.SelectedItem.ParentCategory.ParentCategory.ParentCategory == null)
            {
                // we need to find the category id for the category folder so we can use it as the parent id
                // when we do the insert
                con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 

                cmd = new SqlCommand("USP_GET_CATEGORY_BY_TITLE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@paretCategoryID", SqlDbType.Int, 4);
                cmd.Parameters.Add("@categoryTitle", SqlDbType.VarChar, 200);
                Int32 intCategoryID = 0;
                try
                {
                    con.Open();
                    cmd.Parameters["@paretCategoryID"].Value = 0;
                    cmd.Parameters["@categoryTitle"].Value = CategoryTree.SelectedItem.ParentCategory.CategoryName.Trim();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        intCategoryID = reader.GetInt32(0);

                    }
                    reader.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Code 006 [exception=" + ex.Message + "]");
                }
                finally
                {

                    con.Close();
                }

                con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 

                cmd = new SqlCommand("USP_GET_CATEGORY_BY_TITLE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@paretCategoryID", SqlDbType.Int, 4);
                cmd.Parameters.Add("@categoryTitle", SqlDbType.VarChar, 200);
                //Int32 intCategoryID = 0;
                try
                {
                    con.Open();
                    cmd.Parameters["@paretCategoryID"].Value = intCategoryID;
                    cmd.Parameters["@categoryTitle"].Value = e.NewItem.ToString().Replace("Category: ","").Trim();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        intCategoryID = reader.GetInt32(0);

                    }
                    reader.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Code 007 [exception=" + ex.Message + "]");
                }
                finally
                {

                    con.Close();
                }

               
                // we now have the category id for the category folder and we can use that for the parent id when we insert
                con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))); 

                cmd = new SqlCommand("USP_GET_CATEGORY_DETAILS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int, 4);

                //cmd.Parameters.Add("@itemType", SqlDbType.Int);


                cmd.Parameters["@CategoryID"].Value = intCategoryID;

                //  cmd.Parameters["@itemType"].Value = 0;
                con.Open();
                SqlDataReader reader5;
                try
                {
                    reader5 = cmd.ExecuteReader();
                }
                catch (Exception ex)
                {
                    // Display error
                    MessageBox.Show("Error Code 020 [exception=" + ex.Message + "]");
                    throw;
                }

                while (reader5.Read())
                {

                    strRootToNode = reader5.GetString(3);


                }
                reader5.Close();
                con.Close();
                cmd = new SqlCommand("USP_GET_MYFILES_BY_TYPE_NODE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@rootToNodeKey", SqlDbType.VarChar, 500);
                cmd.Parameters.Add("@boolContent", SqlDbType.Bit);
                cmd.Parameters["@rootToNodeKey"].Value = strRootToNode;
                cmd.Parameters["@boolContent"].Value = true;
                //  cmd.Parameters["@itemType"].Value = 0;
                con.Open();
                SqlDataReader reader2;
                try
                {
                    reader2 = cmd.ExecuteReader();
                }
                catch (Exception ex)
                {

                    throw;
                }

                while (reader2.Read())
                {
                    try
                    {
                        System.Windows.Controls.ListBoxItem myItemC = new ListBoxItem();
                        myItemC.Content = reader2.GetString(0);
                        lbContent.Items.Add(myItemC);
                    }
                    catch (Exception ex)
                    {
                        // Display error
                        MessageBox.Show("Error Code 021 [exception=" + ex.Message + "]");
                        throw;
                    }


                }
                con.Close();

                cmd = new SqlCommand("USP_GET_MYFILES_BY_TYPE_NODE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@rootToNodeKey", SqlDbType.VarChar, 500);
                cmd.Parameters.Add("@boolContent", SqlDbType.Bit);
                cmd.Parameters["@rootToNodeKey"].Value = strRootToNode;
                cmd.Parameters["@boolContent"].Value = false;
                //  cmd.Parameters["@itemType"].Value = 0;
                con.Open();
                SqlDataReader reader4 = cmd.ExecuteReader();
                
                while (reader4.Read())
                {
                    try
                    {
                        System.Windows.Controls.ListBoxItem myItemExecutables2 = new ListBoxItem();
                        myItemExecutables2.Content = reader4.GetString(0);
                        lbExecutables.Items.Add(myItemExecutables2);
                    }
                    catch (Exception ex)
                    {
                        // Display error
                        MessageBox.Show("Error Code 022 [exception=" + ex.Message + "]");
                        throw;
                    }


                }
                con.Close();
                if (lbContent.Items.Count > 0)
                {
                    ListBoxItem li = (ListBoxItem)lbContent.Items[0];
                    li.IsSelected = true;

                }
                if (lbExecutables.Items.Count > 0)
                {
                    ListBoxItem li = (ListBoxItem)lbExecutables.Items[0];
                    li.IsSelected = true;

                }

            }

            if (boolExecutablesHasFocus == true)
            {

               
            }
        }

        #endregion


        #region change sort order

        /// <summary>
        /// Sets the sort order of the reference tree.
        /// </summary>
        private void ChangeSortOrder(object sender, RoutedEventArgs e)
        {
            bool asc = true; //(bool)rbAscending.IsChecked;
            string resourceName = asc ? "AscendingNames" : "DescendingNames";

            IEnumerable<SortDescription> sorts = (IEnumerable<SortDescription>)FindResource(resourceName);
            CategoryTree.NodeSortDescriptions = sorts;
        }

        #endregion


        /// <summary>
        /// Selects a given item on the tree if possible.
        /// </summary>
        private void SelectItem(object sender, RoutedEventArgs e)
        {
            string name = CategoryTree.SelectedItem == null ? null : CategoryTree.SelectedItem.CategoryName.Trim();
            name = ShowInputDialog(name);

            //if the model does not contain a matching category, just create
            //a dummy and create an exception
            ShopModel shop = GetShop();
            ShopCategory category = shop.TryFindCategoryByName(name);
            if (category == null)
            {
                category = CreateDummy(name, shop);
            }

            try
            {
                CategoryTree.SelectedItem = category;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Creates a random dummy item that is not part of the model's
        /// infrastructure. Using it will create an exception with the
        /// tree.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="shop"></param>
        /// <returns></returns>
        private ShopCategory CreateDummy(string category, ShopModel shop)
        {
            ShopCategory parent = null;
            Random rnd = new Random();
            int level = rnd.Next(0, 3);
            for (int i = 0; i < level; i++)
            {
                if (parent == null)
                {
                    //select a root item
                    int index = rnd.Next(0, shop.Categories.Count);
                    parent = shop.Categories[index];
                }
                else
                {
                    //select a child item
                    if (parent.SubCategories.Count == 0) break;
                    int index = rnd.Next(0, parent.SubCategories.Count);
                    parent = parent.SubCategories[index];
                }
            }

            return new ShopCategory(category, parent);
        }



        private void TabItem_Executables_GotFocus(object sender, RoutedEventArgs e)
        {
            
            boolExecutablesHasFocus = true;
            boolContentHasFocus = false;
            //if (txtNewItem.Text == "")
            //{
            //    lbExecutables.Items.Clear();
            //}
            //System.Windows.Controls.ListBoxItem myItem = new ListBoxItem();
            //if (CategoryTree.SelectedItem.CategoryName == "Web Browser")
            //{
            //    myItem.Content = "C:\\Program Files\\Internet Explorer\\iexplore.exe";
            //    lbExecutables.Items.Add(myItem);
            //}
        }

        private void TabItem_Content_GotFocus(object sender, RoutedEventArgs e)
        {
           
            boolExecutablesHasFocus = false;
            boolContentHasFocus = true;
            if (lbContent.Items.Count == 0)
            {

            }
            //if (txtNewItem.Text == "")
            //{
            //    lbContent.Items.Clear();
            //}
            //System.Windows.Controls.ListBoxItem myItem = new ListBoxItem();
            //if (CategoryTree.SelectedItem.CategoryName == "Web Browser")
            //{
            //    myItem.Content = "idealautomate.com";
            //    lbContent.Items.Add(myItem);
            //}
        }


        public void lbContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

       

         [System.Runtime.InteropServices.DllImport("user32", EntryPoint="SetWindowPos", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
 private static extern int SetWindowPos(int hwnd, int hWndInsertAfter, double x, double y, double cx, double cy, uint wFlags);
	private const int HWND_TOPMOST = -1;
	private const int HWND_NOTOPMOST = -2;
	private const int SWP_NOMOVE = 0X2;
	private const int SWP_NOSIZE = 0X1;
	private const int TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
//INSTANT C# TODO TASK: Insert the following converted event handler wireups at the end of the 'InitializeComponent' method for forms, 'Page_Init' for web pages, or into a constructor for other classes:


	public void MakeNormal(int hwnd)
	{


		SetWindowPos(hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
	}
	public void MakeTopMost(int hwnd)
	{
		SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
	}

	
    }


}
internal static class Keyboard
{
	[DllImport("kernel32.dll", CharSet=CharSet.Auto)]
	public extern static IntPtr GetModuleHandle(string lpModuleName);
	[System.Runtime.InteropServices.DllImport("user32", EntryPoint="UnhookWindowsHookEx", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
	public static extern int UnhookWindowsHookEx(int hHook);
    //[System.Runtime.InteropServices.DllImport("user32", EntryPoint="SetWindowsHookExA", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
    //public static extern int SetWindowsHookEx(int idHook, KeyboardHookDelegate lpfn, IntPtr hmod, int dwThreadId);
	[System.Runtime.InteropServices.DllImport("user32", EntryPoint="GetAsyncKeyState", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
	private static extern int GetAsyncKeyState(int vKey);
    //[System.Runtime.InteropServices.DllImport("user32", EntryPoint="CallNextHookEx", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
    //private static extern int CallNextHookEx(int hHook, int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);
	public struct KBDLLHOOKSTRUCT
	{
		public int vkCode;
		public int scanCode;
		public int flags;
		public int time;
		public int dwExtraInfo;
	}
	// Low-Level Keyboard Constants
	private const int HC_ACTION = 0;
	private const int LLKHF_EXTENDED = 0X1;
	private const int LLKHF_INJECTED = 0X10;
	private const int LLKHF_ALTDOWN = 0X20;
	private const int LLKHF_UP = 0X80;
	private const int LLKHF_DOWN = 0X81;
	// Virtual Keys
	public const int VK_TAB = 0X9;
	public const int VK_CONTROL = 0X11;
	public const int VK_ESCAPE = 0X1B;
	public const int VK_DELETE = 0X2E;
	private const int WH_KEYBOARD_LL = 13;
	public static int KeyboardHandle;
	// Implement this function to block as many
	// key combinations as you'd like...

    //public static int KeyboardCallback(int Code, int wParam, ref KBDLLHOOKSTRUCT lParam)
    //{
    //    if (Code == HC_ACTION)
    //    {
    //        Debug.WriteLine("Calling IsHooked");
    //        if (IsHooked(lParam))
    //        {
    //            return 1;
    //        }
    //    }
    //    return CallNextHookEx(KeyboardHandle, Code, wParam, ref lParam);
    //}
//    public delegate int KeyboardHookDelegate(int Code, int wParam, ref KBDLLHOOKSTRUCT lParam);
//    [MarshalAs(UnmanagedType.FunctionPtr)]
//    private static KeyboardHookDelegate callback;
//    public static void HookKeyboard(ref Form f)
//    {
//        callback = new KeyboardHookDelegate(KeyboardCallback);
//        KeyboardHandle = SetWindowsHookEx(WH_KEYBOARD_LL, callback, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
//        //MessageBox.Show(KeyboardHandle.ToString)
//        CheckHooked();
//    }

//    public static void CheckHooked()
//    {
//        if (Hooked())
//        {
//            Debug.WriteLine("Keyboard hooked");
//        }
//        else
//        {
////INSTANT C# TODO TASK: Calls to the VB 'Err' object are not converted by Instant C#:
//            Debug.WriteLine("Keyboard hook failed: " + Err.LastDllError);
//        }
//    }
//    private static bool Hooked()
//    {
//        return KeyboardHandle != 0;
//    }
//    public static void UnhookKeyboard()
//    {
//        if (Hooked())
//        {
//            UnhookWindowsHookEx(KeyboardHandle);
//        }
//    }



    public static string GetMainModuleFilepath(int processId)
    {
        string wmiQueryString = "SELECT ProcessId, ExecutablePath FROM Win32_Process WHERE ProcessId = " + processId;
        using (var searcher = new ManagementObjectSearcher(wmiQueryString))
        {
            using (var results = searcher.Get())
            {
                ManagementObject mo = results.Cast<ManagementObject>().FirstOrDefault();
                if (mo != null)
                {
                    return (string)mo["ExecutablePath"];
                }
            }
        }
        return null;
    }

}