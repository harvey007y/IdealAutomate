using System.Windows;
using IdealAutomate.Core;
using System.Collections.Generic;
using Vigo.AdvancedClipboard;
using System.IO;

namespace RetrieveClips {
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
      if (strWindowTitle.StartsWith("RetrieveClips")) {
        myActions.TypeText("%(\" \"n)", 1000); // minimize visual studio
      }
      myActions.Sleep(1000);
			int intRowCtr = 0;
			ControlEntity myControlEntity = new ControlEntity();
			List<ControlEntity> myListControlEntity = new List<ControlEntity>();
			List<ComboBoxPair> cbp = new List<ComboBoxPair>();
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Heading;
			myControlEntity.ID = "lblListofClips";
			myControlEntity.Text = "";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			intRowCtr++;
			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.Label;
			myControlEntity.ID = "lblClips";
			myControlEntity.Text = "Clips";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ColumnNumber = 0;
			myControlEntity.ColumnSpan = 1;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			myControlEntity.ControlEntitySetDefaults();
			myControlEntity.ControlType = ControlType.ComboBox;
			cbp.Clear();
			 string root = @"C:\Data\Clips";
			string[] subdirectoryEntries = Directory.GetDirectories(root);
			cbp.Add(new ComboBoxPair("--select clip--", "--select clip--"));
			foreach (var item in subdirectoryEntries)
				
            {
				string itemx = item.Replace(root + @"\", "");
				cbp.Add(new ComboBoxPair(itemx, itemx));
			}

			myControlEntity.ListOfKeyValuePairs = cbp;
			myControlEntity.SelectedValue = "--select clip--";
			myControlEntity.ID = "cbxClips";
			myControlEntity.RowNumber = intRowCtr;
			myControlEntity.ToolTipx = "Select clip to put in clipboard";
			myControlEntity.DDLName = "";
			myControlEntity.ColumnNumber = 1;
			myControlEntity.ColumnSpan = 0;
			myListControlEntity.Add(myControlEntity.CreateControlEntity());

			string strButtonPressed = myActions.WindowMultipleControls(ref myListControlEntity, 400, 500, 0, 0);
			if (strButtonPressed == "btnCancel")
			{
				myActions.MessageBoxShow("Okay button not pressed - Script Cancelled");
				goto myExit;
			}

			string strClips = myListControlEntity.Find(x => x.ID == "cbxClips").SelectedValue;
			string fileName1 = @"C:\Data\Clips\" + strClips;
			ClipboardHelper.Deserialize(fileName1);
		myExit:
      myActions.ScriptEndedSuccessfullyUpdateStats();
      Application.Current.Shutdown();
    }
  }
}
