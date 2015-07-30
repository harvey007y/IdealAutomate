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
using System.Windows.Interop;


namespace IdealAutomate.Core {
  public class Methods {
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private const int HOTKEY_ID = 9000;

    //Modifiers:
    private const uint MOD_NONE = 0x0000; //(none)
    private const uint MOD_ALT = 0x0001; //ALT
    private const uint MOD_CONTROL = 0x0002; //CTRL
    private const uint MOD_SHIFT = 0x0004; //SHIFT
    private const uint MOD_WIN = 0x0008; //WINDOWS
    //CAPS LOCK:
    private const uint VK_PAUSE = 0x13;
    private  IntPtr _windowHandle;
    private HwndSource _source;
    private volatile bool boolPausePressed = false;


    private  IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
      const int WM_HOTKEY = 0x0312;
      switch (msg) {
        case WM_HOTKEY:
          switch (wParam.ToInt32()) {
            case HOTKEY_ID:
              int vkey = (((int)lParam >> 16) & 0xFFFF);
           //   System.Diagnostics.Debugger.Break();
              if (vkey == VK_PAUSE) {
                boolPausePressed = true;
                string FileDes = "IdealAutomateScript"; //FileDescription
                var myCurrentProcess = Process.GetCurrentProcess();
                foreach (Process x in Process.GetProcesses()) {
                  try {
                    if (FileDes == x.MainModule.FileVersionInfo.Comments && x.ProcessName != myCurrentProcess.ProcessName) {
                    //  System.Diagnostics.Debugger.Break();
                      x.Kill();
                    }
                  } catch (Exception) {

                    continue;
                  }

                }
            //    System.Diagnostics.Debugger.Break();
                Console.WriteLine("Application Cancelled");
                System.Windows.Forms.MessageBox.Show("IdealAutomateScript Cancelled - " + myCurrentProcess.ProcessName, "Header", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.None,
    System.Windows.Forms.MessageBoxDefaultButton.Button1, (System.Windows.Forms.MessageBoxOptions)0x40000);  // MB_TOPMOST
             
                  _source.RemoveHook(HwndHook);
                  UnregisterHotKey(_windowHandle, HOTKEY_ID);
                  Environment.Exit(0);


                handled = true;
                //     throw new Exception("Application was cancelled!!!");
                break;
                //   System.Diagnostics.Debugger.Break();

                //tblock.Text += "CapsLock was pressed" + Environment.NewLine;
              }
              handled = true;
              break;
          }
          break;
      }
      return IntPtr.Zero;
    }

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
    Window window;
    Process oProcess;
    public Methods() {
      window = new Window() //make sure the window is invisible
 {
   Width = 0,
   Height = 0,
   Left = -2000,
   WindowStyle = WindowStyle.None,
   ShowInTaskbar = false,
   ShowActivated = false,
 };
      window.Show();
      oProcess = Process.GetCurrentProcess();
    }
   
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
      } else {
        MessageBox.Show("Window Not Found!");
        return false;
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
    TimeSpan duration;

    double seconds;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="myImage"></param>
    /// <returns></returns>
    public int[,] PutAll(ImageEntity myImage) {
      RegisterHotKey(window);
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "PutAll:");
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
      while (boolImageFound == false && intAttempts < myImage.ImageAttempts && boolPausePressed == false) {
        ls = Click_PNG(myImage);
        if (ls.Count > 0) {
          boolImageFound = true;
        }
        intAttempts += 1;
      }
      int intRowIndex = 0;
      int[,] myArray = new int[0, 0];

      foreach (var myRow in ls) {
        int[] NewSizes = new int[] { intRowIndex + 1, 2 };
        myArray = (int[,])myArray.ResizeArray(NewSizes);
        myArray[intRowIndex, 0] = myRow.myPoint.X;
        myArray[intRowIndex, 1] = myRow.myPoint.Y;
        //myListObject[2] = myRow.percentcorrect;
        //   myListListObject.Add(myListObject);
        intRowIndex++;

      }

      _source.RemoveHook(HwndHook);
      UnregisterHotKey(_windowHandle, HOTKEY_ID);
      return myArray;

    }
    public int[,] PutCursorPosition() {
      RegisterHotKey(window);
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "PutCursorPosition");
      }
      int[,] myArray = new int[1, 2];
      myArray[0, 0] = System.Windows.Forms.Cursor.Position.X;
      myArray[0, 1] = System.Windows.Forms.Cursor.Position.Y;
      _source.RemoveHook(HwndHook);
      UnregisterHotKey(_windowHandle, HOTKEY_ID);
      return myArray;
    }
    public int[,] PutCaretPositionInArray() {
      RegisterHotKey(window);
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "PutCaretPositionInArray");
      }
      int[,] myArray = new int[1, 2];
      string activeProcess = GetActiveProcess();
      if (activeProcess == string.Empty) {
        MessageBox.Show("No active window found");
      }

      // If window explorer is active window (eg. user has opened any drive)
      // Or for any failure when activeProcess is nothing               

      // Otherwise Calculate Caret position
      EvaluateCaretPosition();

      myArray[0, 0] = caretPosition.X;
      myArray[0, 1] = caretPosition.Y;
      _source.RemoveHook(HwndHook);
      UnregisterHotKey(_windowHandle, HOTKEY_ID);
      return myArray;


    }
    public void ClickImageIfExists(ImageEntity myImage) {
      RegisterHotKey(window);
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "ClickImageIfExists:");
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
      while (boolImageFound == false && intAttempts < myImage.ImageAttempts) {
        ls = Click_PNG(myImage);
        if (ls.Count > 0) {
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
      _source.RemoveHook(HwndHook);
      UnregisterHotKey(_windowHandle, HOTKEY_ID);
    }

    public void LeftClick(int[,] myArray) {
      RegisterHotKey(window);
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "LeftClick:");
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
      _source.RemoveHook(HwndHook);
      UnregisterHotKey(_windowHandle, HOTKEY_ID);
    }
    public void ShiftClick(int[,] myArray) {
      RegisterHotKey(window);
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "ShiftClick:");
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
      _source.RemoveHook(HwndHook);
      UnregisterHotKey(_windowHandle, HOTKEY_ID);
    }
    public void RightClick(int[,] myArray) {
      RegisterHotKey(window);
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "RightClick:");
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
      _source.RemoveHook(HwndHook);
      UnregisterHotKey(_windowHandle, HOTKEY_ID);
    }
    public void PositionCursor(int[,] myArray) {
      RegisterHotKey(window);
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "PositionCursor:");
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
      _source.RemoveHook(HwndHook);
      UnregisterHotKey(_windowHandle, HOTKEY_ID);
    }
    public string PutClipboardInEntity() {
      RegisterHotKey(window);
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "PutClipboardInEntity: ");
      }
      string myEntity = "";
      try {
        Thread thread = new Thread(new ThreadStart(() => {
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
      } catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }
      if (myEntity.Length > 5000) {
        Console.Write("PutEntityInClipboard: myEntity more than 5000 in length");
      } else {
        Console.Write("myEntity=" + myEntity);
      }
      _source.RemoveHook(HwndHook);
      UnregisterHotKey(_windowHandle, HOTKEY_ID);
      return myEntity;
    }
    public string PutWindowTitleInEntity() {
      RegisterHotKey(window);
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "PutWindowTitleInEntity");
      }
      string myEntity = "";
      try {
        myEntity = GetActiveProcessTitle();
        // or call logic here
      } catch (Exception ex) {

        MessageBox.Show(ex.Message);
      }
      _source.RemoveHook(HwndHook);
      UnregisterHotKey(_windowHandle, HOTKEY_ID);
      return myEntity;
    }
   
    public void PutEntityInClipboard(string myEntity) {
      RegisterHotKey(window);
      if (fbDebugMode) {
        if (myEntity.Length > 5000) {
          Console.WriteLine("PutEntityInClipboard: myEntity more than 5000 in length");
        } else {
          Console.WriteLine("PutEntityInClipboard: myEntity=" + myEntity);
        }
      }
      try {

        Thread thread = new Thread(new ThreadStart(() => {

          Clipboard.SetData(DataFormats.Text, (Object)myEntity);
          // or call logic here



        }));

        thread.SetApartmentState(ApartmentState.STA);

        thread.Start();
        thread.Join();
      } catch (Exception ex) {
        MessageBox.Show(ex.Message);
      }

      _source.RemoveHook(HwndHook);
      UnregisterHotKey(_windowHandle, HOTKEY_ID);
    }
    public void TypeText(string myEntity, int intSleep) {
      RegisterHotKey(window);
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "TypeText: myEntity=" + myEntity + " intSleep=" + intSleep.ToString());
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
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        return;
      }
      if (myEntity == "%(\" \")n") {
        InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.SPACE); //System.Windows.Forms.Keys.Alt);
        InputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_N);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        return;
      }
      if (myEntity == "%(\" \")") {
        InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.SPACE); //System.Windows.Forms.Keys.Alt);              
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        return;
      }
      if (myEntity == "%(\" \")x") {
        InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.SPACE); //System.Windows.Forms.Keys.Alt);
        InputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_X);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        return;
      }
      if (myEntity == "%({F8})") {
        InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.F8); //System.Windows.Forms.Keys.Alt);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        return;
      }
      if (myEntity == "{NUMPADADD}") {
        InputSimulator.Keyboard.KeyPress(VirtualKeyCode.ADD);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        return;
      }
      if (myEntity == "{NUMPADMULT}") {
        InputSimulator.Keyboard.KeyPress(VirtualKeyCode.MULTIPLY);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        return;
      }
      if (myEntity == "{ENTER}") {
        InputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        return;
      }
      if (myEntity == "^(n)") {
        InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_N);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        return;
      }
      if (myEntity == "^(v)") {
        InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        return;
      }
      if (myEntity == "^(c)") {
        InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        return;
      }
      if (myEntity == "{F5}") {
        InputSimulator.Keyboard.KeyPress(VirtualKeyCode.F5);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        return;
      }
      if (myEntity == "{F6}") {
        InputSimulator.Keyboard.KeyPress(VirtualKeyCode.F6);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        return;
      }
      if (myEntity == "{DOWN}") {
        InputSimulator.Keyboard.KeyPress(VirtualKeyCode.DOWN);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        return;
      }
      if (myEntity == "{UP}") {
        InputSimulator.Keyboard.KeyPress(VirtualKeyCode.UP);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        return;
      }
      if (myEntity == "^({END})") {
        InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.END);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID); 
        return;
      }
      if (myEntity == "^({HOME})") {
        InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.HOME);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        return;
      }
      if (myEntity == "+({F10})") {
        InputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.F10);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
        return;
      }
      if (myEntity == "{LWin}") {
        KeyboardSend.KeyDown(System.Windows.Forms.Keys.LWin);
        KeyboardSend.KeyDown(System.Windows.Forms.Keys.Shift);
        KeyboardSend.KeyDown(System.Windows.Forms.Keys.Right);
        KeyboardSend.KeyUp(System.Windows.Forms.Keys.LWin);
        KeyboardSend.KeyUp(System.Windows.Forms.Keys.D4);
        KeyboardSend.KeyUp(System.Windows.Forms.Keys.Right);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
      } else {

        System.Windows.Forms.SendKeys.SendWait(myEntity);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);

      }
    }
    public void MessageBoxShow(string myEntity) {
      RegisterHotKey(window);
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "MessageBoxShow: myEntity=" + myEntity);
      }
      System.Windows.Forms.MessageBox.Show(myEntity, "Header", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.None,
    System.Windows.Forms.MessageBoxDefaultButton.Button1, (System.Windows.Forms.MessageBoxOptions)0x40000);  // MB_TOPMOST
      _source.RemoveHook(HwndHook);
      UnregisterHotKey(_windowHandle, HOTKEY_ID);
    }
    public void Run(string myEntityForExecutable, string myEntityForContent) {
      RegisterHotKey(window);
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "Run: myEntityForExecutable=" + myEntityForExecutable + " myEntityForContent=" + myEntityForContent);
      }

      if (myEntityForExecutable == null) {
        string message = "Error - You need to specify executable primitive  "; // +"; EntityName is: " + myEntityForExecutable.EntityName;
        MessageBoxResult result = MessageBox.Show(message, "Run-time Error", MessageBoxButton.OK, MessageBoxImage.Error);
        _source.RemoveHook(HwndHook);
        UnregisterHotKey(_windowHandle, HOTKEY_ID);
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
      _source.RemoveHook(HwndHook);
      UnregisterHotKey(_windowHandle, HOTKEY_ID);
    }
    public void Sleep(int intSleep) {
      RegisterHotKey(window);
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "Sleep:  intSleep=" + intSleep.ToString());
      }
      System.Threading.Thread.Sleep(intSleep);
      _source.RemoveHook(HwndHook);
      UnregisterHotKey(_windowHandle, HOTKEY_ID);
    }
    public void RegisterHotKey(Window window) {
      _windowHandle = new WindowInteropHelper(window).Handle;
      _source = HwndSource.FromHwnd(_windowHandle);
      _source.AddHook(HwndHook);
      //System.Diagnostics.Debugger.Break();

      RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_NONE, VK_PAUSE); //CTRL + CAPS_LOCK
    }
    public static void CloseApp() {
      //       System.Diagnostics.Debugger.Break();
      //_source.RemoveHook(HwndHook);
      //UnregisterHotKey(_windowHandle, HOTKEY_ID);
      Application.Current.Shutdown();
    }

    private List<SubPositionInfo> Click_PNG(ImageEntity myImage) {
      if (fbDebugMode) {
        Console.WriteLine(oProcess.ProcessName + "==> " + "Click_PNG:");
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
      Console.WriteLine(oProcess.ProcessName + "==> " + "Searching..." + myImage.ImageFile + Environment.NewLine);

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
      if (ls.Count > 0) {



        for (int i = 0; i < ls.Count; i++) {
          System.Drawing.Point p = ls[i].myPoint;

          string myfile = "tempsmall" + strfilename + i.ToString() + ".bmp";

          System.IO.File.Delete(directory + myfile);
          bm.Save(directory + myfile, System.Drawing.Imaging.ImageFormat.Bmp);
          myfile = "tempbig" + strfilename + i.ToString() + ".bmp";
          System.IO.File.Delete(directory + myfile);
          bmx.Save(directory + myfile, System.Drawing.Imaging.ImageFormat.Bmp);

          Console.WriteLine(oProcess.ProcessName + "==> " + "Image found at: " + p.ToString() + strfilename + i.ToString() + " highestPercentCorrect=" + ls[i].percentcorrect.ToString() +
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
      } else {

        string myfile = "tempsmall" + strfilename + ".bmp";

        System.IO.File.Delete(directory + myfile);
        bm.Save(directory + myfile, System.Drawing.Imaging.ImageFormat.Bmp);
        myfile = "tempbig" + strfilename + ".bmp";
        System.IO.File.Delete(directory + myfile);
        bmx.Save(directory + myfile, System.Drawing.Imaging.ImageFormat.Bmp);

        myfile = "tempbig" + strfilename + ".bmp";

        Console.WriteLine(oProcess.ProcessName + "==> " + "Image not found" + strfilename + " highestPercentCorrect=" + highestPercentCorrect.ToString() + Environment.NewLine);

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
      // If Active window has some title info
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

  }
}
