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
using WindowsInput;
using WindowsInput.Native;
using System.IO;
using System.Linq;
using System.Reflection;
using IdealAutomateCore;
using System.Windows.Controls;
using System.ServiceProcess;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Snipping_OCR;

namespace IdealAutomate.Core {
    public class Methods: IDisposable {


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

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
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
            string settingsDirectory = GetAppDirectoryForScript();
            if (!Directory.Exists(settingsDirectory)) {
                Directory.CreateDirectory(settingsDirectory);
            }
            string filePath = Path.Combine(settingsDirectory, "IdealAutomateLog.txt");
            //System.Web.HttpContext.Current.Server.MapPath("~//Trace.html")
            StreamWriter sw = null;

            try {
                if (File.Exists(filePath)) {
                    // TODO: Wade - uncomment the following - just commented it out for debugging
                  //  File.Delete(filePath);
                }
            } catch (Exception) {


            }
            oProcess = Process.GetCurrentProcess();
            BreakOnPauseKeyPress();
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dir = new DirectoryInfo(directory);

            foreach (FileInfo fi in dir.GetFiles()) {
                if (fi.Extension.ToUpper() == ".BMP" && fi.Name.StartsWith("temp")) {
                    fi.Delete();
                }
            }
        }
        private static void ForceForegroundWindow1(IntPtr hWnd, int Status, int myShowOption) {

            uint foreThread = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);

            uint appThread = GetCurrentThreadId();

            int SW_SHOW = 5;
            if (Status == 0)
                SW_SHOW = 3;

