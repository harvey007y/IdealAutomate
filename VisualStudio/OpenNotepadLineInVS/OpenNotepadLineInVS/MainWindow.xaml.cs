using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading;

namespace OpenNotepadLineInVS {
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

            myActions.DebugMode = true;
            ImageEntity myImage = new ImageEntity();

            string strWindowTitle = myActions.PutWindowTitleInEntity();
            if (strWindowTitle.StartsWith("OpenNotepadLineInVS")) {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            //myImage.ImageFile = "Images\\Ready.PNG";
            //myImage.Sleep = 3500;
            //myImage.Attempts = 2;
            //myImage.RelativeX = 10;
            //myActions.ClickImageIfExists(myImage);
            List<string> myWindowTitles = myActions.GetWindowTitlesByProcessName("notepad++");
            myWindowTitles.RemoveAll(item => item == "");
            if (myWindowTitles.Count > 0) {
                myActions.ActivateWindowByTitle(myWindowTitles[0], 3);
                myActions.Sleep(1000);
                //int[,] myCursorPosition = myActions.PutCursorPosition();

                //myActions.RightClick(myCursorPosition);
                myActions.TypeText("{RIGHT}", 500);
                myActions.TypeText("{HOME}", 500);
                myActions.TypeText("+({END})", 500);
                myActions.TypeText("^(c)", 500);
                myActions.Sleep(500);
                string strCurrentLine = "";
                RunAsSTAThread(
                () => {
                    strCurrentLine = myActions.PutClipboardInEntity();
                });

            }
            myWindowTitles = myActions.GetWindowTitlesByProcessName("devenv");
            string myWebSite = "";
            TryAgainClip:
            string myOrigEditPlusLine = myActions.PutClipboardInEntity();
            if (myOrigEditPlusLine.Length == 0) {
                System.Windows.Forms.DialogResult myResult = myActions.MessageBoxShowWithYesNo("You forgot to put line in clipboard - Put line in clipboard and click yes to continue");
                if (myResult == System.Windows.Forms.DialogResult.Yes) {
                    goto TryAgainClip;
                } else {
                    goto myExit;
                }
            }
            //string myOrigEditPlusLine = strReadLine;
            bool boolSolutionFileFound = true;
            string strSolutionName = "";
            List<string> myBeginDelim = new List<string>();
            List<string> myEndDelim = new List<string>();
            myBeginDelim.Add("\"");
            myEndDelim.Add("\"");
            FindDelimitedTextParms delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);

            string myQuote = "\"";
            delimParms.lines[0] = myOrigEditPlusLine;


