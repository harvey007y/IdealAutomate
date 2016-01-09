using System;
using System.Collections.Generic;
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
    int _Top;
    int _Left;
    public string SelectedValue { get; set; }
    public WindowMultipleControls(ref List<ControlEntity> myListControlEntity, int intWindowHeight, int intWindowWidth, int intTop, int intLeft) {
      _Top = intTop;
      _Left = intLeft;
      this.Top = _Top;
      this.Left = _Left;
      if (intTop < 0 || intLeft < 0) {
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
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
            Grid.SetRow(myLabel, item.RowNumber);
            Grid.SetColumn(myLabel, item.ColumnNumber);
            if (item.Width > 0) {
              myLabel.Width = item.Width;
            }
            myGrid.Children.Add(myLabel);
            break;
          case ControlType.TextBox:
            TextBox myTextBox = new TextBox();
            myTextBox.Text = item.Text;
            myTextBox.Name = item.ID;
            if (item.Width > 0) {
              myTextBox.Width = item.Width;
            }
            Grid.SetRow(myTextBox, item.RowNumber);
            Grid.SetColumn(myTextBox, item.ColumnNumber);
            myGrid.Children.Add(myTextBox);
            break;
          case ControlType.ComboBox:
            ComboBox myComboBox = new ComboBox();
            myComboBox.Name = item.ID;
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
      boolOkayPressed = true;
      this.Close();
    }
   
    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
{
  //if (boolOkayPressed == false) {
  //  ComboBoxPair cbp = new ComboBoxPair("","");
  //  SelectedComboBoxPair = cbp;
  //}

 	 base.OnClosing(e);
}

    private void btnCancel_Click(object sender, RoutedEventArgs e) {
     
      this.Close();
    }
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
      base.OnMouseLeftButtonDown(e);
      DragMove();
    }

    protected override void OnMouseRightButtonDown(MouseButtonEventArgs e) {
      base.OnMouseRightButtonDown(e);
    }

  }
}
