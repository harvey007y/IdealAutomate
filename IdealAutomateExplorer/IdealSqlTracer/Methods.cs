using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.IO;
using System.Linq;




namespace IdealAutomate.Core {
  public class Methods {


    private bool fbDebugMode = true;
    private int intFileCtr = 0;
    bool boolUseGrayScaleDB = false;
    public bool DebugMode {
      get { return fbDebugMode; }
      set { fbDebugMode = value; }
    }

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    static extern IntPtr GetOpenClipboardWindow();

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    static extern int GetWindowText(int hwnd, StringBuilder text, int count);

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern int GetWindowTextLength(int hwnd);

    //Import the FindWindow API to find our window
    [DllImportAttribute("User32.dll")]
    private static extern int FindWindow(String ClassName, String WindowName);

    //Import the SetForeground API to activate it
    [DllImportAttribute("User32.dll")]
    private static extern IntPtr SetForegroundWindow(int hWnd);

    [DllImport("user32.dll", SetLastError=true,CharSet=CharSet.Auto) ]
    private static extern
      bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
    [DllImport("user32.dll")]
    private static extern
      bool IsIconic(IntPtr hWnd);

    private const int SW_HIDE = 0;
    private const int SW_SHOWNORMAL = 1;
    private const int SW_SHOWMINIMIZED = 2;
    private const int SW_SHOWMAXIMIZED = 3;
    private const int SW_SHOWNOACTIVATE = 4;
    private const int SW_RESTORE = 9;
    private const int SW_SHOWDEFAULT = 10;
    Process oProcess;
    public Methods() {

      oProcess = Process.GetCurrentProcess();
   
      string directory = AppDomain.CurrentDomain.BaseDirectory;
      DirectoryInfo dir = new DirectoryInfo(directory);

      foreach (FileInfo fi in dir.GetFiles()) {
        if (fi.Extension.ToUpper() == ".BMP" && fi.Name.StartsWith("temp")) {
          fi.Delete();
        }
      }
    }
       



        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SetForegroundWindow(IntPtr hwnd);
        [DllImport("user32.dll")]
        static extern bool ForceForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool AllowSetForegroundWindow(int dwProcessId);
        [System.Runtime.InteropServices.DllImport("user32.dll")]

        public static extern void SwitchToThisWindow(IntPtr hWnd, bool
 fAltTab);
        [DllImport("user32")]
        static extern int BringWindowToTop(IntPtr hwnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindow(IntPtr hWnd);
        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
        [DllImport("User32.DLL")]
        private static extern int AttachThreadInput(int CurrentForegroundThread, int MakeThisThreadForegrouond, bool boolAttach);
 

   
   
  /// <summary>
  /// <para>GetValueByKey returns the value for the specified key from KeyValueTable.</para>
    /// <para>The KeyValueTable allows you to store personal settings and</para>
    /// <para>information that you want to keep private (like passwords) in a location</para>
    /// <para>outside of your script on SQLExpress</para>
  /// </summary>
  /// <param name="pKey">The Key for the KeyValuePair</param>
  /// <param name="pInitialCatalog">usually IdealAutomateDB</param>
  /// <returns>String that is the Value for the KeyValuePair</returns>
   
    /// <summary>
    /// <para>WindowMultipleControls takes a list of ControlEntity objects</para>
    /// <para>and positions them in a window. When the user presses the </para>
    /// <para>okay button on the screen, the list of ControlEntity objects</para>
    /// <para>are updated with the values the user entered.  This provides</para>
    /// <para>an easy way to receive multiple values from the user</para>
    /// <para>A string is returned with the name of the button that was pressed</para>
    /// <para>Here is an example of setting background color for a button:</para>
    /// <para>myControlEntity.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);</para>
    /// </summary>
    /// <param name="myListControlEntity">list of ControlEntity objects</param>
    /// <param name="intWindowHeight">integer indicating height of window</param>
    /// <param name="intWindowWidth">integer indicating width of window</param>
    /// <param name="intWindowTop">integer indicating number of pixels from top of screen to display window</param>
    /// <param name="intWindowLeft">integer indicating number of pixels from left side of screen to display window</param>
    /// <returns>System.Windows.Forms.DialogResult to indicate if okay button was pressed</returns>
    public string WindowMultipleControls(ref List<ControlEntity> myListControlEntity, int intWindowHeight, int intWindowWidth,  int intWindowTop, int intWindowLeft) {
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "WindowMultipleControls");
       
      }
      WindowMultipleControls dlg = new WindowMultipleControls(ref myListControlEntity, intWindowHeight, intWindowWidth, intWindowTop, intWindowLeft, WindowState.Normal);

      // dlg.Owner = (Window)Window.GetWindow(this);
      // Shadow.Visibility = Visibility.Visible;
      dlg.ShowDialog();
      
