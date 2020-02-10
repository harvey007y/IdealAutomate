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
            //Version version = typeof(TreeViewBase<object>).Assembly.GetName().Version;
            string version = null;
            try {
                //// get deployment version
                var obj = Assembly.GetExecutingAssembly().GetName().Version;
                 version = string.Format("Application Version {0}.{1}", obj.Build, obj.Revision);
            } catch (Exception ex) {
                //// you cannot read publish version when app isn't installed 
                //// (e.g. during debug)
                version = "not installed";
            }
            // version = "not installed";
            txtVersion.Text = version;
            txtYear.Text = System.DateTime.Now.Year.ToString();
    }
        public static string GetAppVersion() {

            //Package package = Package.Current;
            //PackageId packageId = package.Id;
            //PackageVersion version = packageId.Version;

            return "not installed"; // string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);

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