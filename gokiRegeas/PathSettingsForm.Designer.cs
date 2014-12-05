namespace gokiRegeas
{
    partial class frmPathSettings
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddPath = new System.Windows.Forms.Button();
            this.btnRemovePath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnTogglePath = new System.Windows.Forms.Button();
            this.lstPaths = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(325, 298);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(137, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAddPath
            // 
            this.btnAddPath.Location = new System.Drawing.Point(18, 269);
            this.btnAddPath.Name = "btnAddPath";
            this.btnAddPath.Size = new System.Drawing.Size(137, 23);
            this.btnAddPath.TabIndex = 3;
            this.btnAddPath.Text = "Add Path";
            this.btnAddPath.UseVisualStyleBackColor = true;
            this.btnAddPath.Click += new System.EventHandler(this.btnAddPath_Click);
            // 
            // btnRemovePath
            // 
            this.btnRemovePath.Location = new System.Drawing.Point(18, 298);
            this.btnRemovePath.Name = "btnRemovePath";
            this.btnRemovePath.Size = new System.Drawing.Size(137, 23);
            this.btnRemovePath.TabIndex = 4;
            this.btnRemovePath.Text = "Remove Path";
            this.btnRemovePath.UseVisualStyleBackColor = true;
            this.btnRemovePath.Click += new System.EventHandler(this.btnRemovePath_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(447, 23);
            this.label1.TabIndex = 5;
            this.label1.Text = "Path List";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 227);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(450, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = " Drag and Drop folders here";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnTogglePath
            // 
            this.btnTogglePath.Location = new System.Drawing.Point(15, 194);
            this.btnTogglePath.Name = "btnTogglePath";
            this.btnTogglePath.Size = new System.Drawing.Size(140, 23);
            this.btnTogglePath.TabIndex = 9;
            this.btnTogglePath.Text = "Toggle";
            this.btnTogglePath.UseVisualStyleBackColor = true;
            this.btnTogglePath.Click += new System.EventHandler(this.btnEnablePath_Click);
            // 
            // lstPaths
            // 
            this.lstPaths.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstPaths.FullRowSelect = true;
            this.lstPaths.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstPaths.Location = new System.Drawing.Point(12, 41);
            this.lstPaths.Name = "lstPaths";
            this.lstPaths.Size = new System.Drawing.Size(450, 147);
            this.lstPaths.TabIndex = 11;
            this.lstPaths.UseCompatibleStateImageBehavior = false;
            this.lstPaths.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 446;
            // 
            // frmPathSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 333);
            this.Controls.Add(this.lstPaths);
            this.Controls.Add(this.btnTogglePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRemovePath);
            this.Controls.Add(this.btnAddPath);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPathSettings";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Path Settings";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAddPath;
        private System.Windows.Forms.Button btnRemovePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnTogglePath;
        private System.Windows.Forms.ListView lstPaths;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}