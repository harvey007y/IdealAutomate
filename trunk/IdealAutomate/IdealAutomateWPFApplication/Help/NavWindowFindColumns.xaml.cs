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
    public partial class NavWindowFindColumns 
    {
        public NavWindowFindColumns()
        {
            InitializeComponent();
            frame.Source = new Uri("OverviewFindColumns.xaml", UriKind.RelativeOrAbsolute);
        }
        private void ProgramOverview_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("OverviewFindColumns.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Benefits_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("BenefitsFindColumns.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Inputs_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("InputsFindColumns.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Outputs_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("OutputsFindColumns.xaml", UriKind.RelativeOrAbsolute);
        }
    }

