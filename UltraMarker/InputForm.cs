﻿using System;
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
    public partial class InputForm : Form
    {
        public string text;
        public bool browser = false;
        //public string InitDir = "";

        private string Nm;

        public string Passvalue
        {
            get { return Nm; }
            set { Nm = value; }
        }

       

        public InputForm()
        {
            InitializeComponent();
        }

        private void InputForm_Load(object sender, EventArgs e)
        {
            if (browser)
            {
                button3.Visible = true;
                textBox1.ReadOnly = true;
            }
            else
            {
                button3.Visible = false;
                textBox1.ReadOnly = false;
            }
            label1.Text = Passvalue;
            textBox1.Text = text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // OK button
            if (textBox1.Text.Length < 1)
            {
                MessageBox.Show("Empty title");
                return;
            }
            
            Passvalue = textBox1.Text;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Passvalue = text;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //folderBrowserDialog1.RootFolder = 
            folderBrowserDialog1.SelectedPath = text;
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

     
    }
}
