using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealAutomate.Core
{
    public class ControlEntity
    {
        public ControlType ControlType { get; set; }
        public string ID { get; set; }
        public string Text { get; set; }
        public List<ComboBoxPair> ListOfKeyValuePairs { get; set; }
        public string SelectedKey { get; set; }
        public string SelectedValue { get; set; }       
        public bool Checked { get; set; }
        public bool ButtonPressed { get; set; }
        public string ImageFile { get; set; }
        public int RowNumber { get; set; }
        public int ColumnNumber { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool TextWrap { get; set; }
        public bool Multiline { get; set; }
        public bool ShowTextBox { get; set; }
        public bool ShowFormattedAmount { get; set; }
        public decimal Amount { get; set; }
        public System.Windows.Media.Color? BackgroundColor  { get; set; }
        public System.Windows.Media.Color? ForegroundColor { get; set; }
        public int ParentLkDDLNamesItemsInc { get; set; }

        public string ToolTipx { get; set; }
        public ControlEntity()
        {
            ControlType = ControlType.Label;
            ID = "";
            Text = "";
            ListOfKeyValuePairs = new List<ComboBoxPair>();
            SelectedKey = "";
            SelectedValue = "";
            Checked = false;
            ButtonPressed = false;
            ImageFile = "";
            RowNumber = 0;
            ColumnNumber = 0;
            Width = 0;
            Height = 0;
            TextWrap = true;
            Multiline = false;
            ShowTextBox = true;
            ShowFormattedAmount = true;
      BackgroundColor = null;
      ForegroundColor = null;
            Amount = 0;
            ParentLkDDLNamesItemsInc = -1;
            ToolTipx = "";
        }
        public void ControlEntitySetDefaults() {
          ControlType = ControlType.Label;
          ID = "";
          Text = "";
          ListOfKeyValuePairs = new List<ComboBoxPair>();
          SelectedKey = "";
          SelectedValue = "";
          Checked = false;
          ButtonPressed = false;
          ImageFile = "";
          RowNumber = 0;
          ColumnNumber = 0;
          Width = 0;
          Height = 0;
          TextWrap = true;
          Multiline = false;
          ShowTextBox = true;
          ShowFormattedAmount = true;
      BackgroundColor = null;
      ForegroundColor = null;
      Amount = 0;
            ParentLkDDLNamesItemsInc = -1;
            ToolTipx = "";
        }
        public  ControlEntity CreateControlEntity() {
          ControlEntity myControlEntity = new ControlEntity();
          myControlEntity.ControlType = ControlType;
          myControlEntity.ID = ID;
          myControlEntity.Text = Text;
          myControlEntity.ListOfKeyValuePairs = ListOfKeyValuePairs;
          myControlEntity.SelectedKey = SelectedKey;
          myControlEntity.SelectedValue = SelectedValue;
          myControlEntity.Checked = Checked;
          myControlEntity.ButtonPressed = ButtonPressed;
          myControlEntity.ImageFile = ImageFile;
          myControlEntity.RowNumber = RowNumber;
          myControlEntity.ColumnNumber = ColumnNumber;
          myControlEntity.Width = Width;
          myControlEntity.Height = Height;
          myControlEntity.TextWrap = TextWrap;
          myControlEntity.Multiline = Multiline;
          myControlEntity.ShowTextBox = ShowTextBox;
          myControlEntity.ShowFormattedAmount = ShowFormattedAmount;
          myControlEntity.Amount = Amount;
          myControlEntity.BackgroundColor = BackgroundColor;
          myControlEntity.ForegroundColor = ForegroundColor;
          myControlEntity.ParentLkDDLNamesItemsInc = ParentLkDDLNamesItemsInc;
            myControlEntity.ToolTipx = ToolTipx;
      return myControlEntity;
        }
    }
}
