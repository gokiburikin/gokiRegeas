using Newtonsoft.Json;
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
        private int[] timerOpacities = new int[] { 0, 31, 63, 127, 191, 255 };
        private readonly float[] zoomLevels = new float[] { 0.10f, 0.25f, 0.33f, 0.50f, 0.67f, 0.75f, 1.00f, 1.10f, 1.25f, 1.33f, 1.50f, 1.67f, 1.75f, 2.00f };
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer performanceTimer;
        private int clickX;
        private int clickY;
        private bool rotationDragging;
        private bool moveDragging;
        private int dragThreshold;
        private bool dirty;
        private bool imageDirty;
        private int offsetX;
        private int offsetY;
        private bool controlKey;
        private bool shiftKey;
        private float zoom;
        private int zoomIndex;
        private DateTime viewManipulationTime;
        private Bitmap cleanImage;
        private bool isBicubic;
        private PerformanceCounter performanceCounter;
        public frmMain()
        {
            performanceCounter = new PerformanceCounter("Process","% Processor Time",GokiRegeas.process.ProcessName);

            Location = Properties.Settings.Default.WindowLocation;
            Size = Properties.Settings.Default.WindowSize;
            WindowState = Properties.Settings.Default.WindowState;

            controlKey = false;
            shiftKey = false;
            GokiRegeas.lastTime = DateTime.Now;
            GokiRegeas.pause();
            viewManipulationTime = DateTime.Now;
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
            imageDirty = false;
            isBicubic = false;

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
            GokiLibrary.GokiUtility.setInterval(GokiRegeas.settings.UpdateInterval);
            FormClosing += frmMain_FormClosing;
            timer = new Timer();
            timer.Tick += timer_Tick;
            timer.Interval = GokiRegeas.settings.UpdateInterval;
            timer.Start();

            performanceTimer = new Timer();
            performanceTimer.Tick += performanceTimer_Tick;
            performanceTimer.Interval = 250;
            performanceTimer.Start();

            btnToolStripContinuePause.Click += btnToolStripContinuePause_Click;
            pnlDraw.BackColor = GokiRegeas.settings.BackColor;
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
            foreach (int updateInterval in GokiRegeas.updateIntervals)
            {
                ToolStripMenuItem dropdownItem = new ToolStripMenuItem(updateInterval + "ms");
                dropdownItem.Tag = updateInterval;
                dropdownItem.Click += dropdownItem_Click;
                mnuViewUpdateInterval.DropDownItems.Add(dropdownItem);
            }
            lblFilename.Click += lblFilename_Click;
            btnToolStripZoom.Click += lblToolStripZoom_Click;
            updateToolStrip();
            updateStatusStrip();
            updateMenuStrip();
            this.TopMost = GokiRegeas.settings.AlwaysOnTop;
        }

        void performanceTimer_Tick(object sender, EventArgs e)
        {
            refreshCpuUsage();
        }

        void dropdownItem_Click(object sender, EventArgs e)
        {
            GokiRegeas.settings.UpdateInterval = (int)((ToolStripMenuItem)sender).Tag;
            GokiLibrary.GokiUtility.setInterval(GokiRegeas.settings.UpdateInterval);
            updateMenuStrip();
        }

        void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            GokiRegeas.saveSettings();
            Properties.Settings.Default.WindowLocation = Location;
            Properties.Settings.Default.WindowSize = Size;
            Properties.Settings.Default.WindowState = WindowState;
            Properties.Settings.Default.Save();
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
            makeDirty();
            viewManipulationTime = DateTime.Now;
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
            makeDirty();
            viewManipulationTime = DateTime.Now;
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
            lblMemory.Text = String.Format("{0:N0}KB", GokiRegeas.process.PrivateMemorySize64 / 1024f);
        }

        void refreshCpuUsage()
        {
            double cpuUsage = performanceCounter.NextValue() / Environment.ProcessorCount;
            lblCpu.Text = String.Format("{0:N2}%", cpuUsage);
        }

        void opacityButton_Click(object sender, EventArgs e)
        {
            GokiRegeas.settings.TimerOpacity = (int)((ToolStripButton)sender).Tag;
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
                            makeDirty();
                            viewManipulationTime = DateTime.Now;
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
                            makeDirty();
                            viewManipulationTime = DateTime.Now;
                        }
                        else
                        {
                            GokiRegeas.viewRotation = Math.Atan2(pnlDraw.Height / 2 + offsetY - e.Y, pnlDraw.Width / 2 + offsetX - e.X) * 180 / Math.PI - 90;
                            makeDirty();
                            viewManipulationTime = DateTime.Now;
                        }
                    }
                    else if (moveDragging)
                    {
                        offsetX += e.X - clickX;
                        offsetY += e.Y - clickY;
                        clickX = e.X;
                        clickY = e.Y;
                        makeDirty();
                        viewManipulationTime = DateTime.Now;
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
                makeDirty();
                viewManipulationTime = DateTime.Now;
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
                makeDirty();
                updateMenuStrip();
            }
            if (e.KeyCode == Keys.V)
            {
                GokiRegeas.verticalFlip = !GokiRegeas.verticalFlip;
                makeDirty();
                updateMenuStrip();
            }
            if (e.KeyCode == Keys.G)
            {
                GokiRegeas.settings.ConvertToGreyscale = !GokiRegeas.settings.ConvertToGreyscale;
                getBitmap();
                makeDirty();
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
                makeDirty();
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
            lblTimeStatus.Text = GokiRegeas.timeRemaining.ToString(@"mm\:ss\.ff");
            lblFlagBicubic.Enabled = isBicubic;
        }

        void lengthButton_Click(object sender, EventArgs e)
        {
            GokiRegeas.length = (int)((ToolStripButton)sender).Tag;
            GokiRegeas.lastUsedTime = GokiRegeas.length;
            GokiRegeas.runningTime = TimeSpan.FromMilliseconds(0);
            updateToolStrip();
        }

        void makeDirty ( bool image = true )
        {
            dirty = true;
            imageDirty = image;
            isBicubic = false;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
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
            updateStatusStrip();
            GokiRegeas.percentage = ((DateTime.Now - GokiRegeas.startTime).TotalMilliseconds - timePaused) / GokiRegeas.length;
            if (GokiRegeas.runningTime.TotalMilliseconds > GokiRegeas.length)
            {
                chooseRandomImage();
                onImageChange();
                GokiRegeas.startTime = DateTime.Now;
            }
            if (!isBicubic && (DateTime.Now - viewManipulationTime).TotalMilliseconds > GokiRegeas.settings.QualityAdjustDelay)
            {
                makeDirty();
            }

            if ( dirty || GokiRegeas.settings.ShowBigTimer)
            {
                pnlDraw.Invalidate();
                dirty = false;
            }
            timer.Interval = GokiRegeas.settings.UpdateInterval;
            timer.Start();
        }

        void pnlDraw_Resize(object sender, EventArgs e)
        {
            makeDirty();
        }

        void pnlDraw_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                if (GokiRegeas.currentFileBitmap != null)
                {
                    if (!isBicubic && (DateTime.Now - viewManipulationTime).TotalMilliseconds > GokiRegeas.settings.QualityAdjustDelay)
                    {
                        imageDirty = true;
                        isBicubic = true;
                    }
                    if (imageDirty || cleanImage == null)
                    {
                        e.Graphics.Clear(GokiRegeas.settings.BackColor);
                        if (cleanImage != null)
                        {
                            if (cleanImage.Width != pnlDraw.Width || cleanImage.Height != pnlDraw.Height)
                            {
                                cleanImage.Dispose();
                                cleanImage = new Bitmap(pnlDraw.Width, pnlDraw.Height);
                            }
                        }
                        else
                        {
                            cleanImage = new Bitmap(pnlDraw.Width, pnlDraw.Height);
                        }
                        using (Graphics gfx = Graphics.FromImage(cleanImage))
                        {
                            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                            gfx.Clip = new Region(pnlDraw.ClientRectangle);
                            gfx.Clear(GokiRegeas.settings.BackColor);
                            gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                            if (isBicubic)
                            {
                                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                            }
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
                            gfx.TranslateTransform(offsetX + pnlDraw.Width / 2, offsetY + pnlDraw.Height / 2);
                            gfx.RotateTransform((float)GokiRegeas.viewRotation);
                            gfx.ScaleTransform(scaleX, scaleY);
                            gfx.TranslateTransform(-width / 2, -height / 2);

                            gfx.DrawImage(GokiRegeas.currentFileBitmap, 0, 0, new Rectangle(0, 0, width, height), GraphicsUnit.Pixel);
                            gfx.ResetTransform();
                        }
                        imageDirty = false;
                    }
                    e.Graphics.DrawImageUnscaled(cleanImage, 0, 0);

                    if (GokiRegeas.settings.ShowBigTimer && !GokiRegeas.paused)
                    {
                        Color light = Color.FromArgb(GokiRegeas.settings.TimerOpacity, 255, 255, 255);
                        Color dark = Color.FromArgb(GokiRegeas.settings.TimerOpacity, 0, 0, 0);
                        using (SolidBrush brush = new SolidBrush(dark))
                        {
                            if (GokiRegeas.timeRemaining.Minutes < 1 || GokiRegeas.settings.AlwaysShowTimer)
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
                else
                {
                    Font font = new Font("Tahoma", 14);
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    using (SolidBrush brush = new SolidBrush(Color.White))
                    {
                        if ( GokiLibrary.GokiColor.average(GokiRegeas.settings.BackColor) > 128)
                        {
                            brush.Color = Color.Black;
                        }
                        if (GokiRegeas.filePool.Count == 0)
                        {
                            e.Graphics.DrawString("No files for the session.\nAdd paths in Settings > Paths.", font, brush, pnlDraw.ClientRectangle, format);
                        }
                        else if ( GokiRegeas.currentFileBitmap == null)
                        {
                            e.Graphics.DrawString("No current file.\nClick next or continue to begin.", font, brush, pnlDraw.ClientRectangle, format);
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
                    makeDirty();
                    updateStatusStrip();
                    refreshMemory();
                    refreshCpuUsage();
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

                if (GokiRegeas.settings.ConvertToGreyscale)
                {
                    GokiRegeas.currentFileBitmap = MakeGrayscale3(GokiRegeas.currentFileBitmap);
                }
                if (GokiRegeas.settings.ResetViewOnImageChange)
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
            makeDirty();
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
                makeDirty();
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
            lblStatus.Text = String.Format("{0:N0}/{1:N0}",GokiRegeas.filePool.IndexOf(GokiRegeas.currentFilePath),GokiRegeas.filePool.Count);
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
            makeDirty();
            updateMenuStrip();
        }

        private void mnuViewVerticalFlip_Click(object sender, EventArgs e)
        {
            GokiRegeas.verticalFlip = !GokiRegeas.verticalFlip;
            makeDirty();
            updateMenuStrip();
        }

        private void updateMenuStrip()
        {
            mnuViewHorizontalFlip.Checked = GokiRegeas.horizontalFlip;
            mnuViewVerticalFlip.Checked = GokiRegeas.verticalFlip;
            mnuViewBigTimer.Checked = GokiRegeas.settings.ShowBigTimer;
            mnuViewAlwaysShowTimer.Checked = GokiRegeas.settings.AlwaysShowTimer;
            mnuViewAlwaysOnTop.Checked = GokiRegeas.settings.AlwaysOnTop;
            mnuViewGreyscale.Checked = GokiRegeas.settings.ConvertToGreyscale;
            mnuViewResetView.Checked = GokiRegeas.settings.ResetViewOnImageChange;
            foreach (ToolStripMenuItem item in mnuViewUpdateInterval.DropDownItems)
            {
                if (GokiRegeas.settings.UpdateInterval == (int)item.Tag)
                {
                    item.Checked = true;
                }
                else
                {
                    item.Checked = false;
                }
            }
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

            colorSelectionForm.setColor(GokiRegeas.settings.BackColor);
            if (colorSelectionForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GokiRegeas.settings.BackColor = Color.FromArgb(colorSelectionForm.Red, colorSelectionForm.Green, colorSelectionForm.Blue);
                pnlDraw.BackColor = GokiRegeas.settings.BackColor;
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
            GokiRegeas.settings.ShowBigTimer = !GokiRegeas.settings.ShowBigTimer;
            updateMenuStrip();
            makeDirty();
        }

        private void mnuViewAlwaysShowTimer_Click(object sender, EventArgs e)
        {
            GokiRegeas.settings.AlwaysShowTimer = !GokiRegeas.settings.AlwaysShowTimer;
            updateMenuStrip();
            makeDirty();
        }

        private void mnuViewAlwaysOnTop_Click(object sender, EventArgs e)
        {
            GokiRegeas.settings.AlwaysOnTop = !GokiRegeas.settings.AlwaysOnTop;
            this.TopMost = GokiRegeas.settings.AlwaysOnTop;
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
            GokiRegeas.settings.ConvertToGreyscale = !GokiRegeas.settings.ConvertToGreyscale;
            getBitmap();
            updateMenuStrip();
        }

        private void mnuViewResetZoom_Click(object sender, EventArgs e)
        {
            GokiRegeas.settings.ResetViewOnImageChange = !GokiRegeas.settings.ResetViewOnImageChange;
            updateMenuStrip();
        }

        private void importFilepathsFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try 
            {
                List<string> newFilePool = JsonConvert.DeserializeObject<List<string>>(Clipboard.GetText());
                GokiRegeas.filePool.Clear();
                GokiRegeas.filePool.AddRange(newFilePool);
                chooseRandomImage();
                onImageChange();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Invalid JSON: Could not import file paths.");
            }
        }

        private void btnToolStripZoom_Click(object sender, EventArgs e)
        {
            float xAspect = (float)pnlDraw.Width / GokiRegeas.currentFileBitmap.Width;
            float yAspect = (float)pnlDraw.Height / GokiRegeas.currentFileBitmap.Height;
            zoom = Math.Min(yAspect, xAspect);
            makeDirty();
        }
    }
}
