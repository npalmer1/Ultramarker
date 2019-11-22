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
    public partial class feedselectForm : Form
    {
        private bool[] Nm = new bool[20];

        public bool[] Passvalue
        {
            get { return Nm; }
            set { Nm = value; }
        }
        public feedselectForm()
        {
            InitializeComponent();
        }

        private void feedselectForm_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = Passvalue[0];
            checkBox2.Checked = Passvalue[1];
            checkBox3.Checked = Passvalue[2];
            checkBox4.Checked = Passvalue[3];
            checkBox5.Checked = Passvalue[4];
            checkBox6.Checked = Passvalue[5];
            checkBox7.Checked = Passvalue[6];
            checkBox8.Checked = Passvalue[7];
            checkBox9.Checked = Passvalue[8];
            checkBox10.Checked = Passvalue[9];
            checkBox11.Checked = Passvalue[10];
            checkBox12.Checked = Passvalue[11];
            checkBox13.Checked = Passvalue[12];
            checkBox14.Checked = Passvalue[13];
            checkBox15.Checked = Passvalue[14];

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Save feedback report options Yes/No?", "Feedback report", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Passvalue[0] = checkBox1.Checked;
                Passvalue[1] = checkBox2.Checked;
                Passvalue[2] = checkBox3.Checked;
                Passvalue[3] = checkBox4.Checked;
                Passvalue[4] = checkBox5.Checked;
                Passvalue[5] = checkBox6.Checked;
                Passvalue[6] = checkBox7.Checked;
                Passvalue[7] = checkBox8.Checked;
                Passvalue[8] = checkBox9.Checked;
                Passvalue[10] = checkBox11.Checked;
                Passvalue[11] = checkBox12.Checked;
                Passvalue[12] = checkBox13.Checked;
                Passvalue[13] = checkBox14.Checked;
                Passvalue[14] = checkBox15.Checked;
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Exit without saving feedback options Yes/No?", "Feedback report", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
