


using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;

namespace CreatePopupGuideTest
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            Popup1.CustomPopupPlacementCallback =
                new CustomPopupPlacementCallback(placePopup);
        }

        public CustomPopupPlacement[] placePopup(Size popupSize,
                                           Size targetSize,
                                           Point offset)
        {
            CustomPopupPlacement placement2 =
               new CustomPopupPlacement(new Point(-(popupSize.Width - targetSize.Width / 2), targetSize.Height), PopupPrimaryAxis.Vertical);

            CustomPopupPlacement placement1 =
               new CustomPopupPlacement(new Point(targetSize.Width / 2, targetSize.Height), PopupPrimaryAxis.Vertical);

            CustomPopupPlacement placement3 =
               new CustomPopupPlacement(new Point(targetSize.Width / 2, -popupSize.Height), PopupPrimaryAxis.Horizontal);

            CustomPopupPlacement placement4 =
               new CustomPopupPlacement(new Point(-(popupSize.Width - targetSize.Width / 2), -popupSize.Height), PopupPrimaryAxis.Horizontal);

            CustomPopupPlacement[] ttplaces =
                    new CustomPopupPlacement[] { placement1, placement2, placement3, placement4 };

            return ttplaces;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Popup1.PlacementTarget = sender as Button;
            Popup1.IsOpen = true;
        }

        private void Popup1_Opened(object sender, EventArgs e)
        {
            Path arrow = ((Path)Popup1.FindName("TopArrow"));

            Grid grd = ((Grid)Popup1.FindName("Grd"));
            UIElement elem = (UIElement)Popup1.PlacementTarget;

            Point elem_pos_lefttop = elem.PointToScreen(new Point(0, 0));
            Point popup_pos_lefttop = grd.PointToScreen(new Point(0, 0));

            if ((elem_pos_lefttop.Y < popup_pos_lefttop.Y)
                    &&
                    ((elem_pos_lefttop.X > popup_pos_lefttop.X))
                )
            {
                Canvas.SetLeft(arrow, 280);
                Canvas.SetTop(arrow, 25);
            }
            if ((elem_pos_lefttop.Y < popup_pos_lefttop.Y)
                    &&
                    ((elem_pos_lefttop.X < popup_pos_lefttop.X))
                )
            {
                Canvas.SetLeft(arrow, 30);
                Canvas.SetTop(arrow, 25);
            }
            if ((elem_pos_lefttop.Y > popup_pos_lefttop.Y)
                    &&
                    ((elem_pos_lefttop.X > popup_pos_lefttop.X))
                )
            {
                Canvas.SetLeft(arrow, 280);
                Canvas.SetTop(arrow, 90);
            }
            if ((elem_pos_lefttop.Y > popup_pos_lefttop.Y)
                    &&
                    ((elem_pos_lefttop.X < popup_pos_lefttop.X))
                )
            {
                Canvas.SetLeft(arrow, 30);
                Canvas.SetTop(arrow, 90);
            }

            Tb1.Text = String.Format("Element = {0} \r\n Popup = {1}", elem_pos_lefttop, popup_pos_lefttop);
        }
    }
}
