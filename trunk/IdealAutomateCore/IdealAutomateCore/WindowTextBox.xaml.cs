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
  public partial class WindowTextBox : Window {   
    string _Label;
    public ComboBoxPair SelectedComboBoxPair { get; set; }
    public string TextBoxValue { get; set; }
    public WindowTextBox(string myLabel) {
      InitializeComponent();

      Label1.Text = myLabel;


    }
    

    private void tbTextBox_TextChanged(object sender, TextChangedEventArgs e) {
      TextBoxValue = tbTextBox.Text;
    }

    private void btnOkay_Click(object sender, RoutedEventArgs e) {
      this.Close();
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e) {
      TextBoxValue = "";
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
