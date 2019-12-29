#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using IdealAutomate.Core;
using System.Collections;
using Shell32;
using System.Drawing;

#endregion

namespace System.Windows.Forms.Samples
{
    class DirectoryView : SortableBindingList<FileView>,IDisposable
    {
        private FileView _directory;
        private bool _suspend = false;
        private int _nestingLevel = 0;
        private Icon _plusIcon;
        private Icon _minusIcon;
        private List<ExtensionIcon> _smallImageList = new List<ExtensionIcon>();

        //public DirectoryView() : this(Environment.GetFolderPath(Environment.SpecialFolder.Personal)) { }

        private AsyncOperation _oper = null;
        private ArrayList _myArrayList;
        private Methods myActions;

        public DirectoryView(string dir, ArrayList myArrayList, Icon plusIcon, Icon minusIcon, ref List<ExtensionIcon> smallImageList, Methods pmyActions)
        {
            _myArrayList = myArrayList;
            _plusIcon = plusIcon;
            _minusIcon = minusIcon;
            _smallImageList = smallImageList;
            // Setup Async
            _oper = AsyncOperationManager.CreateOperation(null);
            myActions = pmyActions;

            // Fill
            Fill(dir, true, myActions);

            // Setup the FileSystemWatcher
            FileSystemWatcher fsw = new FileSystemWatcher(dir);
            fsw.EnableRaisingEvents = true;

            fsw.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.LastAccess;
            fsw.Changed += new FileSystemEventHandler(FileSystem_Changed);
            fsw.Created += new FileSystemEventHandler(FileSystem_Created);
            fsw.Deleted += new FileSystemEventHandler(FileSystem_Deleted);
            fsw.Renamed += new RenamedEventHandler(FileSystem_Renamed);

            // Debug info
            WriteDebugThreadInfo("DirectoryView");
        }

        public DirectoryView(string dir, bool fillDir, ArrayList myArrayList, Methods myActions ) {
            _myArrayList = myArrayList;
            // Setup Async
            _oper = AsyncOperationManager.CreateOperation(null);

            // Fill
            Fill(dir, fillDir, myActions);

            // Setup the FileSystemWatcher
            FileSystemWatcher fsw = new FileSystemWatcher(dir);
            fsw.EnableRaisingEvents = true;

            fsw.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.LastAccess;
            fsw.Changed += new FileSystemEventHandler(FileSystem_Changed);
            fsw.Created += new FileSystemEventHandler(FileSystem_Created);
            fsw.Deleted += new FileSystemEventHandler(FileSystem_Deleted);
            fsw.Renamed += new RenamedEventHandler(FileSystem_Renamed);

            // Debug info
            WriteDebugThreadInfo("DirectoryView");
        }

