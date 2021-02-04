using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing.Printing;

namespace UltraMarker
{
    public partial class reportForm : Form
    {
        private Font printFont;
        private StreamReader streamToPrint;
        public string printFilePath;
        string lineToPrint = ""; //lines for the printer
        string[] textlines; //takes lines of text from richtextbox
        int linestoGo = 0; //number of lines in richtextbox remaining for printing

        public string emailaddress = "";

        bool changesSaved = false;
        Color c1;

        public string filepath = "";
        private string Nm;
        
        public RichTextBox RTB = new RichTextBox();
        public string StuName = "";
        public string unitTitle;
        public string assessTitle = "";
        public string attachfile = "";
        public string UnitLeader;
        public string addstr;
        
        
        
        public string Passvalue
        {
            get { return Nm; }
            set { Nm = value; }
        }

        public reportForm()
        {
            InitializeComponent();
        }

        private void reportForm_Load(object sender, EventArgs e)
        {
            
            //string str = "";
            int i = 0;
            string nl = System.Environment.NewLine;

            //string f = Passvalue;
            //Read_Feedback_File(f);
            
            //richTextBox1.Font = new Font("Calibri", 10, FontStyle.Regular);
            changesSaved = false;

            Read_Feedback();
           
            if (textBox1.Text.Length < 1)
            {
                MessageBox.Show("Student name is blank - no report loaded");
                this.Close();
            }

        }

        private void Read_Feedback_File(string filename)
        {
            richTextBox1.Text = "";
           
            if (filename.Length > 0)
            {
                richTextBox1.LoadFile(filename);
                
            }
        }

        private void Read_Feedback()
        {
            richTextBox1.Text = "";
            richTextBox1.ShortcutsEnabled = true;
            richTextBox1.Rtf = RTB.Rtf;
            
            textBox1.Text = StuName;
        }

        private void Save_Report(string filename)
        {
            try
            {
                richTextBox1.SaveFile(filename);
                
               
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
            printFilePath = filename;

        }



        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            attachfile = saveFileDialog1.FileName;
            Save_Report(saveFileDialog1.FileName);
            changesSaved = true;
            richTextBox1.ReadOnly = true;
            cancelButton.Visible = false;
            editButton.Visible = true;
            richTextBox1.BackColor = c1;
            printButton.Visible = true;
            Emailbutton.Visible = true;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (filepath.Length > 0)
            {
                saveFileDialog1.InitialDirectory = filepath;
            }
            saveFileDialog1.DefaultExt = "rtf";
            saveFileDialog1.FileName = textBox1.Text.Trim() + addstr;
            saveFileDialog1.ShowDialog();

        }

        private void editButton_Click_1(object sender, EventArgs e)
        {
            //changesSaved = false;
            c1 = richTextBox1.BackColor;
            richTextBox1.BackColor = Color.Beige;
            RTB.Text = richTextBox1.Text;
            richTextBox1.ReadOnly = false;
            cancelButton.Visible = true;
            editButton.Visible = false;
            Emailbutton.Visible = false;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Cancel edit (all changes will be lost) Yes/No?", "Cancel Edit Report", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                cancelButton.Visible = false;
                editButton.Visible = true;
                richTextBox1.ReadOnly = true;
                richTextBox1.BackColor = c1;
                richTextBox1.Text = RTB.Text;
               
            }

        }

