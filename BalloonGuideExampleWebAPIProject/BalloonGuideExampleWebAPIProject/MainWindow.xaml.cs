using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace BalloonGuideExampleWebAPIProject {
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
      myActions.ScriptStartedUpdateStats();

      InitializeComponent();
      this.Hide();

      string strWindowTitle = myActions.PutWindowTitleInEntity();
      if (strWindowTitle.StartsWith("BalloonGuideExampleWebAPIProject")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(1000);
			int intRowCtr = 0;
			ControlEntity myControlEntity = new ControlEntity();
			byte[] mybytearray;
			System.Drawing.Bitmap bm;
			string strButtonPressed = "";
			List<ControlEntity> myListControlEntity = new List<ControlEntity>();
			List<ComboBoxPair> cbp = new List<ComboBoxPair>();
			ImageEntity myImage;
			//goto WebApiConfigRegister;

			myActions.PutEntityInClipboard("Visual Studio 2019");
			// =======
			myListControlEntity = new List<ControlEntity>();
			cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblhead";
			myControlEntity.Text = "Find Visual Studio on Your Computer";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lbllabel";
			myControlEntity.Text = "" +
"Click on search icon in taskbar. \r\n When input searchbox opens, click on it and press ctrl-v to paste Visual Studio 2019 in to search box. \r\nPress enter to launch Visual Studio \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 640;
			myControlEntity.Height = 50;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());



			myImage = new ImageEntity();


		//	myImage.ImageFile = @"C:\Data\Images\VisualStudioTaskbar1.png";
			myImage.ImageFile = @"C:\Data\Images\TaskbarSearch.png";

			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;
			myImage.Tolerance = 95;

			int[,] resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			int newTop = 0;
			int newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("0", out intRelativeTop);
				Int32.TryParse("25", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 200, 1000, newTop, newLeft, "DEFAULT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			
			// =========

			//========
			myImage = new ImageEntity();

			myListControlEntity = new List<ControlEntity>();
			cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "What is WebApi?";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lblLabel";
			myControlEntity.Text = "" +
" A WebApi allows multiple applications to share a set of actions that all of them need to perform - commonly related to getting and updating info on a database. The WebApi is exposed to the client applications through an Http URL address, like http://localhost:1234/api/values \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 826;
			myControlEntity.Height = 100;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Image;
			myControlEntity.ID = "imgWebAPIOverview";

			myControlEntity.ToolTipx = "";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;

			mybytearray = System.IO.File.ReadAllBytes(@"C:\Data\Images\WebAPIOverview.png");
			bm = BytesToBitmap(mybytearray);
			myControlEntity.Width = bm.Width;
			myControlEntity.Height = bm.Height;
			myControlEntity.Source = BitmapSourceFromImage(bm);
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 700, 1000, 0, 0, "NONE");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}
			// =========
			myImage = new ImageEntity();
			
			 myListControlEntity = new List<ControlEntity>();
			 cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "Localhost Results from ValuesController";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Image;
			myControlEntity.ID = "imglocalhostResults";

			myControlEntity.ToolTipx = "";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;

			mybytearray = System.IO.File.ReadAllBytes(@"C:\Data\Images\localhostResults.png");
			bm = BytesToBitmap(mybytearray);
			myControlEntity.Width = 891;
			myControlEntity.Height = 182;
			myControlEntity.Source = BitmapSourceFromImage(bm);
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lbllabel";
			myControlEntity.Text = "" +
"Here are the results we are attempting to get when we enter the url for /api/values into our browser. \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 891;
			myControlEntity.Height = 30;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 322, 1000, 261, 242, "NONE");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			// =========
			myListControlEntity = new List<ControlEntity>();
			cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "File New Project";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lbllabel";
			myControlEntity.Text = "" +
