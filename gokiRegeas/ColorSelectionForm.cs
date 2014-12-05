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
