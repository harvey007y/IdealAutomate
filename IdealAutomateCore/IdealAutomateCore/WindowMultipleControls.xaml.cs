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

namespace IdealAutomate.Core {
    /// <summary>
    /// Interaction logic for WindowComboBox.xaml
    /// </summary>
    public partial class WindowMultipleControls : Window {

        string _Label;

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
                if (item.ColumnNumber > intMaxColumns) {
                    intMaxColumns = item.ColumnNumber;
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
                        if (item.Width > 0) {
                            myLabel.Width = item.Width;
                        }
                        myGrid.Children.Add(myLabel);
                        break;
                    case ControlType.Button:
                        Button button = new Button();
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
                        if (item.Width > 0) {
                            button.Width = item.Width;
                        }
                        button.Margin = new Thickness(1, 1, 1, 1);
                        myGrid.Children.Add(button);
                        break;
                    case ControlType.TextBox:
                        TextBox myTextBox = new TextBox();
                        myTextBox.Text = item.Text;
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
                        myGrid.Children.Add(myTextBox);
                        break;
                    case ControlType.ComboBox:
                        ComboBox myComboBox = new ComboBox();
                        myComboBox.Name = item.ID;
                        if (item.ListOfKeyValuePairs.Count == 0) {
                            List<ComboBoxPair> cbp = new List<ComboBoxPair>();
                            cbp.Clear();
                            cbp.Add(new ComboBoxPair("--Select Item ---", "--Select Item ---"));
                            SqlConnection con = new SqlConnection("Server=(local)\\SQLEXPRESS;Initial Catalog=IdealAutomateDB;Integrated Security=SSPI");
                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandText = "SELECT lk.inc, i.listItemKey, i.ListItemValue FROM LkDDLNamesItems lk " +
        "join DDLNames n on n.inc = lk.DDLNamesInc " +
        "join DDLItems i on i.inc = lk.ddlItemsInc " +
        "where n.ID = @ID ";
                            cmd.Parameters.Add("@ID", SqlDbType.VarChar, -1);
                            cmd.Parameters["@ID"].Value = item.ID;
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
                                    item.SelectedValue = strDefaultValue.ToString();
                                }
                            } finally {
                                con.Close();
                            }

                        }
                        myComboBox.ItemsSource = item.ListOfKeyValuePairs;
                        Grid.SetRow(myComboBox, item.RowNumber);
                        Grid.SetColumn(myComboBox, item.ColumnNumber);
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
                        myCheckBox.Name = item.ID;
                        myCheckBox.Content = item.Text;
                        myCheckBox.IsChecked = item.Checked;
                        Grid.SetRow(myCheckBox, item.RowNumber);
                        Grid.SetColumn(myCheckBox, item.ColumnNumber);
                        if (item.Width > 0) {
                            myCheckBox.Width = item.Width;
                        }
                        myGrid.Children.Add(myCheckBox);
                        break;
                    default:
                        break;
                }
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
            strButtonClickedName = "btnOkay";
            boolOkayPressed = true;
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            //if (boolOkayPressed == false) {
            //  ComboBoxPair cbp = new ComboBoxPair("","");
            //  SelectedComboBoxPair = cbp;
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

    }
}
