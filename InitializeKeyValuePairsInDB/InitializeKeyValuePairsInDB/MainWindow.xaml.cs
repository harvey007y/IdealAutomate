using System.Windows;
using IdealAutomate.Core;
using System.Data.SqlClient;

namespace InitializeKeyValuePairsInDB {
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
      if (strWindowTitle.StartsWith("InitializeKeyValuePairsInDB")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      SqlConnection con = new SqlConnection("Server=(local)\\SQLEXPRESS;Initial Catalog=IdealAutomateDB;Integrated Security=SSPI");
      SqlCommand cmd = new SqlCommand();
      string strMyKey = "";
      string strMyValue = "";
      
      cmd.Connection = con;
      try {
        con.Open();
        strMyKey = "RunningFromHome";
        strMyValue = "True";
        ExecuteSQLToInsertUpdateKeyValuePair(cmd, strMyKey, strMyValue);

        strMyKey = "SsmsPath";
        strMyValue = @"C:\Program Files (x86)\Microsoft SQL Server\130\Tools\Binn\ManagementStudio\Ssms.exe";
        ExecuteSQLToInsertUpdateKeyValuePair(cmd, strMyKey, strMyValue);

        strMyKey = "VS2013Path";
        strMyValue = @"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\devenv.exe";
        ExecuteSQLToInsertUpdateKeyValuePair(cmd, strMyKey, strMyValue);


        strMyKey = "SVNPath";
        strMyValue = myActions.GetValueByKey("SVNPath","IdealAutomateDB");
        ExecuteSQLToInsertUpdateKeyValuePair(cmd, strMyKey, strMyValue);

        strMyKey = "Clipboard";
        strMyValue = @"";
        ExecuteSQLToInsertUpdateKeyValuePair(cmd, strMyKey, strMyValue);
      } finally {
        con.Close();
       
      }
      goto myExit;
     
    myExit:
      myActions.MessageBoxShow("Script Completed");
      Application.Current.Shutdown();
    }
    private static void ExecuteSQLToInsertUpdateKeyValuePair(SqlCommand cmd, string strMyKey, string strMyValue) {
      cmd.CommandText = " IF EXISTS ( " +
"		SELECT * " +
"		FROM KeyValueTable " +
"		WHERE myKey = '" + strMyKey + "' " +
"		) " +
" BEGIN " +
"	UPDATE [dbo].[KeyValueTable] " +
"	SET [myValue] = '" + strMyValue + "' " +
"	WHERE myKey = '" + strMyKey + "' " +
" END; " +
" ELSE " +
" BEGIN " +
"	INSERT INTO [dbo].[KeyValueTable] ( " +
"		[myKey] " +
"		,[myValue] " +
"		) " +
"	VALUES ( " +
"		'" + strMyKey + "' " +
"		,'" + strMyValue + "' " +
"		) " +
"END; ";
      cmd.ExecuteNonQuery();
    }
  }
}
