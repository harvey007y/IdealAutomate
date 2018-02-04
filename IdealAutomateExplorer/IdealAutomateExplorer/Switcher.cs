using System.Windows.Controls;

namespace Hardcodet.Wpf.Samples
{
  	public static class Switcher
  	{
        public static MainWindow pageSwitcher;

    	public static void Switch(UserControl newPage)
    	{
      		pageSwitcher.Navigate(newPage);
    	}    	
  	}
}
