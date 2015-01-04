using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gokiRegeas
{
    public partial class frmColorSelection : Form
    {
        private int red = 0;

        public int Red
        {
            get { return red; }
            set { red = value; }
        }

        private int green = 0;

        public int Green
        {
            get { return green; }
            set { green = value; }
        }

        private int blue = 0;

        public int Blue
        {
            get { return blue; }
            set { blue = value; }
        }
        public frmColorSelection()
        {
            InitializeComponent();
            btnOk.Click += btnOk_Click;
            btnCancel.Click += btnCancel_Click;
            numRed.ValueChanged += colorChanged;
            numGreen.ValueChanged += colorChanged;
            numBlue.ValueChanged += colorChanged;
        }

        void colorChanged(object sender, EventArgs e)
        {
            updatePanel();
        }

        public void setColor ( Color color)
        {
            numRed.Value= color.R;
            numGreen.Value = color.G;
            numBlue.Value = color.B;
        }

        public void updatePanel()
        {
            pnlColor.BackColor = Color.FromArgb((int)numRed.Value, (int)numGreen.Value, (int)numBlue.Value);
            pnlColor.Invalidate();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        void btnOk_Click(object sender, EventArgs e)
        {
            Red = (int)numRed.Value;
            Green = (int)numGreen.Value;
            Blue = (int)numBlue.Value;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
