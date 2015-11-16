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
      
      _Title= myTitle;
      _Label = myContent;
      _Top = intTop;
      _Left = intLeft;
      this.Top = _Top;
      this.Left = _Left;

      InitializeComponent();
      lblContent.Text = myContent;
      lblTitle.Text = myTitle;
      string strShapeOrientation = myShape + myOrientation;
      Uri uri;
      ImageSource mySource;
      switch (strShapeOrientation) {
        case "RedArrowRight":
          uri = new Uri("/IdealAutomateCore;component/Resources/RedArrowRight.png", UriKind.Relative);
          mySource = new BitmapImage(uri);
          imgBackground.Source = mySource;
          break;
        case "RedArrowLeft":
          uri = new Uri("/IdealAutomateCore;component/Resources/RedArrowLeft.png", UriKind.Relative);
           mySource = new BitmapImage(uri);
          imgBackground.Source = mySource;          
          break;
        case "RedArrowDown":
          uri = new Uri("/IdealAutomateCore;component/Resources/RedArrowDown.png", UriKind.Relative);
          mySource = new BitmapImage(uri);
          imgBackground.Source = mySource;
          break;
        case "RedArrowUp":
          uri = new Uri("/IdealAutomateCore;component/Resources/RedArrowUp.png", UriKind.Relative);
          mySource = new BitmapImage(uri);
          imgBackground.Source = mySource;
          break;
        case "RedBox":
          uri = new Uri("/IdealAutomateCore;component/Resources/RedBox.png", UriKind.Relative);
          mySource = new BitmapImage(uri);
          imgBackground.Source = mySource;
          break;
        default:
          break;
      }

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);
        }
    }
}
