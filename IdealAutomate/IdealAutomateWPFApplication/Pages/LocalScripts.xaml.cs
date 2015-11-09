using Hardcodet.Wpf.Samples;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media;
using System.Threading;
using System.Text;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.Management;
using WindowsInput;
using System.Data;
using System.Data.SqlClient;



namespace Hardcodet.Wpf.Samples.Pages
{
    //if (student.Action== "string" || 
    //     student.Action == "int" ||
    //     student.Action == "array" ||
    //     student.Action == "image" ||
    //     student.Action == "function" ||
    //     student.Action == "label")


    /// <summary>
    /// Interaction logic for IdealAutomater.xaml
    /// </summary>
    /// 
    public class Result
    {
        public ImageSource smallImageSource { get; set; }
        public ImageSource bigImageSource { get; set; }
        public string strResult { get; set; }

        private DateTime myNow;

        public DateTime dtNow
        {
            get { return myNow; }
            set { myNow = value; }
        }


    }
    public partial class LocalScripts : UserControl
    {
        SqlCommand cmd = new SqlCommand();
        # region DllImports

        /*- Retrieves Id of the thread that created the specified window -*/
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(int hWnd, out uint lpdwProcessId);

        /*- Retrieves information about active window or any specific GUI thread -*/
        [DllImport("user32.dll", EntryPoint = "GetGUIThreadInfo")]
        public static extern bool GetGUIThreadInfo(uint tId, out GUITHREADINFO threadInfo);


        /*- Converts window specific point to screen specific -*/
        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, out System.Drawing.Point position);



        # endregion
        System.Drawing.Point caretPosition;
        [StructLayout(LayoutKind.Sequential)]    // Required by user32.dll
        public struct RECT
        {
            public uint Left;
            public uint Top;
            public uint Right;
            public uint Bottom;
        };
        [StructLayout(LayoutKind.Sequential)]    // Required by user32.dll
        public struct GUITHREADINFO
        {
            public uint cbSize;
            public uint flags;
            public IntPtr hwndActive;
            public IntPtr hwndFocus;
            public IntPtr hwndCapture;
            public IntPtr hwndMenuOwner;
            public IntPtr hwndMoveSize;
            public IntPtr hwndCaret;
            public RECT rcCaret;
        };

        // Point required for ToolTip movement by Mouse
        GUITHREADINFO guiInfo;                     // To store GUI Thread Information

        public long prevElapsedTicks = 0;
        public long ticks;
        public long tempTicks;
        TimeSpan duration;

        double seconds;
        private Hardcodet.Wpf.Samples.IdealLauncherEntities _context = new Hardcodet.Wpf.Samples.IdealLauncherEntities();
        private List<DataGridCell> dataCellList = new List<DataGridCell>();
        List<DataGridCellInfo> dataCellInfoList = new List<DataGridCellInfo>();
        int myXprev;
        int myYprev;
        Bitmap bmprev;
        bool boolChildImage = false;
        private int intFileCtr = 0;
       
        int intSelectedScriptID;
        // fields we will replace with logic ((((((((((((((((((((
        string strConditionalLogic = "";
        int intAttemptsDB = 10;
        int intOccurrenceDB = 1;
        string strTextToType = "";
        bool boolUseGrayScaleDB = false;
        int intRelativeXDB = 0;
        int intRelativeYDB = 0;
        int intTolerance = 0;
        Script oScript = new Script();
      
        List<Result> listResults = new List<Result>();

        BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        bool boolCancelBtnPressed = false;
        Script selectedScript;
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
        string strScheduledScriptName = "";




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
        private static extern int GetWindowTextLengthx(int hwnd);
        [DllImport("user32.dll")]
        private extern static bool ShowWindow(IntPtr hWnd, int nCmdShow);
        bool boolContentHasFocus = true;
        bool boolExecutablesHasFocus = false;
        string strRootToNode = "";
        int intActionCtr = 0;
        private void HandleEsc(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Escape)
            {
                backgroundWorker1.CancelAsync();
                return;
            }

        }

        public LocalScripts(string pScheduledScriptName)
        {
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

            strScheduledScriptName = pScheduledScriptName;
            tempTicks = MainWindow.myStopwatch.ElapsedTicks;
            ticks = tempTicks - prevElapsedTicks;
            prevElapsedTicks = tempTicks;
            duration = new TimeSpan(ticks);
            seconds = duration.TotalSeconds;
            Console.WriteLine("IdealAutomater Constructor;" + seconds);
            InitializeComponent();
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;

        }

        public LocalScripts()
        {

            tempTicks = MainWindow.myStopwatch.ElapsedTicks;
            ticks = tempTicks - prevElapsedTicks;
            prevElapsedTicks = tempTicks;
            duration = new TimeSpan(ticks);
            seconds = duration.TotalSeconds;
            Console.WriteLine("IdealAutomater Constructor;" + seconds);
            InitializeComponent();


            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;

        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            WindowLoadedStuff();
            if (strScheduledScriptName != null && strScheduledScriptName != "")
            {
                try
                {
                    listResults = new List<Result>();
                    boolCancelBtnPressed = false;
                    selectedScript = (Script)scriptDataGrid.SelectedItems[0];
                    if (!backgroundWorker1.IsBusy)
                    {
                        backgroundWorker1.RunWorkerAsync();
                    }
                    else
                    {
                        MessageBox.Show("Sorry - Background Worker is busy; Can't run the worker twice!");
                    }

                }
                catch (Exception ex)
                {
                    Result oResulty = new Result();
                    oResulty.strResult = "EXCEPTION: " + ex.Message;
                    Console.WriteLine(oResulty.strResult);
                    oResulty.dtNow = System.DateTime.Now;
                    listResults.Add(oResulty);
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void WindowLoadedStuff()
        {
            try
            {
                tempTicks = MainWindow.myStopwatch.ElapsedTicks;
                ticks = tempTicks - prevElapsedTicks;
                prevElapsedTicks = tempTicks;
                duration = new TimeSpan(ticks);
                seconds = duration.TotalSeconds;
                Console.WriteLine("Begin Window Loaded;" + seconds);
                System.Windows.Data.CollectionViewSource scriptViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("scriptViewSource")));
               

                // Load data by setting the CollectionViewSource.Source property:
                // scriptViewSource.Source = [generic data source]
                // Load is an extension method on IQueryable,  
                // defined in the System.Data.Primitive namespace. 
                // This method enumerates the results of the query,  
                // similar to ToList but without creating a list. 
                // When used with Linq to Primitives this method  
                // creates primitive objects and adds them to the context. 
                //_context.Configuration.LazyLoadingEnabled = true;
            

                _context.Scripts.Load();
        



                // After the data is loaded call the DbSet<T>.Local Property  
                // to use the DbSet<T> as a binding source. 
                if (txtTextSearch.Text != IAUserInfo.TextSearch)
                {
                    txtTextSearch.Text = IAUserInfo.TextSearch;
                }

                scriptViewSource.Source = _context.Scripts.Local;
                LoadFilter();
                scriptViewSource.Filter += scriptViewSource_Filter;
              
                DataGridCellInfo info;
                if (scriptDataGrid.SelectedCells.Count > 0)
                {
                    info = scriptDataGrid.SelectedCells[0];
                }
                else
                {
                    info = new DataGridCellInfo();

                }
                Script myScript;

                if (info.Item == null || info.Item.ToString() == "{NewItemPlaceholder}" || info.Item.ToString() == "{DependencyProperty.UnsetValue}")
                {
                    myScript = new Script();
                    myScript.ScriptID = 0;
                    oScript = myScript;
                }
                else
                {
                    myScript = (Script)info.Item;
                    oScript = myScript;
                }
                if (myScript.ScriptID == 0)
                {
                    oScript = _context.Scripts.Local.First<Script>();
                }
            
               
             
                tempTicks = MainWindow.myStopwatch.ElapsedTicks;
                ticks = tempTicks - prevElapsedTicks;
                prevElapsedTicks = tempTicks;
                duration = new TimeSpan(ticks);
                seconds = duration.TotalSeconds;
                Console.WriteLine("End Window Loaded;" + seconds);
                // here is the selectionchanged event stuff for scripts
                if (strScheduledScriptName != "")
                {
                    myScript = _context.Scripts.Local.Where(x => x.ScriptName == strScheduledScriptName).FirstOrDefault();
                    if (myScript == null)
                    {
                        MessageBox.Show("Unable to find local script " + strScheduledScriptName);
                        strScheduledScriptName = "";
                        return;
                    }
                    else
                    {
                        scriptDataGrid.SelectedItem = myScript;
                    }
                }
                if (myScript.ScriptID == 0 || strScheduledScriptName != "")
                {
                    //CollectionViewSource primitivesViewSource = ((CollectionViewSource)(FindResource("primitivesViewSource")));
                    //primitivesViewSource.Source = null;
                }
                else
                {
                    oScript = (Script)info.Item;
                    txtSelectedScript.Text = oScript.ScriptName;
                    //CollectionViewSource primitivesViewSource = ((CollectionViewSource)(FindResource("primitivesViewSource")));
                    //CollectionViewSource arraysViewSource = ((CollectionViewSource)(FindResource("arraysViewSource")));
                    //CollectionViewSource picturesViewSource = ((CollectionViewSource)(FindResource("picturesViewSource")));
                    //CollectionViewSource expressionsViewSource = ((CollectionViewSource)(FindResource("expressionsViewSource")));
                    //CollectionViewSource sqlcmdsViewSource = ((CollectionViewSource)(FindResource("sqlcmdsViewSource")));
                    //CollectionViewSource integerEntitiesViewSource = ((CollectionViewSource)(FindResource("integerEntitiesViewSource")));
                    //CollectionViewSource entitiesViewSource = ((CollectionViewSource)(FindResource("entitiesViewSource")));

                    //Primitive myPrimitive = new Primitive();
                    //myPrimitive.ScriptID = oScript.ScriptID;
                    //oScript.Primitives.Add(myPrimitive);  




                    this.scriptDataGrid.Items.Refresh();
                    //  this.scriptStepsDataGrid.Items.Refresh();
                

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.InnerException.ToString());
                // Log error (including InnerExceptions!)
                // Handle exception
            }
        }

        private void LoadFilter()
        {
            ObjectDataProvider category1 = (ObjectDataProvider)FindResource("category1");
            ObjectDataProvider category2 = (ObjectDataProvider)FindResource("category2");
            ObjectDataProvider category3 = (ObjectDataProvider)FindResource("category3");
            ObjectDataProvider category4 = (ObjectDataProvider)FindResource("category4");
            ObjectDataProvider category5 = (ObjectDataProvider)FindResource("category5");
 


            ObservableCollection<string> myCategory1 = new ObservableCollection<string>();
            foreach (Script myScript in _context.Scripts.Local.OrderBy(x => x.Category1))
            {
                if (myScript.Category1 != null)
                {
                    myCategory1.Add(myScript.Category1);
                }
            }
            myCategory1.Insert(0, "n/a");
            category1.ObjectInstance = myCategory1.Distinct<string>();

            ObservableCollection<string> myCategory2 = new ObservableCollection<string>();
            foreach (Script myScript2 in _context.Scripts.Local.Where<Script>(x => x.Category1 == IAUserInfo.Category1).OrderBy(x => x.Category2))
            {
                if (myScript2.Category2 != null)
                {
                    myCategory2.Add(myScript2.Category2);
                }
            }
            myCategory2.Insert(0, "n/a");
            category2.ObjectInstance = myCategory2.Distinct<string>();

            ObservableCollection<string> myCategory3 = new ObservableCollection<string>();
            foreach (Script myScript3 in _context.Scripts.Local.Where<Script>(x => x.Category2 == IAUserInfo.Category2).OrderBy(x => x.Category3))
            {
                if (myScript3.Category3 != null)
                {
                    myCategory3.Add(myScript3.Category3);
                }
            }
            myCategory3.Insert(0, "n/a");
            category3.ObjectInstance = myCategory3.Distinct<string>();

            ObservableCollection<string> myCategory4 = new ObservableCollection<string>();
            foreach (Script myScript4 in _context.Scripts.Local.Where<Script>(x => x.Category3 == IAUserInfo.Category3).OrderBy(x => x.Category4))
            {
                if (myScript4.Category4 != null)
                {
                    myCategory4.Add(myScript4.Category4);
                }
            }
            myCategory4.Insert(0, "n/a");
            category4.ObjectInstance = myCategory4.Distinct<string>();

            ObservableCollection<string> myCategory5 = new ObservableCollection<string>();
            foreach (Script myScript5 in _context.Scripts.Local.Where<Script>(x => x.Category4 == IAUserInfo.Category4).OrderBy(x => x.Category5))
            {
                if (myScript5.Category5 != null)
                {
                    myCategory5.Add(myScript5.Category5);
                }
            }
            myCategory5.Insert(0, "n/a");
            category5.ObjectInstance = myCategory5.Distinct<string>();

            comboCategory1.SelectedValue = IAUserInfo.Category1;
            comboCategory2.SelectedValue = IAUserInfo.Category2;
            comboCategory3.SelectedValue = IAUserInfo.Category3;
            comboCategory4.SelectedValue = IAUserInfo.Category4;
            comboCategory5.SelectedValue = IAUserInfo.Category5;

        }



        void scriptViewSource_Filter(object sender, FilterEventArgs e)
        {
            Script myScript = e.Item as Script;
            string strSearchTerm = txtTextSearch.Text.ToUpper();
            if (strSearchTerm != "")
            {
                if (myScript.ScriptName.ToUpper().Contains(strSearchTerm))
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                    return;
                }
            }
            if (comboCategory5.SelectedValue != null)
            {
                if (comboCategory5.SelectedValue.ToString() != "" && comboCategory5.SelectedValue.ToString() != "n\a")
                {
                    if (myScript.Category5 == comboCategory5.SelectedValue.ToString() &&
                        myScript.Category4 == comboCategory4.SelectedValue.ToString() &&
                        myScript.Category3 == comboCategory3.SelectedValue.ToString() &&
                        myScript.Category2 == comboCategory2.SelectedValue.ToString() &&
                        myScript.Category1 == comboCategory1.SelectedValue.ToString()
                        )
                    {
                        e.Accepted = true;
                        return;
                    }
                    else
                    {
                        e.Accepted = false;
                        return;
                    }
                }

            }
            if (comboCategory4.SelectedValue != null)
            {
                if (comboCategory4.SelectedValue.ToString() != "" && comboCategory4.SelectedValue.ToString() != "n\a")
                {
                    if (myScript.Category4 == comboCategory4.SelectedValue.ToString() &&
                        myScript.Category3 == comboCategory3.SelectedValue.ToString() &&
                        myScript.Category2 == comboCategory2.SelectedValue.ToString() &&
                        myScript.Category1 == comboCategory1.SelectedValue.ToString()
                        )
                    {
                        e.Accepted = true;
                        return;
                    }
                    else
                    {
                        e.Accepted = false;
                        return;
                    }
                }
            }

            if (comboCategory3.SelectedValue != null)
            {
                if (comboCategory3.SelectedValue.ToString() != "" && comboCategory3.SelectedValue.ToString() != "n\a")
                {
                    if (myScript.Category3 == comboCategory3.SelectedValue.ToString() &&
                        myScript.Category2 == comboCategory2.SelectedValue.ToString() &&
                        myScript.Category1 == comboCategory1.SelectedValue.ToString()
                        )
                    {
                        e.Accepted = true;
                        return;
                    }
                    else
                    {
                        e.Accepted = false;
                        return;
                    }
                }
            }

            if (comboCategory2.SelectedValue != null)
            {
                if (comboCategory2.SelectedValue.ToString() != "" && comboCategory2.SelectedValue.ToString() != "n\a")
                {
                    if (myScript.Category2 == comboCategory2.SelectedValue.ToString() &&
                        myScript.Category1 == comboCategory1.SelectedValue.ToString()
                        )
                    {
                        e.Accepted = true;
                        return;
                    }
                    else
                    {
                        e.Accepted = false;
                        return;
                    }
                }
            }

            if (comboCategory1.SelectedValue != null)
            {
                if (comboCategory1.SelectedValue.ToString() != "" && comboCategory1.SelectedValue.ToString() != "n\a")
                {
                    if (myScript.Category1 == comboCategory1.SelectedValue.ToString()
                        )
                    {
                        e.Accepted = true;
                        return;
                    }
                    else
                    {
                        e.Accepted = false;
                        return;
                    }
                }
            }
            if (comboCategory1.SelectedValue == null)
            {
                e.Accepted = true;
                return;
            }
            else
            {
                if (comboCategory1.SelectedValue.ToString() == "" || comboCategory1.SelectedValue.ToString() == "n\a")
                {

                    e.Accepted = true;
                    return;

                }
            }
        }
   

        private void UpdateCategories()
        {
           

            SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)));
            con1.Open();
            string updCmd1 =
                  "UPDATE [dbo].[UserInfo] " +
                "SET [Category1] = '" + IAUserInfo.Category1 +
                 "',[Category2] =  '" + IAUserInfo.Category2 +
                 "',[Category3] =  '" + IAUserInfo.Category3 +
                 "',[Category4] =  '" + IAUserInfo.Category4 +
                 "',[Category5] =  '" + IAUserInfo.Category5 +
                  "',[TextSearch] = '" + IAUserInfo.TextSearch +             
                 "' WHERE ID = " + IAUserInfo.ID;
            SqlCommand cmd1 = new SqlCommand(updCmd1, con1);
            cmd1.CommandType = CommandType.Text;
            cmd1.ExecuteNonQuery();
            con1.Close();


        }
        private void btnReset_Clicked(object sender, RoutedEventArgs e)
        {
            

            SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)));
            con1.Open();
            string updCmd1 =
                  "UPDATE [dbo].[UserInfo] " +
                "SET [Category1] = '" +
                 "',[Category2] =  '" +
                 "',[Category3] =  '" +
                 "',[Category4] =  '" +
                 "',[Category5] =  '" +
                  "',[TextSearch] = '" +
                 "' WHERE ID = " + IAUserInfo.ID;
            SqlCommand cmd1 = new SqlCommand(updCmd1, con1);
            cmd1.CommandType = CommandType.Text;
            cmd1.ExecuteNonQuery();
            con1.Close();
            IAUserInfo.Category1 = "";
            IAUserInfo.Category2 = "";
            IAUserInfo.Category3 = "";
            IAUserInfo.Category4 = "";
            IAUserInfo.Category5 = "";
            IAUserInfo.TextSearch = "";

            comboCategory1.SelectedIndex = -1;
            comboCategory2.SelectedIndex = -1;
            comboCategory3.SelectedIndex = -1;
            comboCategory4.SelectedIndex = -1;
            comboCategory5.SelectedIndex = -1;
            txtTextSearch.Text = "";


            WindowLoadedStuff();
        }



 
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            // When you delete an object from the related primitives collection  
            // (in this case Products), the Primitive Framework doesn’t mark  
            // these child primitives as deleted. 
            // Instead, it removes the relationship between the parent and the child 
            // by setting the parent reference to null. 
            // So we manually have to delete the products  
            // that have a Category reference set to null. 

            // The following code uses LINQ to Objects  
            // against the Local collection of Products. 
            // The ToList call is required because otherwise the collection will be modified 
            // by the Remove call while it is being enumerated. 
            // In most other situations you can use LINQ to Objects directly  
            // against the Local property without using ToList first.
            int myUserID = IAUserInfo.ID;


            try
            {
                

                foreach (var entry in _context.ChangeTracker.Entries()
                                           .Where(f => f.State == EntityState.Deleted))
                {
                    if (ObjectContext.GetObjectType(entry.Entity.GetType()).Name == "Script")
                    {
                        Script myDeletedScript = new Script();
                        myDeletedScript.ScriptID = ((Script)(entry.Entity)).ScriptID;

                       
               //         _contextLKTables.SaveChanges();
                    }

                }

                try
                {
                    oScript.LastModifiedDate = System.DateTime.Now;
                    _context.SaveChanges();
                    string message;
                    message = "Data Saved Successfully";
                    MessageBoxResult result = MessageBox.Show(message, "Data Saved", MessageBoxButton.OK, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                    throw;
                }
                // Refresh the grids so the database generated values show up. 
                System.Windows.Data.CollectionViewSource scriptViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("scriptViewSource")));
              


                this.scriptDataGrid.Items.Refresh();
 
                WindowLoadedStuff();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.InnerException.ToString());
                // Log error (including InnerExceptions!)
                // Handle exception
            }
        }

 



   

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
          
          


           

            // Refresh the grids so the database generated values show up. 
            System.Windows.Data.CollectionViewSource scriptViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("scriptViewSource")));
         


            this.scriptDataGrid.Items.Refresh();
         

        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr GetOpenClipboardWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int GetWindowText(int hwnd, StringBuilder text, int count);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int GetWindowTextLength(int hwnd);

        private static string GetOpenClipboardWindowText()
        {
            var hwnd = GetOpenClipboardWindow();
            if (hwnd == IntPtr.Zero)
            {
                return "Unknown";
            }
            var int32Handle = hwnd.ToInt32();
            var len = GetWindowTextLength(int32Handle);
            var sb = new StringBuilder(len);
            GetWindowText(int32Handle, sb, len);
            return sb.ToString();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DoScriptActions(selectedScript, e);
        }
        private void btnRunScript_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                listResults = new List<Result>();
                boolCancelBtnPressed = false;
                selectedScript = (Script)scriptDataGrid.SelectedItems[0];
                if (!backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show("Sorry - Background Worker is busy; Can't run the worker twice!");
                }

            }
            catch (Exception ex)
            {
                Result oResulty = new Result();
                oResulty.strResult = "EXCEPTION: " + ex.Message;
                Console.WriteLine(oResulty.strResult);
                oResulty.dtNow = System.DateTime.Now;
                listResults.Add(oResulty);
                MessageBox.Show(ex.Message);
            }


        }

        public static Boolean IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                //Don't change FileAccess to ReadWrite, 
                //because if a file is in readOnly, it fails.
                stream = file.Open
                (
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.None
                );
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

 
        public static Bitmap BytesToBitmap(byte[] byteArray)
        {


            using (MemoryStream ms = new MemoryStream(byteArray))
            {


                Bitmap img = (Bitmap)System.Drawing.Image.FromStream(ms);


                return img;


            }
        }
        public static BitmapSource BitmapSourceFromImage(System.Drawing.Image img)
        {
            MemoryStream memStream = new MemoryStream();

            // save the image to memStream as a png
            img.Save(memStream, System.Drawing.Imaging.ImageFormat.Png);

            // gets a decoder from this stream
            System.Windows.Media.Imaging.PngBitmapDecoder decoder = new System.Windows.Media.Imaging.PngBitmapDecoder(memStream, System.Windows.Media.Imaging.BitmapCreateOptions.PreservePixelFormat, System.Windows.Media.Imaging.BitmapCacheOption.Default);

            return decoder.Frames[0];
        }

        private void scriptDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            DataGridCellInfo info;
            if (scriptDataGrid.SelectedCells.Count > 0)
            {
                info = scriptDataGrid.SelectedCells[0];
            }
            else
            {
                info = new DataGridCellInfo();

            }
            Script myScript;

            if (info.Item == null || info.Item.ToString() == "{NewItemPlaceholder}" || info.Item.ToString() == "{DependencyProperty.UnsetValue}")
            {
                myScript = new Script();
                myScript.ScriptID = 0;
            }
            else
            {
                myScript = (Script)info.Item;
            }

            if (myScript.ScriptID == 0)
            {
                //CollectionViewSource primitivesViewSource = ((CollectionViewSource)(FindResource("primitivesViewSource")));
                //primitivesViewSource.Source = null;
            }
            else
            {
                oScript = (Script)info.Item;
                txtSelectedScript.Text = oScript.ScriptName;
              


                this.scriptDataGrid.Items.Refresh();

                // this.scriptStepsDataGrid.Items.Refresh();
            }

        }




     


        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

            boolCancelBtnPressed = true;
            backgroundWorker1.CancelAsync();
        }
        public void DoScriptActions(Script selectedScript, DoWorkEventArgs e)
        {
            
                string strExecutable = selectedScript.Executable;


                  string   strContent = selectedScript.ExecuteContent;
        
                if (strContent == "")
                {
                    Process.Start(strExecutable);
                }
                else
                {
                    try
                    {
                        Process.Start(strExecutable, string.Concat("", strContent, ""));
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.ToString());
                    }
                }
          
            selectedScript.LastExecutedDate = System.DateTime.Now;
            selectedScript.NumberTimesExecuted += 1;
            selectedScript.NumberActionsSaved += intActionCtr;

            
         

            // first foreach finds all records where userid is equal to
            // the logged in user id and local scriptid is equal to the
            // selected scriptid
      
        }
        private void ShowHelpDialog(object sender, RoutedEventArgs e)
        {
            NavWindowAutomater dlg = new NavWindowAutomater();
            dlg.Owner = (Window)this.Parent;
            // Shadow.Visibility = Visibility.Visible;
            dlg.Show();
            //Shadow.Visibility = Visibility.Collapsed;
        }

        private void comboCategory1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboCategory1.SelectedValue == null)
            {
                //   IAUserInfo.Category1 = "";
            }
            else
            {
                IAUserInfo.Category1 = comboCategory1.SelectedValue.ToString();
            }
            LoadFilter();
            UpdateCategories();
            // Refresh the grids so the database generated values show up. 
            System.Windows.Data.CollectionViewSource scriptViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("scriptViewSource")));
        


            this.scriptDataGrid.Items.Refresh();
          
            WindowLoadedStuff();
        }

        private void comboCategory2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboCategory2.SelectedValue != null)
            {
                IAUserInfo.Category2 = comboCategory2.SelectedValue.ToString();
            }
            LoadFilter();
            UpdateCategories();
            // Refresh the grids so the database generated values show up. 
            System.Windows.Data.CollectionViewSource scriptViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("scriptViewSource")));
         


            this.scriptDataGrid.Items.Refresh();
          
            WindowLoadedStuff();
        }

        private void comboCategory3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboCategory3.SelectedValue != null)
            {
                IAUserInfo.Category3 = comboCategory3.SelectedValue.ToString();
            }
            LoadFilter();
            UpdateCategories();
            // Refresh the grids so the database generated values show up. 
            System.Windows.Data.CollectionViewSource scriptViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("scriptViewSource")));
   
            this.scriptDataGrid.Items.Refresh();           
            WindowLoadedStuff();
        }

        private void comboCategory4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboCategory4.SelectedValue != null)
            {
                IAUserInfo.Category4 = comboCategory4.SelectedValue.ToString();
            }
            LoadFilter();
            UpdateCategories();
            // Refresh the grids so the database generated values show up. 
            System.Windows.Data.CollectionViewSource scriptViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("scriptViewSource")));



            this.scriptDataGrid.Items.Refresh();
         
            WindowLoadedStuff();
        }

        private void comboCategory5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboCategory5.SelectedValue != null)
            {
                IAUserInfo.Category5 = comboCategory5.SelectedValue.ToString();
            }
            LoadFilter();
            UpdateCategories();
            // Refresh the grids so the database generated values show up. 
            System.Windows.Data.CollectionViewSource scriptViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("scriptViewSource")));
          


            this.scriptDataGrid.Items.Refresh();
         
            WindowLoadedStuff();
        }

        private void txtTextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            IAUserInfo.TextSearch = txtTextSearch.Text;
            LoadFilter();
            UpdateCategories();
            // Refresh the grids so the database generated values show up. 
            System.Windows.Data.CollectionViewSource scriptViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("scriptViewSource")));
           


            this.scriptDataGrid.Items.Refresh();         
            WindowLoadedStuff();
        }

   

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var runExplorer = new System.Diagnostics.ProcessStartInfo();
            runExplorer.FileName = "explorer.exe";
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            runExplorer.Arguments = directory;
            System.Diagnostics.Process.Start(runExplorer);

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

            TitleLength = GetWindowTextLengthx(hWnd.ToInt32());

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
                txtExecutable.Text = myFileName;
                dispatcherTimer.Stop();

                // MessageBox.Show(myFileName);
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

       
        }
        [System.Runtime.InteropServices.DllImport("user32", EntryPoint = "SetWindowPos", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
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

        /// <summary>
        /// Evaluates Cursor Position with respect to client screen.
        /// </summary>
        private void EvaluateCaretPosition()
        {

            caretPosition = new System.Drawing.Point();

            // Fetch GUITHREADINFO
            GetCaretPosition();

            caretPosition.X = (int)guiInfo.rcCaret.Left;
            caretPosition.Y = (int)guiInfo.rcCaret.Top;
            //    MessageBox.Show(caretPosition.X.ToString() + "," + caretPosition.Y.ToString());
            ClientToScreen(guiInfo.hwndCaret, out caretPosition);
            //      MessageBox.Show(caretPosition.X.ToString() + "," + caretPosition.Y.ToString());

            //txtCaretX.Text = (caretPosition.X).ToString();
            //txtCaretY.Text = caretPosition.Y.ToString();

        }

        /// <summary>
        /// Get the caret position
        /// </summary>
        public void GetCaretPosition()
        {
            guiInfo = new GUITHREADINFO();
            guiInfo.cbSize = (uint)Marshal.SizeOf(guiInfo);

            // Get GuiThreadInfo into guiInfo
            GetGUIThreadInfo(0, out guiInfo);
        }

        /// <summary>
        /// Retrieves name of active Process.
        /// </summary>
        /// <returns>Active Process Name</returns>
        private string GetActiveProcess()
        {
            const int nChars = 256;
            int handle = 0;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = (int)GetForegroundWindow();

            // If Active window has some title info
            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                uint lpdwProcessId;
                uint dwCaretID = GetWindowThreadProcessId(handle, out lpdwProcessId);
                uint dwCurrentID = (uint)Thread.CurrentThread.ManagedThreadId;
                return Process.GetProcessById((int)lpdwProcessId).ProcessName;
            }
            // Otherwise either error or non client region
            return String.Empty;
        }
        private string GetActiveProcessTitle()
        {
            const int nChars = 256;
            int handle = 0;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = (int)GetForegroundWindow();

            // If Active window has some title info
            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                uint lpdwProcessId;
                uint dwCaretID = GetWindowThreadProcessId(handle, out lpdwProcessId);
                uint dwCurrentID = (uint)Thread.CurrentThread.ManagedThreadId;
                return Process.GetProcessById((int)lpdwProcessId).MainWindowTitle;
            }
            // Otherwise either error or non client region
            return String.Empty;
        }

    }


}
internal static class Keyboard
{
    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public extern static IntPtr GetModuleHandle(string lpModuleName);
    [System.Runtime.InteropServices.DllImport("user32", EntryPoint = "UnhookWindowsHookEx", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
    public static extern int UnhookWindowsHookEx(int hHook);
    //[System.Runtime.InteropServices.DllImport("user32", EntryPoint="SetWindowsHookExA", ExactSpelling=true, CharSet=System.Runtime.InteropServices.CharSet.Ansi, SetLastError=true)]
    //public static extern int SetWindowsHookEx(int idHook, KeyboardHookDelegate lpfn, IntPtr hmod, int dwThreadId);
    [System.Runtime.InteropServices.DllImport("user32", EntryPoint = "GetAsyncKeyState", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
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
