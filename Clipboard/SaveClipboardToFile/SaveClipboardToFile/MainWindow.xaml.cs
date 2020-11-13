using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Vigo.AdvancedClipboard;

namespace SaveClipboardToFile {
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
      if (strWindowTitle.StartsWith("SaveClipboardToFile")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(1000);
			int intRowCtr = 0;
			ControlEntity myControlEntity = new ControlEntity();
			List<ControlEntity> myListControlEntity = new List<ControlEntity>();
			List<ComboBoxPair> cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lbl";
			myControlEntity.Text = "File Name for Saved Clip";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lblFileName";
			myControlEntity.Text = "FileName:";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.TextBox;
			myControlEntity.ID = "txtFileName";
			myControlEntity.Text = myActions.GetValueByKey("FileName"); ;
			myControlEntity.ToolTipx = "Enter filename for saved clip";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 1;
			myControlEntity.ColumnSpan = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());
			string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			string strFileName = myListControlEntity.Find(x => x.ID == "txtFileName").Text;
			string fileName1 = @"C:\Data\Clips\" + strFileName;

			ReadOnlyCollection<DataClip> myClipData = ClipboardHelper.GetClipboard();
			ClipboardHelper.SaveToFile(myClipData, fileName1);

		myExit:
      myActions.ScriptEndedSuccessfullyUpdateStats();
      Application.Current.Shutdown();
    }
  }
}
