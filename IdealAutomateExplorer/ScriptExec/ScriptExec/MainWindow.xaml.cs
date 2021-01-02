using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using Snipping_OCR;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Forms.Samples;

namespace ScriptExec
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        int intWindowHeight = 110;
        int _maxLineChars = 0;
        string strBalloonArrowDirection = "NONE";
        double windowWidth = 0;
        System.Drawing.Point startPoint = new System.Drawing.Point(0, 0);
        List<string> _myContentList = new List<string>();
        public MainWindow()
        {
            string[] args = Environment.GetCommandLineArgs();
            string strMinimized = "";
            if (args.Length > 1 && args[1] == "Minimized")
            {
                strMinimized = "Minimized";
            }
            string _PositionType = "Absolute";
            string _RelativeTop = "0";
            string _RelativeLeft = "0";
            string _RelativeFullFileName = "";
            string relativeCodeSnippet = "";

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
            StringBuilder sb = new StringBuilder();


            InitializeComponent();
            this.Hide();

            string strWindowTitle = myActions.PutWindowTitleInEntity();
            if (strWindowTitle.StartsWith("MultipleWindowControls"))
            {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            myActions.Sleep(1000);

            FileFolderDialog dialog = new FileFolderDialog();
            //   dialog.ShowDialog();
            dialog.SelectedPath = myActions.GetValueByKey("LastSearchFolder");
            string str = "LastSearchFolder";


            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK && (Directory.Exists(dialog.SelectedPath) || File.Exists(dialog.SelectedPath)))
            {
                //cbxFolder.SelectedValue = dialog.SelectedPath;
                //cbxFolder.Text = dialog.SelectedPath;
                myActions.SetValueByKey("LastSearchFolder", dialog.SelectedPath);
                string strFolder = dialog.SelectedPath;
                myActions.SetValueByKey("cbxFolderSelectedValue", strFolder);
            }


                using (StreamReader reader = File.OpenText(dialog.SelectedPath))
            {
                sb.Clear();
                while (!reader.EndOfStream)
                {
                    string myLine = reader.ReadLine();
                    sb.AppendLine(myLine);
                }
            }

            if (intWindowHeight > 700)
            {
                intWindowHeight = 700;
            }
            int startPointXTemp = startPoint.X;
            int startPointYTemp = startPoint.Y;
            double windowWidthTemp = windowWidth * 2;
            if (windowWidthTemp > 1000)
            { windowWidthTemp = 1000; }
            if (windowWidthTemp < 500)
            { windowWidthTemp = 500; }
            if (_PositionType == "Relative")
            {
                startPointXTemp = 0;
                startPointYTemp = 0;
            }
            else
            {
                startPointXTemp = startPoint.X;
                startPointYTemp = startPoint.Y;
            }
            // http://www.codeproject.com/Tips/715891/Compiling-Csharp-Code-at-Runtime
            string code = sb.ToString();



//            code = @"
//   using System.Windows;
//using IdealAutomate.Core;
//using System.Collections.Generic;
//using System.Linq;
//using System.Diagnostics;
//using System.Text;
//using System;
//using System.Drawing;
//using System.Windows.Media.Imaging;
//using System.IO;


//using System.Reflection;

//    namespace First
//    {
//        public class Program : Window 
//        {
//            public static void Main()
//            {
// var window = new Window() //make sure the window is invisible
//            {
//                Width = 0,
//                Height = 0,
//                Left = -2000,
//                WindowStyle = WindowStyle.None,
//                ShowInTaskbar = false,
//                ShowActivated = false,
//            };
//            window.Show();
//            " +
//"\r\n IdealAutomate.Core.Methods myActions = new Methods();" +
//      sb.ToString()
//+ @"
//            }

//";
        

                CSharpCodeProvider provider = new CSharpCodeProvider();
                CompilerParameters parameters = new CompilerParameters();
                // Reference to System.Drawing library
                parameters.ReferencedAssemblies.Add("System.Drawing.dll");
                parameters.ReferencedAssemblies.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"IdealAutomate\IdealAutomateCore.dll"));
                parameters.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client\PresentationFramework.dll");
                parameters.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client\PresentationCore.dll");
                parameters.ReferencedAssemblies.Add("System.dll");
                parameters.ReferencedAssemblies.Add("System.Core.dll");
                parameters.ReferencedAssemblies.Add("System.Data.dll");
                parameters.ReferencedAssemblies.Add("System.Data.DataSetExtensions.dll");
                parameters.ReferencedAssemblies.Add("System.Xml.Linq.dll");
                parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                parameters.ReferencedAssemblies.Add("System.Xaml.dll");
                parameters.ReferencedAssemblies.Add("System.Xml.dll");
                parameters.ReferencedAssemblies.Add(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client\WindowsBase.dll");


                // True - memory generation, false - external file generation
                parameters.GenerateInMemory = true;
                // True - exe file generation, false - dll file generation
                parameters.GenerateExecutable = true;
                CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
                if (results.Errors.HasErrors)
                {
                    StringBuilder sb5 = new StringBuilder();

                    foreach (CompilerError error in results.Errors)
                    {
                        sb5.AppendLine(String.Format("Error ({0}): {1} Line: {2}", error.ErrorNumber, error.ErrorText, error.Line));
                    }

                    myActions.MessageBoxShow(sb5.ToString());
                }
                Assembly assembly = results.CompiledAssembly;
                Type program = assembly.GetType("First.Program");
                MethodInfo main = program.GetMethod("Main");
                main.Invoke(null, null);
              
              

           
            // Done --------------------
            if (intWindowHeight > 700)
            {
                intWindowHeight = 700;
            }
          

            //bool boolUseNewTab = myListControlEntity.Find(x => x.ID == "myCheckBox").Checked;
            //if (boolUseNewTab == true)
            //{
            //    List<string> myWindowTitles = myActions.GetWindowTitlesByProcessName("iexplore");
            //    myWindowTitles.RemoveAll(item => item == "");
            //    if (myWindowTitles.Count > 0)
            //    {
            //        myActions.ActivateWindowByTitle(myWindowTitles[0], (int)WindowShowEnum.SW_SHOWMAXIMIZED);
            //        myActions.TypeText("%(d)", 1500); // select address bar
            //        myActions.TypeText("{ESC}", 1500);
            //        myActions.TypeText("%({ENTER})", 1500); // Alt enter while in address bar opens new tab
            //        myActions.TypeText("%(d)", 1500);
            //        myActions.TypeText(myWebSite, 1500);
            //        myActions.TypeText("{ENTER}", 1500);
            //        myActions.TypeText("{ESC}", 1500);

            //    }
            //    else {
            //        myActions.Run("iexplore", myWebSite);

            //    }
            //}
            //else {
            //    myActions.Run("iexplore", myWebSite);
            //}

            //myActions.Sleep(1000);
            //myActions.TypeText(mySearchTerm, 500);
            //myActions.TypeText("{ENTER}", 500);


            goto myExit;
        myExit:
            myActions.ScriptEndedSuccessfullyUpdateStats();
            Application.Current.Shutdown();
        }



        private void FindWidthInCharsForContent(List<string> myContentList)
        {
            foreach (var myContent in myContentList)
            {
                var textArr = myContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                //     txtline.Text = textArr.Length.ToString();

                foreach (var item in textArr)
                {
                    if (item.Length > _maxLineChars)
                    {
                        _maxLineChars = item.Length;
                    }
                }
            }

            if (_maxLineChars > 80)
            {
                _maxLineChars = 80;
            }
        }
        private double CalculateStringHeight(string myContent, int controlWidthInChars)
        {
            double dblHeight = 0;
            int intCtr = 0;
            //  int intLineWidthInCharacters = 40;
            int intLineHeight = 25;
            int textLength = myContent.Length;
            int intTextBoxHeight = 0;
            if (textLength > 0)
            {
                //var lines = tb.Lines.Count();               
                var textArr = myContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                //     txtline.Text = textArr.Length.ToString();
                int totalNumberOfLines = 0;
                int numberOfLines = 0;
                foreach (var item in textArr)
                {
                    numberOfLines = item.Length / controlWidthInChars;
                    if (numberOfLines < 1)
                    {
                        numberOfLines = 1;
                    }
                    totalNumberOfLines += numberOfLines;

                }

                intTextBoxHeight = totalNumberOfLines * intLineHeight;

            }
            if (intTextBoxHeight < 29)
            {
                intTextBoxHeight = 30;
            }
            if (intTextBoxHeight > 700 - intWindowHeight)
            {
                intTextBoxHeight = 700 - intWindowHeight;
            }
            dblHeight = intTextBoxHeight;
            return dblHeight;
        }
        private static void GetSavedWindowPosition(Methods myActions, out int intWindowTop, out int intWindowLeft, out string strWindowTop, out string strWindowLeft)
        {
            strWindowLeft = myActions.GetValueByKeyGlobal("WindowLeft");
            strWindowTop = myActions.GetValueByKeyGlobal("WindowTop");
            Int32.TryParse(strWindowLeft, out intWindowLeft);
            Int32.TryParse(strWindowTop, out intWindowTop);
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
        private static void SaveClipboardImageToFile(string filePath)
        {
            var image = Clipboard.GetImage();
            if (image == null)
            {
                MessageBoxResult result = MessageBox.Show("Clipboard Cannot Be Empty", "PopUp Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {

                }
            }
            else
            {
                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(image));
                        encoder.Save(fileStream);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.InnerException == null
             ? ex.Message
             : ex.Message + " --> " + ex.InnerException.ToString());
                }
            }
        }
    }
}