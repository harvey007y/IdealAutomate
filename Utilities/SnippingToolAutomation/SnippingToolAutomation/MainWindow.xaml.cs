using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using Snipping_OCR;
using System.Windows.Media.Imaging;
using System.IO;

namespace SnippingToolAutomation {
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
      if (strWindowTitle.StartsWith("SnippingToolAutomation")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      StartAgain:
      myActions.Sleep(1000);
            //myActions.Run(@"C:\WINDOWS\system32\SnippingTool.exe", "");
            //myActions.Sleep(1000);
            //myActions.TypeText("%(n)", 1000);
            //myActions.TypeText("{ESC}", 1000);
            //myActions.TypeText("%(\" \")n", 1000);


            int intRowCtr = 0;
            ControlEntity myControlEntity = new ControlEntity();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btn";
            myControlEntity.Text = "Snip";
            myControlEntity.ColumnSpan = 0;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btn";
            myControlEntity.Text = "Exit";
            myControlEntity.ColumnSpan = 0;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());
            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 200, 500, 0, 0);
            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                goto myExit;
            }
            SnippingTool.Snip();

            if (SnippingTool.Image != null) {
                Clipboard.SetImage(BitmapSourceFromImage(SnippingTool.Image));
            }
            myActions.Sleep(15000);
            goto myExit;
            myActions.TypeText("^({PRTSC})", 1000);
            myActions.MessageBoxShow("click okay to continue");
            myActions.TypeText("^(c)", 500);
           
 
            
            myActions.TypeText("^(c)", 500);
            myActions.TypeText("%(\" \")n", 1000);

            myActions.Sleep(15000);
            goto myExit;
           myExit:
      myActions.ScriptEndedSuccessfullyUpdateStats();
      Application.Current.Shutdown();
    }
        private static BitmapSource BitmapSourceFromImage(System.Drawing.Image img) {
            MemoryStream memStream = new MemoryStream();

            // save the image to memStream as a png
            img.Save(memStream, System.Drawing.Imaging.ImageFormat.Png);

            // gets a decoder from this stream
            System.Windows.Media.Imaging.PngBitmapDecoder decoder = new System.Windows.Media.Imaging.PngBitmapDecoder(memStream, System.Windows.Media.Imaging.BitmapCreateOptions.PreservePixelFormat, System.Windows.Media.Imaging.BitmapCacheOption.Default);

            return decoder.Frames[0];
        }
    }
}
