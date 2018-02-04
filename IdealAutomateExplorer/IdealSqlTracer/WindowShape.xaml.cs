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

namespace IdealAutomate.Core {
    /// <summary>
    /// Interaction logic for WindowComboBox.xaml
    /// </summary>
    public partial class WindowShape : Window {
        IEnumerable<object> _IEnumerable;
        string _Label;
        string _Title;
        int _Top;
        int _Left;
        private bool boolOkayPressed = false;
        public ComboBoxPair SelectedComboBoxPair { get; set; }
        public string SelectedValue { get; set; }
        public WindowShape(string myShape, string myOrientation, string myTitle, string myContent, int intTop, int intLeft) {

            _Title = myTitle;
            _Label = myContent;
            _Top = intTop;
            _Left = intLeft;


            InitializeComponent();
            this.Top = _Top;
            this.Left = _Left;
            lblContent.Text = myContent;
            lblTitle.Text = myTitle;
            string strShapeOrientation = myShape + myOrientation;
            Uri uri;
            ImageSource mySource;
            switch (strShapeOrientation) {
                case "RedArrowRight":
                    uri = new Uri("/IdealSqlTracer;component/Resources/RedArrowRight.png", UriKind.Relative);
                    mySource = new BitmapImage(uri);
                    imgBackground.Source = mySource;
                    lblTitle.Margin = new System.Windows.Thickness(20, 10, 129, 0);
                    lblContent.Margin = new System.Windows.Thickness(10, 36, 109, 0);
                    lblContent.Height = 125;
                    btnLogin.Margin = new System.Windows.Thickness(40, 0, 0, 23.209);
                    break;
                case "RedArrowLeft":
                    uri = new Uri("/IdealSqlTracer;component/Resources/RedArrowLeft.png", UriKind.Relative);
                    mySource = new BitmapImage(uri);
                    imgBackground.Source = mySource;
                    lblTitle.Margin = new System.Windows.Thickness(118, 10, 31, 0);
                    lblContent.Margin = new System.Windows.Thickness(118, 36, 1, 0);
                    lblContent.Height = 125;
                    btnLogin.Margin = new System.Windows.Thickness(118, 0, 0, 27);
                    break;
                case "RedArrowDown":
                    uri = new Uri("/IdealSqlTracer;component/Resources/RedArrowDown.png", UriKind.Relative);
                    mySource = new BitmapImage(uri);
                    imgBackground.Source = mySource;
                    lblTitle.Margin = new System.Windows.Thickness(20, 10, 129, 0);
                    lblContent.Margin = new System.Windows.Thickness(10, 36, 20, 0);
                    lblContent.Height = 94;
                    btnLogin.Margin = new System.Windows.Thickness(10, 0, 0, 98);
                    break;
                case "RedArrowUp":
                    uri = new Uri("/IdealSqlTracer;component/Resources/RedArrowUp.png", UriKind.Relative);
                    mySource = new BitmapImage(uri);
                    imgBackground.Source = mySource;
                    lblTitle.Margin = new System.Windows.Thickness(10, 102, 139, 0);
                    lblContent.Margin = new System.Windows.Thickness(10, 133, 20, 0);
                    lblContent.Height = 94;
                    btnLogin.Margin = new System.Windows.Thickness(10, 0, 0, 10);
                    break;
                case "RedBox":
                    uri = new Uri("/IdealSqlTracer;component/Resources/RedBox.png", UriKind.Relative);
                    mySource = new BitmapImage(uri);
                    imgBackground.Source = mySource;
                    lblTitle.Margin = new System.Windows.Thickness(10, 102, 139, 0);
                    lblContent.Margin = new System.Windows.Thickness(10, 133, 20, 0);
                    lblContent.Height = 94;
                    btnLogin.Margin = new System.Windows.Thickness(10, 0, 0, 10);
                    break;
                case "GreenBox":
                    uri = new Uri("/IdealSqlTracer;component/Resources/GreenBox.png", UriKind.Relative);
                    mySource = new BitmapImage(uri);
                    imgBackground.Source = mySource;
                    lblTitle.Text = "Trace Started";
                    lblTitle.FontSize = 22;
                    lblTitle.Margin = new System.Windows.Thickness(10, 102, 139, 0);
                    lblContent.Margin = new System.Windows.Thickness(10, 133, 20, 0);
                    lblContent.Height = 94;
                    lblContent.FontSize = 12;
                    btnLogin.Margin = new System.Windows.Thickness(10, 0, 0, 10);
                    btnLogin.Content = "Stop";
                    btnLogin.Background = Brushes.Red;
                    btnLogin.Foreground = Brushes.White;                    
                    break;
                default:
                    break;
            }

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e) {
            base.OnMouseRightButtonDown(e);
        }
    }
}
