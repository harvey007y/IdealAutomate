#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using IdealAutomate.Core;

#endregion

namespace System.Windows.Forms.Samples
{
    class DirectoryView : SortableBindingList<FileView>
    {
        private FileView _directory;
        private bool _suspend = false;

        //public DirectoryView() : this(Environment.GetFolderPath(Environment.SpecialFolder.Personal)) { }

        private AsyncOperation _oper = null;

        public DirectoryView(string dir)
        {
            // Setup Async
            _oper = AsyncOperationManager.CreateOperation(null);

            // Fill
            Fill(dir);

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

        private void Fill(string dir)
        {
            // Suspend
            _suspend = true;

            // Clear
            Clear();

            // Get directory info
            DirectoryInfo info = new DirectoryInfo(dir);

            // Set the current directory
            _directory = new FileView(info);

            // Load child files and directories
            foreach (FileSystemInfo di in info.GetDirectories())
            {
                this.Add(new FileView(di));
                FileView myFileView = new FileView(di);
                if (myFileView.CategoryState == "Expanded") {
                    DirectoryInfo info2 = new DirectoryInfo(di.FullName);
                    foreach (FileSystemInfo di2 in info2.GetDirectories()) {
                        Methods myActions = new Methods();
                        string categoryState = myActions.GetValueByKeyForNonCurrentScript("CategoryState", di2.Name);
                        if (categoryState != "Collapsed" && categoryState != "Expanded") {
                            int categoryLevel = myActions.GetValueByKeyAsIntForNonCurrentScript("CategoryLevel", myFileView.Name);
                            myActions.SetValueByKeyForNonCurrentScript("CategoryState", "Child", di2.Name);
                            int newCategoryLevel = categoryLevel + 1;
                            myActions.SetValueByKeyForNonCurrentScript("CategoryLevel", newCategoryLevel.ToString(), di2.Name);

                        }
                        this.Add(new FileView(di2));
                        FileView myFileView2 = new FileView(di2);
                        if (myFileView2.CategoryState == "Expanded") {
                            DirectoryInfo info3 = new DirectoryInfo(di2.FullName);
                            foreach (FileSystemInfo di3 in info3.GetDirectories()) {      
                                categoryState = myActions.GetValueByKeyForNonCurrentScript("CategoryState", di3.Name);
                                if (categoryState != "Collapsed" && categoryState != "Expanded") {
                                    int categoryLevel = myActions.GetValueByKeyAsIntForNonCurrentScript("CategoryLevel", myFileView2.Name);
                                    myActions.SetValueByKeyForNonCurrentScript("CategoryState", "Child", di3.Name);
                                    int newCategoryLevel = categoryLevel + 1;
                                    myActions.SetValueByKeyForNonCurrentScript("CategoryLevel", newCategoryLevel.ToString(), di3.Name);
                                }
                                this.Add(new FileView(di3));
                                FileView myFileView3 = new FileView(di3);
                                if (myFileView3.CategoryState == "Expanded") {
                                    DirectoryInfo info4 = new DirectoryInfo(di3.FullName);
                                    foreach (FileSystemInfo di4 in info4.GetDirectories()) {
                                        categoryState = myActions.GetValueByKeyForNonCurrentScript("CategoryState", di4.Name);
                                        if (categoryState != "Collapsed" && categoryState != "Expanded") {
                                            int categoryLevel = myActions.GetValueByKeyAsIntForNonCurrentScript("CategoryLevel", myFileView3.Name);
                                            myActions.SetValueByKeyForNonCurrentScript("CategoryState", "Child", di4.Name);
                                            int newCategoryLevel = categoryLevel + 1;
                                            myActions.SetValueByKeyForNonCurrentScript("CategoryLevel", newCategoryLevel.ToString(), di4.Name);
                                        }
                                        this.Add(new FileView(di4));
                                        FileView myFileView4 = new FileView(di4);
                                        if (myFileView4.CategoryState == "Expanded") {
                                            DirectoryInfo info5 = new DirectoryInfo(di4.FullName);

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (FileSystemInfo fi in info.GetFiles())
            {
                this.Add(new FileView(fi));
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
                Fill(fv.FullName);
            }
            else
            {
                Process process = new Process();
                process.StartInfo.FileName = fv.FullName;
                process.StartInfo.Verb = "Open";
                process.Start();
            }
        }

        public void Up()
        {
            DirectoryInfo di = new DirectoryInfo(_directory.FullName);
            DirectoryInfo parent = di.Parent;

            if (null != parent)
            {
                Fill(parent.FullName);
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
            this.Add(new FileView(GetFileSystemInfo(e.FullPath)));
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
            base.OnListChanged(args);

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
        #endregion
    }
}
