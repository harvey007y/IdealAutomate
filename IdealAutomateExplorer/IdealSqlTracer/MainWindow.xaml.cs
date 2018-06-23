using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using IdealAutomate.Core;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Collections;
using System.Data;

using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices;
using Microsoft.Win32;
using System.Linq;
using System.Text;

namespace IdealSqlTracer {

    /// <summary>
    /// Steps to use:
    /// IdealSqlTracer is a simple, free, open source alternative to SQL Profiler. The advantage of IdealSqlTracer 
    /// is that it takes all of the sql generated behind the scenes in a desktop application or web page, and it 
    /// formats it to make it easily readable. IdealSqlTracer takes this beautifully formatted sql, and puts it into 
    /// notepad. This allows you to easily cut-n-paste the sql in notepad, and run it directly in Sql Server 
    /// Management Studio (SSMS). The advantage of doing this is that it makes it possible for you to see exactly 
    /// what is going on in your application or website..
    /// 
    /// 
    /// Steps to use:
    ///  1. Get Latest source for IdealSqlTracer at https://github.com/harvey007y/IdealSqlTracer 
    ///  2. Build and Run IdealSqlTracer
    ///  3. A series of dialog boxes will appear that allow you to specify the server, database, username, password, 
    /// and so on. 
    /// 4. Once the basic info is entered, a red dialog box is displayed telling you the trace has started. That 
    /// dialog tells you that you need to perform the action on the website that you want to trace. After the action 
    /// on the website is done, click the okay button in the red dialog box to end the trace and have the formatted 
    /// sql appear in notepad. The next screenshot shows the greenbox dialog that pops up when I am trying to trace 
    /// what is going on behind a desktop app called IdealAutomate. After the greenbox appears, I hit save in the 
    /// IdealAutomate application to cause some sql to be generated. 
    /// 
    ///  After the save completes in IdealAutomate, I hit okay in the greenbox dialog, to see the following formatted 
    /// sql in notepad that was used in the save:
    /// 
    ///  I can cut-n-paste this sql from notepad into SSMS to run it in realtime so that I can identify where any 
    /// problems might be.
    /// 
    /// If you just want some of the generated sql on the page, you can temporarily 
    /// add the following to lines to your code where you want to start selecting the generated sql:
    ///    con = new SqlConnection("Server=yourserver;Initial Catalog=yourdatabase;Integrated Security=SSPI");
    ///    SqlCommand cmd = new SqlCommand();
    ///    cmd.CommandText = "select top 1 name from sysobjects where name = 'START_TRACE'";
    ///    cmd.Connection = con;
    ///    string strStartTrace = cmd.ExecuteScalar();
    ///    con.Close();

    ///    
    /// Then, you temporarily add the following line at the end of where you 
    /// want to stop selecting the generated sql:
    ///    
    ///    con1 = new SqlConnection("Server=yourserver;Initial Catalog=yourdatabase;Integrated Security=SSPI");
    ///    SqlCommand cmd1 = new SqlCommand();
    ///    cmd1.CommandTex1t = "select top 1 name from sysobjects where name = 'END_TRACE'";
    ///    cmd1.Connection1 = con1;
    ///    string strStartTrace = cmd1.ExecuteScalar();
    ///    con1.Close();
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            try {
                var window = new Window() //make sure the window is invisible
                    {
                    Width = 0,
                    Height = 0,
                    Left = -2000,
                    WindowStyle = WindowStyle.None,
                    ShowInTaskbar = false,
                    ShowActivated = false,
                };
                window.Show();
                IdealAutomate.Core.Methods myActions = new Methods();
                InitializeComponent();
                myActions.DebugMode = true;
                string myBigSqlString = "";
                string junk = "";
                string strScriptName = "IdealSqlTracer";
                string settingsDirectory = GetAppDirectoryForScript(strScriptName);
                string fileName;
                string strSavedDomainName;
                fileName = "DomainName.txt";
                strSavedDomainName = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strSavedDomainName == "") {
                    strSavedDomainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
                }

