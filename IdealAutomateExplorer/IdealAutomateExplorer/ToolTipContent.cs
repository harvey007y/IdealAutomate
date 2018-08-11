using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Forms.Samples;
using System.Windows.Media;
//using System.Windows;

namespace Demo {
    // a very simple Control to be used as the content of the InteractiveToolTip
    public partial class ToolTipContent : UserControl {
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.LinkLabel linkLabel1;
        public int MyProperty { get; set; }
        private string _MyContent = "";
        private string _MyLink = "";
        public string MyContent {
            get { return _MyContent; }
            set { _MyContent = value;
                RefreshToolTip();
            }
        }
        public string MyLink {
            get { return _MyLink; }
            set { _MyLink = value;
                RefreshToolTip();
            }
        }

        private void RefreshToolTip() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolTipContent));
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
           if (this.Controls.Count > 0) {
                this.Controls.RemoveAt(0);                
            }
            if (this.Controls.Count > 0) {
                this.Controls.RemoveAt(0);
            }
            this.linkLabel1.LinkClicked += delegate (object sender, LinkLabelLinkClickedEventArgs e) {
                string strDefaultUrl = MyLink;
                var lastOpenedForm = new Form();

                if (Form.ActiveForm == null) {
                    lastOpenedForm = Application.OpenForms.Cast<Form>().Last();
                } else {
                    lastOpenedForm = Form.ActiveForm;
                }
                if (lastOpenedForm.Name == "ExplorerView") {
                    if ((ExplorerView)Form.ActiveForm == null) {
                        ((ExplorerView)Application.OpenForms[Application.OpenForms.Count - 1]).ExplorerView_Click(null, null);
                    } else {
                        ((ExplorerView)Form.ActiveForm).ExplorerView_Click(null, null);
                    }
                }
                if (lastOpenedForm.Name == "Search") {
                    if ((Search)Form.ActiveForm != null) {
                        ((Search)Form.ActiveForm).Search_Click(null, null);
                    }
                }
                Process.Start("IExplore.exe", strDefaultUrl);
            };
            this.SuspendLayout();
            // 
            // label1
            // 
            string contentWithBars = _MyContent.Replace("\r\n", "|");
            string[] contentArray = contentWithBars.Split('|');
            int labelHeight = contentArray.Length * 13;
            int maxChars = 0;
            foreach (var item in contentArray) {
                if (item.Length > maxChars) {
                    maxChars = item.Length;
                }
            }
            int labelWidth = maxChars * 5;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(labelWidth, labelHeight);
            this.label1.TabIndex = 0;
            this.label1.Text = _MyContent; // resources.GetString("label1.Text");
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(3, labelHeight + 13);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(47, 13);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Click me";
            // 
            // ToolTipContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Name = "ToolTipContent";
            this.Size = new System.Drawing.Size(labelWidth, labelHeight + 40);
            this.ResumeLayout(false);
            this.PerformLayout();
            label1.Text = _MyContent;
            linkLabel1.Text = _MyLink;
        }

        public ToolTipContent() {
            InitializeComponent();
            RefreshToolTip();
        }
    }
}
