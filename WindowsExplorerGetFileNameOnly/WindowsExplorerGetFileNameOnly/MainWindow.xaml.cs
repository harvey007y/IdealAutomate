using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Drawing;
using System.IO;


namespace WindowsExplorerGetFileNameOnly
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
            if (strWindowTitle.StartsWith("WindowsExplorerGetFileNameOnly") || strWindowTitle.StartsWith("Ideal Automate"))
            {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            myActions.Sleep(1000);
            string strActiveTitle = myActions.GetActiveWindowTitle();
          //  myActions.MessageBoxShow(strActiveTitle);
            myActions.ActivateWindowByTitle("WindowMultipleControls", 2);
            myActions.ActivateWindowByTitle(strActiveTitle, 3);

            myActions.TypeText("+({F10})", 500); // right-click filename in windows explorer
            myActions.TypeText("{UP}", 500);  // highlight properties
            myActions.TypeText("+({ENTER})", 500); // select properties
            myActions.TypeText("+({TAB 2})", 500); // focus filename
            myActions.SelectAllCopy(500); // Copy filename
            myActions.TypeText("+({TAB 2})", 500); // focus cancel button
            myActions.TypeText("{ENTER}", 500); // close properties window
            ImageEntity myImage = new ImageEntity();
            myImage.ImageFile = "Images\\imgName.PNG";
            myImage.Sleep = 1700;
            myImage.Attempts = 2;
            myImage.RelativeX = 300;
            myImage.RelativeY = -25;
            int[,] myArray = myActions.PutAll(myImage);
            if (myArray.Length == 0)
            {
                myActions.MessageBoxShow("Could not find imgName - click here to continue");
            }
            else {
                myActions.LeftClick(myArray);
            }
            myActions.TypeText("%(d)", 500); // highlight windows explorer address bar
            myActions.TypeText("{END}", 500); // close properties window
            myActions.TypeText("\\", 500); // type slash
            string strFileNameOnly = myActions.PutClipboardInEntity();
            myActions.TypeText("^(v)", 500); // paste filename
            myActions.SelectAllCopy(500); // Copy full filename
            string strFullFileName = myActions.PutClipboardInEntity();
            Bitmap img = new Bitmap(strFullFileName);
          
            var imageHeight = img.Height;
            var imageWidth = img.Width;
           // System.Drawing.Image img = System.Drawing.Image.FromFile(strFullFileName);
           // MessageBox.Show("Width: " + img.Width + ", Height: " + img.Height);
            myActions.ActivateWindowByTitle("WindowMultipleControls", 3);
            myActions.TypeText("{TAB 3}", 1500);
            myActions.TypeText(strFileNameOnly, 500);
            myActions.TypeText("{TAB 2}", 500);
            myActions.TypeText(img.Height.ToString(), 500);
            myActions.TypeText("{TAB}", 500);
            myActions.TypeText(img.Width.ToString(), 500);
            string strFromPath = @"C:\Users\Rebecca\Downloads\";
            string strToPath = @"C:\Users\Rebecca\Documents\IdealProgrammerAptHost\public_html\wp-photos\";
            string strFromFullFile = strFromPath + strFileNameOnly;
            string strToFullFile = strToPath + strFileNameOnly;
            File.Copy(strFromFullFile, strToFullFile, true);
            myExit:
            myActions.ScriptEndedSuccessfullyUpdateStats();
            Application.Current.Shutdown();
        }
    }
}