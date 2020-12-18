using DataGridFilterTest;
using IdealAutomate.Core;
using SMSParameters;
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


/// <summary>
/// Interaction logic for NavigationWindow.xaml
/// </summary>
public partial class MainWindowJobs
{
    public MainWindowJobs()
    {
        MessageBox.Show("Hi0");
        InitializeComponent();
        Methods myActions = new Methods();
        MessageBox.Show("Hi1");
        ConnectionString1.GetConnectionString(myActions);
        MessageBox.Show("Hi2");
        if (String.IsNullOrEmpty(myActions.GetValueByKey("JobsConnectionString")))
        {
            frame.Source = new Uri("ModifyConnectionsPage.xaml", UriKind.RelativeOrAbsolute);
        }
        else
        {
            frame.Source = new Uri("Overview.xaml", UriKind.RelativeOrAbsolute);
        }
    }

    private void ModifyConnections_MouseLeftButtonDown(object sender, RoutedEventArgs e)
    {
        frame.Source = new Uri("ModifyConnectionsPage.xaml", UriKind.RelativeOrAbsolute);
    }
    private void SMS_MouseLeftButtonDown(object sender, RoutedEventArgs e)
    {
        frame.Source = new Uri("SMSPage.xaml", UriKind.RelativeOrAbsolute);
    }
    private void JobBoards_MouseLeftButtonDown(object sender, RoutedEventArgs e)
    {
        frame.Source = new Uri("JobBoardsPage.xaml", UriKind.RelativeOrAbsolute);
    }
    private void Keywords_MouseLeftButtonDown(object sender, RoutedEventArgs e)
    {
        frame.Source = new Uri("KeywordsPage.xaml", UriKind.RelativeOrAbsolute);
    }
    private void Locations_MouseLeftButtonDown(object sender, RoutedEventArgs e)
    {
        frame.Source = new Uri("LocationsPage.xaml", UriKind.RelativeOrAbsolute);
    }

    private void Search_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        frame.Source = new Uri("JobSearchPage.xaml", UriKind.RelativeOrAbsolute);
    }

    private void Show_MouseLeftButtonDown(object sender, RoutedEventArgs e)
    {
        // frame.Source = new Uri("ShowApplicationsPage.xaml", UriKind.RelativeOrAbsolute);
        Window showApplicationPage = new ShowApplicationsPage();
        showApplicationPage.ShowDialog();
    }
}

