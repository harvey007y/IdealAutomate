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
    public partial class NavWindow 
    {
        public NavWindow()
        {
            InitializeComponent();
            frame.Source = new Uri("Overview.xaml", UriKind.RelativeOrAbsolute);
        }
        private void ProgramOverview_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Overview.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Benefits_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Benefits.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Inputs_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Inputs.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Outputs_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Outputs.xaml", UriKind.RelativeOrAbsolute);
        }
    }

