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
    public partial class CriteriaSelectionForm : Form
    {
        private int Nm;

        public int Passvalue
        {
            get { return Nm; }
            set { Nm = value; }
        }

        public CriteriaSelectionForm()
        {
            InitializeComponent();
        }

        private void CriteriaSelectionForm_Load(object sender, EventArgs e)
        {
            if (Nm == 0)
            {
                radioButton1.Checked = true;
            }
            else if (Nm == 1)
            {
                radioButton2.Checked = true;
            }
            else if (Nm == 2)
            {
                radioButton3.Checked = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                DialogResult dialogResult = MessageBox.Show("Clear all de-selected criteria Yes/No?", "Clear deslected criteria", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Passvalue = 0;
                }
                else
                {
                    Passvalue = 0;
                }
            }
            else if (radioButton2.Checked)
            {
                Passvalue = 1;
            }
            else if (radioButton3.Checked)
            {
                Passvalue = 2;
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
