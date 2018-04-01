using Hardcodet.Wpf.Samples;
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
    public partial class VideoTutorials 
    {
        public VideoTutorials()
        {
            InitializeComponent();
            frame.Source = new Uri("GettingStarted.xaml", UriKind.RelativeOrAbsolute);
        }
       
        
        private void GettingStarted_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("GettingStarted.xaml", UriKind.RelativeOrAbsolute);
        }
        private void BasicFeatures_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("BasicFeatures.xaml", UriKind.RelativeOrAbsolute);
        }
        private void SetBreakpoints_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("SetBreakpoints.xaml", UriKind.RelativeOrAbsolute);
        }
        private void CreateExecutablesMenu_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("CreateExecutablesMenu.xaml", UriKind.RelativeOrAbsolute);
        }

        private void WrapTextInQuotes_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("WrapTextInQuotes.xaml", UriKind.RelativeOrAbsolute);
        }
    private void WrapSqlInQuotes_MouseLeftButtonDown(object sender, RoutedEventArgs e) {
        frame.Source = new Uri("WrapSqlInQuotes.xaml", UriKind.RelativeOrAbsolute);
    }

    private void frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e) {

    }
}

