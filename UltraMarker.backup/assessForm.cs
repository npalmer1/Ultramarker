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
    public partial class assessForm : Form
    {

        public bool cancelEdit = true;
        bool EditMode = true;
        public string thisLOFile;

        private string[] Nm = new string[6];

        public string[] Passvalue 
        {
            get { return Nm; }
            set { Nm = value; }
        }
    
        public assessForm()
        {
            InitializeComponent();
        }


       

        private void button1_Click(object sender, EventArgs e)
        {
            //save data
            if (EditMode)
            {
                cancelEdit = false;
                //DialogResult dialogResult = MessageBox.Show("Save assessment information Yes/No?", "Assessment Information", MessageBoxButtons.YesNo);
                //if (dialogResult == DialogResult.Yes)
                //{
                    Passvalue[0] = textBox1.Text; //title
                    Passvalue[1] = textBox2.Text; //desc
                    Passvalue[2] = textBox3.Text; //code
                    Passvalue[3] = comboBox1.Text; //weight
                   
                //}
            }
           
            this.Hide();
        }

        private void assessForm_Load(object sender, EventArgs e)
        {
            string vis = "";
            for (int i = 1; i <= 100; i++)
            {
                comboBox1.Items.Add(Convert.ToString(i));
            }
            textBox1.Text = Passvalue[0]; //assessment title
            textBox2.Text = Passvalue[1]; //description of assessment
            textBox3.Text = Passvalue[2]; //code or number of assessment
            comboBox1.Text = Passvalue[3]; //weighting in unit                       
            vis = Passvalue[4];
            if (vis.Contains("read"))
            { enableBoxes(true); }
            else 
            { enableBoxes(false); }
            textBox2.Select(0, 0);
        }
        private void enableBoxes(bool b)
        {
            textBox1.ReadOnly = b;
            textBox2.ReadOnly = b;
            textBox3.ReadOnly = b;
            comboBox1.Enabled = !b;
            EditMode = !b;
        }

        private void button2_Click(object sender, EventArgs e)
        {   //cancel
            if (EditMode)
            {   
                //DialogResult dialogResult = MessageBox.Show("Exit without saving assessment info. Yes/No?", "Assessment Information", MessageBoxButtons.YesNo);
                //if (dialogResult == DialogResult.Yes)
                //{
                    this.Hide();
                //}
            }
            else
            {
                this.Hide();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}
