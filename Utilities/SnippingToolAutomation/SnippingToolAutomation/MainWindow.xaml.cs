using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using Snipping_OCR;
using System.Windows.Media.Imaging;
using System.IO;
using System;
using System.Windows.Forms;
using System.ComponentModel;

using System.Runtime.InteropServices;
using EventHook;

namespace SnippingToolAutomation {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private readonly MouseWatcher mouseWatcher;
        private readonly EventHookFactory eventHookFactory = new EventHookFactory();

        public MainWindow() {
            bool boolRunningFromHome = false;


            bool _mouseUp = false;
            MouseHook mh;
            var window = new Window() //make sure the window is invisible
      {
                Width = 0,
                Height = 0,
                Left = -2000,
                WindowStyle = WindowStyle.None,
                ShowInTaskbar = false,
                ShowActivated = false,
            };
            //var hWnd = new WindowInteropHelper(window).EnsureHandle();
            ////WindowInteropHelper windowHwnd = new WindowInteropHelper(this);
            ////IntPtr hWnd = windowHwnd.Handle;
            //HwndSource source = HwndSource.FromHwnd(hWnd);
            //source.AddHook(new HwndSourceHook(WndProc));
            window.Show();
            IdealAutomate.Core.Methods myActions = new Methods();
            myActions.ScriptStartedUpdateStats();

            InitializeComponent();

            this.Hide();

            string strWindowTitle = myActions.PutWindowTitleInEntity();
            if (strWindowTitle.StartsWith("SnippingToolAutomation")) {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            snipDialog:

            myActions.Sleep(1000);
            myActions.Run(@"C:\WINDOWS\system32\SnippingTool.exe", "");
            myActions.Sleep(1000);
            myActions.TypeText("%(n)", 1000);
            myActions.TypeText("{ESC}", 1000);
            myActions.TypeText("%(\" \")n", 1000);


            int intRowCtr = 0;
            ControlEntity myControlEntity = new ControlEntity();
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();
            List<ComboBoxPair> cbp = new List<ComboBoxPair>();

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnSnipWithComments";
            myControlEntity.Text = "Snip w/ Comments";
            myControlEntity.ColumnSpan = 0;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnSnip";
            myControlEntity.Text = "Snip";
            myControlEntity.ColumnSpan = 0;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnExit";
            myControlEntity.Text = "Exit";
            myControlEntity.ColumnSpan = 0;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 200, 200, 500, 1150);
            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                goto myExit;
            }
            if (strButtonPressed == "btnExit") {
                // myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                goto myExit;
            }
            bool snipWithComments = false;
            if (strButtonPressed == "btnSnipWithComments") {
                snipWithComments = true;
            }
            //SnippingTool.Snip();

            //if (SnippingTool.Image != null) {
            //    Clipboard.SetImage(BitmapSourceFromImage(SnippingTool.Image));
            //}
            myActions.TypeText("^({PRTSC})", 1000);


            myActions.SetValueByKey("Mouseup", "false");
            //mh = new MouseHook();
            //mh.Install();
            intRowCtr = 0;
            myControlEntity = new ControlEntity();
            myListControlEntity = new List<ControlEntity>();
            cbp = new List<ComboBoxPair>();

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnDoneSnipping";
            myControlEntity.Text = "Done Snipping";
            myControlEntity.ColumnSpan = 0;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnAddComments";
            myControlEntity.Text = "Add Comments";
            myControlEntity.ColumnSpan = 0;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Button;
            myControlEntity.ID = "btnExit";
            myControlEntity.Text = "Exit";
            myControlEntity.ColumnSpan = 0;
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 200, 200, 500, 1150);
            if (strButtonPressed == "btnCancel") {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                goto myExit;
            }
            if (strButtonPressed == "btnExit") {
                // myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                goto myExit;
            }

            if (strButtonPressed == "btnAddComments") {
                snipWithComments = true;
            }

            myActions.ActivateWindowByTitle("Snipping Tool");
            myActions.TypeText("^(c)", 500);
            //mh.MouseMove += new MouseHook.MouseHookCallback(mouseHook_MouseMove);
            string mouseUp = "false";
            //mouseWatcher = eventHookFactory.GetMouseWatcher();
            //mouseWatcher.Start();
            //mouseWatcher.OnMouseInput += (s, e) => {
            //    if (e.Message.ToString() == "WM_LBUTTONUP") {
            //        myActions.SetValueByKey("MouseUp", "true");
            //    }
            //};
            //while (mouseUp == "false") {
            //    myActions.TypeText("^(c)", 500);
            //    mouseUp = myActions.GetValueByKey("MouseUp");

            // //   myActions.Sleep(5000);
            //}
            string strComments = "";
            if (snipWithComments) {
                intRowCtr = 0;
                myControlEntity = new ControlEntity();
                myListControlEntity = new List<ControlEntity>();
                cbp = new List<ComboBoxPair>();
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Heading;
                myControlEntity.ID = "lbl";
                myControlEntity.Text = "Comments";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                intRowCtr++;
                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.Label;
                myControlEntity.ID = "lblComments";
                myControlEntity.Text = "Comments";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.ColumnNumber = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());

                myControlEntity.ControlEntitySetDefaults();
                myControlEntity.ControlType = ControlType.TextBox;
                myControlEntity.ID = "txtComments";
                myControlEntity.Text = ""; //myActions.GetValueByKey("Comments"); ;
                myControlEntity.ToolTipx = "";
                myControlEntity.RowNumber = intRowCtr;
                myControlEntity.Width = 800;
                myControlEntity.Height = 300;
                myControlEntity.Multiline = true;
                myControlEntity.ColumnNumber = 1;
                myControlEntity.ColumnSpan = 0;
                myListControlEntity.Add(myControlEntity.CreateControlEntity());
                strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 800, 0, 0);
                if (strButtonPressed == "btnCancel") {
                    myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                    goto myExit;
                }

                strComments = myListControlEntity.Find(x => x.ID == "txtComments").Text;
                myActions.SetValueByKey("Comments", strComments);
            }
            myActions.TypeText("^(c)", 500);
            myActions.TypeText("%(\" \")n", 1000);
            List<string> myWindowTitles = myActions.GetWindowTitlesByProcessName("wordpad");
            myWindowTitles.RemoveAll(item => item == "");
            if (myWindowTitles.Count > 0) {
                myActions.ActivateWindowByTitle(myWindowTitles[0]);
                if (strComments != "") {
                    myActions.TypeText(strComments, 1000);
                }
                myActions.TypeText("^(v)", 1000);
                myActions.TypeText("{ENTER}", 1000);

            }
            myActions.TypeText("%(\" \")n", 500);
            goto snipDialog;
            myActions.TypeText("^({PRTSC})", 1000);
            myActions.MessageBoxShow("click okay to continue");
            myActions.TypeText("^(c)", 500);



            myActions.TypeText("^(c)", 500);
            myActions.TypeText("%(\" \")n", 1000);

            myActions.Sleep(15000);
            goto myExit;
            myExit:
            myActions.ScriptEndedSuccessfullyUpdateStats();

            System.Windows.Application.Current.Shutdown();

        }

        //private void mouseEvent(object sender, EventArgs e) {
        //    Methods myActions = new Methods();
        //    myActions.SetValueByKey("MouseUp", "true");
        //    MessageBox.Show("Left mouse click!");

        //}

        private enum MouseMessages {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_LBUTTONDBLCLK = 0x0203,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
    }
}
