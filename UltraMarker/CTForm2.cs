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
    public partial class CTForm : Form
    {
        private int Nm;

        public int Passvalue
        {
            get { return Nm; }
            set { Nm = value; }
        }

        public CTForm()
        {
            InitializeComponent();
        }

        private void CTForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Passvalue = 0;
            }
            if (radioButton2.Checked)
            {
                Passvalue = 1;
    
            }
            if (radioButton3.Checked)
            {
                Passvalue = 2;
            }
            this.Hide();
         
        }
    }
}
