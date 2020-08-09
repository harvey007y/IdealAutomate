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

namespace CreateConnectionString
{

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
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            IdealAutomate.Core.Methods myActions = new Methods();
            try
            {
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
                if (strSavedDomainName == "")
                {
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

                if (strButtonPressed == "btnCancel")
                {
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
                try
                {
                    RegistryKey key = RegistryKey.OpenBaseKey(
                              Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32);
                    key = key.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server");
                    object installedInstances = null;
                    if (key != null) { installedInstances = key.GetValue("InstalledInstances"); }
                    List<string> instances = null;
                    if (installedInstances != null) { instances = ((string[])installedInstances).ToList(); }
                    if (System.Environment.Is64BitOperatingSystem)
                    {
                        /* The above registry check gets routed to the syswow portion of 
                         * the registry because we're running in a 32-bit app. Need 
                         * to get the 64-bit registry value(s) */
                        key = RegistryKey.OpenBaseKey(
                                Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
                        key = key.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server");
                        installedInstances = null;
                        if (key != null) { installedInstances = key.GetValue("InstalledInstances"); }
                        string[] moreInstances = null;
                        if (installedInstances != null)
                        {
                            moreInstances = (string[])installedInstances;
                            if (instances == null)
                            {
                                instances = moreInstances.ToList();
                            }
                            else
                            {
                                instances.AddRange(moreInstances);
                            }
                        }
                    }

                    if (instances != null)
                    {
                        foreach (string item in instances)
                        {
                            string name = System.Environment.MachineName;
                            if (item != "MSSQLSERVER") { name += @"\" + item; }
                            if (!myServers.Contains(name.ToUpper()))
                            {
                                myServers.Add(name.ToUpper());
                                listLocalServers.Add(name.ToUpper());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                    MessageBox.Show(ex.StackTrace);
                    MessageBox.Show(ex.InnerException.ToString());
                }
                try
                {
                    string myldap = FriendlyDomainToLdapDomain(strDomainName);

                    string distinguishedName = string.Empty;
                    string connectionPrefix = "LDAP://" + myldap;
                    DirectoryEntry entry = new DirectoryEntry(connectionPrefix);

                    DirectorySearcher mySearcher = new DirectorySearcher(entry);
                    mySearcher.Filter = "(&(objectClass=Computer)(operatingSystem=Windows Server*) (!cn=wde*))";
                    mySearcher.PageSize = 1000;
                    mySearcher.PropertiesToLoad.Add("name");

                    SearchResultCollection result = mySearcher.FindAll();
                    if (result != null)
                    {
                        foreach (SearchResult item in result)
                        {
                            // Get the properties for 'mySearchResult'.
                            ResultPropertyCollection myResultPropColl;

                            myResultPropColl = item.Properties;

                            foreach (Object myCollection in myResultPropColl["name"])
                            {
                                myServers.Add(myCollection.ToString());
                            }
                        }
                    }

                    entry.Close();
                    entry.Dispose();
                    mySearcher.Dispose();
                }
                catch (Exception)
                {
                    // do not show exception because they may not be using active directory
                }
                myServers.Sort();
                fileName = "Servers.txt";
                WriteArrayListToAppDirectoryFile(settingsDirectory, fileName, myServers);
                if (myServers.Count < 1)
                {
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
                foreach (var item in myServers)
                {
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


                // ===========================================================================
                strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 500, -1, 0);

                if (strButtonPressed == "btnCancel")
                {
                    myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                    goto myExitApplication;
                }


                string strServerName = myListControlEntity.Find(x => x.ID == "myComboBox").SelectedValue;
                fileName = "ServerSelectedValue.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strServerName);
                string strAlternateServer = myListControlEntity.Find(x => x.ID == "txtAlternateServer").Text;
                fileName = "txtAlternateServer.txt";
                WriteValueToAppDirectoryFile(settingsDirectory, fileName, strAlternateServer);
                string myFile = "";
                if (strAlternateServer != "")
                {
                    myActions.PutEntityInClipboard(strAlternateServer);
                    myFile = strAlternateServer;
                } else
                {
                    myActions.PutEntityInClipboard(strServerName);
                    myFile = strServerName;
                }
                myActions.MessageBoxShow("Server name " + myFile + " has been copied to clipboard to help you fill out server name");
            } catch (Exception ex)
            {
                myActions.MessageBoxShow(ex.Message);
            }
            string connectionString = PromptForConnectionString("");
            myActions.PutEntityInClipboard(connectionString);
            myActions.MessageBoxShow("ConnectionString " + connectionString + " has been copied to clipboard"); 

       myExitApplication:
            Application.Current.Shutdown();
        }

        private string PromptForConnectionString(string currentConnectionString)
        {
            MSDASC.DataLinks dataLinks = new MSDASC.DataLinksClass();
            ADODB.Connection dialogConnection;
            string generatedConnectionString = string.Empty;

            if (currentConnectionString == String.Empty)
            {

                dialogConnection = (ADODB.Connection)dataLinks.PromptNew();

                generatedConnectionString = dialogConnection.ConnectionString.ToString();
            }
            else
            {
                dialogConnection = new ADODB.Connection();
                dialogConnection.Provider = "SQLOLEDB.1";
                ADODB.Property persistProperty = dialogConnection.Properties["Persist Security Info"];
                persistProperty.Value = true;

                dialogConnection.ConnectionString = currentConnectionString;
                dataLinks = new MSDASC.DataLinks();

                object objConn = dialogConnection;
                if (dataLinks.PromptEdit(ref objConn))
                {
                    generatedConnectionString = dialogConnection.ConnectionString.ToString();
                }
            }
            generatedConnectionString = generatedConnectionString.Replace("Provider=SQLOLEDB.1;", string.Empty);
            if (
                    !generatedConnectionString.Contains("Integrated Security=SSPI")
                    && !generatedConnectionString.Contains("Trusted_Connection=True")
                    && !generatedConnectionString.Contains("Password=")
                    && !generatedConnectionString.Contains("Pwd=")
                )
                if (dialogConnection.Properties["Password"] != null)
                    generatedConnectionString += ";Password=" + dialogConnection.Properties["Password"].Value.ToString();

            return generatedConnectionString;
        }
        private ArrayList ReadAppDirectoryFileToArrayList(string settingsDirectory, string fileName)
        {
            ArrayList myArrayList = new ArrayList();
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            StreamReader reader = File.OpenText(settingsPath);
            while (!reader.EndOfStream)
            {
                string myLine = reader.ReadLine();
                myArrayList.Add(myLine);
            }
            reader.Close();
            return myArrayList;
        }

        private string ReadValueFromAppDataFile(string settingsDirectory, string fileName)
        {
            StreamReader file = null;
            string strValueRead = "";
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath))
            {
                file = File.OpenText(settingsPath);
                strValueRead = file.ReadToEnd();
                file.Close();
            }
            return strValueRead;
        }

        public string FriendlyDomainToLdapDomain(string friendlyDomainName)
        {
            string ldapPath = null;
            try
            {
                DirectoryContext objContext = new DirectoryContext(
                    DirectoryContextType.Domain, friendlyDomainName);
                Domain objDomain = Domain.GetDomain(objContext);
                ldapPath = objDomain.Name;
            }
            catch (DirectoryServicesCOMException e)
            {
                ldapPath = e.Message.ToString();
            }
            return ldapPath;
        }
        private void WriteValueToAppDirectoryFile(string settingsDirectory, string fileName, string strValueToWrite)
        {
            StreamWriter writer = null;
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            // Hook a write to the text file.
            writer = new StreamWriter(settingsPath);
            // Rewrite the entire value of s to the file
            writer.Write(strValueToWrite);
            writer.Close();
        }

        private void WriteArrayListToAppDirectoryFile(string settingsDirectory, string fileName, ArrayList arrayListToWrite)
        {
            StreamWriter writer = null;
            string settingsPath = Path.Combine(settingsDirectory, fileName);
            // Hook a write to the text file.
            writer = new StreamWriter(settingsPath);
            // Rewrite the entire value of s to the file
            foreach (var item in arrayListToWrite)
            {
                writer.WriteLine(item.ToString());
            }
            writer.Close();
        }

        private string GetAppDirectoryForScript(string strScriptName)
        {
            string settingsDirectory =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\" + strScriptName;
            if (!Directory.Exists(settingsDirectory))
            {
                Directory.CreateDirectory(settingsDirectory);
            }
            return settingsDirectory;
        }


    }
}
