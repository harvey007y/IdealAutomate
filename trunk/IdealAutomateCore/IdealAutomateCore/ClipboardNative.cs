using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IdealAutomateCore {
  public static class ClipboardNative {
    [DllImport("user32.dll")]
    private static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll")]
    private static extern bool CloseClipboard();

    [DllImport("user32.dll")]
    private static extern bool SetClipboardData(uint uFormat, IntPtr data);
    
     [DllImport("user32.dll")]
    private static extern bool EmptyClipboard();
    private const uint CF_TEXT = 1;
    private const uint CF_UNICODETEXT = 13;

    public static bool CopyTextToClipboard(string text) {
      if (!OpenClipboard(IntPtr.Zero)) {
        return false;
      }

      object myText = text;
      var global = Marshal.StringToHGlobalUni(myText.ToString());
      EmptyClipboard();
      SetClipboardData(CF_UNICODETEXT, global);
      CloseClipboard();

      //-------------------------------------------
      // Not sure, but it looks like we do not need 
      // to free HGLOBAL because Clipboard is now 
      // responsible for the copied data. (?)
      //
      // Otherwise the second call will crash
      // the app with a Win32 exception 
      // inside OpenClipboard() function
      //-------------------------------------------
      // Marshal.FreeHGlobal(global);

      return true;
    }
  }
}
