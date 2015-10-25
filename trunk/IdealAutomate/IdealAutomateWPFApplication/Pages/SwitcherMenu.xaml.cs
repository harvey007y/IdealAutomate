
using System;
using System.Collections.Generic;
using System.Linq;
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

using Hardcodet.Wpf.Samples;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media;
using System.Threading;
using System.Text;
using System.Diagnostics;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.Management;
using Hardcodet.Wpf.Samples.Help;
using Hardcodet.Wpf.Samples.Videos;

namespace Hardcodet.Wpf.Samples.Pages
{
    /// <summary>
    /// Logica di interazione per SwitcherMenu.xaml
    /// </summary>
    public partial class SwitcherMenu : UserControl
    {
        public SwitcherMenu()
        {
            InitializeComponent();
            if (MainWindow.lastMenuItem == "local" || MainWindow.lastMenuItem == null)
            {
                MenuItemLocal.Background = System.Windows.Media.Brushes.DarkGray;                
            }
         
            if (MainWindow.lastMenuItem == "help")
            {
                MenuItemHelp.Background = System.Windows.Media.Brushes.DarkGray;
            }
            if (MainWindow.lastMenuItem == "about")
            {
                MenuItemAbout.Background = System.Windows.Media.Brushes.DarkGray;
            }
            if (MainWindow.lastMenuItem == "findcolumns")
            {
                MenuItemFindColumns.Background = System.Windows.Media.Brushes.DarkGray;
            }
            if (MainWindow.lastMenuItem == "sqltogrid")
            {
                MenuItemSqlToGrid.Background = System.Windows.Media.Brushes.DarkGray;
            }
        }

        private void Button_Local_Click(object sender, RoutedEventArgs e)
        {            
            MainWindow.lastMenuItem = "local";
            Switcher.Switch(new LocalScripts());
        }

      

      

        private void Button_FindColumns_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.lastMenuItem = "findcolumns";
            Switcher.Switch(new FindColumns());
        }

        private void Button_SqlToGrid_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.lastMenuItem = "sqltogrid";
            Switcher.Switch(new SQLToGrid());
        }

        private void OnNavigationRequest(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

        }
       

        private void Button_Help_Click(object sender, RoutedEventArgs e)
        {
            
            NavWindowAutomater dlg = new NavWindowAutomater();
           dlg.Owner = (Window)Window.GetWindow(this);
            // Shadow.Visibility = Visibility.Visible;
            dlg.Show();
          
        }

        private void Button_Video_Tutorials_Click(object sender, RoutedEventArgs e)
        {

            VideoTutorials dlg = new VideoTutorials();
            dlg.Owner = (Window)Window.GetWindow(this);
            // Shadow.Visibility = Visibility.Visible;
            dlg.Show();

        }
        private void Button_About_Click(object sender, RoutedEventArgs e)
        {
           
            AboutDialog dlg = new AboutDialog();
            dlg.Owner = (Window)Window.GetWindow(this);
            // Shadow.Visibility = Visibility.Visible;
            dlg.Show();
            //Shadow.Visibility = Visibility.Collapsed;
        }

        
    }
}
