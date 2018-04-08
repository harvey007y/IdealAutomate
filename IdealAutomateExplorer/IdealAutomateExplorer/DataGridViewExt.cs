using IdealAutomate.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace System.Windows.Forms.Samples {

	[Description("DataGridView that Saves Column Order, Width and Visibility to appdata\\roaming\\idealautomate\\idealautomateexplorer")]
	[ToolboxBitmap(typeof(System.Windows.Forms.DataGridView))]
	public partial class DataGridViewExt : DataGridView
	{
        string _InitialDirectory = "myInitialDirectory";
        public DataGridViewExt(string InitialDirectory) {
            _InitialDirectory = InitialDirectory;
        }
        public DataGridViewExt() {

        }
		private void SetColumnOrder()
		{
			//if (!DataGridViewExtSetting.Default.ColumnOrder.ContainsKey(_InitialDirectory))
			//	return;

			//List<ColumnOrderItem> columnOrder =
			//	DataGridViewExtSetting.Default.ColumnOrder[_InitialDirectory];

			//if (columnOrder != null)
			//{
			//	var sorted = columnOrder.OrderBy(i => i.DisplayIndex);
			//	foreach (var item in sorted)
			//	{
			//		this.Columns[item.ColumnIndex].DisplayIndex = item.DisplayIndex;
			//		this.Columns[item.ColumnIndex].Visible = item.Visible;
			//		this.Columns[item.ColumnIndex].Width = item.Width;
			//	}
			//}
		}
		//---------------------------------------------------------------------
		private void SaveColumnOrder()
		{
			if (this.AllowUserToOrderColumns)
			{
				List<ColumnOrderItem> columnOrder = new List<ColumnOrderItem>();
				DataGridViewColumnCollection columns = this.Columns;
				for (int i = 0; i < columns.Count; i++)
				{
					columnOrder.Add(new ColumnOrderItem
					{
						ColumnIndex = i,
						DisplayIndex = columns[i].DisplayIndex,
						Visible = columns[i].Visible,
						Width = columns[i].Width
					});
				}

				//DataGridViewExtSetting.Default.ColumnOrder[_InitialDirectory] = columnOrder;
				//DataGridViewExtSetting.Default.Save();
                Methods myActions = new Methods();
                ArrayList myArrayList = new ArrayList();
                StringBuilder sb = new StringBuilder();
                

                foreach (ColumnOrderItem item in columnOrder) {
                    sb.Length = 0;
                    sb.Append(item.DisplayIndex.ToString());
                    sb.Append("^");
                    sb.Append(item.Width.ToString());
                    sb.Append("^");
                    sb.Append(item.Visible.ToString());
                    sb.Append("^");
                    sb.Append(item.ColumnIndex.ToString());                    
                    myArrayList.Add(sb.ToString());
                }
                myActions.WriteArrayListToAppDirectoryKey("ColumnOrder_" + _InitialDirectory.Replace(":","+").Replace("\\","-"), myArrayList);
                if (this.SortedColumn != null) {
                    myActions.SetValueByKey("SortedColumn_" + _InitialDirectory.Replace(":", "+").Replace("\\", "-"), this.SortedColumn.Index.ToString());
                    myActions.SetValueByKey("SortOrder_" + _InitialDirectory.Replace(":", "+").Replace("\\", "-"), this.SortOrder.ToString());
                } else {
                    myActions.SetValueByKey("SortedColumn_" + _InitialDirectory.Replace(":", "+").Replace("\\", "-"), "-1");
                    myActions.SetValueByKey("SortOrder_" + _InitialDirectory.Replace(":", "+").Replace("\\", "-"), this.SortOrder.ToString());
                }
            }
		}
		//---------------------------------------------------------------------
		protected override void OnCreateControl()
		{

			base.OnCreateControl();
            //_InitialDirectory = "myDataGridView";
            this.AllowUserToOrderColumns = true;
			SetColumnOrder();
		}
		//---------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			SaveColumnOrder();
			base.Dispose(disposing);
		}
        /// <summary>
        /// Catching exceptions in onPaint in custom control because that prevents
        /// a big red x from appearing on datagrid when problems occur
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e) {
            try {
                base.OnPaint(e);
            } catch (Exception) {
                this.Invalidate();
            }
        }
    }
	//-------------------------------------------------------------------------
	internal sealed class DataGridViewExtSetting : ApplicationSettingsBase
	{
		private static DataGridViewExtSetting _defaultInstace =
			(DataGridViewExtSetting)ApplicationSettingsBase
			.Synchronized(new DataGridViewExtSetting());
		//---------------------------------------------------------------------
		public static DataGridViewExtSetting Default
		{
			get { return _defaultInstace; }
		}
		//---------------------------------------------------------------------
		// Because there can be more than one DGV in the user-application
		// a dictionary is used to save the settings for this DGV.
		// As key the name of the control is used.
		[UserScopedSetting]
		[SettingsSerializeAs(SettingsSerializeAs.Binary)]
		[DefaultSettingValue("")]
		public Dictionary<string, List<ColumnOrderItem>> ColumnOrder
		{
			get { return this["ColumnOrder"] as Dictionary<string, List<ColumnOrderItem>>; }
			set { this["ColumnOrder"] = value; }
		}

    }
	//-------------------------------------------------------------------------
	[Serializable]
	public sealed class ColumnOrderItem
	{
		public int DisplayIndex { get; set; }
		public int Width { get; set; }
		public bool Visible { get; set; }
		public int ColumnIndex { get; set; }
	}
}