        private void closeButton_Click_1(object sender, EventArgs e)
        {
            if (!changesSaved)
            {
                DialogResult dialogResult = MessageBox.Show("Save report first ? Yes/No?", "Exit Report", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    saveFileDialog1.DefaultExt = "rtf";
                    saveFileDialog1.FileName = textBox1.Text.Trim()+ addstr;
                    saveFileDialog1.ShowDialog();
                    
                    this.Close();
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
          
            Printing_From_RichTextBox();
            //PrintingFromFile();
           
        }

        private void Printing_From_RichTextBox()
        {
            string txt = richTextBox1.Text;
            textlines = txt.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            linestoGo = 0;
            PrintDialog printDlg = new PrintDialog();
            try
            {
                if (printDlg.ShowDialog() == DialogResult.OK)
                {                  
                        printFont = new Font("Calibri", 10, FontStyle.Regular);
                        PrintDocument pd = new PrintDocument();
                        pd.PrintPage += new PrintPageEventHandler(pd_PrintPageWrapTextBox);
                        
                        // Print the document.
                        pd.Print();       
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        


        // The PrintPage event is raised for each page to be printed. 
        private void pd_PrintPageWrapTextBox(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left - 50;
            float topMargin = ev.MarginBounds.Top - 50;
           
            string str = "";
            bool linend = false;
            

            StringFormat format = new StringFormat();

            //format.FormatFlags = StringFormatFlags.LineLimit;

            // Calculate the number of lines per page.
            linesPerPage = (ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics)) + 4;

            // Iterate over the file, printing each line. 
            while (count < linesPerPage && (linestoGo < textlines.Length))
            {
                lineToPrint = textlines[linestoGo];
                linend = false;
                while (!linend)
                {
                    str = PageWrap();
                    if (str.Length < 110) { linend = true; }

                    yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                    ev.Graphics.DrawString(str, printFont, Brushes.Black,
                    leftMargin, yPos, new StringFormat());
                    count++;
                }
                linestoGo++;
            }

            // If more lines exist, print another page. 
            if (linestoGo < textlines.Length)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }

        private string PageWrap()
        {
            //look for spaces so that only whole words are included in the line
            string str = "";
            int i = 0;
            try
            {
                string[] strSplit = lineToPrint.Split(' ');
                while (str.Length < 110)
                {
                    str = str + " " + strSplit[i];
                    i++;
                    if (i >= strSplit.Length)
                    {
                        break;
                    }
                }
                if (lineToPrint.Length >= str.Length)
                {
                    lineToPrint = lineToPrint.Substring(str.Length, (lineToPrint.Length) - str.Length);
                }
                else
                {
                    lineToPrint = "";
                }
            }
            catch
            {
            }

            return str;
        }











        private void PrintingFromFile() //not used
        {
            PrintDialog printDlg = new PrintDialog();
            try
            {
                if (printDlg.ShowDialog() == DialogResult.OK)
                {
                    streamToPrint = new StreamReader(printFilePath);
                    try
                    {
                        
                        printFont = new Font("Calibri", 10, FontStyle.Regular);
                        PrintDocument pd = new PrintDocument();
                        pd.PrintPage += new PrintPageEventHandler(pd_PrintPageWrap);

                        // Print the document.
                        pd.Print();
                    }
                    finally
                    {
                        streamToPrint.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // The PrintPage event is raised for each page to be printed. 
        private void pd_PrintPageWrap(object sender, PrintPageEventArgs ev) //not used
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left - 50;
            float topMargin = ev.MarginBounds.Top - 50;
            String line = null;
            string str = "";
            bool linend = false;

            StringFormat format = new StringFormat();

            //format.FormatFlags = StringFormatFlags.LineLimit;

            // Calculate the number of lines per page.
            linesPerPage = (ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics)) + 4;

            // Iterate over the file, printing each line. 
            while (count < linesPerPage &&
               ((line = streamToPrint.ReadLine()) != null))
            {
                lineToPrint = line;
                linend = false;
                while (!linend)
                {
                    str = PageWrap();
                    if (str.Length < 110) { linend = true; }

                    yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                    ev.Graphics.DrawString(str, printFont, Brushes.Black,
                    leftMargin, yPos, new StringFormat());
                    count++;
                }
            }

            // If more lines exist, print another page. 
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }

        private void Emailbutton_Click(object sender, EventArgs e)
        {

            if (emailaddress.Length > 0)
            {
                CreateMailItem();
            }
            else
            {
                MessageBox.Show("No email address");
            }
        }

        private void CreateMailItem()
        {
            string nl = System.Environment.NewLine;
            try
            {
                //Outlook.MailItem mailItem = (Outlook.MailItem)
                // this.Application.CreateItem(Outlook.OlItemType.olMailItem);
                Microsoft.Office.Interop.Outlook.Application app = new Microsoft.Office.Interop.Outlook.Application();
                Microsoft.Office.Interop.Outlook.MailItem mailItem = app.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                mailItem.Attachments.Add(attachfile);
                mailItem.Subject = "Feedback report for " + unitTitle + "," + assessTitle;
                mailItem.To = emailaddress;
                mailItem.Body = "Dear " + StuName + nl + "Please find attached a report on your work for unit: " + unitTitle + ". Assessment: " + assessTitle + nl + "Regards" + nl + UnitLeader;
                //mailItem.Attachments.Add(logPath);//logPath is a string holding path to the log.txt file
                //mailItem.Importance = Microsoft.Office.Tools.Outlook.OlImportance.olImportanceHigh;

                //mailItem.Send();
                mailItem.Display(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void reportForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Emailbutton.Visible = false;
        }
    }
    }

