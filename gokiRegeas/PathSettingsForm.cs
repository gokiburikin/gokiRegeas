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
                        if (!GokiRegeas.settings.Paths.Contains(file))
                        {
                            GokiRegeas.settings.Paths.Add(file);
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
            GokiRegeas.fillFilePool();
            GokiRegeas.saveSettings();
        }

        public void updateList( bool clear = false)
        {
            lstPaths.SuspendLayout();
            if ( clear )
            {
                lstPaths.Items.Clear();
            }
            foreach ( string path in GokiRegeas.settings.Paths)
            {
                ListViewItem item = lstPaths.FindItemWithText(path);
                if ( item == null )
                {
                    item = new ListViewItem(path);
                    lstPaths.Items.Add(item);
                }
                if (GokiRegeas.settings.SessionPaths.Contains(path))
                {
                    item.ForeColor = Color.Black;
                }
                else
                {
                    item.ForeColor = Color.DarkGray;
                }
                lstPaths.SelectedIndices.Clear();
            }
            lstPaths.ResumeLayout();
        }

        private void btnAddPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if ( folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if ( !GokiRegeas.settings.Paths.Contains(folderBrowserDialog.SelectedPath))
                {
                    GokiRegeas.settings.Paths.Add(folderBrowserDialog.SelectedPath);
                }
            }
            updateList(true);
        }

        private void btnRemovePath_Click(object sender, EventArgs e)
        {
            foreach(int index in lstPaths.SelectedIndices)
            {
                string selectedPath = lstPaths.Items[index].Text;
                if ( GokiRegeas.settings.Paths.Contains(selectedPath))
                {
                    GokiRegeas.settings.Paths.Remove(selectedPath);
                }
            }
            updateList(true);
        }

        private void toggleSelectedPaths()
        {
            foreach (int index in lstPaths.SelectedIndices)
            {
                string path = lstPaths.Items[index].Text;
                if (GokiRegeas.settings.SessionPaths.Contains(path))
                {
                    GokiRegeas.settings.SessionPaths.Remove(path);
                }
                else
                {
                    GokiRegeas.settings.SessionPaths.Add(path);
                }
                updateList();
            }
        }

        private void btnEnablePath_Click(object sender, EventArgs e)
        {
            toggleSelectedPaths();
        }
    }
}
