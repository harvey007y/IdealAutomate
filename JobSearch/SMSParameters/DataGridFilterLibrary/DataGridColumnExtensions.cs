using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DataGridFilterLibrary
{
    public class DataGridColumnExtensions
    {
        public static DependencyProperty IsCaseSensitiveSearchProperty =
            DependencyProperty.RegisterAttached("IsCaseSensitiveSearch",
                typeof(bool), typeof(DataGridColumnExtensions));

        public static bool GetIsCaseSensitiveSearch(DependencyObject target)
        {
            return (bool)target.GetValue(IsCaseSensitiveSearchProperty);
        }

        public static void SetIsCaseSensitiveSearch(DependencyObject target, bool value)
        {
            target.SetValue(IsCaseSensitiveSearchProperty, value);
        }

        public static DependencyProperty IsBetweenFilterControlProperty =
            DependencyProperty.RegisterAttached("IsBetweenFilterControl",
                typeof(bool), typeof(DataGridColumnExtensions));

        public static bool GetIsBetweenFilterControl(DependencyObject target)
        {
            return (bool)target.GetValue(IsBetweenFilterControlProperty);
        }

        public static void SetIsBetweenFilterControl(DependencyObject target, bool value)
        {
            target.SetValue(IsBetweenFilterControlProperty, value);
        }

        public static DependencyProperty DoNotGenerateFilterControlProperty =
            DependencyProperty.RegisterAttached("DoNotGenerateFilterControl",
                typeof(bool), typeof(DataGridColumnExtensions), new PropertyMetadata(false));

        public static bool GetDoNotGenerateFilterControl(DependencyObject target)
        {
            return (bool)target.GetValue(DoNotGenerateFilterControlProperty);
        }

        public static void SetDoNotGenerateFilterControl(DependencyObject target, bool value)
        {
            target.SetValue(DoNotGenerateFilterControlProperty, value);
        }
    }
}
