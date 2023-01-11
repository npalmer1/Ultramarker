using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Drawing.Printing;

namespace UltraMarker
{
    public partial class PeerForm : Form
    {
        public int PeerReview;

        public string TemplateFile;
        public string OutputFile;
        public string Institution = "";
        public string UnitTitle = "";
        public string UnitCode;
        public string Level;
        public string UnitLeader;
        public string Peer;
        
        public string AssessNo;
        public string AssessTitle;
        public int Weight;
        public int PassMark;
        public string Aggregation;
        public bool Sheet;
        public string Comment1;
        public bool Strategy;
        public string Comment2;
        public bool Instructions;
        public string Comment3;
        public bool Criteria;
        public string Comment4;
        public bool Task;
        public string Comment5;
        public bool Special;
        public string Comment6;
        public string PeerDate;
        public string Comment7;
        public string ULDate;

        public string Moderator;
        public bool Agreed;
        public bool Third;
        public string MarkDir;
        public int MarkType;
        public string ExternalMod;
        public string okstring;
        public string OutFilePath;
        public string ULSigFilePath;
        public string PeerSigFilePath;
        int ftype = 0;
        Image ULSigImg;
        Image PeerSigImg;

        
        struct st_struct
        {
            public string firstname;
            public string surname;
            public string percent;
            public string grade;
        }
        List<st_struct> ModList = new List<st_struct>();

        Color c1;
        bool changesSaved = false;

        private Font printFont;
        private StreamReader streamToPrint;
        string printFilePath;
        string lineToPrint = ""; //lines for the printer
        string[] textlines; //takes lines of text from richtextbox
        int linestoGo = 0; //number of lines in richtextbox remaining for printing

        public PeerForm()
        {
            InitializeComponent();
        }

