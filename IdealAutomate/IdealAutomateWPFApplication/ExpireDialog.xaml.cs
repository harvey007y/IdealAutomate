using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.IO;


namespace Hardcodet.Wpf.Samples
{
  /// <summary>
  /// Interaction logic for AboutDialog.xaml
  /// </summary>
  public partial class ExpireDialog : Window
  {
      
    public ExpireDialog()
    {
      InitializeComponent();

      //set version number
     // Version version = typeof(TreeViewBase<object>).Assembly.GetName().Version;
      string settingsDirectory = 
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealToolsOrganizer";
      string fileName;
      string settingsPath;
      string trialDate;
        if (!Directory.Exists(settingsDirectory))
      {
          Directory.CreateDirectory(settingsDirectory);
          fileName = "tasks.txt"; 

          settingsPath = Path.Combine(settingsDirectory, fileName); 
          trialDate = "01/01/2000 12:00:00";
          // Hook a write to the text file.
          StreamWriter writer = new StreamWriter(settingsPath);
          // Rewrite the entire value of s to the file
          writer.Write(trialDate);
          writer.Close();
      }
      fileName = "tasks.txt";

      settingsPath = Path.Combine(settingsDirectory, fileName); 
      StreamReader file = File.OpenText(settingsPath);
      trialDate = file.ReadToEnd();
      file.Close();
        // if firsttime, make trialdate today
      if (DateTime.Parse(trialDate) < DateTime.Parse("1/1/2010"))
      {
          trialDate = DateTime.Today.ToString();
          StreamWriter writer = new StreamWriter(settingsPath);
          // Rewrite the entire value of s to the file
          writer.Write(trialDate);
          writer.Close(); 
          HeaderText.Text = "Expiration Warning: 10 Days";
      }
      else
      {
          TimeSpan span = System.DateTime.Today - DateTime.Parse(trialDate);
          HeaderText.Text = "Expiration Warning: " + (10 - Convert.ToInt32(span.TotalDays)) + " Days";
      }
    }

      
    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
        string settingsDirectory =
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IdealAutomate\\IdealToolsOrganizer";
        string fileName;
        string settingsPath;
        string trialDate;
        fileName = "tasks.txt";

        settingsPath = Path.Combine(settingsDirectory, fileName);
        if (txtRegistration.Text.Trim() == "153-749-1067")
        {
            trialDate = "01/01/2099 12:00:00";
            StreamWriter writer = new StreamWriter(settingsPath);
            // Rewrite the entire value of s to the file
            writer.Write(trialDate);
            writer.Close(); 
                }
        fileName = "tasks.txt";

        settingsPath = Path.Combine(settingsDirectory, fileName);
        StreamReader file = File.OpenText(settingsPath);
        trialDate = file.ReadToEnd();
        file.Close();
        TimeSpan span = System.DateTime.Today - DateTime.Parse(trialDate);
        if (span.TotalDays > 10)
        {
            DialogResult = false;
        }
        else
        {
            DialogResult = true;
        }
    }


    private void OnNavigationRequest(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
    {
        
      Process.Start(e.Uri.ToString());
      e.Handled = true;
    }
  }
}