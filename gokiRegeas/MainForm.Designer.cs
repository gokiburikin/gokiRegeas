namespace gokiRegeas
{
    partial class frmMain
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFilepathsFromClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewHorizontalFlip = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewVerticalFlip = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewGreyscale = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewBigTimer = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewAlwaysShowTimer = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewAlwaysOnTop = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewResetView = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byGokiburikinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.donateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblHistory = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblFilename = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTimeStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnToolStripReset = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnToolStripContinuePause = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnToolStripPrevious = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnToolStripNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnToolStripLengths = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnToolStripTimerOpacity = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.btnToolStripZoom = new System.Windows.Forms.ToolStripButton();
            this.mnuViewUpdateInterval = new System.Windows.Forms.ToolStripMenuItem();
            this.lblFlagBicubic = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMemory = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCpu = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlDraw = new gokiRegeas.DoubleBufferedPanel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(792, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nextToolStripMenuItem,
            this.previousToolStripMenuItem,
            this.importFilepathsFromClipboardToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // nextToolStripMenuItem
            // 
            this.nextToolStripMenuItem.Name = "nextToolStripMenuItem";
            this.nextToolStripMenuItem.ShortcutKeyDisplayString = "E";
            this.nextToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.nextToolStripMenuItem.Text = "Next";
            this.nextToolStripMenuItem.Click += new System.EventHandler(this.nextToolStripMenuItem_Click);
            // 
            // previousToolStripMenuItem
            // 
            this.previousToolStripMenuItem.Name = "previousToolStripMenuItem";
            this.previousToolStripMenuItem.ShortcutKeyDisplayString = "Q";
            this.previousToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.previousToolStripMenuItem.Text = "Previous";
            this.previousToolStripMenuItem.Click += new System.EventHandler(this.previousToolStripMenuItem_Click);
            // 
            // importFilepathsFromClipboardToolStripMenuItem
            // 
            this.importFilepathsFromClipboardToolStripMenuItem.Name = "importFilepathsFromClipboardToolStripMenuItem";
            this.importFilepathsFromClipboardToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.importFilepathsFromClipboardToolStripMenuItem.Text = "Import Filepaths from Clipboard";
            this.importFilepathsFromClipboardToolStripMenuItem.Click += new System.EventHandler(this.importFilepathsFromClipboardToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(222, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pathsToolStripMenuItem,
            this.backgroundColorToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // pathsToolStripMenuItem
            // 
            this.pathsToolStripMenuItem.Name = "pathsToolStripMenuItem";
            this.pathsToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.pathsToolStripMenuItem.Text = "Paths...";
            this.pathsToolStripMenuItem.Click += new System.EventHandler(this.pathsToolStripMenuItem_Click);
            // 
            // backgroundColorToolStripMenuItem
            // 
            this.backgroundColorToolStripMenuItem.Name = "backgroundColorToolStripMenuItem";
            this.backgroundColorToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.backgroundColorToolStripMenuItem.Text = "Background Color...";
            this.backgroundColorToolStripMenuItem.Click += new System.EventHandler(this.backgroundColorToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewHorizontalFlip,
            this.mnuViewVerticalFlip,
            this.mnuViewGreyscale,
            this.mnuViewBigTimer,
            this.mnuViewAlwaysShowTimer,
            this.mnuViewAlwaysOnTop,
            this.mnuViewResetView,
            this.mnuViewUpdateInterval});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // mnuViewHorizontalFlip
            // 
            this.mnuViewHorizontalFlip.Name = "mnuViewHorizontalFlip";
            this.mnuViewHorizontalFlip.ShortcutKeyDisplayString = "H";
            this.mnuViewHorizontalFlip.Size = new System.Drawing.Size(215, 22);
            this.mnuViewHorizontalFlip.Text = "Horizontal Flip";
            this.mnuViewHorizontalFlip.Click += new System.EventHandler(this.mnuViewHorizontalFlip_Click);
            // 
            // mnuViewVerticalFlip
            // 
            this.mnuViewVerticalFlip.Name = "mnuViewVerticalFlip";
            this.mnuViewVerticalFlip.ShortcutKeyDisplayString = "V";
            this.mnuViewVerticalFlip.Size = new System.Drawing.Size(215, 22);
            this.mnuViewVerticalFlip.Text = "Vertical Flip";
            this.mnuViewVerticalFlip.Click += new System.EventHandler(this.mnuViewVerticalFlip_Click);
            // 
            // mnuViewGreyscale
            // 
            this.mnuViewGreyscale.Name = "mnuViewGreyscale";
            this.mnuViewGreyscale.ShortcutKeyDisplayString = "G";
            this.mnuViewGreyscale.Size = new System.Drawing.Size(215, 22);
            this.mnuViewGreyscale.Text = "Greyscale";
            this.mnuViewGreyscale.Click += new System.EventHandler(this.mnuViewGreyscale_Click);
            // 
            // mnuViewBigTimer
            // 
            this.mnuViewBigTimer.Name = "mnuViewBigTimer";
            this.mnuViewBigTimer.Size = new System.Drawing.Size(215, 22);
            this.mnuViewBigTimer.Text = "Big Timer";
            this.mnuViewBigTimer.Click += new System.EventHandler(this.btnViewTimerBar_Click);
            // 
            // mnuViewAlwaysShowTimer
            // 
            this.mnuViewAlwaysShowTimer.Name = "mnuViewAlwaysShowTimer";
            this.mnuViewAlwaysShowTimer.Size = new System.Drawing.Size(215, 22);
            this.mnuViewAlwaysShowTimer.Text = "Always Show Timer";
            this.mnuViewAlwaysShowTimer.Click += new System.EventHandler(this.mnuViewAlwaysShowTimer_Click);
            // 
            // mnuViewAlwaysOnTop
            // 
            this.mnuViewAlwaysOnTop.Name = "mnuViewAlwaysOnTop";
            this.mnuViewAlwaysOnTop.Size = new System.Drawing.Size(215, 22);
            this.mnuViewAlwaysOnTop.Text = "Always On Top";
            this.mnuViewAlwaysOnTop.Click += new System.EventHandler(this.mnuViewAlwaysOnTop_Click);
            // 
            // mnuViewResetView
            // 
            this.mnuViewResetView.Name = "mnuViewResetView";
            this.mnuViewResetView.Size = new System.Drawing.Size(215, 22);
            this.mnuViewResetView.Text = "Reset View on Image Change";
            this.mnuViewResetView.Click += new System.EventHandler(this.mnuViewResetZoom_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.byGokiburikinToolStripMenuItem,
            this.donateToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // byGokiburikinToolStripMenuItem
            // 
            this.byGokiburikinToolStripMenuItem.Name = "byGokiburikinToolStripMenuItem";
            this.byGokiburikinToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.byGokiburikinToolStripMenuItem.Text = "by gokiburikin";
            this.byGokiburikinToolStripMenuItem.Click += new System.EventHandler(this.byGokiburikinToolStripMenuItem_Click);
            // 
            // donateToolStripMenuItem
            // 
            this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
            this.donateToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.donateToolStripMenuItem.Text = "Donate";
            this.donateToolStripMenuItem.Click += new System.EventHandler(this.donateToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblHistory,
            this.lblFilename,
            this.lblMemory,
            this.lblCpu,
            this.lblFlagBicubic,
            this.lblTimeStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 551);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(792, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = false;
            this.lblStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Right | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(80, 17);
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblHistory
            // 
            this.lblHistory.AutoSize = false;
            this.lblHistory.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Right | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblHistory.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblHistory.Name = "lblHistory";
            this.lblHistory.Size = new System.Drawing.Size(80, 17);
            this.lblHistory.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblFilename
            // 
            this.lblFilename.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Right | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblFilename.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblFilename.IsLink = true;
            this.lblFilename.Name = "lblFilename";
            this.lblFilename.Size = new System.Drawing.Size(400, 17);
            this.lblFilename.Spring = true;
            this.lblFilename.Text = "Filename";
            this.lblFilename.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblTimeStatus
            // 
            this.lblTimeStatus.AutoSize = false;
            this.lblTimeStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Right | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblTimeStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblTimeStatus.Name = "lblTimeStatus";
            this.lblTimeStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblTimeStatus.Size = new System.Drawing.Size(60, 17);
            this.lblTimeStatus.Text = "00:00:00";
            this.lblTimeStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnToolStripReset,
            this.toolStripSeparator5,
            this.btnToolStripContinuePause,
            this.toolStripSeparator1,
            this.btnToolStripPrevious,
            this.toolStripSeparator2,
            this.btnToolStripNext,
            this.toolStripSeparator3,
            this.btnToolStripLengths,
            this.toolStripSeparator6,
            this.btnToolStripTimerOpacity,
            this.toolStripSeparator7,
            this.btnToolStripZoom});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(792, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnToolStripReset
            // 
            this.btnToolStripReset.AutoSize = false;
            this.btnToolStripReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnToolStripReset.Image = ((System.Drawing.Image)(resources.GetObject("btnToolStripReset.Image")));
            this.btnToolStripReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToolStripReset.Name = "btnToolStripReset";
            this.btnToolStripReset.Size = new System.Drawing.Size(60, 22);
            this.btnToolStripReset.Text = "Reset";
            this.btnToolStripReset.Click += new System.EventHandler(this.btnToolStripRestart_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // btnToolStripContinuePause
            // 
            this.btnToolStripContinuePause.AutoSize = false;
            this.btnToolStripContinuePause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnToolStripContinuePause.Image = ((System.Drawing.Image)(resources.GetObject("btnToolStripContinuePause.Image")));
            this.btnToolStripContinuePause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToolStripContinuePause.Name = "btnToolStripContinuePause";
            this.btnToolStripContinuePause.Size = new System.Drawing.Size(60, 22);
            this.btnToolStripContinuePause.Text = "Continue";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnToolStripPrevious
            // 
            this.btnToolStripPrevious.AutoSize = false;
            this.btnToolStripPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnToolStripPrevious.Image = ((System.Drawing.Image)(resources.GetObject("btnToolStripPrevious.Image")));
            this.btnToolStripPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToolStripPrevious.Name = "btnToolStripPrevious";
            this.btnToolStripPrevious.Size = new System.Drawing.Size(60, 22);
            this.btnToolStripPrevious.Text = "Previous";
            this.btnToolStripPrevious.Click += new System.EventHandler(this.btnToolStripPrevious_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnToolStripNext
            // 
            this.btnToolStripNext.AutoSize = false;
            this.btnToolStripNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnToolStripNext.Image = ((System.Drawing.Image)(resources.GetObject("btnToolStripNext.Image")));
            this.btnToolStripNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToolStripNext.Name = "btnToolStripNext";
            this.btnToolStripNext.Size = new System.Drawing.Size(60, 22);
            this.btnToolStripNext.Text = "Next";
            this.btnToolStripNext.Click += new System.EventHandler(this.btnToolStripNext_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnToolStripLengths
            // 
            this.btnToolStripLengths.AutoSize = false;
            this.btnToolStripLengths.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnToolStripLengths.Image = ((System.Drawing.Image)(resources.GetObject("btnToolStripLengths.Image")));
            this.btnToolStripLengths.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToolStripLengths.Name = "btnToolStripLengths";
            this.btnToolStripLengths.Size = new System.Drawing.Size(100, 22);
            this.btnToolStripLengths.Text = "Length";
            this.btnToolStripLengths.ButtonClick += new System.EventHandler(this.btnToolStripLengths_ButtonClick);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // btnToolStripTimerOpacity
            // 
            this.btnToolStripTimerOpacity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnToolStripTimerOpacity.Image = ((System.Drawing.Image)(resources.GetObject("btnToolStripTimerOpacity.Image")));
            this.btnToolStripTimerOpacity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToolStripTimerOpacity.Name = "btnToolStripTimerOpacity";
            this.btnToolStripTimerOpacity.Size = new System.Drawing.Size(86, 22);
            this.btnToolStripTimerOpacity.Text = "Timer Opacity";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // btnToolStripZoom
            // 
            this.btnToolStripZoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnToolStripZoom.Image = ((System.Drawing.Image)(resources.GetObject("btnToolStripZoom.Image")));
            this.btnToolStripZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToolStripZoom.Name = "btnToolStripZoom";
            this.btnToolStripZoom.Size = new System.Drawing.Size(37, 22);
            this.btnToolStripZoom.Text = "Zoom";
            this.btnToolStripZoom.Click += new System.EventHandler(this.btnToolStripZoom_Click);
            // 
            // mnuViewUpdateInterval
            // 
            this.mnuViewUpdateInterval.Name = "mnuViewUpdateInterval";
            this.mnuViewUpdateInterval.Size = new System.Drawing.Size(215, 22);
            this.mnuViewUpdateInterval.Text = "Update Interval";
            // 
            // lblFlagBicubic
            // 
            this.lblFlagBicubic.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Right | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblFlagBicubic.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblFlagBicubic.Enabled = false;
            this.lblFlagBicubic.Name = "lblFlagBicubic";
            this.lblFlagBicubic.Size = new System.Drawing.Size(17, 17);
            this.lblFlagBicubic.Text = "B";
            this.lblFlagBicubic.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // lblMemory
            // 
            this.lblMemory.AutoSize = false;
            this.lblMemory.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Right | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblMemory.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblMemory.Name = "lblMemory";
            this.lblMemory.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMemory.Size = new System.Drawing.Size(80, 17);
            this.lblMemory.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // lblCpu
            // 
            this.lblCpu.AutoSize = false;
            this.lblCpu.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Right | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblCpu.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblCpu.Name = "lblCpu";
            this.lblCpu.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblCpu.Size = new System.Drawing.Size(60, 17);
            this.lblCpu.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // pnlDraw
            // 
            this.pnlDraw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDraw.Location = new System.Drawing.Point(0, 49);
            this.pnlDraw.Name = "pnlDraw";
            this.pnlDraw.Size = new System.Drawing.Size(792, 502);
            this.pnlDraw.TabIndex = 4;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.pnlDraw);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "gokiRegeas";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pathsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblFilename;
        private System.Windows.Forms.ToolStripStatusLabel lblTimeStatus;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnToolStripContinuePause;
        private System.Windows.Forms.ToolStripButton btnToolStripPrevious;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnToolStripNext;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSplitButton btnToolStripLengths;
        private System.Windows.Forms.ToolStripStatusLabel lblHistory;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuViewHorizontalFlip;
        private System.Windows.Forms.ToolStripMenuItem mnuViewVerticalFlip;
        private System.Windows.Forms.ToolStripMenuItem nextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previousToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem backgroundColorToolStripMenuItem;
        private DoubleBufferedPanel pnlDraw;
        private System.Windows.Forms.ToolStripButton btnToolStripReset;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mnuViewBigTimer;
        private System.Windows.Forms.ToolStripMenuItem mnuViewAlwaysShowTimer;
        private System.Windows.Forms.ToolStripMenuItem mnuViewAlwaysOnTop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripDropDownButton btnToolStripTimerOpacity;
        private System.Windows.Forms.ToolStripMenuItem byGokiburikinToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem donateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuViewGreyscale;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton btnToolStripZoom;
        private System.Windows.Forms.ToolStripMenuItem mnuViewResetView;
        private System.Windows.Forms.ToolStripMenuItem importFilepathsFromClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuViewUpdateInterval;
        private System.Windows.Forms.ToolStripStatusLabel lblFlagBicubic;
        private System.Windows.Forms.ToolStripStatusLabel lblMemory;
        private System.Windows.Forms.ToolStripStatusLabel lblCpu;
    }
}

