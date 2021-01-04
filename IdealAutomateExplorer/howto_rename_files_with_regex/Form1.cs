using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Text.RegularExpressions;

namespace howto_rename_files_with_regex
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Start in the test directory.
        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo dir_info = new DirectoryInfo(
                Path.Combine(Application.StartupPath, @"..\..\Test"));
            txtDirectory.Text = dir_info.FullName;
        }

        // Make a list of file names to change from and to.
        private List<string> FullFromNames, FromNames, ToNames;
        private void MakeFileLists()
        {
            try
            {
                // Make the file name lists.
                FullFromNames = new List<string>();
                FromNames = new List<string>();
                ToNames = new List<string>();

                // Get the files that match the pattern.
                DirectoryInfo dir_info = new DirectoryInfo(txtDirectory.Text);
                FileInfo[] files = dir_info.GetFiles(txtFilePattern.Text);
                Regex regex = new Regex(txtOldPattern.Text);
                for (int i = 0; i < files.Length; i++)
                {
                    string new_name = regex.Replace(files[i].Name,
                        txtNewPattern.Text);
                    new_name = new_name.Replace("$i", i.ToString());

                    if (files[i].Name != new_name)
                    {
                        FullFromNames.Add(files[i].FullName);
                        FromNames.Add(files[i].Name);
                        ToNames.Add(new_name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error building file list.\n" + ex.Message);
                FullFromNames = new List<string>();
                FromNames = new List<string>();
                ToNames = new List<string>();
            }
        }

        // Display a list of changes that we will make.
        private void btnPreviewChanges_Click(object sender, EventArgs e)
        {
            // Make the file lists.
            MakeFileLists();

            // Display the lists.
            lvwResults.Items.Clear();
            for (int i = 0; i < FromNames.Count; i++)
            {
                ListViewItem new_item =
                    lvwResults.Items.Add(FromNames[i]);
                new_item.SubItems.Add(ToNames[i]);
            }
            lvwResults.Columns[0].Width = -2;
            lvwResults.Columns[1].Width = -2;

            // Enable the Make Changes button.
            btnMakeChanges.Enabled = true;
        }

        // Make the changes.
        private void btnMakeChanges_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < FromNames.Count; i++)
                {
                    try
                    {
                        FileInfo file_info = new FileInfo(FullFromNames[i]);
                        string new_name = file_info.DirectoryName + "\\" + ToNames[i];

                        file_info.MoveTo(new_name);
                        //Console.WriteLine(i.ToString() + ": " + file_info.FullName + " --> " + new_name);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error moving file '" +
                            FromNames[i] + "' to '" +
                            ToNames[i] + "'.\n" + ex.Message);
                        throw;
                    }
                }
                lvwResults.Items.Clear();
                btnMakeChanges.Enabled = false;
                FullFromNames = new List<string>();
                FromNames = new List<string>();
                ToNames = new List<string>();

                MessageBox.Show("Done");
            }
            catch
            {
            }
        }

        // Remove the selected files from the ListView.
        private void btnRemoveFile_Click(object sender, EventArgs e)
        {
            for (int i = lvwResults.Items.Count - 1; i >= 0; i--)
            {
                if (lvwResults.Items[i].Selected)
                {
                    lvwResults.Items.RemoveAt(i);
                    FromNames.RemoveAt(i);
                    ToNames.RemoveAt(i);
                }
            }
        }
    }
}
