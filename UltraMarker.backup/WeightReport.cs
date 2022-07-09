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
    public partial class WeightReport : Form
    {
        public string WReport = "";

        public WeightReport()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void WeightReport_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = WReport;
        }
    }
}
