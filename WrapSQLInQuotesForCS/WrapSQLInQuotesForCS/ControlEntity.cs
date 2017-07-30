using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace IdealAutomate.Core {
    public class ControlEntity {
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
        public System.Windows.Media.Color? BackgroundColor { get; set; }
        public System.Windows.Media.Color? ForegroundColor { get; set; }
        public int ParentLkDDLNamesItemsInc { get; set; }
        public ImageSource Source { get; set; }
        public bool ComboBoxIsEditable { get; set; }
        /// <summary>
        /// DDLName is used when you want to use the same dropdownlist on the
        /// same screen more than once. In that case, you make the ID unique
        /// for each instance, but you use DDLName to point to the ID of the
        /// dropdown contained in DDLNames table
        /// </summary>
        public string DDLName { get; set; }

        public int ColumnSpan { get; set; }

        public string ToolTipx { get; set; }

        public System.Windows.Media.FontFamily FontFamilyx { get; set; }

        public double FontSize { get; set; }

        public FontStretch FontStretchx { get; set; }

        public System.Windows.FontStyle FontStyle { get; set; }

        public FontWeight FontWeight { get; set; }

        public ControlEntity() {
            ControlType = ControlType.Label;
            ID = "";
            Text = "";
            ListOfKeyValuePairs = new List<ComboBoxPair>();
            SelectedKey = "--Select Item ---";
            SelectedValue = "--Select Item ---";
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
            ComboBoxIsEditable = false;
            DDLName = "";
            ToolTipx = "";
            ColumnSpan = 1;
            FontFamilyx = new System.Windows.Media.FontFamily("Segoe UI");
            FontSize = 12;
            FontStretchx = FontStretches.Normal;
            FontStyle = System.Windows.FontStyles.Normal;
            FontWeight = FontWeights.Normal;
        }
        public void ControlEntitySetDefaults() {
            ControlType = ControlType.Label;
            ID = "";
            Text = "";
            ListOfKeyValuePairs = new List<ComboBoxPair>();
            SelectedKey = "--Select Item ---";
            SelectedValue = "--Select Item ---";
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
            ComboBoxIsEditable = false;
            DDLName = "";
            ToolTipx = "";
            ColumnSpan = 1;
            FontFamilyx = new System.Windows.Media.FontFamily("Segoe UI");
            FontSize = 12;
            FontStretchx = FontStretches.Normal;
            FontStyle = System.Windows.FontStyles.Normal;
            FontWeight = FontWeights.Normal;
        }
        public ControlEntity CreateControlEntity() {
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
            myControlEntity.ComboBoxIsEditable = ComboBoxIsEditable;
            myControlEntity.DDLName = DDLName;
            myControlEntity.ColumnSpan = ColumnSpan;
            myControlEntity.FontFamilyx = FontFamilyx;
            myControlEntity.FontSize = FontSize;
            myControlEntity.FontStretchx = FontStretchx;
            myControlEntity.FontWeight = FontWeight;
            myControlEntity.Source = Source;

            return myControlEntity;
        }
    }
}
