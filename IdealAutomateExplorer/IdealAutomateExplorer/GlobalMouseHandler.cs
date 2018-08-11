using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.Samples {
    public class GlobalMouseHandler : IMessageFilter {

        private const int WM_LBUTTONDOWN = 0x201;

        public bool PreFilterMessage(ref Message m) {
            if (m.Msg == WM_LBUTTONDOWN) {
                // do something
                var lastOpenedForm = new Form();
                
                if (Form.ActiveForm == null) {
                     lastOpenedForm = Application.OpenForms.Cast<Form>().Last();
                } else {
                    lastOpenedForm = Form.ActiveForm;
                }
                if (lastOpenedForm.Name == "ExplorerView") { 
                    if ((ExplorerView)Form.ActiveForm == null) {
                  //      ((ExplorerView)Application.OpenForms[Application.OpenForms.Count -1]).ExplorerView_Click(null, null);
                    } else {
                        ((ExplorerView)Form.ActiveForm).ExplorerView_Click(null, null);
                    }
            }
                if (lastOpenedForm.Name == "Search") {
                    if ((Search)Form.ActiveForm != null) {
                        ((Search)Form.ActiveForm).Search_Click(null, null);
                    }
                }
            }
            return false;
        }
    }
}
