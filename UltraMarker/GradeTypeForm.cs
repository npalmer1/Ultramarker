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
    public partial class GradeTypeForm : Form
    {
        private int Nm;

        public int Passvalue
        {
            get { return Nm; }
            set { Nm = value; }
        }

        public GradeTypeForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Passvalue = 0;
            }
            if (radioButton2.Checked)
            {
                Passvalue = 1;
            }
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GradeTypeForm_Load(object sender, EventArgs e)
        {

        }
    }
}
