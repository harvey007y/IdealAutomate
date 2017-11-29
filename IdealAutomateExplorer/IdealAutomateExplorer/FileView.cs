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
        private DateTime _lastExecuted;
        private string _hotKeyExecutable;
        private int _avgExecutionTime;
        private int _manualExecutionTime;
        private int _totalSavings;
        private string _custom;
        private string _description;
        private string _status;

        public FileView(string path) : this(new FileInfo(path)) {        }

        public FileView(FileSystemInfo fileInfo)
        {
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
            _lastExecuted = myActions.GetValueByKeyAsDateTimeForNonCurrentScript("ScriptStartDateTime",myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
            _avgExecutionTime = myActions.GetValueByKeyAsIntForNonCurrentScript("AvgSuccessfulExecutionTime", myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
            _manualExecutionTime = myActions.GetValueByKeyAsIntForNonCurrentScript("ManualExecutionTime", myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
            _custom = myActions.GetValueByKeyForNonCurrentScript("custom", myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
            _description = myActions.GetValueByKeyForNonCurrentScript("description", myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
            _status = myActions.GetValueByKeyForNonCurrentScript("status", myActions.ConvertFullFileNameToScriptPathWithoutRemoveLastLevel(fileFullName));
            if (_manualExecutionTime == 0) {
                _totalSavings = 0;
            } else {
                _totalSavings = _successfulExecutions * (_manualExecutionTime - _avgExecutionTime);
            }

            ArrayList myArrayList = myActions.ReadAppDirectoryKeyToArrayListGlobal("ScriptInfo");
            foreach (var item in myArrayList) {
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
            CategoryState = "";
            NestingLevel = myActions.GetValueByKeyAsInt("NestingLevel");

            try
            {
                _icon = System.Drawing.Icon.FromHandle(info.hIcon);               
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
                    _icon = new Icon(Properties.Resources._112_Plus_Grey,16,16);
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
                    _icon = new Icon(Properties.Resources._112_Minus_Grey, 16, 16);
                }
                if (categoryState == "Child") {
                    CategoryState = categoryState;                   
                }
            }
            catch (Exception ex)
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
    }
}