        private void Peer_Load(object sender, EventArgs e)
        {
            SetSig();
            changesSaved = false;
            LoadTemplate(TemplateFile);
            if (PeerReview ==0)
            {
                this.Text = "Peer Review Form";
                ftype = 0;
                ModifyPeerForm();
                
            }
            else if (PeerReview == 1)
            {
                this.Text = "Moderation Record Form";
                ftype = 1;
                ModifyModForm();
            }
            else 
            {
                this.Text = "External examiner moderation";
                ftype = 2;
                ModifyExtForm();
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
            catch{
                }           
        }

        private void SetSig()
        {
            try
            {
                if (File.Exists(ULSigFilePath))
                {
                    ULSigImg = Image.FromFile(ULSigFilePath);
                }
                else
                {
                    ULSigImg = null;
                }
                if (File.Exists(PeerSigFilePath))
                {
                    PeerSigImg = Image.FromFile(PeerSigFilePath);
                }
                else
                {
                    PeerSigImg = null;
                }
            }
            catch
            {
            }
           
        }

        private void InsertSig(Image img, string atStr)
        {
            try
            {
                //Clipboard clip;
                if (atStr == null)
                {
                    atStr = "";
                }
                int i = richTextBox1.Find(atStr);
                if (i > -1)
                {
                    richTextBox1.Select(i, atStr.Length);

                    //richTextBox1.Cut();
                    Clipboard.Clear();
                    if (atStr.Length > 0)
                    {
                        Clipboard.SetImage(img);
                    }
                    else { Clipboard.SetText(" "); }
                    richTextBox1.Paste();
                }
            }
            catch
            {
            }    
        }

        private void ModifyPeerForm()
        {
            string padstring = " ".PadRight(100); //pad file to fix bug (diff between text and rich text length)
            richTextBox1.AppendText(padstring);
            ReplaceString("%Institution%", Institution);
            ReplaceString("%UnitTitle%", UnitTitle);
            ReplaceString("%UnitCode%", UnitCode);
            ReplaceString("%Level%", Level);
            ReplaceString("%UnitLeader%", UnitLeader);
            ReplaceString("%Peer%", Peer);
            ReplaceString("%AssessNo%", AssessNo);
            ReplaceString("%AssessTitle%", AssessTitle);
            try
            {
                ReplaceString("%Weight%", Weight.ToString());
            }
            catch { }
            try
            {
                ReplaceString("%PassMark%", PassMark.ToString());
            }
            catch { }
            ReplaceString("Must Pass/Aggregated", Aggregation);
            if (Sheet)
            {
                ReplaceString("%Sheet%", "Y");
            }
            else
            {
                ReplaceString("%Sheet%", " ");
            }
            ReplaceString("%Comment1%", Comment1);
            if (Strategy)
            {
                ReplaceString("%Strategy%", "Y");
            }
            else
            {
                ReplaceString("%Strategy%", " ");
            }
            ReplaceString("%Comment2%", Comment2);
            if (Instructions)
            {
                ReplaceString("%Instructions%", "Y");
            }
            else
            {
                ReplaceString("%Instructions%", " ");
            }
            ReplaceString("%Comment3%", Comment3);
            if (Criteria)
            {
                ReplaceString("%Criteria%", "Y");
            }
            else
            {
                ReplaceString("%Criteria%", " ");
            }
            ReplaceString("%Comment4%", Comment4);
            if (Task)
            {
                ReplaceString("%Task%", "Y");
            }
            else
            {
                ReplaceString("%Task%", " ");
            }
            ReplaceString("%Comment5%", Comment5);
            if (Special)
            {
                ReplaceString("%Special%", "Y");
            }
            else
            {
                ReplaceString("%Special%", " ");
            }
            ReplaceString("%Comment6%", Comment6);
            //ReplaceString("%Peer%", Peer);
            if (PeerSigImg != null)
            {
                InsertSig(PeerSigImg, "%PeerSig%");
            }
            else
            {
                ReplaceString("%PeerSig%", "");
            }
            ReplaceString("%PeerDate%", PeerDate);
            ReplaceString("%Comment7%", Comment7);
            //ReplaceString("%UnitLeader%", UnitLeader);
            if (ULSigImg != null)
            {
                InsertSig(ULSigImg, "%ULSig%");
            }
            else
            {
                ReplaceString("%ULSig%", "");
            }
            ReplaceString("%ULDate%", ULDate);

        }

        private void ModifyModForm()
        {
            ReplaceString("%Institution%", Institution);
            ReplaceString("%UnitTitle%", UnitTitle);
            ReplaceString("%UnitCode%", UnitCode);
            ReplaceString("%Level%", Level);
            ReplaceString("%UnitLeader%", UnitLeader);
            ReplaceString("%Moderator%", Moderator);
            ReplaceString("%AssessNo%", AssessNo);
            ReplaceString("%AssessTitle%", AssessTitle);
            try
            {
                ReplaceString("%Weight%", Weight.ToString());
            }
            catch { }
            try
            {
                ReplaceString("%PassMark%", PassMark.ToString());
            }
            catch { }
            ReplaceString("Must Pass/Aggregated", Aggregation);

            ReplaceString("%Comment1%", Comment1);
            ReplaceString("%Comment2%", Comment2);
            ReplaceString("%Comment3%", Comment3);
            if (Agreed)
            {
                ReplaceString("%Agreed%", "Y");
            }
            else
            {
                ReplaceString("%Agreed%", " ");
            }
            ReplaceString("%Moderator%", Moderator);
            ReplaceString("%UnitLeader%", UnitLeader);
            if (Third)
            {
                ReplaceString("%Third%", "Y");
            }
            else
            {
                ReplaceString("%Third%", "N");
            }
            //BuildModerationList();
            BuildModerationList_New();
        }


        private void ModifyExtForm()
        {
            ReplaceString("%Institution%", Institution);
            ReplaceString("%UnitTitle%", UnitTitle);
            ReplaceString("%UnitCode%", UnitCode);
            ReplaceString("%ExternalModerator%", ExternalMod);
            ReplaceString("%AssessTitle%", AssessTitle);
            ReplaceString("%OK%", okstring);
            
            ReplaceString("%Comment1%", Comment1);
            ReplaceString("%ExternalModerator%", ExternalMod);
            ReplaceString("%Date%", ULDate);
           
        }


        private void BuildModerationList_New()
        {
            string str = "";
            string str3 = "";

            st_struct sts;
            sts.firstname = "";
            sts.surname = "";
            sts.percent = "";
            sts.grade = "";
            float group_total = 0;
            string[] name = new string[3];
            bool Moderated = false;
            if (!Directory.Exists(MarkDir))
            {
                MessageBox.Show("Folder with moderated work doesn't exist");
                return;
            }
            try
            {
                string[] files = Directory.GetFiles(MarkDir, "*.mrk");
                if (files.Count() < 1)
                {
                    MessageBox.Show("No moderated files (.mrm) in this folder");
                    return;
                }

                foreach (string file in files)
                {
                    Moderated = false;
                    sts.firstname = "";
                    sts.surname = "";
                    sts.percent = "";
                    sts.grade = "";
                    if (file.Contains(".mrk"))  //moderated marks
                    {
                        using (StreamReader sw = new StreamReader(file))
                        {
                            while (!sw.EndOfStream)
                            {
                                str = sw.ReadLine();
                                str.Trim();
                                if (str.Length > 0)
                                {
                                    if (str.Contains(":"))
                                    {
                                        int a = str.IndexOf(":");
                                        str3 = str.Substring(a + 1).Trim();

                                    }
                                    else
                                    {
                                        str3 = str;
                                    }
                                    if (str.Contains("Moderated:") && str.Contains("Y"))
                                    {
                                        Moderated = true;
                                    }
                                    if (str.Contains("Name:"))
                                    {
                                        name = str.Split(':');
                                        if (name[1] !=null)
                                        {
                                            sts.firstname = name[1];
                                        }
                                        /*name = str3.Split(' ');                                        
                                        try
                                        {
                                            if (name[2] != null)
                                            {
                                                sts.surname = name[2];
                                            }
                                        }
                                        catch
                                        {
                                        }
                                        try
                                        {
                                            if (name[1] != null)
                                            {
                                                sts.surname = name[1];
                                            }
                                        }
                                        catch
                                        {
                                        }
                                        try
                                        {
                                            if (name[0] != null)
                                            {
                                                sts.firstname = name[0];
                                            }
                                        }
                                        catch
                                        {
                                        }
                                        */

                                    }
                                    else if (str.Contains("Overall mark:") && ((MarkType == 0) || (MarkType == 1)))
                                    {
                                        sts.percent = str3;
                                        try
                                        {
                                            group_total = group_total + Convert.ToSingle(str3);
                                        }
                                        catch
                                        {
                                        }

                                    }
                                    else if (str.Contains("Equivalent grade:") && ((MarkType == 0) || (MarkType == 2)))
                                    {
                                        sts.grade = str3;
                                    }

                                }

                            }
                            if (Moderated)
                            {
                                ModList.Add(sts);
                            }
                            sw.Close();
                        }

                    }

                } //for each

                int i = richTextBox1.Find("%ModList%");
                if (i > -1)
                {
                    richTextBox1.Select(i, "%ModLIst%".Length);

                    Clipboard.Clear();
                    string str4 = "";
                    foreach (st_struct st in ModList)
                    {
                        str4 = str4 + st.surname + ", " + (st.firstname).PadRight(39, '-') + " \t" + st.percent + " \t" + st.grade + System.Environment.NewLine; ;
                    }
                    if (str4.Length > 0)
                    {
                        Clipboard.SetText(str4);
                    }
                    else { Clipboard.SetText(" "); }
                    richTextBox1.Paste();
                }
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }
        private void Closebutton_Click(object sender, EventArgs e)
        {
            if (!changesSaved)
            {
                DialogResult dialogResult = MessageBox.Show("Save report first ? Yes/No?", "Exit Report", MessageBoxButtons.YesNo);
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


        private void BuildModerationList()
        {
            string str = "";
            string str3 = "";

            st_struct sts;
            sts.firstname = "";
            sts.surname = "";
            sts.percent = "";
            sts.grade = "";
            float group_total = 0;
            string[] name = new string[3];
            if (!Directory.Exists(MarkDir))
            {
                MessageBox.Show("Folder with moderated work doesn't exist");
                return;
            }
            try
            {
                    string[] files = Directory.GetFiles(MarkDir, "*.mrm");
                    if (files.Count() < 1)
                    {
                        MessageBox.Show("No moderated files (.mrm) in this folder");
                        return;
                    }
                    foreach (string file in files)
                    {
                        sts.firstname = "";
                        sts.surname = "";
                        sts.percent = "";
                        sts.grade = "";
                        if (file.Contains(".mrm"))  //moderated marks
                        {
                            using (StreamReader sw = new StreamReader(file))
                            {
                                while (!sw.EndOfStream)
                                {
                                    str = sw.ReadLine();
                                    str.Trim();
                                    if (str.Length > 0)
                                    {
                                        if (str.Contains(":"))
                                        {
                                            int a = str.IndexOf(":");
                                            str3 = str.Substring(a + 1).Trim();

                                        }
                                        else
                                        {
                                            str3 = str;
                                        }
                                        if (str.Contains("Student:"))
                                        {
                                            name = str3.Split(' ');
                                            try
                                            {
                                                if (name[2] != null)
                                                {
                                                    sts.surname = name[2];
                                                }
                                            }
                                            catch
                                            {
                                            }
                                            try
                                            {
                                                if (name[1] != null)
                                                {
                                                    sts.surname = name[1];
                                                }
                                            }
                                            catch
                                            {
                                            }
                                            try
                                            {
                                                if (name[0] != null)
                                                {
                                                    sts.firstname = name[0];
                                                }
                                            }
                                            catch
                                            {
                                            }

                                        }
                                        else if (str.Contains("Overall mark:") && ((MarkType == 0) || (MarkType == 1)))
                                        {
                                            sts.percent = str3;
                                            try
                                            {
                                                group_total = group_total + Convert.ToSingle(str3);
                                            }
                                            catch
                                            {
                                            }

                                        }
                                        else if (str.Contains("Equivalent grade:") && ((MarkType ==0) || (MarkType == 2)))
                                        {
                                            sts.grade = str3;
                                        }

                                    }

                                }
                                ModList.Add(sts);
                                sw.Close();
                            }

                        }

                    } //for each
                   
                    int i = richTextBox1.Find("%ModList%");
                    if (i > -1)
                    {
                        richTextBox1.Select(i, "%ModLIst%".Length);
                        
                        Clipboard.Clear();
                        string str4 = "";
                        foreach (st_struct st in ModList)
                        {
                            str4 = str4 + st.surname + ", " + (st.firstname).PadRight(39, '-') + " \t" + st.percent + " \t" + st.grade + System.Environment.NewLine;;
                        }
                        if (str4.Length > 0)
                        {
                            Clipboard.SetText(str4);
                        }
                        else { Clipboard.SetText(" "); }
                        richTextBox1.Paste();
                    }                       
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            string fname = "";
            saveFileDialog1.InitialDirectory = OutFilePath;
            if (ftype == 0)
            {
                fname = "_peer";
            }
            else if (ftype == 1)
            {
                fname = "_mod";
            }
            else if (ftype ==2)
            {
                fname = "_ext";
            }
          
            saveFileDialog1.FileName = AssessNo + fname + ".rtf";
            saveFileDialog1.DefaultExt = ".rtf";
            //saveFileDialog1.FileName = textBox1.Text.Trim();
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            
            Save_Report(saveFileDialog1.FileName);
            changesSaved = true;
            richTextBox1.ReadOnly = true;
            Cancelbutton.Visible = false;
            Editbutton.Visible = true;
            richTextBox1.BackColor = c1;
            Printbutton.Visible = true;
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

        private void Editbutton_Click(object sender, EventArgs e)
        {
            c1 = richTextBox1.BackColor;
            richTextBox1.BackColor = Color.Beige;
            richTextBox1.ReadOnly = false;
            Cancelbutton.Visible = true;
            Editbutton.Visible = false;
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Cancel edit (all changes will be lost) Yes/No?", "Cancel Edit Report", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Cancelbutton.Visible = false;
                Editbutton.Visible = true;
                richTextBox1.ReadOnly = true;
                richTextBox1.BackColor = c1;
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

        private void Printbutton_Click(object sender, EventArgs e)
        {
            Printing_From_RichTextBox();
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

       

    }
}
