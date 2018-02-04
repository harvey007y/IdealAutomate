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
            frame.Source = new Uri("Install.xaml", UriKind.RelativeOrAbsolute);
        }
       
        
        private void Install_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Install.xaml", UriKind.RelativeOrAbsolute);
        }
        private void LaunchExecutable_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("LaunchExecutable.xaml", UriKind.RelativeOrAbsolute);
        }
        private void FindImage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("FindImage.xaml", UriKind.RelativeOrAbsolute);
        }
        private void AutomateLogin_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("AutomateLogin.xaml", UriKind.RelativeOrAbsolute);
        }

        private void AffiliateProgram_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("AffiliateProgram.xaml", UriKind.RelativeOrAbsolute);
        }
        

       
    }