                List<ControlEntity> myListControlEntity = new List<ControlEntity>();
                ControlEntity myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Domain Name";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "myLabel2";
                myControlEntity.Text = "Enter Domain Name";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "myDomainName";
                myControlEntity.Text = strSavedDomainName;
                myControlEntity.ToolTipx = "To find Windows Domain, Open the Control Panel, click the System and Security " + System.Environment.NewLine + "category, and click System. Look under “Computer name, " + System.Environment.NewLine + "domain and workgroup settings” here. If you see “Domain”:" + System.Environment.NewLine + "followed by the name of a domain, your computer is joined to a domain." + System.Environment.NewLine + "Most computers running at home do not have a domain as they do" + System.Environment.NewLine + "not use Active Directory..";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "mylabel";
                myControlEntity.ColumnSpan = 2;
                myControlEntity.Text = "(Leave domain name blank if not using server in Active Directory)";
                myControlEntity.RowNumber = 1;
                myControlEntity.ColumnNumber = 0;
                myControlEntity.Checked = true;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 500, -1, 0);

                if (strButtonPressed == "btnCancel") {
                    myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                    goto myExitApplication;
                }

                string strDomainName = myListControlEntity.Find(x => x.ID == "myDomainName").Text;

                fileName = "DomainName.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strDomainName);

                ArrayList myServers = new ArrayList();
                List<string> servers = new List<string>();
                List<string> listLocalServers = new List<string>();

                // Get servers from the registry (if any)
                try {                   
                    RegistryKey key = RegistryKey.OpenBaseKey(
                              Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32);
                    key = key.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server");
                    object installedInstances = null;                    
                    if (key != null) { installedInstances = key.GetValue("InstalledInstances"); }
                    List<string> instances = null;
                    if (installedInstances != null) { instances = ((string[])installedInstances).ToList(); }
                    if (System.Environment.Is64BitOperatingSystem) {
                        /* The above registry check gets routed to the syswow portion of 
                         * the registry because we're running in a 32-bit app. Need 
                         * to get the 64-bit registry value(s) */
                        key = RegistryKey.OpenBaseKey(
                                Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                        key = key.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server");
                        installedInstances = null;
                        if (key != null) { installedInstances = key.GetValue("InstalledInstances"); }
                        string[] moreInstances = null;
                        if (installedInstances != null) {
                            moreInstances = (string[])installedInstances;
                            if (instances == null) {
                                instances = moreInstances.ToList();
                            } else {
                                instances.AddRange(moreInstances);
                            }
                        }
                    }
              
                    if (instances != null) {       
                        foreach (string item in instances) {                           
                            string name = System.Environment.MachineName;                           
                            if (item != "MSSQLSERVER") { name += @"\" + item; }
                            if (!myServers.Contains(name.ToUpper())) {
                                myServers.Add(name.ToUpper());
                                listLocalServers.Add(name.ToUpper());
                            }
                        }
                    }
                } catch (Exception ex) {

                    MessageBox.Show(ex.Message);
                    MessageBox.Show(ex.StackTrace);
                    MessageBox.Show(ex.InnerException.ToString());
                }
                try {
                    string myldap = FriendlyDomainToLdapDomain(strDomainName);

                    string distinguishedName = string.Empty;
                    string connectionPrefix = "LDAP://" + myldap;
                    DirectoryEntry entry = new DirectoryEntry(connectionPrefix);

                    DirectorySearcher mySearcher = new DirectorySearcher(entry);
                    mySearcher.Filter = "(&(objectClass=Computer)(operatingSystem=Windows Server*) (!cn=wde*))";
                    mySearcher.PageSize = 1000;
                    mySearcher.PropertiesToLoad.Add("name");

                    SearchResultCollection result = mySearcher.FindAll();
                    if (result != null) {
                        foreach (SearchResult item in result) {
                            // Get the properties for 'mySearchResult'.
                            ResultPropertyCollection myResultPropColl;

                            myResultPropColl = item.Properties;

                            foreach (Object myCollection in myResultPropColl["name"]) {
                                myServers.Add(myCollection.ToString());
                            }
                        }
                    }

                    entry.Close();
                    entry.Dispose();
                    mySearcher.Dispose();
                } catch (Exception) {
                    // do not show exception because they may not be using active directory
                }              
                myServers.Sort();
                fileName = "Servers.txt";
                WriteArrayListToAppDirectoryFile(settingsDirectory, fileName, myServers);
                if (myServers.Count < 1) {
                    MessageBox.Show("No servers found on local computer or network - aborting");
                    goto myExitApplication;
                }

                myListControlEntity = new List<ControlEntity>();
                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Select Server";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "myLabel2";
                myControlEntity.Text = "Select Server";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                myControlEntity.ID = "myComboBox";
                myControlEntity.Text = "";
                List<ComboBoxPair> cbp = new List<ComboBoxPair>();
                fileName = "Servers.txt";
                myServers = ReadAppDirectoryFileToArrayList(settingsDirectory, fileName);
                foreach (var item in myServers) {
                    cbp.Add(new ComboBoxPair(item.ToString(), item.ToString()));
                }
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "ServerSelectedValue.txt";
                myControlEntity.SelectedValue = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());
                int intRowCtr = 1;

                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblAlternateServer";
                myControlEntity.Text = "Alternate Server";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtAlternateServer";
                fileName = "txtAlternateServer.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "If server was not in dropdown, you can type it here; otherwise, leave blank";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblUserName";
                myControlEntity.Text = "UserName";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtUserName";
                fileName = "txtUserName.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "User Name for logging into server";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblPassword";
                myControlEntity.Text = "Password";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.PasswordBox;
                myControlEntity.ID = "txtPassword";
                fileName = "txtPss.txt";
                string strPass = "";
                string inputFullFileName1 = Path.Combine(settingsDirectory, fileName.Replace(".", "Encrypted."));
                string outputFullFileName1 = Path.Combine(settingsDirectory, fileName);
                if (File.Exists(inputFullFileName1)) {
                    EncryptDecrypt ed1 = new EncryptDecrypt();
                    ed1.DecryptFile(inputFullFileName1, outputFullFileName1);
                    strPass = ReadValueFromAppDataFile(settingsDirectory, fileName);
                }
                File.Delete(outputFullFileName1);
                myControlEntity.Text = strPass;
                myControlEntity.ToolTipx = "Password for logging into server";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.CheckBox;
                myControlEntity.ID = "myCheckBox";
                myControlEntity.Text = "Remember Password";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                fileName = "RememberPassword.txt";
                string strRememberPassword = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strRememberPassword.ToLower() == "true") {
                    myControlEntity.Checked = true;
                } else {
                    myControlEntity.Checked = false;
                }
                myControlEntity.ForegroundColor = System.Windows.Media.Colors.Red;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());
                // ===========================================================================
                strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 500, -1, 0);

                if (strButtonPressed == "btnCancel") {
                    myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                    goto myExitApplication;
                }

                bool boolRememberPassword = myListControlEntity.Find(x => x.ID == "myCheckBox").Checked;
                fileName = "RememberPassword.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, boolRememberPassword.ToString());
                string strServerName = myListControlEntity.Find(x => x.ID == "myComboBox").SelectedValue;
                fileName = "ServerSelectedValue.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strServerName);
                string strAlternateServer = myListControlEntity.Find(x => x.ID == "txtAlternateServer").Text;
                fileName = "txtAlternateServer.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strAlternateServer);
                string strUserName = myListControlEntity.Find(x => x.ID == "txtUserName").Text;
                fileName = "txtUserName.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strUserName);
                string strPassword = myListControlEntity.Find(x => x.ID == "txtPassword").Text;
                fileName = "txtPss.txt";
                if (boolRememberPassword) {
                    WriteValueToAppDirectoryFile(settingsDirectory, fileName, strPassword);
                    string inputFullFileName = Path.Combine(settingsDirectory, fileName);
                    string outputFullFileName = Path.Combine(settingsDirectory, fileName.Replace(".", "Encrypted."));
                    EncryptDecrypt ed = new EncryptDecrypt();
                    ed.EncryptFile(inputFullFileName, outputFullFileName);
                    File.Delete(inputFullFileName);
                }

                if (strAlternateServer.Trim() != "") {
                    strServerName = strAlternateServer;
                }

                myListControlEntity = new List<ControlEntity>();

                myControlEntity = new ControlEntity();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.Text = "Select Database";
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "myLabel2";
                myControlEntity.Text = "Select Database";
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());
                ArrayList myDatabases = new ArrayList();
                string serverName = strServerName;
                DataTable dtDatabases = GetDatabases(serverName, strUserName, strPassword);

                try {
                    for (int i = 0; i < dtDatabases.Rows.Count; i++) {
                        DataRow dr = dtDatabases.Rows[i];
                        myDatabases.Add(dr["sysdbreg_name"]);
                    }

                } catch (Exception ex) {
                    string exception = ex.Message;
                    myActions.MessageBoxShow(exception);
                }

                fileName = "Databases.txt";
                WriteArrayListToAppDirectoryFile(settingsDirectory, fileName, myDatabases);
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                myControlEntity.ID = "myComboBox";
                myControlEntity.Text = "";
                cbp = new List<ComboBoxPair>();
                fileName = "Databases.txt";
                myDatabases = ReadAppDirectoryFileToArrayList(settingsDirectory, fileName);
                foreach (var item in myDatabases) {
                    cbp.Add(new ComboBoxPair(item.ToString(), item.ToString()));
                }
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "DatabaseSelectedValue.txt";
                myControlEntity.SelectedValue = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.RowNumber = 0;
                myControlEntity.ColumnNumber = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtr = 0;

                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblLocalOutputFolder1";
                myControlEntity.Text = "IMPORTANT: SQL Server needs full control permission to the TraceFile.trc that" + System.Environment.NewLine + "is in the output folder. Failure to do this results in a long list of repetitive error messages";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ForegroundColor = System.Windows.Media.Colors.White;
                myControlEntity.BackgroundColor = System.Windows.Media.Colors.Red;
                myControlEntity.FontWeight = FontWeights.ExtraBold;
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 2;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblLocalOutputFolder";
                myControlEntity.Text = "Local Output Folder";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtLocalOutputFolder";
                fileName = "txtLocalOutputFolder.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "If the SQL Server you are tracing is installed on your local computer, "
                    + System.Environment.NewLine + "specify the folder where the trace can be written to. "
                    + System.Environment.NewLine + "SQLServerMSSQLUser needs Full Control permission rights must be specified for this folder."
                     + System.Environment.NewLine + "EXAMPLE: C:\\Data\\";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblRemoteOutputFolder";
                myControlEntity.Text = "RemoteOutputFolder";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtRemoteOutputFolder";
                fileName = "txtRemoteOutputFolder.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "If the SQL Server you are tracing is installed on a remote computer, " +
                System.Environment.NewLine + "specify the folder where the trace can be written to." +
                System.Environment.NewLine + "SQLServer must have Full Control permission rights for must be specified for the file TraceFile.trc. " +
                System.Environment.NewLine + "If you do not have rights to the remote computer, use shared drive." +
                System.Environment.NewLine + "EXAMPLE: \\\\NetworkShare\\Users\\Wade\\Data\\";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());



                strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 500, -1, 0);

                if (strButtonPressed == "btnCancel") {
                    myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                    goto myExitApplication;
                }

                string strDatabaseName = myListControlEntity.Find(x => x.ID == "myComboBox").SelectedValue;

                fileName = "DatabaseSelectedValue.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strDatabaseName);
                string strLocalOutputFolder = myListControlEntity.Find(x => x.ID == "txtLocalOutputFolder").Text;
                fileName = "txtLocalOutputFolder.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLocalOutputFolder);
                string strRemoteOutputFolder = myListControlEntity.Find(x => x.ID == "txtRemoteOutputFolder").Text;
                fileName = "txtRemoteOutputFolder.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strRemoteOutputFolder);

                intRowCtr = 0;
                myControlEntity = new ControlEntity();
                myListControlEntity = new List<ControlEntity>();
                cbp = new List<ComboBoxPair>();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.ID = "lbl";
                myControlEntity.Text = "Select Filters";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblLocalOutputFolder";
                myControlEntity.Text = "IMPORTANT: if not using localhost, you must uncheck to see any results";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ForegroundColor = System.Windows.Media.Colors.White;
                myControlEntity.BackgroundColor = System.Windows.Media.Colors.Red;
                myControlEntity.FontWeight = FontWeights.ExtraBold;
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 4;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.CheckBox;
                myControlEntity.ID = "myCheckBox";
                myControlEntity.Text = "Using localhost (w3wp and dllhost only)";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 4;
                myControlEntity.Checked = true;
                myControlEntity.ForegroundColor = System.Windows.Media.Colors.Red;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // column headings
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblColumn";
                myControlEntity.Text = "Column";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblLogicalOperator";
                myControlEntity.Text = "Logical Operator";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblComparisonOperator_01";
                myControlEntity.Text = "Comparison Operator";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());


                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblmyValue";
                myControlEntity.Text = "myValue";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_01
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_01.txt";
                string strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_01";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                List<ComboBoxPair> cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_01.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_01";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                List<ComboBoxPair> cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_01.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_01";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_01";
                fileName = "txtmyValue_01.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_02
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_02.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_02";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_02.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_02";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_02.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_02";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_02";
                fileName = "txtmyValue_02.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_03
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_03.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_03";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_03.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_03";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_03.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_03";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_03";
                fileName = "txtmyValue_03.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_04
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_04.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_04";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_04.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_04";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_04.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_04";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_04";
                fileName = "txtmyValue_04.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_05
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_05.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_05";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_05.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_05";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_05.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_05";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_05";
                fileName = "txtmyValue_05.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_06
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_06.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_06";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_06.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_06";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_06.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_06";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_06";
                fileName = "txtmyValue_06.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_07
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_07.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_07";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_07.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_07";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_07.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_07";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_07";
                fileName = "txtmyValue_07.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_08
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_08.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_08";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_08.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_08";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_08.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_08";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_08";
                fileName = "txtmyValue_08.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_09
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_09.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_09";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_09.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_09";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_09.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_09";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_09";
                fileName = "txtmyValue_09.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_10
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_10.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_10";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_10.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_10";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_10.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_10";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_10";
                fileName = "txtmyValue_10.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_11
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_11.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_11";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_11.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_11";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_11.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_11";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_11";
                fileName = "txtmyValue_11.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_12
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_12.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_12";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_12.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_12";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_12.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_12";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_12";
                fileName = "txtmyValue_12.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_13
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_13.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_13";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_13.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_13";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_13.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_13";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_13";
                fileName = "txtmyValue_13.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_14
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_14.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_14";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_14.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_14";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_14.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_14";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_14";
                fileName = "txtmyValue_14.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_15
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_15.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_15";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_15.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_15";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_15.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_15";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_15";
                fileName = "txtmyValue_15.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_16
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_16.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_16";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_16.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_16";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_16.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_16";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_16";
                fileName = "txtmyValue_16.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_17
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_17.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_17";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_17.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_17";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_17.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_17";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_17";
                fileName = "txtmyValue_17.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_18
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_18.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_18";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_18.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_18";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_18.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_18";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_18";
                fileName = "txtmyValue_18.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_19
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_19.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_19";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_19.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_19";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_19.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_19";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_19";
                fileName = "txtmyValue_19.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                // row data_20
                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp.Clear();
                cbp.Add(new ComboBoxPair("--Select--", "-1"));
                cbp.Add(new ComboBoxPair("Application Name", "10"));
                cbp.Add(new ComboBoxPair("NTUserName", "6"));
                cbp.Add(new ComboBoxPair("TextData", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp;
                fileName = "cbxColumn_20.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxColumn_20";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 0;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp1 = new List<ComboBoxPair>();
                cbp1.Add(new ComboBoxPair("--Select--", "-1"));
                cbp1.Add(new ComboBoxPair("And", "0"));
                cbp1.Add(new ComboBoxPair("Or", "1"));
                myControlEntity.ListOfKeyValuePairs = cbp1;
                fileName = "cbxLogicalOperator_20.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxLogicalOperator_20";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.ComboBox;
                cbp2 = new List<ComboBoxPair>();
                cbp2.Add(new ComboBoxPair("--Select--", "-1"));
                cbp2.Add(new ComboBoxPair("equal", "0"));
                cbp2.Add(new ComboBoxPair("not equal", "1"));
                cbp2.Add(new ComboBoxPair("like", "6"));
                cbp2.Add(new ComboBoxPair("not like", "7"));
                myControlEntity.ListOfKeyValuePairs = cbp2;
                fileName = "cbxComparisonOperator_20.txt";
                strTemp = ReadValueFromAppDataFile(settingsDirectory, fileName);
                if (strTemp == "") {
                    myControlEntity.SelectedValue = "-1";
                } else {
                    myControlEntity.SelectedValue = strTemp;
                }
                myControlEntity.ID = "cbxComparisonOperator_20";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ToolTipx = "";
                myControlEntity.DDLName = "";
                myControlEntity.ColumnNumber = 2;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtmyValue_20";
                fileName = "txtmyValue_20.txt";
                myControlEntity.Text = ReadValueFromAppDataFile(settingsDirectory, fileName);
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 3;
                myControlEntity.ColumnSpan = 1;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());



                strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 700, 600, 0, 0);
                if (strButtonPressed == "btnCancel") {
                    myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                    goto myExit;
                }
                bool boolUsingLocalhost = myListControlEntity.Find(x => x.ID == "myCheckBox").Checked;
                // row_01
                string strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_01").SelectedValue;
                fileName = "cbxColumn_01.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                string strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_01").SelectedValue;
                fileName = "cbxLogicalOperator_01.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                string strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_01").SelectedValue;
                fileName = "cbxComparisonOperator_01.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                string strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_01").Text;
                fileName = "txtmyValue_01.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_02
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_02").SelectedValue;
                fileName = "cbxColumn_02.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_02").SelectedValue;
                fileName = "cbxLogicalOperator_02.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_02").SelectedValue;
                fileName = "cbxComparisonOperator_02.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_02").Text;
                fileName = "txtmyValue_02.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_03
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_03").SelectedValue;
                fileName = "cbxColumn_03.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_03").SelectedValue;
                fileName = "cbxLogicalOperator_03.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_03").SelectedValue;
                fileName = "cbxComparisonOperator_03.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_03").Text;
                fileName = "txtmyValue_03.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_04
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_04").SelectedValue;
                fileName = "cbxColumn_04.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_04").SelectedValue;
                fileName = "cbxLogicalOperator_04.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_04").SelectedValue;
                fileName = "cbxComparisonOperator_04.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_04").Text;
                fileName = "txtmyValue_04.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_05
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_05").SelectedValue;
                fileName = "cbxColumn_05.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_05").SelectedValue;
                fileName = "cbxLogicalOperator_05.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_05").SelectedValue;
                fileName = "cbxComparisonOperator_05.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_05").Text;
                fileName = "txtmyValue_05.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_06
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_06").SelectedValue;
                fileName = "cbxColumn_06.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_06").SelectedValue;
                fileName = "cbxLogicalOperator_06.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_06").SelectedValue;
                fileName = "cbxComparisonOperator_06.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_06").Text;
                fileName = "txtmyValue_06.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_07
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_07").SelectedValue;
                fileName = "cbxColumn_07.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_07").SelectedValue;
                fileName = "cbxLogicalOperator_07.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_07").SelectedValue;
                fileName = "cbxComparisonOperator_07.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_07").Text;
                fileName = "txtmyValue_07.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_08
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_08").SelectedValue;
                fileName = "cbxColumn_08.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_08").SelectedValue;
                fileName = "cbxLogicalOperator_08.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_08").SelectedValue;
                fileName = "cbxComparisonOperator_08.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_08").Text;
                fileName = "txtmyValue_08.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_09
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_09").SelectedValue;
                fileName = "cbxColumn_09.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_09").SelectedValue;
                fileName = "cbxLogicalOperator_09.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_09").SelectedValue;
                fileName = "cbxComparisonOperator_09.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_09").Text;
                fileName = "txtmyValue_09.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_10
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_10").SelectedValue;
                fileName = "cbxColumn_10.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_10").SelectedValue;
                fileName = "cbxLogicalOperator_10.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_10").SelectedValue;
                fileName = "cbxComparisonOperator_10.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_10").Text;
                fileName = "txtmyValue_10.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_11
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_11").SelectedValue;
                fileName = "cbxColumn_11.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_11").SelectedValue;
                fileName = "cbxLogicalOperator_11.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_11").SelectedValue;
                fileName = "cbxComparisonOperator_11.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_11").Text;
                fileName = "txtmyValue_11.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_12
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_12").SelectedValue;
                fileName = "cbxColumn_12.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_12").SelectedValue;
                fileName = "cbxLogicalOperator_12.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_12").SelectedValue;
                fileName = "cbxComparisonOperator_12.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_12").Text;
                fileName = "txtmyValue_12.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_13
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_13").SelectedValue;
                fileName = "cbxColumn_13.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_13").SelectedValue;
                fileName = "cbxLogicalOperator_13.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_13").SelectedValue;
                fileName = "cbxComparisonOperator_13.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_13").Text;
                fileName = "txtmyValue_13.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_14
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_14").SelectedValue;
                fileName = "cbxColumn_14.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_14").SelectedValue;
                fileName = "cbxLogicalOperator_14.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_14").SelectedValue;
                fileName = "cbxComparisonOperator_14.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_14").Text;
                fileName = "txtmyValue_14.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_15
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_15").SelectedValue;
                fileName = "cbxColumn_15.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_15").SelectedValue;
                fileName = "cbxLogicalOperator_15.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_15").SelectedValue;
                fileName = "cbxComparisonOperator_15.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_15").Text;
                fileName = "txtmyValue_15.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_16
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_16").SelectedValue;
                fileName = "cbxColumn_16.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_16").SelectedValue;
                fileName = "cbxLogicalOperator_16.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_16").SelectedValue;
                fileName = "cbxComparisonOperator_16.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_16").Text;
                fileName = "txtmyValue_16.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_17
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_17").SelectedValue;
                fileName = "cbxColumn_17.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_17").SelectedValue;
                fileName = "cbxLogicalOperator_17.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_17").SelectedValue;
                fileName = "cbxComparisonOperator_17.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_17").Text;
                fileName = "txtmyValue_17.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_18
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_18").SelectedValue;
                fileName = "cbxColumn_18.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_18").SelectedValue;
                fileName = "cbxLogicalOperator_18.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_18").SelectedValue;
                fileName = "cbxComparisonOperator_18.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_18").Text;
                fileName = "txtmyValue_18.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_19
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_19").SelectedValue;
                fileName = "cbxColumn_19.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_19").SelectedValue;
                fileName = "cbxLogicalOperator_19.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_19").SelectedValue;
                fileName = "cbxComparisonOperator_19.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_19").Text;
                fileName = "txtmyValue_19.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // row_20
                strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_20").SelectedValue;
                fileName = "cbxColumn_20.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strColumn);
                strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_20").SelectedValue;
                fileName = "cbxLogicalOperator_20.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strLogicalOperator);
                strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_20").SelectedValue;
                fileName = "cbxComparisonOperator_20.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strComparisonOperator);
                strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_20").Text;
                fileName = "txtmyValue_20.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strmyValue);

                // build the filters
                StringBuilder sb = new StringBuilder();
                for (int i = 1; i < 21; i++) {
                    string strI = "0";
                    if (i < 10) {
                        strI += i.ToString();
                    } else {
                        strI = i.ToString();
                    }

                    strColumn = myListControlEntity.Find(x => x.ID == "cbxColumn_" + strI).SelectedValue;
                    if (strColumn == "-1") {
                        break;
                    }
                    strLogicalOperator = myListControlEntity.Find(x => x.ID == "cbxLogicalOperator_" + strI).SelectedValue;
                    strComparisonOperator = myListControlEntity.Find(x => x.ID == "cbxComparisonOperator_" + strI).SelectedValue;
                    strmyValue = myListControlEntity.Find(x => x.ID == "txtmyValue_" + strI).Text;
                    sb.AppendLine(" ");
                    //  "exec sp_trace_setfilter  @trace_id, 35, 1, 0, N'" + strDatabaseName + "';   " +
                    sb.AppendLine("exec sp_trace_setfilter  @trace_id, " + strColumn + ", " + strLogicalOperator + ", " + strComparisonOperator + ", N'" + strmyValue + "';   ");
                }

                // Run SqlProfiler
                // Get ProcessID for w3wp.exe
                if (!strRemoteOutputFolder.EndsWith("\\")) {
                    strRemoteOutputFolder = strRemoteOutputFolder + "\\";
                }
                if (!strLocalOutputFolder.EndsWith("\\")) {
                    strLocalOutputFolder = strLocalOutputFolder + "\\";
                }
                string strTraceFullFileName = strRemoteOutputFolder + "TraceFile.trc";
                string strServerType = "Remote";
                if (listLocalServers.Contains(strServerName.ToUpper())) {
                    strTraceFullFileName = strLocalOutputFolder + "TraceFile.trc";
                    strServerType = "Local";
                }

                strDatabaseName = strDatabaseName.Replace("[", "").Replace("]", "");
                int intTraceID = 0;
                SqlConnection thisConnection = new SqlConnection("server=" + strServerName + ";" + "Persist Security Info=True;User ID=" + strUserName + ";Password=" + strPassword + ";database=" + strDatabaseName + "");
                //First insert some records
                //Create Command object
                SqlCommand myCommand = thisConnection.CreateCommand();

                List<int> w3wp_PID = new List<int>();
                List<int> dllhost_PID = new List<int>();
                Process[] localAll = Process.GetProcesses();
                try {
                    // Open Connection
                    thisConnection.Open();
                    Console.WriteLine("Connection Opened");
                    myCommand.CommandText = "declare @trace_id int " +
                    "select @trace_id = id from sys.traces " +
                    @"where path = '" + strTraceFullFileName + "' " +
                    "if @trace_id is not null " +
                    "begin " +
                    "	exec sp_trace_setstatus @trace_id, 0  " +
                    "	exec sp_trace_setstatus @trace_id, 2  " +
                    "end; ";
                    myCommand.ExecuteNonQuery();
                    // need to delete trace file if it exists
                    if (System.IO.File.Exists(strTraceFullFileName)) {
                        System.IO.File.Delete(strTraceFullFileName);
                    }

                    // Create INSERT statement with named parameters
                    myCommand.CommandText = "/* sys.traces shows the existing sql traces on the server */ " +
                    "/* " +
                    "select * from sys.traces " +
                    "*/  " +
                    "/*create a new trace, make sure the @tracefile must NOT exist on the disk yet*/ " +
                    "declare @tracefile nvarchar(500) set @tracefile=N'" + strTraceFullFileName.Replace(".trc", "") + "' " +
                    "declare @trace_id int " +
                    "declare @maxsize bigint " +
                    "set @maxsize =1 " +
                    "exec sp_trace_create @trace_id output,2,@tracefile ,@maxsize " +
                    "select @trace_id " +
                    "  " +
                    "/* add the events of insterest to be traced, and add the result columns of interest " +
                    "  Note: look up in sys.traces to find the @trace_id, here assuming this is the first trace in the server, therefor @trce_id = 5 " +
                    "*/ " +
                    "declare @on bit " +
                    "set @on=1 " +
                    "declare @current_num int " +
                    "set @current_num =1 " +
                    "while(@current_num <65) " +
                    "      begin " +
                    "	  /* " +
                    "      add events to be traced, id 14 is the login event, you add other events per your own requirements, the event id can be found @ BOL http://msdn.microsoft.com/en-us/library/ms186265.aspx " +
                    "      */ " +
                    "	  exec sp_trace_setevent @trace_id,10, @current_num,@on " +
                    "      set @current_num=@current_num+1 " +
                    "      end " +
                    "set @current_num =1 " +
                    "while(@current_num <65) " +
                    "      begin " +
                    "      exec sp_trace_setevent @trace_id,12, @current_num,@on " +
                    "      set @current_num=@current_num+1 " +
                    "      end " +
                    " " +
                    "/* set some filters " +
                    "   " +
                    "--exec sp_trace_setfilter [ @traceid = ] trace_id    " +
                    "--          , [ @columnid = ] column_id   " +
                    "--          , [ @logical_operator = ] logical_operator   " +
                    "--          , [ @comparison_operator = ] comparison_operator   " +
                    "--          , [ @value = ] value   " +
                    "-- Columns " +
                    "-- ApplicationName (10) " +
                    "-- NTUserName (6) " +
                    "-- TextData (1) " +
                    "-- Comparison operators (0: equal; 1: not equal; 6: like; 7: Not like) " +
                    "--Filters: " +
                    "-- DatabaseName (35) " +
                    "--Application Name: (10) " +
                    "--Like: (6) " +
                    "--.N% " +
                    "--Micro% " +
                    " " +
                    "--NTUserName: (6) " +
                    "--Not Like: " +
                    "--myusername " +
                     " " +
                    "--TextData: (1) " +
                    "--Not Like: " +
                    "--exec sp_reset_connection " +
                    "*/ " +
                    " " +
                    "exec sp_trace_setfilter  @trace_id, 35, 0, 0, N'" + strDatabaseName + "';   " +
                       "  " +
                    "exec sp_trace_setfilter  @trace_id, 1, 0, 7, N'exec sp_reset_connection';   " +
                    " " +
                    "exec sp_trace_setfilter  @trace_id, 1, 0, 7, N'/* sys.traces%';  " +
                    sb.ToString() +
                    " " +
                    "/* " +
                    " " +
                    "--turn on the trace: status=1 " +
                    "-- use sys.traces to find the @trace_id, here assuming this is the first trace in the server, so @trce_id = 5 " +
                    "*/ " +
                    "exec sp_trace_setstatus  @trace_id,1 " +
                    "  " +
                    "/* pivot the traced event */ " +
                    "/* " +
                    "select LoginName,DatabaseName,TextData,ClientProcessID,* from fn_trace_gettable(N'" + strTraceFullFileName + "',default) " +
                    "*/ " +
                    "  " +
                    "/* stop trace. Please manually delete the trace file on the disk " +
                    "-- use sys.traces to find the @trace_id, here assuming this is the first trace in the server, so @trce_id = 5 " +
                    "declare @trace_id int " +
                    "set @trace_id=2 " +
                    "exec sp_trace_setstatus @trace_id,0 " +
                    "exec sp_trace_setstatus @trace_id,2 " +
                    "*/ ";

                    // Prepare command for repeated execution
                    myCommand.Prepare();

                    Console.WriteLine("Executing {0}", myCommand.CommandText);
                    intTraceID = (int)myCommand.ExecuteScalar();
                    Console.WriteLine("TraceID : {0}", intTraceID);
                    myActions.WindowShape("GreenBox", "", "", " Trace has been started;\r\n Please perform website\\desktop app action;\r\n After website\\desktop app action completes,\r\n click stop button in this green box to end trace", 400, 500);

                    foreach (var item in localAll) {
                        if (item.ProcessName == "w3wp") {
                            w3wp_PID.Add(item.Id);
                        }
                        if (item.ProcessName == "dllhost") {
                            dllhost_PID.Add(item.Id);
                        }
                    }
                    myCommand.CommandText = @"select TextData,ClientProcessID, DatabaseName from fn_trace_gettable(N'" + strTraceFullFileName + "',default)";
                    SqlDataReader reader = myCommand.ExecuteReader();
                    //(CommandBehavior.SingleRow)
                    string strTextData = "";
                    int intClientProcessID = -1;
                    string strRowDatabaseName = "";

                    while (reader.Read()) {
                        if (!reader.IsDBNull(0)) {
                            strTextData = reader.GetString(0) ?? "";
                        }
                        if (!reader.IsDBNull(1)) {
                            intClientProcessID = reader.GetInt32(1);
                        }
                        if (!reader.IsDBNull(2)) {
                            strRowDatabaseName = reader.GetString(2) ?? "";
                        }

                        bool boolListItemGood = false;
                        foreach (var PID in w3wp_PID) {
                            if (intClientProcessID == PID) {
                                boolListItemGood = true;
                            }
                        }
                        foreach (var PID in dllhost_PID) {
                            if (intClientProcessID == PID) {
                                boolListItemGood = true;
                            }
                        }
                        if (boolUsingLocalhost == false) {
                            if (strRowDatabaseName == strDatabaseName) {
                                boolListItemGood = true;
                            }
                        }
                        if (boolListItemGood) {

                            string mySqlString = "";
                            mySqlString = strTextData;
                            if (mySqlString.Contains("END_TRACE")) {
                                goto myExit;
                            }
                            if (mySqlString.Contains("START_TRACE")) {
                                myBigSqlString = "";
                            } else {
                                myBigSqlString += "\r\n";
                                myBigSqlString += mySqlString;
                            }

                        }
                    }
                    reader.Close();
                    myCommand.CommandText = "declare @trace_id int " +
            "set @trace_id=  " + intTraceID.ToString() + " " +
            "exec sp_trace_setstatus @trace_id,0 " +
            "exec sp_trace_setstatus @trace_id,2 ";
                    myCommand.ExecuteNonQuery();

                } catch (SqlException ex) {
                    // Display error
                    myActions.MessageBoxShow("Error: " + ex.ToString());
                    Console.WriteLine("Error: " + ex.ToString());
                } finally {
                    // Close Connection
                    thisConnection.Close();
                    Console.WriteLine("Connection Closed");
                }


                myExit:
                // close SQL Profiler
                //myActions.TypeText("%fx", 900);
                if (myBigSqlString == "") {
                    myActions.MessageBoxShow("No SQL was generated that was running under w3wp,dllhost, or the selected db ");
                    goto myExitApplication;
                }
                if (!strLocalOutputFolder.EndsWith("\\")) {
                    strLocalOutputFolder += "\\";
                }
                string strOutFile = strLocalOutputFolder + @"UnformattedSql.sql";
                if (File.Exists(strOutFile)) {
                    File.Delete(strOutFile);
                }
                using (System.IO.StreamWriter filex = new System.IO.StreamWriter(strOutFile)) {

                    filex.Write(myBigSqlString);
                }

                try {
                    string strAppPath = System.AppDomain.CurrentDomain.BaseDirectory;
                    strAppPath = strAppPath.Replace("bin\\Debug\\", "");
                    myActions.RunSync(strAppPath + "SqlFormatter.exe", strLocalOutputFolder + @"UnformattedSql.sql /o:" + strLocalOutputFolder + @"FormattedSql.sql");

                } catch (Exception ex) {
                    myActions.MessageBoxShow(ex.Message);
                    myActions.MessageBoxShow(ex.StackTrace);
                    goto myExitApplication;
                }

                string strExecutable = @"C:\Windows\system32\notepad.exe";
                string strContent = strLocalOutputFolder + @"FormattedSql.sql";
                Process.Start(strExecutable, string.Concat("", strContent, ""));
            } catch (Exception ex) {

                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
                MessageBox.Show(ex.InnerException.ToString());

            }
            myExitApplication:
            Application.Current.Shutdown();
        }

        private ArrayList ReadAppDirectoryFileToArrayList(string settingsDirectory, string fileName) {
            ArrayList myArrayList = new ArrayList();
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            StreamReader reader = File.OpenText(settingsPath);
            while (!reader.EndOfStream) {
                string myLine = reader.ReadLine();
                myArrayList.Add(myLine);
            }
            reader.Close();
            return myArrayList;
        }

        private string ReadValueFromAppDataFile(string settingsDirectory, string fileName) {
            StreamReader file = null;
            string strValueRead = "";
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath)) {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            }
            return strValueRead;
        }

        public string FriendlyDomainToLdapDomain(string friendlyDomainName) {
            string ldapPath = null;
            try {
                DirectoryContext objContext = new DirectoryContext(
                    DirectoryContextType.Domain, friendlyDomainName);
                Domain objDomain = Domain.GetDomain(objContext);
                ldapPath = objDomain.Name;
            } catch (DirectoryServicesCOMException e) {
                ldapPath = e.Message.ToString();
            }
            return ldapPath;
        }
        private void WriteValueToAppDirectoryFile(string settingsDirectory, string fileName, string strValueToWrite) {
            StreamWriter writer = null;
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            // Hook a write to the text file.
            writer = new StreamWriter(settingsPath);
            // Rewrite the entire value of s to the file
            writer.Write(strValueToWrite);
            writer.Close();
        }

        private void WriteArrayListToAppDirectoryFile(string settingsDirectory, string fileName, ArrayList arrayListToWrite) {
            StreamWriter writer = null;
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            // Hook a write to the text file.
            writer = new StreamWriter(settingsPath);
            // Rewrite the entire value of s to the file
            foreach (var item in arrayListToWrite) {
                writer.WriteLine(item.ToString());
            }
            writer.Close();
        }

        private string GetAppDirectoryForScript(string strScriptName) {
            string settingsDirectory =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\" + strScriptName;
            if (!Directory.Exists(settingsDirectory)) {
                Directory.CreateDirectory(settingsDirectory);
            }
            return settingsDirectory;
        }

        private DataTable GetDatabases(string ServerName, string strUserName, string strPassword) {
            string queryString =
      "SELECT name FROM master.dbo.sysdatabases " +
      "";
            // Define Connection String
            string strConnectionString = null;
            // SqlConnection thisConnection = new SqlConnection("server=" + ServerName + ";" + "Persist Security Info=True;User ID=" + strUserName + ";Password=" + strPassword + ";database=" + strDatabaseName + "");

            strConnectionString = @"Data Source=" + ServerName + ";" + "Persist Security Info=True;User ID=" + strUserName + ";Password=" + strPassword + ";";

            // Define .net fields to hold each column selected in query
            String str_sysdbreg_name;
            // Define a datatable that we will define columns in to match the columns
            // selected in the query. We will use sqldatareader to read the results
            // from the sql query one row at a time. Then we will add each of those
            // rows to the datatable - this is where you can modify the information
            // returned from the sql query one row at a time. 
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(strConnectionString)) {
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                // Define a column in the table for each column that was selected in the sql query 
                // We do this before the sqldatareader loop because the columns only need to be  
                // defined once. 

                DataColumn column = null;
                column = new DataColumn("sysdbreg_name", Type.GetType("System.String"));
                dt.Columns.Add(column);
                // Read the results from the sql query one row at a time 
                while (reader.Read()) {
                    // define a new datatable row to hold the row read from the sql query 
                    DataRow dataRow = dt.NewRow();
                    // Move each field from the reader to a holding field in .net 
                    // ******************************************************************** 
                    // The holding field in .net is where you can alter the contents of the 
                    // field 
                    // ******************************************************************** 
                    // Then, you move the contents of the holding .net field to the column in 
                    // the datarow that you defined above 
                    if (!(reader.IsDBNull(0))) {
                        str_sysdbreg_name = reader.GetString(0);
                        dataRow["sysdbreg_name"] = str_sysdbreg_name;
                    }
                    // Add the row to the datatable 
                    dt.Rows.Add(dataRow);
                }

                // Call Close when done reading. 
                reader.Close();
            }

            return dt;
        }
    }
}
