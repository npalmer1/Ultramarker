using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace UltraMarker
{
    public partial class help : Form
    {
        public string helpfile = "";
        public help()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void help_Load(object sender, EventArgs e)
        {
            if (File.Exists(helpfile))
                
            {
                try
                {
                    richTextBox1.LoadFile(helpfile);
                    richTextBox1.SelectionFont = new Font("Verdana", 10, FontStyle.Regular);
                }
                catch
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("No help file loaded");
                this.Close();
            }
            
        }
    }
}
