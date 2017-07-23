using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IdealAutomate.Core;
using System.IO;
using System.Collections;

namespace IdealAutomate.Core {
    /// <summary>
    /// Interaction logic for WindowComboBox.xaml
    /// </summary>
    public partial class WindowMultipleControls : Window {

        string _Label;
        bool boolSkipSelectionChanged = false;
        private List<ControlEntity> _ListControlEntity;
        public ComboBoxPair SelectedComboBoxPair { get; set; }
        public bool boolOkayPressed = false;
        public string strButtonClickedName = "";
        int _Top;
        int _Left;
        public string SelectedValue { get; set; }
        public WindowMultipleControls(ref List<ControlEntity> myListControlEntity, int intWindowHeight, int intWindowWidth, int intTop, int intLeft, WindowState windowState) {
            _Top = intTop;
            _Left = intLeft;
            this.Top = _Top;
            this.Left = _Left;
            if (intTop < 0 || intLeft < 0) {
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            if (windowState == WindowState.Minimized) {
                this.WindowState = WindowState.Minimized;
            }
            InitializeComponent();

            if (intWindowHeight > 0) {
                myWindow.Height = intWindowHeight;
                MainBorder.Height = intWindowHeight;
            } else {
                myWindow.Height = 391;
            }
            if (intWindowWidth > 0) {
                myWindow.Width = intWindowWidth;
                MainBorder.Width = intWindowWidth;
            } else {
                myWindow.Width = 487;

            }

            _ListControlEntity = myListControlEntity;
            //// Create a button.
            //Button myButton = new Button();
            //// Set properties.
            //myButton.Content = "Click Me!";

            //// Add created button to a previously created container.
            //myStackPanel.Children.Add(myButton);
            int intMaxRows = 0;
            int intMaxColumns = 0;
            foreach (ControlEntity item in myListControlEntity) {
                if (item.RowNumber > intMaxRows) {
                    intMaxRows = item.RowNumber;
                }
                int intNumSpanSum = 0;
                intNumSpanSum = item.ColumnNumber + item.ColumnSpan;
                if (item.ColumnSpan > 0) {
                    intNumSpanSum--;
                }
                if (intNumSpanSum > intMaxColumns) {
                    intMaxColumns = intNumSpanSum;
                }
            }
            for (int i = 0; i < intMaxRows + 1; i++) {
                myGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < intMaxColumns + 1; i++) {
                myGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            foreach (ControlEntity item in myListControlEntity) {
                switch (item.ControlType) {
                    case ControlType.Heading:
                        Label1.Text = item.Text;
                        if (item.Width > 0) {
                            Label1.Width = item.Width;
                        }
                        break;
                    case ControlType.Label:
                        Label myLabel = new Label();
                        if (item.ToolTipx != null && item.ToolTipx.ToString().Trim() != "") {
                            myLabel.ToolTip = item.ToolTipx;
                        }
                        myLabel.FontFamily = item.FontFamilyx;
                        myLabel.FontSize = item.FontSize;
                        myLabel.FontStretch = item.FontStretchx;
                        myLabel.FontStyle = item.FontStyle;
                        myLabel.FontWeight = FontWeights.Normal;
                        myLabel.Margin = new Thickness(1, 1, 1, 1);
                        myLabel.Name = item.ID;
                        myLabel.Content = item.Text;
                        if (item.BackgroundColor != null) {
                            myLabel.Background = new SolidColorBrush(item.BackgroundColor ?? Color.FromRgb(00, 00, 00));
                        }
                        if (item.ForegroundColor != null) {
                            myLabel.Foreground = new SolidColorBrush(item.ForegroundColor ?? Color.FromRgb(00, 00, 00));
                        }

                        Grid.SetRow(myLabel, item.RowNumber);
                        Grid.SetColumn(myLabel, item.ColumnNumber);
                        if (item.ColumnSpan < 1) {
                            item.ColumnSpan = 1;
                        }
                        Grid.SetColumnSpan(myLabel, item.ColumnSpan);
                        if (item.Width > 0) {
                            myLabel.Width = item.Width;
                        }
                        myGrid.Children.Add(myLabel);
                        break;
                    case ControlType.Button:
                        Button button = new Button();
                        if (item.ToolTipx != null && item.ToolTipx.ToString().Trim() != "") {
                            button.ToolTip = item.ToolTipx;
                        }
                        button.Click += new System.Windows.RoutedEventHandler(button_Click);
                        button.Name = item.ID;
                        button.Content = item.Text;
                        if (item.BackgroundColor != null) {
                            button.Background = new SolidColorBrush(item.BackgroundColor ?? Color.FromRgb(00, 00, 00));
                        }
                        if (item.ForegroundColor != null) {
                            button.Foreground = new SolidColorBrush(item.ForegroundColor ?? Color.FromRgb(00, 00, 00));
                        }
                        Grid.SetRow(button, item.RowNumber);
                        Grid.SetColumn(button, item.ColumnNumber);
                        if (item.ColumnSpan < 1) {
                            item.ColumnSpan = 1;
                        }
                        Grid.SetColumnSpan(button, item.ColumnSpan);
                        if (item.Width > 0) {
                            button.Width = item.Width;
                        }
                        button.Margin = new Thickness(1, 1, 1, 1);
                        myGrid.Children.Add(button);
                        break;
                    case ControlType.TextBox:
                        TextBox myTextBox = new TextBox();
                        myTextBox.Text = item.Text;
                        if (item.ToolTipx != null && item.ToolTipx.ToString().Trim() != "") {
                            myTextBox.ToolTip = item.ToolTipx;
                        }
                        myTextBox.Name = item.ID;
                        if (item.Width > 0) {
                            myTextBox.Width = item.Width;
                        }
                        if (item.Multiline == true) {
                            // AcceptsReturn = "True" TextWrapping = "Wrap" VerticalScrollBarVisibility="Auto"
                            myTextBox.AcceptsReturn = true;
                            myTextBox.TextWrapping = System.Windows.TextWrapping.Wrap;
                            myTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                            myTextBox.Height = 50;

                        }
                        if (item.Height > 0) {
                            myTextBox.Height = item.Height;
                        }
                        Grid.SetRow(myTextBox, item.RowNumber);
                        Grid.SetColumn(myTextBox, item.ColumnNumber);
                        if (item.ColumnSpan < 1) {
                            item.ColumnSpan = 1;
                        }
                        Grid.SetColumnSpan(myTextBox, item.ColumnSpan);
                        myGrid.Children.Add(myTextBox);
                        break;
                    case ControlType.PasswordBox:
                        PasswordBox myPasswordBox = new PasswordBox();
                        myPasswordBox.Password = item.Text;
                        if (item.ToolTipx != null && item.ToolTipx.ToString().Trim() != "") {
                            myPasswordBox.ToolTip = item.ToolTipx;
                        }
                        myPasswordBox.Name = item.ID;
                        if (item.Width > 0) {
                            myPasswordBox.Width = item.Width;
                        }

                        if (item.Height > 0) {
                            myPasswordBox.Height = item.Height;
                        }
                        Grid.SetRow(myPasswordBox, item.RowNumber);
                        Grid.SetColumn(myPasswordBox, item.ColumnNumber);
                        if (item.ColumnSpan < 1) {
                            item.ColumnSpan = 1;
                        }
                        Grid.SetColumnSpan(myPasswordBox, item.ColumnSpan);
                        myGrid.Children.Add(myPasswordBox);
                        break;
                    case ControlType.ComboBox:
                        string strScriptName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                        string settingsDirectory = GetAppDirectoryForScript(strScriptName);
                        string fileName = item.ID + ".txt";
                        string settingsPath = System.IO.Path.Combine(settingsDirectory, fileName);
                        ArrayList alHosts = new ArrayList();
                        List<ComboBoxPair> cbp = new List<ComboBoxPair>();
                        cbp.Clear();
                        cbp.Add(new ComboBoxPair("--Select Item ---", "--Select Item ---"));
                        ComboBox myComboBox = new ComboBox();
                        if (item.ComboBoxIsEditable) {
                            myComboBox.IsEditable = true;
                            myComboBox.DropDownOpened += comboBoxDropDownOpened;
                            myComboBox.LostFocus += comboBoxLostFocus;

                            myComboBox.SelectionChanged += comboBoxSelectionChanged;

                            if (!File.Exists(settingsPath)) {
                                using (StreamWriter objSWFile = File.CreateText(settingsPath)) {
                                    objSWFile.Close();
                                }
                            }
                            using (StreamReader objSRFile = File.OpenText(settingsPath)) {
                                string strReadLine = "";
                                while ((strReadLine = objSRFile.ReadLine()) != null) {
                                    string[] keyvalue = strReadLine.Split('^');
                                    if (keyvalue[0] != "--Select Item ---") {
                                        cbp.Add(new ComboBoxPair(keyvalue[0], keyvalue[1]));
                                    }
                                }
                                objSRFile.Close();
                            }
                            item.ListOfKeyValuePairs = cbp;
                        }
                        if (item.ToolTipx != null && item.ToolTipx.ToString().Trim() != "") {
                            myComboBox.ToolTip = item.ToolTipx;
                        }
                        myComboBox.Name = item.ID;
                        if (item.ListOfKeyValuePairs.Count == 0) {
                            cbp = new List<ComboBoxPair>();
                            cbp.Clear();
                            cbp.Add(new ComboBoxPair("--Select Item ---", "--Select Item ---"));
                            SqlConnection con = new SqlConnection("Server=(local)\\SQLEXPRESS;Initial Catalog=IdealAutomateDB;Integrated Security=SSPI");
                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandText = "SELECT lk.inc, i.listItemKey, i.ListItemValue FROM LkDDLNamesItems lk " +
                            "join DDLNames n on n.inc = lk.DDLNamesInc " +
                            "join DDLItems i on i.inc = lk.ddlItemsInc " +
                            "where n.ID = @ID ";
                            if (item.ParentLkDDLNamesItemsInc > -1) {
                                cmd.CommandText += " and lk.ParentLkDDLNamesItemsInc = @ParentLkDDLNamesItemsInc";
                                cmd.Parameters.Add("@ParentLkDDLNamesItemsInc", SqlDbType.VarChar, -1);
                                cmd.Parameters["@ParentLkDDLNamesItemsInc"].Value = item.ParentLkDDLNamesItemsInc;
                            }
                            cmd.Parameters.Add("@ID", SqlDbType.VarChar, -1);
                            cmd.Parameters["@ID"].Value = item.ID;
                            if (item.DDLName != null && item.DDLName != "") {
                                cmd.Parameters["@ID"].Value = item.DDLName;
                            }
                            cmd.Connection = con;
                            try {
                                con.Open();
                                SqlDataReader reader = cmd.ExecuteReader();
                                //(CommandBehavior.SingleRow)
                                while (reader.Read()) {
                                    int intInc = reader.GetInt32(0);
                                    string strIDx = reader.GetString(1);
                                    cbp.Add(new ComboBoxPair(strIDx, intInc.ToString()));
                                }
                                reader.Close();

                                item.ListOfKeyValuePairs = cbp;
                                if (item.SelectedValue == "" || item.SelectedValue == null) {
                                    cmd.CommandText = "SELECT Top 1 DefaultValue from DDLNames where ID = @ID ";
                                    var strDefaultValue = cmd.ExecuteScalar();
                                    if (strDefaultValue == null) {
                                        item.SelectedValue = "";
                                    } else {
                                        item.SelectedValue = strDefaultValue.ToString();
                                    }
                                }
                            } finally {
                                con.Close();
                            }

                        }
                        myComboBox.ItemsSource = item.ListOfKeyValuePairs;
                        Grid.SetRow(myComboBox, item.RowNumber);
                        Grid.SetColumn(myComboBox, item.ColumnNumber);
                        if (item.ColumnSpan < 1) {
                            item.ColumnSpan = 1;
                        }
                        Grid.SetColumnSpan(myComboBox, item.ColumnSpan);
                        myComboBox.SelectedValue = item.SelectedValue;
                        myComboBox.DisplayMemberPath = "_Key";
                        myComboBox.SelectedValuePath = "_Value";
                        if (item.Width > 0) {
                            myComboBox.Width = item.Width;
                        }
                        myGrid.Children.Add(myComboBox);
                        break;
                    case ControlType.CheckBox:
                        CheckBox myCheckBox = new CheckBox();
                        if (item.ToolTipx != null && item.ToolTipx.ToString().Trim() != "") {
                            myCheckBox.ToolTip = item.ToolTipx;
                        }
                        myCheckBox.Name = item.ID;
                        myCheckBox.Content = item.Text;
                        myCheckBox.IsChecked = item.Checked;
                        Grid.SetRow(myCheckBox, item.RowNumber);
                        Grid.SetColumn(myCheckBox, item.ColumnNumber);
                        if (item.ColumnSpan < 1) {
                            item.ColumnSpan = 1;
                        }
                        Grid.SetColumnSpan(myCheckBox, item.ColumnSpan);
                        if (item.Width > 0) {
                            myCheckBox.Width = item.Width;
                        }
                        myGrid.Children.Add(myCheckBox);
                        break;
                    case ControlType.Image:
                        Image myImage = new Image();
                        if (item.ToolTipx != null && item.ToolTipx.ToString().Trim() != "") {
                            myImage.ToolTip = item.ToolTipx;
                        }
                        myImage.Name = item.ID;

                        Grid.SetRow(myImage, item.RowNumber);
                        Grid.SetColumn(myImage, item.ColumnNumber);
                        if (item.ColumnSpan < 1) {
                            item.ColumnSpan = 1;
                        }
                        Grid.SetColumnSpan(myImage, item.ColumnSpan);
                        if (item.Width > 0) {
                            myImage.Width = item.Width;
                        }
                        if (item.Height > 0) {
                            myImage.Height = item.Height;
                        }
                        myImage.Source = item.Source;
                        myGrid.Children.Add(myImage);

                        break;
                    default:
                        break;
                }
            }

        }

        private void comboBoxSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (boolSkipSelectionChanged == false) {
                string strHostName = ((ComboBox)sender).Text;
                funcUpdateCombobox1(strHostName, sender);
            }

        }

        private void comboBoxLostFocus(object sender, RoutedEventArgs e) {
            boolSkipSelectionChanged = true;
            string strHostName = ((ComboBox)sender).Text;
            funcUpdateCombobox1(strHostName, sender);
            ((ComboBox)sender).SelectedValue = strHostName;
            ((ComboBox)sender).Text = strHostName;
            boolSkipSelectionChanged = false;
        }

        private void comboBoxDropDownOpened(object sender, EventArgs e) {

            string strHostName = ((ComboBox)sender).Text;
            funcUpdateCombobox1(strHostName, sender);
            ((ComboBox)sender).SelectedValue = strHostName;
        }

        public void funcUpdateCombobox1(string strNewHostName, object sender) {
            List<ComboBoxPair> alHosts = ((ComboBox)sender).ItemsSource.Cast<ComboBoxPair>().ToList();
            List<ComboBoxPair> alHostsNew = new List<ComboBoxPair>();
            ComboBoxPair myCbp = new ComboBoxPair(strNewHostName, strNewHostName);
            bool boolNewItem = false;

            alHostsNew.Add(myCbp);

            foreach (ComboBoxPair item in alHosts) {
                if (strNewHostName != item._Key && item._Key != "") {
                    boolNewItem = true;
                    alHostsNew.Add(item);
                }
            }
            if (alHostsNew.Count > 14) {
                for (int i = alHostsNew.Count - 1; i > 0; i--) {
                    if (alHostsNew[i]._Key.Trim() != "--Select Item ---") {
                        alHostsNew.RemoveAt(i);
                        break;
                    }
                }
            }
            string strScriptName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            string settingsDirectory = GetAppDirectoryForScript(strScriptName);
            string fileName = ((ComboBox)sender).Name + ".txt";
            string settingsPath = System.IO.Path.Combine(settingsDirectory, fileName);
            using (StreamWriter objSWFile = File.CreateText(settingsPath)) {
                foreach (ComboBoxPair item in alHostsNew.OrderBy(x => x._Key)) {
                    objSWFile.WriteLine(item._Key + '^' + item._Value);
                }
                objSWFile.Close();
            }

            //  alHosts = alHostsNew;
            if (boolNewItem) {
                ((ComboBox)sender).ItemsSource = alHostsNew;
            }
        }

        protected void button_Click(object sender, EventArgs e) {
            Button button = sender as Button;
            strButtonClickedName = button.Name;

            // identify which button was clicked and perform necessary actions
            foreach (ControlEntity item in _ListControlEntity) {
                switch (item.ControlType) {
                    case ControlType.Button:
                        if (item.ID == strButtonClickedName) {
                            item.ButtonPressed = true;
                        } else {
                            item.ButtonPressed = false;
                        }
                        break;
                    case ControlType.TextBox:
                        TextBox myTextBox = new TextBox();
                        myTextBox = (TextBox)LogicalTreeHelper.FindLogicalNode(this, item.ID);
                        item.Text = myTextBox.Text;
                        break;
                    case ControlType.PasswordBox:
                        PasswordBox myPasswordBox = new PasswordBox();
                        myPasswordBox = (PasswordBox)LogicalTreeHelper.FindLogicalNode(this, item.ID);
                        item.Text = myPasswordBox.Password;
                        break;
                    case ControlType.ComboBox:
                        ComboBox myComboBox = new ComboBox();
                        myComboBox = (ComboBox)LogicalTreeHelper.FindLogicalNode(this, item.ID);
                        if (myComboBox.SelectedIndex > -1) {
                            item.SelectedValue = myComboBox.SelectedValue.ToString();
                            ComboBoxPair mySelectedPair = (ComboBoxPair)(myComboBox.SelectedItem);
                            item.SelectedKey = mySelectedPair._Key;
                        }
                        break;
                    case ControlType.CheckBox:
                        CheckBox myCheckBox = new CheckBox();
                        myCheckBox = (CheckBox)LogicalTreeHelper.FindLogicalNode(this, item.ID);
                        item.Checked = myCheckBox.IsChecked ?? false;
                        break;

                    default:
                        break;
                }
            }

            this.Close();
        }
        private void ComboBox_Loaded(object sender, RoutedEventArgs e) {

            Label1.Text = _Label;

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.DisplayMemberPath = "_Key";
            comboBox.SelectedValuePath = "_Value";



        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            ComboBoxPair cbp = (ComboBoxPair)comboBox.SelectedItem;
            this.Title = "Selected: " + cbp._Value;
            SelectedComboBoxPair = cbp;
        }

        private void btnOkay_Click(object sender, RoutedEventArgs e) {
            //boolOkayPressed = true;
            foreach (ControlEntity item in _ListControlEntity) {
                switch (item.ControlType) {
                    case ControlType.TextBox:
                        TextBox myTextBox = new TextBox();
                        myTextBox = (TextBox)LogicalTreeHelper.FindLogicalNode(this, item.ID);
                        item.Text = myTextBox.Text;
                        break;
                    case ControlType.PasswordBox:
                        PasswordBox myPasswordBox = new PasswordBox();
                        myPasswordBox = (PasswordBox)LogicalTreeHelper.FindLogicalNode(this, item.ID);
                        item.Text = myPasswordBox.Password;
                        break;
                    case ControlType.ComboBox:
                        ComboBox myComboBox = new ComboBox();
                        myComboBox = (ComboBox)LogicalTreeHelper.FindLogicalNode(this, item.ID);
                        if (myComboBox.SelectedIndex > -1) {
                            item.SelectedValue = myComboBox.SelectedValue.ToString();
                            ComboBoxPair mySelectedPair = (ComboBoxPair)(myComboBox.SelectedItem);
                            item.SelectedKey = mySelectedPair._Key;
                        }
                        break;
                    case ControlType.CheckBox:
                        CheckBox myCheckBox = new CheckBox();
                        myCheckBox = (CheckBox)LogicalTreeHelper.FindLogicalNode(this, item.ID);
                        item.Checked = myCheckBox.IsChecked ?? false;
                        break;
                    case ControlType.Image:
                        Image myImage = new Image();
                        myImage = (Image)LogicalTreeHelper.FindLogicalNode(this, item.ID);
                        item.Source = myImage.Source;
                        break;
                    default:
                        break;
                }
            }
            strButtonClickedName = "btnOkay";
            boolOkayPressed = true;
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            //if (boolOkayPressed == false) {
            //  ComboBoxPair cbp = new ComboBoxPair("","");
            //  SelectedComboBoxPair = cbp;
            //}

            //foreach (System.Windows.Window win in System.Windows.Application.Current.Windows) {
            //    string name = win.Name;
            Methods myActions = new Methods();
            //myActions.SetValueByKey("WindowTop", this.Top.ToString(), "IdealAutomateDB");
            //myActions.SetValueByKey("WindowLeft", this.Left.ToString(), "IdealAutomateDB");


            //}
            base.OnClosing(e);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            strButtonClickedName = "btnCancel";
            this.Close();
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {

            base.OnMouseLeftButtonDown(e);
            WindowState = WindowState.Normal;
            DragMove();
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e) {
            base.OnMouseRightButtonDown(e);
        }
        private string GetAppDirectoryForScript(string strScriptName) {
            string settingsDirectory =
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\" + strScriptName;
            if (!Directory.Exists(settingsDirectory)) {
                Directory.CreateDirectory(settingsDirectory);
            }
            return settingsDirectory;
        }
    }
}
