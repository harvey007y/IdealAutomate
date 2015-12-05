using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Hardcodet.Wpf.Samples;
using System.Configuration;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Hardcodet.Wpf.Samples {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>


  public partial class Login : Window {
    public Login() {
      try {
        InitializeComponent();
      } catch (Exception ex) {
        MessageBox.Show(ex.Message);
        MessageBox.Show(ex.InnerException.ToString());
        // Log error (including InnerExceptions!)
        // Handle exception
      }

    }

    MainWindow _MainWindow = new MainWindow();


    private byte[] ObjectToByteArray(Object obj) {
      if (obj == null)
        return null;
      BinaryFormatter bf = new BinaryFormatter();
      MemoryStream ms = new MemoryStream();
      bf.Serialize(ms, obj);
      return ms.ToArray();
    }


    private void Window_Initialized(object sender, EventArgs e) {



      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString);
      con.Open();
      string updCmd =

"USE [master] " +
" " +
"IF NOT EXISTS ( " +
"		SELECT * " +
"		FROM sys.databases " +
"		WHERE NAME = 'IdealAutomateDB' " +
"		) " +
"BEGIN " +
"	/****** Object:  Database [IdealAutomateDB]    Script Date: 12/5/2015 3:18:21 PM ******/ " +
"	CREATE DATABASE [IdealAutomateDB] ON PRIMARY ( " +
"		NAME = N'IdealAutomate_Data' " +
@"		,FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\IdealAutomate_Data.mdf' " +
"		,SIZE = 12288 KB " +
"		,MAXSIZE = UNLIMITED " +
"		,FILEGROWTH = 10 % " +
"		) LOG ON ( " +
"		NAME = N'IdealAutomate_Log' " +
@"		,FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\IdealAutomate_Log.ldf' " +
"		,SIZE = 28672 KB " +
"		,MAXSIZE = 2048 GB " +
"		,FILEGROWTH = 1024 KB " +
"		); " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET COMPATIBILITY_LEVEL = 100 " +
" " +
"	IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled')) " +
"	BEGIN " +
"		EXEC [IdealAutomateDB].[dbo].[sp_fulltext_database] @action = 'enable' " +
"	END " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET ANSI_NULL_DEFAULT OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET ANSI_NULLS OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET ANSI_PADDING OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET ANSI_WARNINGS OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET ARITHABORT OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET AUTO_CLOSE ON " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET AUTO_SHRINK OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET AUTO_UPDATE_STATISTICS ON " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET CURSOR_CLOSE_ON_COMMIT OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET CURSOR_DEFAULT GLOBAL " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET CONCAT_NULL_YIELDS_NULL OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET NUMERIC_ROUNDABORT OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET QUOTED_IDENTIFIER OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET RECURSIVE_TRIGGERS OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET ENABLE_BROKER " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET AUTO_UPDATE_STATISTICS_ASYNC OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET DATE_CORRELATION_OPTIMIZATION OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET TRUSTWORTHY OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET ALLOW_SNAPSHOT_ISOLATION OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET PARAMETERIZATION SIMPLE " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET READ_COMMITTED_SNAPSHOT OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET HONOR_BROKER_PRIORITY OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET RECOVERY SIMPLE " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET MULTI_USER " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET PAGE_VERIFY CHECKSUM " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET DB_CHAINING OFF " +
" " +
"	ALTER DATABASE [IdealAutomateDB] " +
" " +
"	SET READ_WRITE " +
"END " +

             " IF  NOT EXISTS (SELECT * FROM sys.objects " +
" WHERE object_id = OBJECT_ID(N'[dbo].[UserInfo]') AND type in (N'U')) " +
" BEGIN  " +
" CREATE TABLE [dbo].[UserInfo](  " +
"	[ID] [int]  NOT NULL,  " +
"	[UserName] [varchar](50) NOT NULL,  " +
"	[ComputerName] [varchar](500) NULL,  " +
"	[FirstName] [varchar](500) NULL,  " +
"	[LastName] [varchar](500) NULL,  " +
"	[Password] [varchar](500) NOT NULL,  " +
"	[Email] [varchar](500) NULL,  " +
"	[Bio] [varchar](max) NULL,  " +
"	[Photo] [image] NULL,  " +
"	[RequireLogin] [bit] NULL,  " +
"	[Category1] [varchar](50) NULL,  " +
"	[Category2] [varchar](50) NULL,  " +
"	[Category3] [varchar](50) NULL,  " +
"	[Category4] [varchar](50) NULL,  " +
"	[Category5] [varchar](50) NULL,  " +
"	[TextSearch] [varchar](500) NULL,  " +
" CONSTRAINT [PK_UserInfo] PRIMARY KEY CLUSTERED   " +
" (  " +
"	[ID] ASC  " +
" )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]  " +
" ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]  " +
" END  " +
"IF NOT EXISTS ( " +
"        SELECT * " +
"        FROM sys.objects " +
"        WHERE object_id = OBJECT_ID(N'[dbo].[Scripts]') " +
"           AND type IN(N'U') " +
"		) " +
" begin  " +
"CREATE TABLE [dbo].[Scripts]( " +
"	[ScriptID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL, " +
"	[ScriptName] [nvarchar](128) NULL, " +
"	[Description] [text] NULL, " +
"	[CreateDate] [datetime] NULL, " +
"	[LastModifiedDate] [datetime] NULL, " +
"	[NumberTimesExecuted] [int] NOT NULL, " +
"	[NumberTimesDownloaded] [int] NOT NULL, " +
"	[LastExecutedDate] [datetime] NULL, " +
"	[NumberActionsSaved] [int] NOT NULL, " +
"	[Title] [varchar](500) NULL, " +
"	[Summary] [text] NULL, " +
"	[SetupInstructions] [text] NULL, " +
"	[NumberTimesExecutedCorrectly] [int] NOT NULL, " +
"	[NumberTimesExecutedIncorrectly] [int] NOT NULL, " +
"	[PercentCorrect] [int] NOT NULL, " +
"	[NumberTimesCancelled] [int] NOT NULL, " +
"	[Category1] [varchar](50) NULL, " +
"	[Category2] [varchar](50) NULL, " +
"	[Category3] [varchar](50) NULL, " +
"	[Category4] [varchar](50) NULL, " +
"	[Category5] [varchar](50) NULL, " +
"	[Executable] [varchar](500) NULL, " +
"	[ExecuteContent] [varchar](500) NULL, " +
" CONSTRAINT [PK_Script_ScriptID] PRIMARY KEY CLUSTERED " +
"( " +
"	[ScriptID] ASC " +
")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
") ON [PRIMARY] TEXTIMAGE_ON [PRIMARY] " +
" " +

" " +
"SET ANSI_PADDING OFF " +

" " +
"ALTER TABLE [dbo].[Scripts] ADD  DEFAULT (getdate()) FOR [CreateDate] " +

" " +
"ALTER TABLE [dbo].[Scripts] ADD  DEFAULT (getdate()) FOR [LastModifiedDate] " +

" " +
"ALTER TABLE [dbo].[Scripts] ADD  DEFAULT ((0)) FOR [NumberTimesExecuted] " +

" " +
"ALTER TABLE [dbo].[Scripts] ADD  DEFAULT ((0)) FOR [NumberTimesDownloaded] " +

" " +
"ALTER TABLE [dbo].[Scripts] ADD  DEFAULT (getdate()) FOR [LastExecutedDate] " +

" " +
"ALTER TABLE [dbo].[Scripts] ADD  DEFAULT ((0)) FOR [NumberActionsSaved] " +

" " +
"ALTER TABLE [dbo].[Scripts] ADD  DEFAULT ((0)) FOR [NumberTimesExecutedCorrectly] " +

" " +
"ALTER TABLE [dbo].[Scripts] ADD  DEFAULT ((0)) FOR [NumberTimesExecutedIncorrectly] " +

" " +
"ALTER TABLE [dbo].[Scripts] ADD  DEFAULT ((0)) FOR [PercentCorrect] " +

" " +
"ALTER TABLE [dbo].[Scripts] ADD  DEFAULT ((0)) FOR [NumberTimesCancelled]  " +

" end " +
"  " +
"declare @myCount int  " +
"set @mycount = (select COUNT(*) from[IdealAutomateDB].[dbo].[Scripts])  " +
"            if @myCount = 0  " +
"            begin  " +
"            INSERT INTO[IdealAutomateDB].[dbo].[Scripts]  " +
"                       ([ScriptName]  " +
"           ,[Executable] )  " +
"     VALUES  " +
"           ('testnotepad'  " +
"           ,'c:\\windows\\system32\\notepad.exe')  " +
"  end  " +

"IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConnectionStrings]') AND type in (N'U')) " +
"BEGIN " +
"CREATE TABLE [dbo].[ConnectionStrings]( " +
"	[tID] [int] IDENTITY(1,1) NOT NULL, " +
"	[ConnectionString] [varchar](500) NULL, " +
" CONSTRAINT [PK_ConnectionStrings] PRIMARY KEY CLUSTERED  " +
"( " +
"	[tID] ASC " +
")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
") ON [PRIMARY] " +
"END " +
"set @mycount = (select COUNT(*) from [IdealAutomateDB].[dbo].[ConnectionStrings])  " +
"            if @myCount = 0  " +
"            begin  " +
"            INSERT INTO [IdealAutomateDB].[dbo].[ConnectionStrings]  " +
"                       ([ConnectionString])  " +
"     VALUES  " +
"           ('Data Source=.\\SQLEXPRESS;Initial Catalog=IdealAutomateDB;Integrated Security=SSPI')  " +
"  end  " +
"IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[KeyValueTable]') AND type in (N'U')) " +
"BEGIN " +
"CREATE TABLE [dbo].[KeyValueTable]( " +
"	[myKey] [varchar](500) NOT NULL, " +
"	[myValue] [varchar](5000) NULL, " +
"	[Description] [varchar](5000) NULL " +
") ON [PRIMARY] " +
"END " +
"IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ColumnProperties]') AND type in (N'U')) " +
"BEGIN " +
"CREATE TABLE [dbo].[ColumnProperties]( " +
"	[FullyQualifiedColumnName] [varchar](500) NOT NULL, " +
"	[InsertSeq] [int] IDENTITY(1,1) NOT NULL, " +
"	[DBName] [varchar](500) NULL, " +
"	[SchemaName] [varchar](500) NULL, " +
"	[TableViewName] [varchar](500) NULL, " +
"	[SQLColumnName] [varchar](500) NULL, " +
"	[NETGetData] [varchar](500) NULL, " +
"	[ColumnOrdinal] [int] NOT NULL, " +
"	[SQLDataType] [varchar](500) NULL, " +
"	[NETDataType] [varchar](500) NULL, " +
"	[NetColumnName] [varchar](500) NULL, " +
"	[Length] [int] NULL, " +
"	[IsNullable] [int] NULL, " +
" CONSTRAINT [PK_ColumnProperties] PRIMARY KEY CLUSTERED  " +
"( " +
"	[ColumnOrdinal] ASC " +
")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
") ON [PRIMARY] " +
"END " +
"  " +
"IF NOT EXISTS ( " +
"		SELECT * " +
"		FROM sys.objects " +
"		WHERE type = 'P' " +
"			AND OBJECT_ID = OBJECT_ID('dbo.SelectValueByKey') " +
"		) " +
"BEGIN " +
"	EXEC ( " +
"			'CREATE PROCEDURE [dbo].[SelectValueByKey] ( " +
"	@myKey  VARCHAR(500) " +
") " +
"AS " +
"BEGIN " +

"	SET NOCOUNT ON; " +

"	select top 1 myValue " +
"from   dbo.KeyValueTable " +
"where  myKey  = @myKey; " +
"END' " +
"			) " +
"END " +
" " +
"IF NOT EXISTS ( " +
"		SELECT * " +
"		FROM sys.objects " +
"		WHERE type = 'P' " +
"			AND OBJECT_ID = OBJECT_ID('dbo.SetValueByKey') " +
"		) " +
"BEGIN " +
"	EXEC ( " +
"			'CREATE PROCEDURE [dbo].[SetValueByKey] ( " +
"	@myKey  VARCHAR(500) " +
"	,@myValue  VARCHAR(500) " +
") " +
"AS " +
"BEGIN " +
"	SET NOCOUNT ON; " +
"	IF  EXISTS (select * " +
"from   dbo.KeyValueTable " +
"where  myKey  = @myKey) " +
"BEGIN " +
"UPDATE [dbo].[KeyValueTable] " +
"   SET [myValue] = @myValue   " +
" WHERE myKey = @myKey " +
" END " +
" ELSE " +
" BEGIN " +
" INSERT INTO [dbo].[KeyValueTable] " +
"           ([myKey] " +
"           ,[myValue] " +
"           ,[Description]) " +
"     VALUES " +
"           (@myKey " +
"           ,@myValue " +
"           ,null) " +
"		   END " +
" " +
"END' " +
"			) " +
"END " +
" ";




      SqlCommand cmd1 = new SqlCommand(updCmd, con);
      cmd1.CommandType = CommandType.Text;
      cmd1.ExecuteNonQuery();
      SqlCommand cmd = new SqlCommand("Select * from UserInfo", con);
      cmd.CommandType = CommandType.Text;
      SqlDataAdapter adapter = new SqlDataAdapter();
      adapter.SelectCommand = cmd;
      DataSet dataSet = new DataSet();
      adapter.Fill(dataSet);
      bool boolLocalUserInfoExists = false;
      if (dataSet.Tables[0].Rows.Count > 0) {
        boolLocalUserInfoExists = true;
        string username = dataSet.Tables[0].Rows[0]["FirstName"].ToString() + " " + dataSet.Tables[0].Rows[0]["LastName"].ToString();
        IAUserInfo.ID = Convert.ToInt32(dataSet.Tables[0].Rows[0]["ID"]);
        IAUserInfo.UserName = dataSet.Tables[0].Rows[0]["UserName"].ToString();
        IAUserInfo.FirstName = dataSet.Tables[0].Rows[0]["FirstName"].ToString();
        IAUserInfo.LastName = dataSet.Tables[0].Rows[0]["LastName"].ToString();
        IAUserInfo.Password = dataSet.Tables[0].Rows[0]["Password"].ToString();
        IAUserInfo.Email = dataSet.Tables[0].Rows[0]["Email"].ToString();
        IAUserInfo.Bio = dataSet.Tables[0].Rows[0]["Bio"].ToString();
        IAUserInfo.Photo = dataSet.Tables[0].Rows[0]["Photo"] as byte[];
        if (dataSet.Tables[0].Rows[0]["Category1"] == null) {
          IAUserInfo.Category1 = "";
        } else {
          IAUserInfo.Category1 = dataSet.Tables[0].Rows[0]["Category1"].ToString();
        }
        if (dataSet.Tables[0].Rows[0]["Category2"] == null) {
          IAUserInfo.Category2 = "";
        } else {
          IAUserInfo.Category2 = dataSet.Tables[0].Rows[0]["Category2"].ToString();
        }
        if (dataSet.Tables[0].Rows[0]["Category3"] == null) {
          IAUserInfo.Category3 = "";
        } else {
          IAUserInfo.Category3 = dataSet.Tables[0].Rows[0]["Category3"].ToString();
        }
        if (dataSet.Tables[0].Rows[0]["Category4"] == null) {
          IAUserInfo.Category4 = "";
        } else {
          IAUserInfo.Category4 = dataSet.Tables[0].Rows[0]["Category4"].ToString();
        }
        if (dataSet.Tables[0].Rows[0]["Category5"] == null) {
          IAUserInfo.Category5 = "";
        } else {
          IAUserInfo.Category5 = dataSet.Tables[0].Rows[0]["Category5"].ToString();
        }
        if (dataSet.Tables[0].Rows[0]["TextSearch"] == null) {
          IAUserInfo.TextSearch = "";
        } else {
          IAUserInfo.TextSearch = dataSet.Tables[0].Rows[0]["TextSearch"].ToString();
        }

        if (dataSet.Tables[0].Rows[0]["PurchaseDate"] == null || dataSet.Tables[0].Rows[0]["PurchaseDate"].ToString() == "") {
          IAUserInfo.PurchaseDate = null;
        } else {
          IAUserInfo.PurchaseDate = Convert.ToDateTime(dataSet.Tables[0].Rows[0]["PurchaseDate"].ToString());
        }
        if (dataSet.Tables[0].Rows[0]["PurchaseDuration"] == null) {
          IAUserInfo.PurchaseDuration = "";
        } else {
          IAUserInfo.PurchaseDuration = dataSet.Tables[0].Rows[0]["PurchaseDuration"].ToString();
        }
        if (dataSet.Tables[0].Rows[0]["RequireLogin"].ToString() == "True") {
          IAUserInfo.RequireLogin = true;
        } else {
          IAUserInfo.RequireLogin = false;
        }


      }



      con.Close();

      _MainWindow.Show();
      Close();

    }



  }
}