"Click on Create New Project \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 216;
			myControlEntity.Height = 30;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			 myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\CreateANewProject.PNG";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

			resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			 newTop = 0;
			newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("0", out intRelativeTop);
				Int32.TryParse("0", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 160, 500, newTop, newLeft, "DEFAULT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}
			myImage = new ImageEntity();
			CreateProjectPageNotLoaded:

			myImage.ImageFile = @"C:\Data\Images\CreateProjectPageNotLoaded.PNG";
			myImage.Sleep = 1000;
			myImage.Attempts = 1;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;
			myImage.Tolerance = 99;
			


			resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			if (resultArray.Length != 0)
            {
				goto CreateProjectPageNotLoaded;
            }
			// ========
			myImage = new ImageEntity();
			myImage.ImageFile = @"C:\Data\Images\CreateANewProjectClearAll.PNG";
			myImage.Sleep = 1000;
			myImage.Attempts = 2;
			myImage.RelativeX = 10;
			myImage.RelativeY = 10;
			myImage.Tolerance = 99;



			myActions.ClickImageIfExists(myImage);

			// ========
			myListControlEntity = new List<ControlEntity>();
			 cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "3. Select Language C#";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Image;
			myControlEntity.ID = "img";

			myControlEntity.ToolTipx = "Select All Languages";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 1;

			mybytearray = System.IO.File.ReadAllBytes(@"C:\Data\Images\AllLanguages.png");
			bm = BytesToBitmap(mybytearray);
			myControlEntity.Width = 161;
			myControlEntity.Height = 18;
			myControlEntity.Source = BitmapSourceFromImage(bm);
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\AllLanguages.png";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

		resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			 newTop = 0;
			newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("0", out intRelativeTop);
				Int32.TryParse("0", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			 strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 128, 500, newTop, newLeft, "DEFAULT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			myActions.PutEntityInClipboard("ASP.NET Web Application");
			//=======

			myListControlEntity = new List<ControlEntity>();
			cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "4. Search For Templates";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lbllabel";
			myControlEntity.Text = "" +
"ASP.NET Web Application is in your clipboard - paste into searchbox \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 224;
			myControlEntity.Height = 30;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\SearchForTemplates.png";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

			 resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			newTop = 0;
			newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("20", out intRelativeTop);
				Int32.TryParse("0", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 160, 500, newTop, newLeft, "DEFAULT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}



			// ========



			myListControlEntity = new List<ControlEntity>();
		 cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "5. Select ASP.NET Web Application";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
		myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\ASPNETWebApplication.png";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

		resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			newTop = 0;
			newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("25", out intRelativeTop);
				Int32.TryParse("25", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 110, 500, newTop, newLeft, "DEFAULT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			//=====
			myActions.PutEntityInClipboard("WebAPIDemo");
			myListControlEntity = new List<ControlEntity>();
			 cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblhead";
			myControlEntity.Text = "6. Enter Project Name WebAPIDemo";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lbllabel";
			myControlEntity.Text = "" +
"WebAPIDemo is in your clipboard so you can just paste it in project name  \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 512;
			myControlEntity.Height = 30;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			 myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\ProjectName.png";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

			resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			 newTop = 0;
			 newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("35", out intRelativeTop);
				Int32.TryParse("25", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			 strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 160, 1000, newTop, newLeft, "DEFAULT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			// =====
 myListControlEntity = new List<ControlEntity>();
 cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "7. Click Create";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\CreateButton.png";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
newTop = 0;
 newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("0", out intRelativeTop);
				Int32.TryParse("0", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
		    strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 110, 500, newTop, newLeft, "DEFAULT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}
			// ========
			myImage = new ImageEntity();
			myImage.ImageFile = @"C:\Data\Images\ProjectTypeEmpty.PNG";
			myImage.Sleep = 1000;
			myImage.Attempts = 2;
			myImage.RelativeX = 10;
			myImage.RelativeY = 10;
			//myImage.Tolerance = 99;
			myActions.ClickImageIfExists(myImage);
			//=====
			myListControlEntity = new List<ControlEntity>();
		 cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "Select Web API";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\WebAPIProjectType.png";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;
			myImage.Tolerance = 97;

		resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
		 newTop = 0;
		newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("0", out intRelativeTop);
				Int32.TryParse("0", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 110, 500, newTop, newLeft, "DEFAULT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			// =====
			myListControlEntity = new List<ControlEntity>();
			cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "Click Create";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\CreateButton2.png";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

		resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			newTop = 0;
			newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("0", out intRelativeTop);
				Int32.TryParse("0", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 110, 500, newTop, newLeft, "DEFAULT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			//=======

			myListControlEntity = new List<ControlEntity>();
			 cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "Explore Auto-Generated Code";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lbllabel";
			myControlEntity.Text = "" +
"Once the project is created, we will explore the auto-generated code. If you have worked with MVC, the file structure will look familiar. See image below to start reviewing general file structure of project. \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 640;
			myControlEntity.Height = 50;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Image;
			myControlEntity.ID = "imgFileStructure";

			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 1;

			mybytearray = System.IO.File.ReadAllBytes(@"C:\Data\Images\FileStructureWebAPI.png");
			bm = BytesToBitmap(mybytearray);
			myControlEntity.Width = 247;
			myControlEntity.Height = 339;
			myControlEntity.Source = BitmapSourceFromImage(bm);
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lbllabel2";
			myControlEntity.Text = "" +
"Once project is created, click okay to start exploring. \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 640;
			myControlEntity.Height = 30;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			 strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 529, 1000, 36, 43, "NONE");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			// ======
			myListControlEntity = new List<ControlEntity>();
			 cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "Expand Controllers Folder";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lbllabel1";
			myControlEntity.Text = "" +
"Click on arrow to left of Controllers folder to expand the folder. \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 528;
			myControlEntity.Height = 30;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\ControllersFolderArrow.png";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

			resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			newTop = 0;
			newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("0", out intRelativeTop);
				Int32.TryParse("0", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			 strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 160, 1000, newTop, newLeft, "DEFAULT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			//=========
			myListControlEntity = new List<ControlEntity>();
			cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "Click on Values Controller";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lbllabel";
			myControlEntity.Text = "" +
"Click on ValuesController.cs to view code \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 328;
			myControlEntity.Height = 30;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\ValuesController.png";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

			resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			newTop = 0;
			newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("0", out intRelativeTop);
				Int32.TryParse("0", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 160, 656, newTop, newLeft, "DEFAULT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			//========
			myListControlEntity = new List<ControlEntity>();
			cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "Comparing API Controller to MVC";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lbllabel";
			myControlEntity.Text = "" +
"Notice that the ValuesController inherits from APIController class that is present in System.Web.Http namespace. This is different from the MVC controller. The MVC Controller class inherits from the Controller class that is present in System.Web.Mvc namespace. The HomeController class, which is an MVC controller, inherits from the Controller class. \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 640;
			myControlEntity.Height = 125;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\ValuesControllerCompare.png";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

			resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			 newTop = 0;
			newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("10", out intRelativeTop);
				Int32.TryParse("0", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			 strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 235, 1000, newTop, newLeft, "DEFAULT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			// ====== Compare Web Api to MVC
			myListControlEntity = new List<ControlEntity>();
			 cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "Compare Web Api to MVC";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Image;
			myControlEntity.ID = "imgCompareAPIToMVC";

			myControlEntity.ToolTipx = "";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;

			mybytearray = System.IO.File.ReadAllBytes(@"C:\Data\Images\CompareWebApiToMVC.png");
			bm = BytesToBitmap(mybytearray);
			myControlEntity.Width = 443;
			myControlEntity.Height = 287;
			myControlEntity.Source = BitmapSourceFromImage(bm);
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 397, 886, 142, 227, "NONE");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			// ==== Example of MVC Controller calling a Web API controller
			myImage = new ImageEntity();
			myListControlEntity = new List<ControlEntity>();
			cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "MVC Controller Calling Web API Controller";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Image;
			myControlEntity.ID = "imgMVCCallingWebAPI";

			myControlEntity.ToolTipx = "";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;

			mybytearray = System.IO.File.ReadAllBytes(@"C:\Data\Images\MVCCallingWebAPI.png");
			bm = BytesToBitmap(mybytearray);
			myControlEntity.Width = 878;
			myControlEntity.Height = 443;
			myControlEntity.Source = BitmapSourceFromImage(bm);
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lblLabel";
			myControlEntity.Text = "" +
"This shows how an MVC Controller can have a call to a Web API project to perform read and write actions to the database. \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 878;
			myControlEntity.Height = 30;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 583, 1000, 67, 151, "NONE");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			// ======
			intRowCtr = 0;
			 myControlEntity = new ControlEntity();
			 myImage = new ImageEntity();
			
			 myListControlEntity = new List<ControlEntity>();
			 cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "Collapse to Definitions";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lblLabel";
			myControlEntity.Text = "" +
"Right-click in code window. In the context menu that appears, select outlining. Then select Collapse to Definitions \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 640;
			myControlEntity.Height = 30;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\Collapse.png";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

			resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			newTop = 0;
			newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("10", out intRelativeTop);
				Int32.TryParse("0", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			 strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 160, 1000, newTop, newLeft, "DEFAULT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}


			// =====
			myListControlEntity = new List<ControlEntity>();
			 cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "Methods in ValuesController Map to Http verbs";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lbllabel";
			myControlEntity.Text = "" +
"Notice in the ValuesController class we have methods (Get, Put, Post & Delete) that map to the HTTP verbs (GET, PUT, POST, DELETE) respectively. We have 2 overloaded versions of Get() method - One without any parameters and the other with id parameter. Both of these methods respond to the GET http verb depending on whether the id parameter is specified in the URI or not. \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 640;
			myControlEntity.Height = 125;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			 myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\ValuesControllerpng";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

			resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			 newTop = 0;
			 newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("10", out intRelativeTop);
				Int32.TryParse("0", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			 strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 235, 1000, newTop, newLeft, "DEFAULT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			// =====
			intRowCtr = 0;
			myControlEntity = new ControlEntity();
			
			myImage = new ImageEntity();
			
			 myListControlEntity = new List<ControlEntity>();
			cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "Default Route";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lblLabel";
			myControlEntity.Text = "" +
"Now let's look at the default route that is in place for our Web API project. Click on the Global.asax file to view the source code.\r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 640;
			myControlEntity.Height = 200;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\Globalasax.png";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

			resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			 newTop = 0;
			 newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("10", out intRelativeTop);
				Int32.TryParse("0", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 310, 1000, newTop, newLeft, "DEFAULT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			// ======
			intRowCtr = 0;
			 myControlEntity = new ControlEntity();
			 myImage = new ImageEntity();
			
			 myListControlEntity = new List<ControlEntity>();
			 cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lbl";
			myControlEntity.Text = "Application_Start Event";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lblLabel";
			myControlEntity.Text = "" +
"The Application_Start event is fired when the application starts. Notice that in the Application_Start() event handler method, we have configuration for Filters, Bundles etc. \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 640;
			myControlEntity.Height = 50;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\ApplicationStartEvent.png";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

			resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			newTop = 0;
			newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("10", out intRelativeTop);
				Int32.TryParse("0", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 160, 1000, newTop, newLeft, "BOTTOM_LEFT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			// =====
			WebApiConfigRegister:
			intRowCtr = 0;
			myControlEntity = new ControlEntity();
			
			myImage = new ImageEntity();			
			 myListControlEntity = new List<ControlEntity>();
			 cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "WebApiConfig.Register Method";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lblLabel";
			myControlEntity.Text = "" +
"We are interested in the configuration for our Web API project. Right-click on WebApiConfig.Register and select \"Go To Definition\" from the context menu. This will take you to the Register() method in the WebApiConfig class. This class is in App_Start folder. \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 640;
			myControlEntity.Height = 75;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\WebAPIRegisterGoToDefinition.PNG";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

			resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			newTop = 0;
			newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("10", out intRelativeTop);
				Int32.TryParse("100", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 185, 1000, newTop, newLeft, "BOTTOM_LEFT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			// ======
			 intRowCtr = 0;
			 myControlEntity = new ControlEntity();
			
			 myImage = new ImageEntity();
			
			 myListControlEntity = new List<ControlEntity>();
			 cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lbl";
			myControlEntity.Text = "Register Method";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lblLabel";
			myControlEntity.Text = "" +
"In the Register() method we have the default route configured for our Web API project. Web API routes are different from the MVC routes. You can find the MVC routes in RouteConfig.cs file in App_Start folder. \r\n" +
" \r\n" +
"The default Web API route starts with the word api and then / and then the name of the controller and another / and an optiontion id parameter. \r\n" +
"\"api/{controller}/{id}\" \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 640;
			myControlEntity.Height = 200;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\Register.png";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

			resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			newTop = 0;
			 newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("10", out intRelativeTop);
				Int32.TryParse("0", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			 strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 310, 1000, newTop, newLeft, "BOTTOM_LEFT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			// =====
			 intRowCtr = 0;
			 myControlEntity = new ControlEntity();
			
			 myImage = new ImageEntity();
			
			 myListControlEntity = new List<ControlEntity>();
			 cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "Run Application in IIS";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lblLabel";
			myControlEntity.Text = "" +
"IIS, or Internet Information Services, can be used to host Web API projects. Click on IIS Express (Google Chrome) to launch the application in google chrome. \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 640;
			myControlEntity.Height = 40;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\IISExpress.png";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

			 resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			newTop = 0;
			 newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("10", out intRelativeTop);
				Int32.TryParse("25", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			 strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 160, 1000, newTop, newLeft, "DEFAULT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}
			// =====
			myImage = new ImageEntity();
			
			 myListControlEntity = new List<ControlEntity>();
			 cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "IIS WebAPI Diagram";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Image;
			myControlEntity.ID = "imgIISImage";

			myControlEntity.ToolTipx = "";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;

			mybytearray = System.IO.File.ReadAllBytes(@"C:\Data\Images\IISWebApiDiagram.png");
			bm = BytesToBitmap(mybytearray);
			myControlEntity.Width = 830;
			myControlEntity.Height = 182;
			myControlEntity.Source = BitmapSourceFromImage(bm);
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 292, 1000, 281, 276, "NONE");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			// =====
			intRowCtr = 0;
			 myControlEntity = new ControlEntity();
			
			 myImage = new ImageEntity();
			
			myListControlEntity = new List<ControlEntity>();
			 cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblHead";
			myControlEntity.Text = "Test the ValuesController";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lblLabel";
			myControlEntity.Text = "" +
"To test the Values controller, add /api/values to the localhost url and port number that appeared in the address bar and hit enter. \r\n";
			myControlEntity.ToolTipx = "";
			myControlEntity.FontSize = 14;
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.Width = 640;
			myControlEntity.Height = 30;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 2;
			myControlEntity.Multiline = true;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			myImage = new ImageEntity();


			myImage.ImageFile = @"C:\Data\Images\localhost.png";
			myImage.Sleep = 1000;
			myImage.Attempts = 60;
			myImage.RelativeX = 0;
			myImage.RelativeY = 0;

			resultArray = myActions.PutAllFastByStoppingOnPerfectMatch(myImage);
			 newTop = 0;
			 newLeft = 0;
			if (resultArray.Length == 0)
			{
				List<ControlEntity> myListControlEntityBackup = myListControlEntity;
				myListControlEntity = new List<ControlEntity>();

				myControlEntity = new ControlEntity();
				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Heading;
				myControlEntity.Text = "Image Not Found";
				myListControlEntity.Add(myControlEntity.CreateControlEntity());


				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "I could not find image to position popup relative to ";
				myControlEntity.RowNumber = 0;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();
				myControlEntity.ControlType = ControlType.Label;
				myControlEntity.ID = "myLabel";
				myControlEntity.Text = "Here is what that image looks like:";
				myControlEntity.RowNumber = 2;
				myControlEntity.ColumnNumber = 0;
				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				myControlEntity.ControlEntitySetDefaults();

				mybytearray = System.IO.File.ReadAllBytes(myImage.ImageFile);
				bm = BytesToBitmap(mybytearray);
				myControlEntity.Width = bm.Width;
				myControlEntity.Height = bm.Height;
				myControlEntity.Source = BitmapSourceFromImage(bm);


				myControlEntity.ControlType = ControlType.Image;
				myControlEntity.ID = "myImage";
				myControlEntity.RowNumber = 4;
				myControlEntity.ColumnNumber = 0;

				myListControlEntity.Add(myControlEntity.CreateControlEntity());

				strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
				myListControlEntity = myListControlEntityBackup;
			}
			else
			{
				newLeft = resultArray[0, 0];
				newTop = resultArray[0, 1];
				int intRelativeTop = 0;
				int intRelativeLeft = 0;
				Int32.TryParse("10", out intRelativeTop);
				Int32.TryParse("25", out intRelativeLeft);
				newTop += intRelativeTop;
				newLeft += intRelativeLeft;
			}
			 strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 160, 1000, newTop, newLeft, "DEFAULT");
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}






		myExit:
      myActions.ScriptEndedSuccessfullyUpdateStats();
      Application.Current.Shutdown();
    }
		private static BitmapSource BitmapSourceFromImage(System.Drawing.Image img)
		{
			MemoryStream memStream = new MemoryStream();
			// save the image to memStream as a png
			img.Save(memStream, System.Drawing.Imaging.ImageFormat.Png);
			// gets a decoder from this stream
			System.Windows.Media.Imaging.PngBitmapDecoder decoder = new System.Windows.Media.Imaging.PngBitmapDecoder(memStream, System.Windows.Media.Imaging.BitmapCreateOptions.PreservePixelFormat, System.Windows.Media.Imaging.BitmapCacheOption.Default);
			return decoder.Frames[0];
		}
		private static System.Drawing.Bitmap BytesToBitmap(byte[] byteArray)
		{
			using (MemoryStream ms = new MemoryStream(byteArray))
			{
				System.Drawing.Bitmap img = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(ms);
				return img;
			}
		}
	}
}