        private void Fill(string dir,bool fillDir, Methods myActions)
        {
           
            myActions.SetValueByKey("NestingLevel", _nestingLevel.ToString());
            // Suspend
            _suspend = true;

            // Clear
            Clear();

            // Get directory info
            DirectoryInfo info = new DirectoryInfo(dir);

            // Set the current directory
            _directory = new FileView(info, _myArrayList, _plusIcon, _minusIcon, ref _smallImageList, myActions);

            // Load child files and directories
            try {
                if (fillDir) {
                    foreach (FileSystemInfo di in info.GetDirectories()) {
                      //  if (di.Name != "..IdealAutomate") {
                            this.Add(new FileView(di, _myArrayList, _plusIcon, _minusIcon, ref _smallImageList, myActions));
                            FileView myFileView = new FileView(di, _myArrayList, _plusIcon, _minusIcon, ref _smallImageList, myActions);

                            if (myFileView.CategoryState == "Expanded") {
                                _nestingLevel++;
                                myActions.SetValueByKey("NestingLevel", _nestingLevel.ToString());
                                DirectoryInfo info2 = new DirectoryInfo(di.FullName);
                                foreach (FileSystemInfo di2 in info2.GetDirectories()) {
                                  //  if (di2.Name != "..IdealAutomate") {
                                        string categoryState = myActions.GetValueByPublicKeyForNonCurrentScript("CategoryState", myActions.ConvertFullFileNameToPublicPath(di2.FullName));
                                        this.Add(new FileView(di2, _myArrayList, _plusIcon, _minusIcon, ref _smallImageList, myActions));
                                        FileView myFileView2 = new FileView(di2, _myArrayList, _plusIcon, _minusIcon, ref _smallImageList, myActions);
                                        if (myFileView2.CategoryState == "Expanded") {
                                            _nestingLevel++;
                                            myActions.SetValueByKey("NestingLevel", _nestingLevel.ToString());
                                            DirectoryInfo info3 = new DirectoryInfo(di2.FullName);
                                            foreach (FileSystemInfo di3 in info3.GetDirectories()) {
                                               // if (di3.Name != "..IdealAutomate") {
                                                    categoryState = myActions.GetValueByPublicKeyForNonCurrentScript("CategoryState", myActions.ConvertFullFileNameToPublicPath(di3.FullName));
                                                    this.Add(new FileView(di3, _myArrayList, _plusIcon, _minusIcon, ref _smallImageList, myActions));
                                                    FileView myFileView3 = new FileView(di3, _myArrayList, _plusIcon, _minusIcon, ref _smallImageList, myActions);
                                                    if (myFileView3.CategoryState == "Expanded") {
                                                        DirectoryInfo info4 = new DirectoryInfo(di3.FullName);
                                                        _nestingLevel++;
                                                        myActions.SetValueByKey("NestingLevel", _nestingLevel.ToString());
                                                        foreach (FileSystemInfo di4 in info4.GetDirectories()) {
                                                           // if (di4.Name != "..IdealAutomate") {
                                                                categoryState = myActions.GetValueByPublicKeyForNonCurrentScript("CategoryState", myActions.ConvertFullFileNameToPublicPath(di4.FullName));
                                                                this.Add(new FileView(di4, _myArrayList, _plusIcon, _minusIcon, ref _smallImageList, myActions));
                                                                FileView myFileView4 = new FileView(di4, _myArrayList, _plusIcon, _minusIcon, ref _smallImageList, myActions);
                                                                if (myFileView4.CategoryState == "Expanded") {
                                                                    DirectoryInfo info5 = new DirectoryInfo(di4.FullName);
                                                                    foreach (FileSystemInfo fi in info5.GetFiles()) {
                                                                        this.Add(new FileView(fi, _myArrayList, _plusIcon, _minusIcon, ref _smallImageList, myActions));
                                                                    }
                                                                    _nestingLevel--;
                                                                    myActions.SetValueByKey("NestingLevel", _nestingLevel.ToString());
                                                                }
                                                          //  }
                                                        }

                                                        foreach (FileSystemInfo fi in info4.GetFiles()) {
                                                            this.Add(new FileView(fi, _myArrayList, _plusIcon, _minusIcon, ref _smallImageList, myActions));
                                                        }
                                                        _nestingLevel--;
                                                        myActions.SetValueByKey("NestingLevel", _nestingLevel.ToString());
                                                    }
                                              //  }
                                            }
                                            foreach (FileSystemInfo fi in info3.GetFiles()) {
                                                this.Add(new FileView(fi, _myArrayList, _plusIcon, _minusIcon, ref _smallImageList, myActions));
                                            }
                                            _nestingLevel--;
                                            myActions.SetValueByKey("NestingLevel", _nestingLevel.ToString());
                                        }
                                   // }
                                }

                                foreach (FileSystemInfo fi in info2.GetFiles()) {
                                    this.Add(new FileView(fi, _myArrayList, _plusIcon, _minusIcon, ref _smallImageList, myActions));
                                }
                                _nestingLevel--;
                                myActions.SetValueByKey("NestingLevel", _nestingLevel.ToString());
                            }
                      //  }
                    }

                    foreach (FileSystemInfo fi in info.GetFiles()) {
                        this.Add(new FileView(fi, _myArrayList, _plusIcon, _minusIcon, ref _smallImageList, myActions));
                    }
                }
            } catch (Exception ex) {

               // MessageBox.Show(ex.Message);
            }

            // Resume ListChanged events
            _suspend = false;
            
            // Reset
            ResetBindings();
        }

