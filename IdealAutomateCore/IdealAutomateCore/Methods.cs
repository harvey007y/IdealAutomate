using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media;
using System.Threading;
using System.Text;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.Management;
//using WindowsInput;
using System.Data;
using System.Data.SqlClient;
using WindowsInput;
using WindowsInput.Native;

namespace IdealAutomate.Core
{
    public class Methods
    {
      
      private bool fbDebugMode = false;

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

        [DllImport("user32.dll")]
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

      public static bool ActivateWindowByTitle(string myTitle) {

                  //Find the window, using the CORRECT Window Title, for example, Notepad
            int hWnd = FindWindow(null, myTitle);
            if (hWnd > 0) //If found
            {
              {
                ShowWindowAsync((IntPtr)hWnd, SW_RESTORE);
              }
                SetForegroundWindow(hWnd); //Activate it              
                return true;
            }
            else
            {
                MessageBox.Show("Window Not Found!");
                return false;
            }

}
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
        public DateTime dtStartDateTime = System.DateTime.Now;
        
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myImage"></param>
        /// <returns></returns>
        public int[,] PutAll(ImageEntity myImage)
        {
          if (fbDebugMode) {
            Console.WriteLine("PutAll:");
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(myImage)) {
              string name = descriptor.Name;
              object value = descriptor.GetValue(myImage);
              Console.WriteLine("{0}={1}", name, value);
            }
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
            while (boolImageFound == false && intAttempts < myImage.ImageAttempts)
            {
                ls = Click_PNG(myImage);
                if (ls.Count > 0)
                {
                    boolImageFound = true;
                }
                intAttempts += 1;
            }
            int intRowIndex = 0;
            int[,] myArray = new int[0, 0];

            foreach (var myRow in ls)
            {
                int[] NewSizes = new int[] { intRowIndex + 1, 2 };
                myArray = (int[,])myArray.ResizeArray(NewSizes);
                myArray[intRowIndex, 0] = myRow.myPoint.X;
                myArray[intRowIndex, 1] = myRow.myPoint.Y;
                //myListObject[2] = myRow.percentcorrect;
                //   myListListObject.Add(myListObject);
                intRowIndex++;

            }


            return myArray;

        }
        public int[,] PutCursorPosition()
        {
          if (fbDebugMode) {
            Console.WriteLine("PutCursorPosition");
          }
            int[,] myArray = new int[1, 2];
            myArray[0, 0] = System.Windows.Forms.Cursor.Position.X;
            myArray[0, 1] = System.Windows.Forms.Cursor.Position.Y;
            return myArray;
        }
        public int[,] PutCaretPositionInArray()
        {
          if (fbDebugMode) {
            Console.WriteLine("PutCaretPositionInArray");
          }
            int[,] myArray = new int[1, 2];
            string activeProcess = GetActiveProcess();
            if (activeProcess == string.Empty)
            {
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
        public void ClickImageIfExists(ImageEntity myImage)
        {
          if (fbDebugMode) {
            Console.WriteLine("ClickImageIfExists:");
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(myImage)) {
              string name = descriptor.Name;
              object value = descriptor.GetValue(myImage);
              Console.WriteLine("{0}={1}", name, value);
            }
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
            while (boolImageFound == false && intAttempts < myImage.ImageAttempts)
            {
                ls = Click_PNG(myImage);
                if (ls.Count > 0)
                {
                    boolImageFound = true;
                    System.Drawing.Point p = ls[0].myPoint;
                    Position_Cursor.MoveMouse(p.X, p.Y);
                    UInt32 myX = Convert.ToUInt32(p.X);
                    UInt32 myY = Convert.ToUInt32(p.Y);
                    Position_Cursor.DoMouseClick(myX, myY);
                    break;
                }
                intAttempts += 1;
            }
        }
      
        public void LeftClick(int[,] myArray)
        {
          if (fbDebugMode) {
            Console.WriteLine("LeftClick:");
            int bound0 = myArray.GetUpperBound(0);
            int bound1 = myArray.GetUpperBound(1);
            // ... Loop over bounds.
            for (int i = 0; i <= bound0; i++) {
              for (int x = 0; x <= bound1; x++) {
                // Display the element at these indexes.
                Console.WriteLine(myArray[i, x].ToString());
              }
              Console.WriteLine();
            }
          }
            int RelX = myArray[0, 0];
            int RelY = myArray[0, 1];
            Position_Cursor.MoveMouse(RelX, RelY);
            UInt32 myX1 = Convert.ToUInt32(RelX);
            UInt32 myY1 = Convert.ToUInt32(RelY);
            Position_Cursor.DoMouseClick(myX1, myY1);
        }
        public void ShiftClick(int[,] myArray)
        {
          if (fbDebugMode) {
            Console.WriteLine("ShiftClick:");
            int bound0 = myArray.GetUpperBound(0);
            int bound1 = myArray.GetUpperBound(1);
            // ... Loop over bounds.
            for (int i = 0; i <= bound0; i++) {
              for (int x = 0; x <= bound1; x++) {
                // Display the element at these indexes.
                Console.WriteLine(myArray[i, x].ToString());
              }
              Console.WriteLine();
            }
          }
            int RelX = myArray[0, 0];
            int RelY = myArray[0, 1];
            Position_Cursor.MoveMouse(RelX, RelY);
            UInt32 myX1 = Convert.ToUInt32(RelX);
            UInt32 myY1 = Convert.ToUInt32(RelY);
            Position_Cursor.DoMouseShiftClick(myX1, myY1);
        }
        public void RightClick(int[,] myArray)
        {
          if (fbDebugMode) {
            Console.WriteLine("RightClick:");
            int bound0 = myArray.GetUpperBound(0);
            int bound1 = myArray.GetUpperBound(1);
            // ... Loop over bounds.
            for (int i = 0; i <= bound0; i++) {
              for (int x = 0; x <= bound1; x++) {
                // Display the element at these indexes.
                Console.WriteLine(myArray[i, x].ToString());
              }
              Console.WriteLine();
            }
          }
            int RelX = myArray[0, 0];
            int RelY = myArray[0, 1];
            Position_Cursor.MoveMouse(RelX, RelY);
            UInt32 myX1 = Convert.ToUInt32(RelX);
            UInt32 myY1 = Convert.ToUInt32(RelY);
            Position_Cursor.DoMouseRightClick(myX1, myY1);
        }
        public void PositionCursor(int[,] myArray)
        {
          if (fbDebugMode) {
            Console.WriteLine("PositionCursor:");
            int bound0 = myArray.GetUpperBound(0);
            int bound1 = myArray.GetUpperBound(1);
            // ... Loop over bounds.
            for (int i = 0; i <= bound0; i++) {
              for (int x = 0; x <= bound1; x++) {
                // Display the element at these indexes.
                Console.WriteLine(myArray[i, x].ToString());
              }
              Console.WriteLine();
            }
          }
            Position_Cursor.MoveMouse(myArray[0, 0], myArray[0, 1]);
        }
        public string PutClipboardInEntity()
        {
          if (fbDebugMode) {
            Console.WriteLine("PutClipboardInEntity: ");            
          }
            string myEntity = "";
            try
            {
                Thread thread = new Thread(new ThreadStart(() =>
                {
                  try {
                    myEntity = Clipboard.GetData(DataFormats.Text).ToString();
                  } catch (Exception) {
                    Console.WriteLine("Exception occurred in PutClipboardInEntity!!!!");  
                    myEntity = "";
                  }
                    
                    // or call logic here



                }));

                thread.SetApartmentState(ApartmentState.STA);

                thread.Start();
                thread.Join();
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
            }
            if (myEntity.Length > 5000) {
              Console.Write("PutEntityInClipboard: myEntity more than 5000 in length");
            } else {
              Console.Write("myEntity=" + myEntity);
            }
            return myEntity;
        }
        public string PutWindowTitleInEntity()
        {
          if (fbDebugMode) {
            Console.WriteLine("PutWindowTitleInEntity");
          }
            string myEntity = "";
            try
            {
                myEntity = GetActiveProcessTitle();
                // or call logic here
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            return myEntity;
        }
        public void PutEntityInClipboard(string myEntity)
        {
          if (fbDebugMode) {
            if (myEntity.Length > 5000) {
              Console.WriteLine("PutEntityInClipboard: myEntity more than 5000 in length");
            } else {
              Console.WriteLine("PutEntityInClipboard: myEntity=" + myEntity);
            }
          }
            try
            {

                Thread thread = new Thread(new ThreadStart(() =>
                {

                    Clipboard.SetData(DataFormats.Text, (Object)myEntity);
                    // or call logic here



                }));

                thread.SetApartmentState(ApartmentState.STA);

                thread.Start();
                thread.Join();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        public void TypeText(string myEntity, int intSleep)
        {
          if (fbDebugMode) {
            Console.WriteLine("TypeText: myEntity=" + myEntity + " intSleep=" + intSleep.ToString());
          }
          InputSimulator InputSimulator = new InputSimulator();
          //if (myEntity == "{LWin}") {
          //  KeyboardSend.KeyDown(System.Windows.Forms.Keys.ControlKey);
          //  KeyboardSend.KeyDown(System.Windows.Forms.Keys.Alt);
          //  KeyboardSend.KeyDown(System.Windows.Forms.Keys.L);
          //  KeyboardSend.KeyUp(System.Windows.Forms.Keys.L);
          //  KeyboardSend.KeyUp(System.Windows.Forms.Keys.Alt);
          //  KeyboardSend.KeyUp(System.Windows.Forms.Keys.ControlKey);
          //}
            if (intSleep > 0)
            {
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
            if (myEntity == "%(\" \")x") {
              InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.SPACE); //System.Windows.Forms.Keys.Alt);
              InputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_N);
              return;
            }
            if (myEntity == "%({F8})") {
              InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.F8); //System.Windows.Forms.Keys.Alt);
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
            if (myEntity == "{LWin}")
            {
                KeyboardSend.KeyDown(System.Windows.Forms.Keys.LWin);
                KeyboardSend.KeyDown(System.Windows.Forms.Keys.Shift);
                KeyboardSend.KeyDown(System.Windows.Forms.Keys.Right);
                KeyboardSend.KeyUp(System.Windows.Forms.Keys.LWin);
                KeyboardSend.KeyUp(System.Windows.Forms.Keys.D4);
                KeyboardSend.KeyUp(System.Windows.Forms.Keys.Right);
            }
            else
            {

                System.Windows.Forms.SendKeys.SendWait(myEntity);

            }
        }
        public void MessageBoxShow(string myEntity)
        {
            if (fbDebugMode)
            {
                Console.WriteLine("MessageBoxShow: myEntity=" + myEntity);
            }
            System.Windows.Forms.MessageBox.Show(myEntity, "Header", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.None,
          System.Windows.Forms.MessageBoxDefaultButton.Button1, (System.Windows.Forms.MessageBoxOptions)0x40000);  // MB_TOPMOST
        }
        public void Run(string myEntityForExecutable, string myEntityForContent)
        {
          if (fbDebugMode) {
            Console.WriteLine("Run: myEntityForExecutable=" + myEntityForExecutable + " myEntityForContent=" + myEntityForContent);
          }
          
            if (myEntityForExecutable == null)
            {
                string message = "Error - You need to specify executable primitive  "; // +"; EntityName is: " + myEntityForExecutable.EntityName;
                MessageBoxResult result = MessageBox.Show(message, "Run-time Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string strExecutable = myEntityForExecutable;

            string strContent = "";
            if (myEntityForContent != null)
            {
                strContent = myEntityForContent;
            }
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
        }
        public void Sleep(int intSleep)
        {
          if (fbDebugMode) {
            Console.WriteLine("Sleep:  intSleep=" + intSleep.ToString());
          }
            System.Threading.Thread.Sleep(intSleep);
        }
        private List<SubPositionInfo> Click_PNG(ImageEntity myImage)
        {
          if (fbDebugMode) {
            Console.WriteLine("Click_PNG:");
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(myImage)) {
              string name = descriptor.Name;
              object value = descriptor.GetValue(myImage);
              Console.WriteLine("{0}={1}", name, value);
            }
          }
            System.Threading.Thread.Sleep(100);


            //Bitmap bm = BytesToBitmap(myEntityForPicture.ImageFile);
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            Bitmap bm = new Bitmap(directory + myImage.ImageFile);
            // Bitmap bm = new Bitmap(directory + pPNG_File_Name);
            //  Bitmap bm = BytesToBitmap(scriptStep.SubImage);
            // Bitmap bm = null;
            //     bm.MakeTransparent(Color.White);


            // previewImageBox.Source = BitmapSourceFromImage(bm);
            //bm;
            // find the least popular color in sub image and find the relative position in
            // subimage
            Hashtable pixels = new Hashtable();

            //resultsTextBox.Text += "Searching..." + scriptStep.Seq1.ToString() + Environment.NewLine;
             Console.WriteLine("Searching..." + myImage.ImageFile + Environment.NewLine);

            bool boolUseGrayScaleDB = false;
            // Find subimages
            Bitmap bmx;

            bmx = Scraper.getDesktopBitmap(boolUseGrayScaleDB); //forcing both images to be the same Scraper.getDesktopBitmap(boolUseGrayScaleDB); // BytesToBitmap(myEntityForPicture.ImageFile); //forcing both images to be the same


            // bigPictureImageBox.Source = BitmapSourceFromImage(bmx);
            decimal highestPercentCorrect = 0;

            List<SubPositionInfo> ls = Scraper.GetSubPositions(bmx, bm, boolUseGrayScaleDB, ref highestPercentCorrect, myImage.ImageTolerance);
            // List<System.Drawing.Point> ls = null;
            int intLastSlashIndex = myImage.ImageFile.LastIndexOf("\\");
            string strfilename = myImage.ImageFile.Substring(intLastSlashIndex + 1);
            if (ls.Count > 0)
            {



                for (int i = 0; i < ls.Count; i++)
                {
                    System.Drawing.Point p = ls[i].myPoint;

                    string myfile = "tempsmall" + strfilename + i.ToString() + ".bmp";

                    System.IO.File.Delete(directory + myfile);
                    bm.Save(directory + myfile, System.Drawing.Imaging.ImageFormat.Bmp);
                    myfile = "tempbig" + strfilename + i.ToString() + ".bmp";
                    System.IO.File.Delete(directory + myfile);
                    bmx.Save(directory + myfile, System.Drawing.Imaging.ImageFormat.Bmp);

                    Console.WriteLine("Image found at: " + p.ToString() + strfilename + i.ToString() + " highestPercentCorrect=" + ls[i].percentcorrect.ToString() +
                                     Environment.NewLine);
                    int intOffX = p.X + myImage.ImageRelativeX;
                    int intOffY = p.Y + myImage.ImageRelativeY;


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
            }
            else
            {

                string myfile = "tempsmall" + strfilename + ".bmp";

                System.IO.File.Delete(directory + myfile);
                bm.Save(directory + myfile, System.Drawing.Imaging.ImageFormat.Bmp);
                myfile = "tempbig" + strfilename + ".bmp";
                System.IO.File.Delete(directory + myfile);
                bmx.Save(directory + myfile, System.Drawing.Imaging.ImageFormat.Bmp);

                myfile = "tempbig" + strfilename + ".bmp";

                Console.WriteLine("Image not found" + strfilename + " highestPercentCorrect=" + highestPercentCorrect.ToString() + Environment.NewLine);

                return ls;
            }
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
         //       MessageBox.Show(caretPosition.X.ToString() + "," + caretPosition.Y.ToString());
            ClientToScreen(guiInfo.hwndCaret, out caretPosition);
         //        MessageBox.Show(caretPosition.X.ToString() + "," + caretPosition.Y.ToString());

            //txtCaretX.Text = (caretPosition.X).ToString();
            //txtCaretY.Text = caretPosition.Y.ToString();

        }

        /// <summary>
        /// Get the caret position
        /// </summary>
        private void GetCaretPosition()
        {
          if (fbDebugMode) {
            Console.WriteLine("GetCaretPosition");
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
             //   MessageBox.Show(Process.GetProcessById((int)lpdwProcessId).MainWindowTitle);
                return Process.GetProcessById((int)lpdwProcessId).MainWindowTitle;
            }
            // Otherwise either error or non client region
            return String.Empty;
        }
    }
}
