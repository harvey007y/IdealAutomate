using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Reflection;



namespace First
{
    public class Program : Window
    {

        static IdealAutomate.Core.Methods myActions = new Methods();

        static byte[] mybytearray;
        static ImageEntity myImage = new ImageEntity();
        static System.Drawing.Bitmap bm;
        static List<ControlEntity> myListControlEntity = new List<ControlEntity>();
        static List<ComboBoxPair> cbp = new List<ComboBoxPair>();
        static int intRowCtr = 0;
        static ControlEntity myControlEntity = new ControlEntity();
        static string strButtonPressed = "";
        static int[,] resultArray = new int[100, 100];
        static int newTop = 0;
        static int newLeft = 0;

        public static void Main()
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


            strButtonPressed = myActions.WindowBalloonMultipleControls(ref myListControlEntity, 140, 500, 0, 0, "NONE");
            if (strButtonPressed == "btnCancel")
            {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                goto myExit;
            }
        }
        myExit:
Console.WriteLine("Script Ended");
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
    } }