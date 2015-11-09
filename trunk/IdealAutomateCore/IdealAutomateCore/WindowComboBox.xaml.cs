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
  public partial class WindowComboBox : Window {
    IEnumerable<object> _IEnumerable;
    string _Label;
    private bool boolOkayPressed = false;
    public ComboBoxPair SelectedComboBoxPair { get; set; }
    public string SelectedValue { get; set; }
    public WindowComboBox(IEnumerable<object> myIEnumerable, string myLabel) {
      InitializeComponent();
      _IEnumerable = myIEnumerable;
      _Label = myLabel;


    }
    private void ComboBox_Loaded(object sender, RoutedEventArgs e) {

      Label1.Text = _Label;

      // ... Get the ComboBox reference.
      var comboBox = sender as ComboBox;

      // ... Assign the ItemsSource to the List.
      comboBox.DisplayMemberPath = "_Key";
      comboBox.SelectedValuePath = "_Value";

      
      comboBox.ItemsSource = _IEnumerable;

      // ... Make the first item selected.
      comboBox.SelectedIndex = 0;
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
      boolOkayPressed = true;
      this.Close();
    }
    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
{
  if (boolOkayPressed == false) {
    ComboBoxPair cbp = new ComboBoxPair("","");
    SelectedComboBoxPair = cbp;
  }

 	 base.OnClosing(e);
}

    private void btnCancel_Click(object sender, RoutedEventArgs e) {
      this.Close();
    }

  }
}
