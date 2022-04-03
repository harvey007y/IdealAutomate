using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using WindowsInput;
using WindowsInput.Native;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.ComponentModel;
using System.Linq;

using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows;
using System;

namespace ExecuteWithBreakpointTrace
{





    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        bool spaceBarPressed = false;
        List<LineOfCode> listExecutedCode = new List<LineOfCode>();

        System.Timers.Timer aTimer = new System.Timers.Timer();

        static byte[] mybytearray;
        static ControlEntity myControlEntity = new ControlEntity();
        static IdealAutomate.Core.Methods myActions = new Methods();
        static ImageEntity myImage = new ImageEntity();
        static int intRowCtr = 0;
        static int newLeft = 0;
        static int newTop = 0;
        static int[,] resultArray = new int[100, 100];
        static List<ComboBoxPair> cbp = new List<ComboBoxPair>();
        static List<ControlEntity> myListControlEntity = new List<ControlEntity>();
        static string strButtonPressed = "";

        public MainWindow()
        {



            int delay = 500;
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
            // Create a timer and set a two millisecond interval.

            aTimer.Interval = 2;

            // Alternate method: create a Timer with an interval argument to the constructor.
            //aTimer = new System.Timers.Timer(2000);

            // Create a timer with a two millisecond interval.
            aTimer = new System.Timers.Timer(2);

            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;
            spaceBarPressed = false;

            window.Show();

            myActions.ScriptStartedUpdateStats();

            InitializeComponent();
            this.Hide();

            string strWindowTitle = myActions.PutWindowTitleInEntity();
            if (strWindowTitle.StartsWith("ExecuteWithBreakpointTrace"))
            {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }

            myListControlEntity = new List<ControlEntity>();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.ID = "lbl";
            myControlEntity.Text = "ExecuteWithBreakpointTrace";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lbl";
            myControlEntity.Text = "" +
"1. Click on visual studio to debug \r\n";
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 272;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lbl";
            myControlEntity.Text = "" +
"2. Close this window to start debugging \r\n";
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 312;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());

            intRowCtr++;
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "lbl";
            myControlEntity.Text = "" +
"3. PRESS Ctrl-Space to stop debugger \r\n";
            myControlEntity.ToolTipx = "";
            myControlEntity.RowNumber = intRowCtr;
            myControlEntity.Width = 312;
            myControlEntity.ColumnNumber = 0;
            myControlEntity.ColumnSpan = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());
            strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 300, 624, 0, 0);
            if (strButtonPressed == "btnCancel")
            {
                myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
                goto myExit;
            }

            // TODO: In IdealAutomateCore, create another PutAll override that does not ask if you want an alternative image when it cannot find image
            string firstLine = "";
            string currentLine = "";
            string currentLineNumber = "";
            string firstFileName = "";
            string currFileName = "";
            string firstLineNumber = "";

            myActions.TypeText("{F11}", 1000); // compile program and go to first breakpoint
            int intCtr = 0;
        TryToFindYellowArrow:
            intCtr++;
            ImageEntity myImage = new ImageEntity();

            if (boolRunningFromHome)
            {
                myImage.ImageFile = "Images\\imgYellowArrow.PNG";
            }
            else
            {
                myImage.ImageFile = "Images\\imgYellowArrow.PNG";
            }
            myImage.Sleep = 200;
            myImage.Attempts = 1;
            myImage.RelativeX = 10;
            myImage.RelativeY = 10;
            myImage.Tolerance = 60;

            int[,] myArray = myActions.PutAll(myImage);
            if (myArray.Length == 0 && intCtr < 50)
            {
                goto TryToFindYellowArrow;
            }
            if (myArray.Length == 0)
            {
                myActions.MessageBoxShow("I could not find image of YellowArrow");
            }
            else
            {
                // myActions.MessageBoxShow("Found Yellow Arrow");
            }
            myActions.TypeText("^(c)", delay);
            currentLine = myActions.PutClipboardInEntity();
            firstLine = currentLine;
            LineOfCode myLine = new LineOfCode();
            myLine.TextOfCode = currentLine;

            // get line number
            myActions.TypeText("^(g)", delay);
            myActions.TypeText("^(a)", delay);
            myActions.TypeText("^(c)", delay);
            currentLineNumber = myActions.PutClipboardInEntity();
            firstLineNumber = currentLineNumber;
            myLine.LineNumber = currentLineNumber;
            myActions.TypeText("{ESCAPE}", delay);

            // get filename
            myActions.TypeText("%(f)", delay);
            myActions.TypeText("a", delay);
            myActions.TypeText("^(c)", delay);
            currFileName = myActions.PutClipboardInEntity();
            myLine.FileName = currFileName;
            firstFileName = currFileName;
            myActions.TypeText("{ESCAPE}", delay);

            // add the line to list
            listExecutedCode.Add(myLine);
            LineOfCode prevLine = myLine;

        GetNextLine:
            if (spaceBarPressed)
            {
                goto EndOfExecution;
            }
            // get next line            
            myActions.TypeText("{F11}", delay); // next breakpoint
            myActions.TypeText("^(c)", delay);
            currentLine = myActions.PutClipboardInEntity();
            myLine = new LineOfCode();
            myLine.TextOfCode = currentLine;

            // get line number
            myActions.TypeText("^(g)", delay);
            myActions.TypeText("^(a)", delay);
            myActions.TypeText("^(c)", delay);
            currentLineNumber = myActions.PutClipboardInEntity();
            firstLineNumber = currentLineNumber;
            myLine.LineNumber = currentLineNumber;
            myActions.TypeText("{ESCAPE}", delay);

            // get filename
            myActions.TypeText("%(f)", delay);
            myActions.TypeText("a", delay);
            myActions.TypeText("^(c)", delay);
            currFileName = myActions.PutClipboardInEntity();
            myLine.FileName = currFileName;
            firstFileName = currFileName;
            myActions.TypeText("{ESCAPE}", delay);

            // add the line to list
            listExecutedCode.Add(myLine);

            if (myLine != prevLine)
            {
                prevLine = myLine;
                goto GetNextLine;
            }
        EndOfExecution:
            myActions.MessageBoxShow("Successfully Reached End of Execution");

            string strOutFile = @"C:\Data\ExecutedCode.txt";
            if (File.Exists(strOutFile))
            {
                File.Delete(strOutFile);
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(strOutFile))
            {
                // Write list to text file so I can look at it
                foreach (LineOfCode item in listExecutedCode)
                {
                    file.WriteLine(item.FileName + " " + item.LineNumber + " " + item.TextOfCode);
                }

            }


            string strExecutable = @"C:\Windows\system32\notepad.exe";
            myActions.RunSync(strExecutable, strOutFile);


        // We found output completed and now want to copy the results
        // to notepad


        myExit:
            aTimer.Enabled = false;
            aTimer.Stop();
            aTimer.Close();
            myActions.Sleep(1000);
            myActions.ScriptEndedSuccessfullyUpdateStats();
            Application.Current.Shutdown();
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            InputSimulator myInputSimulator = new InputSimulator();

            if (myInputSimulator.InputDeviceState.IsKeyDown(VirtualKeyCode.CONTROL) && myInputSimulator.InputDeviceState.IsKeyDown(VirtualKeyCode.SPACE) && spaceBarPressed == false)
            {
                spaceBarPressed = true;
                MessageBox.Show("hi wade");

                //myActions.ScriptEndedSuccessfullyUpdateStats();
                //Application.Current.Shutdown();

            }


        }
    }
}