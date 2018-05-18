using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using IdealAutomate.Core;
using Microsoft.Win32;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices;
using IdealSqlTracer;

namespace FindValueInAnyTableInDB {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      bool boolRunningFromHome = false;
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
      this.Hide();

      string strWindowTitle = myActions.PutWindowTitleInEntity();
      if (strWindowTitle.StartsWith("FindValueInAnyTableInDB")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(1000);
      myActions.DebugMode = true;
      string myBigSqlString = "";
      string junk = "";
      string strScriptName = "FindValueInAnyTableInDB";
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
      myControlEntity.ToolTipx = "To find Windows Domain, Open the Control Panel, click the System and Security " + System.Environment.NewLine + "category, and click System. Look under “Computer name, " + System.Environment.NewLine + "domain and workgroup settings” here. If you see “Domain”:" + System.Environment.NewLine + "followed by the name of a domain, your computer is joined to a domain." + System.Environment.NewLine + "Most computers running at home do not have a domain as they do" + System.Environment.NewLine + "not use Active Directory";
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
        goto myExit;
      }

      string strDomainName = myListControlEntity.Find(x => x.ID == "myDomainName").Text;

      fileName = "DomainName.txt";
      WriteValueToAppDirectoryFile(settingsDirectory, fileName, strDomainName);

      ArrayList myServers = new ArrayList();
      List<string> servers = new List<string>();
      List<string> listLocalServers = new List<string>();

