using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hardcodet.Wpf.Samples
{
  /// <summary>
  /// Interaction logic for InputDialog.xaml
  /// </summary>
  public partial class InputDialog : Window
  {
    public string CategoryName
    {
      get { return txtCategory.Text; }
      set { txtCategory.Text = value; }
    }

    public InputDialog()
    {
      InitializeComponent();
      txtCategory.Focus();
    }


    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = true;
    }
    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        txtCategory.Text = "";
        DialogResult = false;
    }
  }
}
