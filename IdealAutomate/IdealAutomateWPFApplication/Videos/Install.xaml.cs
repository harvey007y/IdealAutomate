﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hardcodet.Wpf.Samples.Videos
{
    /// <summary>
    /// Interaction logic for CopyrightInformation.xaml
    /// </summary>
    public partial class Install : Page
    {
        public Install()
        {
            InitializeComponent();
        }

       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore", "https://www.youtube.com/watch?v=XI2yGmBN_yU");
        }
    }
}
