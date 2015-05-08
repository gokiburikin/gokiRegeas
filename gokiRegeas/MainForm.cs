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
    public partial class frmMain : Form
    {
        internal static System.Windows.Forms.Timer timer;
        internal static System.Windows.Forms.Timer memoryTimer;
        internal static int clickX;
        internal static int clickY;
        internal static bool dragging;
        internal static int dragThreshold;
        internal static bool dirty;
        internal static int[] timerOpacities = new int[] { 0, 31, 63, 127, 191, 255 };
        public frmMain()
        {
            GokiRegeas.lastTime = DateTime.Now;
            GokiRegeas.pause();
            InitializeComponent();
            clickX = 0;
            clickY = 0;
            dragging = false;
            dragThreshold = 6;
            dirty = false;
            KeyDown += frmMain_KeyDown;
            pnlDraw.Paint += pnlDraw_Paint;
            pnlDraw.Resize += pnlDraw_Resize;
            pnlDraw.MouseDown += pnlDraw_MouseDown;
            pnlDraw.MouseUp += pnlDraw_MouseUp;
            pnlDraw.MouseMove += pnlDraw_MouseMove;
            GokiRegeas.loadSettings();
            GokiRegeas.fillFilePool();
            FormClosed += frmMain_FormClosed;
            memoryTimer = new Timer();
            memoryTimer.Tick += memoryTimer_Tick;
            memoryTimer.Interval = 1000;
            memoryTimer.Start();
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
            foreach (int opacity in timerOpacities)
            {
                ToolStripButton opacityButton = new ToolStripButton(String.Format("{0:N0}", opacity));
                opacityButton.Tag = opacity;
                opacityButton.Click += opacityButton_Click;
                btnToolStripTimerOpacity.DropDownItems.Add(opacityButton);
            }
            updateToolStrip();
            updateStatusStrip();
            updateMenuStrip();
            this.TopMost = GokiRegeas.alwaysOnTop;
        }

        void memoryTimer_Tick(object sender, EventArgs e)
        {
            GokiRegeas.process.Refresh();
            Text = String.Format("gokiRegeas - {0:N0}KB", GokiRegeas.process.PrivateMemorySize64 / 1024);
        }

        void opacityButton_Click(object sender, EventArgs e)
        {
            GokiRegeas.timerOpacity = (int)((ToolStripButton)sender).Tag;
            updateToolStrip();
        }

        void pnlDraw_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        void pnlDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if ( Math.Sqrt(Math.Pow(e.X - clickX,2) + Math.Pow(e.Y - clickY,2) )> dragThreshold)
            {
                if ( dragging )
                {
                    GokiRegeas.viewRotation = Math.Atan2(pnlDraw.Height/2 - e.Y, pnlDraw.Width/2 - e.X) * 180 / Math.PI - 90;
                    dirty = true;
                }
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
                dirty = true;
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
            if (e.KeyCode == Keys.R)
            {
                GokiRegeas.viewRotation = 0;
                dirty = true;
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
            GokiRegeas.lastUsedTime = GokiRegeas.length;
            GokiRegeas.runningTime = TimeSpan.FromMilliseconds(0);
            updateToolStrip();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            double timePaused = 0;
            if (GokiRegeas.paused)
            {
                timePaused = (DateTime.Now - GokiRegeas.pauseTime).TotalMilliseconds;
            }
            else
            {
                GokiRegeas.runningTime += DateTime.Now - GokiRegeas.lastTime;
            }
            GokiRegeas.lastTime = DateTime.Now;
            GokiRegeas.timeRemaining = TimeSpan.FromMilliseconds(GokiRegeas.length - GokiRegeas.runningTime.TotalMilliseconds);
            lblTimeStatus.Text = GokiRegeas.timeRemaining.ToString(@"mm\:ss\.ff");
            GokiRegeas.percentage = ((DateTime.Now - GokiRegeas.startTime).TotalMilliseconds - timePaused) / GokiRegeas.length;
            if (GokiRegeas.runningTime.TotalMilliseconds > GokiRegeas.length)
            {
                chooseRandomImage();
                onImageChange();
                GokiRegeas.startTime = DateTime.Now;
            }

            if ( dirty || GokiRegeas.showBigTimer)
            {
                pnlDraw.Invalidate();
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
                if ( GokiRegeas.resizedCurrentFileBitmap == null || GokiRegeas.resizedCurrentFileBitmap.Width != width || GokiRegeas.resizedCurrentFileBitmap.Height != height)
                {
                    if ( GokiRegeas.resizedCurrentFileBitmap != null )
                    {
                        GokiRegeas.resizedCurrentFileBitmap.Dispose();
                        GokiRegeas.resizedCurrentFileBitmap = null;
                    }
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
                e.Graphics.ResetTransform();
                if( GokiRegeas.showBigTimer && !GokiRegeas.paused )
                {
                    Color light = Color.FromArgb(GokiRegeas.timerOpacity-1, 255, 255, 255);
                    Color dark = Color.FromArgb(GokiRegeas.timerOpacity - 1, 0, 0, 0);
                    using (SolidBrush brush = new SolidBrush(dark))
                    {
                        if ( GokiRegeas.timeRemaining.Minutes < 1 || GokiRegeas.alwaysShowTimer )
                        {
                            string text = Math.Ceiling(GokiRegeas.timeRemaining.TotalSeconds).ToString();
//                            string text = GokiRegeas.timeRemaining.ToString(@"ss");

                            if (GokiRegeas.timeRemaining.TotalSeconds < 10)
                            {
                                text = GokiRegeas.timeRemaining.ToString(@"ss\.ff");
                            }
                            Font font = new Font("Arial", 48);
                            StringFormat format = new StringFormat();
                            format.Alignment = StringAlignment.Center;
                            format.LineAlignment = StringAlignment.Near;
                            e.Graphics.DrawString(text, font, brush, pnlDraw.ClientRectangle, format);
                            brush.Color = light;
                            format.LineAlignment = StringAlignment.Far;
                            e.Graphics.DrawString(text, font, brush, pnlDraw.ClientRectangle, format);
                        }
                    }
                    
                }
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
                    if (GokiRegeas.currentFileBitmap != null)
                    {
                        GokiRegeas.currentFileBitmap.Dispose();
                    }
                    GokiRegeas.currentFileBitmap = (Bitmap)Image.FromFile(GokiRegeas.currentFilePath);
                    GokiRegeas.resizedCurrentFileBitmap = null;
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
                if (GokiRegeas.currentFileBitmap != null)
                {
                    GokiRegeas.currentFileBitmap.Dispose();
                    GokiRegeas.resizedCurrentFileBitmap.Dispose();
                    GokiRegeas.resizedCurrentFileBitmap = null;
                }
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
            onImageChange();
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
                if (GokiRegeas.currentFileBitmap != null)
                {
                    GokiRegeas.currentFileBitmap.Dispose();
                    GokiRegeas.resizedCurrentFileBitmap.Dispose();
                    GokiRegeas.resizedCurrentFileBitmap = null;
                }
                GokiRegeas.currentFileBitmap = (Bitmap)Image.FromFile(GokiRegeas.currentFilePath);
                pnlDraw.Invalidate();
                GokiRegeas.startTime = now;
                updateStatusStrip();
            }
            if (GokiRegeas.paused)
            {
                GokiRegeas.pauseTime = now;
            }
            onImageChange();
        }

        private void onImageChange()
        {
            lblSpring1.Text = Path.GetFileName(GokiRegeas.currentFilePath);
            GokiRegeas.runningTime = TimeSpan.FromMilliseconds(0);
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
            mnuViewBigTimer.Checked = GokiRegeas.showBigTimer;
            mnuViewAlwaysShowTimer.Checked = GokiRegeas.alwaysShowTimer;
            mnuViewAlwaysOnTop.Checked = GokiRegeas.alwaysOnTop;
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
                GokiRegeas.lastUsedTime = GokiRegeas.length;
                updateToolStrip();
                GokiRegeas.startTime = DateTime.Now;
            }
        }

        private void btnToolStripRestart_Click(object sender, EventArgs e)
        {
            GokiRegeas.runningTime = TimeSpan.FromMilliseconds(0);
        }

        private void btnViewTimerBar_Click(object sender, EventArgs e)
        {
            GokiRegeas.showBigTimer = !GokiRegeas.showBigTimer;
            updateMenuStrip();
            dirty = true;
        }

        private void mnuViewAlwaysShowTimer_Click(object sender, EventArgs e)
        {
            GokiRegeas.alwaysShowTimer = !GokiRegeas.alwaysShowTimer;
            updateMenuStrip();
            dirty = true;
        }

        private void mnuViewAlwaysOnTop_Click(object sender, EventArgs e)
        {
            GokiRegeas.alwaysOnTop = !GokiRegeas.alwaysOnTop;
            this.TopMost = GokiRegeas.alwaysOnTop;
            updateMenuStrip();
        }
    }
}
