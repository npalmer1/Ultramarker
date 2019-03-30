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
            switch (Nm)
            {
                case 0:
                    {
                        radioButton1.Checked = true;
                        break;
                    }
                case 1:
                    {
                        radioButton2.Checked = true;
                        break;
                    }
                case 2:
                    {
                        radioButton3.Checked = true;
                        break;
                    }
                case 3:
                    {
                        radioButton4.Checked = true;                      
                        break;
                    }
                case 4:
                    {
                        radioButton5.Checked = true;                        
                        break;
                    }
            }
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
            if (radioButton4.Checked)
            {
                Passvalue = 3;
            }
            if (radioButton5.Checked)
            {
                Passvalue = 4;
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "Choose this if NO Sub-Criteria are used, or Sub-Criteria are being used, \n but the weightings for each subcriteria are not part of the weighting for the criteria. \nFor example Criteria 1 has a weighting of 50%, Subcriteria 1 has a weighting \nof 30% and Subcriteria 2 has a weighting of 20%";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "Sub-Criteria are used - and the weightings for each subcriteria are part of\n the weighting for the criteria. For example Criteria 1 has a weighting of 50%,\n Subcriteria 1 has a weighting of 60% and Subcriteria 2 has a weighting of 40% \nand together they comprise the mark for Criteria 1";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "Checkboxes are used to determine whether a criteria has been selected. \nChoose this if NO Sub-Criteria are used, or Sub-Criteria are used, but \n the weightings for each subcriteria are not part of the weighting for the criteria. \nFor example Criteria 1 has a weighting of 50%, Subcriteria 1 has a weighting of 30% \nand Subcriteria 2 has a weighting of 20%";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "Checkboxes are used to determine whether a criteria has been selected. \nSub-criteria are used - and the weightings for each subcriteria are part \nof the weighting for the criteria. For example Criteria 1 has a weighting of 50%, \nSubcriteria 1 has a weighting of 60% and Subcriteria 2 has a weighting of 40% \nand together they comprise the mark for Criteria 1";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "Checkboxes are used - if a box is ticked the mark for this criteria \nis 'yes' (100%) and if not ticked it is 'no' (0%)";
        }

    }
}
