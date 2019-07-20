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
        public static int maxGradeGroups = 10;
        public static int maxCriteria = 10;
        public int PeerReview;

        public string TemplateFile;
        public string OutputFile;
        public string OutFilePath;
        public string Institution = "";
        public string UnitTitle = "";
        public string UnitCode;
        public string Level;

        public string student;

        public string AssessNo;
        public string AssessTitle;
        public int Weight;

        public string[] G = new string[maxGradeGroups]; //grades
        public string[] C = new string[maxCriteria]; //criteria descr
        public string[,] CG = new string[maxCriteria, maxGradeGroups]; //criteria for grade
        public string[] CF = new string[maxCriteria]; //criteria feedback
        public string[] CT = new string[maxCriteria]; // criteria title
        public string[] CM = new string[maxCriteria]; //mark for criteria
        public string[] comment = new string[maxCriteria]; // comment for mark
        public string OG; // overallgrade
        public string OP; // overall mark
        public string overall; //overall comments for whole assessment



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
            richTextBox1.Clear();
            Clipboard.Clear();
            LoadTemplate(TemplateFile);
            this.Text = "Assessment";
            richTextBox1.Font = new Font("Calibri", 9);
           
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
                Clipboard.Clear();
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
            richTextBox1.ReadOnly = false;
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

            ReplaceString("%Student%", student);
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
            ReplaceString("%CG1%", CG[0, 0]);
            ReplaceString("%CG2%", CG[0, 1]);
            ReplaceString("%CG3%", CG[0, 2]);
            ReplaceString("%CG4%", CG[0, 3]);
            ReplaceString("%CG5%", CG[0, 4]);
            ReplaceString("%CG6%", CG[0, 5]);
            ReplaceString("%CG7%", CG[0, 6]);
            ReplaceString("%CG8%", CG[0, 7]);
            ReplaceString("%CG9%", CG[0, 8]);
            ReplaceString("%CG10%", CG[0, 9]);
            ReplaceString("%CG11%", CG[1, 0]);
            ReplaceString("%CG12%", CG[1, 1]);
            ReplaceString("%CG13%", CG[1, 2]);
            ReplaceString("%CG14%", CG[1, 3]);
            ReplaceString("%CG15%", CG[1, 4]);
            ReplaceString("%CG16%", CG[1, 5]);
            ReplaceString("%CG17%", CG[1, 6]);
            ReplaceString("%CG18%", CG[1, 7]);
            ReplaceString("%CG19%", CG[1, 8]);
            ReplaceString("%CG20%", CG[1, 9]);
            ReplaceString("%CG21%", CG[2, 0]);
            ReplaceString("%CG22%", CG[2, 1]);
            ReplaceString("%CG23%", CG[2, 2]);
            ReplaceString("%CG24%", CG[2, 3]);
            ReplaceString("%CG25%", CG[2, 4]);
            ReplaceString("%CG26%", CG[2, 5]);
            ReplaceString("%CG27%", CG[2, 6]);
            ReplaceString("%CG28%", CG[2, 7]);
            ReplaceString("%CG29%", CG[2, 8]);
            ReplaceString("%CG30%", CG[2, 9]);
            ReplaceString("%CrTitle1%", CT[0]);
            ReplaceString("%CrTitle2%", CT[1]);
            ReplaceString("%CrTitle3%", CT[2]);
            ReplaceString("%CrTitle4%", CT[3]);
            ReplaceString("%CrTitle5%", CT[4]);
            ReplaceString("%CrTitle6%", CT[5]);
            ReplaceString("%CrTitle7%", CT[6]);
            ReplaceString("%CrTitle8%", CT[7]);
            ReplaceString("%CrTitle7%", CT[8]);
            ReplaceString("%CrTitle8%", CT[9]);
            ReplaceString("%CrMark1%", CM[0]);
            ReplaceString("%CrMark2%", CM[1]);
            ReplaceString("%CrMark3%", CM[2]);
            ReplaceString("%CrMark4%", CM[3]);
            ReplaceString("%CrMark5%", CM[4]);
            ReplaceString("%CrMark6%", CM[5]);
            ReplaceString("%CrMark7%", CM[6]);
            ReplaceString("%CrMark8%", CM[7]);
            ReplaceString("%CrMark7%", CM[8]);
            ReplaceString("%CrMark8%", CM[9]);
            ReplaceString("%CrComment1%", comment[0]);
            ReplaceString("%CrComment2%", comment[1]);
            ReplaceString("%CrComment3%", comment[2]);
            ReplaceString("%CrComment4%", comment[3]);
            ReplaceString("%CrComment5%", comment[4]);
            ReplaceString("%CrComment6%", comment[5]);
            ReplaceString("%CrComment7%", comment[6]);
            ReplaceString("%CrComment8%", comment[7]);
            ReplaceString("%CrComment7%", comment[8]);
            ReplaceString("%CrComment8%", comment[9]);
            ReplaceString("%overallGrade%", OG);
            ReplaceString("%overallPercent%", OP);
            ReplaceString("%overall%", overall);
            richTextBox1.ReadOnly = true;
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
                    printFont = new Font("Calibri", 9, FontStyle.Regular);                
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
