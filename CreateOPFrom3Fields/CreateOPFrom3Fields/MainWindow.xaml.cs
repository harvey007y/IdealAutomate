using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;

namespace CreateOPFrom3Fields
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
            if (strWindowTitle.StartsWith("CreateOPFrom3Fields"))
            {
                myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
            }
            myActions.Sleep(1000);
NextOne:
            List<string> myWindowTitles = myActions.GetWindowTitlesByProcessName("chrome");
            myWindowTitles.RemoveAll(item => item == "");
            if (myWindowTitles.Count > 0)
            {
                myActions.ActivateWindowByTitle(myWindowTitles[0],3);
            }
            List<ControlEntity> myListControlEntity = new List<ControlEntity>();

            ControlEntity myControlEntity = new ControlEntity();
            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Heading;
            myControlEntity.Text = "Multiple Controls";
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabelTitle";
            myControlEntity.Text = "Enter Title";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "myTextBoxTitle";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = 0;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabelHours";
            myControlEntity.Text = "Enter Hours";
            myControlEntity.RowNumber = 1;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "myTextBoxHours";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = 1;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.Label;
            myControlEntity.ID = "myLabelURL";
            myControlEntity.Text = "Enter URL";
            myControlEntity.RowNumber = 2;
            myControlEntity.ColumnNumber = 0;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());


            myControlEntity.ControlEntitySetDefaults();
            myControlEntity.ControlType = ControlType.TextBox;
            myControlEntity.ID = "myTextBoxURL";
            myControlEntity.Text = "";
            myControlEntity.RowNumber = 2;
            myControlEntity.ColumnNumber = 1;
            myListControlEntity.Add(myControlEntity.CreateControlEntity());



            myActions.WindowMultipleControls(ref myListControlEntity, 700, 1300, 0, 0);

            string myTitle = myListControlEntity.Find(x => x.ID == "myTextBoxTitle").Text;
            string myHours = myListControlEntity.Find(x => x.ID == "myTextBoxHours").Text;
            string myURL = myListControlEntity.Find(x => x.ID == "myTextBoxURL").Text;
            



            string myCode = "<li><a href=\"" + myURL.Trim() + "\" target=\"_blank\">" + myTitle.Trim() + " - " + myHours.Trim() + " hrs</a></li>";
            myActions.PutEntityInClipboard(myCode);
            myWindowTitles = myActions.GetWindowTitlesByProcessName("notepad++");
            myWindowTitles.RemoveAll(item => item == "");
            if (myWindowTitles.Count > 0)
            {
                myActions.ActivateWindowByTitle(myWindowTitles[0],3);
            }
            myActions.MessageBoxShow("Paste");
            goto NextOne;
            myExit:
            myActions.ScriptEndedSuccessfullyUpdateStats();
            Application.Current.Shutdown();
        }
    }
}