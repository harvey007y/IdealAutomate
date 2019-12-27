#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

#endregion


namespace System.Windows.Forms.Samples.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    };

    public class SHFILEINFOx: IDisposable {
        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;
      
        public string szDisplayName;
      
        public string szTypeName;

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


    public class ShellGetFileInfo: IDisposable
    {
        public const uint SHGFI_ICON = 0x100;         // Gets the icon 
        public const uint SHGFI_DISPLAYNAME = 0x200;  // Gets the Display name
        public const uint SHGFI_TYPENAME = 0x400;     // Gets the type name
        public const uint SHGFI_LARGEICON = 0x0;      // Large icon
        public const uint SHGFI_SMALLICON = 0x1;      // Small icon

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        public static SHFILEINFOx GetFileInfo(string path)
        {
            SHFILEINFO info = new SHFILEINFO();
            SHFILEINFOx infox = new SHFILEINFOx();
            IntPtr icon;
 

            icon = SHGetFileInfo(path, 0, ref info, (uint)Marshal.SizeOf(info), SHGFI_ICON | SHGFI_TYPENAME | SHGFI_SMALLICON);
            infox.hIcon = info.hIcon;
            System.Drawing.Icon.FromHandle(info.hIcon).Dispose();
            DestroyIcon(info.hIcon);
            infox.dwAttributes = info.dwAttributes;
            infox.szDisplayName = info.szDisplayName;
            infox.szTypeName = info.szTypeName;
            

            return infox;
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
