using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gokiRegeas
{
    public partial class frmMain : Form
    {
        private System.Windows.Forms.Timer timer;
        private int clickX;
        private int clickY;
        private bool rotationDragging;
        private bool moveDragging;
        private int dragThreshold;
        private bool dirty;
        private int offsetX;
        private int offsetY;
        private bool controlKey;
        private bool shiftKey;
        private readonly float[] zoomLevels = new float[] { 0.10f, 0.25f, 0.33f, 0.50f, 0.67f, 0.75f, 1.00f, 1.10f, 1.25f, 1.33f, 1.50f, 1.67f, 1.75f, 2.00f };
        private float zoom;
        private int zoomIndex;
        private int[] timerOpacities = new int[] { 0, 31, 63, 127, 191, 255 };
        public frmMain()
        {
            int updateInterval = 16;

            GokiLibrary.GokiUtility.setInterval(updateInterval);
            controlKey = false;
            shiftKey = false;
            GokiRegeas.lastTime = DateTime.Now;
            GokiRegeas.pause();
            InitializeComponent();
            zoom = 1.0f;
            zoomIndex = 0;
            clickX = 0;
            clickY = 0;
            offsetX = 0;
            offsetY = 0;
            rotationDragging = false;
            moveDragging = false;
            dragThreshold = 6;
            dirty = false;
            KeyDown += frmMain_KeyDown;
            KeyUp += frmMain_KeyUp;
            pnlDraw.Paint += pnlDraw_Paint;
            pnlDraw.Resize += pnlDraw_Resize;
            pnlDraw.MouseDown += pnlDraw_MouseDown;
            pnlDraw.MouseUp += pnlDraw_MouseUp;
            pnlDraw.MouseMove += pnlDraw_MouseMove;
            pnlDraw.MouseWheel += pnlDraw_MouseWheel;
            GokiRegeas.loadSettings();
            GokiRegeas.fillFilePool();
            FormClosed += frmMain_FormClosed;
            timer = new Timer();
            timer.Tick += timer_Tick;
            timer.Interval = updateInterval;
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
            lblFilename.Click += lblFilename_Click;
            btnToolStripZoom.Click += lblToolStripZoom_Click;
            updateToolStrip();
            updateStatusStrip();
            updateMenuStrip();
            this.TopMost = GokiRegeas.alwaysOnTop;
        }

        void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            shiftKey = e.Shift;
            controlKey = e.Control;
        }

        void lblToolStripZoom_Click(object sender, EventArgs e)
        {
            offsetX = 0;
            offsetY = 0;
            updateToolStrip();
            updateClosestZoomIndex();
            dirty = true;
        }

        void pnlDraw_MouseWheel(object sender, MouseEventArgs e)
        {
            if ( e.Delta < 0)
            {
                zoomIndex--;
                if ( zoomIndex < 0 )
                {
                    zoomIndex = 0;
                }
            }
            else if (e.Delta > 0)
            {
                zoomIndex++;
                if (zoomIndex > zoomLevels.Length -1)
                {
                    zoomIndex = zoomLevels.Length - 1;
                }
            }
            zoom = zoomLevels[zoomIndex];
            updateToolStrip();
            dirty = true;
        }

        void lblFilename_Click(object sender, EventArgs e)
        {
            if ( File.Exists(GokiRegeas.currentFilePath) )
            {
                Process.Start("explorer.exe", @"/select," + GokiRegeas.currentFilePath);
            }
        }

        void refreshMemory()
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
            rotationDragging = false;
            moveDragging = false;
        }

        void pnlDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (GokiLibrary.GokiUtility.IntervalPassed)
            {
                if (Math.Sqrt(Math.Pow(e.X - clickX, 2) + Math.Pow(e.Y - clickY, 2)) > dragThreshold)
                {
                    if (rotationDragging)
                    {
                        if (shiftKey)
                        {
                            offsetX += e.X - clickX;
                            offsetY += e.Y - clickY;
                            clickX = e.X;
                            clickY = e.Y;
                            dirty = true;
                        }
                        else if (controlKey)
                        {
                            zoom = (float)Math.Sqrt(Math.Pow(pnlDraw.Height / 2 - e.Y, 2) + Math.Pow(pnlDraw.Width / 2 - e.X, 2)) / 100;
                            if ( zoom > 2 )
                            {
                                zoom = 2;
                            }
                            else if (zoom < .01f)
                            {
                                zoom = .01f;
                            }
                            updateToolStrip();
                            updateClosestZoomIndex();
                            dirty = true;
                        }
                        else
                        {
                            GokiRegeas.viewRotation = Math.Atan2(pnlDraw.Height / 2 + offsetY - e.Y, pnlDraw.Width / 2 + offsetX - e.X) * 180 / Math.PI - 90;
                            dirty = true;
                        }
                    }
                    else if (moveDragging)
                    {
                        offsetX += e.X - clickX;
                        offsetY += e.Y - clickY;
                        clickX = e.X;
                        clickY = e.Y;
                        dirty = true;
                    }
                }
            }
        }

        void pnlDraw_MouseDown(object sender, MouseEventArgs e)
        {
            clickX = e.X;
            clickY = e.Y;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                rotationDragging = true;
            }
            else if ( e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                GokiRegeas.viewRotation = 0;
                offsetX = 0;
                offsetY = 0;
                dirty = true;
            }
            else if ( e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                moveDragging = true;
            }
        }

        void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            shiftKey = e.Shift;
            controlKey = e.Control;

            if (e.KeyCode == Keys.H)
            {
                GokiRegeas.horizontalFlip = !GokiRegeas.horizontalFlip;
                dirty = true;
                updateMenuStrip();
            }
            if (e.KeyCode == Keys.V)
            {
                GokiRegeas.verticalFlip = !GokiRegeas.verticalFlip;
                dirty = true;
                updateMenuStrip();
            }
            if (e.KeyCode == Keys.G)
            {
                GokiRegeas.convertToGreyscale = !GokiRegeas.convertToGreyscale;
                getBitmap();
                dirty = true;
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
            btnToolStripZoom.Text = String.Format("{0:P2} Zoom", zoom);
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
            dirty = true;
        }

        void pnlDraw_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                if (GokiRegeas.currentFileBitmap != null)
                {
                    int width = (int)(GokiRegeas.currentFileBitmap.Width);
                    int height = (int)(GokiRegeas.currentFileBitmap.Height);
                    float scaleX = zoom;
                    float scaleY = zoom;
                    if (GokiRegeas.horizontalFlip)
                    {
                        scaleX *= -1;
                    }
                    if (GokiRegeas.verticalFlip)
                    {
                        scaleY *= -1;
                    }
                    e.Graphics.TranslateTransform(offsetX + pnlDraw.Width/2,offsetY + pnlDraw.Height/2);
                    e.Graphics.RotateTransform((float)GokiRegeas.viewRotation);
                    e.Graphics.ScaleTransform(scaleX, scaleY);
                    e.Graphics.TranslateTransform(-width / 2, -height / 2);

                    e.Graphics.DrawImage(GokiRegeas.currentFileBitmap, 0, 0, new Rectangle(0,0,width,height), GraphicsUnit.Pixel);
                    e.Graphics.ResetTransform();

                    if (GokiRegeas.showBigTimer && !GokiRegeas.paused)
                    {
                        Color light = Color.FromArgb(GokiRegeas.timerOpacity, 255, 255, 255);
                        Color dark = Color.FromArgb(GokiRegeas.timerOpacity, 0, 0, 0);
                        using (SolidBrush brush = new SolidBrush(dark))
                        {
                            if (GokiRegeas.timeRemaining.Minutes < 1 || GokiRegeas.alwaysShowTimer)
                            {
                                string text = Math.Floor(GokiRegeas.timeRemaining.TotalSeconds).ToString();

                                if (GokiRegeas.timeRemaining.TotalSeconds < 10)
                                {
                                    text = GokiRegeas.timeRemaining.ToString(@"ss\.ff");
                                }
                                Font font = new Font("Tahoma", 48);
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
            catch (Exception ex)
            {
                if (!File.Exists(GokiRegeas.currentFilePath))
                {
                    lblStatus.Text = "File does not exist";
                }
                else
                {
                    lblStatus.Text = "Failed to paint";
                }
                GokiRegeas.filePool.Remove(GokiRegeas.currentFilePath);
                chooseRandomImage();
                onImageChange();
            }
        }

        private void pathsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPathSettings pathSettingsForm = new frmPathSettings();
            pathSettingsForm.TopMost = this.TopMost;
            pathSettingsForm.ShowDialog();
        }

        private void chooseRandomImage()
        {
            if (GokiRegeas.filePool.Count > 0)
            {
                int random = GokiRegeas.random.Next(GokiRegeas.filePool.Count);
                GokiRegeas.currentFilePath = GokiRegeas.filePool[random];
                try
                {
                    getBitmap();
                    GokiRegeas.pathHistory.Add(GokiRegeas.currentFilePath);
                    GokiRegeas.pathHistoryIndex = GokiRegeas.pathHistory.Count - 1;
                    dirty = true;
                    updateStatusStrip();
                    refreshMemory();
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

        private void getBitmap()
        {
            if ( GokiRegeas.currentFilePath.Length > 0 && File.Exists(GokiRegeas.currentFilePath))
            {
                if (GokiRegeas.currentFileBitmap != null)
                {
                    GokiRegeas.currentFileBitmap.Dispose();
                }
                using (Bitmap bitmap = (Bitmap)Image.FromFile(GokiRegeas.currentFilePath))
                {
                    GokiRegeas.currentFileBitmap = makeBitmapCopy(bitmap);
                }

                if (GokiRegeas.convertToGreyscale)
                {
                    GokiRegeas.currentFileBitmap = MakeGrayscale3(GokiRegeas.currentFileBitmap);
                }
                if (GokiRegeas.resetViewOnImageChange)
                {
                    float xAspect = (float)pnlDraw.Width / GokiRegeas.currentFileBitmap.Width;
                    float yAspect = (float)pnlDraw.Height / GokiRegeas.currentFileBitmap.Height;
                    zoom = Math.Min(yAspect, xAspect);
                    offsetX = 0;
                    offsetY = 0;
                }
            }
        }

        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][] 
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        public static Bitmap makeBitmapCopy(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
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
                getBitmap();
                updateStatusStrip();
                refreshMemory();
            }
            else
            {
                chooseRandomImage();
            }
            if ( GokiRegeas.paused)
            {
                GokiRegeas.pauseTime = now;
            }
            dirty = true;
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
                getBitmap();
                dirty = true;
                GokiRegeas.startTime = now;
                updateStatusStrip();
                refreshMemory();
            }
            if (GokiRegeas.paused)
            {
                GokiRegeas.pauseTime = now;
            }
            onImageChange();
        }

        private void onImageChange()
        {
            lblFilename.Text = Path.GetFileName(GokiRegeas.currentFilePath);
            GokiRegeas.runningTime = TimeSpan.FromMilliseconds(0);
            updateToolStrip();
            updateClosestZoomIndex();
        }

        private void updateClosestZoomIndex()
        {
            int closestIndex = 0;
            float closestDistance = float.MaxValue;
            for (int i = 0; i < zoomLevels.Length; i++)
            {
                float distance = Math.Abs(zoom - zoomLevels[i]);
                if (distance < closestDistance)
                {
                    closestIndex = i;
                    closestDistance = distance;
                }
            }
            zoomIndex = closestIndex;
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
            dirty = true;
            updateMenuStrip();
        }

        private void mnuViewVerticalFlip_Click(object sender, EventArgs e)
        {
            GokiRegeas.verticalFlip = !GokiRegeas.verticalFlip;
            dirty = true;
            updateMenuStrip();
        }

        private void updateMenuStrip()
        {
            mnuViewHorizontalFlip.Checked = GokiRegeas.horizontalFlip;
            mnuViewVerticalFlip.Checked = GokiRegeas.verticalFlip;
            mnuViewBigTimer.Checked = GokiRegeas.showBigTimer;
            mnuViewAlwaysShowTimer.Checked = GokiRegeas.alwaysShowTimer;
            mnuViewAlwaysOnTop.Checked = GokiRegeas.alwaysOnTop;
            mnuViewGreyscale.Checked = GokiRegeas.convertToGreyscale;
            mnuViewResetView.Checked = GokiRegeas.resetViewOnImageChange;
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
            colorSelectionForm.TopMost = this.TopMost;

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
            customLengthForm.TopMost = this.TopMost;
            customLengthForm.numSeconds.Value = (decimal)GokiRegeas.length / 1000;
            if ( customLengthForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GokiRegeas.length = (int)customLengthForm.numSeconds.Value * 1000;
                GokiRegeas.lastUsedTime = GokiRegeas.length;
                GokiRegeas.runningTime = TimeSpan.FromMilliseconds(0);
                updateToolStrip();
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

        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = @"https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=gokiburikin%40gmail%2ecom&lc=CA&item_name=gokiRegeas&amount=2%2e00&currency_code=CAD&bn=PP%2dDonationsBF%3abtn_donate_SM%2egif%3aNonHosted";
            System.Diagnostics.Process.Start(url);
        }

        private void byGokiburikinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = @"https://github.com/gokiburikin";
            System.Diagnostics.Process.Start(url);
        }

        private void mnuViewGreyscale_Click(object sender, EventArgs e)
        {
            GokiRegeas.convertToGreyscale = !GokiRegeas.convertToGreyscale;
            getBitmap();
            updateMenuStrip();
        }

        private void mnuViewResetZoom_Click(object sender, EventArgs e)
        {
            GokiRegeas.resetViewOnImageChange = !GokiRegeas.resetViewOnImageChange;
            updateMenuStrip();
        }
    }
}
