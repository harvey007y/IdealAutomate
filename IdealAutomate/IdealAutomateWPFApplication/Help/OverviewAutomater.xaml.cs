using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Hardcodet.Wpf.Samples.Help
{
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class OverviewAutomater : Page
    {
        public OverviewAutomater()
        {
            InitializeComponent();
        }
        private void OnNavigationRequest(object sender, System.Windows.Navigation.RequestNavigateEventArgs e) {
          Process.Start(e.Uri.ToString());
          e.Handled = true;
        }
    }
}
