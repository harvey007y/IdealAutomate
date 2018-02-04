using Hardcodet.Wpf.Samples;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


    /// <summary>
    /// Interaction logic for NavigationWindow.xaml
    /// </summary>
    public partial class NavWindowAutomater 
    {
        public NavWindowAutomater()
        {
            InitializeComponent();
            frame.Source = new Uri("OverviewAutomater.xaml", UriKind.RelativeOrAbsolute);
        }
        public NavWindowAutomater(string myVerb)
        {
            InitializeComponent();
            frame.Source = new Uri("OverviewAutomater.xaml", UriKind.RelativeOrAbsolute);
            if (myVerb == "IfElseEndIf")
            {
                frame.Source = new Uri("IfElseEndIf.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "AddSQLParameter")
            {
                frame.Source = new Uri("AddSQLParameter.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "ClearSQLParameters")
            {
                frame.Source = new Uri("ClearSQLParameters.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "ExecuteSQLQueryCommand")
            {
                frame.Source = new Uri("ExecuteSQLQueryCommand.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "ExecuteSQLNonQueryCommand")
            {
                frame.Source = new Uri("ExecuteSQLNonQueryCommand.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "ExecuteSQLScalarCommand")
            {
                frame.Source = new Uri("ExecuteSQLScalarCommand.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "ClickImageIfExists")
            {
                frame.Source = new Uri("ClickImageIfExists.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "GoToLabel")
            {
                frame.Source = new Uri("GoToLabel.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "IncludeScript")
            {
                frame.Source = new Uri("IncludeScript.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "Label")
            {
                frame.Source = new Uri("Label.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "LeftClick")
            {
                frame.Source = new Uri("LeftClick.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "MessageBox")
            {
                frame.Source = new Uri("MessageBox.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "PositionCursor")
            {
                frame.Source = new Uri("PositionCursor.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "PutAll")
            {
                frame.Source = new Uri("PutAll.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "PutCaretPositionInArray")
            {
                frame.Source = new Uri("PutCaretPositionInArray.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "PutClipboardInEntity")
            {
                frame.Source = new Uri("PutClipboardInEntity.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "PutCursorPosition")
            {
                frame.Source = new Uri("PutCursorPosition.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "PutEntityInClipboard")
            {
                frame.Source = new Uri("PutEntityInClipboard.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "ReplaceAllStringInEntity")
            {
                frame.Source = new Uri("ReplaceAllStringInEntity.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "RightClick")
            {
                frame.Source = new Uri("RightClick.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "Run")
            {
                frame.Source = new Uri("Run.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "Set")
            {
                frame.Source = new Uri("Set.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "ShiftClick")
            {
                frame.Source = new Uri("ShiftClick.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "Sleep")
            {
                frame.Source = new Uri("Sleep.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "SubImage")
            {
                frame.Source = new Uri("SubImage.xaml", UriKind.RelativeOrAbsolute);
            }
            if (myVerb == "TypeText")
            {
                frame.Source = new Uri("TypeText.xaml", UriKind.RelativeOrAbsolute);
            }
        }
        private void ProgramOverview_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("OverviewAutomater.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Benefits_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("AutomaterBenefits.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Tips_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Tips.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Description_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Description.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Primitives_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Primitives.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Arrays_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Arrays.xaml", UriKind.RelativeOrAbsolute);
        }

        private void Images_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Images.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Expressions_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Expressions.xaml", UriKind.RelativeOrAbsolute);
        }
        private void SQLCmds_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("SQLCmds.xaml", UriKind.RelativeOrAbsolute);
        }
        private void SQLParms_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("SQLParms.xaml", UriKind.RelativeOrAbsolute);
        }
        private void ClearSQLParameters_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("ClearSQLParameters.xaml", UriKind.RelativeOrAbsolute);
        }
        private void ExecuteSQLQueryCommand_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("ExecuteSQLQueryCommand.xaml", UriKind.RelativeOrAbsolute);
        }
        private void ExecuteSQLNonQueryCommand_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("ExecuteSQLNonQueryCommand.xaml", UriKind.RelativeOrAbsolute);
        }
        private void ExecuteSQLScalarCommand_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("ExecuteSQLScalarCommand.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Logic_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Logic.xaml", UriKind.RelativeOrAbsolute);
        }
        private void IfElseEndIf_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("IfElseEndIf.xaml", UriKind.RelativeOrAbsolute);
        }
        private void AddSQLParameter_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("AddSQLParameter.xaml", UriKind.RelativeOrAbsolute);
        }
        private void ClickImageIfExists_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("ClickImageIfExists.xaml", UriKind.RelativeOrAbsolute);
        }
        private void GoToLabel_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("GoToLabel.xaml", UriKind.RelativeOrAbsolute);
        }
        private void IncludeScript_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("IncludeScript.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Label_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Label.xaml", UriKind.RelativeOrAbsolute);
        }
        private void LeftClick_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("LeftClick.xaml", UriKind.RelativeOrAbsolute);
        }
        private void MessageBox_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("MessageBox.xaml", UriKind.RelativeOrAbsolute);
        }
        private void PositionCursor_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("PositionCursor.xaml", UriKind.RelativeOrAbsolute);
        }
        private void PutAll_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("PutAll.xaml", UriKind.RelativeOrAbsolute);
        }
        private void PutCaretPositionInArray_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("PutCaretPositionInArray.xaml", UriKind.RelativeOrAbsolute);
        }
        private void PutClipboardInEntity_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("PutClipboardInEntity.xaml", UriKind.RelativeOrAbsolute);
        }
        private void PutCursorPosition_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("PutCursorPosition.xaml", UriKind.RelativeOrAbsolute);
        }
        private void PutEntityInClipboard_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("PutEntityInClipboard.xaml", UriKind.RelativeOrAbsolute);
        }
        private void RightClick_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("RightClick.xaml", UriKind.RelativeOrAbsolute);
        }
        private void ReplaceAllStringInEntity_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("ReplaceAllStringInEntity.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Run_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Run.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Set_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Set.xaml", UriKind.RelativeOrAbsolute);
        }
        private void ShiftClick_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("ShiftClick.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Sleep_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Sleep.xaml", UriKind.RelativeOrAbsolute);
        }
        private void SubImage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("SubImage.xaml", UriKind.RelativeOrAbsolute);
        }
        private void TypeText_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("TypeText.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Results_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Results.xaml", UriKind.RelativeOrAbsolute);
        }
        private void Shortcuts_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("Shortcuts.xaml", UriKind.RelativeOrAbsolute);
        }
        private void WindowsTaskScheduler_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            frame.Source = new Uri("WindowsTaskScheduler.xaml", UriKind.RelativeOrAbsolute);
        }

       
    }

