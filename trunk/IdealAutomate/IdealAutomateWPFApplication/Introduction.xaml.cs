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

namespace Hardcodet.Wpf.Samples
{
    /// <summary>
    /// Interaction logic for Introduction.xaml
    /// </summary>
    public partial class Introduction : UserControl
    {
         
        public static string mySelectedItem;
        
        public Introduction()
        {
            InitializeComponent();
           
             
        }
        
        public void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = ShowInputDialog(null);

            if (name != "")
            {
                System.Windows.Controls.ListBoxItem myItem = new ListBoxItem();
                myItem.Content = name;
                lbContent.Items.Add(myItem);
                

                //string myString = myMainWindow.txtNewItem.Text;
            }
        }
        public void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            lbContent.Items.Remove(lbContent.SelectedItem);

        }
        public void btnLaunch_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem item in lbContent.SelectedItems)
            {
                string vsPath = "C:\\Program Files\\Internet Explorer\\iexplore.exe";
                Process.Start(vsPath, string.Concat("\"", item.Content, "\""));
            }

        }

        public void OnNavigationRequest(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
            e.Handled = true;
        }

        public void lbContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public string ShowInputDialog(string defaultValue)
        {
            InputDialog dlg = new InputDialog();
            dlg.CategoryName = defaultValue;
            //dlg.Owner = this;
            dlg.ShowDialog();

            return dlg.CategoryName;

        }

    }
}
