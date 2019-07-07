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
using System.Diagnostics;

namespace UltraMarker
{
    public partial class GradeGroup : Form
    {
        public int PeerReview;

        public string TemplateFile;
        public string OutputFile;
        public string OutFilePath;
        public string Institution = "";
        public string UnitTitle = "";
        public string UnitCode;
        public string Level;
      
        public string AssessNo;
        public string AssessTitle;
        public int Weight;

        public string[] G = new string[10];
        public string[] C = new string[10];
        public string[] CM = new string[10];
        public string[] CF = new string[10];

       

        Color c1;
        bool changesSaved = false;

        private Font printFont;
        private StreamReader streamToPrint;
        string printFilePath;
        string lineToPrint = ""; //lines for the printer
        string[] textlines; //takes lines of text from richtextbox
        int linestoGo = 0; //number of lines in richtextbox remaining for printing

        public GradeGroup()
        {
            InitializeComponent();
        }

        private void GradeGroup_Load(object sender, EventArgs e)
        {
            changesSaved = false;
            LoadTemplate(TemplateFile);
            this.Text = "Assessment";
           
            ModifyGeneratedForm();
        }

        private void LoadTemplate(string filename)
        {
            string str2 = "";
            string str = "";
            try
            {
                richTextBox1.LoadFile(filename, RichTextBoxStreamType.RichText);

            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }

        }
        private void ReplaceString(string str, string str2)
        {
            try
            {
                //Clipboard clip;
                if (str2 == null)
                {
                    str2 = "";
                }
                int i = richTextBox1.Find(str);
                if (i > -1)
                {
                    richTextBox1.Select(i, str.Length);

                    //richTextBox1.Cut();
                    Clipboard.Clear();
                    if (str2.Length > 0)
                    {
                        Clipboard.SetText(str2);
                    }
                    else { Clipboard.SetText(" "); }
                    richTextBox1.Paste();
                }
            }
            catch
            {
            }
        }
        private void ModifyGeneratedForm()
        {
            ReplaceString("%Institution%", Institution);
            ReplaceString("%UnitTitle%", UnitTitle);
            ReplaceString("%UnitCode%", UnitCode);
            ReplaceString("%Level%", Level);
            
            ReplaceString("%AssessNo%", AssessNo);
            ReplaceString("%AssessTitle%", AssessTitle);
            try
            {
                ReplaceString("%Weight%", Weight.ToString());
            }
            catch { }


            ReplaceString("%G1%", G[0]);
            ReplaceString("%G2%", G[1]);
            ReplaceString("%G3%", G[2]);
            ReplaceString("%G4%", G[3]);
            ReplaceString("%G5%", G[4]);
            ReplaceString("%G6%", G[5]);
            ReplaceString("%G7%", G[6]);
            ReplaceString("%G8%", G[7]);
            ReplaceString("%G9%", G[8]);
            ReplaceString("%G10%", G[9]);
            ReplaceString("%Criteria1%", C[0]);
            ReplaceString("%Criteria2%", C[1]);
            ReplaceString("%Criteria3%", C[2]);
            ReplaceString("%Criteria4%", C[3]);
            ReplaceString("%Criteria5%", C[4]);
            ReplaceString("%Criteria6%", C[5]);
            ReplaceString("%Criteria7%", C[6]);
            ReplaceString("%Criteria8%", C[7]);
            ReplaceString("%Criteria7%", C[8]);
            ReplaceString("%Criteria8%", C[9]);
        }

       
        private void savebutton_Click(object sender, EventArgs e)
        {
            string fname = "";
            saveFileDialog1.InitialDirectory = OutFilePath;

            saveFileDialog1.FileName = UnitCode+ "_" + AssessNo + ".rtf";
            saveFileDialog1.DefaultExt = ".rtf";          
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Save_Report(saveFileDialog1.FileName);
            changesSaved = true;
            richTextBox1.ReadOnly = true;
            cancelbutton.Visible = false;
            editbutton.Visible = true;
            richTextBox1.BackColor = c1;
            printbutton.Visible = true;
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

        private void editbutton_Click(object sender, EventArgs e)
        {
            c1 = richTextBox1.BackColor;
            richTextBox1.BackColor = Color.Beige;
            richTextBox1.ReadOnly = false;
            cancelbutton.Visible = true;
            editbutton.Visible = false;
        }

        private void cancelbutton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Cancel edit (all changes will be lost) Yes/No?", "Cancel Edit Report", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                cancelbutton.Visible = false;
                editbutton.Visible = true;
                richTextBox1.ReadOnly = true;
                richTextBox1.BackColor = c1;
            }
        }

        private void closebutton_Click(object sender, EventArgs e)
        {
            if (!changesSaved)
            {
                DialogResult dialogResult = MessageBox.Show("Save document first ? Yes/No?", "Exit Document", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    saveFileDialog1.DefaultExt = "rtf";
                    //saveFileDialog1.FileName = textBox1.Text.Trim();
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

        private void printbutton_Click(object sender, EventArgs e)
        {
            Printing_From_RichTextBox();
        }
    }

}
