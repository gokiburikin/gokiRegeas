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

namespace gokiRegeas
{
    public partial class frmPathSettings : Form
    {
        public frmPathSettings()
        {
            InitializeComponent();
            updateList();
            AllowDrop = true;
            DragDrop += dragDrop;
            DragEnter += dragEnter;
            lstPaths.DoubleClick += lstPaths_DoubleClick;
        }

        void lstPaths_DoubleClick(object sender, EventArgs e)
        {
            toggleSelectedPaths();
        }

        void dragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        void dragDrop(object sender, DragEventArgs e)
        {
            List<string> files = ((string[])e.Data.GetData(DataFormats.FileDrop)).ToList();
            for (int i = 0; i < files.Count; i++)
            {
                string file = files[i];
                try
                {
                    if (Directory.Exists(file))
                    {
                        IEnumerable<string> subfiles = Directory.EnumerateFiles(file, "*", SearchOption.AllDirectories);
                        foreach (string subfile in subfiles)
                        {
                            files.Add(subfile);
                        }
                        if (!GokiRegeas.paths.Contains(file))
                        {
                            GokiRegeas.paths.Add(file);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            updateList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            GokiRegeas.saveSettings();
        }

        public void refreshList()
        {
            lstPaths.SuspendLayout();
            foreach (string path in GokiRegeas.paths)
            {
                ListViewItem item = lstPaths.FindItemWithText(path);
                   if (  item != null )
                   {
                    if (GokiRegeas.sessionPaths.Contains(path))
                    {
                        item.ForeColor = Color.Black;
                    }
                    else
                    {
                        item.ForeColor = Color.DarkGray;
                    }
                }
            }
            lstPaths.ResumeLayout();
        }

        public void updateList()
        {
            lstPaths.SuspendLayout();
            lstPaths.Items.Clear();
            foreach ( string path in GokiRegeas.paths)
            {
                ListViewItem item = new ListViewItem(path);
                if (GokiRegeas.sessionPaths.Contains(path))
                {
                    item.ForeColor = Color.Black;
                }
                else
                {
                    item.ForeColor = Color.DarkGray;
                }
                lstPaths.Items.Add(item);
            }
            lstPaths.ResumeLayout();
        }

        private void btnAddPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if ( folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if ( !GokiRegeas.paths.Contains(folderBrowserDialog.SelectedPath))
                {
                    GokiRegeas.paths.Add(folderBrowserDialog.SelectedPath);
                }
            }
            updateList();
        }

        private void btnRemovePath_Click(object sender, EventArgs e)
        {
            foreach(int index in lstPaths.SelectedIndices)
            {
                string selectedPath = lstPaths.Items[index].Text;
                if ( GokiRegeas.paths.Contains(selectedPath))
                {
                    GokiRegeas.paths.Remove(selectedPath);
                }
            }
            updateList();
        }

        private void toggleSelectedPaths()
        {
            foreach (int index in lstPaths.SelectedIndices)
            {
                string path = lstPaths.Items[index].Text;
                if (GokiRegeas.sessionPaths.Contains(path))
                {
                    GokiRegeas.sessionPaths.Remove(path);
                }
                else
                {
                    GokiRegeas.sessionPaths.Add(path);
                }
                ///updateList();
                refreshList();
                GokiRegeas.fillFilePool();
            }
        }

        private void btnEnablePath_Click(object sender, EventArgs e)
        {
            toggleSelectedPaths();
        }
    }
}
