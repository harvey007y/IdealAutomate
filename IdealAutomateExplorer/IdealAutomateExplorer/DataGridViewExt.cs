using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Windows.Forms.Samples {
    public partial class DataGridViewExt : DataGridView {
        public DataGridViewExt() {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e) {
            try {
                base.OnPaint(e);
            } catch (Exception) {
                this.Invalidate();
            }
        }
    }
}
