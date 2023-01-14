using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Controls;


namespace UltraMarker
{
    public partial class addForm : Form
    {
      
        private string[] Nm = new string[2];

        //public CommentForm CForm = new CommentForm();
        CommentForm CForm = new CommentForm();
        public string ComFile;
        public bool isLinux;
        public bool viewonly = false;

        public string[] Passvalue
        {
            get { return Nm; }
            set { Nm = value; }
        }
        public addForm()
        {
            InitializeComponent();
        }

        private void addForm_Load(object sender, EventArgs e)
        {
            
            button1.Visible = !viewonly;
            button3.Visible = !viewonly;
            
            label2.Text = Passvalue[0];
            textBox1.Text = Passvalue[1];
            
        }
        private void addForm_Closing(object sender, EventArgs e)
        {
            Close();
        }


        private void button1_Click(object sender, EventArgs e)  //save
        {
            Passvalue[1] = textBox1.Text;
            this.Close();
            //this.Hide();
           
        }

        private void button3_Click(object sender, EventArgs e)  //clear form
        {
            DialogResult dialogResult = MessageBox.Show("Clear feedback comments for this criteria Yes/No?", "Clear comments", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                textBox1.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)  //close form
        {
            this.Close();
            //this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
                          
        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            string CommentStr;
            if (viewonly)
            {
                return;
            }
           
            try
            {
                if (File.Exists(ComFile))
                {
                    CForm.selectComment = true; //loads form anyway
                    CForm.CFile = ComFile;
                    CForm.ShowDialog();
                    CommentStr = CForm.Passvalue;
                    if (CommentStr != null && CommentStr != "")
                    {
                        textBox1.Text = textBox1.Text.Insert(textBox1.SelectionStart + textBox1.SelectionLength, CommentStr);

                    }
                    ComFile = CForm.CFile;
                }
                else
                { 

                    MessageBox.Show("No comments file selected - you need to select or create one from Comments menu");
                }
                
            }
            catch
            {
                MessageBox.Show("Error");
            }
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

       
    }
}
