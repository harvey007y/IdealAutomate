using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;


namespace Hardcodet.Wpf.Samples
{
  /// <summary>
  /// Interaction logic for AboutDialog.xaml
  /// </summary>
  public partial class IdealAutomaterDialog : Window
  {
    public IdealAutomaterDialog()
    {
      InitializeComponent();

      //set version number
     // Version version = typeof(TreeViewBase<object>).Assembly.GetName().Version;
      txtVersion.Text = "1.0";
    }


    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = true;
    }


    private void OnNavigationRequest(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
    {
      Process.Start(e.Uri.ToString());
      e.Handled = true;
    }
  }
}