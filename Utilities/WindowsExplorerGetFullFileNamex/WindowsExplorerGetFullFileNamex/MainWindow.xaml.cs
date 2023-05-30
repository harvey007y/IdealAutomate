using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System;

namespace WindowsExplorerGetFullFileNamex {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      bool boolRunningFromHome = false;
      var window = new Window() //make sure the window is invisible
{
        Width = 0,
        Height = 0,
        Left = -2000,
        WindowStyle = WindowStyle.None,
        ShowInTaskbar = false,
        ShowActivated = false,
      };
      window.Show();
      IdealAutomate.Core.Methods myActions = new Methods();
      myActions.ScriptStartedUpdateStats();

      InitializeComponent();
      this.Hide();

      string strWindowTitle = myActions.PutWindowTitleInEntity();
      if (strWindowTitle.StartsWith("WindowsExplorerGetFullFileNamex")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(1000);
          string[] myArgs =   Environment.GetCommandLineArgs();
            foreach (var item in myArgs)
                //{
                //    myActions.WindowShape("RedBox", "", "Step 1", " If you want to exit the tutorial at any time, just hit the pause key on your computer. \nAfter you have completed the action in a step of the tutorial, hit Okay button to continue to next step " + item, 0, 0);
                //}
                myActions.PutEntityInClipboard(myArgs[1]);
            myActions.WindowShape("RedBox", "", "File Name Copied", " Filename copied to clipboard: " + myArgs[1], 0, 0);
     
      goto myExit;
    
      myExit:
      myActions.ScriptEndedSuccessfullyUpdateStats();
      Application.Current.Shutdown();
    }
  }
}