      return dlg.strButtonClickedName;
      


    }
    /// <summary>
    /// <para>WindowMultipleControls takes a list of ControlEntity objects</para>
    /// <para>and positions them in a window. When the user presses the </para>
    /// <para>okay button on the screen, the list of ControlEntity objects</para>
    /// <para>are updated with the values the user entered.  This provides</para>
    /// <para>an easy way to receive multiple values from the user</para>
    /// <para>A string is returned with the name of the button that was pressed</para>
    /// <para>Here is an example of setting background color for a button:</para>
    /// <para>myControlEntity.BackgroundColor = System.Windows.Media.Color.FromRgb(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B);</para>
    /// </summary>
    /// <param name="myListControlEntity">list of ControlEntity objects</param>
    /// <param name="intWindowHeight">integer indicating height of window</param>
    /// <param name="intWindowWidth">integer indicating width of window</param>
    /// <param name="intWindowTop">integer indicating number of pixels from top of screen to display window</param>
    /// <param name="intWindowLeft">integer indicating number of pixels from left side of screen to display window</param>
    /// <returns>System.Windows.Forms.DialogResult to indicate if okay button was pressed</returns>
    public string WindowMultipleControlsMinimized(ref List<ControlEntity> myListControlEntity, int intWindowHeight, int intWindowWidth, int intWindowTop, int intWindowLeft) {
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "WindowMultipleControls");
       
      }
      WindowMultipleControls dlg = new WindowMultipleControls(ref myListControlEntity, intWindowHeight, intWindowWidth, intWindowTop, intWindowLeft, WindowState.Minimized);

      // dlg.Owner = (Window)Window.GetWindow(this);
      // Shadow.Visibility = Visibility.Visible;
      dlg.ShowDialog();
      

      return dlg.strButtonClickedName;



    }
    /// <summary>
    /// <para>WindowComboBox receives an IEnumerable of objects (ComboBoxPair) </para>
    /// <para>and a string for the label for the combobox. It returns the</para>
    /// <para>selected ComboBoxPair</para>
    /// </summary>
    /// <param name="myEntity">IEnumerable of objects</param>
    /// <param name="myEntity2">String for the label for the combobox</param>
    /// <returns>Selected ComboBoxPair</returns>
  
  
    /// <summary>
    /// <para>WindowShape allows you to display info to the user and to position that </para>
    /// <para>the window on the screen</para>
    /// </summary>
    /// <param name="myShape">string "Box" or "Arrow"</param>
    /// <param name="myOrientation">string "Left","Right","Up","Down",""</param>
    /// <param name="myTitle">string title for window</param>
    /// <param name="myContent">string content for window</param>
    /// <param name="intTop">integer indicating number of pixels from top of screen to display window</param>
    /// <param name="intLeft">integer indicating number of pixels from left of screen to display window</param>
    public void WindowShape(string myShape, string myOrientation, string myTitle, string myContent, int intTop, int intLeft) {
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "WindowShape: myEntity=" + myTitle);
      
      }
      WindowShape dlg = new WindowShape(myShape, myOrientation, myTitle, myContent, intTop, intLeft);

      // dlg.Owner = (Window)Window.GetWindow(this);
      // Shadow.Visibility = Visibility.Visible;
      dlg.ShowDialog();
      return;
    }
    /// <summary>
    /// MessageBoxShow receives an input string and displays it in a messagebox
    /// </summary>
    /// <param name="myEntity">string that you want to display in messagebox</param>
    public void MessageBoxShow(string myEntity) {
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "MessageBoxShow: myEntity=" + myEntity);
      
      }
      System.Windows.Forms.MessageBox.Show(myEntity, "Header", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.None,
    System.Windows.Forms.MessageBoxDefaultButton.Button1, (System.Windows.Forms.MessageBoxOptions)0x40000);  // MB_TOPMOST
    }
    /// <summary>
    /// MessageBoxShowWithYesNo receives an input string and displays it in a messagebox with Yes and No Buttons
    /// </summary>
    /// <param name="myEntity">string that you want to display in messagebox</param>

    /// <returns>System.Windows.Forms.DialogResult</returns>
  
    public void Run(string myEntityForExecutable, string myEntityForContent) {
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "Run: myEntityForExecutable=" + myEntityForExecutable + " myEntityForContent=" + myEntityForContent);
       
      }

      if (myEntityForExecutable == null) {
        string message = "Error - You need to specify executable primitive  "; // +"; EntityName is: " + myEntityForExecutable.EntityName;
       
        MessageBoxResult result = MessageBox.Show(message, "Run-time Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }
      string strExecutable = myEntityForExecutable;

      string strContent = "";
      if (myEntityForContent != null) {
        strContent = myEntityForContent;
      }
      if (strContent == "") {
        Process.Start(strExecutable);
      } else {
        try {
          Process.Start(strExecutable, string.Concat("", strContent, ""));
        } catch (Exception ex) {

          MessageBox.Show(ex.ToString());
        }
      }
    }
    /// <summary>
    /// <para>RunSync receives two input strings. The first is the path to the executable.</para>
    /// <para>The second is optional and it is the content you want to open with the executable.</para>
    /// <para>Run starts the executable as a thread and continues to the next statement</para>
    /// <para>AFTER the thread completes</para>
    /// </summary>
    /// <param name="myEntityForExecutable">string for the path of the executable</param>
    /// <param name="myEntityForContent">string for the content for the executable to open</param>

    public void RunSync(string myEntityForExecutable, string myEntityForContent) {
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "RunSync: myEntityForExecutable=" + myEntityForExecutable + " myEntityForContent=" + myEntityForContent);
      
      }

      if (myEntityForExecutable == null) {
        string message = "Error - You need to specify executable primitive  "; // +"; EntityName is: " + myEntityForExecutable.EntityName;
        MessageBoxResult result = MessageBox.Show(message, "Run-time Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }
      string strExecutable = myEntityForExecutable;

      string strContent = "";
      if (myEntityForContent != null) {
        strContent = myEntityForContent;
      }
      var p = new Process();
      p.StartInfo.FileName = strExecutable;
      if (strContent != "") {
        p.StartInfo.Arguments = string.Concat("", strContent, "");
      }
      bool started = true;
      p.Start();
      int procId = 0;
      try {
        procId = p.Id;
        Console.WriteLine("ID: " + procId);
        
      } catch (InvalidOperationException) {
        started = false;
      } catch (Exception ex) {
        started = false;
      }
      while (started == true && GetProcByID(procId) != null) {
        System.Threading.Thread.Sleep(1000);
      }

    }
    private Process GetProcByID(int id) {
      Process[] processlist = Process.GetProcesses();
      return processlist.FirstOrDefault(pr => pr.Id == id);
    }

  }
}
