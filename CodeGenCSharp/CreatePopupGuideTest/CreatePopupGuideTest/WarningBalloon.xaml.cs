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
using System.Windows.Shapes;

namespace CreatePopupGuideTest
{
    /// <summary>
    /// Interaction logic for WarningBalloon.xaml
    /// </summary>
    public partial class WarningBalloon : Window
    {
        public WarningBalloon()
        {
            InitializeComponent();
            this.UpperLeft.Visibility = Visibility.Hidden;
            this.BottomLeft.Visibility = Visibility.Hidden;
            this.BottomRight.Visibility = Visibility.Hidden;
            
        }
    }
}
