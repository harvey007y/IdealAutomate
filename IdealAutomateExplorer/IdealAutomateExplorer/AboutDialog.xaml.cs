using System;
//using System.Deployment.Application;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Hardcodet.Wpf.Samples
{
  /// <summary>
  /// Interaction logic for AboutDialog.xaml
  /// </summary>
  public partial class AboutDialog : Window
  {
    public AboutDialog()
    {
      InitializeComponent();

      //set version number
   //   Version version = typeof(TreeViewBase<object>).Assembly.GetName().Version;
      string version = null;
      //try
      //{
      //    //// get deployment version
      //    version = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
      //}
      //catch (InvalidDeploymentException)
      //{
      //    //// you cannot read publish version when app isn't installed 
      //    //// (e.g. during debug)
      //    version = "not installed";
      //}
            version = "not installed";
            txtVersion.Text = version;
            txtYear.Text = System.DateTime.Now.Year.ToString();
    }
    public static Version GetCurrentVersion()
    {
        Version version;

        var assembly = Assembly.GetExecutingAssembly().FullName;
        var fullVersionNumber = assembly.Split('=')[1].Split(',')[0];

        version = new Version(fullVersionNumber);

        return version;
    }


    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }


    private void OnNavigationRequest(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
    {
      Process.Start(e.Uri.ToString());
      e.Handled = true;
    }
  }
}