using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace CreatePopupGuideTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
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
            if (strWindowTitle.StartsWith("CreatePopupGuideTest"))
            {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            myActions.Sleep(1000);

            int intRowCtr = 0;
            ControlEntity myControlEntity = new ControlEntity();
            byte[] mybytearray;
            ImageEntity myImage = new ImageEntity();
            System.Drawing.Bitmap bm;
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();
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
            myImage.Attempts = 1;
            myImage.RelativeX = 0;
            myImage.RelativeY = 0;

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
                bm = Methods.BytesToBitmap(mybytearray);
                myControlEntity.Width = bm.Width;
                myControlEntity.Height = bm.Height;
                myControlEntity.Source = Methods.BitmapSourceFromImage(bm);


                myControlEntity.ControlType = ControlType.Image;
                myControlEntity.ID = "myImage";
                myControlEntity.RowNumber = 4;
                myControlEntity.ColumnNumber = 0;

                myListControlEntity.Add(myControlEntity.CreateControlEntity());

               myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
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
            string strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 160, 1000, newTop, newLeft, "DEFAULT");
            if (strButtonPressed == "btnCancel")
            {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                goto myExit;
            }




        myExit:
            myActions.ScriptEndedSuccessfullyUpdateStats();
            Application.Current.Shutdown();
        }

        //private static BitmapSource BitmapSourceFromImage(System.Drawing.Image img)
        //{
        //    MemoryStream memStream = new MemoryStream();

        //    // save the image to memStream as a png
        //    img.Save(memStream, System.Drawing.Imaging.ImageFormat.Png);

        //    // gets a decoder from this stream
        //    System.Windows.Media.Imaging.PngBitmapDecoder decoder = new System.Windows.Media.Imaging.PngBitmapDecoder(memStream, System.Windows.Media.Imaging.BitmapCreateOptions.PreservePixelFormat, System.Windows.Media.Imaging.BitmapCacheOption.Default);

        //    return decoder.Frames[0];
        //}
        //private static System.Drawing.Bitmap BytesToBitmap(byte[] byteArray)
        //{


        //    using (MemoryStream ms = new MemoryStream(byteArray))
        //    {


        //        System.Drawing.Bitmap img = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(ms);


        //        return img;


        //    }
        //}
    }
}
