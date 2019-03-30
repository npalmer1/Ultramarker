using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UltraMarker
{
    public partial class labelForm : Form
    {

        private string[] Nm = new string[2];
        public string[] Passvalue
        {
            get { return Nm; }
            set { Nm = value; }
        }
        public labelForm()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            this.Close();      
        }

        private void labelForm_Load(object sender, EventArgs e)
        {
            label1.Text = Nm[0];
            timer1.Interval = Convert.ToInt32(Nm[1]);
            timer1.Enabled = true;
        }
    }
}
