using System;
using System.Windows.Forms;

namespace System.Windows.Forms.Samples {
    public class BaseForm : Form
	{
		public delegate void LoadCompletedEventHandler();
		public event LoadCompletedEventHandler LoadCompleted;

		public BaseForm()
		{
			this.Shown += new EventHandler(BaseForm_Shown);
		}

		void BaseForm_Shown(object sender, EventArgs e)
		{
			Application.DoEvents();
			if (LoadCompleted != null)
				LoadCompleted();
		}

	}
}
