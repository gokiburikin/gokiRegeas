namespace gokiRegeas
{
    partial class frmColorSelection
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
            this.pnlColor = new System.Windows.Forms.Panel();
            this.lblRed = new System.Windows.Forms.Label();
            this.numRed = new System.Windows.Forms.NumericUpDown();
            this.numGreen = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numBlue = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBlue)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlColor
            // 
            this.pnlColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlColor.Location = new System.Drawing.Point(11, 14);
            this.pnlColor.Name = "pnlColor";
            this.pnlColor.Size = new System.Drawing.Size(159, 100);
            this.pnlColor.TabIndex = 0;
            // 
            // lblRed
            // 
            this.lblRed.Location = new System.Drawing.Point(11, 117);
            this.lblRed.Name = "lblRed";
            this.lblRed.Size = new System.Drawing.Size(88, 23);
            this.lblRed.TabIndex = 1;
            this.lblRed.Text = "Red";
            this.lblRed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numRed
            // 
            this.numRed.Location = new System.Drawing.Point(105, 120);
            this.numRed.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numRed.Name = "numRed";
            this.numRed.Size = new System.Drawing.Size(65, 20);
            this.numRed.TabIndex = 2;
            // 
            // numGreen
            // 
            this.numGreen.Location = new System.Drawing.Point(105, 146);
            this.numGreen.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numGreen.Name = "numGreen";
            this.numGreen.Size = new System.Drawing.Size(65, 20);
            this.numGreen.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Green";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numBlue
            // 
            this.numBlue.Location = new System.Drawing.Point(105, 172);
            this.numBlue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numBlue.Name = "numBlue";
            this.numBlue.Size = new System.Drawing.Size(65, 20);
            this.numBlue.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(11, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "Blue";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(11, 198);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(95, 198);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // frmColorSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(181, 234);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.numBlue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numGreen);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numRed);
            this.Controls.Add(this.lblRed);
            this.Controls.Add(this.pnlColor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmColorSelection";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Set Background Color...";
            ((System.ComponentModel.ISupportInitialize)(this.numRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBlue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlColor;
        private System.Windows.Forms.Label lblRed;
        private System.Windows.Forms.NumericUpDown numRed;
        private System.Windows.Forms.NumericUpDown numGreen;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numBlue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}