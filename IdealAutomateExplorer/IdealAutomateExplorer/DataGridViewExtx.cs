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
    /// <summary>
    /// This user control allows you to avoid getting a large red x on your datagridview
    /// control when an exception occurs during painting. It fixes that problem by catching exceptions
    /// that occur onPaint.
    /// </summary>
    public partial class DataGridViewExty : DataGridView {
        public DataGridViewExty() {
            InitializeComponent();
            this.AllowUserToOrderColumns = true;
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
