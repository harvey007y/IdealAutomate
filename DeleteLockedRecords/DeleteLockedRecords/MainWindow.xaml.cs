using System.Windows;
using IdealAutomate.Core;
using System;
using System.Data;
using System.Data.SqlClient;


namespace DeleteLockedRecords {
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
      if (strWindowTitle.StartsWith("DeleteLockedRecords")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      // Alternatives are: SqlServer2008, SqlServer2014
      string strServer = myActions.GetValueByKey("SqlServer2008", "IdealAutomateDB");
      // Alternatives are: ChristysDB, QA6DB
      string strDatabaseName = myActions.GetValueByKey("ChristysDB", "IdealAutomateDB");
      SqlConnection thisConnection = new SqlConnection("server=" + strServer + ";" + "integrated security=sspi;database=" + strDatabaseName + "");
      //First insert some records
      //Create Command object
      SqlCommand nonqueryCommand = thisConnection.CreateCommand();

      try {
        // Open Connection
        thisConnection.Open();
        Console.WriteLine("Connection Opened");

        // Create INSERT statement with named parameters
        nonqueryCommand.CommandText = "DECLARE @rectable VARCHAR(500)  " +
"DECLARE @recinc int   " +


"DECLARE db_cursor CURSOR FOR  " +
"select si.REC_TABLE " +
"	,si.REC_INC " +
"from SIGNOUT_INFO si " +
"left join CLIENTSESSION cs on si.CLIENTKEY = cs.CLIENTKEY " +
"left join OPER o on o.OPERINC = 2300 " +
"order by REC_TABLE " +

"OPEN db_cursor  " +
"FETCH NEXT FROM db_cursor INTO @rectable, @recinc  " +

"WHILE @@FETCH_STATUS = 0  " +
"BEGIN  " +
"delete SIGNOUT_INFO " +
"where REC_TABLE = @rectable " +
"	and REC_INC = @recinc " +

"FETCH NEXT FROM db_cursor INTO @rectable, @recinc  " +
"END  " +

"CLOSE db_cursor  " +
"DEALLOCATE db_cursor  ";



        // Prepare command for repeated execution
        nonqueryCommand.Prepare();



        Console.WriteLine("Executing {0}", nonqueryCommand.CommandText);
        Console.WriteLine("Number of rows affected : {0}", nonqueryCommand.ExecuteNonQuery());

      } catch (SqlException ex) {
        // Display error
        Console.WriteLine("Error: " + ex.ToString());
      } finally {
        // Close Connection
        thisConnection.Close();
        Console.WriteLine("Connection Closed");

      }


      Application.Current.Shutdown();
    }
  }
}
