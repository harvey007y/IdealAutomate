using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

public static class NativeMethods {
    public static Icon GetShellIcon(string path) {
        SHFILEINFO info = new SHFILEINFO();
        IntPtr retval = SHGetFileInfo(path, 0, ref info, Marshal.SizeOf(info), 0x100);
        if (retval == IntPtr.Zero) {
            throw new ApplicationException("Could not retrieve icon");
        }
        //' Invoke private Icon constructor so we do not have to copy the icon
        Type[] cargt = { typeof(IntPtr) };
        ConstructorInfo ci = typeof(Icon).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, cargt, null);
        object[] cargs = { info.IconHandle };
        Icon icon = (Icon)ci.Invoke(cargs);
        return icon;
    }

    //' P/Invoke declaration
    private struct SHFILEINFO {
        public IntPtr IconHandle;
        public int IconIndex;
        public int Attributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string DisplayString;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string TypeName;
    }

    [System.Runtime.InteropServices.DllImport("Shell32.dll", EntryPoint = "SHGetFileInfo", ExactSpelling = false, CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SHGetFileInfo(string path, int attributes, ref SHFILEINFO info, int infoSize, int flags);

}
