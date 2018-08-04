#region License

//
// InteractiveToolTip.Designer.cs
//
// Copyright (C) 2012-2013 Alex Taylor.  All Rights Reserved.
//
// InteractiveToolTip is published under the terms of the Code Project Open License.
// http://www.codeproject.com/info/cpol10.aspx
//

#endregion License

namespace Digitalis.GUI.Controls
{
    partial class InteractiveToolTip
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != components)
                    components.Dispose();

                Hide();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion
    }
}