      // Get servers from the registry (if any)
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
      foreach (string item in instances) {
        string name = System.Environment.MachineName;
        if (item != "MSSQLSERVER") { name += @"\" + item; }
        if (!servers.Contains(name.ToUpper())) {
          myServers.Add(name.ToUpper());
          listLocalServers.Add(name.ToUpper());
        }
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
        foreach (SearchResult item in result) {
          // Get the properties for 'mySearchResult'.
          ResultPropertyCollection myResultPropColl;

          myResultPropColl = item.Properties;

          foreach (Object myCollection in myResultPropColl["name"]) {
            myServers.Add(myCollection.ToString());
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

      strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 500, -1, 0);

      if (strButtonPressed == "btnCancel") {
        myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
        goto myExit;
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
      string strSelectedServer = strServerName;
      DataTable dtDatabases = GetDatabases(strSelectedServer, strUserName, strPassword);

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
      myControlEntity.ID = "myLabel";
      myControlEntity.Text = "Search for Value";
      myControlEntity.RowNumber = 3;
      myControlEntity.ColumnNumber = 0;
      myListControlEntity.Add(myControlEntity.CreateControlEntity());


      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.TextBox;
      myControlEntity.ID = "myTextBox";
      myControlEntity.Text = "";
      myControlEntity.RowNumber = 3;
      myControlEntity.ColumnNumber = 1;
      myListControlEntity.Add(myControlEntity.CreateControlEntity());

      intRowCtr++;
      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.Label;
      myControlEntity.ID = "lblContainsEqual";
      myControlEntity.Text = "ContainsEqual";
      myControlEntity.RowNumber = intRowCtr;
      myControlEntity.ColumnNumber = 0;
      myControlEntity.ColumnSpan = 1;
      myListControlEntity.Add(myControlEntity.CreateControlEntity());

      List<ComboBoxPair> cbp1 = new List<ComboBoxPair>();

      myControlEntity.ControlEntitySetDefaults();
      myControlEntity.ControlType = ControlType.ComboBox;
      cbp1.Clear();
      cbp1.Add(new ComboBoxPair("Contains", "Contains"));
      cbp1.Add(new ComboBoxPair("Equals", "Equals"));
      myControlEntity.ListOfKeyValuePairs = cbp1;
      myControlEntity.SelectedValue = "Contains";
      myControlEntity.ID = "cbxContainsEqual";
      myControlEntity.RowNumber = intRowCtr;
      myControlEntity.ToolTipx = "";
      myControlEntity.DDLName = "";
      myControlEntity.ColumnNumber = 1;
      myControlEntity.ColumnSpan = 0;
      myListControlEntity.Add(myControlEntity.CreateControlEntity());



      strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 500, -1, 0);

      if (strButtonPressed == "btnCancel") {
        myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
        goto myExit;
      }

      string strSelectedDB = myListControlEntity.Find(x => x.ID == "myComboBox").SelectedValue;

      fileName = "DatabaseSelectedValue.txt";
      WriteValueToAppDirectoryFile(settingsDirectory, fileName, strSelectedDB);




      
      
      myActions.SetValueByKey("GwOptionSetupSelectedServer", strSelectedServer);
      myActions.SetValueByKey("GwOptionSetupSelectedDB", strSelectedDB);
      string strContainsEqual = myListControlEntity.Find(x => x.ID == "cbxContainsEqual").SelectedValue;




      //if (strSelectedDB != "TEST_HOSPIRA_1017" && strSelectedServer == ("cssql2014prod")) {
      //  myActions.MessageBoxShow("cssql2014prod was specified as the server, but TEST_HOSPIRA_1017 was not specified as the DB");
      //  goto DisplayWindow;
      //}










      if (strButtonPressed == "btnCancel") {
        myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
        goto myExit;
      }

      string mySearchTerm = myListControlEntity.Find(x => x.ID == "myTextBox").Text;

      int intNumOfRecordsUnlocked = 0;
      SqlConnection con = new SqlConnection("server=" + strSelectedServer + ";" + "Persist Security Info=True;User ID=" + strUserName + ";Password=" + strPassword + ";database=" + strSelectedDB + "");
      string sqlText = "";
      if (strContainsEqual == "Equals") {
        sqlText = "DECLARE @SearchStr nvarchar(100) " +
  "SET @SearchStr = '" + mySearchTerm + "' " +
  " " +
  " " +
  //"    -- Copyright � 2002 Narayana Vyas Kondreddi. All rights reserved. " +
  //"    -- Purpose: To search all columns of all tables for a given search string " +
  //"    -- Written by: Narayana Vyas Kondreddi " +
  //"    -- Site: http://vyaskn.tripod.com " +
  //"    -- Updated and tested by Tim Gaunt " +
  //"    -- http://www.thesitedoctor.co.uk " +
  //"    -- http://blogs.thesitedoctor.co.uk/tim/2010/02/19/Search+Every+Table+And+Field+In+A+SQL+Server+Database+Updated.aspx " +
  //"    -- Tested on: SQL Server 7.0, SQL Server 2000, SQL Server 2005 and SQL Server 2010 " +
  //"    -- Date modified: 03rd March 2011 19:00 GMT " +
  "    CREATE TABLE #Results (ColumnName nvarchar(370), ColumnValue nvarchar(3630)) " +
  " " +
  "    SET NOCOUNT ON " +
  " " +
  "    DECLARE @TableName nvarchar(256), @ColumnName nvarchar(128), @SearchStr2 nvarchar(110) " +
  "    SET  @TableName = '' " +
  "    SET @SearchStr2 = QUOTENAME(@SearchStr,'''') " +
  " " +
  "    WHILE @TableName IS NOT NULL " +
  "     " +
  "    BEGIN " +
  "        SET @ColumnName = '' " +
  "        SET @TableName =  " +
  "        ( " +
  "            SELECT MIN(QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)) " +
  "            FROM     INFORMATION_SCHEMA.TABLES " +
  "            WHERE         TABLE_TYPE = 'BASE TABLE' " +
  "                AND    QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) > @TableName " +
  "                AND    OBJECTPROPERTY( " +
  "                        OBJECT_ID( " +
  "                            QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) " +
  "                             ), 'IsMSShipped' " +
  "                               ) = 0 " +
  "        ) " +
  " " +
  "        WHILE (@TableName IS NOT NULL) AND (@ColumnName IS NOT NULL) " +
  "             " +
  "        BEGIN " +
  "            SET @ColumnName = " +
  "            ( " +
  "                SELECT MIN(QUOTENAME(COLUMN_NAME)) " +
  "                FROM     INFORMATION_SCHEMA.COLUMNS " +
  "                WHERE         TABLE_SCHEMA    = PARSENAME(@TableName, 2) " +
  "                    AND    TABLE_NAME    = PARSENAME(@TableName, 1) " +
  "                    AND    DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar', 'int', 'decimal') " +
  "                    AND    QUOTENAME(COLUMN_NAME) > @ColumnName " +
  "            ) " +
  "     " +
  "            IF @ColumnName IS NOT NULL " +
  "             " +
  "            BEGIN " +
  "                INSERT INTO #Results " +
  "                EXEC " +
  "                ( " +
  "                    'SELECT ''' + @TableName + '.' + @ColumnName + ''', LEFT(' + @ColumnName + ', 3630) FROM ' + @TableName + ' (NOLOCK) ' + " +
  "                    ' WHERE ' + @ColumnName + ' = ' + @SearchStr2 " +
  "                ) " +
  "            END " +
  "        END     " +
  "    END " +
  " " +
  "    SELECT '\"'  + ColumnName + '\"', '\"' + ColumnValue + '\"' FROM #Results " +
  " " +
  "DROP TABLE #Results ";
      } else {
         sqlText = "DECLARE @SearchStr nvarchar(100) " +
"SET @SearchStr = '" + mySearchTerm + "' " +
" " +
" " +
//"    -- Copyright � 2002 Narayana Vyas Kondreddi. All rights reserved. " +
//"    -- Purpose: To search all columns of all tables for a given search string " +
//"    -- Written by: Narayana Vyas Kondreddi " +
//"    -- Site: http://vyaskn.tripod.com " +
//"    -- Updated and tested by Tim Gaunt " +
//"    -- http://www.thesitedoctor.co.uk " +
//"    -- http://blogs.thesitedoctor.co.uk/tim/2010/02/19/Search+Every+Table+And+Field+In+A+SQL+Server+Database+Updated.aspx " +
//"    -- Tested on: SQL Server 7.0, SQL Server 2000, SQL Server 2005 and SQL Server 2010 " +
//"    -- Date modified: 03rd March 2011 19:00 GMT " +
"    CREATE TABLE #Results (ColumnName nvarchar(370), ColumnValue nvarchar(3630)) " +
" " +
"    SET NOCOUNT ON " +
" " +
"    DECLARE @TableName nvarchar(256), @ColumnName nvarchar(128), @SearchStr2 nvarchar(110) " +
"    SET  @TableName = '' " +
"    SET @SearchStr2 = QUOTENAME('%' + @SearchStr + '%','''') " +
" " +
"    WHILE @TableName IS NOT NULL " +
"     " +
"    BEGIN " +
"        SET @ColumnName = '' " +
"        SET @TableName =  " +
"        ( " +
"            SELECT MIN(QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)) " +
"            FROM     INFORMATION_SCHEMA.TABLES " +
"            WHERE         TABLE_TYPE = 'BASE TABLE' " +
"                AND    QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) > @TableName " +
"                AND    OBJECTPROPERTY( " +
"                        OBJECT_ID( " +
"                            QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) " +
"                             ), 'IsMSShipped' " +
"                               ) = 0 " +
"        ) " +
" " +
"        WHILE (@TableName IS NOT NULL) AND (@ColumnName IS NOT NULL) " +
"             " +
"        BEGIN " +
"            SET @ColumnName = " +
"            ( " +
"                SELECT MIN(QUOTENAME(COLUMN_NAME)) " +
"                FROM     INFORMATION_SCHEMA.COLUMNS " +
"                WHERE         TABLE_SCHEMA    = PARSENAME(@TableName, 2) " +
"                    AND    TABLE_NAME    = PARSENAME(@TableName, 1) " +
"                    AND    DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar', 'int', 'decimal') " +
"                    AND    QUOTENAME(COLUMN_NAME) > @ColumnName " +
"            ) " +
"     " +
"            IF @ColumnName IS NOT NULL " +
"             " +
"            BEGIN " +
"                INSERT INTO #Results " +
"                EXEC " +
"                ( " +
"                    'SELECT ''' + @TableName + '.' + @ColumnName + ''', LEFT(' + @ColumnName + ', 3630) FROM ' + @TableName + ' (NOLOCK) ' + " +
"                    ' WHERE ' + @ColumnName + ' LIKE ' + @SearchStr2 " +
"                ) " +
"            END " +
"        END     " +
"    END " +
" " +
"    SELECT '\"'  + ColumnName + '\"', '\"' + ColumnValue + '\"' FROM #Results " +
" " +
"DROP TABLE #Results ";
      }

      SqlCommand cmd = new SqlCommand(sqlText, con);

      cmd.CommandType = CommandType.Text;

     // Add Parameters to Command Parameters collection
      //cmd.Parameters.Add("@Value", SqlDbType.VarChar);
      //cmd.Parameters["@Value"].Value = mySearchTerm;
    
      StringBuilder sb = new StringBuilder();
      int intCount = 0;
      string strOutFile = @"C:\Data\FindDataInTable.csv";
      try {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutFile)) {
          try {
            con.Open();
            cmd.CommandTimeout = 0;
            SqlDataReader reader = cmd.ExecuteReader();
            //(CommandBehavior.SingleRow)
            while (reader.Read()) {
              file.WriteLine(String.Format("{0},{1}", reader.GetString(0), reader.GetString(1).Replace("\"", "")));
              intCount++;
            }
            reader.Close();
          } finally {
            con.Close();
          }
          file.WriteLine("Total Found:," + intCount.ToString());
        }
      } catch (Exception ex) {

        myActions.MessageBoxShow("Exception Message: " + ex.Message + " Exception InnerException: " + ex.InnerException);
        
      }

      string strExecutable = @"C:\Program Files\Microsoft Office\Office16\EXCEL.EXE";
      string strContent = @"C:\Data\FindDataInTable.csv";
      myActions.MessageBoxShow("Launching Excel - hit okay and do not type until it loads");
      Process.Start(strExecutable, string.Concat("", strContent, ""));
      myActions.Sleep(3000);
      myActions.ActivateWindowByTitle("FindDataInTable.csv - Excel");
      myActions.TypeText("^({F10})", 1000);
      myActions.TypeText("%(H)", 1000);
      myActions.TypeText("O", 1000);
      myActions.TypeText("I", 1000);


      myExit:
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