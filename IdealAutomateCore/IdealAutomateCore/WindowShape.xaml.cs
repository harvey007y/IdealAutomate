using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public string strButtonClickedName = "";
        IEnumerable<object> _IEnumerable;
    string _Label;
        string _MyCopyableText;
        string _Title;
    int _Top;
    int _Left;
        int _maxLineChars = 0;
    private bool boolOkayPressed = false;
    public ComboBoxPair SelectedComboBoxPair { get; set; }
    public string SelectedValue { get; set; }
    public WindowShape(string myShape, string myOrientation, string myTitle, string myContent, int intTop, int intLeft) {
      
      _Title= myTitle;
      _Label = myContent;
      _Top = intTop;
      _Left = intLeft;
            this.Width = 300; 


      InitializeComponent();
      this.Top = _Top;
      this.Left = _Left;
      lblContent.Text = myContent;
      lblTitle.Text = myTitle;
            txtCopyableText.Visibility = Visibility.Hidden;
            string strShapeOrientation = myShape + myOrientation;
      Uri uri;
      ImageSource mySource;
      switch (strShapeOrientation) {
        case "RedArrowRight":
          uri = new Uri("/IdealAutomateCore;component/Resources/RedArrowRight.png", UriKind.Relative);
          mySource = new BitmapImage(uri);
          imgBackground.Source = mySource;
          lblTitle.Margin = new System.Windows.Thickness(20, 10, 129, 0);
          lblContent.Margin = new System.Windows.Thickness(10, 36, 109, 0);
          lblContent.Height = 125;
          btnLogin.Margin = new System.Windows.Thickness(40, 0, 0, 23.209);
          break;
        case "RedArrowLeft":
          uri = new Uri("/IdealAutomateCore;component/Resources/RedArrowLeft.png", UriKind.Relative);
           mySource = new BitmapImage(uri);
          imgBackground.Source = mySource;
          lblTitle.Margin = new System.Windows.Thickness(118, 10, 31, 0);
          lblContent.Margin = new System.Windows.Thickness(118, 36, 1, 0);
          lblContent.Height = 125;
          btnLogin.Margin = new System.Windows.Thickness(118, 0, 0, 27);
          break;
        case "RedArrowDown":
          uri = new Uri("/IdealAutomateCore;component/Resources/RedArrowDown.png", UriKind.Relative);
          mySource = new BitmapImage(uri);
          imgBackground.Source = mySource;
          lblTitle.Margin = new System.Windows.Thickness(20, 10, 129, 0);
          lblContent.Margin = new System.Windows.Thickness(10, 36, 20, 0);
          lblContent.Height = 94;
          btnLogin.Margin = new System.Windows.Thickness(10, 0, 0, 98);
          break;
        case "RedArrowUp":
          uri = new Uri("/IdealAutomateCore;component/Resources/RedArrowUp.png", UriKind.Relative);
          mySource = new BitmapImage(uri);
          imgBackground.Source = mySource;
          lblTitle.Margin = new System.Windows.Thickness(10, 102, 139, 0);
          lblContent.Margin = new System.Windows.Thickness(10, 133, 20, 0);
          lblContent.Height = 94;
          btnLogin.Margin = new System.Windows.Thickness(10, 0, 0, 10);
          break;
        case "RedBox":
          uri = new Uri("/IdealAutomateCore;component/Resources/RedBox.png", UriKind.Relative);
          mySource = new BitmapImage(uri);
          imgBackground.Source = mySource;
          lblTitle.Margin = new System.Windows.Thickness(10, 183, 139, 0);
          lblContent.Margin = new System.Windows.Thickness(10, 10, 20, 0);
          lblContent.Height = 94;
          btnLogin.Margin = new System.Windows.Thickness(10, 0, 0, 10);
          break;
        default:
          break;
      }

        }

        public WindowShape(string myShape, string myOrientation, string myTitle, string myContent, string myCopyableText, int intTop, int intLeft)
        {

            _Title = myTitle;
            _Label = myContent;
            _Top = intTop;
            _Left = intLeft;
            _MyCopyableText = myCopyableText;


            InitializeComponent();
        //    this.Height = 800;
            this.Top = _Top;
            this.Left = _Left;
            lblContent.Text = myContent;
            FindWidthInCharsForContent(myContent,myCopyableText);

            lblContent.Height = CalculateStringHeight(myContent, _maxLineChars);
            lblTitle.Text = myTitle;
            txtCopyableText.Text = myCopyableText;
            txtCopyableText.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            txtCopyableText.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            txtCopyableText.Height = CalculateStringHeight(myCopyableText, _maxLineChars);
            this.Height = lblContent.Height + txtCopyableText.Height + 100;
            double windowWidth = _maxLineChars * 8;
            //if (windowWidth > 500)
            //{
            //    windowWidth = 500;
            //}
            this.Width = windowWidth;
            string strShapeOrientation = myShape + myOrientation;
            Uri uri;
            ImageSource mySource;
            switch (strShapeOrientation)
            {
                case "RedArrowRight":
                    uri = new Uri("/IdealAutomateCore;component/Resources/RedArrowRight.png", UriKind.Relative);
                    mySource = new BitmapImage(uri);
                    imgBackground.Source = mySource;
                    lblTitle.Margin = new System.Windows.Thickness(windowWidth/15, windowWidth/30, windowWidth/2.33, 0);
                    lblContent.Margin = new System.Windows.Thickness(windowWidth / 30, windowWidth/8.33, windowWidth / 2.33, 0);
                    txtCopyableText.Margin = new System.Windows.Thickness(windowWidth / 30, windowWidth / 8.33, windowWidth / 2.33, 0);
                    //  lblContent.Height = 125;
                    btnLogin.Margin = new System.Windows.Thickness(windowWidth / 7.5, 0, 0, windowWidth / 13);
                    break;
                case "RedArrowLeft":
                    uri = new Uri("/IdealAutomateCore;component/Resources/RedArrowLeft.png", UriKind.Relative);
                    mySource = new BitmapImage(uri);
                    imgBackground.Source = mySource;
                    lblTitle.Margin = new System.Windows.Thickness(windowWidth / 2.50, 10, windowWidth / 15, 0);
                    lblContent.Margin = new System.Windows.Thickness(windowWidth / 2.50, 10, windowWidth / 30, 0);
                    txtCopyableText.Margin = new System.Windows.Thickness(windowWidth / 2.50,10, windowWidth / 30, 0);
                    //lblContent.Height = 125;
                    btnLogin.Margin = new System.Windows.Thickness(windowWidth / 2.50, 10, 0, 10);
                    break;
                case "RedArrowLeftUp":
                    uri = new Uri("/IdealAutomateCore;component/Resources/RedArrowLeftUp.png", UriKind.Relative);
                    mySource = new BitmapImage(uri);
                    imgBackground.Source = mySource;
                    lblTitle.Margin = new System.Windows.Thickness(windowWidth / 2.50, 10, windowWidth / 15, 0);
                    lblContent.Margin = new System.Windows.Thickness(windowWidth / 2.50, 10, windowWidth / 30, 0);
                    txtCopyableText.Margin = new System.Windows.Thickness(windowWidth / 2.50, 10, windowWidth / 30, 0);
                    //lblContent.Height = 125;
                    btnLogin.Margin = new System.Windows.Thickness(windowWidth / 2.50, 10, 0, 10);
                    break;
                case "RedArrowDown":
                    uri = new Uri("/IdealAutomateCore;component/Resources/RedArrowDown.png", UriKind.Relative);
                    mySource = new BitmapImage(uri);
                    imgBackground.Source = mySource;
                    lblTitle.Margin = new System.Windows.Thickness(20, 10, 129, 0);
                    lblContent.Margin = new System.Windows.Thickness(10, 36, 20, 0);
                    txtCopyableText.Margin = new System.Windows.Thickness(10, 36, 20, 0);
                    //  lblContent.Height = 94;
                    btnLogin.Margin = new System.Windows.Thickness(10, 0, 0, 98);
                    break;
                case "RedArrowUp":
                    uri = new Uri("/IdealAutomateCore;component/Resources/RedArrowUp.png", UriKind.Relative);
                    mySource = new BitmapImage(uri);
                    imgBackground.Source = mySource;
                    lblTitle.Margin = new System.Windows.Thickness(10, 102, 139, 0);
                    lblContent.Margin = new System.Windows.Thickness(10, 133, 20, 0);
                    txtCopyableText.Margin = new System.Windows.Thickness(10, 133, 20, 0);
                    // lblContent.Height = 94;
                    btnLogin.Margin = new System.Windows.Thickness(10, 0, 0, 10);
                    break;
                case "RedBox":
                    uri = new Uri("/IdealAutomateCore;component/Resources/RedBox.png", UriKind.Relative);
                    mySource = new BitmapImage(uri);
                    imgBackground.Source = mySource;
                    lblTitle.Margin = new System.Windows.Thickness(10, 10, 139, 0);
                    lblContent.Margin = new System.Windows.Thickness(10, 133, 20, 0);
                    txtCopyableText.Margin = new System.Windows.Thickness(10, 133, 20, 0);
                    // lblContent.Height = 94;
                    btnLogin.Margin = new System.Windows.Thickness(10, 0, 0, 10);
                    break;
                default:
                    break;
            }

        }

        private void FindWidthInCharsForContent(string myContent, string myCopyableText)
        {
          
            var textArr = myContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            //     txtline.Text = textArr.Length.ToString();
     
            foreach (var item in textArr)
            {
                 if (item.Length > _maxLineChars)
                {
                    _maxLineChars = item.Length;
                }
            }

             textArr = myCopyableText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            //     txtline.Text = textArr.Length.ToString();

            foreach (var item in textArr)
            {
                if (item.Length > _maxLineChars)
                {
                    _maxLineChars = item.Length;
                }
            }
            if (_maxLineChars > 70)
            {
                _maxLineChars = 70;
            }
        }

        private double CalculateStringHeight(string myContent, int controlWidthInChars)
        {
            double dblHeight = 0;
            int intCtr = 0;
          //  int intLineWidthInCharacters = 40;
            int intLineHeight = 25;
            int textLength = myContent.Length;
            int intTextBoxHeight = 0;
            if (textLength > 0)
            {
                //var lines = tb.Lines.Count();               
                var textArr = myContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                //     txtline.Text = textArr.Length.ToString();
                int totalNumberOfLines = 0;
                int numberOfLines = 0;
                foreach (var item in textArr)
                {
                    numberOfLines = item.Length / controlWidthInChars;
                    if (numberOfLines < 1)
                    {
                        numberOfLines = 1;
                    }
                    totalNumberOfLines += numberOfLines;

                }

                intTextBoxHeight = totalNumberOfLines * intLineHeight;
 
            }
            if (intTextBoxHeight < 29 )
            {
                intTextBoxHeight = 30;
            }
            if (intTextBoxHeight > 650 - lblContent.Height)
            {
                intTextBoxHeight = 650 - (int)lblContent.Height;
            }
            dblHeight = intTextBoxHeight;
            return dblHeight;
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
