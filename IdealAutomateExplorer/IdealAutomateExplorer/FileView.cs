#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

using System.IO;
using System.Drawing;
using IdealAutomate.Core;
using System.Collections;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Resources;
using System.Linq;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Samples
{
    public class FileView: IDisposable
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool DestroyIcon(IntPtr hIcon);
        private string _path;
        private string _name;
        private string _ext;
        private int _iconIndex;
        private long _size;
        private DateTime _modified;
        private DateTime _created;
        private string _type;
        private Icon _icon;
        private string _hotkey;
        private int _totalExecutions;
        private int _successfulExecutions;
        private int _percentSuccesful;
        private DateTime _lastExecuted;
        private string _hotKeyExecutable;
        private int _avgExecutionTime;
        private int _manualExecutionTime;
        private int _totalSavings;
        private string _custom;
        private string _description;
        private string _status;
        private ArrayList _myArrayList;
        private Icon _plusIcon;
        private Icon _minusIcon;
        private List<ExtensionIcon> _smallImageList = new List<ExtensionIcon>();
        #region Win32 declarations
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0;
        private const uint SHGFI_SMALLICON = 0x1;
        Methods myActions;

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
        #endregion

        //public FileView(string path) : this(new FileInfo(path)) {        }

        public FileView(FileSystemInfo fileInfo, ArrayList myArrayList, Icon plusIcon, Icon minusIcon, ref List<ExtensionIcon> smallImageList, Methods pmyActions) {
            _myArrayList = myArrayList;
            _plusIcon = plusIcon;
            _minusIcon = minusIcon;
            _smallImageList = smallImageList;
            myActions = pmyActions;
            SetState(fileInfo);
        }

        private string _categoryState;
        private int _nestingLevel;

        public string CategoryState {
            get { return _categoryState; }
            set { _categoryState = value; }
        }

        public int NestingLevel {
            get { return _nestingLevel; }
            set { _nestingLevel = value; }
        }


        public bool IsDirectory
        {
            get { return (_size == -1); }
        }

        private void SetState(FileSystemInfo fileInfo) {
            Methods myActions = new Methods();
            _path = fileInfo.FullName;
            _name = fileInfo.Name;
            _ext = fileInfo.Extension;
            string fileFullName = fileInfo.FullName;
            _hotkey = "";
            _totalExecutions = myActions.GetValueByKeyAsIntForNonCurrentScript("ScriptTotalExecutions", myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
            _successfulExecutions = myActions.GetValueByKeyAsIntForNonCurrentScript("ScriptSuccessfulExecutions", myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
            if (_totalExecutions == 0) {
                _percentSuccesful = 0;
            } else {
                decimal decPercentSuccessful = ((decimal)_successfulExecutions / (decimal)_totalExecutions) * 100;
                _percentSuccesful = Decimal.ToInt32(decPercentSuccessful);
            }
            _lastExecuted = myActions.GetValueByKeyAsDateTimeForNonCurrentScript("ScriptStartDateTime", myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
            _avgExecutionTime = myActions.GetValueByKeyAsIntForNonCurrentScript("AvgSuccessfulExecutionTime", myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
            _manualExecutionTime = myActions.GetValueByKeyAsIntForNonCurrentScript("ManualExecutionTime", myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
            _custom = myActions.GetValueByPublicKeyInCurrentFolder("custom", fileFullName);
            _description = myActions.GetValueByPublicKeyInCurrentFolder("description", fileFullName);
            _status = myActions.GetValueByPublicKeyInCurrentFolder("status", fileFullName);
            if (_manualExecutionTime == 0) {
                _totalSavings = 0;
            } else {
                _totalSavings = _successfulExecutions * (_manualExecutionTime - _avgExecutionTime);
            }

            // TODO: move the populate of arrayList to higher level
            _myArrayList = myActions.ReadAppDirectoryKeyToArrayListGlobal("ScriptInfo");
            foreach (var item in _myArrayList) {
                string[] myScriptInfoFields = item.ToString().Split('^');
                string scriptName = myScriptInfoFields[0];
                if (scriptName == myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName)) {
                    string strHotKey = myScriptInfoFields[1];
                    //string strTotalExecutions = myScriptInfoFields[2];
                    //string strSuccessfulExecutions = myScriptInfoFields[3];
                    //string strLastExecuted = myScriptInfoFields[4];
                    string strHotKeyExecutable = myScriptInfoFields[5];
                    //int intTotalExecutions = 0;
                    //Int32.TryParse(strTotalExecutions, out intTotalExecutions);
                    //int intSuccessfulExecutions = 0;
                    //Int32.TryParse(strSuccessfulExecutions, out intSuccessfulExecutions);
                    //DateTime dateLastExecuted = DateTime.MinValue;
                    //DateTime.TryParse(strLastExecuted, out dateLastExecuted);
                    _hotkey = strHotKey;
                    //_totalExecutions = intTotalExecutions;
                    //_successfulExecutions = intSuccessfulExecutions;
                    //if (_totalExecutions == 0) {
                    //    _percentSuccesful = 0;
                    //} else {
                    //    _percentSuccesful = (_successfulExecutions / _totalExecutions) * 100;
                    //}
                    //_lastExecuted = dateLastExecuted;
                    //if (_lastExecuted == DateTime.MinValue) {
                    //    _lastExecuted = null;
                    //}
                    _hotKeyExecutable = strHotKeyExecutable;
                }
            }

            // Check if not a directory (size is not valid)
            if (fileInfo is FileInfo) {
                fileInfo.Refresh();
                try {
                    _size = (fileInfo as FileInfo).Length;
                } catch (Exception) {

                    // throw;
                }

            } else {
                _size = -1;
            }

            _modified = fileInfo.LastWriteTime;
            _created = fileInfo.CreationTime;

            // Get Type Name
            using (Win32.SHFILEINFOx info = Win32.ShellGetFileInfo.GetFileInfo(fileInfo.FullName)) {
                _type = info.szTypeName;

                // Get ICON
                CategoryState = "";
                NestingLevel = myActions.GetValueByKeyAsInt("NestingLevel");

                try {
                    
                    _iconIndex = GetIconIndex(fileInfo.FullName);
                    _icon = _smallImageList[_iconIndex].Iconx;
                    //System.Drawing.Icon.FromHandle(info.hIcon).Dispose();
                    //DestroyIcon(info.hIcon);
                    string scriptName = myActions.ConvertFullFileNameToPublicPath(fileInfo.FullName) + "\\" + fileInfo.Name;
                    string categoryState = myActions.GetValueByPublicKeyForNonCurrentScript("CategoryState", scriptName);
                    string expandCollapseAll = myActions.GetValueByKey("ExpandCollapseAll");

                    if (categoryState == "Collapsed") {
                        if (expandCollapseAll == "Expand") {
                            categoryState = "Expanded";
                            myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Expanded", scriptName);
                        }
                        if (expandCollapseAll == "Collapse") {
                            categoryState = "Collapsed";
                            myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Collapsed", scriptName);
                        }
                        CategoryState = categoryState;
                        _icon = _plusIcon;
                    }
                    if (categoryState == "Expanded") {
                        if (expandCollapseAll == "Expand") {
                            categoryState = "Expanded";
                            myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Expanded", scriptName);
                        }
                        if (expandCollapseAll == "Collapse") {
                            categoryState = "Collapsed";
                            myActions.SetValueByPublicKeyForNonCurrentScript("CategoryState", "Collapsed", scriptName);
                        }
                        CategoryState = categoryState;
                        _icon = _minusIcon;
                    }
                    if (categoryState == "Child") {
                        CategoryState = categoryState;
                    }
                } catch (Exception ex) {
                    string myname = "wade";
                    //DestroyIcon(info.hIcon);
                    // I think this error is just occurring during debugging
                }

            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        public string Ext {
            get { return _ext; }
            set {
                _ext = value;
            }
        }

        public int IconIndex {
            get { return _iconIndex; }
            set {
                _iconIndex = value;
            }
        }


        public string HotKey {
            get {
                return _hotkey;
                }           
        }

        public string HotKeyExecutable {
            get {
                return _hotKeyExecutable;
            }
        }

        public int TotalExecutions {
            get {
                return _totalExecutions;
            }
        }

        public int SuccessfulExecutions {
            get {
                return _successfulExecutions;
            }
        }

        public int PercentCorrect {
            get {
                return _percentSuccesful;
            }
        }

        public int AvgExecutionTime {
            get {
                return _avgExecutionTime;
            }
        }

        public int ManualExecutionTime {
            get {
                return _manualExecutionTime;
            }
        }

        public string Custom {
            get {
                return _custom;
            }
        }

        public string Description {
            get {
                return _description;
            }
        }

        public string Status {
            get {
                return _status;
            }
        }

        public int TotalSavings {
            get {
                return _totalSavings;
            }
        }

        public DateTime LastExecuted {
            get {
                return _lastExecuted;
            }
        }


        public long Size
        {
            get { return _size; }
        }

        public DateTime DateModified
        {
            get { return _modified; }
        }

        public DateTime DateCreated
        {
            get { return _created; }
        }

        public string Type
        {
            get { return _type; }
        }

        public Icon Icon
        {
            get { return _icon; }
        }

        public void Update()
        {
            // Reset state
            SetState(new FileInfo(_path));
        }

        public string FullName
        {
            get { return _path; }
        }

        public void Update(FileSystemInfo fsi)
        {
            // Reset state - name changed
            SetState(fsi);
        }
        /// <summary>
        /// Returns index of an icon based on FileName. Note: File should exists!
        /// </summary>
        /// <param name="FileName">Name of an existing File or Directory</param>
        /// <returns>Index of an Icon</returns>
        public int GetIconIndex(string FileName) {
            SHFILEINFO shinfo = new SHFILEINFO();

            FileInfo info = new FileInfo(FileName);

            string ext = info.Extension;
            if (String.IsNullOrEmpty(ext)) {
                if ((info.Attributes & FileAttributes.Directory) != 0)
                    ext = "5EEB255733234c4dBECF9A128E896A1E"; // for directories
                else
                    ext = "F9EB930C78D2477c80A51945D505E9C4"; // for files without extension
            } else
                if (ext.Equals(".exe", StringComparison.InvariantCultureIgnoreCase) ||
                    ext.Equals(".lnk", StringComparison.InvariantCultureIgnoreCase))
                ext = info.Name;

            if (_smallImageList.Any(x => x.Ext == ext)) { 
                return _smallImageList.FindIndex(x => x.Ext == ext);
            } else {
                SHGetFileInfo(FileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON);
                Icon smallIcon;
                try {
                    smallIcon = Icon.FromHandle(shinfo.hIcon);
                } catch (ArgumentException ex) {
                    throw new ArgumentException(String.Format("File \"{0}\" doesn not exist!", FileName), ex);
                }
                ExtensionIcon extensionIcon = new ExtensionIcon();
                extensionIcon.Ext = ext;
                extensionIcon.Iconx = smallIcon;
                _smallImageList.Add(extensionIcon);

                //SHGetFileInfo(FileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);
                //Icon largeIcon = Icon.FromHandle(shinfo.hIcon);
                //_largeImageList.Images.Add(ext, largeIcon);

                return _smallImageList.Count - 1;
            }
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