            if (foreThread != appThread) {

                AttachThreadInput((int)foreThread, (int)appThread, true);

                BringWindowToTop(hWnd);

                ShowWindow(hWnd, myShowOption);

                AttachThreadInput((int)foreThread, (int)appThread, false);

            } else {

                BringWindowToTop(hWnd);

                ShowWindow(hWnd, myShowOption);

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
        // Category::Window
        public bool ActivateWindowByTitle(string myTitle) {

            //Find the window, using the CORRECT Window Title, for example, Notepad
            int hWnd = FindWindow(null, myTitle);
            if (hWnd > 0) //If found
            {
                {
                    ShowWindowAsync((IntPtr)hWnd, SW_RESTORE);
                }
                SetForegroundWindow(hWnd); //Activate it              
                return true;
            } else {
                MessageBox.Show("Window Not Found! - ActivateWindowByTitle:" + myTitle + " You can try to manually activate it and then click okay on this popup");
                return false;
            }

        }
        /// <summary>
        /// SW_HIDE = 0;
        /// SW_SHOWNORMAL = 1;
        /// SW_SHOWMINIMIZED = 2;
        /// SW_SHOWMAXIMIZED = 3;
        /// SW_SHOWNOACTIVATE = 4;
        /// SW_RESTORE = 9;
        /// SW_SHOWDEFAULT = 10;
        /// </summary>
        /// <param name="myTitle"></param>
        /// <param name="myShowOption"></param>
        /// <returns></returns>
        /// Category::Window
        public bool ActivateWindowByTitle(string myTitle, int myShowOption) {

            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "ActivateWindowByTitle: myTitle=" + myTitle);
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "ActivateWindowByTitle: myTitle = " + myTitle);
            }
            //Find the window, using the CORRECT Window Title, for example, Notepad
            int hWnd = FindWindow(null, myTitle);
            Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "ActivateWindowByTitle: hWnd = " + hWnd.ToString());
            if (hWnd > 0) //If found
            {
                {
                    ShowWindow((IntPtr)hWnd, myShowOption);
                }
                ForceForegroundWindow1((IntPtr)hWnd, 1, myShowOption); //Activate it 
                return true;
            } else {
                MessageBox.Show("Window Not Found! - ActivateWindowByTitle:" + myTitle + " You can try to manually activate it and then click okay on this popup");
                return false;
            }

        }
        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);



        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("USER32.DLL")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("USER32.DLL")]
        private static extern IntPtr GetShellWindow();

        public IDictionary<IntPtr, string> GetOpenWindowsFromPID(int processID) {
            IntPtr hShellWindow = GetShellWindow();
            Dictionary<IntPtr, string> dictWindows = new Dictionary<IntPtr, string>();

            EnumWindows(delegate (IntPtr hWnd, int lParam) {
                if (hWnd == hShellWindow) return true;
                if (!IsWindowVisible(hWnd)) return true;

                int length = GetWindowTextLength(hWnd);
                if (length == 0) return true;

                uint windowPid;
                GetWindowThreadProcessId(hWnd, out windowPid);
                if (windowPid != processID) return true;

                StringBuilder stringBuilder = new StringBuilder(length);
                GetWindowText(hWnd, stringBuilder, length + 1);
                dictWindows.Add(hWnd, stringBuilder.ToString());
                return true;
            }, 0);

            return dictWindows;
        }
        /// <summary>
        /// GetWindowTitlesByProcessName allows you to get a list of all the WindowTitles
        /// for a process name. ProcessNames are called image names in task manager.
        /// You can find all of the iexplore or devenv and activate each one to see if it 
        /// is the one you want
        /// </summary>
        /// <param name="myProcessName"></param>
        /// <returns></returns>
        /// Category::Window
        public List<string> GetWindowTitlesByProcessName(string myProcessName) {
            List<string> lstWindowTitles = new List<string>();

            Process[] processesx = Process.GetProcessesByName(myProcessName);
            foreach (Process process in processesx) {
                IDictionary<IntPtr, string> windows = this.GetOpenWindowsFromPID(process.Id);
                foreach (KeyValuePair<IntPtr, string> kvp in windows) {
                    // Do whatever you want here
                    lstWindowTitles.Add(kvp.Value);
                }
            }

            return lstWindowTitles;

        }
        /// Category::Processes
        public void KillAllProcessesByProcessName(string myProcessName) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "KillAllProcessesByProcessName: myProcessName=" + myProcessName);
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "KillAllProcessesByProcessName: myProcessName = " + myProcessName);
            }
            try {
                foreach (Process proc in Process.GetProcessesByName(myProcessName)) {
                    if (fbDebugMode) {
                        Console.WriteLine(oProcess.ProcessName + "==> " + "Killing: proc=" + proc.ToString());
                        Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "Killing: proc=" + proc.ToString());
                    }
                    proc.Kill();
                }
            } catch (Exception ex) {
                MessageBox.Show("Here is an exception thrown in the KillAllProcessesByProcessName method in IdealAutomateCore for ProcessName " + myProcessName + ": " + ex.Message);
            }

        }

        private static string GetOpenClipboardWindowText() {

            var hwnd = GetOpenClipboardWindow();
            if (hwnd == IntPtr.Zero) {
                return "Unknown";
            }
            var int32Handle = hwnd.ToInt32();
            var len = GetWindowTextLength(int32Handle);
            var sb = new StringBuilder(len);
            GetWindowText(int32Handle, sb, len);
            return sb.ToString();
        }
        public DateTime dtStartDateTime = System.DateTime.Now;

        private const int SW_SHOWMINNOACTIVE = 6;
        private const uint SWP_NOACTIVATE = 0X10;




        private const Int32 WM_GETTEXT = 0XD;
        private const Int32 WM_GETTEXTLENGTH = 0XE;


        #region DllImports

        /*- Retrieves Id of the thread that created the specified window -*/
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(int hWnd, out uint lpdwProcessId);

        /*- Retrieves information about active window or any specific GUI thread -*/
        [DllImport("user32.dll", EntryPoint = "GetGUIThreadInfo")]
        public static extern bool GetGUIThreadInfo(uint tId, out GUITHREADINFO threadInfo);


        /*- Converts window specific point to screen specific -*/
        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, out System.Drawing.Point position);

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

        #endregion
        System.Drawing.Point caretPosition;
        [StructLayout(LayoutKind.Sequential)]    // Required by user32.dll
        public struct RECT {
            public uint Left;
            public uint Top;
            public uint Right;
            public uint Bottom;
        };
        [StructLayout(LayoutKind.Sequential)]    // Required by user32.dll
        public struct GUITHREADINFO {
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

        [System.Runtime.InteropServices.DllImport("user32", EntryPoint = "GetAsyncKeyState", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern int GetAsyncKeyState(uint vkey);

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e) {
            uint VK_PAUSE = 0x13;
            int intPauseKeyState = GetAsyncKeyState(VK_PAUSE);
            int intNumberOfKilledProcesses = 0;
            if (intPauseKeyState != 0) {
                string FileDes = "IdealAutomateScript"; //FileDescription
                var myCurrentProcess = Process.GetCurrentProcess();
                foreach (Process x in Process.GetProcesses()) {
                    try {
                        if (FileDes == x.MainModule.FileVersionInfo.Comments && x.ProcessName != myCurrentProcess.ProcessName) {
                            //  System.Diagnostics.Debugger.Break();
                            x.Kill();
                            intNumberOfKilledProcesses++;
                        }
                    } catch (Exception ex) {
                        continue;
                    }

                }
                if (intNumberOfKilledProcesses > 0) {
                    try {
                        Thread thread = new Thread(new ThreadStart(() => {
                            var window = new Window() //make sure the window is invisible
                            {
                                Width = 300,
                                Height = 300,
                                Title = "Application Cancelled",
                                Content = "Application Cancelled",
                                WindowStyle = WindowStyle.ToolWindow,
                                Visibility = System.Windows.Visibility.Visible,
                                Topmost = true,
                                FontSize = 24,
                                ShowInTaskbar = true,
                                ShowActivated = true,
                            };
                            window.Show();
                            Sleep(5000);
                        }));

                        thread.SetApartmentState(ApartmentState.STA);

                        thread.Start();
                        thread.Join();
                    } catch (Exception ex) {
                        // MessageBox.Show(ex.Message);
                    }
                }
                Sleep(500);
                // Environment.Exit(0);
                //Here is the code that runs when the hotkey is pressed'
            }
        }
        private void BreakOnPauseKeyPress() {
            // Create a timer and set a two millisecond interval.
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Interval = 2;

            // Alternate method: create a Timer with an interval argument to the constructor. 
            //aTimer = new System.Timers.Timer(2000); 

            // Create a timer with a two millisecond interval.
            aTimer = new System.Timers.Timer(2);

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;
        }
        /// <summary>
        /// <para>PutAll receives an ImageEntity object and returns</para>
        /// <para>an integer array of all of x,y coordinates of where the</para>
        /// <para>image was found</para>
        /// <para>Images are used to locate a specific area of the screen. The Windows Snipping Tool is very useful for saving image files to your images folder. If you are not familiar with the Windows Snipping Tool, you should google it to learn about it as it is very helpful. There is a trick to taking snapshots of popup windows. The trick involves clicking on the new option in the snipping tool to cause it to make the entire screen go out of focus. Then, you hit the escape key to remove opacity cloud that prevent you from accessing your screen. At this point, you can activate your popup or dropdown window. Next, you press the control key plus the print key to cause the opacity cloud to appear without removing the popup or dropdown window. You can now drag the Windows Snipping Tool around the window to get a snapshot of it without it disappearing. The Image Tabs has these columns:</para>
        /// <para>1. ImageFile</para>
        /// <para>The imageFile is the file path where the image resides.</para>
        /// <para>2. Attempts</para>
        /// <para>If Attempts is specified, it must be defined as an integer (int). The default value is 1. Sometimes an image will not be found on the first attempt because your computer may be running slowly, and the window may not be fully loaded when the script is looking for it. By specifying an Attempts value of 10, for example, the script will try to find the image up to 10 times before moving on to the next action. In this example, if the image was found on the third attempt, it would not continue looking for the other 7 times since the image was already found.</para>
        /// <para>3. Occurrence</para>
        /// <para>If Occurrence is specified, it must be defined  as an integer (int). If you are only interested in locating a specific occurrence of an image, for example - the second occurrence, you can specify that number as the value of the primitive that is specified in this column.</para>
        /// <para>4. Sleep</para>
        /// <para>If Sleep is specified, it must be defined as an integer (int). If you are only interested in locating a specific occurrence of an image, for example - the second occurrence, you can specify that number as the value of the primitive that is specified in this column.</para>
        /// <para>5. RelativeX</para>
        /// <para>If RelativeX is specified, it must be defined  as an integer (int). The default value for RelativeX is 0. When an image is found, the position of the upper-left corner is returned. You can specify a RelativeX value of pixels to be added to the original X value returned to realign the pixel that you want to click on to the left or to the right.</para>
        /// <para>6. RelativeY</para>
        /// <para>If RelativeY is specified, it must be defined as an integer (int). The default value for RelativeY is 0. When an image is found, the position of the upper-left corner is returned. You can specify a RelativeY value of pixels to be added to the original Y value returned to realign the pixel that you want to click on to be higher or lower.</para>
        /// <para>7. UseGrayScale</para>
        /// <para>If UseGrayScale is true, it attempts to ignore color when looking for the image. GreyScale is created by add the RGB values for a pixel together and dividing that sum by three so color is still a factor, but it is just not as sensitive.</para>
        /// <para>8. Tolerance</para>
        /// <para>The default value for Tolerance is 90. Tolerance specifies the percent of pixels that must match in order for an image to be considered found. When searching for images, the application starts by comparing the least frequent occurrence of a pattern of 10 pixels to every location on the screen in order speed up the process. If there is no match on the least frequent pattern of 10 pixels in the smaller image, it will try to match the second to least most popular pattern of 10 pixels. If there is no match on that second-least frequently occurring pattern, the image will not be found even though there may be more than a 90 percent match between the smaller image and an area on the screen. Sometimes, it helps to try to cut and paste a different image if you are having trouble finding a particular image.</para>
        /// </summary>
        /// <param name="myImage">ImageEntity object</param>
        /// <returns>an integer array of all of x,y coordinates of where the image was found</returns>
        /// Category::Image
        public int[,] PutAll(ImageEntity myImage) {

            PutAllBegin:
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "PutAll:");
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "PutAll:");
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(myImage)) {
                    string name = descriptor.Name;
                    object value = descriptor.GetValue(myImage);
                    Console.WriteLine("{0}={1}", name, value);
                    Logging.WriteLogSimple(String.Format("{0}={1}", name, value));
                }
            }
            // move cursor off screen so it will not interfer with finding image
            int[,] intArray = new int[,] { { 0, 2000 } };

            PositionCursor(intArray);
            string directory = AppDomain.CurrentDomain.BaseDirectory;

            // if alt image exists, use it
            int intLastSlashIndex = myImage.ImageFile.LastIndexOf("\\");
            string strAltFileName = myImage.ImageFile.Substring(intLastSlashIndex + 1);
            string settingsDirectory = GetAppDirectoryForScript();
            string strFullFileName = Path.Combine(settingsDirectory, strAltFileName);
            if (File.Exists(strFullFileName)) {
                myImage.ImageFile = strFullFileName;
            }



            // If ParentImage != null, we need to get the parent image and 
            // do everything that we normally to for an image 
            // to the parent image. If the parent image is found,
            // we need to continue on to get the child image.
            // If the child image is found, we have to adjust the
            // coordinates by adding the coordinates for the parent
            // to the child.

            bool boolImageFound = false;
            int intAttempts = 0;
            List<SubPositionInfo> ls = new List<SubPositionInfo>();
            boolUseGrayScaleDB = myImage.UseGrayScale;
            while (boolImageFound == false && intAttempts < myImage.Attempts) {
                try {
                    ls = Click_PNG(myImage, boolUseGrayScaleDB);
                } catch (Exception ex) {
                    MessageBox.Show("Here is exception thrown in PutAll method for file " + myImage.ImageFile + ": " + ex.Message + Environment.NewLine + Environment.NewLine + "PutAll image file is probably missing from project. Make sure properties for the image file are: Build Action=> Content;  Copy to Output Directory=> Copy if Newer;");
                }

                if (ls.Count > 0) {
                    boolImageFound = true;
                }
                intAttempts += 1;
                // boolUseGrayScaleDB = false; //!boolUseGrayScaleDB;
            }
            // my goal is to have them paste image in popup dialog
            // this image will be saved to a table with fullpathname as key
            // the putall method will start at top again to see if we can use the alternate image from the file
            // At the beginning of putall, we will check to see if there is an alternate image to use from the table
            //if (!boolImageFound) {


            //  List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            //  ControlEntity myControlEntity = new ControlEntity();
            //  myControlEntity.ControlEntitySetDefaults();
            //  myControlEntity.ControlType = ControlType.Heading;
            //  myControlEntity.Text = "Image Not Found";
            //  myListControlEntity.Add(myControlEntity.CreateControlEntity());


            //  myControlEntity.ControlEntitySetDefaults();
            //  myControlEntity.ControlType = ControlType.Label;
            //  myControlEntity.ID = "myLabel";
            //  myControlEntity.Text = "Image not found for " + myImage.ImageFile;
            //  myControlEntity.RowNumber = 0;
            //  myControlEntity.ColumnNumber = 0;
            //  myListControlEntity.Add(myControlEntity.CreateControlEntity());

            //  myControlEntity.ControlEntitySetDefaults();
            //  myControlEntity.ControlType = ControlType.Label;
            //  myControlEntity.ID = "myLabel";
            //  myControlEntity.Text = "The application is " + directory;
            //  myControlEntity.RowNumber = 1;
            //  myControlEntity.ColumnNumber = 0;
            //  myListControlEntity.Add(myControlEntity.CreateControlEntity());


            //  myControlEntity.ControlEntitySetDefaults();
            //  myControlEntity.ControlType = ControlType.Label;
            //  myControlEntity.ID = "myLabel";
            //  myControlEntity.Text = "Here is what that image looks like:";
            //  myControlEntity.RowNumber = 3;
            //  myControlEntity.ColumnNumber = 0;
            //  myListControlEntity.Add(myControlEntity.CreateControlEntity());

            //  myControlEntity.ControlEntitySetDefaults();
            //  if (myImage.ImageFile.Contains(":")) {
            //    byte[] mybytearray = File.ReadAllBytes(myImage.ImageFile);
            //    System.Drawing.Bitmap bm = BytesToBitmap(mybytearray);
            //    myControlEntity.Width = bm.Width;
            //    myControlEntity.Height = bm.Height;
            //    myControlEntity.Source = BitmapSourceFromImage(bm);
            //  } else {
            //    byte[] mybytearray = File.ReadAllBytes(directory.Replace("bin\\Debug", "") + myImage.ImageFile);
            //    System.Drawing.Bitmap bm = BytesToBitmap(mybytearray);
            //    myControlEntity.Width = bm.Width;
            //    myControlEntity.Height = bm.Height;
            //    myControlEntity.Source = BitmapSourceFromImage(bm);
            //  }

            //  myControlEntity.ControlType = ControlType.Image;
            //  myControlEntity.ID = "myImage";
            //  myControlEntity.RowNumber = 4;
            //  myControlEntity.ColumnNumber = 0;

            //  myListControlEntity.Add(myControlEntity.CreateControlEntity());

            //  myControlEntity.ControlEntitySetDefaults();
            //  myControlEntity.ControlType = ControlType.Label;
            //  myControlEntity.ID = "myLabel";
            //  myControlEntity.Text = "Do you want to override the image with a new one?" + System.Environment.NewLine + "Click Okay to add alt image or Cancel to continue without adding alt image";
            //  myControlEntity.RowNumber = 5;
            //  myControlEntity.ColumnNumber = 0;
            //  myListControlEntity.Add(myControlEntity.CreateControlEntity());

            //  myControlEntity.ControlEntitySetDefaults();
            //  myControlEntity.ControlType = ControlType.Label;
            //  myControlEntity.ID = "myLabel";
            //  myControlEntity.Text = "If you click okay, use the cross hairs to get the image and put it into clipboard;" + System.Environment.NewLine + "You can exit the cross hairs screen by hitting escape if you decide not to add alt image";
            //  myControlEntity.RowNumber = 6;
            //  myControlEntity.ColumnNumber = 0;
            //  myListControlEntity.Add(myControlEntity.CreateControlEntity());





            //  string strButtonPressed = WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
            //  if (strButtonPressed != "btnCancel") {
            //    SnippingTool.Snip();
            //    if (SnippingTool.Image != null) {
            //      Clipboard.SetImage(BitmapSourceFromImage(SnippingTool.Image));
            //      myListControlEntity = new List<ControlEntity>();

            //      myControlEntity = new ControlEntity();
            //      myControlEntity.ControlEntitySetDefaults();
            //      myControlEntity.ControlType = ControlType.Heading;
            //      myControlEntity.Text = "Create Alt Image";
            //      myListControlEntity.Add(myControlEntity.CreateControlEntity());




            //      myControlEntity.ControlEntitySetDefaults();
            //      myControlEntity.ControlType = ControlType.Image;
            //      myControlEntity.ID = "myImage";
            //      myControlEntity.RowNumber = 1;
            //      myControlEntity.ColumnNumber = 0;
            //      myControlEntity.ColumnSpan = 2;
            //      myImage.ImageFile = strFullFileName;
            //      SaveClipboardImageToFile(myImage.ImageFile);
            //      byte[] mybytearray = File.ReadAllBytes(myImage.ImageFile);
            //      System.Drawing.Bitmap bm = BytesToBitmap(mybytearray);
            //      myControlEntity.Width = bm.Width;
            //      myControlEntity.Height = bm.Height;


            //      myControlEntity.Source = BitmapSourceFromImage(bm);
            //      myListControlEntity.Add(myControlEntity.CreateControlEntity());

            //      WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);

            //      goto PutAllBegin;
            //    }
            //  }
            //}
            int intRowIndex = 0;
            int[,] myArray = new int[0, 0];
            List<SubPositionInfo> SortedList = ls.OrderByDescending(o => o.percentcorrect).ToList();
            foreach (var myRow in SortedList) {
                int[] NewSizes = new int[] { intRowIndex + 1, 2 };
                if (myRow.percentcorrect < myImage.Tolerance) {
                    break;
                }
                myArray = (int[,])myArray.ResizeArray(NewSizes);
                myArray[intRowIndex, 0] = myRow.myPoint.X;
                myArray[intRowIndex, 1] = myRow.myPoint.Y;
                //myListObject[2] = myRow.percentcorrect;
                //   myListListObject.Add(myListObject);
                intRowIndex++;

            }
            return myArray;
        }

        /// <summary>

        /// <para>PutAll receives an ImageEntity object and returns</para>

        /// <para>an integer array of all of x,y coordinates of where the</para>

        /// <para>image was found</para>

        /// <para>Images are used to locate a specific area of the screen. The Windows Snipping Tool is very useful for saving image files to your images folder. If you are not familiar with the Windows Snipping Tool, you should google it to learn about it as it is very helpful. There is a trick to taking snapshots of popup windows. The trick involves clicking on the new option in the snipping tool to cause it to make the entire screen go out of focus. Then, you hit the escape key to remove opacity cloud that prevent you from accessing your screen. At this point, you can activate your popup or dropdown window. Next, you press the control key plus the print key to cause the opacity cloud to appear without removing the popup or dropdown window. You can now drag the Windows Snipping Tool around the window to get a snapshot of it without it disappearing. The Image Tabs has these columns:</para>

        /// <para>1. ImageFile</para>

        /// <para>The imageFile is the file path where the image resides.</para>

        /// <para>2. Attempts</para>

        /// <para>If Attempts is specified, it must be defined as an integer (int). The default value is 1. Sometimes an image will not be found on the first attempt because your computer may be running slowly, and the window may not be fully loaded when the script is looking for it. By specifying an Attempts value of 10, for example, the script will try to find the image up to 10 times before moving on to the next action. In this example, if the image was found on the third attempt, it would not continue looking for the other 7 times since the image was already found.</para>

        /// <para>3. Occurrence</para>

        /// <para>If Occurrence is specified, it must be defined  as an integer (int). If you are only interested in locating a specific occurrence of an image, for example - the second occurrence, you can specify that number as the value of the primitive that is specified in this column.</para>

        /// <para>4. Sleep</para>

        /// <para>If Sleep is specified, it must be defined as an integer (int). If you are only interested in locating a specific occurrence of an image, for example - the second occurrence, you can specify that number as the value of the primitive that is specified in this column.</para>

        /// <para>5. RelativeX</para>

        /// <para>If RelativeX is specified, it must be defined  as an integer (int). The default value for RelativeX is 0. When an image is found, the position of the upper-left corner is returned. You can specify a RelativeX value of pixels to be added to the original X value returned to realign the pixel that you want to click on to the left or to the right.</para>

        /// <para>6. RelativeY</para>

        /// <para>If RelativeY is specified, it must be defined as an integer (int). The default value for RelativeY is 0. When an image is found, the position of the upper-left corner is returned. You can specify a RelativeY value of pixels to be added to the original Y value returned to realign the pixel that you want to click on to be higher or lower.</para>

        /// <para>7. UseGrayScale</para>

        /// <para>If UseGrayScale is true, it attempts to ignore color when looking for the image. GreyScale is created by add the RGB values for a pixel together and dividing that sum by three so color is still a factor, but it is just not as sensitive.</para>

        /// <para>8. Tolerance</para>

        /// <para>The default value for Tolerance is 90. Tolerance specifies the percent of pixels that must match in order for an image to be considered found. When searching for images, the application starts by comparing the least frequent occurrence of a pattern of 10 pixels to every location on the screen in order speed up the process. If there is no match on the least frequent pattern of 10 pixels in the smaller image, it will try to match the second to least most popular pattern of 10 pixels. If there is no match on that second-least frequently occurring pattern, the image will not be found even though there may be more than a 90 percent match between the smaller image and an area on the screen. Sometimes, it helps to try to cut and paste a different image if you are having trouble finding a particular image.</para>

        /// </summary>

        /// <param name="myImage">ImageEntity object</param>

        /// <returns>an integer array of all of x,y coordinates of where the image was found</returns>

        /// Category::Image

        public int[,] PutAllDoNotCheckForAlternative(ImageEntity myImage) {



            PutAllBegin:

            if (fbDebugMode) {

                Console.WriteLine(oProcess.ProcessName + "==> " + "PutAll:");

                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "PutAll:");

                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(myImage)) {

                    string name = descriptor.Name;

                    object value = descriptor.GetValue(myImage);

                    Console.WriteLine("{0}={1}", name, value);

                    Logging.WriteLogSimple(String.Format("{0}={1}", name, value));

                }

            }

            // move cursor off screen so it will not interfer with finding image

            int[,] intArray = new int[,] { { 0, 2000 } };



            PositionCursor(intArray);

            string directory = AppDomain.CurrentDomain.BaseDirectory;



            // if alt image exists, use it

            int intLastSlashIndex = myImage.ImageFile.LastIndexOf("\\");

            string strAltFileName = myImage.ImageFile.Substring(intLastSlashIndex + 1);

            string settingsDirectory = GetAppDirectoryForScript();

            string strFullFileName = Path.Combine(settingsDirectory, strAltFileName);

            if (File.Exists(strFullFileName)) {

                myImage.ImageFile = strFullFileName;

            }







            // If ParentImage != null, we need to get the parent image and 

            // do everything that we normally to for an image 

            // to the parent image. If the parent image is found,

            // we need to continue on to get the child image.

            // If the child image is found, we have to adjust the

            // coordinates by adding the coordinates for the parent

            // to the child.



            bool boolImageFound = false;

            int intAttempts = 0;

            List<SubPositionInfo> ls = new List<SubPositionInfo>();

            boolUseGrayScaleDB = myImage.UseGrayScale;

            while (boolImageFound == false && intAttempts < myImage.Attempts) {

                try {

                    ls = Click_PNG(myImage, boolUseGrayScaleDB);

                } catch (Exception ex) {

                    MessageBox.Show("Here is exception thrown in PutAll method for file " + myImage.ImageFile + ": " + ex.Message + Environment.NewLine + Environment.NewLine + "PutAll image file is probably missing from project. Make sure properties for the image file are: Build Action=> Content;  Copy to Output Directory=> Copy if Newer;");

                }



                if (ls.Count > 0) {

                    boolImageFound = true;

                }

                intAttempts += 1;

                // boolUseGrayScaleDB = false; //!boolUseGrayScaleDB;

            }



            int intRowIndex = 0;

            int[,] myArray = new int[0, 0];

            List<SubPositionInfo> SortedList = ls.OrderByDescending(o => o.percentcorrect).ToList();

            foreach (var myRow in SortedList) {

                int[] NewSizes = new int[] { intRowIndex + 1, 2 };

                if (myRow.percentcorrect < myImage.Tolerance) {

                    break;

                }

                myArray = (int[,])myArray.ResizeArray(NewSizes);

                myArray[intRowIndex, 0] = myRow.myPoint.X;

                myArray[intRowIndex, 1] = myRow.myPoint.Y;

                //myListObject[2] = myRow.percentcorrect;

                //   myListListObject.Add(myListObject);

                intRowIndex++;



            }

            return myArray;

        }

        private static BitmapSource BitmapSourceFromImage(System.Drawing.Image img) {
            MemoryStream memStream = new MemoryStream();

            // save the image to memStream as a png
            img.Save(memStream, System.Drawing.Imaging.ImageFormat.Png);

            // gets a decoder from this stream
            System.Windows.Media.Imaging.PngBitmapDecoder decoder = new System.Windows.Media.Imaging.PngBitmapDecoder(memStream, System.Windows.Media.Imaging.BitmapCreateOptions.PreservePixelFormat, System.Windows.Media.Imaging.BitmapCacheOption.Default);

            return decoder.Frames[0];
        }
        private static System.Drawing.Bitmap BytesToBitmap(byte[] byteArray) {


            using (MemoryStream ms = new MemoryStream(byteArray)) {


                System.Drawing.Bitmap img = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(ms);


                return img;


            }
        }
        private static void SaveClipboardImageToFile(string filePath) {
            var image = Clipboard.GetImage();
            if (image == null) {
                MessageBoxResult result = MessageBox.Show("Clipboard Cannot Be Empty", "PopUp Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) {

                }
            } else {
                using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    encoder.Save(fileStream);
                }
            }
        }
        /// <summary>
        /// <para>PutCursorPosition provides a way to save the cursor position in</para>
        /// <para>an array so you can come back to it later without having to find</para>
        /// <para>the original image again.</para>
        /// </summary>
        /// <returns></returns>
        /// Category::Cursor
        public int[,] PutCursorPosition() {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "PutCursorPosition");
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "PutCursorPosition");
            }
            int[,] myArray = new int[1, 2];
            myArray[0, 0] = System.Windows.Forms.Cursor.Position.X;
            myArray[0, 1] = System.Windows.Forms.Cursor.Position.Y;
            return myArray;
        }
        /// <summary>
        /// <para>PutCaretPositionInArray provides a way to save the caret position</para>
        /// <para>in an array so you can come back to it later without having to find </para>
        /// the original position again.
        /// </summary>
        /// <returns></returns>
        /// Category::Mouse
        public int[,] PutCaretPositionInArray() {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "PutCaretPositionInArray");
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "PutCaretPositionInArray");
            }
            int[,] myArray = new int[1, 2];
            string activeProcess = GetActiveProcess();
            if (activeProcess == string.Empty) {
                Logging.WriteLogSimple("No active window found");
                MessageBox.Show("No active window found");
            }

            // If window explorer is active window (eg. user has opened any drive)
            // Or for any failure when activeProcess is nothing               

            // Otherwise Calculate Caret position
            EvaluateCaretPosition();

            myArray[0, 0] = caretPosition.X;
            myArray[0, 1] = caretPosition.Y;
            return myArray;


        }
        /// <summary>
        /// <para>ClickImageIfExists is similar to PutAll, but it left clicks on the </para>
        /// <para>best match for the image and does not return an array containing the position. </para>
        /// <para>Images are used to locate a specific area of the screen. The Windows Snipping Tool is very useful for saving image files to your images folder. If you are not familiar with the Windows Snipping Tool, you should google it to learn about it as it is very helpful. There is a trick to taking snapshots of popup windows. The trick involves clicking on the new option in the snipping tool to cause it to make the entire screen go out of focus. Then, you hit the escape key to remove opacity cloud that prevent you from accessing your screen. At this point, you can activate your popup or dropdown window. Next, you press the control key plus the print key to cause the opacity cloud to appear without removing the popup or dropdown window. You can now drag the Windows Snipping Tool around the window to get a snapshot of it without it disappearing. The Image Tabs has these columns:</para>
        /// <para>1. ImageFile</para>
        /// <para>The imageFile is the file path where the image resides.</para>
        /// <para>2. Attempts</para>
        /// <para>If Attempts is specified, it must be defined as an integer (int). The default value is 1. Sometimes an image will not be found on the first attempt because your computer may be running slowly, and the window may not be fully loaded when the script is looking for it. By specifying an Attempts value of 10, for example, the script will try to find the image up to 10 times before moving on to the next action. In this example, if the image was found on the third attempt, it would not continue looking for the other 7 times since the image was already found.</para>
        /// <para>3. Occurrence</para>
        /// <para>If Occurrence is specified, it must be defined  as an integer (int). If you are only interested in locating a specific occurrence of an image, for example - the second occurrence, you can specify that number as the value of the primitive that is specified in this column.</para>
        /// <para>4. Sleep</para>
        /// <para>If Sleep is specified, it must be defined as an integer (int). If you are only interested in locating a specific occurrence of an image, for example - the second occurrence, you can specify that number as the value of the primitive that is specified in this column.</para>
        /// <para>5. RelativeX</para>
        /// <para>If RelativeX is specified, it must be defined  as an integer (int). The default value for RelativeX is 0. When an image is found, the position of the upper-left corner is returned. You can specify a RelativeX value of pixels to be added to the original X value returned to realign the pixel that you want to click on to the left or to the right.</para>
        /// <para>6. RelativeY</para>
        /// <para>If RelativeY is specified, it must be defined as an integer (int). The default value for RelativeY is 0. When an image is found, the position of the upper-left corner is returned. You can specify a RelativeY value of pixels to be added to the original Y value returned to realign the pixel that you want to click on to be higher or lower.</para>
        /// <para>7. UseGrayScale</para>
        /// <para>If UseGrayScale is true, it attempts to ignore color when looking for the image. GreyScale is created by add the RGB values for a pixel together and dividing that sum by three so color is still a factor, but it is just not as sensitive.</para>
        /// <para>8. Tolerance</para>
        /// <para>The default value for Tolerance is 90. Tolerance specifies the percent of pixels that must match in order for an image to be considered found. When searching for images, the application starts by comparing the least frequent occurrence of a pattern of 10 pixels to every location on the screen in order speed up the process. If there is no match on the least frequent pattern of 10 pixels in the smaller image, it will try to match the second to least most popular pattern of 10 pixels. If there is no match on that second-least frequently occurring pattern, the image will not be found even though there may be more than a 90 percent match between the smaller image and an area on the screen. Sometimes, it helps to try to cut and paste a different image if you are having trouble finding a particular image.</para>

        /// </summary>
        /// <param name="myImage"></param>
        /// Category::Image
        public void ClickImageIfExists(ImageEntity myImage) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "ClickImageIfExists:");
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "ClickImageIfExists:");
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(myImage)) {
                    string name = descriptor.Name;
                    object value = descriptor.GetValue(myImage);
                    Console.WriteLine("{0}={1}", name, value);
                    Logging.WriteLogSimple(String.Format("{0}={1}", name, value));
                }
            }
            int[,] intArray = new int[,] { { 2000, 2000 } };

            PositionCursor(intArray);

            // If ParentImage != null, we need to get the parent image and 
            // do everything that we normally to for an image 
            // to the parent image. If the parent image is found,
            // we need to continue on to get the child image.
            // If the child image is found, we have to adjust the
            // coordinates by adding the coordinates for the parent
            // to the child.

            bool boolImageFound = false;
            int intAttempts = 0;
            List<SubPositionInfo> ls = new List<SubPositionInfo>();
            while (boolImageFound == false && intAttempts < myImage.Attempts) {
                try {
                    ls = Click_PNG(myImage, boolUseGrayScaleDB);
                } catch (Exception ex) {
                    MessageBox.Show("Here is an exception thrown in the PutAll method in IdealAutomateCore for file " + myImage.ImageFile + ": " + ex.Message + Environment.NewLine + Environment.NewLine + "PutAll image file is probably missing from project. Make sure properties for the image file are: Build Action=> Content;  Copy to Output Directory=> Copy if Newer;");
                }
                if (ls.Count > 0) {
                    List<SubPositionInfo> SortedList = ls.OrderByDescending(o => o.percentcorrect).ToList();
                    boolImageFound = true;
                    System.Drawing.Point p = SortedList[0].myPoint;
                    Position_Cursor.MoveMouse(p.X, p.Y);
                    UInt32 myX = Convert.ToUInt32(p.X);
                    UInt32 myY = Convert.ToUInt32(p.Y);
                    Position_Cursor.DoMouseClick(myX, myY);
                    break;
                }
                intAttempts += 1;
                //  boolUseGrayScaleDB = false; // !boolUseGrayScaleDB;
            }
        }
        /// <summary>
        /// LeftClick allows you to left-click on the X and Y coordinates of an int array passed into the method.
        /// </summary>
        /// <param name="myArray">int array containing X and Y coordinates of position on screen to click</param>
        /// Category::Mouse
        public void LeftClick(int[,] myArray) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "LeftClick:");
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "LeftClick:");
                int bound0 = myArray.GetUpperBound(0);
                int bound1 = myArray.GetUpperBound(1);
                // ... Loop over bounds.
                for (int i = 0; i <= bound0; i++) {
                    for (int x = 0; x <= bound1; x++) {
                        // Display the element at these indexes.
                        Console.WriteLine(myArray[i, x].ToString());
                        Logging.WriteLogSimple(myArray[i, x].ToString());
                    }
                    Console.WriteLine();
                    Logging.WriteLogSimple("");
                }
            }
            int RelX = myArray[0, 0];
            int RelY = myArray[0, 1];
            Position_Cursor.MoveMouse(RelX, RelY);
            UInt32 myX1 = Convert.ToUInt32(RelX);
            UInt32 myY1 = Convert.ToUInt32(RelY);
            Position_Cursor.DoMouseClick(myX1, myY1);
        }
        /// <summary>
        /// ShiftClick allows you to shift-click on the X and Y coordinates of an int array passed into the method.
        /// </summary>
        /// <param name="myArray">int array containing X and Y coordinates of position on screen to click</param>
        /// Category::Mouse
        public void ShiftClick(int[,] myArray) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "ShiftClick:");
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "ShiftClick:");
                int bound0 = myArray.GetUpperBound(0);
                int bound1 = myArray.GetUpperBound(1);
                // ... Loop over bounds.
                for (int i = 0; i <= bound0; i++) {
                    for (int x = 0; x <= bound1; x++) {
                        // Display the element at these indexes.
                        Console.WriteLine(myArray[i, x].ToString());
                        Logging.WriteLogSimple(myArray[i, x].ToString());
                    }
                    Console.WriteLine();
                    Logging.WriteLogSimple(" ");
                }
            }
            int RelX = myArray[0, 0];
            int RelY = myArray[0, 1];
            Position_Cursor.MoveMouse(RelX, RelY);
            UInt32 myX1 = Convert.ToUInt32(RelX);
            UInt32 myY1 = Convert.ToUInt32(RelY);
            Position_Cursor.DoMouseShiftClick(myX1, myY1);
        }
        /// <summary>
        /// RightClick allows you to right-click on the X and Y coordinates of an int array passed into the method.
        /// </summary>
        /// <param name="myArray">int array containing X and Y coordinates of position on screen to click</param>
        /// Category::Mouse
        public void RightClick(int[,] myArray) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "RightClick:");
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "RightClick:");
                int bound0 = myArray.GetUpperBound(0);
                int bound1 = myArray.GetUpperBound(1);
                // ... Loop over bounds.
                for (int i = 0; i <= bound0; i++) {
                    for (int x = 0; x <= bound1; x++) {
                        // Display the element at these indexes.
                        Console.WriteLine(myArray[i, x].ToString());
                        Logging.WriteLogSimple(myArray[i, x].ToString());
                    }
                    Console.WriteLine();
                    Logging.WriteLogSimple(" ");
                }
            }
            int RelX = myArray[0, 0];
            int RelY = myArray[0, 1];
            Position_Cursor.MoveMouse(RelX, RelY);
            UInt32 myX1 = Convert.ToUInt32(RelX);
            UInt32 myY1 = Convert.ToUInt32(RelY);
            Position_Cursor.DoMouseRightClick(myX1, myY1);
        }
        /// <summary>
        /// PositionCursor allows you to move the cursor to the X and Y coordinates of an int array passed into the method.
        /// </summary>
        /// <param name="myArray">int array containing X and Y coordinates of position on screen to click</param>
        /// Category::Cursor
        public void PositionCursor(int[,] myArray) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "PositionCursor:");
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "PositionCursor:");
                int bound0 = myArray.GetUpperBound(0);
                int bound1 = myArray.GetUpperBound(1);
                // ... Loop over bounds.
                for (int i = 0; i <= bound0; i++) {
                    for (int x = 0; x <= bound1; x++) {
                        // Display the element at these indexes.
                        Console.WriteLine(myArray[i, x].ToString());
                        Logging.WriteLogSimple(myArray[i, x].ToString());
                    }
                    Console.WriteLine();
                    Logging.WriteLogSimple(" ");
                }
            }
            Position_Cursor.MoveMouse(myArray[0, 0], myArray[0, 1]);
        }
        /// <summary>
        /// PutClipboardInEntity returns a string that contains the text in the clipboard.
        /// </summary>
        /// <returns>string that contains the text in the clipboard</returns>
        /// Category::Clipboard
        public string PutClipboardInEntity() {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "PutClipboardInEntity: ");
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "PutClipboardInEntity: ");
            }
            int intTryAgainCtr = 0;
            TryAgain:
            string myEntity = "";
            try {
                //Thread thread = new Thread(new ThreadStart(() => {
                try {

                    for (int i = 0; i < 45; i++) {
                        if (Clipboard.GetData(DataFormats.Text) == null) {
                            Sleep(100);
                        } else {
                            break;
                        }
                        if (i == 10 || i == 20 || i == 30 || i == 40) {
                            TypeText("^(c)", 500);
                            Sleep(1001);
                            // System.Diagnostics.Debugger.Break();
                        }
                    }

                    myEntity = Clipboard.GetData(DataFormats.Text).ToString();
                    if (myEntity.Contains("Failed to copy selection to the clipboard")) {
                        TypeText("^(c)", 500);
                        Sleep(1002);
                        intTryAgainCtr++;
                        if (intTryAgainCtr < 10) {
                            goto TryAgain;
                        }

                    }
                    // myEntity = Clipboard.GetText(System.Windows.TextDataFormat.Html);
                } catch (Exception e) {
                    Console.WriteLine("Exception occurred in PutClipboardInEntity!!!! " + e.Message);
                    Logging.WriteLogSimple("Exception occurred in PutClipboardInEntity!!!! " + e.Message);
                    myEntity = "";
                }

                // or call logic here



                //}));

                //thread.SetApartmentState(ApartmentState.STA);

                //thread.Start();
                //thread.Join();
            } catch (Exception ex) {
                try {
                    TypeText("^(c)", 500);
                    //Thread thread = new Thread(new ThreadStart(() => {
                    try {

                        for (int i = 0; i < 45; i++) {
                            if (Clipboard.GetData(DataFormats.Text) == null) {
                                Sleep(100);
                            } else {
                                break;
                            }
                            if (i == 10 || i == 20 || i == 30 || i == 40) {
                                TypeText("^(c)", 500);
                                Sleep(1003);
                            }
                        }

                        myEntity = Clipboard.GetData(DataFormats.Text).ToString();
                        // myEntity = Clipboard.GetText(System.Windows.TextDataFormat.Html);
                    } catch (Exception e) {
                        Console.WriteLine("Exception occurred in PutClipboardInEntity!!!! " + e.Message);
                        Logging.WriteLogSimple("Exception occurred in PutClipboardInEntity!!!! " + e.Message);
                        myEntity = "";
                    }

                    // or call logic here



                    //}));

                    //thread.SetApartmentState(ApartmentState.STA);

                    //thread.Start();
                    //thread.Join();
                } catch (Exception ex1) {
                    MessageBox.Show(ex1.Message + "Line 991 in Methods");
                }
            }

            Console.Write("myEntity=" + myEntity);
            Logging.WriteLogSimple("myEntity=" + myEntity);

            return myEntity;
        }
        /// <summary>
        /// PutWindowTitleInEntity returns a string that contains the title of the Active Window
        /// </summary>
        /// <returns>string that contains the title of the Active Window</returns>
        /// Category::Window
        public string PutWindowTitleInEntity() {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "PutWindowTitleInEntity");
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "PutWindowTitleInEntity");
            }
            string myEntity = "";
            try {
                myEntity = GetActiveProcessTitle();
                // or call logic here
            } catch (Exception ex) {

                MessageBox.Show("Here is an exception thrown in PutWindowTitleInEntity method in IdealAutomateCore: " + ex.Message);
            }
            return myEntity;
        }
        /// <summary>
        /// PutInternetExplorerTabTitleInEntity returns a string that contains the Title for the active tab in internet explorer
        /// </summary>
        /// <returns>string that contains the Title for the active tab in internet explorer</returns>
        /// Category::Internet Explorer
        public string PutInternetExplorerTabTitleInEntity() {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "PutInternetExplorerTabTitleInEntity");
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "PutInternetExplorerTabTitleInEntity");
            }

            TypeText("{UP}", 1000);
            TypeText("%(v)", 1000);
            TypeText("c", 1000);

            string strPageTitle = PutWindowTitleInEntity();
            strPageTitle = strPageTitle.Replace(" - Original Source", "");
            CloseApplicationAltFc(500);
            return strPageTitle;

        }
        /// <summary>
        /// PutInternetExplorerTabTitleInEntity returns a string that contains the url in the address bar for the active tab in internet explorer
        /// </summary>
        /// <returns>string that contains the url for the address bar for the active tab in internet explorer</returns>
        /// Category::Internet Explorer
        public string PutInternetExplorerTabURLContainingStringInEntity(string myEntity) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "PutInternetExplorerTabURLContainingStringInEntity: myEntity=" + myEntity);
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "PutInternetExplorerTabURLContainingStringInEntity: myEntity=" + myEntity);
            }

            List<string> myWindowTitles = GetWindowTitlesByProcessName("iexplore");
            myWindowTitles.RemoveAll(item => item == "");

            string strCurrentTabURL = "";
            bool boolTargetURLFound = false;
            bool boolInternetExplorerFound = false;
            foreach (var myWindowTitle in myWindowTitles) {
                boolInternetExplorerFound = true;
                if (myWindowTitle != "") {
                    ActivateWindowByTitle(myWindowTitle);
                    // go to last tab;
                    // myActions.TypeText("^(9)",2500);
                    Sleep(500);
                    TypeText("%(d)", 500); // select address bar
                    string strFirstTabURL = SelectAllCopyIntoEntity(500);
                    TypeText("{ESC}", 500);
                    if (strFirstTabURL.ToLower().Contains(myEntity)) {
                        strCurrentTabURL = strFirstTabURL;
                        boolTargetURLFound = true;
                        break;
                    }

                    // go to next tab
                    while (strCurrentTabURL != strFirstTabURL) {
                        TypeText("^({TAB})", 2500);
                        TypeText("%(d)", 500); // select address bar
                        strCurrentTabURL = SelectAllCopyIntoEntity(500);
                        TypeText("{ESC}", 500);
                        if (strCurrentTabURL.ToLower().Contains(myEntity)) {
                            boolTargetURLFound = true;
                            break;
                        }
                    }
                    if (boolTargetURLFound) {

                        break;
                    }

                }
            }
            if (boolInternetExplorerFound == false) {
                strCurrentTabURL = "IE not found";
                return strCurrentTabURL;
            }
            if (boolTargetURLFound == false) {
                return "";
            }
            return strCurrentTabURL;

        }
        /// <summary>
        /// PutEntityInClipboard takes the input parameter string and puts it in the clipboard
        /// </summary>
        /// <param name="myEntity">string that you want to put into the clipboard</param>
        /// Category::Clipboard
        public void PutEntityInClipboard(string myEntity) {
            if (fbDebugMode) {

                Console.WriteLine("PutEntityInClipboard: myEntity=" + myEntity);
                Logging.WriteLogSimple("PutEntityInClipboard: myEntity=" + myEntity);

            }
            try {

                //  Thread thread = new Thread(new ThreadStart(() => {
                int intTries = 0;
                TryAgain:

                bool boolClipboardWorks = ClipboardNative.CopyTextToClipboard(myEntity);
                //  Clipboard.Clear();

                if (boolClipboardWorks == false && intTries < 20) {
                    intTries++;
                    goto TryAgain;
                }
                //// Clipboard.SetText(myEntity);
                //Clipboard.SetDataObject((Object)myEntity, true);
                // or call logic here



                //}));

                //thread.SetApartmentState(ApartmentState.STA);

                //thread.Start();
                //thread.Join();
            } catch (Exception ex) {
                MessageBox.Show("Here is an exception thrown in PutEntityInClipboard method in IdealAutomateCore for myEntity " + myEntity + ": " + ex.Message);
            }
        }
        /// <summary>
        /// <para>TypeText - The visual basic SendKeys function is used to mimic </para>
        /// <para>pressing special keys (like the enter or alt keys). This means you </para>
        /// <para>need to use the Shortcut Keys help file in the IdealAutomate application </para>
        /// <para>or google in order to learn what characters can be used to represent</para>
        /// <para>special keys. For example, the ^ character is used to represent the </para>
        /// <para>control key and here is how you indicate the enter key is pressed: </para>
        /// <para>{ENTER}. You will also need to learn how to "escape" special characters</para>
        /// <para>(like the bracket character). If you are trying to type a lot of special</para>
        /// <para>characters, it may be easier to create a string primitive with the </para>
        /// <para>text you want to type and use the PutEntityInClipboard verb to copy </para>
        /// <para>the string into the clipboard. After the string is in the clipboard,</para>
        /// <para>you can use the TypeText verb with control v to paste what is in the</para>
        /// clipboard to where you want it.
        /// </summary>
        /// <param name="myEntity">string representing the keys you want to press</param>
        /// <param name="intSleep">integer representing the number of milliseconds to wait before sending the text</param>
        /// Category::Keyboard
        public void TypeText(string myEntity, int intSleep) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "TypeText: myEntity=" + myEntity + " intSleep=" + intSleep.ToString());
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "TypeText: myEntity=" + myEntity + " intSleep=" + intSleep.ToString());
            }
            InputSimulator InputSimulator = new InputSimulator();
            if (myEntity == "{LWin}") {
                KeyboardSend.KeyDown(System.Windows.Forms.Keys.ControlKey);
                KeyboardSend.KeyDown(System.Windows.Forms.Keys.Alt);
                KeyboardSend.KeyDown(System.Windows.Forms.Keys.L);
                KeyboardSend.KeyUp(System.Windows.Forms.Keys.L);
                KeyboardSend.KeyUp(System.Windows.Forms.Keys.Alt);
                KeyboardSend.KeyUp(System.Windows.Forms.Keys.ControlKey);

            }
            if (intSleep > 0) {
                System.Threading.Thread.Sleep(intSleep);
            }
            //          public void SimulateSomeModifiedKeystrokes()
            //{
            //  // CTRL-C (effectively a copy command in many situations)
            //  InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);

            //  // You can simulate chords with multiple modifiers
            //  // For example CTRL-K-C whic is simulated as
            //  // CTRL-down, K, C, CTRL-up
            //  InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, new [] {VirtualKeyCode.VK_K, VirtualKeyCode.VK_C});

            //  // You can simulate complex chords with multiple modifiers and key presses
            //  // For example CTRL-ALT-SHIFT-ESC-K which is simulated as
            //  // CTRL-down, ALT-down, SHIFT-down, press ESC, press K, SHIFT-up, ALT-up, CTRL-up
            //  InputSimulator.SimulateModifiedKeyStroke(
            //    new[] { VirtualKeyCode.CONTROL, VirtualKeyCode.MENU, VirtualKeyCode.SHIFT },
            //    new[] { VirtualKeyCode.ESCAPE, VirtualKeyCode.VK_K });
            //}
            if (myEntity == "%(f)e") {
                InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.VK_F); //System.Windows.Forms.Keys.Alt);
                InputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_E);
                return;
            }
            if (myEntity == "%(d)") {
                InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.VK_D); //System.Windows.Forms.Keys.Alt);        
                return;
            }
            if (myEntity == "%(f)") {
                InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.VK_F); //System.Windows.Forms.Keys.Alt);        
                return;
            }
            if (myEntity == "%(\" \")n") {
                InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.SPACE); //System.Windows.Forms.Keys.Alt);
                InputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_N);
                return;
            }
            if (myEntity == "%(\" \")") {
                InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.SPACE); //System.Windows.Forms.Keys.Alt);              
                return;
            }
            if (myEntity == "%(\" \")x") {
                InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.SPACE); //System.Windows.Forms.Keys.Alt);
                InputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_X);
                return;
            }
            if (myEntity == "%({DOWN})") {
                InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.DOWN); //System.Windows.Forms.Keys.Alt);

                return;
            }
            if (myEntity == "%({F8})") {
                InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.F8); //System.Windows.Forms.Keys.Alt);
                return;
            }
            if (myEntity == "{NUMPADADD}") {
                InputSimulator.Keyboard.KeyPress(VirtualKeyCode.ADD);
                return;
            }
            if (myEntity == "{NUMPADMULT}") {
                InputSimulator.Keyboard.KeyPress(VirtualKeyCode.MULTIPLY);
                return;
            }
            if (myEntity == "{ENTER}") {
                InputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                return;
            }
            if (myEntity == "^(n)") {
                InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_N);
                return;
            }
            if (myEntity == "^(v)") {
                InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
                return;
            }
            if (myEntity == "^(c)") {
                try {

                    Thread thread = new Thread(new ThreadStart(() => {
                        Clipboard.Clear();
                        // Clipboard.SetText(myEntity);            
                        // or call logic here



                    }));

                    thread.SetApartmentState(ApartmentState.STA);

                    thread.Start();
                    thread.Join();
                } catch (Exception ex) {
                    MessageBox.Show("Here is an exception thrown in TypeText method in IdealAutomateCore for myEntity " + myEntity + ": " + ex.Message);
                }
                InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
                return;
            }
            if (myEntity == "{F5}") {
                InputSimulator.Keyboard.KeyPress(VirtualKeyCode.F5);
                return;
            }
            if (myEntity == "{F6}") {
                InputSimulator.Keyboard.KeyPress(VirtualKeyCode.F6);
                return;
            }
            if (myEntity == "{DOWN}") {
                InputSimulator.Keyboard.KeyPress(VirtualKeyCode.DOWN);
                return;
            }
            if (myEntity == "{UP}") {
                InputSimulator.Keyboard.KeyPress(VirtualKeyCode.UP);
                return;
            }
            if (myEntity == "^({END})") {
                InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.END);
                return;
            }
            if (myEntity == "^({HOME})") {
                InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.HOME);
                return;
            }
            if (myEntity == "+({F10})") {
                InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.F10);
                return;
            }
            if (myEntity == "{LWin}") {
                KeyboardSend.KeyDown(System.Windows.Forms.Keys.LWin);
                //KeyboardSend.KeyDown(System.Windows.Forms.Keys.Shift);
                //KeyboardSend.KeyDown(System.Windows.Forms.Keys.Right);
                KeyboardSend.KeyUp(System.Windows.Forms.Keys.LWin);
                //KeyboardSend.KeyUp(System.Windows.Forms.Keys.D4);
                //KeyboardSend.KeyUp(System.Windows.Forms.Keys.Right);
            } else {

                try
                {
                    System.Windows.Forms.SendKeys.SendWait(myEntity);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }
        /// <summary>
        /// <para>CloseApplicationAltFx accepts an input integer to indicate how long</para>
        /// <para>to wait before sending text Alt(F)x to close an application</para>
        /// </summary>
        /// <param name="intSleep">integer indicating how many milliseconds to wait before sending the text</param>
        /// Category::Keyboard
        public void CloseApplicationAltFx(int intSleep) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "CloseInternetExplorer: intSleep=" + intSleep.ToString());
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "CloseInternetExplorer: intSleep=" + intSleep.ToString());
            }
            InputSimulator InputSimulator = new InputSimulator();
            if (intSleep > 0) {
                System.Threading.Thread.Sleep(intSleep);
            }
            InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.VK_F); //System.Windows.Forms.Keys.Alt);
            System.Threading.Thread.Sleep(200);
            InputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_X);
        }
        /// <summary>
        /// <para>CloseApplicationAltFc accepts an input integer to indicate how many milliseconds</para>
        /// <para>to wait before sending text Alt(F)c to close an application</para>
        /// </summary>
        /// <param name="intSleep">integer indicating how many milliseconds to wait before sending the text</param>
        /// Category::Keyboard
        public void CloseApplicationAltFc(int intSleep) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "CloseInternetExplorer: intSleep=" + intSleep.ToString());
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "CloseInternetExplorer: intSleep=" + intSleep.ToString());
            }
            InputSimulator InputSimulator = new InputSimulator();
            if (intSleep > 0) {
                System.Threading.Thread.Sleep(intSleep);
            }
            InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.VK_F); //System.Windows.Forms.Keys.Alt);
            System.Threading.Thread.Sleep(200);
            InputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_C);
        }
        /// <summary>
        /// <para>SelectAllCopy accepts an input integer to indicate how many milliseconds</para>
        /// <para>to wait before sending text Ctrl(a) and Ctrl(c). This will put</para>
        /// <para>all of the input into the clipboard</para>
        /// </summary>
        /// <param name="intSleep">integer indicating how many milliseconds to wait before sending the text</param>
        /// Category::Clipboard
        public void SelectAllCopy(int intSleep) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "SelectAllCopy: intSleep=" + intSleep.ToString());
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "SelectAllCopy: intSleep=" + intSleep.ToString());
            }
            InputSimulator InputSimulator = new InputSimulator();
            if (intSleep > 0) {
                System.Threading.Thread.Sleep(intSleep);
            }
            InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A);
            System.Threading.Thread.Sleep(200);
            InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
        }
        /// <summary>
        /// <para>SelectAllCopyIntoEntity accepts an input integer to indicate how many milliseconds</para>
        /// <para>to wait before sending text Ctrl(a) and Ctrl(c). This will put</para>
        /// <para>all of the input into the clipboard and then it will return</para>
        /// <para>what is in the clipboard as a string</para>
        /// </summary>
        /// <param name="intSleep">integer indicating how many milliseconds to wait before sending the text</param>
        /// Category::Clipboard
        public string SelectAllCopyIntoEntity(int intSleep) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "SelectAllCopy: intSleep=" + intSleep.ToString());
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "SelectAllCopy: intSleep=" + intSleep.ToString());
            }
            Clipboard.Clear();
            InputSimulator InputSimulator = new InputSimulator();
            if (intSleep > 0) {
                System.Threading.Thread.Sleep(intSleep);
            }
            InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A);
            System.Threading.Thread.Sleep(500);
            if (intSleep > 0) {
                System.Threading.Thread.Sleep(intSleep);
            }
            InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
            return PutClipboardInEntity();
        }
        /// <summary>
        /// <para>SelectAllPaste accepts an input integer to indicate how many milliseconds</para>
        /// <para>to wait before sending text Ctrl(a) and Ctrl(v). This will select</para>
        /// <para>all of the input on the screen and replace it with what is in the clipboard</para>
        /// </summary>
        /// <param name="intSleep">integer indicating how many milliseconds to wait before sending the text</param>
        /// Category::Clipboard
        public void SelectAllPaste(int intSleep) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "SelectAllPaste: intSleep=" + intSleep.ToString());
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "SelectAllPaste: intSleep=" + intSleep.ToString());
            }
            InputSimulator InputSimulator = new InputSimulator();
            if (intSleep > 0) {
                System.Threading.Thread.Sleep(intSleep);
            }
            InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A);
            System.Threading.Thread.Sleep(200);
            if (intSleep > 0) {
                System.Threading.Thread.Sleep(intSleep);
            }
            InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
        }
        /// <summary>
        /// <para>SelectAllPasteFromEntity accepts a string to paste and an </para>
        /// <para>input integer to indicate how many milliseconds</para>
        /// <para>to wait before sending text Ctrl(a) and Ctrl(v). This will select</para>
        /// <para>all of the input on the screen and replace it with what is in input string</para>
        /// </summary>
        /// <param name="myEntity">string that you want to paste</param>
        /// <param name="intSleep">integer indicating how many milliseconds to wait before sending the text</param>
        /// Category::Clipboard
        public void SelectAllPasteFromEntity(string myEntity, int intSleep) {
            PutEntityInClipboard(myEntity);
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "SelectAllPaste: intSleep=" + intSleep.ToString());
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "SelectAllPaste: intSleep=" + intSleep.ToString());
            }
            InputSimulator InputSimulator = new InputSimulator();
            if (intSleep > 0) {
                System.Threading.Thread.Sleep(intSleep);
            }
            InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A);
            System.Threading.Thread.Sleep(200);
            InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
        }
        /// <summary>
        /// <para>SelectAllDelete accepts an input integer to indicate how many milliseconds</para>
        /// <para>to wait before sending text Ctrl(a) and {DELETE}. This will delete</para>
        /// <para>what is in the input field on the screen</para>    
        /// </summary>
        /// <param name="intSleep">integer indicating how many milliseconds to wait before sending the text</param>
        /// Category::Keyboard
        public void SelectAllDelete(int intSleep) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "SelectAllDelete: intSleep=" + intSleep.ToString());
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "SelectAllDelete: intSleep=" + intSleep.ToString());
            }
            InputSimulator InputSimulator = new InputSimulator();
            if (intSleep > 0) {
                System.Threading.Thread.Sleep(intSleep);
            }
            InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A);
            System.Threading.Thread.Sleep(200);
            InputSimulator.Keyboard.KeyPress(VirtualKeyCode.DELETE);
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
        /// Category::Window
        public string WindowMultipleControls(ref List<ControlEntity> myListControlEntity, int intWindowHeight, int intWindowWidth, int intWindowTop, int intWindowLeft) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "WindowMultipleControls");
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "WindowMultipleControls");
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
        /// Category::Window
        public string WindowMultipleControlsMinimized(ref List<ControlEntity> myListControlEntity, int intWindowHeight, int intWindowWidth, int intWindowTop, int intWindowLeft) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "WindowMultipleControls");
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "WindowMultipleControls");
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
        /// Category::Window
        public void WindowShape(string myShape, string myOrientation, string myTitle, string myContent, int intTop, int intLeft) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "WindowShape: myEntity=" + myTitle);
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "WindowShape: myEntity=" + myTitle);
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
        /// Category::MessageBox
        public void MessageBoxShow(string myEntity) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "MessageBoxShow: myEntity=" + myEntity);
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "MessageBoxShow: myEntity=" + myEntity);
            }
            System.Windows.Forms.MessageBox.Show(myEntity, "Header", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.None,
          System.Windows.Forms.MessageBoxDefaultButton.Button1, (System.Windows.Forms.MessageBoxOptions)0x40000);  // MB_TOPMOST
        }
        /// <summary>
        /// MessageBoxShowWithYesNo receives an input string and displays it in a messagebox with Yes and No Buttons
        /// </summary>
        /// <param name="myEntity">string that you want to display in messagebox</param>

        /// <returns>System.Windows.Forms.DialogResult</returns>
        /// Category::MessageBox
        public System.Windows.Forms.DialogResult MessageBoxShowWithYesNo(string myEntity) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "MessageBoxShow: myEntity=" + myEntity);
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "MessageBoxShow: myEntity=" + myEntity);
            }
            System.Windows.Forms.DialogResult _dialogResult =
            System.Windows.Forms.MessageBox.Show(myEntity, "Header", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.None,
          System.Windows.Forms.MessageBoxDefaultButton.Button1, (System.Windows.Forms.MessageBoxOptions)0x40000);  // MB_TOPMOST
            return _dialogResult;
        }
        /// <summary>
        /// <para>Run receives two input strings. The first is the path to the executable.</para>
        /// <para>The second is optional and it is the content you want to open with the executable.</para>
        /// <para>Run starts the executable as a thread and continues to the next statement</para>
        /// <para>without waiting the the thread to complete.</para>
        /// </summary>
        /// <param name="myEntityForExecutable">string for the path of the executable</param>
        /// <param name="myEntityForContent">string for the content for the executable to open</param>
        /// Category::Run
        public void Run(string myEntityForExecutable, string myEntityForContent) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "Run: myEntityForExecutable=" + myEntityForExecutable + " myEntityForContent=" + myEntityForContent);
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "Run: myEntityForExecutable=" + myEntityForExecutable + " myEntityForContent=" + myEntityForContent);
            }

            if (myEntityForExecutable == null) {
                string message = "Error - You need to specify executable primitive  "; // +"; EntityName is: " + myEntityForExecutable.EntityName;
                Logging.WriteLogSimple("Error - You need to specify executable primitive  ");
                MessageBoxResult result = MessageBox.Show(message, "Run-time Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string strExecutable = myEntityForExecutable;

            if (!File.Exists(strExecutable) && strExecutable != "iexplore" && strExecutable != "microsoft-edge:") {
                string directory = AppDomain.CurrentDomain.BaseDirectory;
                string altExecutable = GetValueByKey(strExecutable);

                if (altExecutable == "" || altExecutable == null) {
                    //TODO: ask them if they want to add an alt executable
                    // if they do, move it to strExecutable
                    List<ControlEntity> myListControlEntity = new List<ControlEntity>();

                    ControlEntity myControlEntity = new ControlEntity();
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Heading;
                    myControlEntity.Text = "Executable Not Found";
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());


                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "myLabel";
                    myControlEntity.Text = "Executable not found for " + strExecutable;
                    myControlEntity.RowNumber = 0;
                    myControlEntity.ColumnNumber = 0;
                    myControlEntity.TextWrap = true;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "myLabel";
                    myControlEntity.Text = "The application is " + directory;
                    myControlEntity.RowNumber = 1;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());




                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "myLabel";
                    myControlEntity.Text = "Do you want to override the executable with a new one?" + System.Environment.NewLine + "Click Okay to add alt executable or Cancel to continue without adding alt execuatble";
                    myControlEntity.RowNumber = 2;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());


                    string strButtonPressed = WindowMultipleControls(ref myListControlEntity, 600, 800, 0, 0);
                    if (strButtonPressed != "btnCancel") {

                        myListControlEntity = new List<ControlEntity>();

                        myControlEntity = new ControlEntity();
                        myControlEntity.ControlEntitySetDefaults();
                        myControlEntity.ControlType = ControlType.Heading;
                        myControlEntity.Text = "Create Alt Executable";
                        myListControlEntity.Add(myControlEntity.CreateControlEntity());


                        myControlEntity.ControlEntitySetDefaults();
                        myControlEntity.ControlType = ControlType.Label;
                        myControlEntity.ID = "myLabel";
                        myControlEntity.Text = "Alt Executable:";
                        myControlEntity.RowNumber = 0;
                        myControlEntity.ColumnNumber = 0;
                        myListControlEntity.Add(myControlEntity.CreateControlEntity());

                        myControlEntity.ControlEntitySetDefaults();
                        myControlEntity.ControlType = ControlType.TextBox;
                        myControlEntity.ID = "myAltExecutable";
                        myControlEntity.Text = "";
                        myControlEntity.RowNumber = 0;
                        myControlEntity.ColumnNumber = 1;
                        myListControlEntity.Add(myControlEntity.CreateControlEntity());

                        WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
                        string strAltExecutable = myListControlEntity.Find(x => x.ID == "myAltExecutable").Text;
                        SetValueByKey(strExecutable, strAltExecutable);
                        strExecutable = strAltExecutable;

                    }
                } else {
                    strExecutable = altExecutable;
                }
            }

            string strContent = "";
            if (myEntityForContent != null) {
                strContent = myEntityForContent;
            }
            if (strContent == "") {

                Process.Start(strExecutable);
            } else {
                try {
                    if (strExecutable == "microsoft-edge:") {
                        Process.Start(strExecutable + strContent);
                    } else {

                        Process.Start(strExecutable, string.Concat("", strContent, ""));
                    }
                } catch (Exception ex) {

                    MessageBox.Show("Here is an exception thrown in the Run method in IdealAutomateCore for Executable " + strExecutable + ": " + ex.Message);
                }
            }



        }
        /// Category::Process
        public int RunProcessAsAdmin(string exeName, string parameters) {
            try {
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.UseShellExecute = true;
                // startInfo.WorkingDirectory = CurrentDirectory;;
                startInfo.FileName = exeName;
                startInfo.Verb = "runas";
                //MLHIDE
                startInfo.Arguments = parameters;
                startInfo.ErrorDialog = true;

                Process process = System.Diagnostics.Process.Start(startInfo);
                process.WaitForExit();
                return process.ExitCode;
            } catch (Win32Exception ex) {
                MessageBox.Show(ex.ToString() + "Line 1735 in Methods");
                switch (ex.NativeErrorCode) {
                    case 1223:
                        return ex.NativeErrorCode;
                    default:
                        return 777;
                }

            } catch (Exception ex) {
                MessageBox.Show(ex.ToString() + "Line 1744 in Methods");
                return 7777;
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
        /// Category::Run
        public void RunSync(string myEntityForExecutable, string myEntityForContent) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "RunSync: myEntityForExecutable=" + myEntityForExecutable + " myEntityForContent=" + myEntityForContent);
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "RunSync: myEntityForExecutable=" + myEntityForExecutable + " myEntityForContent=" + myEntityForContent);
            }

            if (myEntityForExecutable == null) {
                string message = "Error - You need to specify executable primitive  "; // +"; EntityName is: " + myEntityForExecutable.EntityName;
                MessageBoxResult result = MessageBox.Show(message, "Run-time Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string strExecutable = myEntityForExecutable;

            if (!File.Exists(strExecutable) && strExecutable != "iexplore") {
                string directory = AppDomain.CurrentDomain.BaseDirectory;
                string altExecutable = GetValueByKey(strExecutable);

                if (altExecutable == "" || altExecutable == null) {
                    //TODO: ask them if they want to add an alt executable
                    // if they do, move it to strExecutable
                    List<ControlEntity> myListControlEntity = new List<ControlEntity>();

                    ControlEntity myControlEntity = new ControlEntity();
                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Heading;
                    myControlEntity.Text = "Executable Not Found";
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());


                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "myLabel";
                    myControlEntity.Text = "Executable not found for " + strExecutable;
                    myControlEntity.RowNumber = 0;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "myLabel";
                    myControlEntity.Text = "The application is " + directory;
                    myControlEntity.RowNumber = 1;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());




                    myControlEntity.ControlEntitySetDefaults();
                    myControlEntity.ControlType = ControlType.Label;
                    myControlEntity.ID = "myLabel";
                    myControlEntity.Text = "Do you want to override the executable with a new one?" + System.Environment.NewLine + "Click Okay to add alt executable or Cancel to continue without adding alt execuatble";
                    myControlEntity.RowNumber = 2;
                    myControlEntity.ColumnNumber = 0;
                    myListControlEntity.Add(myControlEntity.CreateControlEntity());


                    string strButtonPressed = WindowMultipleControls(ref myListControlEntity, 600, 800, 0, 0);
                    if (strButtonPressed != "btnCancel") {

                        myListControlEntity = new List<ControlEntity>();

                        myControlEntity = new ControlEntity();
                        myControlEntity.ControlEntitySetDefaults();
                        myControlEntity.ControlType = ControlType.Heading;
                        myControlEntity.Text = "Create Alt Executable";
                        myListControlEntity.Add(myControlEntity.CreateControlEntity());


                        myControlEntity.ControlEntitySetDefaults();
                        myControlEntity.ControlType = ControlType.Label;
                        myControlEntity.ID = "myLabel";
                        myControlEntity.Text = "Alt Executable:";
                        myControlEntity.RowNumber = 0;
                        myControlEntity.ColumnNumber = 0;
                        myListControlEntity.Add(myControlEntity.CreateControlEntity());

                        myControlEntity.ControlEntitySetDefaults();
                        myControlEntity.ControlType = ControlType.TextBox;
                        myControlEntity.ID = "myAltExecutable";
                        myControlEntity.Text = "";
                        myControlEntity.RowNumber = 0;
                        myControlEntity.ColumnNumber = 1;
                        myListControlEntity.Add(myControlEntity.CreateControlEntity());

                        WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
                        string strAltExecutable = myListControlEntity.Find(x => x.ID == "myAltExecutable").Text;
                        SetValueByKey(strExecutable, strAltExecutable);
                        strExecutable = strAltExecutable;

                    }
                } else {
                    strExecutable = altExecutable;
                }
            }

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
                Logging.WriteLogSimple("ID: " + procId);
            } catch (InvalidOperationException) {
                started = false;
            } catch (Exception ex) {
                started = false;
            }
            while (started == true && GetProcByID(procId) != null) {
                System.Threading.Thread.Sleep(1000);
            }

        }
        /// <summary>
        /// <para>The following method tries to start a service specified by a service name. Then it waits until the service </para>
        /// <para>is running or a timeout occurs.</para>
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="timeoutMilliseconds"></param>
        /// Category::Service
        public void StartService(string serviceName, int timeoutMilliseconds) {
            ServiceController service = new ServiceController(serviceName);

            try {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            } catch (Exception ex) {
                string message = "Error - Problem starting service  " + serviceName + " " + ex.Message + " " + ex.InnerException; // +"; EntityName is: " + myEntityForExecutable.EntityName;
                MessageBoxResult result = MessageBox.Show(message, "Run-time Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        /// <summary>
        /// <para>The following method tries to stop the specified service and it waits until the service is stopped or a timeout occurs.</para>
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="timeoutMilliseconds"></param>
        /// Category::Service
        public void StopService(string serviceName, int timeoutMilliseconds) {
            ServiceController service = new ServiceController(serviceName);
            try {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            } catch {
                string message = "Error - Problem stopping service  " + serviceName; // +"; EntityName is: " + myEntityForExecutable.EntityName;
                MessageBoxResult result = MessageBox.Show(message, "Run-time Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        /// <summary>
        /// <para>This method combinates both previous methods. It tries to stop the service (and waits until it's stopped) </para>
        /// <para>then it begins to start the service (and waits until the service is running). The specified timeout is used </para>
        /// for both operations together.
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="timeoutMilliseconds"></param>
        /// Category::Service
        public void RestartService(string serviceName, int timeoutMilliseconds) {
            ServiceController service = new ServiceController(serviceName);
            try {
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            } catch {
                string message = "Error - Problem restarting service  " + serviceName; // +"; EntityName is: " + myEntityForExecutable.EntityName;
                MessageBoxResult result = MessageBox.Show(message, "Run-time Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        /// Category::Window
        public string GetActiveWindowTitle() {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr hwnd = GetForegroundWindow();
            var int32Handle = hwnd.ToInt32();
            if (GetWindowText(int32Handle, Buff, nChars) > 0) {
                return Buff.ToString();
            }
            return null;
        }

        private Process GetProcByID(int id) {
            Process[] processlist = Process.GetProcesses();
            return processlist.FirstOrDefault(pr => pr.Id == id);
        }
        /// <summary>
        /// Sleep receives an integer that indicates the number of milliseconds that you want the program to wait.
        /// </summary>
        /// <param name="intSleep">integer that indicates the number of milliseconds that you want the program to wait.</param>
        /// Category::Sleep
        public void Sleep(int intSleep) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "Sleep:  intSleep=" + intSleep.ToString());
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "Sleep:  intSleep=" + intSleep.ToString());
            }
            System.Threading.Thread.Sleep(intSleep);
        }

        /// Category::Search
        public void FindDelimitedText(FindDelimitedTextParms myParms) {
            // set defaults of not found and empty string
            myParms.intDelimFound = -1;
            myParms.strDelimitedTextFound = "";
            int intStartingLine = myParms.intLineCtr;

            ExamineALine:
            // if we reach the end of the lines array,
            // we could not find delimited text and we are done
            if (myParms.intLineCtr > myParms.lines.Count()) {
                myParms.strResultTypeFound = "Not Found";
                return;
            }
            // move the current line that we want to examine
            // to myLine

            string myLine = myParms.lines[myParms.intLineCtr];
            if (myParms.intLineCtr == intStartingLine) {
                myLine = myLine.Substring(myParms.intStartingCol);
            }

            // we are going to loop the the begin and end 
            // delim pairs looking for a match for any pair
            // in the entire lines array starting at the initial
            // intLinesCtr.
            // this will exit on the first one that it finds

            for (int i = 0; i < myParms.lsBeginDelim.Count; i++) {
                string strBeginDelim = myParms.lsBeginDelim[i].ToUpper();
                string strEndDelim = myParms.lsEndDelim[i].ToUpper();
                // Find location of beginning delim
                int intBeginDelimLength = strBeginDelim.Length;

                LookForBeginDelim:
                int indexBeginDelim = myLine.ToUpper().IndexOf(strBeginDelim);
                // Get the rest of the line after Begin Delim
                if (indexBeginDelim == -1) {
                    if (myParms.intLineCtr < myParms.lines.Count() - 1) {
                        myParms.intLineCtr++;
                        myLine = myParms.lines[myParms.intLineCtr];
                        goto LookForBeginDelim;
                    }
                }

                // if we went thru the entire lines array
                // and did not find BeginDelim, we need to
                // go get the next begin and end delim pair
                // and start over

                if (indexBeginDelim == -1) {
                    myParms.intLineCtr = intStartingLine;
                    myLine = myParms.lines[myParms.intLineCtr];
                    myLine = myLine.Substring(myParms.intStartingCol);
                    continue;
                }

                // we have found the begin delim so we want to save
                // it as strResultType to tell us which begin and end
                // delim pair we found. We still need to look for the 
                // end delim in order to have a successful find

                strBeginDelim = myLine.Substring(indexBeginDelim, intBeginDelimLength);
                if (myParms.lsBeginDelim.Count > 1) {
                    myParms.strResultTypeFound = strBeginDelim;
                }
                myParms.intDelimFound = i;
                string d = myLine.Substring(indexBeginDelim + intBeginDelimLength);
                // Find location of EndDelim

                LookForEndDelim:

                int indexEndDelim = d.ToUpper().IndexOf(strEndDelim);
                if (indexEndDelim == -1) {
                    if (myParms.intLineCtr < myParms.lines.Count() - 1) {
                        myParms.intLineCtr++;
                        myLine = myParms.lines[myParms.intLineCtr];
                        goto LookForEndDelim;
                    }
                }

                // if we went thru the entire lines array
                // and did not find EndDelim, we need to
                // go get the next begin and end delim pair
                // and start over

                if (indexEndDelim == -1) {
                    myParms.intLineCtr = intStartingLine;
                    myLine = myParms.lines[myParms.intLineCtr];
                    myLine = myLine.Substring(myParms.intStartingCol);
                    continue;
                }

                d = d.Substring(0, indexEndDelim);
                myParms.strDelimitedTextFound = d;
                myParms.intEndDelimColPosFound = indexEndDelim + strEndDelim.Length;
                return;
            }

            if (myParms.lsBeginDelim.Count > 1) {
                myParms.intLineCtr++;
                goto ExamineALine;
            }
        }
        /// Category::Internet Explorer
        public void IEGoToURL(Methods myActions, string myWebSite, bool boolUseNewTab) {

            if (boolUseNewTab == true) {
                List<string> myWindowTitles = myActions.GetWindowTitlesByProcessName("iexplore");
                myWindowTitles.RemoveAll(item => item == "");
                if (myWindowTitles.Count > 0) {
                    myActions.ActivateWindowByTitle(myWindowTitles[0], (int)WindowShowEnum.SW_SHOWMAXIMIZED);
                    myActions.TypeText("%(d)", 1500); // select address bar
                    myActions.TypeText("{ESC}", 1500);
                    myActions.TypeText("%({ENTER})", 1500); // Alt enter while in address bar opens new tab
                    myActions.TypeText("%(d)", 1500);
                    myActions.TypeText(myWebSite, 1500);
                    myActions.TypeText("{ENTER}", 1500);
                    myActions.TypeText("{ESC}", 1500);

                } else {
                    myActions.Run("iexplore", myWebSite);

                }
            } else {
                myActions.Run("iexplore", myWebSite);
            }
        }

        private List<SubPositionInfo> Click_PNG(ImageEntity myImage, bool boolUseGrayScaleDB) {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "Click_PNG:");
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "Click_PNG:");
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(myImage)) {
                    string name = descriptor.Name;
                    object value = descriptor.GetValue(myImage);
                    Console.WriteLine("{0}={1}", name, value);
                    Logging.WriteLogSimple(String.Format("{0}={1}", name, value));
                }
            }
            System.Threading.Thread.Sleep(100);


            //Bitmap bm = BytesToBitmap(myEntityForPicture.ImageFile);
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            Bitmap bm;
            if (myImage.ImageFile.Contains(":")) {
                bm = new Bitmap(myImage.ImageFile);
            } else {
                bm = new Bitmap(directory + myImage.ImageFile);
            }
            // Bitmap bm = new Bitmap(directory + pPNG_File_Name);
            //  Bitmap bm = BytesToBitmap(scriptStep.SubImage);
            // Bitmap bm = null;
            //     bm.MakeTransparent(Color.White);


            // previewImageBox.Source = BitmapSourceFromImage(bm);
            //bm;
            // find the least popular color in sub image and find the relative position in
            // subimage
            if (boolUseGrayScaleDB) {
                intFileCtr += 1;
                string myfile = "temp" + intFileCtr + ".bmp";
                System.IO.File.Delete(directory + myfile);
                bm.Save(directory + myfile, System.Drawing.Imaging.ImageFormat.Bmp);
                System.Drawing.Image myImageD = System.Drawing.Image.FromFile(directory + myfile);
                // myImage = System.Drawing.Image.FromFile(@"C:\TFS\WadeHome\Applications\TreeView\Sample Application\Images\Small.png");

                myImageD = Scraper.ConvertToGrayscale(myImageD);
                bm = new Bitmap(myImageD);
                intFileCtr += 1;
                myfile = "temp" + intFileCtr + ".bmp";
                System.IO.File.Delete(directory + myfile);
                bm.Save(directory + myfile, System.Drawing.Imaging.ImageFormat.Bmp);

                myImageD.Dispose();
            }
            Hashtable pixels = new Hashtable(); // looks like this is no longer used and can be deleted

            //resultsTextBox.Text += "Searching..." + scriptStep.Seq1.ToString() + Environment.NewLine;
            Console.WriteLine(oProcess.ProcessName + "==> " + "Searching..." + myImage.ImageFile + Environment.NewLine);
            Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "Searching..." + myImage.ImageFile + Environment.NewLine);


            // Find subimages
            Bitmap bmx;

            bmx = Scraper.getDesktopBitmap(boolUseGrayScaleDB); //forcing both images to be the same Scraper.getDesktopBitmap(boolUseGrayScaleDB); // BytesToBitmap(myEntityForPicture.ImageFile); //forcing both images to be the same


            // bigPictureImageBox.Source = BitmapSourceFromImage(bmx);
            decimal highestPercentCorrect = 0;

            List<SubPositionInfo> ls = Scraper.GetSubPositions(bmx, bm, boolUseGrayScaleDB, ref highestPercentCorrect, myImage.Tolerance);
            // List<System.Drawing.Point> ls = null;
            int intLastSlashIndex = myImage.ImageFile.LastIndexOf("\\");
            string strfilename = myImage.ImageFile.Substring(intLastSlashIndex + 1);
            if (ls.Count > 0) {



                for (int i = 0; i < ls.Count; i++) {
                    System.Drawing.Point p = ls[i].myPoint;

                    string myfile = "tempsmall" + strfilename + i.ToString() + ".bmp";

                    System.IO.File.Delete(directory + myfile);
                    bm.Save(directory + myfile, System.Drawing.Imaging.ImageFormat.Bmp);
                    myfile = "tempbig" + strfilename + i.ToString() + ".bmp";
                    System.IO.File.Delete(directory + myfile);
                    bmx.Save(directory + myfile, System.Drawing.Imaging.ImageFormat.Bmp);

                    Console.WriteLine(oProcess.ProcessName + "==> " + "Image found at: " + p.ToString() + strfilename + i.ToString() + " highestPercentCorrect=" + ls[i].percentcorrect.ToString() + ";Search Method: " + ls[i].strSearchMethod + Environment.NewLine);
                    Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "Image found at: " + p.ToString() + strfilename + i.ToString() + " highestPercentCorrect=" + ls[i].percentcorrect.ToString() + ";Search Method: " + ls[i].strSearchMethod + Environment.NewLine);
                    int intOffX = p.X + myImage.RelativeX;
                    int intOffY = p.Y + myImage.RelativeY;


                    p.X = intOffX;
                    p.Y = intOffY;
                    ls[i].myPoint = p;
                    //Position_Cursor.MoveMouse(intOffX, intOffY);
                    //UInt32 myX = Convert.ToUInt32(intOffX);
                    //UInt32 myY = Convert.ToUInt32(intOffY);
                    //Position_Cursor.DoMouseClick(myX, myY);
                    //System.Threading.Thread.Sleep(1000);
                    // System.Windows.Forms.SendKeys.SendWait("Wade is a nut");

                }

                return ls;
            } else {

                string myfile = "tempsmall" + strfilename + ".bmp";

                System.IO.File.Delete(directory + myfile);
                bm.Save(directory + myfile, System.Drawing.Imaging.ImageFormat.Bmp);
                myfile = "tempbig" + strfilename + ".bmp";
                System.IO.File.Delete(directory + myfile);
                bmx.Save(directory + myfile, System.Drawing.Imaging.ImageFormat.Bmp);

                myfile = "tempbig" + strfilename + ".bmp";

                Console.WriteLine(oProcess.ProcessName + "==> " + "Image not found" + strfilename + " highestPercentCorrect=" + highestPercentCorrect.ToString() + Environment.NewLine);
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "Image not found" + strfilename + " highestPercentCorrect=" + highestPercentCorrect.ToString() + Environment.NewLine);
                return ls;
            }
        }

        /// <summary>
        /// Evaluates Cursor Position with respect to client screen.
        /// </summary>
        private void EvaluateCaretPosition() {

            caretPosition = new System.Drawing.Point();

            // Fetch GUITHREADINFO
            GetCaretPosition();

            caretPosition.X = (int)guiInfo.rcCaret.Left;
            caretPosition.Y = (int)guiInfo.rcCaret.Top;
            //       MessageBox.Show(caretPosition.X.ToString() + "," + caretPosition.Y.ToString());
            ClientToScreen(guiInfo.hwndCaret, out caretPosition);
            //        MessageBox.Show(caretPosition.X.ToString() + "," + caretPosition.Y.ToString());

            //txtCaretX.Text = (caretPosition.X).ToString();
            //txtCaretY.Text = caretPosition.Y.ToString();

        }

        /// <summary>
        /// Get the caret position
        /// </summary>
        private void GetCaretPosition() {
            if (fbDebugMode) {
                Console.WriteLine(oProcess.ProcessName + "==> " + "GetCaretPosition");
                Logging.WriteLogSimple(oProcess.ProcessName + "==> " + "GetCaretPosition");
            }
            guiInfo = new GUITHREADINFO();
            guiInfo.cbSize = (uint)Marshal.SizeOf(guiInfo);

            // Get GuiThreadInfo into guiInfo
            GetGUIThreadInfo(0, out guiInfo);
        }

        /// <summary>
        /// Retrieves name of active Process.
        /// </summary>
        /// <returns>Active Process Name</returns>
        private string GetActiveProcess() {
            const int nChars = 256;
            int handle = 0;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = (int)GetForegroundWindow();

            // If Active window has some title info
            if (GetWindowText(handle, Buff, nChars) > 0) {
                uint lpdwProcessId;
                uint dwCaretID = GetWindowThreadProcessId(handle, out lpdwProcessId);
                uint dwCurrentID = (uint)Thread.CurrentThread.ManagedThreadId;
                return Process.GetProcessById((int)lpdwProcessId).ProcessName;
            }
            // Otherwise either error or non client region
            return String.Empty;
        }
        private string GetActiveProcessTitle() {
            const int nChars = 256;
            int handle = 0;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = (int)GetForegroundWindow();
            // If Active window has some title info..
            if (GetWindowText(handle, Buff, nChars) > 0) {
                uint lpdwProcessId;
                uint dwCaretID = GetWindowThreadProcessId(handle, out lpdwProcessId);
                uint dwCurrentID = (uint)Thread.CurrentThread.ManagedThreadId;
                //   MessageBox.Show(Process.GetProcessById((int)lpdwProcessId).MainWindowTitle);
                return Process.GetProcessById((int)lpdwProcessId).MainWindowTitle;
            }
            // Otherwise either error or non client region
            return String.Empty;
        }

        /// <summary>
        /// <para>GetAppDirectoryForScript gets the application </para>
        /// <para>data folder and adds \IdealAutomate\yourscriptname to it.</para>
        /// <para>The AppDirectory allows you to store personal settings and</para>
        /// <para>information that you want to keep private (like passwords) in a location</para>
        /// <para>outside of your script on in the application directory</para>
        /// </summary>
        /// <returns>string that is the app_data/roaming directory path for the script</returns>
        /// Category::Directory
        public string GetAppDirectoryForScript() {

            string directory = AppDomain.CurrentDomain.BaseDirectory;
            directory = directory.Replace("\\bin\\Debug\\", "");
            int intLastSlashIndex = directory.LastIndexOf("\\");
            //string strScriptName = directory.Substring(intLastSlashIndex + 1);
            // string strScriptName = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
            string settingsDirectory =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealAutomateExplorer";
            if (!Directory.Exists(settingsDirectory)) {
                Directory.CreateDirectory(settingsDirectory);
            }
            return settingsDirectory;
        }

        /// <summary>
        /// <para>GetAppDirectoryForIdealAutomate gets the application </para>
        /// <para>data folder and adds \IdealAutomate\yourscriptname to it.</para>
        /// <para>The AppDirectory allows you to store personal settings and</para>
        /// <para>information that you want to keep private (like passwords) in a location</para>
        /// <para>outside of your script on in the application directory</para>
        /// </summary>
        /// <returns>string that is the app_data/roaming directory path for the script</returns>
        /// Category::Directory
        public string GetAppDirectoryForIdealAutomate() {
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            directory = directory.Replace("\\bin\\Debug\\", "");
            int intLastSlashIndex = directory.LastIndexOf("\\");
            string strScriptName = directory.Substring(intLastSlashIndex + 1);
            // string strScriptName = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
            string settingsDirectory =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate";
            if (!Directory.Exists(settingsDirectory)) {
                Directory.CreateDirectory(settingsDirectory);
            }
            return settingsDirectory;
        }


        /// <summary>
        /// <para>GetValueByKey takes a key and adds .txt to it in order to create</para>
        /// <para>a file name. It gets the app data path and adds \IdealAutomate\yourscriptname</para>
        /// <para>to it. By combining that path to the file name created from the key,</para>
        /// <para>it can retrieve a value from the key that is unique to your script application.</para>
        ///  <para>The AppDirectory allows you to store personal settings and</para>
        /// <para>information that you want to keep private (like passwords) in a location</para>
        /// <para>outside of your script on in the application directory</para>
        /// </summary>
        /// <param name="strKey">Unique key within the script application</param>
        /// <returns>string that was in application directory for that key</returns>
        /// Category::Storage>Private
        public string GetValueByKey(string strKey) {
            string fileName = strKey + ".txt";
            StreamReader file = null;
            string strValueRead = "";
            string settingsDirectory = GetAppDirectoryForScript();
            string settingsPath = "";
            try {
                settingsPath = Path.Combine(settingsDirectory, fileName);
            } catch (Exception e) {

                MessageBoxShow(e.Message + " - filename = " + fileName + " - Line 2369 in Methods.cs");
            }
            if (File.Exists(settingsPath)) {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            }
            return strValueRead;
        }
        /// Category::Storage>Private
        public int GetValueByKeyAsInt(string strKey) {
            string fileName = strKey + ".txt";
            StreamReader file = null;
            string strValueRead = "";
            string settingsDirectory = GetAppDirectoryForScript();
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            }
            int intValue = 0;
            Int32.TryParse(strValueRead, out intValue);
            return intValue;
        }
        /// Category::Storage>Private
        public int GetValueByKeyAsIntForNonCurrentScript(string strKey, string strScriptName) {
            string fileName = strKey + ".txt";
            StreamReader file = null;
            string strValueRead = "";
            string settingsDirectory = GetAppDirectoryForIdealAutomate();
            settingsDirectory = Path.Combine(settingsDirectory, strScriptName);
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            }
            int intValue = 0;
            Int32.TryParse(strValueRead, out intValue);
            return intValue;
        }
        /// Category::Storage>Public
        public int GetValueByPublicKeyAsIntForNonCurrentScript(string strKey, string strFullFolderPath) {
            string fileName = strKey + ".txt";
            StreamReader file = null;
            string strValueRead = "";
            string settingsDirectory = strFullFolderPath;
            settingsDirectory = Path.Combine(settingsDirectory, "..IdealAutomate");
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            }
            int intValue = 0;
            Int32.TryParse(strValueRead, out intValue);
            return intValue;
        }
        /// Category::Storage>Private
        public string GetValueByKeyForNonCurrentScript(string strKey, string strScriptName) {
            string fileName = strKey + ".txt";
            StreamReader file = null;
            string strValueRead = "";
            string settingsDirectory = GetAppDirectoryForIdealAutomate();
            settingsDirectory = Path.Combine(settingsDirectory, strScriptName);
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            }
            return strValueRead;
        }
        /// Category::Storage>Public
        public string GetValueByPublicKeyForNonCurrentScript(string strKey, string strFullFolderPath) {
            string fileName = strKey + ".txt";
            StreamReader file = null;
            string strValueRead = "";
            string settingsDirectory = strFullFolderPath;
            settingsDirectory = Path.Combine(settingsDirectory, "..IdealAutomate");
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            }
            return strValueRead;
        }
        /// Category::Storage>Public
        public string GetValueByPublicKeyInCurrentFolder(string strKey, string strFullFileName) {
            string strValueRead = "";
            string fileName = strKey + ".txt";
            StreamReader file = null;
            string settingsDirectory = "";
            string fileNamePartOfFullPath = "";
            bool isDirectory = false;
            if (File.Exists(strFullFileName)) {

                // path is a file.
                settingsDirectory = ConvertFullFileNameToPublicPath(strFullFileName);
                fileNamePartOfFullPath = Path.GetFileName(strFullFileName);

            } else if (Directory.Exists(strFullFileName)) {

                // path is a directory.
                settingsDirectory = strFullFileName;
                isDirectory = true;

            } else {
                // file does not exist yet...so we make assumption that it is a file if it contains period
                if (strFullFileName.Contains(".")) {
                    // path is a file.
                    settingsDirectory = ConvertFullFileNameToPublicPath(strFullFileName);
                    fileNamePartOfFullPath = Path.GetFileName(strFullFileName);
                } else {
                    // path is a directory.
                    settingsDirectory = strFullFileName;
                    isDirectory = true;
                }


            }

            settingsDirectory = Path.Combine(settingsDirectory, "..IdealAutomate");
            if (!isDirectory) {
                if (fileNamePartOfFullPath.Contains(".")) {
                    settingsDirectory = Path.Combine(settingsDirectory, fileNamePartOfFullPath.Substring(0, fileNamePartOfFullPath.IndexOf(".")));
                } else {
                    settingsDirectory = Path.Combine(settingsDirectory, fileNamePartOfFullPath);
                }
            }
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            }
            return strValueRead;
        }
        /// Category::Storage>Private
        public double GetValueByKeyAsDouble(string strKey) {
            string fileName = strKey + ".txt";
            StreamReader file = null;
            string strValueRead = "";
            string settingsDirectory = GetAppDirectoryForScript();
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            }
            double intValue = 0;
            double.TryParse(strValueRead, out intValue);
            return intValue;
        }
        /// Category::Storage>Private
        public DateTime GetValueByKeyAsDateTime(string strKey) {
            string fileName = strKey + ".txt";
            StreamReader file = null;
            string strValueRead = "";
            string settingsDirectory = GetAppDirectoryForScript();
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            }
            DateTime dtScriptStartDateTime = DateTime.MinValue;
            bool boolDTSuccessful = DateTime.TryParse(strValueRead, out dtScriptStartDateTime);
            return dtScriptStartDateTime;
        }
        /// Category::Storage>Private
        public DateTime GetValueByKeyAsDateTimeForNonCurrentScript(string strKey, string strScriptName) {
            string fileName = strKey + ".txt";
            StreamReader file = null;
            string strValueRead = "";
            string settingsDirectory = GetAppDirectoryForIdealAutomate();
            settingsDirectory = Path.Combine(settingsDirectory, strScriptName);
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            }
            DateTime dtScriptStartDateTime = DateTime.MinValue;
            bool boolDTSuccessful = DateTime.TryParse(strValueRead, out dtScriptStartDateTime);
            return dtScriptStartDateTime;
        }
        /// Category::Storage>Private
        public DateTime GetValueByPublicKeyAsDateTimeForNonCurrentScript(string strKey, string strFullFolderPath) {
            string fileName = strKey + ".txt";
            StreamReader file = null;
            string strValueRead = "";
            string settingsDirectory = strFullFolderPath;
            settingsDirectory = Path.Combine(settingsDirectory, "..IdealAutomate");
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            }
            DateTime dtScriptStartDateTime = DateTime.MinValue;
            bool boolDTSuccessful = DateTime.TryParse(strValueRead, out dtScriptStartDateTime);
            return dtScriptStartDateTime;
        }
        /// Category::Storage>Private
        public int GetValueByKeyAsIntGlobal(string strKey) {
            string fileName = strKey + ".txt";
            StreamReader file = null;
            string strValueRead = "";
            string settingsDirectory = GetAppDirectoryForIdealAutomate();
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            }
            int intValue = 0;
            Int32.TryParse(strValueRead, out intValue);
            return intValue;
        }
        /// Category::Storage>Private
        public int IncrementValueByKeyByValue(string strKey, int intIncrementValue) {
            string fileName = strKey + ".txt";
            StreamReader file = null;
            string strValueRead = "";
            string settingsDirectory = GetAppDirectoryForScript();
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            }
            int intValue = 0;
            Int32.TryParse(strValueRead, out intValue);
            int intResult = intValue + intIncrementValue;
            SetValueByKey(strKey, intResult.ToString());
            return intResult;
        }
        /// Category::Storage>Private
        public int IncrementValueByKeyByValueGlobal(string strKey, int intIncrementValue) {
            string fileName = strKey + ".txt";
            StreamReader file = null;
            string strValueRead = "";
            string settingsDirectory = GetAppDirectoryForIdealAutomate();
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            }
            int intValue = 0;
            Int32.TryParse(strValueRead, out intValue);
            int intResult = intValue + intIncrementValue;
            SetValueByKey(strKey, intResult.ToString());
            return intResult;
        }

        /// <summary>
        /// <para>SetValueByKey takes a key and adds .txt to it in order to create</para>
        /// <para>a file name. It gets the app data path and adds \IdealAutomate\yourscriptname</para>
        /// <para>to it. By combining that path to the file name,</para>
        /// <para>it can write a value to the key that is unique to your script application.</para>
        /// <para>The AppDirectory allows you to store personal settings and</para>
        /// <para>information that you want to keep private (like passwords) in a location</para>
        /// <para>outside of your script on in the application directory</para>
        /// </summary>
        /// <param name="strKey">Unique key within the script application</param>
        /// <param name="strValueToWrite">Value to write to the Unique key 
        /// within the script application</param>
        /// Category::Storage>Private
        public void SetValueByKey(string strKey, string strValueToWrite) {
            string fileName = strKey + ".txt";
            StreamWriter writer = null;
            string settingsDirectory = GetAppDirectoryForScript();
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            // Hook a write to the text file.
            writer = new StreamWriter(settingsPath);
            // Rewrite the entire value of s to the file
            writer.Write(strValueToWrite);
            writer.Close();
        }
        /// Category::Storage>Private
        public void SetValueByKeyForNonCurrentScript(string strKey, string strValueToWrite, string strScriptName) {
            string fileName = strKey + ".txt";
            StreamWriter writer = null;
            string settingsDirectory = GetAppDirectoryForIdealAutomate();
            settingsDirectory = Path.Combine(settingsDirectory, strScriptName);
            if (!Directory.Exists(settingsDirectory)) {
                Directory.CreateDirectory(settingsDirectory);
            }
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            // Hook a write to the text file.
            writer = new StreamWriter(settingsPath);
            // Rewrite the entire value of s to the file
            writer.Write(strValueToWrite);
            writer.Close();
        }
        /// Category::Storage>Public
        public void SetValueByPublicKeyForNonCurrentScript(string strKey, string strValueToWrite, string strFullFolderPath) {
            string fileName = strKey + ".txt";
            StreamWriter writer = null;
            string settingsDirectory = strFullFolderPath;
            settingsDirectory = Path.Combine(settingsDirectory, "..IdealAutomate");
            if (!Directory.Exists(settingsDirectory)) {
                Directory.CreateDirectory(settingsDirectory);
            }
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            // Hook a write to the text file.
            writer = new StreamWriter(settingsPath);
            // Rewrite the entire value of s to the file
            writer.Write(strValueToWrite);
            writer.Close();
        }
        /// Category::Storage>Public
        public void SetValueByPublicKeyInCurrentFolder(string strKey, string strValueToWrite, string strFullFileName) {
            string fileName = strKey + ".txt";
            StreamWriter writer = null;
            string settingsDirectory = "";
            string fileNamePartOfFullPath = "";
            bool isDirectory = false;
            if (File.Exists(strFullFileName)) {

                // path is a file.
                settingsDirectory = ConvertFullFileNameToPublicPath(strFullFileName);
                fileNamePartOfFullPath = Path.GetFileName(strFullFileName);

            } else if (Directory.Exists(strFullFileName)) {

                // path is a directory.
                settingsDirectory = strFullFileName;
                isDirectory = true;

            } else {
                // file does not exist yet...so we make assumption that it is a file if it contains period
                if (strFullFileName.Contains(".")) {
                    // path is a file.
                    settingsDirectory = ConvertFullFileNameToPublicPath(strFullFileName);
                    fileNamePartOfFullPath = Path.GetFileName(strFullFileName);
                } else {
                    // path is a directory.
                    settingsDirectory = strFullFileName;
                    isDirectory = true;
                }


            }

            settingsDirectory = Path.Combine(settingsDirectory, "..IdealAutomate");
            if (!Directory.Exists(settingsDirectory)) {
                Directory.CreateDirectory(settingsDirectory);
            }
            if (!isDirectory) {
                if (fileNamePartOfFullPath.Contains(".")) {
                    settingsDirectory = Path.Combine(settingsDirectory, fileNamePartOfFullPath.Substring(0, fileNamePartOfFullPath.IndexOf(".")));
                } else {
                    settingsDirectory = Path.Combine(settingsDirectory, fileNamePartOfFullPath);
                }
                if (!Directory.Exists(settingsDirectory)) {
                    Directory.CreateDirectory(settingsDirectory);
                }
            }
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            // Hook a write to the text file.
            writer = new StreamWriter(settingsPath);
            // Rewrite the entire value of s to the file
            writer.Write(strValueToWrite);
            writer.Close();
        }
        /// <summary>
        /// <para>GetValueByKeyGlobal takes a key and adds .txt to it in order to create</para>
        /// <para>a file name. It gets the app data path and adds \IdealAutomate\yourscriptname</para>
        /// <para>to it. By combining that path to the file name created from the key,</para>
        /// <para>it can retrieve a value from the key that is unique to your script application.</para>
        ///  <para>The AppDirectory allows you to store personal settings and</para>
        /// <para>information that you want to keep private (like passwords) in a location</para>
        /// <para>outside of your script on in the application directory</para>
        /// </summary>
        /// <param name="strKey">Unique key within the script application</param>
        /// <returns>string that was in application directory for that key</returns>
        /// Category::Storage>Private
        public string GetValueByKeyGlobal(string strKey) {
            string fileName = strKey + ".txt";
            StreamReader file = null;
            string strValueRead = "";
            string settingsDirectory = GetAppDirectoryForIdealAutomate();
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            }
            return strValueRead;
        }

        /// <summary>
        /// <para>GetValueByKeyGlobal takes a key and adds .txt to it in order to create</para>
        /// <para>a file name. It gets the app data path and adds \IdealAutomate\yourscriptname</para>
        /// <para>to it. By combining that path to the file name created from the key,</para>
        /// <para>it can retrieve a value from the key that is unique to your script application.</para>
        ///  <para>The AppDirectory allows you to store personal settings and</para>
        /// <para>information that you want to keep private (like passwords) in a location</para>
        /// <para>outside of your script on in the application directory</para>
        /// </summary>
        /// <param name="strKey">Unique key within the script application</param>
        /// <returns>string that was in application directory for that key</returns>
        /// Category::Storage>Private
        public string GetValueByKeyGlobalRespondWithDialogIfEmpty(string strKey) {
            string fileName = strKey + ".txt";
            StreamReader file = null;
            string strValueRead = "";
            string settingsDirectory = GetAppDirectoryForIdealAutomate();
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            } else {
                List<ControlEntity> myListControlEntity = new List<ControlEntity>();

                ControlEntity myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = strKey + " not found";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());


                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "myLabel";
                myControlEntity.Text = strKey + "Value";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());


                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "myTextBox";
                myControlEntity.Text = "";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());



                this.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);

                strValueRead = myListControlEntity.Find(x => x.ID == "myTextBox").Text;
                SetValueByKeyGlobal(strKey, strValueRead);
            }
            return strValueRead;
        }

        /// <summary>
        /// <para>SetValueByKeyGlobal takes a key and adds .txt to it in order to create</para>
        /// <para>a file name. It gets the app data path and adds \IdealAutomate\yourscriptname</para>
        /// <para>to it. By combining that path to the file name,</para>
        /// <para>it can write a value to the key that is unique to your script application.</para>
        /// <para>The AppDirectory allows you to store personal settings and</para>
        /// <para>information that you want to keep private (like passwords) in a location</para>
        /// <para>outside of your script on in the application directory</para>
        /// </summary>
        /// <param name="strKey">Unique key within the script application</param>
        /// <param name="strValueToWrite">Value to write to the Unique key 
        /// within the script application</param>
        /// Category::Storage>Private
        public void SetValueByKeyGlobal(string strKey, string strValueToWrite) {
            string fileName = strKey + ".txt";
            StreamWriter writer = null;
            string settingsDirectory = GetAppDirectoryForIdealAutomate();
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            // Hook a write to the text file.
            writer = new StreamWriter(settingsPath);
            // Rewrite the entire value of s to the file
            writer.Write(strValueToWrite);
            writer.Close();
        }

        /// <summary>
        /// <para>ReadAppDirectoryKeyToArrayList takes a key and adds .txt to it in order to create</para>
        /// <para>a file name. It gets the app data path and adds \IdealAutomate\yourscriptname</para>
        /// <para>to it. By combining that path to the file name created from the key,</para>
        /// <para>it can retrieve an arraylist that is unique to your script application.</para>
        /// <para>The AppDirectory allows you to store personal settings and</para>
        /// <para>information that you want to keep private (like passwords) in a location</para>
        /// <para>outside of your script on in the application directory</para>
        /// </summary>
        /// <param name="strKey">Unique key within the script application</param>
        /// <returns>ArrayList that was in application directory for that key</returns>
        /// Category::Storage>Private
        public ArrayList ReadAppDirectoryKeyToArrayList(string strKey) {
            string fileName = strKey + ".txt";
            ArrayList myArrayList = new ArrayList();
            string settingsDirectory = GetAppDirectoryForScript();
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                try {
                    StreamReader reader = File.OpenText(settingsPath);
                    while (!reader.EndOfStream) {
                        string myLine = reader.ReadLine();
                        myArrayList.Add(myLine);
                    }
                    reader.Close();
                } catch (Exception ex) {

                    string message = "Error - Reading  " + fileName + " " + ex.Message + " " + ex.InnerException; // +"; EntityName is: " + myEntityForExecutable.EntityName;
                    MessageBox.Show(message);
                }
            }
            return myArrayList;
        }

        /// <summary>
        /// <para>ReadPublicKeyToArrayList takes a key and adds .txt to it in order to create</para>
        /// <para>a file name. It gets the parent folder of the full filename that is passed in</para>
        /// <para>to it. It creates a folder with the path for the fullfilename in ..\IdealAutomate folder,</para>
        /// </summary>
        /// <param name="strKey">Unique key within the script application</param>
        /// <returns>ArrayList that was in parent folder ..IdealAutomate for that key</returns>
        /// Category::Storage>Public
        public ArrayList ReadPublicKeyToArrayList(string strKey, string strFullFileName) {
            ArrayList myArrayList = new ArrayList();
            string strValueRead = "";
            string fileName = strKey + ".txt";
            StreamReader file = null;
            string settingsDirectory = "";
            string fileNamePartOfFullPath = "";
            bool isDirectory = false;
            if (File.Exists(strFullFileName)) {

                // path is a file.
                settingsDirectory = ConvertFullFileNameToPublicPath(strFullFileName);
                fileNamePartOfFullPath = Path.GetFileName(strFullFileName);

            } else if (Directory.Exists(strFullFileName)) {

                // path is a directory.
                settingsDirectory = strFullFileName;
                isDirectory = true;

            } else {
                // file does not exist yet...so we make assumption that it is a file if it contains period
                if (strFullFileName.Contains(".")) {
                    // path is a file.
                    settingsDirectory = ConvertFullFileNameToPublicPath(strFullFileName);
                    fileNamePartOfFullPath = Path.GetFileName(strFullFileName);
                } else {
                    // path is a directory.
                    settingsDirectory = strFullFileName;
                    isDirectory = true;
                }


            }

            settingsDirectory = Path.Combine(settingsDirectory, "..IdealAutomate");
            if (!isDirectory) {
                if (fileNamePartOfFullPath.Contains(".")) {
                    settingsDirectory = Path.Combine(settingsDirectory, fileNamePartOfFullPath.Substring(0, fileNamePartOfFullPath.IndexOf(".")));
                } else {
                    settingsDirectory = Path.Combine(settingsDirectory, fileNamePartOfFullPath);
                }
            }
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                try {
                    StreamReader reader = File.OpenText(settingsPath);
                    while (!reader.EndOfStream) {
                        string myLine = reader.ReadLine();
                        myArrayList.Add(myLine);
                    }
                    reader.Close();
                } catch (Exception ex) {

                    string message = "Error - Reading  " + fileName + " " + ex.Message + " " + ex.InnerException; // +"; EntityName is: " + myEntityForExecutable.EntityName;
                    MessageBox.Show(message);
                }
            }
            return myArrayList;
        }

        /// <summary>
        /// <para>WriteArrayListToAppDirectoryKey takes a key and adds .txt to it in order to create</para>
        /// <para>a file name. It gets the app data path and adds \IdealAutomate\yourscriptname</para>
        /// <para>to it. By combining that path to the file name created from the key,</para>
        /// <para>it can write an arraylist to the key filename is unique to your script application.</para>
        /// <para>The AppDirectory allows you to store personal settings and</para>
        /// <para>information that you want to keep private (like passwords) in a location</para>
        /// <para>outside of your script on in the application directory</para>
        /// </summary>
        /// <param name="strKey">Unique key within the script application</param>
        /// <param name="arrayListToWrite">ArrayList that is to written to the application directory for that key</param>
        /// Category::Storage>Private
        public void WriteArrayListToAppDirectoryKey(string strKey, ArrayList arrayListToWrite) {
            string fileName = strKey + ".txt";
            StreamWriter writer = null;
            string settingsDirectory = GetAppDirectoryForScript();
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            // Hook a write to the text file.
            writer = new StreamWriter(settingsPath);
            // Rewrite the entire value of s to the file
            foreach (var item in arrayListToWrite) {
                writer.WriteLine(item.ToString());
            }
            writer.Close();
        }
        /// Category::Storage>Public
        public void WriteArrayListToPublicKey(string strKey, ArrayList arrayListToWrite, string strFullFileName) {
            string fileName = strKey + ".txt";
            StreamWriter writer = null;
            string settingsDirectory = "";
            string fileNamePartOfFullPath = "";
            bool isDirectory = false;
            if (File.Exists(strFullFileName)) {

                // path is a file.
                settingsDirectory = ConvertFullFileNameToPublicPath(strFullFileName);
                fileNamePartOfFullPath = Path.GetFileName(strFullFileName);

            } else if (Directory.Exists(strFullFileName)) {

                // path is a directory.
                settingsDirectory = strFullFileName;
                isDirectory = true;

            } else {
                // file does not exist yet...so we make assumption that it is a file if it contains period
                if (strFullFileName.Contains(".")) {
                    // path is a file.
                    settingsDirectory = ConvertFullFileNameToPublicPath(strFullFileName);
                    fileNamePartOfFullPath = Path.GetFileName(strFullFileName);
                } else {
                    // path is a directory.
                    settingsDirectory = strFullFileName;
                    isDirectory = true;
                }


            }

            settingsDirectory = Path.Combine(settingsDirectory, "..IdealAutomate");
            if (!Directory.Exists(settingsDirectory)) {
                Directory.CreateDirectory(settingsDirectory);
            }
            if (!isDirectory) {
                if (fileNamePartOfFullPath.Contains(".")) {
                    settingsDirectory = Path.Combine(settingsDirectory, fileNamePartOfFullPath.Substring(0, fileNamePartOfFullPath.IndexOf(".")));
                } else {
                    settingsDirectory = Path.Combine(settingsDirectory, fileNamePartOfFullPath);
                }
                if (!Directory.Exists(settingsDirectory)) {
                    Directory.CreateDirectory(settingsDirectory);
                }
            }
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            // Hook a write to the text file.
            writer = new StreamWriter(settingsPath);
            // Rewrite the entire value of s to the file
            foreach (var item in arrayListToWrite) {
                writer.WriteLine(item.ToString());
            }
            writer.Close();
        }

        /// <summary>
        /// <para>ReadAppDirectoryKeyToArrayList takes a key and adds .txt to it in order to create</para>
        /// <para>a file name. It gets the app data path and adds \IdealAutomate\yourscriptname</para>
        /// <para>to it. By combining that path to the file name created from the key,</para>
        /// <para>it can retrieve an arraylist that is unique to your script application.</para>
        /// <para>The AppDirectory allows you to store personal settings and</para>
        /// <para>information that you want to keep private (like passwords) in a location</para>
        /// <para>outside of your script on in the application directory</para>
        /// </summary>
        /// <param name="strKey">Unique key within the script application</param>
        /// <returns>ArrayList that was in application directory for that key</returns>
        /// Category::Storage>Private
        public ArrayList ReadAppDirectoryKeyToArrayListGlobal(string strKey) {
            string fileName = strKey + ".txt";
            ArrayList myArrayList = new ArrayList();
            string settingsDirectory = GetAppDirectoryForIdealAutomate();
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                try {
                    StreamReader reader = File.OpenText(settingsPath);
                    while (!reader.EndOfStream) {
                        string myLine = reader.ReadLine();
                        myArrayList.Add(myLine);
                    }
                    reader.Close();
                } catch (Exception ex) {

                    string message = "Error - Reading  " + fileName + " " + ex.Message + " " + ex.InnerException; // +"; EntityName is: " + myEntityForExecutable.EntityName;
                    MessageBox.Show(message);
                }
            }
            return myArrayList;
        }

        /// <summary>
        /// <para>WriteArrayListToAppDirectoryKey takes a key and adds .txt to it in order to create</para>
        /// <para>a file name. It gets the app data path and adds \IdealAutomate\yourscriptname</para>
        /// <para>to it. By combining that path to the file name created from the key,</para>
        /// <para>it can write an arraylist to the key filename is unique to your script application.</para>
        /// <para>The AppDirectory allows you to store personal settings and</para>
        /// <para>information that you want to keep private (like passwords) in a location</para>
        /// <para>outside of your script on in the application directory</para>
        /// </summary>
        /// <param name="strKey">Unique key within the script application</param>
        /// <param name="arrayListToWrite">ArrayList that is to written to the application directory for that key</param>
        /// Category::Storage>Private
        public void WriteArrayListToAppDirectoryKeyGlobal(string strKey, ArrayList arrayListToWrite) {
            string fileName = strKey + ".txt";
            StreamWriter writer = null;
            string settingsDirectory = GetAppDirectoryForIdealAutomate();
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            // Hook a write to the text file.
            writer = new StreamWriter(settingsPath);
            // Rewrite the entire value of s to the file
            foreach (var item in arrayListToWrite) {
                writer.WriteLine(item.ToString());
            }
            writer.Close();
        }
        /// <summary>
        /// At beginning of template, Write StartTime, StartDate, Add 1 to total executions. 
        /// 
        /// </summary>
        /// Category::Stats
        public void ScriptStartedUpdateStats() {
            SetValueByKey("ScriptStartDateTime", System.DateTime.Now.ToString());
            IncrementValueByKeyByValue("ScriptTotalExecutions", 1);

        }

        /// <summary>
        /// ScriptEndedSuccessfullyUpdateStats - At end of template, Subtract start time and start date
        /// from Current date and time, add 1 to successful executions. 
        /// (avg successful execution time * 
        /// (total successful executions - 1)) + current successful execution time) / total successful executions. 
        /// Write PercentSuccessful = (SuccessfulExecutions / TotalExecutions) * 100. 
        /// CurrentSavedExecutionTime = CurrentExecutionTime - ManualExecutionTime.
        /// ScriptTotalSavedExecutionTime += CurrentSavedExecutionTime
        ///  StartDate is the LastExecutedDate.
        /// </summary>
        /// Category::Stats
        public void ScriptEndedSuccessfullyUpdateStats() {
            DateTime dtScriptStartDateTime = GetValueByKeyAsDateTime("ScriptStartDateTime");


            if (dtScriptStartDateTime == DateTime.MinValue) {
                return;
            }

            TimeSpan ts = System.DateTime.Now - dtScriptStartDateTime;

            int intAvgSuccessfulExecutionTime = GetValueByKeyAsInt("AvgSuccessfulExecutionTime");
            int intScriptSuccessfulExecutions = GetValueByKeyAsInt("ScriptSuccessfulExecutions");
            int intTotalSuccessfulExecutionTimePrev = intAvgSuccessfulExecutionTime *
                (intScriptSuccessfulExecutions);
            int intTotalSuccessfulExecutionTime = intTotalSuccessfulExecutionTimePrev + (int)ts.TotalSeconds;
            intScriptSuccessfulExecutions = IncrementValueByKeyByValue("ScriptSuccessfulExecutions", 1);
            intAvgSuccessfulExecutionTime = intTotalSuccessfulExecutionTime / intScriptSuccessfulExecutions;
            SetValueByKey("AvgSuccessfulExecutionTime", intAvgSuccessfulExecutionTime.ToString());
            int intScriptTotalExecutions = GetValueByKeyAsInt("ScriptTotalExecutions");
            decimal decPercentSuccessful = (intScriptSuccessfulExecutions / intScriptTotalExecutions) * 100;
            int intPercentSuccessful = (int)Math.Round(decPercentSuccessful);
            SetValueByKey("ScriptPercentSuccessful", intPercentSuccessful.ToString());
            int intManualExecutionTime = GetValueByKeyAsInt("ManualExecutionTime");
            int intScriptCurrentExecutionTimeSavings = 0;
            if (intManualExecutionTime != 0) {
                intScriptCurrentExecutionTimeSavings = intManualExecutionTime - intTotalSuccessfulExecutionTime;
            }

            IncrementValueByKeyByValue("ScriptTotalSavedExecutionTime", intScriptCurrentExecutionTimeSavings);




        }
        /// Category::Directory
        public string ConvertFullFileNameToScriptPath(string fullFileName) {
            int intIndex = fullFileName.LastIndexOf(@"\");
            if (intIndex > -1) {
                fullFileName = fullFileName.Substring(0, intIndex);
            }
            string scriptPath = fullFileName;
            scriptPath = scriptPath.Replace(":", "+").Replace(@"\", "-");
            return scriptPath;
        }
        /// Category::Directory
        public string ConvertFullFileNameToPublicPath(string fullFileName) {
            int intIndex = fullFileName.LastIndexOf(@"\");
            if (intIndex > -1) {
                fullFileName = fullFileName.Substring(0, intIndex);
            }
            string scriptPath = fullFileName;
            return scriptPath;
        }
        /// Category::Directory
        public string ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(string fullFileName) {
            string scriptPath = fullFileName;
            scriptPath = scriptPath.Replace(":", "+").Replace(@"\", "-");
            return scriptPath;
        }
        /// Category::Directory
        public string GetPathForScriptNoBinDebug() {
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            directory = directory.Replace("\\bin\\Debug\\", "");
            return directory;
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~FileView() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
