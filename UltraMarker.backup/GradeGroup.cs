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
        string newline = " \\line ";
        string n = " \\line";
        string boldS = "\\b ";
        string highS = "{\\colortbl;\\red0\\green0\\blue0;\\red255\\green0\\blue0;\\red255\\green255\\blue0;}\\highlight3";
        //string highS = "{\\rtlch\\fcs1 \\highlight1}";
        string boldE = "\\b0 ";
        string highE = " \\highlight0 ";
        //string highE = "{\\rtlch\\fcs1 \\highlight0}";
        string italS = "\\i\\f0";
        string italE = "\\i0";

        public static int maxGradeGroups = 14;
        public static int maxCriteria = 10;
        public int PeerReview;
        public bool addtick = false;
        public bool highlight = false;
        public bool bold = false;
        public bool onlyTick = false;

        public string TemplateFile;
        public string OutputFile;
        public string OutFilePath;
        public string Institution = "";
        public string UnitTitle = "";
        public string UnitCode;
        public string Level;
        public string SittingType;

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
        public bool[,] GChecked = new bool[maxCriteria, maxGradeGroups]; //whihc box is checked when marked?
        public string Marker = "";


        Color c1;
        bool changesSaved = false;
        char tick = '\u2714';
        string nl = Environment.NewLine;

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

        public void ClearChecked()
        {
            for (int i = 0; i < maxCriteria; i++)
            {
                for (int g=0; g < maxGradeGroups; g++)
                {
                    GChecked[i, g] = false;
                }
            }
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
            int i = 0;
            
            try
            {
                if (str == null || str.Trim().Length < 1)
                { 
                    return;
                }
                Clipboard.Clear();
                //Clipboard clip;
                if (str2 == null)
                {
                    str2 = " ";
                }
                 
                str2 = str2.PadRight(str.Length);

                //i = richTextBox1.Find(str,0,richTextBox1.Text.Length, RichTextBoxFinds.None);
                //i = richTextBox1.Text.IndexOf(str);
               
                //i = richTextBox1.Rtf.IndexOf(str);
                i = richTextBox1.Find(str, 0, RichTextBoxFinds.None);
                if (i > -1)
                {
                    richTextBox1.Select(i, str.Length);                                        
                                                           
                    Clipboard.Clear();
                    if (str2.Length > 0)
                    {
                        Clipboard.SetText(str2);
                    }
                    else 
                    { 
                        Clipboard.SetText(" "); 
                    }
                  
                    richTextBox1.Paste();
                    richTextBox1.Modified = true;
                }
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }
        private void ModifyGeneratedForm()
        {
            //MessageBox.Show("Rich Text length = ", Convert.ToString(richTextBox1.Text.Length) + " and TextLength = " + Convert.ToString(richTextBox1.TextLength));
            string padstring = " ".PadRight(100); //pad file to fix bug (diff between text and rich text length)
            richTextBox1.AppendText(padstring);
            
            richTextBox1.ReadOnly = false;
            //richTextBox1.Text = @"{\rtf1\ansi \paperw15840\paperh12240\margl1440\margr1440\margt1440\margb1440\gutter0\ltrsect " + richTextBox1.Text + "}";

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
            
            ReplaceString("%G1%", G[0]);    //grade
            ReplaceString("%G2%", G[1]);
            ReplaceString("%G3%", G[2]);
            ReplaceString("%G4%", G[3]);
            ReplaceString("%G5%", G[4]);
            ReplaceString("%G6%", G[5]);
            ReplaceString("%G7%", G[6]);
            ReplaceString("%G8%", G[7]);
            ReplaceString("%G9%", G[8]);
            ReplaceString("%G10%", G[9]);
            ReplaceString("%G11%", G[10]);
            ReplaceString("%G12%", G[11]);
            ReplaceString("%G13%", G[12]);
            ReplaceString("%G14%", G[13]);
            ReplaceString("%Criteria1%", C[0]); //criteria
            ReplaceString("%Criteria2%", C[1]);
            ReplaceString("%Criteria3%", C[2]);
            ReplaceString("%Criteria4%", C[3]);
            ReplaceString("%Criteria5%", C[4]);
            ReplaceString("%Criteria6%", C[5]);
            ReplaceString("%Criteria7%", C[6]);
            ReplaceString("%Criteria8%", C[7]);
            ReplaceString("%Criteria9%", C[8]);
            ReplaceString("%Criteria10%", C[9]);
            //MessageBox.Show("Rich Text length = ", Convert.ToString(richTextBox1.Text.Length) + " and TextLength = " + Convert.ToString(richTextBox1.TextLength));


            if (highlight || bold && !onlyTick)
            {
               
                for (int c = 0; c < maxCriteria; c++)
                {
                    for (int t = 0; t < maxGradeGroups; t++)
                    {
                        if (GChecked[c, t])
                        {
                            CG[c, t] = "%HStart%" + CG[c, t] + "%HEnd%";
                        }
                    }
                }
            }
            if (addtick) //if a tick then add a tick
            {
                for (int c = 0; c < maxCriteria; c++)
                {
                    for (int t = 0; t < maxGradeGroups; t++)
                    {
                        if (GChecked[c, t])
                        {
                            if (!onlyTick)
                            {
                                CG[c, t] = CG[c, t] + nl + "===" + tick + "===";
                            }
                            else
                            {
                                CG[c, t] = nl + "%HStart% ===" + tick + "=== %HEnd%";
                            }
                        }
                        else
                        {
                            if (onlyTick)
                            {
                                CG[c, t] = "";
                            }
                        }
                    }
                }
            } 
            

            ReplaceString("%CG1%", CG[0, 0]);   //criteria and grade
            ReplaceString("%CG2%", CG[0, 1]);
            ReplaceString("%CG3%", CG[0, 2]);
            ReplaceString("%CG4%", CG[0, 3]);
            ReplaceString("%CG5%", CG[0, 4]);
            ReplaceString("%CG6%", CG[0, 5]);
            ReplaceString("%CG7%", CG[0, 6]);
            ReplaceString("%CG8%", CG[0, 7]);
            ReplaceString("%CG9%", CG[0, 8]);
            ReplaceString("%CG10%", CG[0, 9]);
            ReplaceString("%CG11%", CG[0, 10]);
            ReplaceString("%CG12%", CG[0, 11]);
            ReplaceString("%CG13%", CG[0, 12]);
            ReplaceString("%CG14%", CG[0, 13]);

            ReplaceString("%CG21%", CG[1, 0]);
            ReplaceString("%CG22%", CG[1, 1]);
            ReplaceString("%CG23%", CG[1, 2]);
            ReplaceString("%CG24%", CG[1, 3]);
            ReplaceString("%CG25%", CG[1, 4]);
            ReplaceString("%CG26%", CG[1, 5]);
            ReplaceString("%CG27%", CG[1, 6]);
            ReplaceString("%CG28%", CG[1, 7]);
            ReplaceString("%CG29%", CG[1, 8]);
            ReplaceString("%CG30%", CG[1, 9]);
            ReplaceString("%CG31%", CG[1, 10]);
            ReplaceString("%CG32%", CG[1, 11]);
            ReplaceString("%CG33%", CG[1, 12]);
            ReplaceString("%CG34%", CG[1, 13]);
            

            ReplaceString("%CG41%", CG[2, 0]);
            ReplaceString("%CG42%", CG[2, 1]);
            ReplaceString("%CG43%", CG[2, 2]);
            ReplaceString("%CG44%", CG[2, 3]);
            ReplaceString("%CG45%", CG[2, 4]);
            ReplaceString("%CG46%", CG[2, 5]);
            ReplaceString("%CG47%", CG[2, 6]);
            ReplaceString("%CG48%", CG[2, 7]);
            ReplaceString("%CG49%", CG[2, 8]);
            ReplaceString("%CG50%", CG[2, 9]);
            ReplaceString("%CG51%", CG[2, 10]);
            ReplaceString("%CG52%", CG[2, 11]);
            ReplaceString("%CG53%", CG[2, 12]);
            ReplaceString("%CG54%", CG[2, 13]);

            ReplaceString("%CG61%", CG[3, 0]);
            ReplaceString("%CG62%", CG[3, 1]);
            ReplaceString("%CG63%", CG[3, 2]);
            ReplaceString("%CG64%", CG[3, 3]);
            ReplaceString("%CG65%", CG[3, 4]);
            ReplaceString("%CG66%", CG[3, 5]);
            ReplaceString("%CG67%", CG[3, 6]);
            ReplaceString("%CG68%", CG[3, 7]);
            ReplaceString("%CG69%", CG[3, 8]);
            ReplaceString("%CG70%", CG[3, 9]);
            ReplaceString("%CG71%", CG[3, 10]);
            ReplaceString("%CG72%", CG[3, 11]);
            ReplaceString("%CG73%", CG[3, 12]);
            ReplaceString("%CG74%", CG[3, 13]);

            ReplaceString("%CG81%", CG[4, 0]);
            ReplaceString("%CG82%", CG[4, 1]);
            ReplaceString("%CG83%", CG[4, 2]);
            ReplaceString("%CG84%", CG[4, 3]);
            ReplaceString("%CG85%", CG[4, 4]);
            ReplaceString("%CG86%", CG[4, 5]);
            ReplaceString("%CG87%", CG[4, 6]);
            ReplaceString("%CG88%", CG[4, 7]);
            ReplaceString("%CG89%", CG[4, 8]);
            ReplaceString("%CG90%", CG[4, 9]);
            ReplaceString("%CG91%", CG[4, 10]);
            ReplaceString("%CG92%", CG[4, 11]);
            ReplaceString("%CG93%", CG[4, 12]);
            ReplaceString("%CG94%", CG[4, 13]);

            ReplaceString("%CG101%", CG[5, 0]);
            ReplaceString("%CG102%", CG[5, 1]);
            ReplaceString("%CG103%", CG[5, 2]);
            ReplaceString("%CG104%", CG[5, 3]);
            ReplaceString("%CG105%", CG[5, 4]);
            ReplaceString("%CG106%", CG[5, 5]);
            ReplaceString("%CG107%", CG[5, 6]);
            ReplaceString("%CG108%", CG[5, 7]);
            ReplaceString("%CG109%", CG[5, 8]);
            ReplaceString("%CG110%", CG[5, 9]);
            ReplaceString("%CG111%", CG[5, 10]);
            ReplaceString("%CG112%", CG[5, 11]);
            ReplaceString("%CG113%", CG[5, 12]);
            ReplaceString("%CG114%", CG[5, 13]);

            ReplaceString("%CG121%", CG[6, 0]);
            ReplaceString("%CG122%", CG[6, 1]);
            ReplaceString("%CG123%", CG[6, 2]);
            ReplaceString("%CG124%", CG[6, 3]);
            ReplaceString("%CG125%", CG[6, 4]);
            ReplaceString("%CG126%", CG[6, 5]);
            ReplaceString("%CG127%", CG[6, 6]);
            ReplaceString("%CG128%", CG[6, 7]);
            ReplaceString("%CG129%", CG[6, 8]);
            ReplaceString("%CG130%", CG[6, 9]);
            ReplaceString("%CG131%", CG[6, 10]);
            ReplaceString("%CG132%", CG[6, 11]);
            ReplaceString("%CG133%", CG[6, 12]);
            ReplaceString("%CG134%", CG[6, 13]);

            ReplaceString("%CG141%", CG[7, 0]);
            ReplaceString("%CG142%", CG[7, 1]);
            ReplaceString("%CG143%", CG[7, 2]);
            ReplaceString("%CG144%", CG[7, 3]);
            ReplaceString("%CG145%", CG[7, 4]);
            ReplaceString("%CG146%", CG[7, 5]);
            ReplaceString("%CG147%", CG[7, 6]);
            ReplaceString("%CG148%", CG[7, 7]);
            ReplaceString("%CG149%", CG[7, 8]);
            ReplaceString("%CG150%", CG[7, 9]);
            ReplaceString("%CG151%", CG[7, 10]);
            ReplaceString("%CG152%", CG[7, 11]);
            ReplaceString("%CG153%", CG[7, 12]);
            ReplaceString("%CG154%", CG[7, 13]);
            
            ReplaceString("%CrTitle1%", CT[0]);
            ReplaceString("%CrTitle2%", CT[1]);
            ReplaceString("%CrTitle3%", CT[2]);
            ReplaceString("%CrTitle4%", CT[3]);
            ReplaceString("%CrTitle5%", CT[4]);
            ReplaceString("%CrTitle6%", CT[5]);
            ReplaceString("%CrTitle7%", CT[6]);
            ReplaceString("%CrTitle8%", CT[7]);
            ReplaceString("%CrTitle9%", CT[8]);
            ReplaceString("%CrTitle10%", CT[9]);

            ReplaceString("%CrTitle1%", CT[0]);
            ReplaceString("%CrTitle2%", CT[1]);
            ReplaceString("%CrTitle3%", CT[2]);
            ReplaceString("%CrTitle4%", CT[3]);
            ReplaceString("%CrTitle5%", CT[4]);
            ReplaceString("%CrTitle6%", CT[5]);
            ReplaceString("%CrTitle7%", CT[6]);
            ReplaceString("%CrTitle8%", CT[7]);
            ReplaceString("%CrTitle9%", CT[8]);
            ReplaceString("%CrTitle10%", CT[9]);

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
            //MessageBox.Show("Rich Text length = ", Convert.ToString(richTextBox1.Text.Length) + " and TextLength = " + Convert.ToString(richTextBox1.TextLength));
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
            ReplaceString("%marker%", Marker);
            ReplaceString("%date%", DateTime.Now.ToString("dd/MM/yy"));


            richTextBox1.ReadOnly = true;
        }
       
        private void savebutton_Click(object sender, EventArgs e)
        {
            string fname = "";
            saveFileDialog1.InitialDirectory = OutFilePath;

            saveFileDialog1.FileName = student+ "_" + AssessNo + "T" + SittingType+ ".rtf"; //t for table
            saveFileDialog1.DefaultExt = ".rtf";          
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Save_Report(saveFileDialog1.FileName);
            changesSaved = true;
            richTextBox1.ReadOnly = true;
            //cancelbutton.Visible = false;
            //editbutton.Visible = true;
            richTextBox1.BackColor = c1;
            printbutton.Visible = true;
        }
        private void Save_Report(string filename)
        {
            string filetmp = filename + ".tmp";
            try
            {
                //richTextBox1.Text = richTextBox1.Text + @"{ \landscape }";
                richTextBox1.SaveFile(filetmp, RichTextBoxStreamType.RichText);
                formatOrientation(filename, filetmp);
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
            printFilePath = filename;
        }

        private void formatOrientation(string fname, string ftemp)
        {
            string str = "";
            try
            {
                using (StreamReader rw = new StreamReader(ftemp))
                {
                    using (StreamWriter nw = new StreamWriter(fname))
                    {
                        string Start = "";
                        string End = "";
                        if (highlight && !bold)
                        {
                            Start = highS;
                            End = highE;
                        }
                        else if (highlight && bold)
                        {
                            Start = highS + boldS;
                            End = boldE + highE;
                        }
                        else if (!highlight && bold)
                        {
                            Start = boldS;
                            End = boldE;
                        }
                        else //no highlight or bold
                        {

                        }
                        while (!rw.EndOfStream)
                        {
                            str = rw.ReadLine();
                            if (str.StartsWith(@"{\rtf1"))
                            {
                                nw.WriteLine(str);
                                nw.WriteLine(@"\paperw16838\paperh11906\margl1000\margr1440\margt1440\margb1440\gutter0\ltrsect");
                            }
                            else if (str.Contains("%HStart%"))
                            {
                                str = str.Replace("%HStart%", Start);
                                //str = str.Replace("%BStart%", "\\highlight7 ");
                                if (str.Contains("%HEnd%"))
                                {
                                    str = str.Replace("%HEnd%", End);
                                    //str = str.Replace("%BEnd%", "\\highlight0 ");
                                }
                                nw.WriteLine(str);
                            }
                            else if (str.Contains("%HEnd%"))
                            {
                                str = str.Replace("%HEnd%", End);
                                //str = str.Replace("%BEnd%", "\\highlight0 ");
                                nw.WriteLine(str);
                            }
                            else
                            {
                                nw.WriteLine(str);
                            }
                        }
                        nw.Close();
                    }
                    rw.Close();
                }
                File.Delete(ftemp);
            }
            catch
            {
                MessageBox.Show("Problem saving file");
            }
        }
        private void editbutton_Click(object sender, EventArgs e)
        {
            c1 = richTextBox1.BackColor;
            richTextBox1.BackColor = Color.Beige;
            richTextBox1.ReadOnly = false;
            //cancelbutton.Visible = true;
            //editbutton.Visible = false;
        }

        private void cancelbutton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Cancel edit (all changes will be lost) Yes/No?", "Cancel Edit Report", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //cancelbutton.Visible = false;
                //editbutton.Visible = true;
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
