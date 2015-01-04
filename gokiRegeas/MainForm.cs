﻿using System;
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
    public partial class frmMain : Form
    {
        internal static System.Windows.Forms.Timer timer;
        internal static int clickX;
        internal static int clickY;
        internal static bool dragging;
        public frmMain()
        {
            InitializeComponent();
            clickX = 0;
            clickY = 0;
            dragging = false;
            KeyDown += frmMain_KeyDown;
            pnlDraw.Paint += pnlDraw_Paint;
            pnlDraw.Resize += pnlDraw_Resize;
            pnlDraw.MouseDown += pnlDraw_MouseDown;
            pnlDraw.MouseUp += pnlDraw_MouseUp;
            pnlDraw.MouseMove += pnlDraw_MouseMove;
            GokiRegeas.loadSettings();
            GokiRegeas.fillFilePool();
            FormClosed += frmMain_FormClosed;
            timer = new Timer();
            timer.Tick += timer_Tick;
            timer.Interval = 35;
            timer.Start();
            btnToolStripContinuePause.Click += btnToolStripContinuePause_Click;
            pnlDraw.BackColor = GokiRegeas.backColor;
            foreach (int length in GokiRegeas.lengths)
            {
                ToolStripButton lengthButton = new ToolStripButton(String.Format("{0:N0} seconds", length / 1000));
                lengthButton.Tag = length;
                lengthButton.Click += lengthButton_Click;
                btnToolStripLengths.DropDownItems.Add(lengthButton);

            }
            updateToolStrip();
            updateStatusStrip();
            chooseRandomImage();
        }

        void pnlDraw_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        void pnlDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if ( dragging && GokiLibrary.GokiUtility.IntervalPassed )
            {
                GokiRegeas.viewRotation = Math.Atan2(pnlDraw.Height/2 - e.Y, pnlDraw.Width/2 - e.X) * 180 / Math.PI - 90;
                clickX = e.X;
                clickY = e.Y;
                pnlDraw.Invalidate();
            }
        }

        void pnlDraw_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                clickX = e.X;
                clickY = e.Y;
                dragging = true;
            }
            else if ( e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                GokiRegeas.viewRotation = 0;
                pnlDraw.Invalidate();
            }
        }

        void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.H)
            {
                GokiRegeas.horizontalFlip = !GokiRegeas.horizontalFlip;
                pnlDraw.Invalidate();
                updateMenuStrip();
            }
            if (e.KeyCode == Keys.V)
            {
                GokiRegeas.verticalFlip = !GokiRegeas.verticalFlip;
                pnlDraw.Invalidate();
                updateMenuStrip();
            }
            if (e.KeyCode == Keys.E)
            {
                historyNext();
            }
            if (e.KeyCode == Keys.Q)
            {
                historyPrevious();
            }
            if (e.KeyCode == Keys.Space)
            {
                GokiRegeas.togglePause();
            }
        }

        void btnToolStripContinuePause_Click(object sender, EventArgs e)
        {
            GokiRegeas.togglePause();
            updateToolStrip();
        }

        void updateToolStrip()
        {
            if (GokiRegeas.paused)
            {
                btnToolStripContinuePause.Text = "Continue";
            }
            else
            {
                btnToolStripContinuePause.Text = "Pause";
            }
            btnToolStripLengths.Text = String.Format("{0:N0} seconds", GokiRegeas.length / 1000);
        }

        void updateStatusStrip()
        {
            if (GokiRegeas.pathHistory.Count > 0)
            {
                lblHistory.Text = String.Format("{0:N0}/{1:N0}", GokiRegeas.pathHistoryIndex + 1, GokiRegeas.pathHistory.Count);
            }
            else
            {
                lblHistory.Text = "No history";
            }
        }

        void lengthButton_Click(object sender, EventArgs e)
        {
            GokiRegeas.length = (int)((ToolStripButton)sender).Tag;
            GokiRegeas.startTime = DateTime.Now;
            updateToolStrip();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            double timePaused = 0;
            if (GokiRegeas.paused)
            {
                timePaused = (DateTime.Now - GokiRegeas.pauseTime).TotalMilliseconds;
            }
            TimeSpan timeRemaining = (GokiRegeas.startTime.AddMilliseconds(GokiRegeas.length + timePaused) - DateTime.Now);
            lblTimeStatus.Text = timeRemaining.ToString(@"mm\:ss\.ff");
            if ((DateTime.Now - GokiRegeas.startTime).TotalMilliseconds - timePaused > GokiRegeas.length)
            {
                chooseRandomImage();
                GokiRegeas.startTime = DateTime.Now;
            }
        }

        void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            GokiRegeas.saveSettings();
        }

        void pnlDraw_Resize(object sender, EventArgs e)
        {
            pnlDraw.Invalidate();
        }

        void pnlDraw_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            if (GokiRegeas.currentFileBitmap != null)
            {
                float xAspect = (float)pnlDraw.Width / GokiRegeas.currentFileBitmap.Width;
                float yAspect = (float)pnlDraw.Height / GokiRegeas.currentFileBitmap.Height;
                xAspect = Math.Min(yAspect, xAspect);
                yAspect = xAspect;
                int width = (int)(GokiRegeas.currentFileBitmap.Width * xAspect);
                int height = (int)(GokiRegeas.currentFileBitmap.Height * yAspect);
                if (  GokiRegeas.resizedCurrentFileBitmap == null || GokiRegeas.resizedCurrentFileBitmap.Width != width || GokiRegeas.resizedCurrentFileBitmap.Height != height)
                {
                    GokiRegeas.resizedCurrentFileBitmap = new Bitmap(GokiRegeas.currentFileBitmap, (int)width, (int)height);
                }
                float xOffset = (pnlDraw.Width - width) / 2;
                float yOffset = (pnlDraw.Height - height) / 2;
                e.Graphics.TranslateTransform(xOffset + width / 2, yOffset + height / 2);
                if (GokiRegeas.horizontalFlip)
                {
                    e.Graphics.ScaleTransform(-1.0f, 1.0f);
                }
                if (GokiRegeas.verticalFlip)
                {
                    e.Graphics.ScaleTransform(1.0f, -1.0f);
                }
                e.Graphics.RotateTransform((float)GokiRegeas.viewRotation);
                e.Graphics.TranslateTransform(-width / 2, -height / 2);

                e.Graphics.DrawImage(GokiRegeas.resizedCurrentFileBitmap,0,0);
            }
        }

        private void pathsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPathSettings pathSettingsForm = new frmPathSettings();
            pathSettingsForm.ShowDialog();
        }

        private void chooseRandomImage()
        {
            if (GokiRegeas.filePool.Count > 0)
            {
                GokiRegeas.currentFilePath = GokiRegeas.filePool[GokiRegeas.random.Next(GokiRegeas.filePool.Count)];
                try
                {
                    GokiRegeas.currentFileBitmap = (Bitmap)Image.FromFile(GokiRegeas.currentFilePath);
                    GokiRegeas.pathHistory.Add(GokiRegeas.currentFilePath);
                    GokiRegeas.pathHistoryIndex = GokiRegeas.pathHistory.Count - 1;
                    pnlDraw.Invalidate();
                    updateStatusStrip();
                    //GokiRegeas.unpause();
                    lblStatus.Text = "";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                lblStatus.Text = "No valid files.";
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void historyNext()
        {
            DateTime now = DateTime.Now;
            GokiRegeas.pathHistoryIndex++;
            if (GokiRegeas.pathHistoryIndex > GokiRegeas.pathHistory.Count)
            {
                GokiRegeas.pathHistoryIndex = GokiRegeas.pathHistory.Count;
            }
            if (GokiRegeas.pathHistoryIndex <= GokiRegeas.pathHistory.Count - 1)
            {
                GokiRegeas.currentFilePath = GokiRegeas.pathHistory[GokiRegeas.pathHistoryIndex];
                GokiRegeas.currentFileBitmap = (Bitmap)Image.FromFile(GokiRegeas.currentFilePath);
                updateStatusStrip();
            }
            else
            {
                chooseRandomImage();
            }
            if ( GokiRegeas.paused)
            {
                GokiRegeas.pauseTime = now;
            }
            pnlDraw.Invalidate();
            GokiRegeas.startTime = now;
        }

        private void historyPrevious()
        {
            DateTime now = DateTime.Now;
            GokiRegeas.pathHistoryIndex--;
            if (GokiRegeas.pathHistoryIndex < 0)
            {
                GokiRegeas.pathHistoryIndex = 0;
            }
            if (GokiRegeas.pathHistoryIndex < GokiRegeas.pathHistory.Count)
            {

                GokiRegeas.currentFilePath = GokiRegeas.pathHistory[GokiRegeas.pathHistoryIndex];
                GokiRegeas.currentFileBitmap = (Bitmap)Image.FromFile(GokiRegeas.currentFilePath);
                pnlDraw.Invalidate();
                GokiRegeas.startTime = now;
                updateStatusStrip();
            }
            if (GokiRegeas.paused)
            {
                GokiRegeas.pauseTime = now;
            }
        }

        private void btnToolStripNext_Click(object sender, EventArgs e)
        {
            historyNext();
        }

        private void btnToolStripPrevious_Click(object sender, EventArgs e)
        {
            historyPrevious();
        }

        private void mnuViewHorizontalFlip_Click(object sender, EventArgs e)
        {
            GokiRegeas.horizontalFlip = !GokiRegeas.horizontalFlip;
            pnlDraw.Invalidate();
            updateMenuStrip();
        }

        private void mnuViewVerticalFlip_Click(object sender, EventArgs e)
        {
            GokiRegeas.verticalFlip = !GokiRegeas.verticalFlip;
            pnlDraw.Invalidate();
            updateMenuStrip();
        }

        private void updateMenuStrip()
        {
            mnuViewHorizontalFlip.Checked = GokiRegeas.horizontalFlip;
            mnuViewVerticalFlip.Checked = GokiRegeas.verticalFlip;
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            historyNext();
        }

        private void previousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            historyPrevious();
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmColorSelection colorSelectionForm = new frmColorSelection();
            colorSelectionForm.setColor(GokiRegeas.backColor);
            if (colorSelectionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GokiRegeas.backColor = Color.FromArgb(colorSelectionForm.Red, colorSelectionForm.Green, colorSelectionForm.Blue);
                pnlDraw.BackColor = GokiRegeas.backColor;
            }
        }

        private void btnToolStripLengths_ButtonClick(object sender, EventArgs e)
        {
            frmCustomLength customLengthForm = new frmCustomLength();
            customLengthForm.numSeconds.Value = (decimal)GokiRegeas.length/1000;
            if ( customLengthForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GokiRegeas.length = (int)customLengthForm.numSeconds.Value * 1000;
                updateToolStrip();
                GokiRegeas.startTime = DateTime.Now;
            }
        }
    }
}
