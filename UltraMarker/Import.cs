using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace UltraMarker
{
    public partial class Import : Form
    {
        public char RoundUpDown = '1';
        public Import()
        {
            InitializeComponent();
        }


        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           if (radioButton2.Checked) //round up & down
            {
                RoundUpDown = '1';
            }
            else //round up only
            {
                RoundUpDown = '2';
            }
            this.Close();
           
        }

        private void Import_Load(object sender, EventArgs e)
        {
            if (RoundUpDown == '1')
            {
                radioButton2.Checked = true;
            }
            else if (RoundUpDown == '2')
            {
                radioButton3.Checked = true;
            }
        }
    }
}
