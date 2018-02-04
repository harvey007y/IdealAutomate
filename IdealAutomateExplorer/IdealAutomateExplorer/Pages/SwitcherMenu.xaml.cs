using System.Windows;
using System.Windows.Controls;

namespace Hardcodet.Wpf.Samples.Pages {
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
            //Switcher.Switch(new LocalScripts());
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
           
            //AboutDialog dlg = new AboutDialog();
            //dlg.Owner = (Window)Window.GetWindow(this);
            // Shadow.Visibility = Visibility.Visible;
            //dlg.Show();
            //Shadow.Visibility = Visibility.Collapsed;
        }

        
    }
}