        public FileView FileView
        {
            get { return _directory; }
        }

        public void Activate(FileView fv)
        {
            if (fv.IsDirectory)
            {
                // Reload the list
                Fill(fv.FullName, true, myActions);
            }
            else
            {
                Process process = new Process();
                process.StartInfo.FileName = fv.FullName;
                if (fv.FullName.EndsWith(".lnk")) {
                    process.StartInfo.FileName = GetShortcutTargetFile(fv.FullName);

                }
                process.StartInfo.Verb = "Open";
                process.Start();
            }
        }
        public static string GetShortcutTargetFile(string shortcutFilename) {
            string pathOnly = System.IO.Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = System.IO.Path.GetFileName(shortcutFilename);

            Shell32.Shell shell = new Shell32.Shell();
            Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null) {
                Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return string.Empty;
        }



        public void Up()
        {
            DirectoryInfo di = new DirectoryInfo(_directory.FullName);
            DirectoryInfo parent = di.Parent;

            if (null != parent)
            {
                Fill(parent.FullName, true, myActions);
            }
        }

        public FileView Find(string name)
        {
            FileView item = null;

            foreach (FileView fv in this.Items)
            {
                if (fv.Name == name)
                {
                    item = fv;
                    break;
                }
            }

            return item;
        }

        private FileSystemInfo GetFileSystemInfo(string path)
        {
            // Return fileInfo for files and dirInfo for directories
            FileSystemInfo fileInfo = new FileInfo(path);

            if ((fileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                fileInfo = new DirectoryInfo(path);
            }

            return fileInfo;
        }

        void FileSystem_Changed(object sender, FileSystemEventArgs e)
        {
            FileView fv = Find(e.Name);

            if (null != fv)
            {
                fv.Update();
                this.ResetItem(this.IndexOf(fv));
            }
        }

        void FileSystem_Created(object sender, FileSystemEventArgs e)
        {
            this.Add(new FileView(GetFileSystemInfo(e.FullPath), _myArrayList, _plusIcon, _minusIcon, ref _smallImageList, myActions));
        }

        void FileSystem_Deleted(object sender, FileSystemEventArgs e)
        {
            FileView fv = this.Find(e.Name);

            if (null != fv)
            {
                this.Remove(fv);
            }
        }

        void FileSystem_Renamed(object sender, RenamedEventArgs e)
        {
            FileView fv = Find(e.OldName);

            if (null != fv)
            {
                fv.Update(GetFileSystemInfo(e.FullPath));
                this.ResetItem(this.IndexOf(fv));
            }
        }

        #region List Changed
        private void PostCallback(object state)
        {
            // Make sure we fire ListChanged on the UI thread
            ListChangedEventArgs args = (state as ListChangedEventArgs);
            try {
                base.OnListChanged(args);
            } catch (Exception) {

                
            }

            // Debug info
            WriteDebugThreadInfo("PostCallback");
        }

        protected override void OnListChanged(System.ComponentModel.ListChangedEventArgs e)
        {
            //base.OnListChanged(e);
            if (!_suspend)
            {
                // Debug Info
                WriteDebugThreadInfo("OnListChanged");

                // We need to marshall changes back to the UI thread since FileSystemWatcher fires
                // updates on any ol' thread.
                _oper.Post(new SendOrPostCallback(PostCallback), e);
            }
        }
        #endregion

        #region DEBUG
        private void WriteDebugThreadInfo(string source)
        {
#if DEBUG
            Thread thread = System.Threading.Thread.CurrentThread;
            string code = thread.GetHashCode().ToString();

            Debug.WriteLine(source + ": " + code + ", background: " + thread.IsBackground.ToString() + ", ThreadPoolThread: " + thread.IsThreadPoolThread.ToString());
#endif
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
        // ~DirectoryView() {
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
        #endregion
    }
}
