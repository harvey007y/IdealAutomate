using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Windows.Forms.Samples {
    public partial class Peek : Form {
        public static List<TextLineInfo> textLineInfoList;
        string _strFullName;
        string _strLineNumber;
        string _strFindWhat;
        public Peek(string strFullName, string strLineNumber, string strFindWhat) {
            _strFullName = strFullName;
            _strLineNumber = strLineNumber;
            _strFindWhat = strFindWhat;
            InitializeComponent();
        }
        private void dgvPeek_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
            if (e.Value == null) return;

            StringFormat sf = StringFormat.GenericTypographic;
            sf.FormatFlags = sf.FormatFlags | StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.DisplayFormatControl;
            e.PaintBackground(e.CellBounds, true);

            SolidBrush br = new SolidBrush(Color.White);
            if (((int)e.State & (int)DataGridViewElementStates.Selected) == 0)
                br.Color = Color.Black;

            string text = e.Value.ToString();
            SizeF textSize = e.Graphics.MeasureString(text, Font, e.CellBounds.Width, sf);

            int keyPos = text.IndexOf(_strFindWhat, StringComparison.OrdinalIgnoreCase);
            if (keyPos >= 0) {
                SizeF textMetricSize = new SizeF(0, 0);
                if (keyPos >= 1) {
                    string textMetric = text.Substring(0, keyPos);
                    textMetricSize = e.Graphics.MeasureString(textMetric, Font, e.CellBounds.Width, sf);
                }

                SizeF keySize = e.Graphics.MeasureString(text.Substring(keyPos, _strFindWhat.Length), Font, e.CellBounds.Width, sf);
                float left = e.CellBounds.Left + (keyPos <= 0 ? 0 : textMetricSize.Width) + 2;
                RectangleF keyRect = new RectangleF(left, e.CellBounds.Top + 1, keySize.Width, e.CellBounds.Height - 2);

                var fillBrush = new SolidBrush(Color.Yellow);
                e.Graphics.FillRectangle(fillBrush, keyRect);
                fillBrush.Dispose();
            }
            e.Graphics.DrawString(text, Font, br, new PointF(e.CellBounds.Left + 2, e.CellBounds.Top + (e.CellBounds.Height - textSize.Height) / 2), sf);
            e.Handled = true;

            br.Dispose();
        }

        private void Peek_Load(object sender, EventArgs e) {
            dgvPeek.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvPeek_CellPainting);

            int intLineNumber = 0;
            int.TryParse(_strLineNumber, out intLineNumber);
            int intStartLineNumber = intLineNumber - 10;
            int intEndLineNumber = intLineNumber + 10;
            int intLineCtr = 0;
            ReadFileToString(_strFullName, intLineCtr, out textLineInfoList,  intStartLineNumber, intEndLineNumber);
            dgvPeek.DataSource = ConvertToDataTable<TextLineInfo>(textLineInfoList);

        }
        public  void ReadFileToString(string fullFilePath, int intLineCtr, out List<TextLineInfo> textLineInfoList, int intStartLineNumber, int intEndLineNumber) {
            textLineInfoList = new List<TextLineInfo>();
            while (true) {
               
                try {
                    using (FileStream fs = new FileStream(fullFilePath, FileMode.Open)) {
                        using (StreamReader sr = new StreamReader(fs, Encoding.Default)) {
                            string s;
                            string s_lower = "";
                            while ((s = sr.ReadLine()) != null) {
                                intLineCtr++;
                                if (intLineCtr > intStartLineNumber && intLineCtr < intEndLineNumber) {
                                    TextLineInfo mytextLineInfo = new TextLineInfo();
                                   
                                    mytextLineInfo.LineNumber = intLineCtr;                                 
                                    mytextLineInfo.LineText = s;
                                    textLineInfoList.Add(mytextLineInfo);
                                }
                            }
                            return;
                        }

                    }
                } catch (FileNotFoundException ex) {
                    Console.WriteLine("Output file {0} not yet ready ({1})", fullFilePath, ex.Message);
                    break;
                } catch (IOException ex) {
                    Console.WriteLine("Output file {0} not yet ready ({1})", fullFilePath, ex.Message);
                    break;
                } catch (UnauthorizedAccessException ex) {
                    Console.WriteLine("Output file {0} not yet ready ({1})", fullFilePath, ex.Message);
                    break;
                }
            }
        }
        public DataTable ConvertToDataTable<T>(IList<T> data) {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data) {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
    }
}
