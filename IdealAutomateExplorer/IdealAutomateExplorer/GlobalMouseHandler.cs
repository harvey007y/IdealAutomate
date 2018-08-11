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
                ((ExplorerView)Form.ActiveForm).ExplorerView_Click(null, null);
            }
            return false;
        }
    }
}
