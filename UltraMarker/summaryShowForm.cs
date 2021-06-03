using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;

namespace UltraMarker
{
    public partial class summaryShowForm : Form
    {
        private Font printFont;
        private StreamReader streamToPrint;
        public string filePath;
        public string module;
        

        private string Nm;

        public string Passvalue
        {
            get { return Nm; }
            set { Nm = value; }
        }

        public summaryShowForm()
        {
            InitializeComponent();
        }

        private void summaryShowForm_Load(object sender, EventArgs e)
        {
            int i = 0;
            string str = Passvalue;
            string str2 = "Report for assessment: ";
            richTextBox1.Font = new Font("Calibri", 10, FontStyle.Regular);
            richTextBox1.Text = str;
            if (str.Contains("Summary report for assessment: "))
            {

                //i = str.IndexOf("----");
                //label1.Text = str2 + str.Substring("Summary report for assessment: ".Length, i - "Summary report for assessment: ".Length).Trim();
                label1.Text = module;
            }
               
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Printing();
            
        }

        private void Printing()
        {
            try
            {
                streamToPrint = new StreamReader(filePath);
                try
                {
                    printFont = new Font("Calibri", 10, FontStyle.Regular);
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    // Print the document.
                    pd.Print();
                }
                finally
                {
                    streamToPrint.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // The PrintPage event is raised for each page to be printed. 
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            String line = null;

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics);

            // Iterate over the file, printing each line. 
            while (count < linesPerPage &&
               ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black,
                   leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page. 
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }
    }
}
