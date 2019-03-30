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
    public partial class ShowLO : Form
    {
        private string[] Nm = new string[2];

        public string[] Passvalue
        {
            get { return Nm; }
            set { Nm = value; }
        }

        public string LOTitle;
        public string LODesc;
        public string LOType;

        public ShowLO()
        {
            InitializeComponent();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void ShowLO_Load(object sender, EventArgs e)
        {
            try
            {
                if (LOTitle != null)
                {
                    textBox1.Text = LOTitle;
                    if (LODesc != null)
                    {
                        textBox3.Text = LODesc;
                    }
                    textBox2.Text = LOType;
                }
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
