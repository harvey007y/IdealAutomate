#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

using System.IO;
using System.Drawing;
using IdealAutomate.Core;
using System.Collections;

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
        private string _hotkey;
        private int _totalExecutions;
        private int _successfulExecutions;
        private int _percentSuccesful;
        private DateTime? _lastExecuted;
        private string _hotKeyExecutable;
        private int _avgExecutionTime;
        private int _manualExecutionTime;
        private int _totalSavings;

        public FileView(string path) : this(new FileInfo(path)) {        }

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
            Methods myActions = new Methods();
            _path = fileInfo.FullName;
            _name = fileInfo.Name;
            _hotkey = "";
            _totalExecutions = myActions.GetValueByKeyAsIntForNonCurrentScript("ScriptTotalExecutions", _name);
            _successfulExecutions = myActions.GetValueByKeyAsIntForNonCurrentScript("ScriptSuccessfulExecutions", _name);
            _percentSuccesful = myActions.GetValueByKeyAsIntForNonCurrentScript("ScriptPercentSuccessful", _name);
            _lastExecuted = myActions.GetValueByKeyAsDateTimeForNonCurrentScript("ScriptStartDateTime",_name);
            _avgExecutionTime = myActions.GetValueByKeyAsIntForNonCurrentScript("AvgSuccessfulExecutionTime", _name);
            _manualExecutionTime = myActions.GetValueByKeyAsIntForNonCurrentScript("ManualExecutionTime", _name);
            _totalSavings = myActions.GetValueByKeyAsIntForNonCurrentScript("ScriptTotalSavedExecutionTime", _name);


            ArrayList myArrayList = myActions.ReadAppDirectoryKeyToArrayListGlobal("ScriptInfo");
            foreach (var item in myArrayList) {
                string[] myScriptInfoFields = item.ToString().Split('^');
                string scriptName = myScriptInfoFields[0];
                if (scriptName == _name) {
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

        public int TotalSavings {
            get {
                return _totalSavings;
            }
        }

        public DateTime? LastExecuted {
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
    }
}