            myActions.FindDelimitedText(delimParms);
            int intLastSlash = delimParms.strDelimitedTextFound.LastIndexOf('\\');
            if (intLastSlash < 1) {
                myActions.MessageBoxShow("Could not find last slash in in EditPlusLine - aborting");
                goto myExit;
            }
            string strPathOnly = delimParms.strDelimitedTextFound.SubstringBetweenIndexes(0, intLastSlash);
            string strFileNameOnly = delimParms.strDelimitedTextFound.Substring(intLastSlash + 1);
            myBeginDelim.Clear();
            myEndDelim.Clear();
            myBeginDelim.Add("(");
            myEndDelim.Add(",");
            delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);
            delimParms.lines[0] = myOrigEditPlusLine;
            myActions.FindDelimitedText(delimParms);
            string strLineNumber = delimParms.strDelimitedTextFound;

            //========
            string strFullName = Path.Combine(strPathOnly, strFileNameOnly);
            string strSolutionFullFileName = "";

            string currentTempName = strFullName;
            while (currentTempName.IndexOf("\\") > -1) {
                currentTempName = currentTempName.Substring(0, currentTempName.LastIndexOf("\\"));
                FileInfo fi = new FileInfo(currentTempName);
                if (Directory.Exists(currentTempName)) {
                    string[] files = null;
                    try {
                        files = System.IO.Directory.GetFiles(currentTempName, "*.sln");
                        if (files.Length > 0) {
                            // TODO: Currently defaulting to last one, but should ask the user which one to use if there is more than one                               
                            strSolutionFullFileName = files[files.Length - 1];
                            boolSolutionFileFound = true;
                            strSolutionName = strSolutionFullFileName.Substring(strSolutionFullFileName.LastIndexOf("\\") + 1).Replace(".sln", "");
                            myWindowTitles = myActions.GetWindowTitlesByProcessName("devenv");
                            myWindowTitles.RemoveAll(vsItem => vsItem == "");
                            bool boolVSMatchingSolutionFound = false;
                            foreach (var vsTitle in myWindowTitles) {
                                if (vsTitle.StartsWith(strSolutionName + " - ") || vsTitle.StartsWith(strSolutionName + " (Running) - ")) {
                                    boolVSMatchingSolutionFound = true;
                                    myActions.ActivateWindowByTitle(vsTitle, 3);
                                    myActions.Sleep(1000);
                                    myActions.TypeText("{ESCAPE}", 500);
                                    myBeginDelim = new List<string>();
                                    myEndDelim = new List<string>();
                                    myBeginDelim.Add("\"");
                                    myEndDelim.Add("\"");
                                    delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);

                                    myQuote = "\"";
                                    delimParms.lines[0] = myOrigEditPlusLine;


                                    myActions.FindDelimitedText(delimParms);
                                    intLastSlash = delimParms.strDelimitedTextFound.LastIndexOf('\\');
                                    if (intLastSlash < 1) {
                                        myActions.MessageBoxShow("Could not find last slash in in EditPlusLine - aborting");
                                        break;
                                    }
                                    strPathOnly = delimParms.strDelimitedTextFound.SubstringBetweenIndexes(0, intLastSlash);
                                    strFileNameOnly = delimParms.strDelimitedTextFound.Substring(intLastSlash + 1);
                                    myBeginDelim.Clear();
                                    myEndDelim.Clear();
                                    myBeginDelim.Add("(");
                                    myEndDelim.Add(",");
                                    delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);
                                    delimParms.lines[0] = myOrigEditPlusLine;
                                    myActions.FindDelimitedText(delimParms);
                                    strLineNumber = delimParms.strDelimitedTextFound;
                                    myActions.TypeText("{ESC}", 2000);
                                    myActions.TypeText("%(f)", 1000);
                                    myActions.TypeText("{DOWN}", 1000);
                                    myActions.TypeText("{RIGHT}", 1000);
                                    myActions.TypeText("f", 1000);
                                    // myActions.TypeText("^(o)", 2000);
                                    myActions.TypeText("%(d)", 1500);
                                    myActions.TypeText(strPathOnly, 1500);
                                    myActions.TypeText("{ENTER}", 500);
                                    myActions.TypeText("%(n)", 500);
                                    myActions.TypeText(strFileNameOnly, 1500);
                                    myActions.TypeText("{ENTER}", 1000);
                                    break;
                                }
                            }
                            if (boolVSMatchingSolutionFound == false) {
                                System.Windows.Forms.DialogResult myResult = myActions.MessageBoxShowWithYesNo("I could not find the solution (" + strSolutionName + ") currently running.\n\r\n\r Do you want me to launch it in Visual Studio for you.\n\r\n\rTo go ahead and launch the solution, click yes, otherwise, click no to cancel");
                                if (myResult == System.Windows.Forms.DialogResult.No) {
                                    return;
                                }
                                string strVSPath = myActions.GetValueByKeyGlobal("VS2013Path");
                                // C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe
                                if (strVSPath == "") {
                                    List<ControlEntity> myListControlEntity = new List<ControlEntity>();

                                    ControlEntity myControlEntity = new ControlEntity();
                                    myControlEntity.ControlEntitySetDefaults();
                                    myControlEntity.ControlType = ControlType.Heading;
                                    myControlEntity.Text = "Specify location of Visual Studio";
                                    myListControlEntity.Add(myControlEntity.CreateControlEntity());


                                    myControlEntity.ControlEntitySetDefaults();
                                    myControlEntity.ControlType = ControlType.Label;
                                    myControlEntity.ID = "myLabel";
                                    myControlEntity.Text = "Visual Studio Executable:";
                                    myControlEntity.RowNumber = 0;
                                    myControlEntity.ColumnNumber = 0;
                                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                                    myControlEntity.ControlEntitySetDefaults();
                                    myControlEntity.ControlType = ControlType.TextBox;
                                    myControlEntity.ID = "myAltExecutable";
                                    myControlEntity.Text = "";
                                    myControlEntity.RowNumber = 0;
                                    myControlEntity.ColumnNumber = 1;
                                    myListControlEntity.Add(myControlEntity.CreateControlEntity());

                                    myActions.WindowMultipleControls(ref myListControlEntity, 600, 500, 0, 0);
                                    string strAltExecutable = myListControlEntity.Find(x => x.ID == "myAltExecutable").Text;
                                    myActions.SetValueByKeyGlobal("VS2013Path", strAltExecutable);
                                    strVSPath = strAltExecutable;
                                }
                                myActions.Run(strVSPath, "\"" + strSolutionFullFileName + "\"");
                                myActions.Sleep(10000);
                                myActions.MessageBoxShow("When visual studio finishes loading, please click okay to continue");
                                myActions.TypeText("{ESCAPE}", 500);
                                boolSolutionFileFound = true;
                                strSolutionName = currentTempName.Substring(currentTempName.LastIndexOf("\\") + 1).Replace(".sln", "");
                                myWindowTitles = myActions.GetWindowTitlesByProcessName("devenv");
                                myWindowTitles.RemoveAll(vsItem => vsItem == "");
                                boolVSMatchingSolutionFound = false;
                                foreach (var vsTitle in myWindowTitles) {
                                    if (vsTitle.StartsWith(strSolutionName + " - ")) {
                                        boolVSMatchingSolutionFound = true;
                                        myActions.ActivateWindowByTitle(vsTitle, 3);
                                        myActions.Sleep(1000);
                                        myActions.TypeText("{ESCAPE}", 500);
                                        myBeginDelim = new List<string>();
                                        myEndDelim = new List<string>();
                                        myBeginDelim.Add("\"");
                                        myEndDelim.Add("\"");
                                        delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);

                                        myQuote = "\"";
                                        delimParms.lines[0] = myOrigEditPlusLine;


                                        myActions.FindDelimitedText(delimParms);
                                        intLastSlash = delimParms.strDelimitedTextFound.LastIndexOf('\\');
                                        if (intLastSlash < 1) {
                                            myActions.MessageBoxShow("Could not find last slash in in EditPlusLine - aborting");
                                            break;
                                        }
                                        strPathOnly = delimParms.strDelimitedTextFound.SubstringBetweenIndexes(0, intLastSlash);
                                        strFileNameOnly = delimParms.strDelimitedTextFound.Substring(intLastSlash + 1);
                                        myBeginDelim.Clear();
                                        myEndDelim.Clear();
                                        myBeginDelim.Add("(");
                                        myEndDelim.Add(",");
                                        delimParms = new FindDelimitedTextParms(myBeginDelim, myEndDelim);
                                        delimParms.lines[0] = myOrigEditPlusLine;
                                        myActions.FindDelimitedText(delimParms);
                                        strLineNumber = delimParms.strDelimitedTextFound;
                                        myActions.TypeText("{ESC}", 2000);
                                        myActions.TypeText("%(f)", 1000);
                                        myActions.TypeText("{DOWN}", 1000);
                                        myActions.TypeText("{RIGHT}", 1000);
                                        myActions.TypeText("f", 1000);
                                        // myActions.TypeText("^(o)", 2000);
                                        myActions.TypeText("%(d)", 1500);
                                        myActions.TypeText(strPathOnly, 1500);
                                        myActions.TypeText("{ENTER}", 500);
                                        myActions.TypeText("%(n)", 500);
                                        myActions.TypeText(strFileNameOnly, 1500);
                                        myActions.TypeText("{ENTER}", 1000);
                                        break;
                                    }
                                }
                            }
                            if (boolVSMatchingSolutionFound == false) {
                                myActions.MessageBoxShow("Could not find visual studio for " + strSolutionName);
                            }
                            break;

                        }
                    } catch (UnauthorizedAccessException e) {

                        Console.WriteLine(e.Message);
                        continue;
                    } catch (System.IO.DirectoryNotFoundException e) {
                        Console.WriteLine(e.Message);
                        continue;
                    } catch (System.IO.PathTooLongException e) {
                        Console.WriteLine(e.Message);
                        continue;
                    } catch (Exception e) {
                        Console.WriteLine(e.Message);
                        continue;
                    }
                }
            }

            myActions.TypeText("^(g)", 500);
            myActions.TypeText(strLineNumber, 500);
            myActions.TypeText("{ENTER}", 500);
            goto myExit;


            myExit:

            //myActions.MessageBoxShow("Script completed");
            myActions.ScriptEndedSuccessfullyUpdateStats();
            Application.Current.Shutdown();
        }
        static void RunAsSTAThread(Action goForIt) {
            AutoResetEvent @event = new AutoResetEvent(false);
            Thread thread = new Thread(
                () => {
                    goForIt();
                    @event.Set();
                });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            @event.WaitOne();
        }
    }
}
