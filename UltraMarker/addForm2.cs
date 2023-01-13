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
    public partial class addForm2 : Form  //windows only
    {
        private string[] Nm = new string[2];

        //public CommentForm CForm = new CommentForm();
        CommentForm CForm = new CommentForm();
        public string ComFile;
        //Control[] control1;

        public string[] Passvalue
        {
            get { return Nm; }
            set { Nm = value; }
        }
        public addForm2()
        {
            InitializeComponent();           
        }
        private void addForm2_Load(object sender, EventArgs e)
        {
            label2.Text = Passvalue[0];
            addComment1.TextBox1.Text = Passvalue[1];
            addComment1.ComFile = ComFile;
            addComment1.TextBox1.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Visible;
            //addComment1.TextBox1.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Visible;
            addComment1.TextBox1.TextWrapping = System.Windows.TextWrapping.Wrap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
                Passvalue[1] = addComment1.TextBox1.Text;
                this.ComFile = addComment1.ComFile;
                this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Clear feedback comments for this criteria Yes/No?", "Clear comments", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                addComment1.TextBox1.Text = "";
            }
        }

       /* private void OpenCommentsFile()
        {
            string CommentStr;

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
                        //textBox1.Text = textBox1.Text.Insert(textBox1.SelectionStart + textBox1.SelectionLength, CommentStr);

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
        }*/

        private void button2_Click(object sender, EventArgs e)
        {
            this.ComFile = addComment1.ComFile;
            this.Close();
        }

       /* private void elementHost1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }*/

        private void label2_Click(object sender, EventArgs e)
        {

        }

    }
}
