using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Windows.Forms.Samples {
    public partial class Saved : Form {
        public Saved() {
            InitializeComponent();
        }
        Timer formCloser = new Timer();
        private void Saved_Load(object sender, EventArgs e) {
            formCloser.Interval = 500;
            formCloser.Enabled = true;
            formCloser.Tick += new EventHandler(formClose_Tick);
        }
        private void formClose_Tick(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
        }
    }
}
