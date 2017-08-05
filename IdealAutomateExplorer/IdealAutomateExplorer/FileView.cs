#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

using System.IO;
using System.Drawing;

namespace System.Windows.Forms.Samples
{
    public class FileView
    {
        private string _path;
        private string _name;
        private long _size;
        private DateTime _modified;
        private DateTime _created;
        private string _type;
        private Icon _icon;

        public FileView(string path) : this(new FileInfo(path)) { }

        public FileView(FileSystemInfo fileInfo)
        {
            SetState(fileInfo);
        }

        public bool IsDirectory
        {
            get { return (_size == -1); }
        }

        private void SetState(FileSystemInfo fileInfo)
        {
            _path = fileInfo.FullName;
            _name = fileInfo.Name;

            // Check if not a directory (size is not valid)
            if (fileInfo is FileInfo)
            {
                fileInfo.Refresh();
                try
                {
                    _size = (fileInfo as FileInfo).Length;
                }
                catch (Exception)
                {
                    
                   // throw;
                }
               
            }
            else
            {
                _size = -1;
            }

            _modified = fileInfo.LastWriteTime;
            _created = fileInfo.CreationTime;

            // Get Type Name
            Win32.SHFILEINFO info = Win32.ShellGetFileInfo.GetFileInfo(fileInfo.FullName);

            _type = info.szTypeName;

            // Get ICON
            try
            {
                _icon = System.Drawing.Icon.FromHandle(info.hIcon);
            }
            catch (Exception)
            {
                
                // I think this error is just occurring during debugging
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
    }
}
