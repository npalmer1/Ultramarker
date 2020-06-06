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
using System.Net;
using System.Text.RegularExpressions;

//using Microsoft.Office.Core;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Graph = Microsoft.Office.Interop.Graph;
using System.Runtime.InteropServices;

namespace UltraMarker
{
    public partial class Form1 : Form
    {
        const bool sub = true;
        const bool cri = false;
        bool firstS = true;
        bool addCode = true; //adds asesment code to end of filename when saving marks

        string DefaultDir = "";
        string ConfigDir = "";
        String theVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();


        //List<Label> myGradeList = new List<Label>();
        int X = 0;
        int AGrades = 0;
        int S = 0;
        //int B = 0;
        int CritZ = 0; //number of criteria in this assessment
        int[] SubZ = new int[2];
        int Found = 0;

        string UnitFile = "";
        string UnitFilePath = "C:";
        string GradeFile = "";
        string GradePath = "";
        string CriteriaFile = "";
        string CriteriaPath = "";
        string LOFile = "";
        string LOFilePath = "";
        string SessionPath = "";
        string SessionFile = "";
        string CommentFile = "";
        string CommentFilePath = "";
        string Institution = "";
        string StudentImportFile = "";
        string ULSigFilePath = "";
        string PeerSigFilePath = "";

        string marksDirectory = "";

        string Feedback = "";
        int CriteriaType = 0;
        int Summary_Sort_Type = 0; //sort sumamry of marks by firstname
        string Summary_percentgrade = "11";
        bool promptsoff = false; //whether to confirm yes/no ech time saving criteria

        bool tempflag = false;


        //gradeimages if using images with the grades (eg. smileys)
        bool Smileys = false;

        bool CriteriaSelected = false;
        bool GradeTitleChanged = false;
        bool SessionTitleChanged = false;
        int SCriteria = 0;
        int SSub = 0;
        const int MaxCriteria = 50;
        static int MaxSub = 30;
        //const int MaxGrades = 25;
        const int MaxGrades = 50;
        const int MaxSessions = 20;
        const int MaxGradeGroups = 10; //nunber of groups that grades can be assigned to - for criteria design
        int Session = 0;
        int SessionS = 0;
        int SessionType = 0; //no sessions
        bool AddSession = false;
        bool allowoverride = true;

        bool TChanged = false;
        //bool EditMode = false;
        bool EditStudent = false;
        bool EditCriteria = false; //if criteria being edited
        bool EditGrades = false;
        bool singleGrades = true;
        bool loading = true;
        bool change1 = false;

        struct aliases
        {
            public string alias;
            public float percent;
            public int grindex;

        }
        List<aliases> aliaslist = new List<aliases>();

        int PrevIndex = 0;
        bool[,] CriteriaExists = new bool[MaxCriteria, MaxSub + 1];
        bool insertingCriteria = false;
        bool yesno = false;
        bool UseChecked = false;
        bool firstcount = false;
        bool replicate_Criteria = false;
        bool replicate_Feedback = false;
        bool replicate_Description = false;
        bool replicate_LO = false;

        string slash = "\\";
        int MarkMode = 0;

        string Sitting = "Main";


        TreeNode SelNode;
        TreeNode PreNode;
        int SelSub = 0;
        int SelCr = 0;

        bool LOs = false;  //show learning outcomes?

        string[] fbstring = new string[MaxGrades];

        reportForm rep1 = new reportForm(); //form to output student report to

        assessForm AForm = new assessForm();    //assessment title and description form

        GradeGroup GForm = new GradeGroup(); //form for output to a template criteria form

        CommentForm CommentsForm = new CommentForm();
        addForm addcommentForm = new addForm();
        addForm2 addcommentForm2 = new addForm2();

        addComment addcommentX = new addComment();

        WeightReport weightForm = new WeightReport();

        int[] Listboxlist = new int[MaxGrades]; //list of selected indices for listbox1
        int CurrentlySelected = -1;

        //string StudentName = "";

        private string backvalue;

        public string BackedValu
        {
            get { return backvalue; }
            set { backvalue = value; }
        }

        struct GradesStruct
        {
            public string grtitle;
            public string grfb;
            public float grpercent;
            public float grlower;
            public float grupper;
            public bool grselected;
            public string gralias;
            public int grGroup;
        }
        GradesStruct[] gradelist = new GradesStruct[MaxGrades];

       struct GradeGroupStruct
        {
            public int GroupNo;
            public string GroupText;
        }
        GradeGroupStruct[] gradegrouplist = new GradeGroupStruct[MaxGradeGroups];

        string[, ,] Marks = new string[MaxCriteria, MaxSub + 1, MaxSessions];
        float[,] Percents = new float[MaxCriteria, MaxSub + 1];


        //Note: use maxsub as the entry used by criteria and others used by sub-criteria:
        string[,] crtitle = new string[MaxCriteria, MaxSub + 1];

        string[,] crdesc = new string[MaxCriteria, MaxSub + 1];
        string[,] crLO = new string[MaxCriteria, MaxSub + 1];  //learning outcome

        //int[, ,] crweight = new int[MaxCriteria, MaxSub + 1, MaxSessions];
        float[,,] crweight = new float[MaxCriteria, MaxSub + 1, MaxSessions];
        bool[,] crsub = new bool[MaxCriteria, MaxSub + 1];
        //int[,] crpos = new int[MaxCriteria,2];
        string[, ,] grcrtitle = new string[MaxCriteria, MaxSub + 1, MaxGrades]; //grade title
        string[, ,] grcr = new string[MaxCriteria, MaxSub + 1, MaxGrades]; //grade criteria description (maxsub for criteria, and anything less = sub-criteria)
        string[, ,] grcrfb = new string[MaxCriteria, MaxSub + 1, MaxGrades]; //grade criteria feedback

        string[, ,] crComment = new string[MaxCriteria, MaxSub + 1, MaxSessions]; //comments for each criteria
        bool[, ,] crSelected = new bool[MaxCriteria, MaxSub + 1, MaxSessions];  //selects which criteria is used for each session
        bool[, ,] treeSelected = new bool[MaxCriteria, MaxSub + 1, MaxSessions];  //selects which criteria is used for each session in treeview2

        bool[, ,] nodeChecked = new bool[MaxCriteria, MaxSub + 1, MaxSessions];
        bool NChecked = false;
        bool Aggregation = false;

        double fineWeight = 1; //determines whether criteria weighting can used fine (1%) or coarse (5%) weighting in combobox

        string[] SessionTitle = new string[MaxSessions];
        string[] SessionDesc = new string[MaxSessions];
        int[] SessionWeight = new int[MaxSessions];
        bool SessionsEqual = false;
        bool[] DeselectSession = new bool[MaxSessions];


        struct assessmentStruct
        {
            public string Description;
            public string Title;
            public string Code;
            public string Weight;
            public string LOs;
        }
        assessmentStruct assess;

        Color assesstitleBackcolor;

        struct feedbackoptionstruct
        {
            public bool generic;
            public bool description;
            public bool suggested;
            public bool additional;
            public bool criteriaComment;
            public bool percent;
            public bool grade;
            public bool LO;
            public bool full;
            public bool fullLO;
            public bool SubLO;
            public bool CriteriaGrade;
            public bool subdescription;
            public bool includeheader;
            public bool CriteriaPercent;
        }
        feedbackoptionstruct feedOptions;
       

        struct Student
        {
            public string studentname;
            public string studentmail;
        }
        List<Student> studentlist = new List<Student>();

        //Learning outcomes:
        public List<string> LODesc = new List<string>();
        public List<string> LOTitle = new List<string>();
        public List<string> LOType = new List<string>();
        public int LOSelected;
        public string SelectedLO;
        public string LOList;
        //public string LOFilePath;
        bool addLOmode = false;
        bool insertLO = false;
        int oldindex;
        bool savedLO = false;
        bool selectLOs = false;

        int peerfiletype = 0;
        string peerFile = "";
        string modFile = "";
        string extFile = "";
        //bool showpeer = false;
        string modDirectory = "";

        bool savedStudent = false;
        string prevStudent = "";
        bool startMark = false;
        int CriteriaSelectionType = 0;
        string templateGenfile = "";
        bool AllowImpComment = false;
        bool CalculateImportbyLines = true;
        

        Font TVFont = new Font("Microsoft Sans Serif", 9.75f);

        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {            
            string str = "";
            feedOptions.includeheader = true;
           
            if (RunningPlatform() == Platform.Windows)
            {
                slash = "\\";
                string startPath = Environment.GetEnvironmentVariable("HOMEDRIVE");
                startPath = startPath + Environment.GetEnvironmentVariable("HOMEPATH") + "\\Ultramarker";
                if (startPath == null)
                {
                    ConfigDir = "C:\\Ultramarker" + slash;
                    DefaultDir = startPath;
                    setDefaultDir("Set initial default directory: ");
                    if (DefaultDir == "" || DefaultDir == null)
                    {
                        MessageBox.Show("Default directory will be set to C:\\Ultramarker as you did not make a selection");
                        DefaultDir = "C:\\Ultramarker";
                    }                    
                }
                else if (!File.Exists(startPath + slash + "UltraMarker.dir"))
                {
                    DefaultDir = startPath;                    
                    setDefaultDir("Set initial default directory: "); //set the default directory on initial startup
                    if (DefaultDir == "" || DefaultDir == null)
                    {
                        MessageBox.Show("Default directory will be set to C:\\Ultramarker as you did not make a selection");
                        DefaultDir = "C:\\Ultramarker";
                    }
                    ConfigDir = startPath + slash;                   
                }
                else //if ultramarker.dir file exists
                {
                    ConfigDir = startPath + slash;
                    DefaultDir = startPath;  //this will be overridden if present in ultramarker.dir file in config dir
                }
                if (!Directory.Exists(DefaultDir) && (DefaultDir.Trim() != "") && (DefaultDir != null))
                {
                    try
                    {
                        Directory.CreateDirectory(DefaultDir);
                    }
                    catch { }
                }
                if (!(DefaultDir.LastIndexOf("\\") == DefaultDir.Length -1 ))
                {
                    DefaultDir = DefaultDir + slash; //add a slash to end of directory name if it doesn't have one!
                }                
                if (!Directory.Exists(DefaultDir))
                {
                    MessageBox.Show("Problem creating default directory");
                }
            }
            else if (RunningPlatform() == Platform.Linux)
            {
                slash = "/";
                var homePath = Environment.GetEnvironmentVariable("HOME");
                DefaultDir = Path.Combine(homePath, "Ultramarker");
                Directory.CreateDirectory(DefaultDir);
                DefaultDir = DefaultDir + slash;
                ConfigDir = DefaultDir + slash;
            }
            defaultdirlabel.Text = "Default directory currently set to: " + DefaultDir;
            configdirlabel.Text = "Configuration path/file : " + ConfigDir + "Ultramarker.dir";
            this.Text = "UltraMarker                   " + theVersion + "                      GNU GPL v3 project managed by N. Palmer 2020                    (F1 for help)";
            tabControl1.TabPages.Remove(tabPage3); //don't show sessions tab initially
            tabControl1.TabPages.Remove(tabPage10); //don't who web connection page as it's a prototype test
            label23.Text = "";
            LOtextBox.Text = "";
            button7.Text = "Start marking student";
            //treeView1.Font.Style = FontStyle.Regular;
            aggregatedlist.SelectedIndex = 0;
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;
            dateTimePicker3.Value = DateTime.Today;
            markradio1.Checked = true;
            setFeedbackOptions(true);
            assesstitleBackcolor = assessTitleBox.BackColor;
            for (int i = 0; i < MaxSessions; i++)
            {
                DeselectSessioncheckBox.Checked = false;
                DeselectSession[i] = false;
            }
            Reset_Selected(true);

            //GroupscheckBox.Checked = true;
           // GroupscheckBox.Checked = false;
            LoadSettings();
           
            if (StudentImportFile.Length > 0)
            {
                StudentImportTextBox.Text = StudentImportFile;
                Import_Students();
            }
            Build_Grade_Lists();

            Set_Weights();

            Build_Session_Weight();
            LoadLOCombo();
            setGradeTree();
            Change_Session_Selection();
            loading = false;
        }

        public enum Platform
        {
            Windows,
            Linux,
            Mac
        }

        public static Platform RunningPlatform()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:                   
                    return Platform.Linux;

                case PlatformID.MacOSX:
                    return Platform.Mac;

                default:
                    return Platform.Windows;
            }
        }

        void LoadLOCombo()
        {
            LOcomboBox.Items.Add("Cognitive Skills");
            LOcomboBox.Items.Add("Practical & Professional Skills");
            LOcomboBox.Items.Add("Knowledge and Understanding");
            LOcomboBox.Items.Add("Employabaility Skills");
            LOcomboBox.Items.Add("Transferable and Key Skills");
            LOcomboBox.Items.Add("Employability Skills");
            LOcomboBox.Items.Add("Soft Skills");
        }

        private void setGradeTree()
        {
            loading = true;
            if (singleGrades)
            {
                graderadioButton1.Checked = true;
                graderadioButton2.Checked = false;                
            }
            else
            {
                graderadioButton2.Checked = true;
                graderadioButton1.Checked = false;
            }
            loading = false;
        }

        private void LoadLOlists()
        {
            if (LOFile != null)
            {
                if (LOFile.Length > 0)
                {
                    ReadLOFromFile(LOFile); //read LOs from file if one exists
                    if (listBox2.Items.Count > 0)
                    {
                        listBox2.SelectedIndex = 0;
                        LOcomboBox.Text = LOType[0];
                    }
                }
                /*listBox3.Items.Clear();
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    listBox3.Items.Add(listBox2.Items[i]);
                }*/
            }

        }

        private void setFeedbackOptions(bool b)
        {
            feedOptions.generic = b;
            feedOptions.description = b;
            feedOptions.suggested = b;
            feedOptions.additional = b;
            feedOptions.criteriaComment = b;
            feedOptions.percent = b;
            feedOptions.grade = b;
            feedOptions.LO = b;
            feedOptions.full = b;
            feedOptions.fullLO = b;
            feedOptions.SubLO = b;
            feedOptions.CriteriaGrade = b;
            feedOptions.subdescription = b;
            feedOptions.includeheader = b;
            feedOptions.CriteriaPercent = b;
        }

        private void Reset_Selected(bool b) //reset selected criteria and sub criteria
        {
            for (int i = 0; i < MaxCriteria; i++)
            {
                for (int j = MaxSub; j > -1; j--)
                {
                    for (int s = 0; s < MaxSessions; s++)
                    {
                        crSelected[i, j, s] = b;
                    }
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            firstcount = false;
        }

        private void ContextMenuItemClick(object sender, EventArgs e)
        {
            string str;
            if (X > MaxGrades - 1)
            {
                MessageBox.Show("Can only have a maximum of " + Convert.ToString(MaxGrades) + " grades");
                return;
            }
            try
            {
                if (contextMenuStrip1.Items[0].Selected)
                {
                    //Add a new grade:   
                    InputForm input = new InputForm();
                    input.browser = false;
                    input.text = "";
                    input.Passvalue = "Enter a title for Grade:";
                    input.ShowDialog();
                    str = input.Passvalue;
                    if (str.Length > 0)
                    {
                        treeView1.Nodes[0].Nodes.Add(str);
                        treeView1.Nodes[0].Expand();
                        treeView1.SelectedNode = treeView1.Nodes[0].LastNode;
                        foreach (TreeNode RootNode in treeView1.Nodes)
                        {
                            RootNode.ContextMenuStrip = contextMenuStrip1;
                            foreach (TreeNode ChildNode in RootNode.Nodes)
                            {
                                ChildNode.ContextMenuStrip = contextMenuStrip2;
                            }
                        }
                        X++;
                        gradelist[X].grtitle = str;
                       
                        //Now populate listbox on next tab:
                        if (singleGrades)
                        {
                            listBox1.Items.Add(str);
                        }
                        textBox1.Text = str;
                        
                        //X = X + 1;

                    }

                }
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void SubItemClick(object sender, EventArgs e)
        {
            string alias = "";
            TreeNode n = new TreeNode();
            textBox1.Text = treeView1.SelectedNode.Text;
            //Edit or delete grades:
            if (contextMenuStrip2.Items[0].Selected) //edit grade
            {
                GradeTitleChanged = false;
                Editable_Grades(true);
            }
            else if (contextMenuStrip2.Items[1].Selected) //delete grade
            {
                try
                {
                    //remove the node (gets selected node from treeview1_NodeMouseClick):
                    DialogResult dialogResult = MessageBox.Show("Delete Yes/No?", "Remove Grade", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        treeView1.SelectedNode = n;
                        //treeView1.SelectedNode.Remove();
                        if (gradelist[AGrades + 1].grtitle != null)
                        {
                            int i = AGrades;
                            while (gradelist[i + 1].grtitle != null)
                            {
                                gradelist[i].grtitle = gradelist[i + 1].grtitle;
                                gradelist[i].grpercent = gradelist[i + 1].grpercent;
                                gradelist[i].grfb = gradelist[i + 1].grfb;
                                gradelist[i].grupper = gradelist[i + 1].grupper;
                                gradelist[i].grlower = gradelist[i + 1].grlower;
                                gradelist[i].grselected = gradelist[i + 1].grselected;
                                alias = gradelist[i].gralias;
                                gradelist[i].gralias = gradelist[i + 1].gralias;
                                i++;
                                if (gradelist[i + 1].grtitle == null)
                                {
                                    gradelist[i].grtitle = null;
                                }
                            }
                        }
                        else
                        {
                            gradelist[AGrades].grtitle = null;
                        }
                        treeView1.SelectedNode.Remove();
                        int pos = listBox1.FindString(alias);
                        if (pos > -1)
                        {
                            listBox1.Items.RemoveAt(pos);
                            listBox1.SelectedIndex = 0;
                            CurrentlySelected = 0;
                        }

                        treeView1.SelectedNode = n;
                        if (!singleGrades)
                        {
                            int p = 0;
                            foreach (aliases a in aliaslist)
                            {
                                if (a.alias == alias)
                                {
                                    aliaslist.RemoveAt(p);
                                }
                            }

                        }
                        Change_Selected_Grade();

                        //treeView1.Nodes[0].Nodes[A].Remove();
                    }
                }
                catch (System.Exception excep)
                {
                    StackTrace stackTrace = new StackTrace();
                    MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
                }
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //need to find which child node is selected in order to remove it ***

            if (treeView1.Nodes[0].Nodes.Count > 0)
            {
                treeView1.SelectedNode = e.Node;
                AGrades = treeView1.SelectedNode.Index;
                Change_Selected_Grade();

            }
        }

        private void Change_Selected_Grade()
        {
            if (AGrades < MaxGrades)
            {
                textBox1.Text = gradelist[AGrades].grtitle;
                textBox2.Text = gradelist[AGrades].grfb;
                percentcombo.Text = Convert.ToString(gradelist[AGrades].grpercent);
                comboBox1.Text = Convert.ToString(gradelist[AGrades].grlower);
                comboBox2.Text = Convert.ToString(gradelist[AGrades].grupper);
                AliastextBox.Text = gradelist[AGrades].gralias;
            }
            else
            {
                MessageBox.Show("Maximum grades " + MaxGrades.ToString() + " limit reached - cannot add grade");
            }
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                if (treeView1.Nodes[0].Nodes.Count > 0)
                {
                    if ((treeView1.SelectedNode != e.Node) && EditGrades)
                    {
                        if (firstcount) { MessageBox.Show("Need to save grade"); }
                        e.Cancel = true;
                        firstcount = false;
                    }
                }
            }
            catch
            {
            }

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.Nodes[0].Nodes.Count > 0)
            {
                treeView1.SelectedNode = e.Node;
                AGrades = e.Node.Index;
                Change_Selected_Grade();
            }
        }

        private void Editable_Grades(bool b)
        {
            //Allow grades to be editted:
            cancelButton.Visible = b;
            saveButton.Visible = b;
            textBox1.ReadOnly = !b;
            textBox2.ReadOnly = !b;
            AliastextBox.ReadOnly = !b;
            comboBox1.Enabled = b;
            comboBox2.Enabled = b;
            percentcombo.Enabled = b;
            EditGrades = b;
            firstcount = b;
            treeView1.Enabled = !b;

        }

        private void Editable_Criteria(bool b)
        {
            //Allow criteria to be editted:
            if (!EditStudent)
            {
                cancelButton2.Visible = b;
                saveButton2.Visible = b;
                criteriaTitleBox.ReadOnly = !b;
                textBox4.ReadOnly = !b;
                comboBox3.Enabled = b;
                textBox7.ReadOnly = !b;
                textBox8.ReadOnly = !b;
                EditCriteria = b;
                LObutton.Enabled = b;
                //assessTitleBox.ReadOnly = !b;
                treeView2.Enabled = !b;
                button3.Enabled = !b;
            }
            else
            {
                MessageBox.Show("Cannot edit criteria whilst editing student");
            }
            //listBox1.Enabled = !b;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            //Save grades to gradelist struct:
            DialogResult dialogResult = MessageBox.Show("Save Yes/No?", "Save Grade", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Editable_Grades(false);
                gradelist[AGrades].grtitle = textBox1.Text;
                if (singleGrades)
                { listBox1.Items[AGrades] = textBox1.Text; }

                gradelist[AGrades].grfb = textBox2.Text;
                gradelist[AGrades].grpercent = Convert.ToSingle(percentcombo.Text);
                gradelist[AGrades].grlower = Convert.ToSingle(comboBox1.Text);
                gradelist[AGrades].grupper = Convert.ToSingle(comboBox2.Text);
                gradelist[AGrades].gralias = AliastextBox.Text;
                if (GradeTitleChanged)
                {
                    treeView1.Nodes[0].Nodes[AGrades].Text = textBox1.Text;
                    GradeTitleChanged = false;
                }
                Show_Label("Don't forget to save changes from the File menu!", 2000);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            //Don't save the grades if cancelled:
            DialogResult dialogResult = MessageBox.Show("Cancel edit Yes/No?", "Cancel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Editable_Grades(false);
                Show_Label("Don't forget to save changes!", 2000);
            }
        }


        private void saveGradesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Open save file dialog to select file to save grades:
            if (GradePath.Length < 1)
            {
                GradePath = DefaultDir;
                saveFileDialog1.InitialDirectory = GradePath;
            }
            GradePath = DefaultDir;
            saveFileDialog1.InitialDirectory = GradePath;
            saveFileDialog1.ShowDialog();
        }

        private void loadGradesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EditGrades)
            {
                MessageBox.Show("Editing grades - save first");
                return;
            }
            //Open file dialog to select file to read grades:
            if (GradePath.Length < 1)
            {
                GradePath = DefaultDir;
            }
            openFileDialog1.FileName = "";
            openFileDialog1.InitialDirectory = GradePath;
            openFileDialog1.ShowDialog();
        }

        private void newGradesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EditGrades)
            {
                MessageBox.Show("Editing grades - save first");
                return;
            }
            DialogResult dialogResult = MessageBox.Show("This will delete current grades - proceed Yes/No?", "New Grades", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Remove_Grades();
                X = 0;
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            SaveGradesToFile(saveFileDialog1.FileName);
            GradeFile = saveFileDialog1.FileName;

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            ReadGradesFromFile(openFileDialog1.FileName);
        }


        private void SaveGradesToFile(string filename)
        {
            // write grades to file:
            string grtype = "single";
            if (!singleGrades) { grtype = "dual"; }
            try
            {
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    sw.WriteLine("Grade type: " + grtype);
                    for (int i = 0; i < MaxGrades; i++)
                    {
                        if (gradelist[i].grtitle != null)
                        {
                            sw.WriteLine("grtitle: " + gradelist[i].grtitle);
                            sw.WriteLine("grfb: ");
                            sw.WriteLine(gradelist[i].grfb);
                            sw.WriteLine("endfb:");
                            sw.WriteLine("grpercent: " + gradelist[i].grpercent);
                            sw.WriteLine("grlower: " + gradelist[i].grlower);
                            sw.WriteLine("grupper: " + gradelist[i].grupper);
                            if (gradelist[i].grselected)
                            {
                                sw.WriteLine("grselected: true");
                            }
                            else
                            {
                                sw.WriteLine("grselected: false");
                            }
                            sw.WriteLine("gralias: " + gradelist[i].gralias);
                            sw.WriteLine("");
                        }
                    }
                    for (int i=0; i< MaxGradeGroups; i++)   //save grade groups in sequence starting from 1
                    {                      
                        sw.WriteLine("Group title: " + gradegrouplist[i].GroupText);
                    }
                   
                    sw.Close();
                    GradeFile = filename;
                    GradePath = Path.GetDirectoryName(filename);
                }
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void Remove_Grades()
        {
            for (int i = 0; i < MaxGrades; i++)
            {
                gradelist[i].grfb = "";
                gradelist[i].grtitle = null;
                gradelist[i].grpercent = 0;
                gradelist[i].grpercent = 0;
                gradelist[i].grpercent = 0;
                gradelist[i].gralias = "";
                gradelist[i].grselected = false;
                treeView1.Nodes[0].Remove();
                treeView1.Nodes.Add("Grades");
            }
            for (int i =0; i < MaxGradeGroups; i++)
            {
                gradegrouplist[i].GroupText = "Group i+1";
                //GrouplistBox.Items.Clear();
            }
            listBox1.Items.Clear();
            aliaslist.Clear();
        }

        private void ReadGradesFromFile(string filename)
        {
            string str, nl, substr;
            int B = -1; //start below zero and increment to 0 at first title
            bool fl = true;
            int g = 0;
            loading = true;
            //treeView1.SelectedNode.ForeColor = Color.Black;
            try
            {
                Remove_Grades();
                BlankGradeGroups();

                // Create an instance of StreamWriter to read grades from file:
                using (StreamReader sw = new StreamReader(filename))
                {
                    while (!sw.EndOfStream)
                    {
                        str = sw.ReadLine();
                        str.Trim();
                        if (str.StartsWith("grtitle: "))
                        {
                            B++; //increments B at start of each grade
                            gradelist[B].grtitle = str.Substring("grtitle: ".Length, str.Length - "grtitle: ".Length);
                            {
                                treeView1.Nodes[0].Nodes.Add(gradelist[B].grtitle);
                                treeView1.SelectedNode = treeView1.Nodes[0].LastNode;
                                if (singleGrades)
                                {
                                    listBox1.Items.Add(gradelist[B].grtitle);
                                }
                                treeView1.Nodes[0].Expand();
                                foreach (TreeNode RootNode in treeView1.Nodes)
                                {
                                    RootNode.ContextMenuStrip = contextMenuStrip1;
                                    foreach (TreeNode ChildNode in RootNode.Nodes)
                                    {
                                        ChildNode.ContextMenuStrip = contextMenuStrip2;
                                    }
                                }
                            }
                        }
                        else if (str.StartsWith("grpercent: "))
                        {
                            substr = str.Substring("grpercent: ".Length, str.Length - "grpercent: ".Length);
                            gradelist[B].grpercent = Convert.ToSingle(substr.Trim());
                        }
                        else if (str.StartsWith("grlower: "))
                        {
                            substr = str.Substring("grlower: ".Length, str.Length - "grlower: ".Length);
                            gradelist[B].grlower = Convert.ToSingle(substr.Trim());
                        }
                        else if (str.StartsWith("grupper: "))
                        {
                            substr = str.Substring("grupper: ".Length, str.Length - "grupper: ".Length);
                            gradelist[B].grupper = Convert.ToSingle(substr.Trim());

                        }
                        else if (str.StartsWith("grselected: "))
                        {
                            substr = str.Substring("grselected: ".Length, str.Length - "grselected: ".Length);
                            if (substr.Trim() == "true")
                            {
                                gradelist[B].grselected = true;
                            }
                            else
                            {
                                gradelist[B].grselected = false;
                            }

                        }
                        else if (str.StartsWith("gralias: "))
                        {
                            substr = str.Substring("gralias: ".Length, str.Length - "gralias: ".Length);
                            if (substr != null)
                            {
                                gradelist[B].gralias = substr;
                                if (gradelist[B].grselected)
                                {
                                    aliases a;
                                    a.alias = gradelist[B].gralias;
                                    a.percent = gradelist[B].grpercent;
                                    a.grindex = B;
                                    aliaslist.Add(a);
                                    if (!singleGrades)
                                    {
                                        if (gradelist[B].grselected)
                                        {
                                            listBox1.Items.Add(gradelist[B].gralias);
                                            treeView1.SelectedNode.NodeFont = new Font(TVFont, FontStyle.Italic | FontStyle.Bold);
                                        }
                                        else
                                        {
                                            treeView1.SelectedNode.NodeFont = new Font(TVFont, FontStyle.Regular);
                                        }
                                    }
                                }
                            }

                        }
                        else if (str.StartsWith("Grade type: "))
                        {
                            substr = str.Substring("Grade type: ".Length, str.Length - "Grade type: ".Length);
                            if (substr.Trim() == "dual")
                            {
                                singleGrades = false;
                            }

                            else
                            {
                                singleGrades = true;
                            }
                        }
                        else if (str.StartsWith("grfb: "))
                        {
                            fl = true;
                            while (!str.StartsWith("endfb:"))
                            {
                                str = sw.ReadLine();

                                if (!str.StartsWith("endfb:"))
                                {
                                    if (!fl)
                                    { nl = System.Environment.NewLine; }
                                    else
                                    { nl = ""; fl = false; }
                                    gradelist[B].grfb = gradelist[B].grfb + nl + str;
                                }
                            }
                        }
                        else if (str.StartsWith("Group title: "))   // Grade groups
                        {   //starting from 1 (don't need number - just put them in order when saving)                 
                            substr = str.Substring("Group title: ".Length, str.Length - "Group title: ".Length);
                            gradegrouplist[g].GroupText = substr;
                            gradegrouplist[g].GroupNo = g + 1;
                            g++;
                            
                        }


                        if (B > MaxGrades - 1)
                        {
                            break;
                        }
                    }
                    X = B;
                    sw.Close();
                    SetGradeGroup();
                    GradeFile = filename;
                    GradePath = Path.GetDirectoryName(filename);
                }
                loading = false;
            }//try

            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
                loading = false;
            }
        }//readfrom file



        private void Build_Grade_Lists()
        {
            //build the list box for grade boundaries:
            double i = 100;
            double f = 1.0;
            double j = 100;

            if (!checkBox1.Checked)
            {
                f = 1;
            }
            else
            {
                f = 0.5;
            }
            while (i > -1)
            {
                if (j > -1)
                {
                    percentcombo.Items.Add(Convert.ToString(j));
                    j--;
                }
                comboBox1.Items.Add(Convert.ToString(i));
                comboBox2.Items.Add(Convert.ToString(i));
                i = i - f;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //fine or coarse grade boundaries by selecting checkbox:
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            Build_Grade_Lists();
        }

        //---------------------------------End grade functions ------------------------------------------------------------

        private void saveCriteriaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //prepare to save criteria

            //saveFileDialogda2.InitialDirectory = CriteriaPath;
            
                CriteriaPath = UnitFilePath;
                saveFileDialog2.InitialDirectory = CriteriaPath;
                saveFileDialog2.ShowDialog();
        }

        private void loadCriteriaToolStripMenuItem_Click(object sender, EventArgs e)
        {   //load assessment
            if (EditCriteria)
            {
                MessageBox.Show("Editing criteria - save first");
                return;
            }
            //prepare to read criteria
            CriteriaPath = UnitFilePath;
            /*if (CriteriaPath.Length < 1)
            {
                CriteriaPath = UnitFilePath;
            }*/
            openFileDialog2.FileName = "";
            openFileDialog2.InitialDirectory = CriteriaPath;
            openFileDialog2.ShowDialog();
        }

        private void AddCriteria(bool insert)
        {
            //Ad or insert new criteria
            string str;


            //Add a new criteria:   
            InputForm input = new InputForm();
            input.browser = false;
            input.text = "";
            input.Passvalue = "Enter a title for Criteria:";
            input.ShowDialog();
            str = input.Passvalue;
            int insI = 0;
            insI = treeView2.SelectedNode.Index;
            if (str.Length > 0)
            {
                if (insert)
                {
                    insertingCriteria = true;
                    treeView2.Nodes[0].Nodes.Insert(insI + 1, str);
                    treeView2.SelectedNode = treeView2.Nodes[0].Nodes[insI + 1];
                    criteriaTitleBox.Text = str;
                    InsertMoveCriteria(insI + 1, str);
                    insertingCriteria = false;
                }
                else
                {
                    treeView2.Nodes[0].Nodes.Add(str);
                }
                treeView2.Nodes[0].Expand();
                int num = treeView2.Nodes[0].LastNode.Index;
                foreach (TreeNode RootNode in treeView2.Nodes)
                {
                    RootNode.ContextMenuStrip = contextMenuStrip3;
                    foreach (TreeNode ChildNode in RootNode.Nodes)
                    {
                        ChildNode.ContextMenuStrip = contextMenuStrip4;
                    }
                }
                crtitle[num, MaxSub] = str; //Use MaxSub as second index for Criteria and then use 0 to MaxSub-1 for sub-criteria  
                CriteriaExists[num, MaxSub] = true;
                CritZ = CritZ + 1;
            }


        }

        private void InsertMoveCriteria(int InsI, string str)
        {
            //re-align when insert criteria
            try
            {
                for (int i = CritZ + 1; i >= InsI; i--)
                {
                    for (int j = 0; j < MaxSub + 1; j++)
                    {
                        crtitle[i, j] = crtitle[i - 1, j];
                        crdesc[i, j] = crdesc[i - 1, j];
                        crLO[i, j] = crLO[i - 1, j];
                        crweight[i, j, Session] = crweight[i - 1, j, Session];
                        crsub[i, j] = crsub[i - 1, j];
                        crComment[i, j, Session] = crComment[i - 1, j, Session];
                        crSelected[i, j, Session] = crSelected[i - 1, j, Session];
                        for (int g = 0; g < listBox1.Items.Count; g++)
                        {
                            grcrtitle[i, j, g] = grcrtitle[i - 1, j, g];
                            grcr[i, j, g] = grcr[i - 1, j, g];
                            grcrfb[i, j, g] = grcrfb[i - 1, j, g];
                            //crComment[i, j, g] = crComment[i - 1, j, g];
                            //crSelected[i, j, g] = crSelected[i - 1, j, g];
                        }
                    }
                }
                for (int j = 0; j < MaxSub + 1; j++)
                {
                    crtitle[InsI, j] = str;
                    criteriaTitleBox.Text = str;
                    crdesc[InsI, j] = "";
                    crLO[InsI, j] = "";
                    crweight[InsI, j, Session] = 0;
                    crsub[InsI, j] = false;
                    crComment[InsI, j, Session] = "";
                    //crSelected[InsI, j, Session] = false;
                    crSelected[InsI, j, Session] = true;
                    for (int g = 0; g < listBox1.Items.Count; g++)
                    {
                        grcrtitle[InsI, j, g] = "";
                        grcr[InsI, j, g] = "";
                        grcrfb[InsI, j, g] = "";
                        //crComment[InsI, j, g] = "";
                        //crSelected[InsI, j, g] = false;
                        //crSelected[InsI, j, g] = true;
                    }
                }

                textBox4.Text = "";
                LOtextBox.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";

            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void RemoveCriteria(int item)
        {
            //re-align when delete criteria
            try
            {
                if (item < CritZ)
                {
                    CriteriaExists[item, MaxSub] = CriteriaExists[item + 1, MaxSub];
                }
                //crtitle[SCriteria, MaxSub] = null;
                for (int i = item; i < CritZ; i++)
                {
                    for (int j = 0; j < MaxSub + 1; j++)
                    {
                        crtitle[i, j] = crtitle[i + 1, j];
                        crdesc[i, j] = crdesc[i + 1, j];
                        crLO[i, j] = crLO[i + 1, j];
                        crweight[i, j, Session] = crweight[i + 1, j, Session];
                        crsub[i, j] = crsub[i + 1, j];
                        for (int g = 0; g < listBox1.Items.Count; g++)
                        {
                            grcrtitle[i, j, g] = grcrtitle[i + 1, j, g];
                            grcr[i, j, g] = grcr[i + 1, j, g];
                            grcrfb[i, j, g] = grcrfb[i + 1, j, g];
                            crComment[i, j, g] = crComment[i + 1, j, g];
                            crSelected[i, j, g] = crSelected[i + 1, j, g];
                        }
                    }
                }
                CriteriaExists[CritZ, MaxSub] = false;
                CritZ--;

            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void contextMenuStrip3_Click(object sender, EventArgs e)
        {
            bool addcr = false;
            //add new CRITERIA            
            if (CritZ > MaxCriteria - 1)
            {
                MessageBox.Show("Can only have a maximum of " + Convert.ToString(MaxCriteria) + " criteria");
                return;
            }
            if (contextMenuStrip3.Items[0].Selected)
            {
                //call addcriteria
                AddCriteria(addcr);
            }

        }

        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //need to find which child node is selected in order to remove it ***      

            if (treeView2.Nodes[0].Nodes.Count > 0)
            {
                if ((!replicate_Criteria) || (!replicate_Feedback) || (!replicate_LO) || (!replicate_Description))
                {
                    treeView2.SelectedNode = e.Node;
                }

                Change_Selected_Criteria();
            }
        }

        private void Change_Selected_Criteria()
        {
            //change criteria that has been selected in the criteria treeview
            int i;
            bool s = false;
            string p1;

            try
            {
                foreach (TreeNode RootNode in treeView2.Nodes)
                {
                    foreach (TreeNode ChildNode in RootNode.Nodes)
                    {
                        if (ChildNode.IsSelected)
                        {
                            SCriteria = ChildNode.Index;

                            CriteriaSelected = true;
                            break;
                        }
                        foreach (TreeNode SubNode in ChildNode.Nodes)
                        {
                            if (SubNode.IsSelected)
                            {
                                SCriteria = ChildNode.Index;
                                SSub = SubNode.Index;

                                CriteriaSelected = false;
                                break;
                            }

                        }
                    }
                }
                if (!CriteriaSelected)
                {
                    i = SSub;
                }
                else
                {
                    i = MaxSub;
                }

                if ((!replicate_Criteria) && (!replicate_Feedback) && (!replicate_LO) && (!replicate_Description)) //do this unless replicating criteria, feedback or LO
                {
                    criteriaTitleBox.Text = crtitle[SCriteria, i];
                    textBox4.Text = crdesc[SCriteria, i];
                    LOtextBox.Text = crLO[SCriteria, i];
                    if (crweight[SCriteria, i, Session] > 0)
                    {
                        comboBox3.Text = Convert.ToString(crweight[SCriteria, i, Session]);
                    }
                    else
                    {
                        comboBox3.Text = "0";
                    }
                    if (listBox1.Items.Count > 0)
                    {
                        if (singleGrades)
                        {
                            textBox7.Text = grcr[SCriteria, i, listBox1.SelectedIndex];
                            textBox8.Text = grcrfb[SCriteria, i, listBox1.SelectedIndex];
                            gradelbl.Text = listBox1.GetItemText(listBox1.SelectedItem);
                        }
                        else
                        {
                            textBox7.Text = grcr[SCriteria, i, aliaslist[listBox1.SelectedIndex].grindex];
                            textBox8.Text = grcrfb[SCriteria, i, aliaslist[listBox1.SelectedIndex].grindex];
                            gradelbl.Text = listBox1.GetItemText(aliaslist[listBox1.SelectedIndex].grindex);
                        }
                    }
                    fblbl.Text = gradelbl.Text;
                    if (Aggregation && CriteriaSelected)
                    {
                        s = Has_Subcriteria(SCriteria);
                        if ((!s) && (firstS))
                        {
                            MessageBox.Show("Warning: Check your criteria type matches the criteria");
                            firstS = false;
                        }
                    }
                    if (!s) //if no subcriteria and no aggregation of subcriteria
                    {

                        if (EditStudent)
                        {
                            overrideBox.Text = "";
                            if (crSelected[SCriteria, i, Session] == true) //if this criteria has been selected
                            {
                                label18.Visible = true;
                                label16.Visible = true;
                                label18.Text = Marks[SCriteria, i, Session];
                                p1 = label18.Text;
                                if (p1.EndsWith("%"))
                                {
                                    overrideBox.Text = p1.Substring(0, p1.Length - 1);
                                }
                                else
                                {
                                    overrideBox.Text = "";
                                }
                                Find_Grade_In_List(label18.Text.Trim());
                            }
                            else
                            {
                                label18.Text = "n/a";
                            }
                        }
                    }
                    else
                    {
                        //if criteria selected and aggregation don't show marks for this criteria
                        label18.Visible = false;
                        label16.Visible = false;
                    }
                }
                else if (replicate_Criteria) //replicate criteria
                {
                    if (PreNode != treeView2.SelectedNode)
                    {
                        Replicate_Criteria_Function(SCriteria, i);
                        PreNode = treeView2.SelectedNode;
                    }
                }
                else if (replicate_Feedback) //replicate feedback
                {
                    if (PreNode != treeView2.SelectedNode)
                    {
                        Replicate_Feedback_Function(SCriteria, i);
                        PreNode = treeView2.SelectedNode;
                    }
                }
                else if (replicate_Description) //replicate feedback
                {
                    if (PreNode != treeView2.SelectedNode)
                    {
                        Replicate_Criteria_Description_Function(SCriteria, i);
                        PreNode = treeView2.SelectedNode;
                    }
                }
                else if (replicate_LO) //replicate LO
                {
                    if (PreNode != treeView2.SelectedNode)
                    {
                        Replicate_LO_Function(SCriteria, i);
                        PreNode = treeView2.SelectedNode;
                    }
                }

            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }



        private void Replicate_Criteria_Function(int s, int i)
        {
            bool all = false;
            DialogResult dialogResult = MessageBox.Show("Replicate comments for this grade to selected criteria or cancel replication?", "Replicate grade comments", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes)
            {
                //crdesc[s, i] = crdesc[SelCr, SelSub];
                DialogResult dialogResult2 = MessageBox.Show("Warning: Do this for all grades?", "All grades or just this one?", MessageBoxButtons.YesNo);
                if (dialogResult2 == DialogResult.Yes)
                {
                    all = true;
                }
                if (all)
                {
                    for (int g = 0; g < listBox1.Items.Count; g++)
                    {
                        grcr[s, i, g] = grcr[SelCr, SelSub, g];
                    }
                }
                else
                {
                    grcr[s, i, listBox1.SelectedIndex] = textBox7.Text;
                }

            }
            else if (dialogResult == DialogResult.Cancel)
            {
                replicate_Criteria = false;
                repCancelbutton1.Visible = false;
                treeView2.SelectedNode = SelNode;
            }
            criteriaTitleBox.Text = treeView2.SelectedNode.Text;

        }

        private void Replicate_Criteria_Description_Function(int s, int i)
        {
            bool all = false;
            DialogResult dialogResult = MessageBox.Show("Replicate criteria description or cancel replication?", "Replicate criteria description", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes)
            {
                crdesc[s, i] = textBox4.Text;                

            }
            else if (dialogResult == DialogResult.Cancel)
            {
                replicate_Description = false;
                repCancelbutton4.Visible = false;
                treeView2.SelectedNode = SelNode;
            }
            criteriaTitleBox.Text = treeView2.SelectedNode.Text;

        }

        private void Replicate_Feedback_Function(int s, int i)
        {
            bool all = false;
            DialogResult dialogResult = MessageBox.Show("Replicate Feedback for this grade to selected feedback or cancel replication?", "Replicate grade Feedback", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes)
            {
                DialogResult dialogResult2 = MessageBox.Show("Warning: Do this for all grades?", "All grades or just this one?", MessageBoxButtons.YesNo);
                if (dialogResult2 == DialogResult.Yes)
                {
                    all = true;
                }
                if (all)
                {
                    for (int g = 0; g < listBox1.Items.Count; g++)
                    {
                        grcrfb[s, i, g] = grcrfb[SelCr, SelSub, g];
                    }
                }
                else
                {
                    grcrfb[s, i, listBox1.SelectedIndex] = textBox8.Text;
                }

            }
            else if (dialogResult == DialogResult.Cancel)
            {
                replicate_Feedback = false;
                repCancelbutton2.Visible = false;
                treeView2.SelectedNode = SelNode;
            }

        }

        private void Replicate_LO_Function(int s, int i)
        {

            DialogResult dialogResult = MessageBox.Show("Replicate learning outcomes to selected criteria or cancel replication?", "Replicate Learning Outcomes", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes)
            {
                crLO[s, i] = crLO[SelCr, SelSub];

            }
            else if (dialogResult == DialogResult.Cancel)
            {
                replicate_LO = false;
                repCancelbutton3.Visible = false;
                treeView2.SelectedNode = SelNode;
            }

        }

        private void Find_Grade_In_List(string grade)
        {
            int i = listBox1.FindString(grade, 0);
            listBox1.SelectedIndex = i;
            CurrentlySelected = i;
        }

        private bool Has_Subcriteria(int c)
        {
            bool s = false;
            for (int j = 0; j < MaxSub; j++)
            {
                if (crtitle[c, j] != null)
                {
                    if (crtitle[c, j].Trim().Length > 0)
                    {
                        s = true;
                        break;
                    }
                }
            }
            return s;
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }

        private void contextMenuStrip4_Click(object sender, EventArgs e)
        {
            bool insert = true;
            if (EditStudent) { MessageBox.Show("Cannot do this whilst marking student"); return; }

            try
            {
                //edit, insert or delete criteria, or add Sub-criteria:
                if (contextMenuStrip4.Items[0].Selected)
                {
                    //EDIT CRITERIA
                    //MessageBox.Show("Edit");
                    Editable_Criteria(true);
                    //EditMode = true;
                    firstcount = true;
                    SSub = MaxSub;
                }
                else if (contextMenuStrip4.Items[1].Selected)
                {
                    //remove the criteria node (gets selected node from treeview1_NodeMouseClick):
                    DialogResult dialogResult = MessageBox.Show("Delete Yes/No?", "Remove Criteria", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        RemoveCriteria(SCriteria);
                        treeView2.Nodes[0].Nodes[SCriteria].Remove();
                        
                        

                    }
                }
                else if (contextMenuStrip4.Items[2].Selected)
                {
                    if (CritZ > MaxCriteria - 1)
                    {
                        MessageBox.Show("Can only have a maximum of " + Convert.ToString(MaxCriteria) + " criteria");
                    }
                    else
                    {
                        AddCriteria(insert); //insert criteria
                    }
                }
                else if (contextMenuStrip4.Items[3].Selected)
                {
                    if (CriteriaType == 4)
                    { MessageBox.Show("Criteria type (checkboxes yes/no) doesn't currently support sub-criteria"); return; }
                    Add_Sub_Criteria(); //Add sub-criteria
                }
                else if (contextMenuStrip4.Items[5].Selected)
                {
                    //replicate criteria
                    replicate_Criteria = true;
                    treeView2.Enabled = true;
                    SelNode = treeView2.SelectedNode;
                    PreNode = SelNode;
                    SelSub = SSub;
                    if (CriteriaSelected)
                    {
                        SelSub = MaxSub;
                    }
                    SelCr = SCriteria;
                    repCancelbutton1.Visible = true;
                    MessageBox.Show("Select criteria to replicate to");

                }
                else if (contextMenuStrip4.Items[6].Selected)
                {
                    //replicate feedback
                    replicate_Feedback = true;
                    treeView2.Enabled = true;
                    SelNode = treeView2.SelectedNode;
                    SelSub = SSub;
                    if (CriteriaSelected)
                    {
                        SelSub = MaxSub;
                    }
                    SelCr = SCriteria;
                    PreNode = SelNode;
                    repCancelbutton2.Visible = true;
                    MessageBox.Show("Select feedback to replicate to");

                }
                else if (contextMenuStrip4.Items[7].Selected)
                {
                    //replicate LO
                    replicate_LO = true;
                    treeView2.Enabled = true;
                    SelNode = treeView2.SelectedNode;
                    PreNode = SelNode;
                    SelSub = SSub;
                    if (CriteriaSelected)
                    {
                        SelSub = MaxSub;
                    }
                    SelCr = SCriteria;
                    repCancelbutton3.Visible = true;
                    MessageBox.Show("Select criteria to replicate to");
                }
                else if (contextMenuStrip4.Items[4].Selected)
                {
                    //replicate criteria Description
                    replicate_Description = true;
                    treeView2.Enabled = true;
                    SelNode = treeView2.SelectedNode;
                    SelSub = SSub;
                    if (CriteriaSelected)
                    {
                        SelSub = MaxSub;
                    }
                    SelCr = SCriteria;
                    PreNode = SelNode;
                    repCancelbutton4.Visible = true;
                    MessageBox.Show("Select criteria description to replicate to");

                }
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void Add_Sub_Criteria()
        {
            string str;

            //Add a new SUB criteria:   
            InputForm input = new InputForm();
            input.browser = false;
            input.text = "";
            input.Passvalue = "Enter a title for Sub-Criteria:";
            input.ShowDialog();
            str = input.Passvalue;
            try
            {
                if (str.Length > 0)
                {
                    TreeNode tnode = treeView2.SelectedNode;
                    tnode.Nodes.Add(str);
                    int num = treeView2.Nodes[0].Nodes[SCriteria].LastNode.Index;

                    //treeView2.Nodes[0].Nodes[SCriteria].Add(str);
                    treeView2.Nodes[0].Nodes[SCriteria].Expand();

                    if (num >= MaxSub)
                    {
                        treeView2.Nodes[0].Nodes[SCriteria].LastNode.Remove();                       
                        MessageBox.Show("Cannot have more than " + Convert.ToString(MaxSub) + " sub-criteria");
                        return;
                    }

                    foreach (TreeNode RootNode in treeView2.Nodes)
                    {
                        RootNode.ContextMenuStrip = contextMenuStrip3;
                        foreach (TreeNode ChildNode in RootNode.Nodes)
                        {
                            ChildNode.Expand();
                            ChildNode.ContextMenuStrip = contextMenuStrip4;
                            foreach (TreeNode SubNode in ChildNode.Nodes)
                            {
                                SubNode.Expand();
                                SubNode.ContextMenuStrip = contextMenuStrip5;
                            }
                        }
                    }
                    crtitle[SCriteria, num] = str;
                    CriteriaExists[SCriteria, num] = true;
                    //SubZ = SubZ + 1;
                }
            }//try
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void Build_Criteria_List()
        {
            //Populate the criteria weight combo box:
            double i = 100;
            double f = 1.0;
            comboBox3.Items.Clear();
            if (fineWeight ==1)
            {
                f = 1;
            }
            else if (fineWeight == 5)
            {
                f = 5;
            }
            else
            {
                f = 0.5;
            }
            while (i > 0)
            {
                comboBox3.Items.Add(Convert.ToString(i));
                i = i - f;
            }
            Show_Label("Weight box set to " + Convert.ToString(f) + "%", 1500);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //If checkbox on criteria tab checked then rebuild the combo box:
            //comboBox3.Items.Clear();
            //Build_Criteria_List();
        }

        private void saveButton2_Click(object sender, EventArgs e)
        {
            int i;
            DialogResult dialogResult = DialogResult.Yes;
            //Save the criteria to the criteria list struct
            if (!promptsoff)
            {
                dialogResult = MessageBox.Show("Save Yes/No?", "Save Criteria", MessageBoxButtons.YesNo);
            }
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    if (CriteriaSelected)  //criteria or sub-criteria selected?
                    {
                        i = MaxSub;
                    }
                    else
                    {
                        i = SSub;
                    }
                    Editable_Criteria(false);
                    crtitle[SCriteria, i] = criteriaTitleBox.Text;
                    crdesc[SCriteria, i] = textBox4.Text;
                    crLO[SCriteria, i] = LOtextBox.Text;
                    crweight[SCriteria, i, Session] = Convert.ToSingle(comboBox3.Text);
                    //ADD CODE HERE TO SAVE TO 
                    grcr[SCriteria, SSub, listBox1.SelectedIndex] = textBox7.Text;
                    grcrfb[SCriteria, SSub, listBox1.SelectedIndex] = textBox8.Text;
                    assess.Title = assessTitleBox.Text;
                    //EditMode = false;
                    TChanged = false;
                    treeView2.SelectedNode.Text = criteriaTitleBox.Text;
                    //listBox3.Enabled = false;
                    clearLObutton.Visible = false;
                }
                catch (System.Exception excep)
                {
                    StackTrace stackTrace = new StackTrace();
                    MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
                }
            }
            Show_Label("Don't forget to save changes to the Assessment from File menu!", 2500);
        }


        private void Show_Label(string label, int t)
        {
            string[] str = new string[2];
            labelForm labelF = new labelForm();

            labelF.Passvalue[0] = label;
            labelF.Passvalue[1] = Convert.ToString(t);
            labelF.Show();
        }

        private void cancelButton2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Cancel edit Yes/No?", "Cancel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Editable_Criteria(false);
                //listBox3.Enabled = false;
                clearLObutton.Visible = false;
                Show_Label("Don't forget to save changes!", 1500);
            }
        }

        private void SaveCriteriaToFile(string filename)
        {
            bool nullText = false;
            // write criteria to file:
            int j = MaxSub;
            string endchar = "";
            string ct = "0";
            try
            {
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    sw.WriteLine("File Version: 2");
                    sw.WriteLine("Assessment title: " + assessTitleBox.Text);
                    sw.WriteLine("assessdesc: ");
                    sw.WriteLine(assess.Description);
                    sw.WriteLine("endassessdesc:");
                    sw.WriteLine("Assessment weight: " + assess.Weight);
                    sw.WriteLine("Assessment code: " + assess.Code);
                    sw.WriteLine("assessLO: ");
                    sw.WriteLine(assess.LOs);
                    sw.WriteLine("endassessLO:");
                    try
                    {
                        ct = Convert.ToString(CriteriaType);
                    }
                    catch
                    {
                        ct = "0";
                    }
                    
                    sw.WriteLine("Criteria type: " + ct);
                    string grtype = "single";
                    if (!singleGrades) { grtype = "dual"; }
                    sw.WriteLine("Criteria Selection Type: " + CriteriaSelectionType.ToString());
                    sw.WriteLine("Grade type: " + grtype);
                    for (int i = 0; i <= CritZ; i++)
                    {
                        if (crtitle[i, j] == null)
                        {
                            nullText = true;
                        }
                        else { nullText = false; }
                        if (!nullText && CriteriaExists[i, j])
                        {
                            sw.WriteLine("Criteria: " + Convert.ToString(i + 1));
                            sw.WriteLine("crtitle: " + crtitle[i, j]);
                            sw.WriteLine("crdesc: ");
                            sw.WriteLine(crdesc[i, j]);
                            sw.WriteLine("enddesc:");
                            sw.WriteLine("crLO:");
                            sw.WriteLine(crLO[i, j]);
                            sw.WriteLine("endcrLO: ");
                            sw.Write("crweight: ");
                            int temp = 0;
                            if (SessionType == 0)
                            {
                                temp = 1;
                            }
                            else
                            {
                                temp = SessionS;
                            }
                            for (int se = 0; se < temp; se++)
                            {
                                if (se < temp - 1)
                                { endchar = ","; }
                                else { endchar = ""; }
                                sw.Write(Convert.ToString(crweight[i, j, se]) + endchar);
                            }
                            sw.WriteLine();
                            int k = 0;
                            foreach (string s in listBox1.Items)
                            {
                                sw.WriteLine("grade: " + s);
                                sw.WriteLine("grcr: ");
                                sw.WriteLine(grcr[i, j, k]);
                                sw.WriteLine("endgrcr: ");
                                sw.WriteLine("gradefb: ");
                                sw.WriteLine(grcrfb[i, j, k]);
                                sw.WriteLine("endgrcrfb: ");
                                k++;
                            }
                            sw.WriteLine("endCriteria: ");
                            sw.WriteLine(" ");
                        }


                        for (int m = 0; m < MaxSub; m++)
                        {
                            // try
                            //{
                            if (crtitle[i, m] == null)
                            {
                                nullText = true;
                            }
                            else { nullText = false; }
                            if (!nullText && CriteriaExists[i, m])
                            {
                                sw.WriteLine("Sub_criteria: " + Convert.ToString(m + 1));
                                sw.WriteLine("crtitle: " + crtitle[i, m]);
                                sw.WriteLine("crdesc: ");
                                sw.WriteLine(crdesc[i, m]);
                                sw.WriteLine("enddesc:");
                                for (int se = 0; se < SessionS + 1; se++)
                                {
                                    if (se < SessionS)
                                    { endchar = ","; }
                                    else { endchar = ""; }
                                    sw.Write("crweight: " + Convert.ToString(crweight[i, m, se]) + endchar);
                                }
                                sw.WriteLine();
                                int k = 0;
                                foreach (string s in listBox1.Items)
                                {
                                    sw.WriteLine("grade: " + s);
                                    sw.WriteLine("grcr: ");
                                    sw.WriteLine(grcr[i, m, k]);
                                    sw.WriteLine("endgrcr: ");
                                    sw.WriteLine("gradefb: ");
                                    sw.WriteLine(grcrfb[i, m, k]);
                                    sw.WriteLine("endgrcrfb: ");

                                    k++;
                                }

                                sw.WriteLine("endSub: ");
                                sw.WriteLine(" ");
                            }//!nullText
                            //}
                            //catch
                            // { }
                        }
                    }
                    sw.Close();
                    CriteriaFile = filename;
                    CriteriaPath = Path.GetDirectoryName(filename);
                }
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void Clear_All_Criteria(bool clearboxes)
        {
            treeView2.Nodes[0].Remove();
            treeView2.Nodes.Add("Criteria");
            CritZ = 0;
            if (clearboxes)
            {
                criteriaTitleBox.Text = "";
                textBox4.Text = "";
                textBox7.Text = ""; //textbox7 is text box containing "criteria to achieve grade"
                textBox8.Text = "";
                textBox10.Text = "";
            }
            //Feedback = "";
            //comboBox3.Items.Clear();
            for (int i = 0; i < MaxCriteria; i++)
            {
                for (int j = 0; j < MaxSub + 1; j++)
                {
                    CriteriaExists[i, j] = false;
                    crtitle[i, j] = "";
                    crdesc[i, j] = "";
                    crLO[i, j] = "";
                    for (int se = 0; se < SessionS + 1; se++)
                    {
                        crweight[i, j, se] = 0;
                    }
                    crsub[i, j] = false;
                    for (int k = 0; k < MaxGrades; k++)
                    {
                        grcrtitle[i, j, k] = "";
                        grcr[i, j, k] = "";
                        grcrfb[i, j, k] = "";
                    }
                }
            }
        }

        private void ReadCriteriaFromFile(string filename, bool cleartextboxes)
        {
            string str, nl, substr;
            int C = -1;
            int S = -1;
            int L = 0;

            bool fl = true;
            bool first = true;
            string ct = "0";
            string gt = "single";
            string[] crs = new string[MaxSessions];
            bool newversion = false;
            assess.Description = "";
            loading = true;
            try
            {

                listBox1.Items.Clear();
                listBox3.Items.Clear();
                assess.LOs = "";
                foreach (string s in listBox1.Items)
                {
                    listBox1.Items.Remove(s);
                }
                CriteriaFile = filename;
                Clear_All_Criteria(cleartextboxes);

                // Create an instance of StreamWriter to read grades from file:
                using (StreamReader sw = new StreamReader(filename))
                {
                    while (!sw.EndOfStream)
                    {
                        str = sw.ReadLine();
                        str.Trim();
                        if (str.StartsWith("File Version: 2"))
                        {
                            newversion = true;
                            MaxSub = 20;
                        }  
                        if (!newversion)
                        {
                            MaxSub = 20;
                        }
                        if (str.StartsWith("Assessment title:"))
                        {
                            assessTitleBox.Text = str.Substring("Assessment title: ".Length, str.Length - "Assessment title: ".Length);
                            assess.Title = assessTitleBox.Text;
                        }
                        else if (str.StartsWith("Assessment weight:"))
                        {
                            assess.Weight = str.Substring("Assessment weight: ".Length, str.Length - "Assessment weight: ".Length);
                        }
                        else if (str.StartsWith("Assessment code:"))
                        {
                            assess.Code = str.Substring("Assessment code: ".Length, str.Length - "Assessment code: ".Length);
                        }
                        else if (str.StartsWith("Criteria type:"))
                        {
                            ct = str.Substring("Criteria type: ".Length, str.Length - "Criteria type: ".Length);
                            try
                            {
                                CriteriaType = Convert.ToInt32(ct.Trim());
                            }
                            catch
                            {

                            }

                        }
                        else if (str.StartsWith("Criteria Selection Type:"))
                        {
                            ct = str.Substring("Criteria Selection Type: ".Length, str.Length - "Criteria Selection Type: ".Length);
                            try
                            {
                                CriteriaSelectionType = Convert.ToInt32(ct.Trim());
                            }
                            catch
                            {

                            }

                        }
                        else if (str.StartsWith("Grade type:"))
                        {
                            gt = str.Substring("Grade type: ".Length, str.Length - "Grade type: ".Length);
                            try
                            {
                                if (gt.Trim() != "dual" && !singleGrades)
                                {
                                    Clear_Selected_Grades();
                                }
                                if (gt.Trim() == "dual")
                                {
                                    singleGrades = false;
                                }
                                else
                                {                                  
                                    singleGrades = true;
                                }
                            }
                            catch
                            {

                            }

                        }
                        else if (str.StartsWith("assessdesc: "))
                        {
                            fl = true;
                            while (!str.StartsWith("endassessdesc:"))
                            {
                                str = sw.ReadLine();

                                if (!str.StartsWith("endassessdesc:"))
                                {
                                    if (!fl)
                                    { nl = System.Environment.NewLine; }
                                    else
                                    { nl = ""; fl = false; }
                                    assess.Description = assess.Description + nl + str;
                                }
                            }
                        }
                        else if (str.StartsWith("assessLO: "))
                        {
                            fl = true;
                            while (!str.StartsWith("endassessLO:"))
                            {
                                str = sw.ReadLine();

                                if (!str.StartsWith("endassessLO:"))
                                {
                                    if (!fl)
                                    { nl = System.Environment.NewLine; }
                                    else
                                    { nl = ""; fl = false; }
                                    if (str.Trim().Length > 0)
                                    {
                                        assess.LOs = assess.LOs + nl + str;
                                        listBox3.Items.Add(str);
                                    }

                                }
                            }
                        }
                        else if (str.StartsWith("Criteria: "))
                        {
                            C++;
                            S = MaxSub;
                            while (!str.StartsWith("endCriteria: "))
                            {
                                str = sw.ReadLine();
                                str.Trim();
                                if (str.StartsWith("crtitle: "))
                                {
                                    //add criteria
                                    CriteriaExists[C, S] = true;
                                    crtitle[C, S] = str.Substring("crtitle: ".Length, str.Length - "crtitle: ".Length);
                                    {
                                        treeView2.Nodes[0].Nodes.Add(crtitle[C, S]);
                                        treeView2.Nodes[0].Expand();
                                        foreach (TreeNode RootNode in treeView2.Nodes)
                                        {
                                            RootNode.ContextMenuStrip = contextMenuStrip3;
                                            foreach (TreeNode ChildNode in RootNode.Nodes)
                                            {
                                                ChildNode.ContextMenuStrip = contextMenuStrip4;
                                            }
                                        }
                                    }
                                }
                                else if (str.StartsWith("crweight: "))
                                {
                                    substr = str.Substring("crweight: ".Length, str.Length - "crweight: ".Length);
                                    crs = substr.Split(',');
                                    for (int se = 0; se < crs.Count(); se++)
                                    {
                                        //try
                                        //{
                                        SessionS = SessionS++;
                                        if (SessionS > 1) { SessionType = 1; }
                                        else { SessionType = 0; }
                                        if (crs[se] != null)
                                        {
                                            if (crs[se].Trim() != "")
                                            { crweight[C, S, se] = Convert.ToSingle(crs[se].Trim()); }
                                        }
                                        //}
                                        //catch
                                        //{
                                        //}
                                    }

                                }
                                else if (str.StartsWith("endCriteria: "))
                                {
                                    L = 0;
                                    first = false;
                                    S = -1;
                                }
                                else if (str.StartsWith("crdesc: "))
                                {
                                    fl = true;
                                    while (!str.StartsWith("enddesc:"))
                                    {
                                        str = sw.ReadLine();
                                        if (!str.StartsWith("enddesc:"))
                                        {
                                            if (!fl)
                                            { nl = System.Environment.NewLine; }
                                            else
                                            { nl = ""; fl = false; }
                                            crdesc[C, S] = crdesc[C, S] + nl + str;
                                        }
                                    }
                                }
                                else if (str.StartsWith("crLO:"))
                                {
                                    fl = true;
                                    while (!str.StartsWith("endcrLO:"))
                                    {
                                        str = sw.ReadLine();
                                        if (!str.StartsWith("endcrLO:"))
                                        {
                                            if (!fl)
                                            { nl = System.Environment.NewLine; }
                                            else
                                            { nl = ""; fl = false; }
                                            crLO[C, S] = crLO[C, S] + nl + str;
                                        }
                                    }
                                }
                                else if (str.StartsWith("grade: "))
                                {
                                    substr = str.Substring("grade: ".Length, str.Length - "grade: ".Length);
                                    grcrtitle[C, S, L] = substr.Trim();
                                    if (first)
                                    {
                                        listBox1.Items.Add(substr);
                                    }
                                    L++;
                                }
                                else if (str.StartsWith("grcr: "))
                                {
                                    fl = true;
                                    while (!str.StartsWith("endgrcr:"))
                                    {
                                        str = sw.ReadLine();
                                        if (!str.StartsWith("endgrcr:"))
                                        {
                                            if (!fl)
                                            { nl = System.Environment.NewLine; }
                                            else
                                            { nl = ""; fl = false; }
                                            grcr[C, S, L - 1] = grcr[C, S, L - 1] + nl + str;
                                        }
                                    }
                                }
                                else if (str.StartsWith("gradefb: "))
                                {
                                    fl = true;
                                    while (!str.StartsWith("endgrcrfb:"))
                                    {
                                        str = sw.ReadLine();

                                        if (!str.StartsWith("gradefb:"))
                                        {
                                            if (!fl)
                                            { nl = System.Environment.NewLine; }
                                            else
                                            { nl = ""; fl = false; }
                                            if (!str.StartsWith("endgrcrfb:"))
                                            {
                                                grcrfb[C, S, L - 1] = grcrfb[C, S, L - 1] + nl + str;
                                            }
                                        }
                                    }
                                }


                            }
                        }

                        else if (str.StartsWith("Sub_criteria: "))
                        {
                            while (!str.StartsWith("endSub: "))
                            {
                                str = sw.ReadLine();
                                str.Trim();
                                if (str.StartsWith("crtitle: "))
                                {
                                    //add sub criteria                              
                                    S++;
                                    CriteriaExists[C, S] = true;
                                    crtitle[C, S] = str.Substring("crtitle: ".Length, str.Length - "crtitle: ".Length);
                                    treeView2.Nodes[0].Nodes[C].Nodes.Add(crtitle[C, S]);
                                    treeView2.Nodes[0].Nodes[C].Expand();

                                    foreach (TreeNode RootNode in treeView2.Nodes)
                                    {
                                        RootNode.ContextMenuStrip = contextMenuStrip3;
                                        foreach (TreeNode ChildNode in RootNode.Nodes)
                                        {
                                            ChildNode.ContextMenuStrip = contextMenuStrip4; foreach (TreeNode SubNode in ChildNode.Nodes)
                                            {
                                                SubNode.ContextMenuStrip = contextMenuStrip5;
                                            }
                                        }
                                    }
                                }
                                else if (str.StartsWith("crweight: "))
                                {
                                    substr = str.Substring("crweight: ".Length, str.Length - "crweight: ".Length);
                                    crs = substr.Split(',');
                                    for (int se = 0; se < crs.Count(); se++)
                                    {
                                        try
                                        {
                                            if (crs[se] != null)
                                            {
                                                crweight[C, S, se] = Convert.ToSingle(crs[se].Trim());
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                                else if (str.StartsWith("endSub: "))
                                {

                                    L = 0;
                                    if (S > MaxSub - 1)
                                    {
                                        S = 0;
                                    }
                                    first = false;
                                }
                                else if (str.StartsWith("crdesc: "))
                                {
                                    fl = true;
                                    while (!str.StartsWith("enddesc:"))
                                    {
                                        str = sw.ReadLine();

                                        if (!str.StartsWith("enddesc:"))
                                        {
                                            if (!fl)
                                            { nl = System.Environment.NewLine; }
                                            else
                                            { nl = ""; fl = false; }
                                            crdesc[C, S] = crdesc[C, S] + nl + str;
                                        }
                                    }
                                }
                                else if (str.StartsWith("grade: "))
                                {
                                    substr = str.Substring("grade: ".Length, str.Length - "grade: ".Length);
                                    grcrtitle[C, S, L] = substr.Trim();
                                    if (first)
                                    {
                                        listBox1.Items.Add(substr);
                                    }
                                    L++;
                                }
                                else if (str.StartsWith("grcr: "))
                                {
                                    fl = true;
                                    while (!str.StartsWith("endgrcr:"))
                                    {
                                        str = sw.ReadLine();
                                        if (!str.StartsWith("endgrcr:"))
                                        {
                                            if (!fl)
                                            { nl = System.Environment.NewLine; }
                                            else
                                            { nl = ""; fl = false; }
                                            grcr[C, S, L - 1] = grcr[C, S, L - 1] + nl + str;
                                        }
                                    }
                                }
                                else if (str.StartsWith("gradefb: "))
                                {
                                    fl = true;
                                    while (!str.StartsWith("endgrcrfb:"))
                                    {
                                        str = sw.ReadLine();

                                        if (!str.StartsWith("gradefb:"))
                                        {
                                            if (!fl)
                                            { nl = System.Environment.NewLine; }
                                            else
                                            { nl = ""; fl = false; }
                                            if (!str.StartsWith("endgrcrfb:"))
                                            {
                                                grcrfb[C, S, L - 1] = grcrfb[C, S, L - 1] + nl + str;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (C > MaxCriteria - 1)
                        {
                            break;
                        }
                    }
                    //X = C;
                    CritZ = C;
                    sw.Close();
                    SetUpCriteriaType(CriteriaType);
                    CriteriaFile = filename;
                    CriteriaPath = Path.GetDirectoryName(filename);
                }
                if (listBox1.Items.Count > 0)
                {
                    listBox1.SelectedIndex = 0;
                    CurrentlySelected = 0;
                }
                loading = false;
                Copy_Criteria_Data();
            }//try
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
                loading = false;
            }
        }//readfrom file


        private void Copy_Criteria_Data()
        {
            //copy form data to peer and mod forms etc.
            assessTBox.Text = assess.Title;
            ATitleBox.Text = assess.Title;
            ACodeBox.Text = assess.Code;
            assessCBox.Text = assess.Code;
            assessWeightBox.Text = assess.Weight;
            AWeightBox.Text = assess.Weight;
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {

            ReadCriteriaFromFile(openFileDialog2.FileName, true);
            updatePeerForms();
        }

        private void updatePeerForms()
        {
            //update peer and mod forms etc.

        }

        private void saveFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            SaveCriteriaToFile(saveFileDialog2.FileName);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nl = System.Environment.NewLine;
            try
            {   //if changing from tab 1 to 2 ensure that first grade is selected
                if (tabControl1.SelectedIndex == 1)
                {
                    if (listBox1.Items.Count > 0)
                    {
                        listBox1.SetSelected(0, true);
                        gradelbl.Text = listBox1.SelectedItem.ToString() + ":";
                        fblbl.Text = gradelbl.Text;
                    }
                }
                if (tabControl1.SelectedTab.Text == "Assess" || tabControl1.SelectedTab.Text == "Sessions") //assess and sessions tabs
                {
                    if (treeView1.Nodes[0].Nodes.Count < 2)
                    {
                        MessageBox.Show("Warning no grades loaded - see Grades tab");
                    }
                    defaultdirassesslabel.Text = "Default directory: " + DefaultDir;
                    unitlabel.Text = "Unit: " + UnitTitletextBox.Text;
                }

                /*if (tabControl1.SelectedIndex == 1)
                {
                    listBox3.Items.Clear();
                    for (int i =0; i < listBox2.Items.Count; i++)
                    {                      
                        listBox3.Items.Add(listBox2.Items[i]);
                        assess.LOs = assess.LOs + nl + listBox2.Items[i];
                    }
                }*/

            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }



        private void contextMenuStrip5_Click(object sender, EventArgs e)
        {
            if (EditStudent) { MessageBox.Show("Cannot do this whilst editing student"); return; }
            //edit or delete criteria, or add sub-criteria:
            if (contextMenuStrip5.Items[0].Selected)
            {
                Editable_Criteria(true);
                firstcount = true;
                //EditMode = true;

            }
            else if (contextMenuStrip5.Items[1].Selected)
            {
                //remove the node (gets selected node from treeview1_NodeMouseClick):
                DialogResult dialogResult = MessageBox.Show("Delete Yes/No?", "Remove Sub-Criteria", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    treeView2.Nodes[0].Nodes[SCriteria].Nodes[SSub].Remove();
                    CriteriaExists[SCriteria, SSub] = false;
                }
            }
            else if (contextMenuStrip5.Items[2].Selected)
            {
                //replicate criteria description
                replicate_Description = true;
                treeView2.Enabled = true;
                SelNode = treeView2.SelectedNode;
                PreNode = SelNode;
                SelSub = SSub;
                if (CriteriaSelected)
                {
                    SelSub = MaxSub;
                }
                SelCr = SCriteria;
                repCancelbutton4.Visible = true;
                MessageBox.Show("Select criteria description to replicate to");
            }
            else if (contextMenuStrip5.Items[3].Selected)
            {
                //replicate criteria
                replicate_Criteria = true;
                treeView2.Enabled = true;
                SelNode = treeView2.SelectedNode;
                PreNode = SelNode;
                SelSub = SSub;
                if (CriteriaSelected)
                {
                    SelSub = MaxSub;
                }
                SelCr = SCriteria;
                repCancelbutton1.Visible = true;
                MessageBox.Show("Select criteria to replicate to");
            }
            else if (contextMenuStrip5.Items[4].Selected)
            {
                //replicate feedback
                replicate_Feedback = true;
                treeView2.Enabled = true;
                SelNode = treeView2.SelectedNode;
                PreNode = SelNode;
                SelSub = SSub;
                if (CriteriaSelected)
                {
                    SelSub = MaxSub;
                }
                SelCr = SCriteria;
                repCancelbutton2.Visible = true;
                MessageBox.Show("Select criteria to replicate to");
            }
            else if (contextMenuStrip5.Items[5].Selected)
            {
                //replicate LO
                replicate_LO = true;
                treeView2.Enabled = true;
                SelNode = treeView2.SelectedNode;
                PreNode = SelNode;
                SelSub = SSub;
                if (CriteriaSelected)
                {
                    SelSub = MaxSub;
                }
                SelCr = SCriteria;
                repCancelbutton3.Visible = true;
                MessageBox.Show("Select criteria to replicate to");
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            DialogResult dialogResult;
            try
            {
                    if (listBox1.Items.Count > 0)
                    {
                       
                        if (TChanged)
                        {
                            if (promptsoff)
                            {
                                dialogResult = DialogResult.Yes;
                            }
                            else
                            {
                                dialogResult = MessageBox.Show("Save Feedback for grade Yes/No?", "Grade Feedback", MessageBoxButtons.YesNo);
                            }
                            if (dialogResult == DialogResult.Yes)
                            {
                                grcr[SCriteria, SSub, PrevIndex] = textBox7.Text; //textbox7 is text box containing "criteria to achieve grade"
                                grcrfb[SCriteria, SSub, PrevIndex] = textBox8.Text;
                                TChanged = false;
                            }
                        }
                        if (listBox1.SelectedIndex < 0)
                        {
                            listBox1.SelectedIndex = 0;
                            CurrentlySelected = 0;
                        }
                        if (CriteriaSelected)
                        {
                            i = MaxSub;
                        }
                        else
                        {
                            i = SSub;
                        }
                        TChanged = false;
                        if (singleGrades)
                        {
                            textBox7.Text = grcr[SCriteria, i, listBox1.SelectedIndex];
                            textBox8.Text = grcrfb[SCriteria, i, listBox1.SelectedIndex];

                            gradelbl.Text = listBox1.GetItemText(listBox1.SelectedItem);
                        }
                        else
                        {
                            textBox7.Text = grcr[SCriteria, i, aliaslist[listBox1.SelectedIndex].grindex];
                            textBox8.Text = grcrfb[SCriteria, i, aliaslist[listBox1.SelectedIndex].grindex];
                            gradelbl.Text = listBox1.GetItemText(aliaslist[listBox1.SelectedIndex].grindex);
                        }
                        fblbl.Text = gradelbl.Text;
                        fbstring[listBox1.SelectedIndex] = fblbl.Text;


                        //label18.Text = listBox1.SelectedItem.ToString();
                        PrevIndex = listBox1.SelectedIndex;

                    }
                    else
                    {
                        MessageBox.Show("No Grades - load a grade file!");
                    }
                    if (listBox1.SelectionMode == SelectionMode.MultiSimple)
                    {
                        //SaveListbox1Selected();
                    }                                
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e) //textbox7 is text box containing "criteria to achieve grade"
        {
            if (EditCriteria)
            {
                TChanged = true;
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (EditCriteria) { TChanged = true; }
        }

        private void menuStrip2_Click(object sender, EventArgs e)
        {

        }

       /* private void button3_Click(object sender, EventArgs e)
        {   //edit criteria or mark student
            if (button3.Text.StartsWith("Edit"))
            {
                if (treeView1.Nodes[0].Nodes.Count < 2 || treeView1.Nodes[0].Nodes.Count < listBox1.Items.Count)
                {
                    MessageBox.Show("Grading schema doesn't match grades in criteria - check Grades tab");
                    return;
                }
                selectLOs = false;
                replicate_Criteria = false;
                replicate_Feedback = false;
                replicate_LO = false;
                if (listBox1.Items.Count > 0)
                {
                    if (SessionType > 0)
                    {
                        if (SessionS < 1)
                        {
                            MessageBox.Show("You have selected Multiple Sessions, but you don't have any Sessions - see Session tab");
                            return;
                        }
                        else if (SessionS == 1)
                        {
                            MessageBox.Show("Note: you have selected multiple sessions, but you only have one session!");
                        }
                    }
                    button3.Text = "Marking Mode";
                    MarkingMode(true);
                    EditStudent = true;                 
                }
                else
                {
                    MessageBox.Show("You need some Grades to start marking!");
                }
            }
            else
            {
                button3.Text = "Edit Criteria Mode";
                MarkingMode(false);
                EditStudent = false;                
                //button1.Visible = false;  //show button
                Show_Label("Don't forget to Save Marks!", 2000);
            }

        }*/

        private void button3_Click(object sender, EventArgs e)
        {   //edit criteria or mark student (or generate criteria)
            if (button3.Text.StartsWith("Gen") || (button3.Text.StartsWith("Edit") && !showGenAssessToolStripMenuItem.Checked))
            {
                highlightButton.Visible = false;
                Ticklabel.Visible = false;
                SaveListbox1Selected();
                listBox1.SelectionMode = SelectionMode.One;
                try
                {
                    listBox1.SelectedIndex = CurrentlySelected;
                }
                catch
                {
                    listBox1.SelectedIndex = -1;
                }
                
                if (treeView1.Nodes[0].Nodes.Count < 2 || treeView1.Nodes[0].Nodes.Count < listBox1.Items.Count)
                {
                    MessageBox.Show("Grading schema doesn't match grades in criteria - check Grades tab");
                    

                    DialogResult ret = MessageBox.Show("Do you want to copy grades from Grades tab - yes/no?", "Copy grades", MessageBoxButtons.YesNo);
                    if (ret == DialogResult.Yes)
                    {
                        Copy_GradestoAssess();
                        Show_Label("Now don't forget to Save updated Assessment before continuing!", 2000);                                         
                    }
                    //go back to save assessment:
                    button3.Text = "Edit Criteria Mode";
                    Show_GenTemplate(false);
                    MarkingMode(false);
                    EditStudent = false;
                    return; //user needs to save the assessment now before continuing
                }
                selectLOs = false;
                replicate_Criteria = false;
                replicate_Feedback = false;
                replicate_LO = false;
                if (listBox1.Items.Count > 0)
                {
                    if (SessionType > 0)
                    {
                        if (SessionS < 1)
                        {
                            MessageBox.Show("You have selected Multiple Sessions, but you don't have any Sessions - see Session tab");
                            return;
                        }
                        else if (SessionS == 1)
                        {
                            MessageBox.Show("Note: you have selected multiple sessions, but you only have one session!");
                        }
                    }
                    button3.Text = "Marking Mode";
                    Show_GenTemplate(false);
                    MarkingMode(true);
                    EditStudent = true;
                }
                else
                {
                    MessageBox.Show("You need some Grades to start marking!");
                }
                
            }
            else if (button3.Text.StartsWith("Marking"))
            {
                highlightButton.Visible = false;
                Ticklabel.Visible = false;
                //SaveListbox1Selected();
                listBox1.SelectionMode = SelectionMode.One;
                try
                {
                    listBox1.SelectedIndex = CurrentlySelected;
                }
                catch
                {
                    listBox1.SelectedIndex = -1;
                }
                button3.Text = "Edit Criteria Mode";
                Show_GenTemplate(false);
                MarkingMode(false);
                EditStudent = false;
                //button1.Visible = false;  //show button
                Show_Label("Don't forget to Save Marks!", 2000);
               
            }
            else if (showGenAssessToolStripMenuItem.Checked ) //if editting and now switch to generating assessment mode
            {
                highlightButton.Visible = true;
                Ticklabel.Visible = true;
                listBox1.SelectionMode = SelectionMode.MultiSimple;
                RecoverSelected();
                button3.Text = "Gen Assess Mode";
                try
                {
                    listBox1.SelectedIndex = 0;
                    //listBox1.SelectedIndex = -1;
                }
                catch
                {
                    listBox1.SelectedIndex = -1;
                }
                Show_GenTemplate(true);
                EditStudent = false;
               
            }

        }
        private void Show_GenTemplate(bool b) //show template generation buttons and boxes?
        {
            generateButton.Visible = b;
            templatelabel.Visible = b;
            templatetextBox.Visible = b;
            templatebutton.Visible = b;
            Gradegrouphelplabel.Visible = b;
            //gradeDirectioncheckBox.Visible = b;
        }
        private void MarkingMode(bool b)
        {
            //textBox9.Visible = b;
            StudentcomboBox.Visible = b;
            label15.Visible = b;
            label16.Visible = b;
            label18.Visible = b;
            contextMenuStrip4.Enabled = !b;
            contextMenuStrip3.Enabled = !b;
            contextMenuStrip5.Enabled = !b;
            toolStripMenuItem1.Enabled = !b;
            settingsToolStripMenuItem.Enabled = !b;
            LOtoolStripMenuItem2.Enabled = !b;
            promptsToolStripMenuItem.Enabled = !b;
           
            
            //menuStrip2.Enabled = !b;
            if (startMark)
            {
                button4.Visible = b;
                button5.Visible = b;
                button6.Visible = b;
                addButton.Visible = b;
                //textBox10.Enabled = true;
                ImportcheckBox.Visible = true;
               
            }
            else
            {
                button4.Visible = false;
                button5.Visible = b;
                button6.Visible = false;
                addButton.Visible = false;
                //textBox10.Enabled = false;
                ImportcheckBox.Visible = false;
                
            }
            ImportcheckBox.Visible = b;
        
            if (ImportcheckBox.Checked)
            {
                importGroupBox.Visible = b;
                importCalcLabel.Visible = b;
            }
            textBox10.Visible = b;
            Clicklabel1.Visible = b;
            markModeButton.Visible = b;
            markModelabel.Visible = b;
            sittingButton.Visible = b;
            sittinglabel.Visible = b;
            modSelect.Visible = b;

            label20.Visible = b;
            
            label85.Visible = b;
            label21.Visible = false;
            label22.Visible = false;
            label16.Visible = b;
            label18.Visible = b;
            button7.Visible = b;
            assessTitleBox.ReadOnly = b;
           
            label30.Visible = b;
            if (SessionType > 0 || Session > 1)
            {
                copyWeightbutton.Visible = !b;
            }
            else
            {
                copyWeightbutton.Visible = false;
            }
            if (UseChecked)
            {
                treeView2.CheckBoxes = b;
                treeView2.Nodes[0].ExpandAll();
            }
            if (SessionType == 0)
            {
                //checkWbutton.Visible = !b;
            }

            if (allowoverride) //of % in individual criteria
            {
                overrideBox.Visible = b;
                overrideButton.Visible = b;
            }
            else
            {
                overrideBox.Visible = false;
                overrideButton.Visible = false;
            }
            overridecheckBox.Visible = b; //override overall grade
            if (b && overridecheckBox.Checked)
            {
                Overriedlabel.Visible = true;
                OverrideGradelabel.Visible = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            GradeTitleChanged = true;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {   //double click the grade list box to select grade for currenet criteria
            int sub = 0;
            if (startMark)
            {
                try
                {
                    if (EditStudent)
                    {
                        button7.Visible = true;
                    }
                    if (listBox1.SelectedIndex > -1)
                    {
                        if (overridecheckBox.Checked) //override overall grade
                        {
                            OverrideGradelabel.Text = listBox1.SelectedItem.ToString();
                            return;
                        }
                        if (CriteriaSelected)
                        {
                            sub = MaxSub;
                        }
                        else
                        { sub = SSub; }
                        if (!DeselectSessioncheckBox.Checked)
                        {
                            Marks[SCriteria, sub, Session] = listBox1.SelectedItem.ToString();
                            if (EditStudent)
                            {
                                if (crSelected[SCriteria, sub, Session])
                                {
                                    label18.Text = listBox1.SelectedItem.ToString();
                                }
                                else
                                {
                                    label18.Text = "n/a";
                                }

                            }
                        }
                    }
                }
                catch
                {
                }
                    
            }
        }

        private void button4_Click(object sender, EventArgs e)      //save button
        {   //save student mark
            string addstr = "";
            if (addCode)
            {
                addstr = "_" + assess.Code;
            }
                                 
            if (marksDirectory.Length < 1)
            {
                marksDirectory = UnitFilePath;
            }
            string str = marksDirectory + "\\" + assess.Code;
            if (!Directory.Exists(marksDirectory))
            {   //check if you've lost connection to online resource nad if so save locally instead temporarily
                MessageBox.Show("Unable to save - check path or connection to: " + marksDirectory);
                DialogResult ret = MessageBox.Show("Save marks temporarily in: " + ConfigDir + " Yes/No?", "Save marks", MessageBoxButtons.YesNo);
                if (ret == DialogResult.Yes)
                {
                    if (unitCodeTextBox.Text.Trim() == "" || unitCodeTextBox.Text == null)
                    {
                        marksDirectory = ConfigDir + "Temp" + "\\" + assess.Code;
                    }
                    else
                    {
                        marksDirectory = ConfigDir + unitCodeTextBox.Text + "\\" + assess.Code;
                    }
                    str = marksDirectory;
                }
                else
                {
                    return;
                }                
            }                       
            
            saveFileDialog3.InitialDirectory = str;
            if (!Directory.Exists(str))
            {
                try
                {
                    Directory.CreateDirectory(str);
                    assessHeaderlabel.Text = "Assessments mark in: " + str;
                }
                catch
                {
                    
                }
            }
            DialogResult dialogResult3 = DialogResult.Yes;
            if (listBox1.Items.Count < 1)
            {
                dialogResult3 = MessageBox.Show("No grades are loaded - still save Yes/No?", "Save marks", MessageBoxButtons.YesNo);
            }

            if (dialogResult3 == DialogResult.Yes)
            {
                if (StudentcomboBox.Text.Trim().Length > 0)
                {

                    saveFileDialog3.FileName = "";
                    if (modSelect.Checked)
                    {
                        if (MarkMode == 0)
                        {
                            saveFileDialog3.Filter = "Moderated Marked files (.mrm)|*.mrm";
                            saveFileDialog3.DefaultExt = "mrm";
                        }
                    }
                    else
                    {
                        if (MarkMode == 0)
                        {
                            saveFileDialog3.Filter = "Marked files (.mrk)|*.mrk";
                            saveFileDialog3.DefaultExt = "mrk";
                        }
                        else if (MarkMode == 1)
                        {
                            saveFileDialog3.Filter = "2nd Marked files (.2nd)|*.2nd";
                            saveFileDialog3.DefaultExt = "2nd";
                        }
                        else if (MarkMode == 2)
                        {
                            saveFileDialog3.Filter = "3rd Marked files (.3rd)|*.3rd";
                            saveFileDialog3.DefaultExt = "3rd";
                        }
                    }
                    if (Sitting != "Main")
                    {
                        saveFileDialog3.FileName = StudentcomboBox.Text + addstr + " " + sittingButton.Text;
                    }
                    else 
                    {
                        saveFileDialog3.FileName = StudentcomboBox.Text + addstr;
                    }

                    saveFileDialog3.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Student name blank");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //load marked files
            DialogResult dialogResult = DialogResult.Yes;
            if (marksDirectory.Length < 1)
            {
                if (Directory.Exists(UnitFilePath))
                {
                    marksDirectory = UnitFilePath;
                }
                else
                {
                    marksDirectory = DefaultDir;  
                }
            }
         
            openFileDialog3.FileName = "";
            openFileDialog3.InitialDirectory = marksDirectory + "\\" +assess.Code;
            if (StudentcomboBox.Text.Trim().Length > 0)
            {
                dialogResult = MessageBox.Show("Clear form data and load marks from file Yes/No?", "Load marks", MessageBoxButtons.YesNo);

            }
            if (dialogResult == DialogResult.Yes)
            {
                overrideBox.Text = "";
                if (MarkMode == 0)
                {
                    openFileDialog3.DefaultExt = "mrk";
                    openFileDialog3.Filter = "Marked files (*.mrk) |*.mrk|Moderated Marked files (*.mrm) |*.mrm";
                }
                else if (MarkMode == 1)
                {
                    openFileDialog3.DefaultExt = "2nd";
                    openFileDialog3.Filter = "2nd Marked files (*.2nd) |*.2nd";
                }
                else if (MarkMode == 2)
                {
                    openFileDialog3.DefaultExt = "3rd";
                    openFileDialog3.Filter = "3rd Marked files (*.3rd) |*.3rd";
                }
                openFileDialog3.FileName = "";

                openFileDialog3.ShowDialog();
            }
        }

        private void openFileDialog3_FileOk(object sender, CancelEventArgs e)
        {  //reuse openfiledialog3 for two purposes
            string str = openFileDialog3.DefaultExt;
            if (str == "mrk" || str == "mrm" || str == "2nd" || str == "3rd") //if marked files
            {
                Load_Marked_File(openFileDialog3.FileName);
                assessHeaderlabel.Text = Path.GetDirectoryName(openFileDialog3.FileName);
            }
            else
            {
                Load_Sessions(openFileDialog3.FileName);
            }
        }

        private void saveFileDialog3_FileOk(object sender, CancelEventArgs e)
        {   //saved marked student work
            if (saveFileDialog3.DefaultExt == "mrk")
            {
                Save_Marked_File(saveFileDialog3.FileName);
                assessHeaderlabel.Text = "Assessments mark in: " + Path.GetDirectoryName(saveFileDialog3.FileName);
            }
            else
            {
                Save_Sessions(saveFileDialog3.FileName);
            }
            savedStudent = true;
           
        }

        private void Save_Marked_File(string filename)
        {   //save students marks
            string PC;
            string str;
            int Se = 0;
            float fl = 0;
            float f = 0;
            string des = "false";
            string ctype = "0";
            bool nullText = false;
            string D = "";
            try
            {
                if (listBox1.Items.Count > 0)
                {
                    Calculate_Checks();
                    using (StreamWriter sw = new StreamWriter(filename))
                    {
                        if (SessionS == 0)
                        {
                            Se = 1;
                        }
                        else
                        { 
                            Se = SessionS;
                        }
                        sw.WriteLine("Unit: " + UnitTitletextBox.Text);
                        sw.WriteLine("Assessment title: " + assess.Title);
                        sw.WriteLine("Student: " + StudentcomboBox.Text);
                        try
                        {
                            ctype = CriteriaType.ToString();
                        }
                        catch
                        {
                            ctype = "0";
                        }
                        sw.WriteLine("Sitting: " + Sitting);
                        sw.WriteLine("Criteria type: " + ctype);
                        sw.WriteLine("Criteria path: " + CriteriaPath);
                        sw.WriteLine("Criteria file: " + CriteriaFile);
                        sw.WriteLine("Grade Path: " + GradePath);
                        sw.WriteLine("Grade file: " + GradeFile);
                        sw.WriteLine("Session path: " + SessionPath);
                        sw.WriteLine("Session file: " + SessionFile);
                        for (int s = 0; s < Se; s++)
                        {
                            if (SessionS > 0)
                            {
                                sw.WriteLine("----------------------------------------");
                                if (DeselectSession[s])
                                {
                                    D = " des";
                                }
                                else
                                {
                                    D = "";
                                }
                                sw.WriteLine("Session: " + Convert.ToString(s + 1) + D);
                                sw.WriteLine("Session title: " + SessionTitle[s]);
                                sw.WriteLine("----------------------------------------");
                               
                                
                            }
                            else
                            {
                                sw.WriteLine("Session: 0");
                            }
                            for (int i = 0; i < CritZ + 1; i++)
                            {

                                int j = MaxSub;
                                if (crSelected[i, j, s])
                                {
                                    if ((crtitle[i, j] == null || crtitle[i, j].Length < 1))
                                    {
                                        nullText = true;
                                    }
                                    else { nullText = false; }
                                    if (!nullText)
                                    {
                                        sw.WriteLine("Criteria: " + Convert.ToString(i + 1));
                                        sw.WriteLine("Criteria title: " + crtitle[i, j]);
                                       
                                        bool Has;
                                        Has = Has_Subcriteria(i);
                                        if (!Has && Aggregation)
                                        {
                                            Has = true;
                                        }
                                        if (!Aggregation || (Aggregation && Has))
                                        {
                                            sw.WriteLine("Grade: " + Marks[i, j, s]);
                                            sw.WriteLine("Criteria comment: ");
                                            sw.WriteLine(crComment[i, j, s]);
                                            sw.WriteLine("endComment:");
                                            PC = Find_Percent(Marks[i, j, s]);
                                            sw.WriteLine("Percentage: " + PC);
                                        }
                                        else
                                        {
                                            sw.WriteLine("Aggregated:");
                                        }

                                    }
                                }//if criteria selected
                                else
                                {
                                    /*try
                                    {
                                        sw.WriteLine("Criteria: " + Convert.ToString(i + 1));
                                        sw.WriteLine("Criteria title: " + crtitle[i, j]);
                                        sw.WriteLine("Grade: n/a");
                                    }
                                    catch { }*/

                                    /*if (CriteriaSelectionType > 1) //if allow individual criteria deselection
                                    {
                                        sw.WriteLine("Criteria DeSelect: " + Convert.ToString(i + 1));
                                    }*/
                                }

                                for (int k = 0; k < MaxSub; k++)
                                {
                                    if (crSelected[i, k, s])
                                    {
                                        if ((crtitle[i, k] == null || crtitle[i, k].Length < 1))
                                        {
                                            nullText = true;
                                        }
                                        else { nullText = false; }
                                        if (!nullText)
                                        {
                                            sw.WriteLine("Sub-Criteria: " + Convert.ToString(k + 1));
                                            sw.WriteLine("Sub-criteria title: " + crtitle[i, k]);
                                            sw.WriteLine("Grade: " + Marks[i, k, s]);
                                            sw.WriteLine("Criteria comment: ");
                                            sw.WriteLine(crComment[i, k, s]);
                                            sw.WriteLine("endComment:");
                                            PC = Find_Percent(Marks[i, k, s]);
                                            sw.WriteLine("Percentage: " + PC);                                           
                                            
                                        }
                                    }
                                    else
                                    {
                                        /*try
                                        {
                                            sw.WriteLine("Sub-Criteria: " + Convert.ToString(k + 1));
                                            sw.WriteLine("Sub-criteria title: " + crtitle[i, k]);
                                            sw.WriteLine("Grade: n/a");
                                        }
                                        catch { }*/
                                        /* if (CriteriaSelectionType > 1) //if allow individual criteria deselection
                                         {
                                             sw.WriteLine("Criteria DeSelect: " + Convert.ToString(i + 1));
                                         }*/
                                    }
                                }
                            }

                            fl = Generate_Overall_Mark(s);

                            if ((SessionType > 0) && (!SessionsEqual)) //if multisession and sessions are weighted
                            {
                                float w = Convert.ToSingle(SessionWeight[s]);
                                f = f + (fl * (w / 100));   //apply session weight
                            }
                        } //sessions s

                        if (SessionType == 0)
                        {
                            f = fl;

                        }
                        else if (SessionsEqual)
                        {
                            f = f / SessionS; //only if sessions are equally weighted
                        }
                        if (overridecheckBox.Checked) //if allowed to override whole grade for assessment
                        {
                            if (OverrideGradelabel.Text.Trim().LastIndexOf("%") == OverrideGradelabel.Text.Length -1)
                            {
                                PC = OverrideGradelabel.Text.TrimEnd('%');
                            }
                            else
                            {
                                PC = Find_Percent(OverrideGradelabel.Text.Trim());
                            }
                            f = Convert.ToSingle(PC);
                        }
                        sw.WriteLine();
                        str = Convert.ToString(f);
                        sw.WriteLine("Overall mark: " + str);
                        label22.Text = str;
                        label22.Visible = true;
                        label21.Visible = true;
                        sw.WriteLine("Equivalent grade: " + Convert_Percent_To_Grade(f));
                        string org = OverrideGradelabel.Text.Trim();
                        if (!overridecheckBox.Checked)
                        {
                            org = "";
                        }
                        sw.WriteLine("ORG: " + org);
                        sw.WriteLine();

                        sw.WriteLine("General feedback: ");
                        sw.WriteLine(textBox10.Text);
                        sw.WriteLine("endFeedback:");
                        sw.WriteLine("");


                        sw.Close();
                    }// using
                } //if listbox
                else
                {
                    MessageBox.Show("No grades loaded - cannot save blank marks!");
                }

            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void Load_Marked_File(string filename)
        {

            int i = 0;
            int j = 0;
            int s = 0;
            string nl, str, str3;
            bool fl;
            string[] str2 = new string[2];
            Found = 0;
            Feedback = "";
            str3 = "";
            bool noerror = true;
            bool moresessions = true;
            string comment = "";
            label18.Visible = true;
            label18.Text = "";
            string tempFeedback = "";
            firstS = true;
            bool foundgrade = false;
            bool foundcriteria = false;
            bool foundsession = false;
            
            loading = true;
            try
            {
                /*if (listBox1.Items.Count < 1)
                {
                    
                    MessageBox.Show("Warning - cannot load student file - have you loaded a grade file?");
                }
                else
                {
                 * */
                Clear_Form_Data();
                Reset_Selected(false);
               
                using (StreamReader sw1 = new StreamReader(filename))
                {    //find files first - just to make sure that marks are read in properley!
                    // ovecomes a bug if filenames are placed at end of file! slows it down but betetr to be safe than fast
                    while (!sw1.EndOfStream)
                    {

                        str = sw1.ReadLine();
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
                            if (str.StartsWith("Sitting:"))
                            {                             
                                try
                                {
                                   Sitting = str3;
                                    sittingButton.Text = str3;
                                }
                                catch
                                {
                                    MessageBox.Show("Error reading sitting");
                                }
                            }
                            if (str.StartsWith("Grade file:"))
                            {
                                try
                                {
                                    foundgrade = true;
                                    if (listBox1.Items.Count < 1)
                                    {
                                        if (File.Exists(str3))
                                        {
                                            ReadGradesFromFile(str3);
                                        }
                                    }
                                }
                                catch
                                {
                                    MessageBox.Show("Error reading grade file");
                                }
                            }

                            else if (str.StartsWith("Criteria file:"))
                            {
                                try
                                {
                                    foundcriteria = true;
                                    if (File.Exists(str3))
                                    {
                                        ReadCriteriaFromFile(str3, true);
                                    }

                                }
                                catch
                                {
                                    MessageBox.Show("Error reading assessment file - ensure that correct assessment file is loaded to match these marks!");
                                }
                            }

                            else if (str.StartsWith("Session file:"))
                            {
                                try
                                {
                                    foundsession = true;
                                    if (File.Exists(str3))
                                    {
                                        Load_Sessions(str3);
                                        Set_Session(true);
                                    }

                                }
                                catch
                                {
                                    MessageBox.Show("Error reading session file - ensure that correct session file is loaded!");
                                }
                            }
                            if (str.StartsWith("Criteria file: "))
                            {
                            }
                        }
                        if (foundgrade && foundcriteria && foundsession)
                        {
                            break;
                        }

                    }
                    sw1.Close();
                }
                // Create an instance of StreamWriter to read marks from file:
                using (StreamReader sw = new StreamReader(filename))
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
                                //str2 = str.Split(':');
                                //str3 = str2[1].Trim();
                            }
                            else
                            {
                                str3 = str;
                            }

                        }
                        if (str.StartsWith("Session: "))
                        {
                            s = 0;
                            if (str3.Equals("0"))
                            {
                                s = 0;
                            }
                            else
                            {
                                try
                                {
                                    int index = 0;
                                    if (str3.Contains("des"))
                                    {
                                        index = str3.IndexOf("des");
                                        str3 = str3.Substring(0,index);
                                    }
                                   
                                    s = Convert.ToInt32(str3);
                                    if (index > 0)
                                    {
                                        DeselectSession[s-1] = true;
                                    }
                                    else
                                    {
                                        DeselectSession[s-1] = false;
                                    }
                                    if (s == 1)
                                    {
                                        DeselectSessioncheckBox.Checked = DeselectSession[s-1];
                                    }
                                }
                                catch
                                {
                                }
                                if (s > SessionS)
                                {
                                    moresessions = true;

                                    noerror = false;
                                    SessionS = s;
                                }
                                else if (s < SessionS)
                                {
                                    moresessions = false;
                                    noerror = false;

                                }
                                else
                                {
                                    noerror = true;
                                }

                                s--;
                            }
                        }
                        if (str.StartsWith("Student: "))
                        {
                            StudentcomboBox.Text = str3;
                        }
                        if (str.StartsWith("Unit: "))
                        {
                            // do nothing at this stage
                        }
                        if (str.StartsWith("Assessment title: "))
                        {
                            // do nothing at this stage
                        }
                        else if (str.StartsWith("Criteria type:"))
                        {

                            try
                            {
                                CriteriaType = Convert.ToInt32(str3);
                                SetUpCriteriaType(CriteriaType);
                            }
                            catch
                            {
                                Show_Label("No matching criteria type - see Settings", 2000);
                            }

                        }

                        else if (str.StartsWith("Criteria title:"))
                        {


                            TreeNodeCollection nodes = treeView2.Nodes;
                            foreach (TreeNode ChildNode in nodes)
                            {
                                FindRecursive(ChildNode, str3);
                                i = Found;
                            }
                            j = MaxSub;


                        }
                        else if (str.StartsWith("Grade:"))
                        {
                            Marks[i, j, s] = str3;
                            crSelected[i, j, s] = true;
                        }
                        else if (str.StartsWith("Criteria DeSelect"))
                        {
                            if (CriteriaSelectionType > 1) //if allow individual criteria deselection
                            {
                                crSelected[i, j, s] = false;
                            }
                        }
                        else if (str.StartsWith("Aggregated:"))
                        {
                            Aggregation = true;
                        }
                        else if (str.StartsWith("Percentage:"))
                        {

                            try
                            {
                                Percents[i, j] = Convert.ToSingle(str3);
                            }
                            catch
                            {
                            }

                        }
                        else if (str.StartsWith("Criteria comment:"))
                        {
                            comment = "";
                            fl = true;
                            while (!str.StartsWith("endComment:"))
                            {
                                str = sw.ReadLine();

                                if (!str.StartsWith("endComment:"))
                                {
                                    if (!fl)
                                    { nl = System.Environment.NewLine; }
                                    else
                                    { nl = ""; fl = false; }
                                    comment = comment + nl + str;
                                }
                            }
                            crComment[i, j, s] = comment;
                        }
                        else if (str.StartsWith("Sub-criteria title:"))
                        {

                            TreeNodeCollection nodes = treeView2.Nodes;
                            foreach (TreeNode ChildNode in nodes)
                            {
                                foreach (TreeNode SubNode in ChildNode.Nodes)
                                {
                                    FindRecursive(SubNode, str3);
                                    j = Found;
                                }
                            }
                        }
                        else if (str.StartsWith("General feedback:"))
                        {
                            fl = true;
                            while (!str.StartsWith("endFeedback:"))
                            {
                                str = sw.ReadLine();

                                if (!str.StartsWith("endFeedback:"))
                                {
                                    if (!fl)
                                    { nl = System.Environment.NewLine; }
                                    else
                                    { nl = ""; fl = false; }
                                    tempFeedback = tempFeedback + nl + str;
                                }
                            }

                        }
                        else if (str.StartsWith("Overall mark:"))
                        {
                            label22.Text = str3;
                            label22.Visible = true;
                            label21.Visible = true;
                        }
                        else if (str.StartsWith("ORG"))
                        {
                            if (str3 != null && str3.Length > 0)
                            {
                                overridecheckBox.Checked = true;
                                OverrideGradelabel.Visible = true;
                                OverrideGradelabel.Text = str3;
                                Overriedlabel.Visible = true;
                            }
                        }

                    }

                    if (!noerror)
                    {
                        if (moresessions)
                        {
                            MessageBox.Show("Warning - more sessions in sessions \nfile than in marked file \n- need to mark the extra session or delete it", "Session Warning");
                        }
                        else
                        {
                            MessageBox.Show("Warning - more sessions in marked file \nthan in sessions file \n- need to add a session", "Session Warning");
                        }
                    }
                    sw.Close();
                    //Select_Session();
                    Change_Session_Selection();
                    textBox10.Text = tempFeedback;
                    //Feedback = tempFeedback;
                }




                int x = 0;
                try
                {
                    x = listBox1.FindString(Marks[0, 0, 0], x);
                    if (x < 0 || x >= listBox1.Items.Count)
                    {
                        x = 0;
                    }
                    listBox1.SelectedIndex = x;
                    CurrentlySelected = x;
                }
                catch
                {
                }

                treeView2.SelectedNode = treeView2.Nodes[0];
                //treeView2.TopNode = treeView2.Nodes[0].FirstNode;
                //} //if
                if (listBox1.Items.Count < 1)
                {
                    MessageBox.Show("Warning - cannot load student file - no grade file loaded that matches!");
                    Clear_Form_Data();
                }
                Change_Session_Selection();
                loading = false;
            }//try
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
                loading = false;
            }
        }



        private void FindRecursive(TreeNode treeNode, string s)
        {

            foreach (TreeNode tn in treeNode.Nodes)
            {
                // if the text properties match
                if (tn.Text.Trim() == s.Trim())
                {
                    Found = tn.Index;
                }
                FindRecursive(tn, s);
            }

        }


        private void tabPage2_Click(object sender, EventArgs e)
        {
            firstcount = false;
        }

        /*private void saveFileDialog4_FileOk(object sender, CancelEventArgs e)
        {
            Generate_Feedback_Report(saveFileDialog4.FileName);

        }*/


        private void button6_Click(object sender, EventArgs e) //show report
        {  //report button
            string stn = "";
            string ste = "";
            if (addCode)
            {
                rep1.addstr = "_" + assess.Code;
            }
            else
            {
                rep1.addstr = "";
            }


            if (StudentcomboBox.SelectedIndex > -1)
            {
                if (studentlist[StudentcomboBox.SelectedIndex].studentname != null)
                {
                    stn = studentlist[StudentcomboBox.SelectedIndex].studentname;
                    ste = studentlist[StudentcomboBox.SelectedIndex].studentmail;
                }
            }
            
            if (listBox1.Items.Count > 0)
            {
                try
                {
                    rep1.emailaddress = "";
                    if (stn != null)
                    {
                        if (stn.Length > 0 && stn.Trim() == StudentcomboBox.Text.Trim())
                        {
                            rep1.assessTitle = assess.Title;
                            rep1.unitTitle = UnitTitletextBox.Text;
                            rep1.UnitLeader = ULTextBox.Text;
                            rep1.emailaddress = ste;
                        }
                    }
                }
                catch (System.Exception excep)
                {
                    StackTrace stackTrace = new StackTrace();
                    MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
                    return;
                }
                try
                {
                    if (Generate_Feedback())
                    {
                        rep1.ShowDialog();
                    }
                }
                catch (System.Exception excep)
                {
                    StackTrace stackTrace = new StackTrace();
                    MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);                   
                }
                //saveFileDialog4.FileName = StudentcomboBox.Text;
                //saveFileDialog4.ShowDialog();
            }
            else
            {
                MessageBox.Show("No grades loaded - cannot report on nothing!");
            }
        }

        private void Calculate_Checks()
        {
            //work out treeview checked boxes on criteria
            if (UseChecked)
            {
                for (int i = 0; i < CritZ; i++)
                {
                    for (int j = 0; j < MaxSub + 1; j++)
                    {
                        if (nodeChecked[i, j, Session])
                        {
                            if (yesno)
                            {
                                Marks[i, j, Session] = "100";
                                //listBox1.Items[0].Text;
                            }
                            else
                            {

                            }

                        }
                        else
                        {
                            if (yesno)
                            {
                                Marks[i, j, Session] = "0";
                            }
                            else
                            {
                                Marks[i, j, Session] = null;
                            }
                        }
                    }

                }

            }
        }

        private bool Generate_Feedback()
        {   //generate feedback report for student
            bool nullText = false;
            int x = 0;
            bool firstSession = true;
            //int s = 0;

            string str;
            string str2 = "";

            float f = 0;
            float fl = 0;
            int ST;
            //string nl = System.Environment.NewLine;
            string nl = " \\line ";
            string boldS = "\\b ";
            string boldE = " \\b0 ";
            string italS = "\\i\\f0";
            string italE = "\\i0";
            bool minusone = false;
            string m1 = "";
            int SessionCount = 0;


            FontFamily family = new FontFamily("Calibri");
            Font fontbold = new Font(family, 12.0f, FontStyle.Bold);

            Calculate_Checks();
            if (SessionType == 0)
            {
                ST = 1;
            }
            else
            {
                ST = SessionS;
            }
            try
            {

                rep1.RTB.Text = "";
                str = "{\\rtf1\\ansi"; //start rich text format
                rep1.StuName = StudentcomboBox.Text;

                rep1.RTB.Font = new Font("Calibri", 10, FontStyle.Regular);

                if (feedOptions.includeheader)
                {
                    str = str + boldS + "Unit: " + UnitTitletextBox.Text + boldE + nl;
                    str = str + boldS + "Student name: " + StudentcomboBox.Text + nl + boldE + "Date /time: " + DateTime.Now.ToString("dd/MM/yy  HH:mm") + nl;
                    str = str + boldS + "Assessment title: " + assess.Title + boldE + ", Code: " + assess.Code + nl;
                    str = str + "Iteration: " + Sitting + nl;


                    //richTextBox1.Rtf = @"{\rtf1\ansi \b " + selstr + " \b0 ";
                    //rep1.RTB.Rtf = richTextBox1.Rtf;
                    // str = "{\\rtf1\\ansi \\b " + selstr + " \\b0 ";

                    str = str + "Assessment weight for unit: " + assess.Weight + nl;
                    str = str + "-----------------" + nl;
                    if (feedOptions.full)
                    {
                        str = str + italS + "Description: " + italE + nl + assess.Description.Replace("\r\n", nl) + nl;
                    }
                    if (feedOptions.fullLO)
                    {
                        string lo = assess.LOs;
                        lo = lo.Replace("\r\n", nl);
                        str = str + italS + "Learning Outcomes: " + italE + nl + lo + nl;
                    }
                } //end include report header
                //str = str + "-------------------------------" + nl;

                for (int s = 0; s < ST; s++) //sessions
                {
                    if (s > 0)
                    {
                        firstSession = false;
                    }
                    if (SessionType > 0)
                    {
                        if (DeselectSession[s])
                        {
                            break;
                        }
                        else
                        {
                            SessionCount++;
                        }
                        str = str + "--------------------------------" + nl;
                        str = str + "Session: " + SessionTitle[s] + nl;
                        str = str + "--------------------------------" + nl;
                    }
                    for (int i = 0; i < CritZ + 1; i++)  //save criteria grade and feedback
                    {

                        int j = MaxSub;
                        //if (Marks[i, j, s] == null) { continue; }
                        try
                        {
                            if (Marks[i, j, s] != null)
                            {
                                x = listBox1.FindString(Marks[i, j, s].Trim(), x);
                                if (x < 0)
                                {
                                    minusone = true;
                                }
                                else
                                {
                                    minusone = false;
                                }
                            }
                            else
                            {
                                x = 0;
                            }
                        }
                        catch
                        {
                        }
                        if (x < 0 || x > MaxGrades) { x = 0; }
                        if ((crtitle[i, j] == null || crtitle[i, j].Length < 1))
                        {
                            nullText = true;
                        }
                        else { nullText = false; }
                        if ((!nullText) && (crSelected[i, j, s]))
                        {
                            str = str + "-----------" + nl;
                            str = str + boldS + "Criteria: " + crtitle[i, j] + boldE + nl;
                            if (firstSession && feedOptions.description)
                            {
                                str = str + italS + "Description of requirements for this criteria: " + italE + nl;

                                str = str + crdesc[i, j] + nl;
                            }
                            if (firstSession && feedOptions.LO)
                            {
                                str = str + italS + "Learning Outcomes for this criteria: " + italE + nl;
                                str = str + crLO[i, j] + nl;
                            }
                            if (!Aggregation || (Aggregation && !Has_Subcriteria(i)))
                            {
                                str = str + italS + nl + "Assessment of achievement for this criteria: " + italE + nl;
                                
                                if (Marks[i, j, s] != null)
                                {
                                    if (!minusone)
                                    {
                                        str = str + grcr[i, j, x] + nl;
                                    }
                                    else
                                    {
                                        str = str + nl;
                                    }                                  
                                }
                                m1 = Marks[i, j, s];
                                if (m1 == null)
                                {
                                    m1 = "";
                                }
                                bool perc = false;
                                if (m1.EndsWith("%"))
                                {
                                    m1 = m1.Substring(0, m1.Length - 1) + " %";
                                    perc = true;
                                    //m1 = "Test";
                                }
                                if (feedOptions.CriteriaGrade)
                                {
                                    str = str + italS + "Grade for this criteria: " + italE + " " + m1 + nl;
                                }
                                if (perc == false && feedOptions.CriteriaPercent)
                                {
                                    try
                                    {
                                        str = str + italS + "Equivalent %: " + italE + " " + Find_Percent(m1).ToString() + nl;
                                    }
                                    catch { }
                                }
                               
                                str2 = Find_Grade_Comments(Marks[i, j, s]);
                                if ((str2.Length > 1 && feedOptions.generic) && (!minusone))
                                {
                                    str = str + italS + "Generic description of this grade: " + italE + nl;
                                    str = str + str2 + nl;
                                }
                                str2 = crComment[i, j, s];
                                if ((str2 != null) && feedOptions.criteriaComment && (str2.Trim().Length > 0))
                                {
                                    str = str + nl + italS + "Additional comment: " + italE + nl;
                                    str = str + str2 + nl;
                                }

                                if ((grcrfb[i, j, x].Length > 1) && (!minusone))
                                {
                                    if (firstSession && feedOptions.suggested)
                                    {
                                        str = str + nl + italS + "Suggested feedback for improvement for this criteria: " + italE + nl;


                                        if (Marks[i, j, s] != null)
                                        {
                                            str = str + grcrfb[i, j, x] + nl;
                                        }
                                        else
                                        {
                                            str = str + "------- Need to submit something for this criteria" + nl;
                                        }
                                    }
                                }
                            }
                            //rep1.Passvalue = rep1.Passvalue + nl;
                        }
                        for (int k = 0; k < MaxSub; k++)  //save sub-criteria grade and feedback
                        {

                            //if (Marks[i, k, s] == null) { continue; }
                            x = 0;
                            if (Marks[i, k, s] != null)
                            {
                                try
                                {
                                    x = listBox1.FindString(Marks[i, k, s].Trim(), x);
                                }
                                catch
                                {
                                }
                            }
                            //x = listBox1.FindString(Marks[i, k, s], x);
                            if (x < 0 || x > MaxGrades) { x = 0; }
                            if ((crtitle[i, k] == null || crtitle[i, k].Length < 1))
                            {
                                nullText = true;
                            }
                            else { nullText = false; }
                            if ((!nullText) && (crSelected[i, k, s]))
                            {
                                str = str + "+++++++" + nl;
                                str = str + boldS + " Sub-Criteria: " + crtitle[i, k] + boldE + nl;
                                if (firstSession && feedOptions.subdescription)
                                {
                                    str = str + italS + "Description of requirements for this sub criteria: " + italE + nl;
                                    str = str + crdesc[i, k] + nl;
                                }
                                if (firstSession && feedOptions.SubLO)
                                {
                                    str = str + italS + "Learning Outcomes for this sub criteria: " + italE + nl;
                                    str = str + crLO[i, k] + nl;
                                }
                                str = str + nl + italS + "Assessment of achievement for this sub criteria: " + italE + nl;
                                if (Marks[i, k, s] != null)
                                {
                                    if (!minusone)
                                    {
                                        str = str + grcr[i, k, x] + nl;
                                    }
                                    else
                                    {
                                        str = str + nl;
                                    }
                                }
                                m1 = Marks[i, k, s];
                                if (m1 == null)
                                {
                                    m1 = "";
                                    //MessageBox.Show("Need to mark all of the work first!");
                                    //return false;
                                }
                                bool perc = false;
                                if (m1.EndsWith("%"))
                                {
                                    m1 = m1.Substring(0, m1.Length - 1) + " %";
                                    perc = true;
                                }
                                if (feedOptions.CriteriaGrade)
                                {
                                    str = str + italS + "Grade for this criteria: " + italE + " " + m1 + nl;
                                }
                                if (perc == false && feedOptions.CriteriaPercent)
                                {
                                    try
                                    {
                                        str = str + italS + "Equivalent %: " + italE + " " + Find_Percent(m1).ToString() + nl;
                                    }
                                    catch { }
                            }
                            str2 = Find_Grade_Comments(Marks[i, k, s]);
                                if ((str.Length > 1 && feedOptions.generic) && (!minusone))
                                {
                                    str = str + italS + "Generic description of this grade: " + italE + nl;
                                    str = str + str2 + nl;
                                }
                                str2 = crComment[i, k, s];
                                if (str == null) { str = ""; }
                                if (str2 != null)
                                {
                                    if (str2.Length > 0 && feedOptions.criteriaComment && (str.Trim().Length > 0))
                                    {
                                        str = str + nl + italS + "Additional comment: " + italE + nl;
                                        str = str + str2 + nl;
                                    }
                                }
                                if (grcrfb[i, k, x].Length > 1)
                                {
                                    if (firstSession && feedOptions.suggested && (!minusone))
                                    {
                                        str = str + nl + italS + "Suggested feedback for improvement for this criteria: " + italE + nl;
                                        //if (Marks[i, k, s] == null) { break; }
                                        if (Marks[i, k, s] != null)
                                        {
                                            str = str + grcrfb[i, k, x] + nl;
                                        }
                                        else
                                        {
                                            str = str + "--------Need to submit something for this criteria" + nl;
                                        }
                                    }
                                }
                                //sw.WriteLine();


                            }


                        }
                    }
                    fl = Generate_Overall_Mark(s);
                   


                    if (SessionType > 0)
                    {
                        str = str + nl + "Overall marks for session " + (s + 1).ToString() + ": " + fl.ToString() + " %" + nl;
                    }

                    f = f + fl;
                } //sessions


                if ((textBox10.Text.Trim().Length > 1) && feedOptions.additional)
                {
                    str = str + "-------" + nl;
                    str = str + italS + "General feedback for this work: " + italE + nl;
                    str = str + textBox10.Text.Trim() + nl;
                }
                //str = str +  nl;
                str = str + "-------------------------------" + nl;


               
                    if (overridecheckBox.Checked)
                    {
                        string tmp = "";
                        if (OverrideGradelabel.Text.Trim().IndexOf("%") == label18.Text.Length -1)
                        {
                            tmp = OverrideGradelabel.Text.TrimEnd('%');
                        }
                        else
                        { 
                            tmp = Find_Percent(OverrideGradelabel.Text.Trim());
                        }
                        f = Convert.ToSingle(tmp);
                        if (feedOptions.percent) { str = str + boldS + "Overall Mark " + tmp + " %" + boldE + nl; }
                    }
                    else if (SessionType > 0)
                    {

                        f = f / SessionCount;
                        if (feedOptions.percent) { str = str + boldS + "Overall mark for all sessions in %: " + Convert.ToString(f) + boldE + nl; }
                    }
                    else
                    {
                        f = fl;
                        if (feedOptions.percent) { str = str + boldS + "Overall Mark " + Convert.ToString(f) + " %" + boldE + nl; }
                    }
                
                
                if (feedOptions.grade)
                {
                    str = str + boldS + "Overall Grade: " + Convert_Percent_To_Grade(f) + boldE + nl;
                }
                str = str + nl;

                rep1.RTB.Rtf = str + " }";  //end of rtf input to richtext box on reportform


                //sw.Close();
                label21.Visible = true;
                label22.Visible = true;
                //label22.Text = Convert.ToString(f);


            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
                return false;
            }
            return true;          
        }

        private bool Generate_Grade_Group_RTF()
        {   //generate grade group form layout
            bool nullText = false;
            int x = 0;
          
            string str = "";
            string str2 = "";

            float f = 0;
            float fl = 0;
            int ST;
          
            bool minusone = false;
            string m1 = "";

            FontFamily family = new FontFamily("Calibri");
            Font fontbold = new Font(family, 12.0f, FontStyle.Bold);

            GForm.ClearChecked();   //clear all checkboxes in template
            Calculate_Checks();
            if (SessionType > 0)
            {
                MessageBox.Show("Cannot do this for more than one session");
            }
            ST = 1;
            
            try
            {

                GForm.UnitTitle = UnitTitletextBox.Text;
                GForm.AssessTitle = assess.Title;
                GForm.AssessNo = assess.Code;
                GForm.student = StudentcomboBox.Text;

                for (int s = 0; s < ST; s++) //sessions - only allow one here
                {
                   
                    for (int i = 0; i < CritZ + 1; i++)  //criteria grade and feedback
                    {

                        int j = MaxSub; //for Criteria only (=maxsub - not sub-criteria)
                       
                        try
                        {
                            if (Marks[i, j, s] != null) //i = criteria, j = maxsub, s = sessions (0)
                            {
                                x = listBox1.FindString(Marks[i, j, s].Trim(), x);
                                if (x < 0)
                                {
                                    minusone = true;
                                }
                                else
                                {
                                    minusone = false;
                                }
                            }
                            else
                            {
                                x = 0;
                            }
                        }
                        catch
                        {
                        }
                        if (x < 0 || x > MaxGrades) { x = 0; }
                        if ((crtitle[i, j] == null || crtitle[i, j].Length < 1))
                        {
                            nullText = true;
                        }
                        else { nullText = false; }
                        if ((!nullText) && (crSelected[i, j, s]))
                        {
                            ////////
                            GForm.CT[i] = crtitle[i, j];    //criteria title

                           
                            if (!Aggregation || (Aggregation && !Has_Subcriteria(i)))
                            {                               
                               
                                m1 = Marks[i, j, s];
                                if (m1 == null)
                                {
                                    m1 = "";
                                }
                                if (m1.EndsWith("%"))
                                {
                                    m1 = m1.Substring(0, m1.Length - 1) + " %";                                  
                                }
                                GForm.CM[i] = m1;   //grade mark for each criteria

                                FindGradePositionV2(m1, i);

                                str2 = Find_Grade_Comments(Marks[i, j, s]);
                              
                                str2 = crComment[i, j, s];
                                if ((str2 != null) && feedOptions.criteriaComment && (str2.Trim().Length > 0))
                                {
                                    GForm.comment[i] = "Comments: " + str2;
                                  
                                }

                              
                            }
                           
                        }
                       
                    }
                    fl = Generate_Overall_Mark(s);

                    f = f + fl;
                } //sessions
                if ((textBox10.Text.Trim().Length > 1))
                {
                    GForm.overall = "General comments: " + textBox10.Text.Trim();
                  
                }            
                    if (overridecheckBox.Checked)
                    {
                        string tmp = "";
                        if (label18.Text.Trim().IndexOf("%") == label18.Text.Length - 1)
                        {
                            tmp = label18.Text.TrimEnd('%');
                        }
                        else
                        {
                            tmp = Find_Percent(label18.Text.Trim());
                        }
                        f = Convert.ToSingle(tmp);
                        GForm.OP = tmp; //overall %
                        
                    }
                 
                    else
                    {
                        f = fl;
                        GForm.OP =  Convert.ToString(f);
                    }
                
                GForm.OG = Convert_Percent_To_Grade(f);


            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
                return false;
            }
            return true;
        }

        private void FindGradePosition(string m1, int Cr) //Cr = criteria
        {   //locate in which grade group (box) the current mark is located so that the box can be highlighted (with a cross)
            bool firstthru = true;
            int i = 0;
            int Gg = 0;
            int prev = -1;           
            char tick = '\u2714';

            i = listBox1.FindString(m1);
            for (int a = 0; a < listBox1.Items.Count; a++)
            {
                if (listBox1.GetSelected(a))
                {                    
                    if (i < a || i == a)
                    {
                        if (i == a)
                        {
                            Gg = a;
                        }
                        if (i < a)
                        { Gg = prev; }
                        Gg = (Gg / 2) -1;
                        //GForm.GChecked[Cr, Gg] =  "=== " + tick + " ===";
                        GForm.GChecked[Cr, Gg] = true;
                        break;
                    }
                    //prev = a;
                    if (firstthru)  //every other count of grade groups
                    {
                       prev = a;
                       firstthru = false;
                    }
                    else
                    {
                        firstthru = true;
                    }
                }
            }
        }
        private void FindGradePositionV2(string m1, int Cr) //Cr = criteria
        {   //locate in which grade group (box) the current mark is located so that the box can be highlighted (with a cross)
           
            int i = 0;
            int Gg = 0;
            int prev = -1;
            char tick = '\u2714';
            int maxGG = 14;
            int[] list = new int[listBox1.Items.Count];
            int pos = 0;
            bool firstthru = true;
            for (int a = 0; a < listBox1.Items.Count; a++)
            {
                if (listBox1.GetSelected(a) )
                {    if (firstthru)
                    {
                        firstthru = false;
                    }
                    else
                    {
                        firstthru = true;
                        list[a] = pos;
                        pos++;
                    }
                }
                if (!firstthru)
                {
                    list[a] = pos;
                }               
            }

            i = listBox1.FindString(m1);          
            Gg = list[i];
            GForm.GChecked[Cr, Gg] = true;
                     
        }

        /*private void Generate_Feedback_Report(string filename)
        {
            bool nullText = false;
            int x = 0;
            bool firstSession = true;
            //int s = 0;

            string str;
            float f = 0;
            float fl = 0;
            int ST;

            Calculate_Checks();
            if (SessionType == 0)
            {
                ST = 1;
            }
            else
            {
                ST = SessionS;
            }
            try
            {
                using (StreamWriter sw = new StreamWriter(filename))
                {

                    sw.WriteLine("Student name: " + StudentcomboBox.Text + "       Date /time: " + DateTime.Now.ToString("dd/MM/yy  HH:mm"));
                    sw.WriteLine("Assessment title: " + assess.Title + ", Code: " + assess.Code);
                    sw.WriteLine("Assessment weight for unit: " + assess.Weight);
                    if (feedOptions.full)
                    {
                        sw.WriteLine("Description: ");
                        sw.WriteLine(assess.Description);

                    }
                    if (feedOptions.fullLO)
                    {
                        sw.WriteLine("Learning Outcomes: ");
                        sw.WriteLine(assess.LOs);
                    }
                    sw.WriteLine("---------------");

                    for (int s = 0; s < ST; s++)
                    {
                        if (s > 0)
                        {
                            firstSession = false;
                        }
                        if (SessionType > 0)
                        {
                            sw.WriteLine("--------------------------------");
                            sw.WriteLine("Session: " + SessionTitle[s]);
                            sw.WriteLine("--------------------------------");
                        }
                        for (int i = 0; i < CritZ + 1; i++)  //save criteria grade and feedback
                        {

                            int j = MaxSub;
                            //if (Marks[i, j, s] == null) { continue; }
                            x = 0;
                            if (Marks[i, j, s] != null)
                            {
                                try
                                {
                                    x = listBox1.FindString(Marks[i, j, s].Trim(), x);
                                }
                                catch
                                {
                                }
                            }

                            if (x < 0 || x > MaxGrades) { x = 0; }
                            if ((crtitle[i, j] == null || crtitle[i, j].Length < 1))
                            {
                                nullText = true;
                            }
                            else { nullText = false; }
                            if (!nullText)
                            {
                                sw.WriteLine("---");
                                sw.WriteLine("Criteria: " + crtitle[i, j]);
                                if (firstSession && feedOptions.description)
                                {
                                    sw.WriteLine("Description of requirements for this criteria: ");

                                    sw.WriteLine(crdesc[i, j]);
                                }
                                if (firstSession && feedOptions.LO)
                                {
                                    sw.WriteLine("Learning Outcomes for this criteria: ");

                                    sw.WriteLine(crLO[i, j]);
                                }
                                if (!Aggregation)
                                {
                                    sw.WriteLine("Assessment of achievement for this criteria: ");
                                    if (Marks[i, j, s] != null)
                                    {
                                        sw.WriteLine(grcr[i, j, x]);
                                    }

                                    sw.WriteLine("Grade for this criteria: " + Marks[i, j, s]);
                                    str = Find_Grade_Comments(Marks[i, j, s]);
                                    if (str.Length > 1 && feedOptions.generic)
                                    {
                                        sw.WriteLine("Generic description of this grade: ");
                                        sw.WriteLine(str);
                                    }
                                    str = crComment[i, j, s];
                                    if ((str != null) && feedOptions.criteriaComment)
                                    {
                                        sw.WriteLine("Additional comment: ");
                                        sw.WriteLine(str);
                                    }

                                    if (grcrfb[i, j, x].Length > 1)
                                    {
                                        if (firstSession && feedOptions.suggested)
                                        {
                                            sw.WriteLine("Suggested feedback for improvement for this criteria: ");


                                            if (Marks[i, j, s] != null)
                                            {
                                                sw.WriteLine(grcrfb[i, j, x]);
                                            }
                                            else
                                            {
                                                sw.WriteLine("------- Need to submit something for this criteria");
                                            }
                                        }
                                    }
                                }
                                sw.WriteLine();
                            }
                            for (int k = 0; k < MaxSub; k++)  //save sub-criteria grade and feedback
                            {
                                //if (Marks[i, k, s] == null) { continue; }
                                x = 0;
                                if (Marks[i, k, s] != null)
                                {
                                    try
                                    {
                                        x = listBox1.FindString(Marks[i, k, s].Trim(), x);
                                    }
                                    catch
                                    {
                                    }
                                }

                                //x = listBox1.FindString(Marks[i, k, s], x);
                                if (x < 0 || x > MaxGrades) { x = 0; }
                                if ((crtitle[i, k] == null || crtitle[i, k].Length < 1))
                                {
                                    nullText = true;
                                }
                                else { nullText = false; }
                                if (!nullText)
                                {
                                    sw.WriteLine("---");
                                    sw.WriteLine("Sub-Criteria: " + crtitle[i, k]);
                                    if (firstSession && feedOptions.description)
                                    {
                                        sw.WriteLine("Description of requirements for this sub criteria: ");
                                        sw.WriteLine(crdesc[i, k]);
                                    }
                                    if (firstSession && feedOptions.LO)
                                    {
                                        sw.WriteLine("Learning Outcomes for this sub-criteria: ");

                                        sw.WriteLine(crLO[i, k]);
                                    }
                                    sw.WriteLine("Assessment of achievement for this sub criteria: ");
                                    if (Marks[i, k, s] != null)
                                    {
                                        sw.WriteLine(grcr[i, k, x]);
                                    }

                                    sw.WriteLine("Grade for this criteria: " + Marks[i, k, s]);
                                    str = Find_Grade_Comments(Marks[i, k, s]);
                                    if (str.Length > 1 && feedOptions.generic)
                                    {
                                        sw.WriteLine("Generic description of this grade: ");
                                        sw.WriteLine(str);
                                    }
                                    str = crComment[i, k, s];
                                    if (str == null) { str = ""; }
                                    if (str.Length > 0 && feedOptions.criteriaComment)
                                    {
                                        sw.WriteLine("Additional comment: ");
                                        sw.WriteLine(str);
                                    }
                                    if (grcrfb[i, k, x].Length > 1)
                                    {
                                        if (firstSession && feedOptions.suggested)
                                        {
                                            sw.WriteLine("Suggested feedback for improvement for this criteria: ");
                                            //if (Marks[i, k, s] == null) { break; }
                                            if (Marks[i, k, s] != null)
                                            {
                                                sw.WriteLine(grcrfb[i, k, x]);
                                            }
                                            else
                                            {
                                                sw.WriteLine("--------Need to submit something for this criteria");
                                            }
                                        }
                                    }
                                    //sw.WriteLine();

                                }


                            }
                        }
                        fl = Generate_Overall_Mark(s);
                        if (SessionType > 0)
                        {
                            sw.WriteLine("Overall marks for session " + (s + 1).ToString() + ": " + fl.ToString() + " %");
                        }

                        f = f + fl;
                    } //sessions
                    if ((textBox10.Text.Trim().Length > 1) && feedOptions.additional)
                    {
                        sw.WriteLine("----");
                        sw.WriteLine("General feedback for this work: ");
                        sw.WriteLine(textBox10.Text.Trim());
                    }
                    sw.WriteLine();
                    sw.WriteLine("-------------------------------");
                    //f = Generate_Overall_Mark();

                    if (feedOptions.percent)
                    {
                        if (SessionType > 0)
                        {

                            f = f / SessionS;
                            sw.WriteLine("Overall mark for all sessions in %: " + Convert.ToString(f));
                        }
                        else
                        {
                            f = fl;
                            sw.WriteLine("Overall Mark " + Convert.ToString(f) + " %");
                        }
                    }
                    if (feedOptions.grade)
                    {
                        sw.WriteLine("Overall Grade: " + Convert_Percent_To_Grade(f));
                    }
                    sw.WriteLine("");

                    sw.Close();
                    label21.Visible = true;
                    label22.Visible = true;
                    label22.Text = Convert.ToString(f);
                }
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
            button1.Visible = true;

            rep1.Passvalue = filename;

            //rep1.ShowDialog();
        }*/

        private string Convert_Percent_To_Grade(float percent)
        {
            for (int i = 0; i < MaxGrades; i++)
            {
                if ((percent >= gradelist[i].grlower) && (percent <= gradelist[i].grupper))
                {
                    return gradelist[i].grtitle;
                }
            }
            return "";

        }

        private string Find_Grade_Comments(string str)
        {
            string s = "";
            try
            {
                for (int i = 0; i < MaxGrades - 1; i++)
                {
                    if (str.Trim() == gradelist[i].grtitle.Trim())
                    {
                        s = gradelist[i].grfb;
                        return s;
                    }
                }
            }
            catch { }
            return s;
        }

        private string Find_Percent(string str)
        {
            string s = "";
            string b = "";
            try
            {
                if (str.EndsWith("%"))
                {
                    s = str.Substring(0, str.Length-1);
                }
                else
                {
                    for (int i = 0; i < MaxGrades - 1; i++)
                    {
                        if (singleGrades)
                        {
                            b = gradelist[i].grtitle.Trim();
                        }
                        else
                        {
                            b = gradelist[i].gralias.Trim();
                        }

                        if (str.Trim() == b)
                        {
                            s = Convert.ToString(gradelist[i].grpercent).Trim();
                        }
                        if (s.Length > 0) { break; }
                    }
                }
            }
            catch
            {
                s = "";
            }
            return s;
        }

        private string Find_Upper(string str)
        {
            string s = "";
            for (int i = 0; i < MaxGrades - 1; i++)
            {
                if (str.Trim() == gradelist[i].grtitle.Trim())
                {
                    s = Convert.ToString(gradelist[i].grupper).Trim();
                }
                if (s.Length > 0) { break; }
            }
            return s;
        }

        private string Find_Lower(string str)
        {
            string s = "";
            for (int i = 0; i < MaxGrades - 1; i++)
            {
                if (str.Trim() == gradelist[i].grtitle.Trim())
                {
                    s = Convert.ToString(gradelist[i].grlower).Trim();
                }
                if (s.Length > 0) { break; }
            }
            return s;
        }

        private float Generate_Overall_Mark(int Se)
        {
            float f = 0;
            float g = 0;
            float a = 0;
            double d;
            string s = "";

            for (int i = 0; i < CritZ + 1; i++)
            {
                if (yesno)
                {
                    s = Marks[i, MaxSub, Se];
                }
                else
                {
                    if (crSelected[i,MaxSub,Se])
                    {
                        s = Find_Percent(Marks[i, MaxSub, Se]);
                    }
                }
                if (s != null && s.Length > 0)
                {
                    if (crSelected[i, MaxSub, Se])
                    {
                        // if marks are not aggregated or they are aggregated, but this criteria has no subcriteria:
                        if (!Aggregation || (Aggregation && !Has_Subcriteria(i)))
                        {
                            g = Convert.ToSingle(crweight[i, MaxSub, Se]);
                            f = f + (g / 100) * Convert.ToSingle(s);
                        }
                    }

                }
                else
                {
                    int o = 0;
                }
            }
            for (int i = 0; i < CritZ + 1; i++)
            {
                for (int j = 0; j < MaxSub; j++)
                {
                    if (yesno)
                    {
                        s = Marks[i, j, Se];
                    }
                    else
                    {
                        if (crSelected[i, j, Se])
                        {
                            s = Find_Percent(Marks[i, j, Se]);
                        }
                    }
                    if (s != null && s.Length > 0)
                    {
                        if (crSelected[i, j, Se])
                        {
                            g = Convert.ToSingle(crweight[i, j, Se]);
                            if (!Aggregation)
                            {
                                //with no aggregation the subcriteria mark is independent of criteria mark
                                f = f + (g / 100) * Convert.ToSingle(s);
                            }
                            else
                            {
                                //with aggregation need to work mark for subcriteria times criteria
                                a = Convert.ToSingle(crweight[i, MaxSub, Se]);
                                f = f + (g / 100 * a / 100) * Convert.ToSingle(s);
                            }
                        }
                    }
                }
            }
            d = Math.Round(f);
            return Convert.ToSingle(d);
        }


        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            //Feedback = textBox10.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Clear the form or start marking button
            if (startMark)
            {
                DialogResult dialogResult = MessageBox.Show("This will delete form data (you should save your marks first) - do you wish to clear form Yes/No?", "Clear Form", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Markerlabel.Visible = false;
                    MarkertextBox.Visible = false;
                    Clear_Form_Data();
                    overridecheckBox.Checked = false; //stop overriding student grade
                    OverrideGradelabel.Visible = false;
                    Overriedlabel.Visible = false;
                       
                    if (CriteriaSelectionType > 1)
                    {
                        Reset_Selected(true);
                        Change_Session_Selection();
                    }
                    //button1.Visible = false;
                    savedStudent = false;
                    button7.Text = "Start marking student";
                    startMark = false;
                    button4.Visible = false;
                    button5.Visible = true;
                    button6.Visible = false;
                    //textBox10.Enabled = false;
                    addButton.Visible = false;
                    StudentcomboBox.Enabled = true;
                    overrideBox.Enabled = false;
                    ImportcheckBox.Visible = false;
                    importGroupBox.Visible = false;
                    importCalcLabel.Visible = false;
                    Clicklabel1.Visible = false;
                }
            }
            else
            {
                if (StudentcomboBox.Text.Trim() == "")
                {
                    MessageBox.Show("Student name is blank");
                    return;
                }
                Markerlabel.Visible = true;
                MarkertextBox.Visible = true;
                Clicklabel1.Visible = true;
                button7.Text = "Clear form/mark another";
                button4.Visible = true;
                button5.Visible = false;
                button6.Visible = true;
                //textBox10.Visible = true;
                //textBox10.Enabled = true;
                addButton.Visible = true;
                startMark = true;
                StudentcomboBox.Enabled = false;
                overrideBox.Enabled = true;
                ImportcheckBox.Visible = true;
                importCalcLabel.Visible = true;
                if (ImportcheckBox.Checked)
                {
                    importGroupBox.Visible = true;
                    importCalcLabel.Visible = true;
                }               
                
            }
        }

        private void Clear_Form_Data()
        {
            //clears all marking data on the form (warnings should be placed in calling method)

            StudentcomboBox.Text = "";
            textBox10.Text = "";
            LOtextBox.Text = "";
            //assessTitleBox.Text = "";
            //assess.Title = assessTitleBox.Text;
            //assess.Description = "";
            //assess.Code = "";
            //assess.Weight = "";

            label22.Text = "";
            //Feedback = "";
            label18.Text = "  ";
            gradelbl.Text = "    ";
            fblbl.Text = "    ";
           
            for (int i = 0; i < MaxCriteria; i++)
            {
                for (int j = 0; j <= MaxSub; j++)
                {
                    if (SessionS == 0)
                    {
                        Marks[i, j, 0] = null;
                        crComment[i, j, 0] = null;
                    }
                    else
                    {
                        for (int s = 0; s < SessionS; s++)
                        {
                            Marks[i, j, s] = null;
                            crComment[i, j, 0] = null;
                        }
                    }
                }
            }
            for (int m = 0; m < MaxSessions; m++)
            {
                DeselectSessioncheckBox.Checked = false;
                DeselectSession[m] = false;
            }
            if (UseChecked)
            {
                treeView2.CheckBoxes = false;
                treeView2.CheckBoxes = true;
                treeView2.Nodes[0].ExpandAll();
            }


        }

        private void criteriaTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //int critype;
            CTForm input = new CTForm();
            //set criteria type to unchecked by default:
            input.Passvalue = CriteriaType;

            input.ShowDialog();

            CriteriaType = input.Passvalue;
            SetUpCriteriaType(CriteriaType);
        }

        private void SetUpCriteriaType(int critype)
        {

            switch (critype)
            {
                case 0:
                    {
                        label11.Visible = true;
                        listBox1.Visible = true;
                        yesno = false;
                        treeView2.CheckBoxes = false;
                        treeView2.Nodes[0].ExpandAll();
                        UseChecked = false;
                        label23.Text = "Criteria type - no checkboxes, no aggregation of subcriteria";
                        label24.Visible = false;
                        label25.Visible = false;
                        gradelbl.Visible = true;
                        fblbl.Visible = true;
                        //label16.Visible = true;
                        //label18.Visible = true;
                        Aggregation = false;
                        break;
                    }
                case 1:
                    {
                        label11.Visible = true;
                        listBox1.Visible = true;
                        yesno = false;
                        treeView2.CheckBoxes = false;
                        treeView2.Nodes[0].ExpandAll();
                        UseChecked = false;
                        label23.Text = "Criteria type - no checkboxes, with aggregation of subcriteria";
                        label24.Visible = false;
                        label25.Visible = false;
                        gradelbl.Visible = true;
                        fblbl.Visible = true;
                        //label16.Visible = true;
                        //label18.Visible = true;
                        Aggregation = true;
                        break;
                    }
                case 2:
                    {
                        label11.Visible = true;
                        listBox1.Visible = true;
                        yesno = false;
                        //treeView2.CheckBoxes = true;
                        treeView2.Nodes[0].ExpandAll();
                        UseChecked = true;
                        Clear_Checks();
                        label24.Visible = false;

                        label25.Visible = false;
                        label23.Text = "Criteria type - selectable checkboxes graded, no aggregation";
                        gradelbl.Visible = true;
                        fblbl.Visible = true;
                        //label16.Visible = true;
                        //label18.Visible = true;
                        Aggregation = false;
                        break;
                    }
                case 3:
                    {
                        label11.Visible = true;
                        listBox1.Visible = true;
                        yesno = false;
                        //treeView2.CheckBoxes = true;
                        treeView2.Nodes[0].ExpandAll();
                        UseChecked = true;
                        Clear_Checks();
                        label24.Visible = false;

                        label25.Visible = false;
                        label23.Text = "Criteria type - selectable checkboxes graded, with aggregation";
                        gradelbl.Visible = true;
                        fblbl.Visible = true;
                        //label16.Visible = true;
                        //label18.Visible = true;
                        Aggregation = true;
                        break;
                    }
                case 4:
                    {
                        label11.Visible = false;
                        listBox1.Visible = false;
                        yesno = true;
                        //treeView2.CheckBoxes = true;
                        treeView2.Nodes[0].ExpandAll();
                        UseChecked = true;
                        Clear_Checks();

                        label24.Text = "  ";
                        label24.Visible = true;
                        label25.Visible = true;
                        label23.Text = "Criteria type - selectable checkboxes with yes/no selection";
                        gradelbl.Visible = false;
                        fblbl.Visible = false;
                        label16.Visible = false;
                        label18.Visible = false;
                        Aggregation = false;
                        break;
                    }
            }
            label23.Visible = true;
        }

        private void Clear_Checks()
        {
            for (int i = 0; i < MaxCriteria; i++)
            {
                for (int j = 0; j < MaxSub + 1; j++)
                {
                    nodeChecked[i, j, Session] = false;
                }
            }
            NChecked = false;
        }

        private void treeView2_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //if using yes/no grades:
            int Crit = 0;
            int Sub = 0;

            if (treeView2.Nodes[0].Nodes.Count > 0)
            {
                treeView2.SelectedNode = e.Node;

                foreach (TreeNode RootNode in treeView2.Nodes)
                {
                    foreach (TreeNode ChildNode in RootNode.Nodes)
                    {
                        Crit = ChildNode.Index;
                        if (ChildNode.IsSelected)
                        {
                            if (ChildNode.Checked)
                            {
                                NChecked = true;
                                label24.Text = "Yes";

                            }
                            else
                            {
                                NChecked = false;
                                label24.Text = "No ";
                                foreach (TreeNode SubNode in ChildNode.Nodes)
                                {
                                    tempflag = true;
                                    SubNode.Checked = false;
                                }
                            }
                            nodeChecked[Crit, MaxSub, Session] = NChecked;

                        }

                        foreach (TreeNode SubNode in ChildNode.Nodes)
                        {

                            if (SubNode.IsSelected)
                            {
                                Sub = SubNode.Index;

                                if (SubNode.Checked)
                                {
                                    //if (!nodeenabled[Crit, MaxSub])
                                    if (!treeView2.SelectedNode.Parent.Checked)
                                    {
                                        nodeChecked[Crit, Sub, Session] = false;
                                        SubNode.Checked = false;
                                        label24.Text = "No ";
                                        //e.Cancel;
                                    }
                                    else
                                    {
                                        nodeChecked[Crit, Sub, Session] = true;
                                        label24.Text = "Yes";
                                    }
                                }
                                else
                                {
                                    //if (nodeenabled[Crit, MaxSub])
                                    if (treeView2.SelectedNode.Parent.Checked)
                                    {
                                        if (SubNode.Checked)
                                        {
                                            nodeChecked[Crit, Sub, Session] = true;
                                            label24.Text = "Yes";
                                        }
                                        else
                                        {
                                            nodeChecked[Crit, Sub, Session] = false;
                                            label24.Text = "No ";
                                        }
                                    }


                                }
                                return;


                            }

                        }
                    }
                }
            }

        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Close application without saving data Yes/No?", "Close form", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                e.Cancel = true;
                MessageBox.Show("Don't forget to save your data if you need to!");
            }
            else
            {
                SaveSettings();
            }
        }

        private void SaveSettings()
        {
            char[] c = new char[20];
            string bl = "true";
            //string c = "111111111";
            string str = "";
            string over = "true";
            try
            {
                if (!LOs)
                {
                    bl = "false";
                }
                for (int i = 0; i < 20; i++)
                {
                    c[i] = '1';  //set feedbackoptions to selected
                }

                if (fineWeight == 1)
                { str = "1%"; }
                else if (fineWeight == 5)
                { str = "5%"; }
                else
                {
                    str = "0.5%";
                }

                try
                {
                    if (!Directory.Exists(ConfigDir))
                    {
                        Directory.CreateDirectory(ConfigDir);
                    }
                    using (StreamWriter cw = new StreamWriter(ConfigDir + "Ultramarker.dir"))
                    {
                        cw.WriteLine("Default Dir: " + DefaultDir);
                        cw.Close();
                    }
                }
                catch {
                    MessageBox.Show("Unable to save Ultramarker.dir file!");
                }
                using (StreamWriter sw = new StreamWriter(DefaultDir + "Ultramarker.cfg"))
                {
                    sw.WriteLine("Default Dir: " + DefaultDir);
                    sw.WriteLine("Institution: " + Institution);
                    sw.WriteLine("UL Sig File: " + ULSigFilePath);
                    sw.WriteLine("Unit file: " + UnitFile);
                    sw.WriteLine("Unit file path: " + UnitFilePath);
                    sw.WriteLine("Marks Directory: " + marksDirectory);
                    sw.WriteLine("Criteria type: " + CriteriaType.ToString());
                    sw.WriteLine("Criteria Selection Type: " + CriteriaSelectionType.ToString());
                    sw.WriteLine("Show LO: " + bl);
                    sw.WriteLine("Weighting type: " + str);
                    sw.WriteLine("Grade path: " + GradePath);
                    sw.WriteLine("Grade file: " + GradeFile);
                    if (allowoverride) { over = "true"; } else { over = "false"; }
                    sw.WriteLine("Allow grade override: " + over);
                    sw.WriteLine("LO path: " + LOFilePath);
                    sw.WriteLine("LO file: " + LOFile);
                    sw.WriteLine("Criteria path: " + CriteriaPath);
                    sw.WriteLine("Criteria file: " + CriteriaFile);
                    sw.WriteLine("Comments file: " + CommentFile);
                    sw.WriteLine("Comments path: " + CommentFilePath);
                    sw.WriteLine("Moderation Dir: " + modDirectory);
                    sw.WriteLine("Student import file: " + StudentImportFile);

                    if (SessionType == 0)
                    {
                        sw.WriteLine("Sessions: 0");
                        sw.WriteLine("Session path: ");
                        sw.WriteLine("Session file: ");
                    }
                    else
                    {
                        sw.WriteLine("Sessions: " + Convert.ToString(SessionType));
                        sw.WriteLine("Session path: " + SessionPath);
                        sw.WriteLine("Session file: " + SessionFile);
                    }
                    if (!feedOptions.generic) { c[0] = '0'; }
                    if (!feedOptions.description) { c[1] = '0'; }
                    if (!feedOptions.suggested) { c[2] = '0'; }
                    if (!feedOptions.additional) { c[3] = '0'; }
                    if (!feedOptions.criteriaComment) { c[4] = '0'; }
                    if (!feedOptions.percent) { c[5] = '0'; }
                    if (!feedOptions.grade) { c[6] = '0'; }
                    if (!feedOptions.LO) { c[7] = '0'; }
                    if (!feedOptions.full) { c[8] = '0'; }
                    if (!feedOptions.fullLO) { c[9] = '0'; }
                    if (!feedOptions.SubLO) { c[10] = '0'; }
                    if (!feedOptions.CriteriaGrade) { c[11] = '0'; }
                    if (!feedOptions.subdescription) { c[12] = '0'; }
                    if (!feedOptions.includeheader) { c[13] = '0'; }
                    if (!feedOptions.CriteriaPercent) { c[14] = '0'; }
                    sw.WriteLine("Feedback options: " + c[0] + c[1] + c[2] + c[3] + c[4] + c[5] + c[6] + c[7] + c[8] + c[9] + c[10] + c[11] + c[12] + c[13] +c[14]);
                    try
                    {
                        sw.WriteLine("Summary sort type: " + Convert.ToString(Summary_Sort_Type));
                        sw.WriteLine("Summary percent or grade: " + Summary_percentgrade);
                    }
                    catch
                    {
                    }
                    string ch = "false";
                    if (CalculateImportbyLines)
                    {
                        ch = "true";
                    }
                    sw.WriteLine("Import calculation by lines?: " + ch);
                    if (AllowImpComment)
                    {
                        ch = "true";
                    }
                    else { ch = "false"; }
                    sw.WriteLine("Import comments?: " + ch);
                    if (showGenAssessToolStripMenuItem.Checked)
                    {
                        ch = "true";
                    }
                    else { ch = "false"; }
                    sw.WriteLine("ShowGenAssess?: " + ch);
                    sw.WriteLine("Gen template: " + templatetextBox.Text);
                    sw.WriteLine("Tick: " + highlightButton.Text);
                    sw.WriteLine("Marker: " + MarkertextBox.Text);
                    sw.Close();
                }
                SaveGradeListbox();
            }
           
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void SaveGradeListbox()
        {
            int y = 0;
            try
            {
                using (FileStream stream = new FileStream(DefaultDir + "gradeselected.bin", FileMode.Create))
                {
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        try
                        {
                            for (int i = 0; i < listBox1.Items.Count; i++)
                            {
                                if (button3.Text.StartsWith("Gen"))
                                {
                                    if (listBox1.GetSelected(i))
                                    {
                                        y = 1;
                                    }
                                    else
                                    {
                                        y = 0;
                                    }
                                }
                                else
                                {
                                    if (Listboxlist[i] == 1)
                                    {
                                        y = 1;
                                    }
                                    else
                                    {
                                        y = 0;
                                    }
                                }
                                writer.Write(y);
                            }
                            writer.Close();
                        }
                        catch { }

                    }
                }
            }
            catch { }
        }
        private void LoadGradeListbox()
        {

            int i = 0;
            int a = 0;
            try
            {
                using (FileStream stream = File.OpenRead(DefaultDir + "gradeselected.bin"))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        // Read in all pairs.
                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            i = reader.ReadInt32();
                            Listboxlist[a] = i;
                            a++;

                        }
                        reader.Close();
                    }

                }
            }
            catch { }
           
        }

        private void LoadSettings()
        {
            //on startup
            char[] c = new char[20];
            string str = "";
            string[] str2 = new string[2];
            string str3 = "";

            for (int i = 0; i < 20; i++)
            {
                c[i] = '1';  //set feedbackoptions to selected
            }

            if (File.Exists(ConfigDir + "Ultramarker.dir"))
            {
                try
                {
                    using (StreamReader rw = new StreamReader(ConfigDir + "Ultramarker.dir"))
                    {
                        str = rw.ReadLine();
                        if (str.StartsWith("Default Dir:"))
                        {
                            str3 = str.Substring(0,"Default Dir:".Length).Trim();
                            str = str.Substring(str3.Length, str.Length - str3.Length).Trim();
                            if (Directory.Exists(str))
                            {
                                DefaultDir = str;
                                GradePath = str;
                                defaultdirlabel.Text = "Default directory currently set to: " + DefaultDir;                                
                            }                        
                        }
                        rw.Close();
                    }
                }
                catch { }
            }             
            if (File.Exists(DefaultDir + "Ultramarker.cfg"))
            {
                try
                {
                    using (StreamReader sw = new StreamReader(DefaultDir + "Ultramarker.cfg"))
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
                                    //str2 = str.Split(':');
                                    //str3 = str2[1].Trim();
                                }
                                else
                                {
                                    str3 = str;
                                }
                            }
                            if (str.StartsWith("Criteria type:"))
                            {
                                try
                                {
                                    CriteriaType = Convert.ToInt32(str3);
                                }
                                catch
                                {
                                    CriteriaType = 0;
                                }
                            }
                            else if (str.StartsWith("Criteria Selection Type:"))
                            {
                                try
                                {
                                    CriteriaSelectionType = Convert.ToInt32(str3);
                                }
                                catch
                                {
                                    CriteriaSelectionType = 0;
                                }
                            }
                            else if (str.StartsWith("Show LO:"))
                            {
                                if (str.Contains("true"))
                                {
                                    LOs = true;
                                }
                                else
                                {
                                    LOs = false;
                                    LOtextBox.Visible = false;
                                    LObutton.Visible = false;
                                }
                            }
                            else if (str.StartsWith("Weighting type:"))
                            {
                                if (str3 == "1%") { fineWeight = 1; }
                                else if (str3 == "5%") { fineWeight = 5; }
                                else { fineWeight = 0.5; }
                            }

                            else if (str.StartsWith("Marks Directory:"))
                            {
                                marksDirectory = str3;
                            }
                            else if (str.StartsWith("Grade file:"))
                            {
                                GradeFile = str3;
                                //str2[1] + ":" + str2[2];
                                if (File.Exists(GradeFile))
                                {
                                    ReadGradesFromFile(GradeFile);
                                }
                            }
                            else if (str.StartsWith("Allow grade override: true"))
                            {
                                allowoverride = true;
                            }
                            else if (str.StartsWith("Allow grade override: false"))
                            {
                                allowoverride = false;
                            }

                            else if (str.StartsWith("LO path:"))
                            {
                                LOFilePath = str3;
                            }
                            else if (str.StartsWith("LO file:"))
                            {
                                LOFile = str3;
                                if (File.Exists(LOFile))
                                {
                                    //ReadLOFromFile(LOFile);   //load LOs separately
                                    LoadLOlists();
                                }
                            }
                            else if (str.StartsWith("Institution:"))
                            {
                                Institution = str3;
                                institutionTextBox.Text = Institution;
                            }
                            else if (str.StartsWith("UL Sig File:"))
                            {
                                ULSigFilePath = str3;
                                ULSigBox.Text = ULSigFilePath;
                            }
                            else if (str.StartsWith("Default Dir:"))
                            {
                                DefaultDir = str3;
                            }
                            else if (str.StartsWith("Unit file path:"))
                            {
                                if (str3.Length > 0)
                                {
                                    UnitFilePath = str3;
                                }
                            }
                            else if (str.StartsWith("Unit file:"))
                            {
                                UnitFile = str3;
                                if (File.Exists(UnitFile))
                                {
                                    LoadUnitFile(UnitFile);
                                    unitFoldertextBox.Text = Path.GetDirectoryName(str3);
                                    UnitFilePath = unitFoldertextBox.Text;
                                }
                            }
                            else if (str.StartsWith("Moderation Dir:"))
                            {
                                modDirectory = str3;
                            }

                            else if (str.StartsWith("Criteria file:"))
                            {
                                CriteriaFile = str3;
                                //str2[1] + ":" + str2[2];
                                if (File.Exists(CriteriaFile))
                                {
                                    ReadCriteriaFromFile(CriteriaFile, true);
                                }
                            }
                            else if (str.StartsWith("Criteria path:"))
                            {
                                if (str3.Length > 0)
                                {
                                    CriteriaPath = str3;
                                }
                            }
                            else if (str.StartsWith("Comments file:"))
                            {
                                CommentFile = str3; //used to open comments on comments form                              
                            }
                            else if (str.StartsWith("Comments path:"))
                            {
                                CommentFilePath = str3; //used to open comments on comments form                              
                            }
                            else if (str.StartsWith("Student import file:"))
                            {
                                StudentImportFile = str3; //used to open comments on comments form                              
                            }
                            else if (str.StartsWith("Sessions:"))
                            {
                                if (str3.Equals("0"))
                                {
                                    SessionS = 0;
                                    SessionType = 0;

                                }
                                else
                                {
                                    SessionType = 1;

                                }

                            }
                            else if (str.StartsWith("Session file:") && (SessionType > 0))
                            {
                                SessionFile = str3;
                                //str2[1] + ":" + str2[2];
                                if (File.Exists(SessionFile))
                                {
                                    Load_Sessions(SessionFile);
                                }
                                tabControl1.TabPages.Insert(4, tabPage3);
                            }
                            else if (str.StartsWith("Feedback options:"))
                            {
                                try
                                {
                                    c[0] = str3[0];
                                    c[1] = str3[1];
                                    c[2] = str3[2];
                                    c[3] = str3[3];
                                    c[4] = str3[4];
                                    c[5] = str3[5];
                                    c[6] = str3[6];
                                    c[7] = str3[7];
                                    c[8] = str3[8];
                                    c[9] = str3[9];
                                    c[10] = str3[10];
                                    c[11] = str3[11];
                                    c[12] = str3[12];
                                    c[13] = str3[13];
                                    c[14] = str3[14];
                                }
                                catch
                                {

                                }
                            }
                            else if (str.StartsWith("Summary sort type:"))
                            {
                                try
                                {
                                    if (str3 != "")
                                    {
                                        Summary_Sort_Type = Convert.ToInt32(str3);
                                    }
                                    else
                                    {
                                        Summary_Sort_Type = 0;
                                    }
                                }
                                catch
                                {
                                    Summary_Sort_Type = 0;
                                }

                            }
                            else if (str.StartsWith("Summary percent or grade:"))
                            {
                                try
                                {
                                    Summary_percentgrade = str3;
                                }
                                catch
                                {
                                }
                            }
                            else if (str.StartsWith("Import calc"))
                            {
                                if (str.Contains("true"))
                                {
                                     CalculateImportbyLines= true;
                                     importCalcLabel.Text = "Calculate by lines";
                                }
                                else
                                {
                                    CalculateImportbyLines = false;
                                    importCalcLabel.Text = "Calculate by %ages";
                                }
                            }
                            else if (str.StartsWith("Import comm"))
                            {
                                if (str.Contains("true"))
                                {
                                    AllowImpComment = true;
                                }
                                else
                                { AllowImpComment = false; }
                            }
                            else if (str.StartsWith("ShowGenAssess"))
                            {
                                if (str.Contains("true"))
                                {
                                    showGenAssessToolStripMenuItem.Checked = true;
                                }
                                else
                                { showGenAssessToolStripMenuItem.Checked = false; }
                            }
                            else if (str.StartsWith("Gen template:"))
                            {
                                string tmp = str.Substring(str.IndexOf("Gen template:")+("Gen template:").Length);
                                templatetextBox.Text = tmp.Trim();
                            }
                            else if (str.StartsWith("Tick:"))
                            {
                                string tmp = str.Substring(str.IndexOf("Tick:") + ("Tick:").Length);
                                highlightButton.Text = tmp.Trim();
                            }
                            else if (str.StartsWith("Marker:"))
                            {
                                string tmp = str.Substring(str.IndexOf("Marker:") + ("Marker:").Length);
                               MarkertextBox.Text = tmp.Trim();
                            }
                        }
                        if (SessionType == 0)
                        {
                            Set_Session(false); //single session (no sessions)
                        }
                        else
                        {
                            Set_Session(true); //multi-session
                        }

                        sw.Close();
                        if (c[0] == '0') { feedOptions.generic = false; }
                        if (c[1] == '0') { feedOptions.description = false; }
                        if (c[2] == '0') { feedOptions.suggested = false; }
                        if (c[3] == '0') { feedOptions.additional = false; }
                        if (c[4] == '0') { feedOptions.criteriaComment = false; }
                        if (c[5] == '0') { feedOptions.percent = false; }
                        if (c[6] == '0') { feedOptions.grade = false; }
                        if (c[7] == '0') { feedOptions.LO = false; }
                        if (c[8] == '0') { feedOptions.full = false; }
                        if (c[9] == '0') { feedOptions.fullLO = false; }
                        if (c[10] == '0') { feedOptions.SubLO = false; }
                        if (c[11] == '0') { feedOptions.CriteriaGrade = false; }
                        if (c[12] == '0') { feedOptions.subdescription = false; }
                        if (c[13] == '0') { feedOptions.includeheader = false; }
                        if (c[14] == '0') { feedOptions.CriteriaPercent = false; }

                    } //using
                    LoadGradeListbox();
                } //try
                catch (System.Exception excep)
                {
                    StackTrace stackTrace = new StackTrace();
                    MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
                }

            } //if
            else
            {
                try
                {
                    Directory.CreateDirectory(DefaultDir);
                }
                catch (System.Exception excep)
                {
                    StackTrace stackTrace = new StackTrace();
                    MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
                }
            }



        }

        private void treeView2_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (treeView2.Nodes[0].Nodes.Count > 0)
            {
                try
                {
                    //need to prompt to save if editing and then changing criteria:
                    if ((treeView2.SelectedNode != e.Node) && EditCriteria)
                    {
                        if (firstcount) { MessageBox.Show("Need to save criteria"); }
                        e.Cancel = true;
                        firstcount = false;
                    }

                }
                catch
                {
                }
            }
        }

        //------------------------ Select grade type:---------------------

        private void gradeTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //int critype;
            GradeTypeForm gradeF = new GradeTypeForm();
            //set grade type to normal by default:
            gradeF.Passvalue = 0;

            gradeF.ShowDialog();

            if (gradeF.Passvalue == 0)
            {
                Smileys = false;
            }
            else
            {
                Smileys = true;
            }
        }


        private void Set_Session(bool a)
        {
            sessionlabel1.Visible = a;
            sessionlabel2.Visible = a;
            sessionbutton2.Visible = a;
            sessionlabel5.Visible = a;
            DeselectSessioncheckBox.Visible = a;
            
            sessionlabel3.Visible = a;
            sessionlabel4.Visible = a;

            if (a)  //true = multi-sessions
            {
                //Session = 1;
                Session = 0;
                sessionlabel2.Text = "1";
                label7.Text = "Criteria weight this session %:";
                if (tabControl1.TabCount < 9)
                {
                    tabControl1.TabPages.Insert(4, tabPage3);
                }
                //checkWbutton.Visible = false;
            }
            else //single (or no) sessions
            {
                if (tabControl1.TabCount > 8)
                {
                    tabControl1.TabPages.Remove(tabPage3);
                }
                Session = 0;
                SessionS = 0;
                label7.Text = "....Weight for this criteria %:";
                checkWbutton.Visible = true;
            }
        }

        private void sessionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int t = 0;
            SessionForm SForm = new SessionForm();
            //set grade type to normal by default:
            SForm.Passvalue = SessionType;

            SForm.ShowDialog();

            t = SForm.Passvalue;
            SessionType = t;
            if (t > 0)
            {
                //Multi-session:
                Set_Session(true);

            }
            else
            {
                //no sessions:
                insessionLabel.Visible = false;
                copyWeightbutton.Visible = false;
                Set_Session(false);
                Reset_Selected(true); //reset criteria selections - only use this in multi-session
            }
            //Change_Session_Selection();
        }


        private void sessionbutton1_Click(object sender, EventArgs e)
        {
            //Previous session button:
            if (Session > 0)
            {
                Session--;
                DeselectSessioncheckBox.Checked = DeselectSession[Session];
                sessionbutton2.Visible = true;
                if (Session == 0)
                {
                    copyWeightbutton.Visible = false;
                    sessionbutton1.Visible = false;
                }
                sessionlabel2.Text = (Session + 1).ToString();
                sessionlabel4.Text = SessionTitle[Session];
                label18.Text = "";
                //treeView2.SelectedNode = treeView2.Nodes[0].Nodes[0];
                SelNode = treeView2.SelectedNode;
                if (EditStudent)
                {
                    Change_Selected_Criteria();
                }
                Change_Session_Selection();
                treeView2.SelectedNode = SelNode;
            }

        }

        private void sessionbutton2_Click(object sender, EventArgs e)
        {
            //Next session button:
            if (SessionS < 1)
            {
                MessageBox.Show("You don't have any sessions!");
                return;
            }
            if (SessionS < 2)
            {
                MessageBox.Show("You only have one session!");
                return;
            }
            if (Session < SessionS - 1)
            {
                label18.Text = "";
                Session++;
                DeselectSessioncheckBox.Checked = DeselectSession[Session];
                sessionbutton1.Visible = true;
                if (Session < SessionS - 1)
                {
                    sessionbutton2.Visible = true;
                }
                else
                {
                    sessionbutton2.Visible = false;
                }
                sessionlabel2.Text = (Session + 1).ToString();
                sessionlabel4.Text = SessionTitle[Session];
                //treeView2.SelectedNode = treeView2.Nodes[0].Nodes[0];
                SelNode = treeView2.SelectedNode;
                if ((Session > 0) && (!EditStudent))
                {
                    copyWeightbutton.Visible = true;
                }
                if (EditStudent)
                {
                    Change_Selected_Criteria();
                }
                Change_Session_Selection();
                treeView2.SelectedNode = SelNode;
            }

        }

        //---------------------------------Manage sessions if used: ------------------------------------------


        private void contextMenuStrip6_Click(object sender, EventArgs e)
        {
            string str;
            if (SessionS > MaxSessions - 1)
            {
                MessageBox.Show("Can only have a maximum of " + Convert.ToString(MaxGrades) + " sessions");
                return;
            }
            if (contextMenuStrip6.Items[0].Selected)
            {
                sessionTextBox.Text = "";
                sessionDescBox.Text = "";
                //Add a new session:
                sessionTextBox.ReadOnly = false;
                sessionDescBox.ReadOnly = false;
                sessionCombo.Enabled = true;
                saveSessionbutton.Visible = true;
                cancelSessionbutton.Visible = true;
                AddSession = true;

                /*InputForm input = new InputForm();
                input.Passvalue = "Enter a title for Session:";
                input.ShowDialog();
                str = input.Passvalue;


                if (str.Length > 0)
                {
                    //sessionTextBox.Enabled = true;
                    

                    if (str.Length > 0)
                    {
                        treeView3.Nodes[0].Nodes.Add(str);
                        treeView3.Nodes[0].Expand();
                        foreach (TreeNode RootNode in treeView3.Nodes)
                        {
                            RootNode.ContextMenuStrip = contextMenuStrip6;
                            foreach (TreeNode ChildNode in RootNode.Nodes)
                            {
                                ChildNode.ContextMenuStrip = contextMenuStrip7;
                            }
                        }
                        SessionTitle[SessionS] = str;
                        if (SessionS == 0)
                        {
                            sessionlabel4.Text = str; //label on assess tab
                        }
                        if (SessionS > 0)
                        {
                            DialogResult dialogResult = MessageBox.Show("Use weights from Session 1 Yes/No?", "Use weights", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                            {
                                for (int s = 0; s < CritZ + 1; s++)
                                {
                                    for (int i = 0; i < MaxSub + 1; i++)
                                    {
                                        crweight[s, i, SessionS] = crweight[s, i, 0];
                                    }
                                }
                            }
                        }
                        SessionS++;                       
                    }
                 

                }*/
            }
        }

        private void contextMenuStrip7_Click(object sender, EventArgs e)
        {
            //Edit or delete session:
            if (contextMenuStrip7.Items[0].Selected)
            {
                //edit session:
                saveSessionbutton.Visible = true;
                cancelSessionbutton.Visible = true;
                firstcount = true;
                sessionTextBox.ReadOnly = false;
                sessionDescBox.ReadOnly = false;
                treeView3.Enabled = false;
                sessionCombo.Enabled = true;

            }
            else if (contextMenuStrip7.Items[1].Selected)
            {
                //remove the session (gets selected node from treeview3_NodeMouseClick):
                DialogResult dialogResult = MessageBox.Show("Delete Yes/No?", "Remove Session", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (S < SessionS)
                    {
                        SessionTitle[S] = SessionTitle[S + 1];
                        SessionTitle[S + 1] = null;
                        sessionTextBox.Text = SessionTitle[S];
                    }
                    else if (S == SessionS)
                    {
                        SessionTitle[S] = null;
                    }
                    treeView3.SelectedNode.Remove();
                    SessionS--;


                }
            }
        }

        private void treeView3_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeView3.SelectedNode = e.Node;
            S = e.Node.Index;
            //Change text boxes as move up and down session treeviw
            sessionTextBox.Text = SessionTitle[S];
            sessionDescBox.Text = SessionDesc[S];
            sessionCombo.Text = SessionWeight[S].ToString();
        }

        private void treeView3_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //need to find which child node is selected in order to remove it ***

           /* treeView3.SelectedNode = e.Node;
            sessionTextBox.Text = SessionTitle[S];
            sessionDescBox.Text = SessionDesc[S];
            sessionCombo.Text = SessionWeight[S].ToString();*/

        }

        private void saveSessionbutton_Click(object sender, EventArgs e)
        {

            //Save sessions to sessiontitle array:
            DialogResult dialogResult = MessageBox.Show("Save Yes/No?", "Save Session title", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                SessionTitle[S] = sessionTextBox.Text;
                SessionWeight[S] = Convert.ToInt32(sessionCombo.Text);
                SessionDesc[S] = sessionDescBox.Text;

                if (SessionTitleChanged && !AddSession)
                {
                    treeView3.Nodes[0].Nodes[S].Text = sessionTextBox.Text;
                    SessionTitleChanged = false;
                    if (S == 0)
                    {
                        sessionlabel4.Text = sessionTextBox.Text; //update label on assess tab
                    }
                }
                if (AddSession)
                {
                    AddNewSession();
                    AddSession = false;
                }
                sessionTextBox.ReadOnly = true;
                sessionDescBox.ReadOnly = true;
                saveSessionbutton.Visible = false;
                cancelSessionbutton.Visible = false;
                sessionCombo.Enabled = false;
                Show_Label("Don't forget to save changes from the File menu!", 2000);
            }
            treeView3.Enabled = true;

        }
        private void AddNewSession()
        {
            string str = sessionTextBox.Text;
            if (str.Length > 0)
            {

                if (str.Length > 0)
                {
                    treeView3.Nodes[0].Nodes.Add(str);
                    treeView3.Nodes[0].Expand();
                    foreach (TreeNode RootNode in treeView3.Nodes)
                    {
                        RootNode.ContextMenuStrip = contextMenuStrip6;
                        foreach (TreeNode ChildNode in RootNode.Nodes)
                        {
                            ChildNode.ContextMenuStrip = contextMenuStrip7;
                        }
                    }
                    SessionTitle[SessionS] = str;
                    if (SessionS == 0)
                    {
                        sessionlabel4.Text = str; //label on assess tab
                    }
                    if (SessionS > 0)
                    {
                        DialogResult dialogResult = MessageBox.Show("Use weights from Session 1 Yes/No?", "Use weights", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            for (int s = 0; s < CritZ + 1; s++)
                            {
                                for (int i = 0; i < MaxSub + 1; i++)
                                {
                                    crweight[s, i, SessionS] = crweight[s, i, 0];
                                }
                            }
                        }
                    }
                    SessionS++;
                }


            }
        }

        private void cancelSessionbutton_Click(object sender, EventArgs e)
        {
            //Don't save the session if cancelled:
            DialogResult dialogResult = MessageBox.Show("Cancel edit Yes/No?", "Cancel", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                saveSessionbutton.Visible = false;
                cancelSessionbutton.Visible = false;
                sessionDescBox.ReadOnly = true;
                sessionTextBox.ReadOnly = true;
                sessionCombo.Enabled = false;
                Show_Label("Don't forget to save changes!", 1500);
            }
            treeView3.Enabled = true;

        }

        private void sessionTextBox_TextChanged(object sender, EventArgs e)
        {
            SessionTitleChanged = true;
        }

        private void treeView3_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if ((treeView3.SelectedNode != e.Node) && (saveSessionbutton.Visible == true))
            {
                if (firstcount) { MessageBox.Show("Need to save session"); }
                e.Cancel = true;
                firstcount = false;
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
            firstcount = false;
        }

        private void saveSessionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog3.DefaultExt = "ssn";
            if (UnitFilePath.Length < 1)
            {
                UnitFilePath = DefaultDir;
                saveFileDialog3.InitialDirectory = UnitFilePath;
            }
            saveFileDialog3.FileName = "";
            saveFileDialog3.Filter = "Session files (.ssn)|*.ssn";

            saveFileDialog3.ShowDialog();

        }

        private void Save_Sessions(string filename)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    sw.WriteLine("Unit: " + UnitTitletextBox.Text);
                    sw.WriteLine("Assessment title: " + assess.Title);
                    sw.WriteLine("Session file");
                    for (int i = 0; i < SessionS; i++)
                    {
                        sw.WriteLine("Session " + Convert.ToString(i + 1) + ": " + SessionTitle[i]);
                        sw.WriteLine("SessDesc:");
                        sw.WriteLine(SessionDesc[i]);
                        sw.WriteLine("EndDesc:");
                        if (!SessionsEqual)
                        {
                            sw.WriteLine("Weight: " + SessionWeight[i].ToString());
                        }
                        else
                        {
                            sw.WriteLine("Weight: equal");
                        }
                        sw.Write("SelectedCriteria: ");
                        for (int c = 0; c < MaxCriteria; c++)
                        {
                            for (int s = MaxSub; s > -1; s--)
                            {
                                if (crSelected[c, s, i])
                                {
                                    sw.Write("1");
                                }
                                else
                                {
                                    sw.Write("0");
                                }
                            }
                        }
                        sw.WriteLine();
                    }

                    sw.Close();
                    SessionFile = filename;
                    SessionPath = Path.GetDirectoryName(filename);
                }
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void loadSessionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!treeView3.Enabled)
            {
                MessageBox.Show("Editing sessions - save first");
                return;
            }
            if (SessionPath.Length < 1)
            {
                SessionPath = UnitFilePath;
            }
            openFileDialog3.InitialDirectory = SessionPath;
            openFileDialog3.DefaultExt = "ssn";
            openFileDialog3.Filter = "Session files (*.ssn) |*.ssn";
            openFileDialog3.FileName = "";
            openFileDialog3.ShowDialog();
        }


        private void Load_Sessions(string filename)
        {
            string str;
            string[] str2 = new string[2];
            string str3 = "";
            char ch = '1';

            try
            {
                for (int i = 0; i < MaxSessions; i++)
                {
                    SessionTitle[i] = null;
                    SessionS = 0;
                }

                treeView3.Nodes[0].Remove();
                treeView3.Nodes.Add("Sessions");

                // Create an instance of StreamWriter to read sessions from file:
                using (StreamReader sw = new StreamReader(filename))
                {
                    while (!sw.EndOfStream)
                    {
                        str = sw.ReadLine();
                        str.Trim();
                        if (str.StartsWith("Session file"))
                        {
                        }
                        else if (str.StartsWith("Session"))
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
                            SessionTitle[SessionS] = str3;
                            treeView3.Nodes[0].Nodes.Add(str3);

                            treeView3.Nodes[0].Expand();
                            foreach (TreeNode RootNode in treeView3.Nodes)
                            {
                                RootNode.ContextMenuStrip = contextMenuStrip6;
                                foreach (TreeNode ChildNode in RootNode.Nodes)
                                {
                                    ChildNode.ContextMenuStrip = contextMenuStrip7;
                                }
                            }

                        }
                        else if (str.StartsWith("SessDesc:"))
                        {
                            str3 = "";
                            while (!str.StartsWith("EndDesc:"))
                            {
                                str = sw.ReadLine();
                                if (!str.StartsWith("EndDesc:"))
                                {
                                    str3 = str3 + str;
                                }
                            }
                            SessionDesc[SessionS] = str3;

                        }
                        else if (str.StartsWith("Weight:"))
                        {
                            if (str.Contains("equal"))
                            {
                                SessionsEqual = true;
                                sessionCombo.Visible = false;
                                sessionWlabel.Visible = false;
                                checkBox2.Checked = true;
                            }
                            else
                            {
                                SessionsEqual = false;
                                int a = str.IndexOf(":");
                                str3 = str.Substring(a + 1).Trim();
                                SessionWeight[SessionS] = Convert.ToInt32(str3);
                                sessionCombo.Visible = true;
                                sessionWlabel.Visible = true;
                                checkBox2.Checked = false;
                            }
                        }
                        else if (str.StartsWith("SelectedCriteria:"))
                        {
                            int a = str.IndexOf(":");
                            str3 = str.Substring(a + 1).Trim();
                            int t = 0;
                            for (int c = 0; c < MaxCriteria; c++)
                            {
                                for (int s = MaxSub; s > -1; s--)
                                {
                                    ch = str3[t];
                                    if (ch == '0')
                                    {
                                        crSelected[c, s, SessionS] = false;
                                    }
                                    t++;
                                    if (t == str3.Length)
                                    {
                                        break;
                                    }
                                }
                                if (t == str3.Length)
                                {
                                    break;
                                }
                            }
                            SessionS++;
                        }

                    }
                    sw.Close();
                    SessionFile = filename;
                    SessionPath = Path.GetDirectoryName(filename);
                    sessionTextBox.Text = SessionTitle[0];
                    sessionCombo.Text = SessionWeight[0].ToString();
                    sessionDescBox.Text = SessionDesc[0];
                    treeView3.SelectedNode = treeView3.Nodes[0].Nodes[0];

                }
            }//try
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }

        }

        private void newCriteriaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EditCriteria)
            {
                MessageBox.Show("Editing criteria - save this first");
                return;
            }
            DialogResult dialogResult = MessageBox.Show("This will Delete all current criteria & form data - proceed Yes/No?", "New Criteria", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Clear_All_Criteria(true);
                Clear_Form_Data();
                MaxSub = 20;
                Reset_Selected(true); //clear selected criteria

                assess.Title = "";
                assessTitleBox.Text = "";
                assessTBox.Text = ""; //update peer and mod
                ATitleBox.Text = "";
                assess.Description = "";
                assess.Code = "";
                ACodeBox.Text = ""; //update peer and mod forms
                assessCBox.Text = "";
                assess.Weight = "";
                AWeightBox.Text = ""; //update peer and mod
                assessWeightBox.Text = "";
                DialogResult dialogResult2 = MessageBox.Show("Remove Assessment Learning Outcomes Yes/No?", "Learning Outcomes", MessageBoxButtons.YesNo);
                if (dialogResult2 == DialogResult.Yes)
                {
                    assess.LOs = "";
                    listBox3.Items.Clear();
                }
                Open_Assessment_Form(true);
                Show_Label("Now add Criteria and save Assessment from File menu!", 4000);
                //assessTitleBox.Focus();
                //assessTitleBox.BackColor = Color.Aquamarine;
            }
        }

        private void newSessionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!treeView3.Enabled)
            {
                MessageBox.Show("Editing sessions - save first");
                return;
            }
            DialogResult dialogResult = MessageBox.Show("This will delete all current sessions - proceed Yes/No?", "New Sessions", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Remove_Sessions();
            }

        }

        private void Remove_Sessions()
        {
            for (int i = 0; i < MaxSessions; i++)
            {
                SessionTitle[i] = null;
                SessionS = 0;
            }
            sessionTextBox.Text = "";
            treeView3.Nodes[0].Remove();
            treeView3.Nodes.Add("Sessions");

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void promptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string prompts = "";
            if (promptsoff)
            {
                prompts = "ON";
            }
            else
            {
                prompts = "OFF";
            }
            DialogResult dialogResult = MessageBox.Show("Switch editing prompts " + prompts + " Yes/No?", "Editing Prompts", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                promptsoff = !promptsoff;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            rep1.ShowDialog();
            //button1.Visible = false;
        }




        private void weightingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            return;
            string fine = "";
            string oldfine = "";
            if (fineWeight == 1)
            {
                fine = "Coarse 5%";
                oldfine = "Medium 1%";
            }
            else if (fineWeight == 5)
            {
                fine = "Fine (0.5%)";
                oldfine = "Coarse (5%)";
            }
            else
            {

            }
            DialogResult dialogResult = MessageBox.Show("Weightings for criteria are set to: " + oldfine + ", change to: " + fine + " Yes/No?", "Criteria Weighting", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //fineWeight = !fineWeight;
                Build_Criteria_List();
            }
        }

        private void assessTitleBox_TextChanged(object sender, EventArgs e)
        {
            assessTitleBox.BackColor = assesstitleBackcolor;
        }

        private void Open_Assessment_Form(bool b)
        {
            string mode = "read";
            //assessForm AForm = new assessForm();

            AForm.Passvalue[0] = assessTitleBox.Text;
            AForm.Passvalue[1] = assess.Description;
            AForm.Passvalue[2] = assess.Code;
            AForm.Passvalue[3] = assess.Weight;
            //AForm.Passvalue[5] = assess.LOs;
            if (b) { mode = "write"; }
            AForm.Passvalue[4] = mode;
            AForm.thisLOFile = LOFile;
            AForm.ShowDialog();
            assessTitleBox.Text = AForm.Passvalue[0];
            assess.Title = assessTitleBox.Text;

            assess.Description = AForm.Passvalue[1];

            assess.Code = AForm.Passvalue[2];

            assess.Weight = AForm.Passvalue[3];

            Copy_Criteria_Data(); //copy data to peer and mod forms etc.

            //assess.LOs = AForm.Passvalue[5];
        }
        private void descriptionButton_Click(object sender, EventArgs e)
        {
            //if editing criteria allow assessment to be edited
            Open_Assessment_Form(EditCriteria);

        }

        private void feedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            feedselectForm FeedForm = new feedselectForm();
            FeedForm.Passvalue[0] = feedOptions.generic;
            FeedForm.Passvalue[1] = feedOptions.description;
            FeedForm.Passvalue[2] = feedOptions.suggested;
            FeedForm.Passvalue[3] = feedOptions.additional;
            FeedForm.Passvalue[4] = feedOptions.criteriaComment;
            FeedForm.Passvalue[5] = feedOptions.percent;
            FeedForm.Passvalue[6] = feedOptions.grade;
            FeedForm.Passvalue[7] = feedOptions.LO;
            FeedForm.Passvalue[8] = feedOptions.full;
            FeedForm.Passvalue[9] = feedOptions.fullLO;
            FeedForm.Passvalue[10] = feedOptions.SubLO;
            FeedForm.Passvalue[11] = feedOptions.CriteriaGrade;
            FeedForm.Passvalue[12] = feedOptions.subdescription;
            FeedForm.Passvalue[13] = feedOptions.includeheader;
            FeedForm.Passvalue[14] = feedOptions.CriteriaPercent;

            FeedForm.ShowDialog();
            feedOptions.generic = FeedForm.Passvalue[0];
            feedOptions.description = FeedForm.Passvalue[1];
            feedOptions.suggested = FeedForm.Passvalue[2];
            feedOptions.additional = FeedForm.Passvalue[3];
            feedOptions.criteriaComment = FeedForm.Passvalue[4];
            feedOptions.percent = FeedForm.Passvalue[5];
            feedOptions.grade = FeedForm.Passvalue[6];
            feedOptions.LO = FeedForm.Passvalue[7];
            feedOptions.full = FeedForm.Passvalue[8];
            feedOptions.fullLO = FeedForm.Passvalue[9];
            feedOptions.SubLO = FeedForm.Passvalue[10];
            feedOptions.CriteriaGrade = FeedForm.Passvalue[11];
            feedOptions.subdescription = FeedForm.Passvalue[12];
            feedOptions.includeheader = FeedForm.Passvalue[13];
            feedOptions.CriteriaPercent = FeedForm.Passvalue[14];
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            addCommentsX();
        }
        private void addComments()
        {
            int s = 0;
            if (CriteriaSelected)//criteria or sub-criteria selected?
            {
                s = MaxSub;
            }
            else
            {
                s = SSub;
            }

            addcommentForm.Passvalue[0] = criteriaTitleBox.Text;
            addcommentForm.Passvalue[1] = crComment[SCriteria, s, Session];
            //addcommentForm.CForm = CommentsForm;
            addcommentForm.ComFile = CommentFile;
            addcommentForm.ShowDialog();

            crComment[SCriteria, s, Session] = addcommentForm.Passvalue[1];
            if (addcommentForm.ComFile != CommentFile && addcommentForm.ComFile != null)
            {
                CommentFile = addcommentForm.ComFile;
            }
        }
        private void addCommentsX() //open XML add comments form - experimental version - not working
        {
            int s = 0;
            if (CriteriaSelected)//criteria or sub-criteria selected?
            {
                s = MaxSub;
            }
            else
            {
                s = SSub;
            }

            addcommentForm2.Passvalue[0] = criteriaTitleBox.Text;
            addcommentForm2.Passvalue[1] = crComment[SCriteria, s, Session];
            //addcommentForm.CForm = CommentsForm;
            addcommentForm2.ComFile = CommentFile;
            addcommentForm2.ShowDialog();

            crComment[SCriteria, s, Session] = addcommentForm2.Passvalue[1];
            if (addcommentForm2.ComFile != CommentFile && addcommentForm2.ComFile != null)
            {
                CommentFile = addcommentForm2.ComFile;
            }
        }



        private void editAssess_Click(object sender, EventArgs e)
        {
            Open_Assessment_Form(true);
            Show_Label("Now save Assessment from File menu!", 2000);
        }

        private void createSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!insertingCriteria)
            {
                Change_Selected_Criteria();
            }
        }



        private void treeView2_DoubleClick(object sender, EventArgs e)
        {
            Select_Session();

        }
        private void Select_Session()
        {
            //if double clicked while selecting criteria in sessions do this:
            int s = 0;
           /* if ((SessionType == 0) || (EditStudent))  //if single session or not editing criteria
            {
                return;
            }*/
            //Allow single sessions to be selected too!
            /*if (EditStudent)  //if not editing criteria
            {
                return;
            }*/
            if ((CriteriaSelectionType < 2) && EditStudent || CriteriaSelectionType == 0)
            {
                return;
            }
            if (CriteriaSelected)//criteria or sub-criteria selected?
            {
                s = MaxSub;
            }
            else
            {
                s = SSub;
            }
            if (crSelected[SCriteria, s, Session])
            {
                crSelected[SCriteria, s, Session] = false;
                Marks[SCriteria, s, Session] = "n/a";             
                label18.Text = "n/a";
            }
            else
            {
                crSelected[SCriteria, s, Session] = true;
            }

            if (crSelected[SCriteria, s, Session])
            {
                //select this session
                treeView2.SelectedNode.ForeColor = Color.Black;
                treeView2.SelectedNode.NodeFont = new Font(TVFont, FontStyle.Regular);
                insessionLabel.Visible = true;
            }
            else
            {
                treeView2.SelectedNode.ForeColor = Color.Gray;

                treeView2.SelectedNode.NodeFont = new Font(TVFont, FontStyle.Strikeout | FontStyle.Italic);
                insessionLabel.Visible = false;
            }
        }

        private void Change_Session_Selection()
        {
            bool sel = true;
            bool isChecked = false;
            try
            {
                for (int c = 0; c < CritZ + 1; c++)
                {
                    for (int s = MaxSub; s > -1; s--)
                    {
                        sel = crSelected[c, s, Session];
                        if (s == MaxSub)
                        {
                            if (sel)
                            {
                                treeView2.Nodes[0].Nodes[c].ForeColor = Color.Black;
                                treeView2.Nodes[0].Nodes[c].NodeFont = new Font(TVFont, FontStyle.Regular);
                                isChecked = nodeChecked[c, s, Session];
                                treeView2.Nodes[0].Nodes[c].Checked = isChecked;

                            }
                            else
                            {
                                treeView2.Nodes[0].Nodes[c].ForeColor = Color.Gray;
                                treeView2.Nodes[0].Nodes[c].NodeFont = new Font(TVFont, FontStyle.Strikeout | FontStyle.Italic);
                                treeView2.Nodes[0].Nodes[c].Checked = false;
                            }
                        }
                        else
                        {
                            try
                            {
                                if (sel)
                                {
                                    treeView2.Nodes[0].Nodes[c].Nodes[s].ForeColor = Color.Black;
                                    treeView2.Nodes[0].Nodes[c].Nodes[s].NodeFont = new Font(TVFont, FontStyle.Regular);
                                    isChecked = nodeChecked[c, s, Session];
                                    treeView2.Nodes[0].Nodes[c].Nodes[s].Checked = isChecked;
                                }
                                else
                                {
                                    treeView2.Nodes[0].Nodes[c].Nodes[s].ForeColor = Color.Gray;
                                    treeView2.Nodes[0].Nodes[c].Nodes[s].NodeFont = new Font(TVFont, FontStyle.Strikeout | FontStyle.Italic);
                                    treeView2.Nodes[0].Nodes[c].Nodes[s].Checked = false;
                                }
                            }
                            catch
                            {
                            }
                        }//if maxsub

                    }//for int s
                }//for int n

            }//try
            catch
            {
            }

        }

        private void sessionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Build_Session_Weight()
        {
            //Populate the criteria weight combo box:
            double i = 100;
            double f = 1.0;
            sessionCombo.Items.Clear();

            f = 1;

            while (i > 0)
            {
                sessionCombo.Items.Add(Convert.ToString(i));
                i = i - f;
            }
        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                SessionsEqual = true;
                sessionCombo.Visible = false;
                sessionWlabel.Visible = false;
            }
            else
            {
                SessionsEqual = false;
                sessionCombo.Visible = true;
                sessionWlabel.Visible = true;
            }
        }

        private void repCancelbutton1_Click(object sender, EventArgs e)
        {
            replicate_Criteria = false;
            treeView2.SelectedNode = SelNode;
            Show_Label("Don't forget to save changes!", 1500);
            repCancelbutton1.Visible = false;
        }

        private void repCancelbutton2_Click(object sender, EventArgs e)
        {
            replicate_Feedback = false;
            treeView2.SelectedNode = SelNode;
            Show_Label("Don't forget to save changes!", 1500);
            repCancelbutton2.Visible = false;
        }

        private void repCancelbutton3_Click(object sender, EventArgs e)
        {
            replicate_LO = false;
            treeView2.SelectedNode = SelNode;
            Show_Label("Don't forget to save changes!", 1500);
            repCancelbutton3.Visible = false;
        }


        private void LObutton_Click(object sender, EventArgs e)
        {
            if (!EditStudent)
            {
                listBox3.Enabled = true;
                clearLObutton.Visible = true;
                selectLOs = true;
                Select_LO();              
            }
        }
        private void Select_LO()
        {
            listBox3.Enabled = true;

        }




        private void showToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Show Learning Outcomes for criteria?", "Learning Outcomes", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                LOtextBox.Visible = true;
                LObutton.Visible = true;
                LOs = true;
            }
            else
            {
                LOtextBox.Visible = false;
                LObutton.Visible = false;
                LOs = false;
            }
        }



        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        //---------------------------------------------------------------------------
        // Learning Outcomes section:
        //---------------------------------------------------------------------------

        void Add_LO()
        {
            //Add or insert new LO
            string str, str2, str3;
            str = loTitleBox.Text;
            str2 = loDescBox.Text;
            str3 = LOcomboBox.Text;
            int insI = 0;

            if (listBox2.Items.Count > 0)
            {
                insI = listBox2.SelectedIndex;
            }
            if (str.Length > 0)
            {
                if (insertLO)
                {
                    LOTitle.Insert(insI + 1, str);
                    LODesc.Insert(insI + 1, str2);
                    LOType.Insert(insI + 1, str3);
                    listBox2.Items.Insert(insI + 1, str2);
                    listBox2.SelectedIndex = listBox2.SelectedIndex + 1;

                }
                else
                {
                    LOTitle.Add(str);
                    LODesc.Add(str2);
                    LOType.Add(str3);
                    listBox2.Items.Add(str);
                    listBox2.SelectedIndex = listBox2.Items.Count - 1;

                }

            }
        }


        private void saveLObutton_Click(object sender, EventArgs e)
        {
            string temp = "";
            temp = LOcomboBox.Text;
            if (loTitleBox.Text.Trim().Length > 0)
            {
                if (!addLOmode)
                {
                    try
                    {
                        int i = listBox2.SelectedIndex;

                        LOTitle[i] = loTitleBox.Text;
                        LODesc[i] = loDescBox.Text;
                        LOType[i] = LOcomboBox.Text;
                        listBox2.Items.RemoveAt(i);
                        listBox2.Items.Insert(i, loTitleBox.Text);
                        listBox2.SelectedIndex = oldindex;


                    }
                    catch
                    {
                    }
                }
                else
                {
                    Add_LO();
                    insertLO = false;
                    addLOmode = false;

                }

                loTitleBox.ReadOnly = true;
                loDescBox.ReadOnly = true;
                LOcomboBox.Enabled = false;
                LOcomboBox.Text = temp;
                saveLObutton.Visible = false;
                cancelLObutton.Visible = false;
                transferButton.Visible = true;
                listBox2.EndUpdate();
                savedLO = false;
            }
            else
            {
                MessageBox.Show("Learning Outcome title cannot be empty");
            }
        }

        private void addLOToolStripMenuItem_Click(object sender, EventArgs e)
        {

            oldindex = listBox2.SelectedIndex;

            //add LOs
            loTitleBox.ReadOnly = false;
            loDescBox.ReadOnly = false;
            saveLObutton.Visible = true;
            cancelLObutton.Visible = true;
            transferButton.Visible = false;
            LOcomboBox.Enabled = true;
            loTitleBox.Text = "";
            loDescBox.Text = "";
            addLOmode = true;
            insertLO = false;
            //editLO = true;

        }

        private void editStripMenuItem_Click(object sender, EventArgs e) 
        {  //edit LO
            string str;
         
            listBox2.BeginUpdate();
            if (listBox2.SelectedIndex >= 0)
            {              
                oldindex = listBox2.SelectedIndex;
                //editingLO = true;
                addLOmode = false;
                saveLObutton.Visible = true;
                cancelLObutton.Visible = true;
                transferButton.Visible = false;

                loTitleBox.ReadOnly = false;
                loDescBox.ReadOnly = false;
                LOcomboBox.Enabled = true;
                try
                {
                    str = listBox2.SelectedItem.ToString();
                    loTitleBox.Text = str.Trim();
                    loDescBox.Text = LODesc[listBox2.SelectedIndex];
                }
                catch
                {
                }
            }
            else
            {
                MessageBox.Show("Select the item to edit");
            }
        }

        private void insertStripMenuItem_Click(object sender, EventArgs e)
        {  //insert LO

            listBox2.BeginUpdate();
            oldindex = listBox2.SelectedIndex;

            addLOmode = true;
            insertLO = true;
            saveLObutton.Visible = true;
            cancelLObutton.Visible = true;
            transferButton.Visible = false;

            loTitleBox.ReadOnly = false;
            loDescBox.ReadOnly = false;
            LOcomboBox.Enabled = true;
        }

        private void removeLOtripMenuItem_Click(object sender, EventArgs e)
        {
            string str;
            //remove the LO node 
            DialogResult dialogResult = MessageBox.Show("Delete Yes/No?", "Remove LO", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {

                int i = listBox2.SelectedIndex;
                if (i > -1)
                {
                    str = listBox2.Items[i].ToString();
                    LOTitle.Remove(str);
                    LODesc.RemoveAt(i);
                    listBox2.Items.Remove(str);
                }
                else
                {
                    MessageBox.Show("Select an LO to delete");
                }

            }
        }

        private void cancelLObutton_Click(object sender, EventArgs e)
        {  //cancel adding LO
            loTitleBox.ReadOnly = true;
            loDescBox.ReadOnly = true;
            LOcomboBox.Enabled = false;
            saveLObutton.Visible = false;
            cancelLObutton.Visible = false;
            transferButton.Visible = true;
            listBox2.EndUpdate();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LOFilePath.Length < 1)
            {
                LOFilePath = UnitFilePath;
                saveFileDialog5.InitialDirectory = LOFilePath;
            }
            saveFileDialog5.InitialDirectory = LOFilePath;
            saveFileDialog5.ShowDialog();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {  //load LOs
            if (LOFilePath.Length < 1)
            {
                LOFilePath = UnitFilePath;
            }
            openFileDialog4.FileName = "";
            openFileDialog4.InitialDirectory = LOFilePath;
            openFileDialog4.ShowDialog();
        }

        private void saveFileDialog5_FileOk(object sender, CancelEventArgs e)
        {
            SaveLOToFile(saveFileDialog5.FileName);
            //LOFile = saveFileDialog5.FileName;
        }

        private void SaveLOToFile(string filename)
        {
            // write grades to file:
            try
            {
                //int i =0;
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    for (int i = 0; i < listBox2.Items.Count; i++)
                    {
                        sw.WriteLine("Learning Outcome: ");
                        sw.Write(LOTitle[i] + " || ");
                        sw.Write(LOType[i] + " || ");
                        sw.WriteLine(LODesc[i]);
                        sw.WriteLine("EndLO:");
                    }
                    sw.Close();
                    LOFile = filename;
                    LOFilePath = Path.GetDirectoryName(LOFile);
                    savedLO = true;
                }
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void ReadLOFromFile(string filename)
        {

            string[] parts = new string[3];
            string str = "";
            string line = "";


            try
            {
                Remove_LOs();

                // Create an instance of StreamWriter to read grades from file:
                using (StreamReader sw = new StreamReader(filename))
                {
                    while (!sw.EndOfStream)
                    {
                        str = sw.ReadLine();
                        if (str.StartsWith("Learning Outcome: "))
                        {
                        }
                        else if (str.StartsWith("EndLO:"))
                        {
                            parts = line.Split(new string[] { "||" }, System.StringSplitOptions.None);
                            if (parts.Count() > 0)
                            {
                                LOTitle.Add(parts[0].Trim());
                                listBox2.Items.Add(parts[0].Trim());
                            }
                            if (parts.Count() > 1)
                            {
                                LOType.Add(parts[1].Trim());
                            }
                            if (parts.Count() > 2)
                            {
                                LODesc.Add(parts[2].Trim());
                            }

                            line = "";
                        }
                        else
                        {
                            line = line + str;

                        }
                    }
                    sw.Close();
                    LOFile = filename;
                    LOFilePath = Path.GetDirectoryName(LOFile);
                }

            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void Remove_LOs()
        {

            try
            {
                LOTitle.Clear();
                LODesc.Clear();
            }
            catch
            {
            }

            listBox2.Items.Clear();
            loTitleBox.Text = "";
            loDescBox.Text = "";
            //listBox3.Items.Clear(); //remove LOs froma sessment tab too

        }

        private void openFileDialog4_FileOk(object sender, CancelEventArgs e)
        {
            ReadLOFromFile(openFileDialog4.FileName);
            //LOFile = openFileDialog4.FileName;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clear all current LOs to start a new series of LOs
            DialogResult dialogResult = MessageBox.Show("Clear all current Learning Outcomes Yes/No?", "Clear LO", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                listBox2.Items.Clear();
                loTitleBox.Text = "";
                loDescBox.Text = "";
                LOTitle.Clear();
                LODesc.Clear();
                savedLO = false;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {  //lo listbox
            try
            {
                int i = listBox2.SelectedIndex;
                loTitleBox.Text = LOTitle[i];
                loDescBox.Text = LODesc[i];
                LOcomboBox.Text = LOType[i];
            }
            catch
            {
            }
        }

        private void clearLObutton_Click(object sender, EventArgs e)
        {
            LOtextBox.Text = "";
        }

        private void listBox3_DoubleClick(object sender, EventArgs e)
        {           //LO listbox3 on assess tab
            try
            {
                if (listBox3.SelectedIndex >= 0)
                {
                    if (selectLOs)
                    {
                        string str = listBox3.SelectedItem.ToString();
                        if (LOtextBox.Text.Contains(str))
                        {
                        }
                        else
                        {
                            if (LOtextBox.Text.Trim().Length > 0)
                            {
                                LOtextBox.Text = LOtextBox.Text + System.Environment.NewLine + str;

                            }
                            else
                            {
                                LOtextBox.Text = str;
                            }
                        }
                    }
                    else
                    {
                        //show lo on form
                        int LO = listBox3.SelectedIndex;
                        ShowLO LOForm = new ShowLO();
                        LOForm.LOTitle = LOTitle[LO];
                        LOForm.LODesc = LODesc[LO];
                        LOForm.LOType = LOType[LO];
                        LOForm.ShowDialog();
                    }
                }

            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }

        }

        private void transferButton_Click(object sender, EventArgs e)
        {
            string str = "";
            string nl = System.Environment.NewLine;
            DialogResult dialogResult = MessageBox.Show("Use these Learning Outcomes in the current Assessment Yes/No?", "Use LOs", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                DialogResult dialogResult2 = MessageBox.Show("Overwrite currently selected LOs Yes/No?", "Overwrite LOs", MessageBoxButtons.YesNo);
                if (dialogResult2 == DialogResult.Yes)
                {
                    listBox3.Items.Clear();
                    assess.LOs = "";
                    for (int i = 0; i < listBox2.Items.Count; i++)
                    {
                        str = listBox2.Items[i].ToString();
                        listBox3.Items.Add(str);
                        assess.LOs = assess.LOs + str + nl;
                    }

                }
            }
        }



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            help helpForm = new help();

            if (keyData == Keys.F1)
            {
                int sel = tabControl1.SelectedIndex;
                switch (sel)
                {
                    case 0: //Unit tab
                        helpForm.helpfile = DefaultDir + "helpfiles\\UnitHelp.rtf";
                        break;
                    case 1: //Grades tab  
                        helpForm.helpfile = DefaultDir + "helpfiles\\GradesHelp.rtf";
                        break;
                    case 2: //LO tab
                        helpForm.helpfile = DefaultDir + "helpfiles\\LOHelp.rtf";
                        break;
                    case 3: //Assess tab  
                        if (!EditStudent)
                        {
                            helpForm.helpfile = DefaultDir + "helpfiles\\AssessHelp.rtf";
                        }
                        else
                        {
                            helpForm.helpfile = DefaultDir + "helpfiles\\MarkingHelp.rtf";
                        }
                        break;
                    case 4: //Sessiontab
                        if (SessionType ==0)
                        {
                            helpForm.helpfile = DefaultDir + "helpfiles\\PeerHelp.rtf";                         
                        }
                        else
                        {
                            helpForm.helpfile = DefaultDir + "helpfiles\\SessionHelp.rtf";
                        }
                        break;
                    case 5: //Unit tab
                        if (SessionType == 0)
                        {
                            helpForm.helpfile = DefaultDir + "helpfiles\\ModHelp.rtf";
                        }
                        else
                        {
                            helpForm.helpfile = DefaultDir + "helpfiles\\PeerHelp.rtf"; 
                        }
                        break;
                    case 6:
                        if (SessionType == 0)
                        {
                            helpForm.helpfile = DefaultDir + "helpfiles\\ExtHelp.rtf";
                        }
                        else
                        {
                            helpForm.helpfile = DefaultDir + "helpfiles\\ModHelp.rtf";                          
                        }
                        break;
                    case 7:
                        if (SessionType == 0)
                        {
                            helpForm.helpfile = DefaultDir + "helpfiles\\StudentHelp.rtf";
                        }
                        else
                        {
                            helpForm.helpfile = DefaultDir + "helpfiles\\ExtHelp.rtf";
                        }
                        break;
                    case 8:
                        if (SessionType == 1)
                        {
                            helpForm.helpfile = DefaultDir + "helpfiles\\StudentHelp.rtf";
                        }                     
                        break;

                }
                helpForm.ShowDialog();
                return true;

            }
            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CheckWeights() //not used
        {
            float TotW = 0;
            int TotS = 0;
            float SubW = 0;
            bool ok = true;
            if (SessionType == 0) //single session
            {
                if (CriteriaType == 0 || CriteriaType == 2)  //criteria and sub-criteria are independant of each other
                {
                    for (int s = 0; s < CritZ + 1; s++)
                    {
                        for (int i = 0; i < MaxSub + 1; i++)
                        {
                            TotW = TotW + crweight[s, i, Session];
                            /* For future purpose:
                             if (crSelected[s, i, Session])
                            {
                                TotS = TotS + crweight[s, i];
                            }*/
                        }
                    }
                    if (TotW != 100)
                    {
                        MessageBox.Show("Total weight for criteria is " + (TotW).ToString() + " but should be 100%");
                    }
                    else
                    {
                        MessageBox.Show("Weights ok = 100%");
                    }
                }
                else if (CriteriaType == 1 || CriteriaType == 3)  //sub-criteria are part of the weight for criteria
                {
                    ok = true;
                    for (int s = 0; s < CritZ + 1; s++)
                    {
                        TotW = TotW + crweight[s, MaxSub, Session];
                        SubW = 0;
                        for (int i = 0; i < MaxSub; i++)
                        {
                            SubW = SubW + crweight[s, i, Session];
                        }
                        if (SubW != 100)
                        {
                            MessageBox.Show("Total weight for sub-criteria in criteria " + (s + 1).ToString() + " is " + SubW.ToString() + " but should be 100%");
                            ok = false;
                        }
                    }
                    if (TotW != 100)
                    {
                        MessageBox.Show("Total weight for criteria is " + (TotW).ToString() + " but should be 100%");
                        ok = false;
                    }
                    if (ok)
                    {
                        MessageBox.Show("Weights ok = 100%");
                    }

                }
            }
        }

        private void CheckWeightReport()
        {
            float TotW = 0;
            int TotS = 0;
            float SubW = 0;
            bool ok = true;
            int TempSessionNum = 0;
            string nl = System.Environment.NewLine;
            if (SessionType == 0)
            {
                weightForm.WReport = "This is a report on the weights for each criteria:" + nl;
            }
            else
            {
                weightForm.WReport = "This is a report on the weights for the criteria within each session, and on the weightings of sessions:" + nl;
            }
            weightForm.WReport = weightForm.WReport + "_______________________________________________________" + nl;
            string sess = "";

            // if (SessionType == 0) //single session
            //{
            int temp = 0; //temp value to allow code to run with no sessions if criteria type 0
            if (SessionType == 0)
            {
                temp = 1;
            }
            else
            {
                temp = SessionS;
            }

            for (int se = 0; se < temp; se++)
            {
                TotW = 0;
                SubW = 0;
                ok = true;
                if (CriteriaType == 0 || CriteriaType == 2 || CriteriaType == 4)  //criteria and sub-criteria are independant of each other
                {
                    for (int s = 0; s < CritZ + 1; s++)
                    {
                        for (int i = 0; i < MaxSub + 1; i++)
                        {
                            if (SessionType == 0 || crSelected[s, i, se])
                            { TotW = TotW + crweight[s, i, se]; }
                        }
                    }
                    if (SessionType == 0) { sess = ""; }
                    else { sess = "In Session " + (se + 1).ToString() + ":  "; }
                    if (TotW != 100)
                    {
                        weightForm.WReport = weightForm.WReport + sess + "Total weight for criteria is " + TotW.ToString() + "% ,but should be 100%" + nl;
                        ok = false;
                        //MessageBox.Show("Total weight for criteria is " + (TotW + 1).ToString() + " but should be 100%");
                    }
                    else
                    {
                        weightForm.WReport = weightForm.WReport + sess + "Criteria weights ok at 100%" + nl;

                        //MessageBox.Show("Weights ok = 100%");
                    }
                }
                else if (CriteriaType == 1 || CriteriaType == 3)  //sub-criteria are part of the weight for criteria
                {
                    TotW = 0;
                    SubW = 0;
                    ok = true;
                    if (SessionType == 0) { sess = ""; }
                    else { sess = "In Session " + (se + 1).ToString() + ":  "; }
                    for (int s = 0; s < CritZ + 1; s++)
                    {
                        if (SessionType == 0 || crSelected[s, MaxSub, se])
                        { TotW = TotW + crweight[s, MaxSub, se]; }
                        SubW = 0;
                        for (int i = 0; i < MaxSub; i++)
                        {
                            if (SessionType == 0 || crSelected[s, i, se])
                            { SubW = SubW + crweight[s, i, se]; }
                        }

                        if (SubW != 100)
                        {
                            if (Has_Subcriteria(s))
                            {
                                weightForm.WReport = weightForm.WReport + sess + "Total weight for sub-criteria in criteria " + (s + 1).ToString() + " is " + SubW.ToString() + "% ,but should be 100%" + nl;
                                //MessageBox.Show("Total weight for sub-criteria in criteria " + (s + 1).ToString() + " is " + SubW.ToString() + " but should be 100%");
                                ok = false;
                            }
                        }
                    }
                    if (TotW != 100)
                    {
                        weightForm.WReport = weightForm.WReport + sess + "Total weight for all criteria is " + TotW.ToString() + "% ,but should be 100%" + nl;
                        //MessageBox.Show("Total weight for criteria is " + (TotW + 1).ToString() + " but should be 100%");
                        ok = false;
                    }
                    if (ok)
                    {
                        weightForm.WReport = weightForm.WReport + sess + "Criteria weights ok at 100%" + nl;
                        //MessageBox.Show("Weights ok = 100%");
                    }

                    //}
                }
            } // for se
            weightForm.WReport = weightForm.WReport + "_____________________________" + nl;
            if (SessionType != 0)
            {

                TotW = 0;
                for (int se = 0; se < SessionS; se++)
                {
                    TotW = TotW + SessionWeight[se];
                    TempSessionNum = TempSessionNum + 1;
                }
                if ((TotW > 100) || (TotW < 100))
                {
                    weightForm.WReport = weightForm.WReport + "Total Session weights are " + TotW.ToString() + "% ,but should be 100%" + nl;
                    ok = false;
                }
                else
                {
                    weightForm.WReport = weightForm.WReport + "Total Session weights are ok at 100%" + nl;
                }
                if (TempSessionNum == 0)
                {
                    weightForm.WReport = weightForm.WReport + "Note: you have selected multiple sessions, but you don't have any sessions - either add sessions or check Sessions from the Settings menu" + nl;
                }
                else if (TempSessionNum == 1)
                {
                    weightForm.WReport = weightForm.WReport + "Note: you have selected multiple sessions, but you only have one session - you may need to add more sessions or check Sessions from the Settings menu" + nl;
                }
                else
                {
                    weightForm.WReport = weightForm.WReport + "You have selected multiple sessions. There are " + TempSessionNum.ToString() + " sessions in this assessment" + nl;
                }
            }
            else
            {
                weightForm.WReport = weightForm.WReport + "You have selected no sessions (ie. a single session) in this assessment" + nl;
            }
            weightForm.WReport = weightForm.WReport + "________________________________________________________" + nl;
            if (ok)
            {
                weightForm.WReport = weightForm.WReport + "All weightings ok." + nl;
            }
            else
            {
                weightForm.WReport = weightForm.WReport + "Weightings have errors! -- fix these before you start marking" + nl;
            }

            weightForm.ShowDialog();
        }

        private void checkWbutton_Click(object sender, EventArgs e)
        {
            CheckWeightReport();
            /*if (SessionType == 0)
            {
                CheckWeights();
            }*/
        }

        private void copyWeightbutton_Click(object sender, EventArgs e)
        {
            if (Session > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Use weights from Session 1 Yes/No?", "Use weights", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    for (int s = 0; s < CritZ + 1; s++)
                    {
                        for (int i = 0; i < MaxSub + 1; i++)
                        {
                            crweight[s, i, Session] = crweight[s, i, 0];
                        }
                    }
                }
            }
        }



        private void PeerFormPopulate()
        {
            int i;
            if (peerFile == "")
            {
                MessageBox.Show("No template file specified");
                return;
            }
            PeerForm PForm = new PeerForm();
            PForm.TemplateFile = peerFile;
            PForm.PeerReview = 0;
            PForm.Institution = instituteBox.Text;

            PForm.UnitTitle = unitTitleBox.Text;
            PForm.UnitCode = unitCodeBox.Text;
            PForm.Level = levelBox.Text;
            PForm.UnitLeader = unitLeaderBox.Text;
            PForm.Peer = peerBox.Text;
            PForm.AssessNo = assessCBox.Text;
            PForm.AssessTitle = assessTBox.Text;
            PForm.ULSigFilePath = ULSigFilePath;
            PForm.PeerSigFilePath = PeerSigFilePath;

            try
            {
                PForm.Weight = Convert.ToInt32(assessWeightBox.Text);
            }
            catch
            {
            }
            try
            {
                PForm.PassMark = Convert.ToInt32(passMarkBox.Text);
            }
            catch
            {
            }

            PForm.Aggregation = aggregatedlist.Items[aggregatedlist.SelectedIndex].ToString();

            PForm.Sheet = checklist1.Checked;
            PForm.Comment1 = C1.Text;
            PForm.Strategy = checklist2.Checked;
            PForm.Comment2 = C2.Text;
            PForm.Instructions = checklist3.Checked;
            PForm.Comment3 = C3.Text;
            PForm.Criteria = checklist4.Checked;
            PForm.Comment4 = C4.Text;
            PForm.Task = checklist5.Checked;
            PForm.Comment5 = C5.Text;
            PForm.Special = checklist6.Checked;
            PForm.Comment6 = C6.Text;
            if (ULCheck.Checked)
            {
                PForm.ULDate = dateTimePicker1.Value.ToString("dd/MM/yy");
            }
            if (PeerCheck.Checked)
            {
                PForm.PeerDate = dateTimePicker2.Value.ToString("dd/MM/yy");
            }
            PForm.Comment7 = C7.Text;
            PForm.OutFilePath = UnitFilePath;
            PForm.ShowDialog();
        }

        private void ShowPeerButton_Click(object sender, EventArgs e)
        {
            PeerFormPopulate();
        }



        private void ModFormPopulate()
        {
            int i;
            if (modFile == "")
            {
                MessageBox.Show("No template file specified");
                return;
            }
            PeerForm PForm = new PeerForm();
            //PForm.MarkDir = modDirectory;
            PForm.MarkDir = modDirectory;

            if (markradio1.Checked)
            {
                PForm.MarkType = 0;

            }
            else if (markradio2.Checked)
            {
                PForm.MarkType = 1;
            }
            else
            {
                PForm.MarkType = 2;
            }
            PForm.TemplateFile = modFile;
            PForm.PeerReview = 1;
            PForm.Institution = instituteBox.Text;

            PForm.UnitTitle = UTitleBox.Text;
            PForm.UnitCode = UCodeBox.Text;
            PForm.Level = ULevelBox.Text;
            PForm.UnitLeader = ULBox.Text;
            //PForm.Peer = moderatorBox.Text;
            PForm.Moderator = moderatorBox.Text;
            PForm.AssessNo = ACodeBox.Text;
            PForm.AssessTitle = assessTBox.Text;

            try
            {
                PForm.Weight = Convert.ToInt32(AWeightBox.Text);
            }
            catch
            {
            }
            try
            {
                PForm.PassMark = Convert.ToInt32(APassBox.Text);
            }
            catch
            {
            }
            PForm.Comment1 = actionBox.Text;
            PForm.Comment2 = ULresponseBox.Text;
            PForm.Comment3 = thirdBox.Text;
            PForm.Agreed = agreedcheck.Checked;
            PForm.Third = thirdcheck.Checked;
            PForm.Aggregation = aggregatedlist.Items[aggregatedlist.SelectedIndex].ToString();

            PForm.OutFilePath = UnitFilePath;
            PForm.ShowDialog();
        }

        private void ModerationButton_Click(object sender, EventArgs e)
        {
            ModFormPopulate();
        }

        private void show2ndMarker(bool t)
        {
            label1st.Visible = t;
            label2nd.Visible = t;
            textBox1st.Visible = t;
            textBox2nd.Visible = t;
            label2ndComments.Visible = t;
            textBox2Comments.Visible = t;
            Clicklabel2.Visible = t;
        }
        private void markModeButton_Click(object sender, EventArgs e)
        {
            if (markModeButton.Text == "1st Marker")
            {
                markModeButton.Text = "2nd Mark";
                MarkMode = 1;
                modSelect.Visible = false;
                show2ndMarker(false);
            }
            else if (markModeButton.Text == "2nd Mark")
            {
                markModeButton.Text = "3rd Mark";
                MarkMode = 2;
                modSelect.Visible = true;
                show2ndMarker(false);
            }
            else if (markModeButton.Text == "3rd Mark")
            {
                //markModeButton.Text = "1st Marker";
                markModeButton.Text = "Agreed mark";
                MarkMode = 0;
                modSelect.Visible = false;
                show2ndMarker(true);
            }
            else if (markModeButton.Text == "Agreed mark")
            {
                markModeButton.Text = "1st Marker";
                MarkMode = 3;
                modSelect.Visible = false;
                show2ndMarker(false);
            }

        }

        private void modSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (MarkMode != 0)
            {
                modSelect.Checked = false;
            }
        }

        private void UnitsaveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            //save unit information           
            SaveUnitFile(UnitsaveFileDialog.FileName);
            Copy_Form_Data();
            unitButtons(false);
        }

        private void unitSaveButton_Click(object sender, EventArgs e)
        {   //save units
            if (UnitTitletextBox.Text != null && UnitTitletextBox.Text.Length > 0)
            {
                if (Directory.Exists(UnitFilePath))
                {
                    UnitsaveFileDialog.InitialDirectory = UnitFilePath;
                }
                else
                {
                    MessageBox.Show("Select a directory for new unit");
                    folderBrowserDialog1.SelectedPath = DefaultDir;
                    folderBrowserDialog1.ShowDialog();
                    UnitFilePath = folderBrowserDialog1.SelectedPath;
                }
                UnitsaveFileDialog.InitialDirectory = UnitFilePath;
                UnitsaveFileDialog.FileName = UnitTitletextBox.Text.Trim() + ".uni";
                UnitsaveFileDialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("Unit title is empty");
            }


        }
        private void loadUnitButton_Click(object sender, EventArgs e)
        {
            openUnitFileDialog.FileName = "";
            openUnitFileDialog.InitialDirectory = DefaultDir;
            DialogResult dialogResult = MessageBox.Show("Load unit - this will clear all form data - Yes/No?", "Load Unit", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                DialogResult dialogResult2 = MessageBox.Show("Are you sure - Yes/No?", "Load Unit", MessageBoxButtons.YesNo);
                if (dialogResult2 == DialogResult.Yes)
                {
                    openUnitFileDialog.ShowDialog();
                }
            }
        }


        private void unitEditbutton_Click(object sender, EventArgs e)
        {
            unitButtons(true);
        }

        private void unitButtons(bool b)
        {
            unitEditbutton.Visible = !b;
            unitSaveButton.Visible = b;
            loadUnitButton.Visible = !b;
            unitCancelButton.Visible = b;
            newUnitbutton.Visible = !b;

            ULSigButton.Enabled = b;
            unitFolderbutton.Enabled = b;
            marksFolderbutton.Enabled = b;
            peerTemplateButton.Enabled = b;
            moderationTemplateButton.Enabled = b;
            extTemplatebutton.Enabled = b;

            institutionTextBox.ReadOnly = !b;
            UnitTitletextBox.ReadOnly = !b;
            unitCodeTextBox.ReadOnly = !b;
            levelTextBox.ReadOnly = !b;
            ULTextBox.ReadOnly = !b;
            tutorsTextBox.ReadOnly = !b;
            passMarktextBox.ReadOnly = !b;
            StudentFiletextBox.ReadOnly = !b;
            peerfileTextBox.ReadOnly = !b;
            modfileTextBox.ReadOnly = !b;
            extfileTextBox.ReadOnly = !b;
            unitFoldertextBox.ReadOnly = !b;
            marksFoldertextBox.ReadOnly = !b;

        }



        private void unitCancelButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Cancel Edit Yes/No?", "Cancel Edit", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                unitButtons(false);
            }
        }

        private void SaveUnitFile(string filename)
        {
            // write unit to file:
            try
            {
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    sw.WriteLine("Unit Title: " + UnitTitletextBox.Text);
                    sw.WriteLine("Unit Code: " + unitCodeTextBox.Text);
                    sw.WriteLine("Level: " + levelTextBox.Text);
                    sw.WriteLine("Unit Leader: " + ULTextBox.Text);
                    sw.WriteLine("Unit Tutors: " + tutorsTextBox.Text);
                    sw.WriteLine("Pass mark: " + passMarktextBox.Text);
                    sw.WriteLine("Student marks folder: " + marksDirectory);
                    sw.WriteLine("Student names file: " + StudentImportFile);
                    sw.WriteLine("Unit peer template file: " + peerfileTextBox.Text);
                    sw.WriteLine("Unit moderation template file: " + modfileTextBox.Text);
                    sw.WriteLine("Institution: " + institutionTextBox.Text);
                    sw.WriteLine("Template genfile: " + templatetextBox.Text);
                    Institution = institutionTextBox.Text;
                    sw.Close();
                    UnitFile = filename;
                    UnitFilePath = Path.GetDirectoryName(filename);
                }
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }
        private void LoadUnitFile(string filename)
        {
            string[] parts = new string[3];
            string str = "";
            int i = 0;
            int k = 0;
            try
            {
                UnitFile = filename;
                UnitFilePath = Path.GetDirectoryName(filename);
                // Create an instance of StreamWriter to read grades from file:
                using (StreamReader sw = new StreamReader(filename))
                {
                    while (!sw.EndOfStream)
                    {
                        str = sw.ReadLine();
                        if (str.StartsWith("Unit Title: "))
                        {
                            i = str.IndexOf("Unit Title: ");
                            k = "Unit Title: ".Length;
                            str = str.Substring(i + k, str.Length - k);
                            UnitTitletextBox.Text = str;
                        }
                        else if (str.StartsWith("Unit Code: "))
                        {
                            i = str.IndexOf("Unit Code: ");
                            k = "Unit Code: ".Length;
                            str = str.Substring(i + k, str.Length - k);
                            unitCodeTextBox.Text = str;
                        }
                        else if (str.StartsWith("Level: "))
                        {
                            i = str.IndexOf("Level: ");
                            k = "Level: ".Length;
                            str = str.Substring(i + k, str.Length - k);
                            levelTextBox.Text = str;
                        }
                        else if (str.StartsWith("Unit Leader: "))
                        {
                            i = str.IndexOf("Unit Leader: ");
                            k = "Unit Leader: ".Length;
                            str = str.Substring(i + k, str.Length - k);
                            ULTextBox.Text = str;
                        }
                        else if (str.StartsWith("Unit Tutors: "))
                        {
                            i = str.IndexOf("Unit Tutors: ");
                            k = "Unit Tutors: ".Length;
                            str = str.Substring(i + k, str.Length - k);
                            tutorsTextBox.Text = str;
                        }
                        else if (str.StartsWith("Pass mark: "))
                        {
                            i = str.IndexOf("Pass mark: ");
                            k = "Pass mark: ".Length;
                            str = str.Substring(i + k, str.Length - k);
                            passMarktextBox.Text = str;
                        }
                        else if (str.StartsWith("Student names file: "))
                        {
                            i = str.IndexOf("Student names file: ");
                            k = "Student names file: ".Length;
                            str = str.Substring(i + k, str.Length - k);
                            StudentImportFile = str;
                            StudentImportTextBox.Text = str;
                            StudentFiletextBox.Text = str;
                        }
                        else if (str.StartsWith("Student marks folder: "))
                        {
                            i = str.IndexOf("Student marks folder: ");
                            k = "Student marks folder: ".Length;
                            str = str.Substring(i + k, str.Length - k);
                            marksFoldertextBox.Text = str;
                            marksDirectory = str;
                        }
                        else if (str.StartsWith("Unit peer template file: "))
                        {
                            i = str.IndexOf("Unit peer template file: ");
                            k = "Unit peer template file: ".Length;
                            str = str.Substring(i + k, str.Length - k);
                            peerfileTextBox.Text = str;
                            peerFile = str;
                        }
                        else if (str.StartsWith("Unit moderation template file: "))
                        {
                            i = str.IndexOf("Unit moderation template file: ");
                            k = "Unit moderation template file: ".Length;
                            str = str.Substring(i + k, str.Length - k);
                            modfileTextBox.Text = str;
                            modFile = str;
                        }
                        else if (str.StartsWith("Institution: "))
                        {
                            i = str.IndexOf("Institution: ");
                            k = "Institution: ".Length;
                            str = str.Substring(i + k, str.Length - k);
                            institutionTextBox.Text = str;
                            Institution = str;
                        }
                        else if (str.StartsWith("Template genfile: "))
                        {
                            i = str.IndexOf("Template genfile: ");
                            k = "Template genfile: ".Length;
                            str = str.Substring(i + k, str.Length - k);
                            templatetextBox.Text = str;
                            templateGenfile = str;
                        }
                    }
                    sw.Close();
                }
                Copy_Form_Data();

            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void openUnitFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            LoadUnitFile(openUnitFileDialog.FileName);
            unitButtons(false);
            unitEditbutton.Visible = true;
            //UnitFilePath = openUnitFileDialog.FileName;
            unitFoldertextBox.Text = UnitFilePath;
        }


        private void rtfsaveFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void rtfopenFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (peerfiletype == 0)
            {
                peerfileTextBox.Text = rtfopenFileDialog.FileName;
                peerFile = peerfileTextBox.Text;
            }
            else if (peerfiletype == 1)
            {
                modfileTextBox.Text = rtfopenFileDialog.FileName;
                modFile = modfileTextBox.Text;
            }
            else
            {
                extfileTextBox.Text = rtfopenFileDialog.FileName;
                extFile = extfileTextBox.Text;
            }
        }

        private void peerTemplateButton_Click(object sender, EventArgs e)
        {
            peerfiletype = 0;
            rtfopenFileDialog.InitialDirectory = DefaultDir;
            rtfopenFileDialog.ShowDialog();
        }

        private void moderationTemplateButton_Click(object sender, EventArgs e)
        {
            peerfiletype = 1;
            rtfopenFileDialog.InitialDirectory = DefaultDir;
            rtfopenFileDialog.ShowDialog();
        }

        private void modfileButton_Click(object sender, EventArgs e)
        {
            if (modDirectory.Length < 1)
            {
                if (marksDirectory.Length < 1)
                {
                    modDirectory = UnitFilePath;
                }
                modDirectory = marksDirectory;
            }
            folderBrowserDialog1.SelectedPath = modDirectory;//default mod dir
            folderBrowserDialog1.ShowDialog();
            modDirTextBox.Text = folderBrowserDialog1.SelectedPath;
            modDirectory = modDirTextBox.Text;
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (!singleGrades)
            {
                SelectGradeAlias();
            }
        }


        private void SelectGradeAlias()
        {
            FontStyle fs = FontStyle.Italic | FontStyle.Bold;
            int grsel = treeView1.SelectedNode.Index;
            try
                {
            if (grsel > -1)
            {               
                    if (gradelist[grsel].grselected)
                    {     //delselect grade from dual marks      
                        treeView1.SelectedNode.ForeColor = Color.Black;
                        treeView1.SelectedNode.NodeFont = new Font(TVFont, FontStyle.Regular);

                        AliastextBox.Visible = false;
                        grAliaslabel.Visible = false;
                        gradelist[treeView1.SelectedNode.Index].grselected = false;
                    }
                    else
                    {
                        //select this grade
                        treeView1.SelectedNode.ForeColor = Color.Black;
                        treeView1.SelectedNode.NodeFont = new Font(TVFont, FontStyle.Italic | FontStyle.Bold);

                        AliastextBox.Visible = true;
                        grAliaslabel.Visible = true;
                        gradelist[treeView1.SelectedNode.Index].grselected = true;

                    }
                }                
                RebuildAliasList();
            }
            catch { }
        }

        private void RebuildAliasList()
        {
            aliases a;
            aliaslist.Clear();
            listBox1.Items.Clear();
            for (int i = 0; i < MaxGrades; i++)
            {
                if (gradelist[i].grselected)
                {
                    a.alias = gradelist[i].gralias;
                    a.grindex = i;
                    a.percent = gradelist[i].grpercent;
                    aliaslist.Add(a);
                    listBox1.Items.Add(a.alias);
                }
            }

        }

        private void Copy_Grades_To_ListBox()
        {
            listBox1.Items.Clear();
            try
            {
                foreach (TreeNode n in treeView1.Nodes[0].Nodes)
                {
                    listBox1.Items.Add(n.Text);                   
                }
            }
            catch
            {
            }
        }

        /*private void Copy_Alias_To_ListBox()
        {
            listBox1.Items.Clear();
            try
            {
                for (int i = 0; i < MaxGrades; i++)
                {
                    if (gradelist[i].gralias != null)
                    {
                        listBox1.Items.Add(gradelist[i].gralias);
                    }
                }
            }
            catch
            {
            }
        }*/

        private void graderadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading && change1 && graderadioButton1.Checked)
            {
                singleGrades = true;
                AliastextBox.Visible = false;
                grAliaslabel.Visible = false;
                Clear_Selected_Grades();
                change1 = false;
                Copy_Grades_To_ListBox();
            }
        }
        private void Clear_Selected_Grades()
        {
            for (int i = 0; i < gradelist.Count(); i++)
            {
                gradelist[i].grselected = false;
                if (i < treeView1.Nodes[0].Nodes.Count)
                {
                    treeView1.Nodes[0].Nodes[i].NodeFont = new Font(TVFont, FontStyle.Regular);
                }
            }
        }

        private void graderadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading && change1 && graderadioButton2.Checked)
            {               
                    listBox1.Items.Clear();
                    singleGrades = false;
                    AliastextBox.Visible = true;
                    grAliaslabel.Visible = true;
                    change1 = false;
                    //Copy_Alias_To_ListBox();
            }

        }

        private void graderadioButton1_MouseHover(object sender, EventArgs e)
        {
            if (!graderadioButton1.Checked && !change1)
            {
                MessageBox.Show("Note: Checking this will clear selections - do you need to save grades first?");
                change1 = true;
            }
        }

        private void graderadioButton2_MouseHover(object sender, EventArgs e)
        {
            if (!graderadioButton2.Checked && !change1)
            {
                MessageBox.Show("Note: Checking this will clear selections - do you need to save grades first?");
                change1 = true;
            }
        }
        //-----------------------------Comments: ----------------------------------------






        private void ExternalFormPopulate()
        {
            int i;
            if (extFile == "")
            {
                MessageBox.Show("No template file specified");
                return;
            }
            PeerForm PForm = new PeerForm();

            PForm.TemplateFile = extFile;
            PForm.PeerReview = 2;
            PForm.Institution = instituteBox.Text;

            PForm.UnitTitle = utBox.Text;
            PForm.UnitCode = ucBox.Text;
            PForm.ExternalMod = extBox.Text;
            if (OKcheckBox.Checked)
            {
                PForm.okstring = "satisfied";
            }
            else
            {
                PForm.okstring = "not satisfied";
            }
            PForm.Comment1 = extComBox.Text;
            PForm.ULDate = dateTimePicker3.Value.ToString("dd/MM/yy");
            PForm.OutFilePath = UnitFilePath;
            PForm.ShowDialog();

        }

        private void externalButton_Click(object sender, EventArgs e)
        {
            ExternalFormPopulate();
        }

        private void extTemplatebutton_Click(object sender, EventArgs e)
        {
            peerfiletype = 2;
            rtfopenFileDialog.InitialDirectory = DefaultDir;
            rtfopenFileDialog.ShowDialog();
        }

        private void commentsToolStripMenuItem_Click(object sender, EventArgs e) //comments menu
        {
            //CommentForm CommentsForm = new CommentForm();
            CommentsForm.selectComment = false;
            CommentsForm.CFile = CommentFile;
            CommentsForm.CPath = CommentFilePath;
            CommentsForm.ShowDialog();
            try
            {
                CommentFile = CommentsForm.Passvalue;
                CommentFilePath = Path.GetDirectoryName(CommentFile);
            }
            catch
            {
                //commentfile may be null or empty
            }
        }



        private void textBox10_DoubleClick(object sender, EventArgs e)
        {
            textBox10 = EditSpecific(textBox10); //edit comments text box if double click on it

            /*string CommentStr;
            try
            {
                if (File.Exists(CommentFile))
                {
                    CommentsForm.selectComment = true;
                    CommentsForm.CFile = CommentFile;
                    CommentsForm.ShowDialog();
                    CommentStr = CommentsForm.Passvalue;
                    if (CommentStr != null || CommentStr != "")
                    {
                        textBox10.Text = textBox10.Text.Insert(textBox10.SelectionStart + textBox10.SelectionLength, CommentStr);
                    }
                    if (CommentsForm.CFile != CommentFile && CommentsForm.CFile != null)
                    {
                        CommentFile = CommentsForm.CFile;
                    }
                }
                else
                {
                    MessageBox.Show("No comments file");
                }
            }
            catch { }
            */

        }

        private void unitFolderbutton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = UnitFilePath;//default unit dir
            folderBrowserDialog1.ShowDialog();
            unitFoldertextBox.Text = folderBrowserDialog1.SelectedPath;

            UnitFilePath = unitFoldertextBox.Text;

        }

        private void marksFolderbutton_Click(object sender, EventArgs e)
        {
            if (marksDirectory.Length < 1)
            {
                marksDirectory = UnitFilePath;
            }
            folderBrowserDialog1.SelectedPath = marksDirectory;//default unit dir
            folderBrowserDialog1.ShowDialog();
            marksFoldertextBox.Text = folderBrowserDialog1.SelectedPath;

            marksDirectory = marksFoldertextBox.Text;
        }

        private void newUnitbutton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Start new unit - this will clear all form data - Yes/No?", "New Unit", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                DialogResult dialogResult2 = MessageBox.Show("Are you sure - Yes/No?", "New Unit", MessageBoxButtons.YesNo);
                if (dialogResult2 == DialogResult.Yes)
                {

                    UnitTitletextBox.Text = "";
                    unitCodeTextBox.Text = "";
                    levelTextBox.Text = "";
                    ULTextBox.Text = "";
                    unitFoldertextBox.Text = "";
                    marksFoldertextBox.Text = "";
                    StudentFiletextBox.Text = "";
                    StudentImportTextBox.Text = "";
                    marksDirectory = "";
                    SessionPath = "";
                    CriteriaPath = "";
                    UnitFilePath = DefaultDir;
                    unitButtons(true);
                    Copy_Form_Data();
                }
            }
        }

        private void Copy_Form_Data()
        {   //copy data from unit for to peer and mod forms etc.
            utBox.Text = UnitTitletextBox.Text;
            UTitleBox.Text = UnitTitletextBox.Text;
            unitTitleBox.Text = UnitTitletextBox.Text;

            ULBox.Text = ULTextBox.Text;
            unitLeaderBox.Text = ULTextBox.Text;


            unitCodeBox.Text = unitCodeTextBox.Text;
            UCodeBox.Text = unitCodeTextBox.Text;
            ucBox.Text = unitCodeTextBox.Text;

            ULevelBox.Text = levelTextBox.Text;
            levelBox.Text = levelTextBox.Text;

            APassBox.Text = passMarktextBox.Text;
            passMarkBox.Text = passMarktextBox.Text;

        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void institutionTextBox_TextChanged(object sender, EventArgs e)
        {
            Institution = institutionTextBox.Text;
            instituteBox.Text = Institution;
            InstBox.Text = Institution;
            inBox.Text = Institution;
        }

        private void importGradesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog5.FileName = "";
            openFileDialog5.DefaultExt = ".txt";

            openFileDialog5.ShowDialog();

        }

        private void Import_Grades_From_CSV_File(string filename)
        {
            string str;
            string[] str2;
            int i = 0;
            Remove_Grades();
            using (StreamReader sw = new StreamReader(filename))
            {
                while (!sw.EndOfStream)
                {
                    try
                    {
                        str = sw.ReadLine();
                        str.Trim();
                        str2 = str.Split(',');
                        if (str.Trim().Length > 0)
                        {
                            if (str2[0] != null)
                            {
                                string grade = str2[0];
                                gradelist[i].grtitle = grade;
                                treeView1.Nodes[0].Nodes.Add(grade);
                            }
                            if (str2[1] != null)
                            {
                                string percent = str2[1];
                                gradelist[i].grpercent = Convert.ToSingle(percent);
                            }
                            if (str2[2] != null)
                            {
                                string lower = str2[2];
                                gradelist[i].grlower = Convert.ToSingle(lower);
                            }
                            if (str2[3] != null)
                            {
                                string upper = str2[3];
                                gradelist[i].grupper = Convert.ToSingle(upper);
                            }
                            if (str2[4] != null)
                            {
                                string feedback = str2[4];
                                gradelist[i].grfb = feedback;
                            }
                        }

                    }
                    catch
                    {
                    }
                    treeView1.Nodes[0].ExpandAll();
                    i++;
                }
                sw.Close();
            }
        }

        private void openFileDialog5_FileOk(object sender, CancelEventArgs e)
        {
            Import_Grades_From_CSV_File(openFileDialog5.FileName);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            
        }

        private void ImportFileSelectbutton_Click(object sender, EventArgs e)
        {
            openFileDialog6.DefaultExt = "csv";
            openFileDialog6.Filter = "Comma separated textfile (*.csv) |*.csv";
            openFileDialog6.FileName = "";
            openFileDialog6.ShowDialog();
        }

        private void openFileDialog6_FileOk(object sender, CancelEventArgs e)
        {
            StudentImportTextBox.Text = openFileDialog6.FileName;
            StudentImportFile = StudentImportTextBox.Text;
            StudentFiletextBox.Text = StudentImportFile;
        }

        private void Importbutton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Load new student list - (this will overwrite current list) Y/N?", "Import Students", MessageBoxButtons.YesNo);
             if (dialogResult == DialogResult.Yes)
             {
                 Import_Students();
             }
        }

        private void Import_Students()
        {
            studentlist.Clear();
            listBox4.Items.Clear();
            StudentcomboBox.Items.Clear();
            Student st;
            st.studentname = "";
            st.studentmail = "";
            string str = "";
            string[] str2;
            int i = 0;
            string filename = StudentImportTextBox.Text;
            if (!File.Exists(filename))
            {
                MessageBox.Show("File does not exist");
                return;
            }
            using (StreamReader sw = new StreamReader(filename))
            {
                while (!sw.EndOfStream)
                {
                    try
                    {
                        st.studentname = "";
                        st.studentmail = "";
                        str = sw.ReadLine();
                        str.Trim();
                        str2 = str.Split(',');
                        if (str.Trim().Length > 0)
                        {
                            if (str2[0] != null)
                            {
                                listBox4.Items.Add(str2[0]);
                                StudentcomboBox.Items.Add(str2[0]);
                                st.studentname = str2[0];
                            }
                            if (str2[1] != null)
                            {
                                st.studentmail = str2[1];
                            }
                            if (st.studentname.Length > 0)
                            {
                                studentlist.Add(st);
                                listBox4.SelectedIndex = 0;
                                StuEmailtextBox.Text = studentlist[0].studentmail;
                            }
                        }
                    }
                    catch
                    {
                    }

                    i++;
                }
                sw.Close();
                if (i > 1)
                {
                    StudentcomboBox.MaxDropDownItems = 8;
                }
            }
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            StuEmailtextBox.Text = studentlist[listBox4.SelectedIndex].studentmail;
        }

       

        private void StudentcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           Student_Changed();
        }



        private void Student_Changed()
        {
            if (!savedStudent)
            {
                MessageBox.Show("Warning: student name changed before saving");
            }
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void institutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = "";
            InputForm input = new InputForm();
            input.browser = false;
            input.text = Institution;
            input.Passvalue = "Enter Institution name:";
            input.ShowDialog();
            Institution = input.Passvalue;
            institutionTextBox.Text = Institution;
        }

        private void defaultDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setDefaultDir("Enter default directory: ");  //set the default directory for ultramarker work, eg. C:\Ultramarker   
            defaultdirlabel.Text = "Default directory currently set to: " + DefaultDir;
        }
        private void setDefaultDir(string message) 
        {
            string str = "";
            string sl = "";
            bool outl = false;
            DialogResult reply;
            //setup the default directory - C:\Ultramarker is recommended default
            InputForm input = new InputForm();
            input.editable = true;//allows manual input of directory
            input.browser = true; //allows selection of directory from browser
            input.text = DefaultDir;
            input.Passvalue = message;
            while (outl == false)
            {
                input.ShowDialog();
                if (input.cancel)
                {
                    return;
                }
                str = input.Passvalue;
                DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(str));
                str = Convert.ToString(dir);
                reply = MessageBox.Show("Full path: " + str + " - Is this correct - yes/no?", "Path Correct ?", MessageBoxButtons.YesNo);
                if (reply == DialogResult.No)
                {
                    //ask again
                }
                else
                {                    
                    if (Directory.Exists(str) == false)
                    {
                        reply = MessageBox.Show("Directory does not exist, create yes/no?", "Directory", MessageBoxButtons.YesNo);
                        if (reply == DialogResult.Yes)
                        {
                            try
                            {
                                Directory.CreateDirectory(str);
                                outl = true;
                            }
                            catch
                            {
                                MessageBox.Show("Invalid path");
                            }
                        }
                    }
                    else
                    {
                        outl = true;
                    }
                }
            }            
            if (str.Trim() == DefaultDir.Trim())
            {
                reply = MessageBox.Show("This is already the Default directory for Ultamarker - reset all paths yes/no?", "Reset default Directory", MessageBoxButtons.YesNoCancel);
            }
            else
            {
                reply = MessageBox.Show("Change the Default directory and rest all paths yes/no?", "Change Default Directory", MessageBoxButtons.YesNoCancel);
            }
            if (reply == DialogResult.Yes)
            {
                if (str.LastIndexOf("\\") == str.Length -1 ) //is last charcater a slash?
                { sl = ""; }
                else { sl = slash; }
                DefaultDir = str + sl;
                GradeFile = "";
                GradePath = str + sl;
                UnitFile = "";
                UnitFilePath = str + sl;
                LOFilePath = str + sl;
                LOFile = "";
                CriteriaPath = str + sl;
                CriteriaFile = "";
                CommentFile = "";
                CommentFilePath = str + sl;
                MessageBox.Show("Resetting default path for Ultramarker - copy all files into there!");
            }
        }

        private void loadConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog7.ShowDialog(); //not used at present - loads configs from config files again
        }

        private void openFileDialog7_FileOk(object sender, CancelEventArgs e)
        {
            DefaultDir = Path.GetDirectoryName(openFileDialog7.FileName) + slash; //reload configs again - not used at present
        }

        private void overrideButton_Click(object sender, EventArgs e)
        {
            //override grade with %
            overridegrade();
            
        }
        private void overridegrade()
        {   //override grade with percent
            int sub = 0;
            Single pc = -1;
            try
            {
                pc = Convert.ToSingle(overrideBox.Text);
            }
            catch
            {
            }
            if (startMark)
            {
                try
                {
                    if (pc >= 0 && pc <= 100)                                    
                    {                      
                            if (EditStudent)
                            {
                                button7.Visible = true;
                            }
                            if (listBox1.SelectedIndex > -1)
                            {

                                if (CriteriaSelected)
                                {
                                    sub = MaxSub;
                                }
                                else
                                { sub = SSub; }
                                Marks[SCriteria, sub, Session] = overrideBox.Text + "%";
                                if (EditStudent)
                                {
                                    if (crSelected[SCriteria, sub, Session])
                                    {
                                        label18.Text = overrideBox.Text + "%";
                                    }
                                    else
                                    {
                                        label18.Text = "n/a";
                                    }

                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid %");
                        }                   

                }
                catch
                {
                }

            }
        }

        private void overrideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult reply;
            if (allowoverride)
            {
                reply = MessageBox.Show("DISABLE override of grade by %, yes/no?", "Disable grade override",MessageBoxButtons.YesNoCancel);
                if (reply == DialogResult.Yes)
                {
                    allowoverride = false;
                }
            }
            else
            {
                reply = MessageBox.Show("ENABLE override of grade by %, yes/no?", "Enable grade override", MessageBoxButtons.YesNoCancel);
                if (reply == DialogResult.Yes)
                {
                    allowoverride = true;
                }
            }        
        }

      
        private void overrideBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
            {
                overridegrade();
                return;
            }
            if (e.KeyValue < 48 || e.KeyValue > 57) //between 0 and 9
            {
                if (e.KeyData == Keys.Back || e.KeyData == Keys.Delete || e.KeyData == Keys.NumLock)
                {
                    e.SuppressKeyPress = false;
                }
                else
                {
                    e.SuppressKeyPress = true;
                }
            }           
        }

        private void summaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string stype;
            summaryForm sForm = new summaryForm();
            sForm.marksDirectory = DefaultDir;
            if (marksDirectory.Trim() == "")
            {
                sForm.marksDirectory = UnitFilePath;

            }
            else
            {
                sForm.marksDirectory = marksDirectory;
            }
            sForm.assessmentFilePath = CriteriaFile;
            sForm.assessmentTitle = assess.Title;
            sForm.assessmentCode = assess.Code;
            if (UnitFilePath.Trim() == "")
            {
                sForm.UnitPath = DefaultDir;
            }
            else
            {
                sForm.UnitPath = UnitFilePath;
            }
            sForm.Passvalue[1] = Convert.ToString(Summary_Sort_Type);
            sForm.Passvalue[2] = Summary_percentgrade;

            sForm.ShowDialog();
            stype = sForm.Passvalue[1];
            Summary_percentgrade = sForm.Passvalue[2];
            Summary_Sort_Type = Convert.ToInt32(stype);
        }

        private void sittingButton_Click(object sender, EventArgs e)
        {
            if (sittingButton.Text == "Main")
            {
                sittingButton.Text = "Ref/Def 1";
                Sitting = "Referral/deferral 1";                
            }
            else if (sittingButton.Text == "Ref/Def 1")
            {
                sittingButton.Text = "Ref/Def 2";
                Sitting = "Referral/Deferral 2";             
            }
            else if (sittingButton.Text == "Ref/Def 2")
            {
                sittingButton.Text = "Ref/Def 3";
                Sitting = "Referral/Deferral 3";
            }
            else if (sittingButton.Text == "Ref/Def 3")
            {
                sittingButton.Text = "Main";
                Sitting = "Main";
            }
        }

        private void DeselectSessioncheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (DeselectSessioncheckBox.Checked)
            {
                DeselectSession[Session] = true;
            }
            else
            {
                DeselectSession[Session] = false;
            }
        }

        private void criteriaSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int t = 0;
            CriteriaSelectionForm CSForm = new CriteriaSelectionForm();
            //set grade type to normal by default:
            CSForm.Passvalue = CriteriaSelectionType;

            CSForm.ShowDialog();

            t = CSForm.Passvalue;
            CriteriaSelectionType = t;
            
        }

      

        private void ULSigButton_Click_1(object sender, EventArgs e)
        {
            openFileDialog8.Filter = "jpg file (*.jpg)|*.jpg|png file (*.png)|*.png";
            openFileDialog8.DefaultExt = "jpg";
            openFileDialog8.FileName = ULSigFilePath;//default unit dir
            openFileDialog8.ShowDialog();
            ULSigBox.Text = openFileDialog8.FileName;

            ULSigFilePath = ULSigBox.Text;
        }

        private void PeerSigButton_Click_1(object sender, EventArgs e)
        {
            openFileDialog8.Filter = "jpg file (*.jpg)|*.jpg|png file (*.png)|*.png";
            openFileDialog8.DefaultExt = "jpg";
            openFileDialog8.FileName = PeerSigFilePath;//default unit dir
            openFileDialog8.ShowDialog();
            PeerSigBox.Text = openFileDialog8.FileName;

            PeerSigFilePath = PeerSigBox.Text;
        }

        private void addAssessmentCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Add assessment code to end of filename when saving marks", "Add assessment code", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                addCode = true;
            }
            else
            {
                addCode = false;
            }
        }

        private void overridecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (overridecheckBox.Checked && EditStudent)
            {
                //treeView2.Enabled = false;
                Overriedlabel.Visible = true;
                OverrideGradelabel.Visible = true;
            }
            else
            {
                //treeView2.Enabled = true;
                Overriedlabel.Visible = false;
                OverrideGradelabel.Visible = false;
            }
        }
       
        private void generateButton_Click(object sender, EventArgs e)
        {
            //genList();                
            Generate_Grade_Group_RTF();
            if (GenerateFormPopulate())
            {
                string str = marksDirectory + "\\" + assess.Code;
                GForm.OutFilePath = str;

                GForm.addtick = false;
                GForm.highlight = false;
                GForm.bold = false;
                GForm.onlyTick = false;
                if (highlightButton.Text.Contains("tick") && !highlightButton.Text.Contains("no"))
                {
                    GForm.addtick = true;
                }
                if (highlightButton.Text.Contains("high"))
                {
                    GForm.highlight = true;
                }
                if (highlightButton.Text.Contains("bold"))
                {
                    GForm.bold = true;
                }
                if (highlightButton.Text.ToLower().Contains("append"))
                {
                    GForm.onlyTick = true;
                }
                GForm.ShowDialog();
            }
        }

        private bool GenerateFormPopulate()
        {
            int i;
            string str1 = "";
            string str2 = "";
            

            if (!File.Exists(templatetextBox.Text))
            {
                MessageBox.Show("No template file specified");
                return false;
            }
           
            //PForm.MarkDir = modDirectory;

            GForm.TemplateFile = templatetextBox.Text;        
            GForm.Institution = instituteBox.Text;

            GForm.UnitTitle = UTitleBox.Text;
            GForm.UnitCode = UCodeBox.Text;
          
            GForm.AssessNo = ACodeBox.Text;
            GForm.AssessTitle = assessTBox.Text;
            GForm.Marker = MarkertextBox.Text;
            
            int grade = 0;
            int counter = 0;
            int Gg = 0; //grade group counter
            bool firstthru = true;            

            try
            {
                if (listBox1.SelectedIndices.Count > 0)
                {
                    foreach (Object selecteditem in listBox1.SelectedItems)
                    {
                        str1 = selecteditem as String;
                        GForm.G[grade] = str1;  //each grade, eg. A1
                        grade++;
                        if (grade == MaxGradeGroups && grade == -1)
                        {
                            break;
                        }
                    }                 

                    for (int c = 0; c < CritZ + 1; c++) //print criteria detail
                    {
                        str1 = crdesc[c, MaxSub];
                        GForm.C[c] = str1;
                        counter = 0;
                        Gg = 0;  //gradegroup
                        firstthru = true;
                        foreach (Object selecteditem in listBox1.SelectedItems)
                        {
                            if (firstthru) //take criteria from first item selected (eg. A1 criteria text from A1...A4)
                            {
                                str1 = selecteditem as string;
                                counter = listBox1.FindString(str1);
                                str2 = grcr[c, MaxSub, counter];
                                GForm.CG[c, Gg] = str2;  //criteria for each grade
                                firstthru = false;
                                Gg++;
                            }
                            else
                            {
                                firstthru = true;
                            }                          
                        }
                    }
                }
                    
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }

            GForm.OutFilePath = UnitFilePath;
            return true;
            //GForm.ShowDialog();
        }
        private void PrimeListbox1()
        {
                     
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                Listboxlist[i] = 0;                            
            }           
        }
        private void SaveListbox1Selected()
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (listBox1.GetSelected(i))
                {
                    Listboxlist[i] = 1;                   
                }   
                else
                {
                    Listboxlist[i] = 0;
                }
            }
        }
        private void RecoverSelected()
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (Listboxlist[i] == 1)
                {
                    listBox1.SetSelected(i, true);
                }
                else
                {
                    listBox1.SetSelected(i, false);
                }
            }
        }

        /*private void genList()
        {
            string[] row = new string[3];
            genlistView.Columns.Add("Criteria");
            genlistView.Columns.Add("Grade 1");

            TreeNodeCollection nodes = treeView2.Nodes[0].Nodes;
            foreach (TreeNode n in nodes)
            {
                row[0] = n.Text;
                row[1] = "";
                row[2] = "";

                var listItem = new ListViewItem(row);
                genlistView.Items.Add(listItem);

            }
            //genlistView.Items.Add(lvi);     
        }*/

        private void GrouptextBox_TextChanged(object sender, EventArgs e)
        {
            gradegrouplist[GrouplistBox.SelectedIndex].GroupText = GrouptextBox.Text;
        }
        private void BlankGradeGroups()
        {
            string s = "";
            for (int i = 0; i < MaxGradeGroups; i++)
            {
                s = Convert.ToString(i + 1);
                if (GrouplistBox.FindString(s) < 0)
                {
                    GrouplistBox.Items.Add(s);
                }
                gradegrouplist[i].GroupNo = i+1;
                gradegrouplist[i].GroupText = "";
            }
         
        }
        private void SetGradeGroup()
        {
            
            GrouplistBox.SelectedIndex = 0;
            GrouptextBox.Text = gradegrouplist[0].GroupText;
            
        }

       

        private void gbutton_Click(object sender, EventArgs e)
        {
            if (gbutton.Text == "Edit Group")
            {
                gbutton.Text = "Save Group";
                GrouptextBox.Enabled = true;
                
            }
            else
            {
                gbutton.Text = "Edit Group";
                gradegrouplist[GrouplistBox.SelectedIndex].GroupText = GrouptextBox.Text;
                gradegrouplist[GrouplistBox.SelectedIndex].GroupNo = GrouplistBox.SelectedIndex + 1;
                GrouptextBox.Enabled = false;
            }
        }

       

        private void GrouplistBox_SelectedValueChanged(object sender, EventArgs e)
        {
            string s = "";
            s = " ";

           
        }

      

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {

                if (e.NewValue > e.OldValue)
                {
                    if (GrouplistBox.SelectedIndex < GrouplistBox.Items.Count -1)
                    {
                        GrouplistBox.SelectedIndex++;
                    }
                }
                else
                {
                    if (GrouplistBox.SelectedIndex > 0)
                    {
                        GrouplistBox.SelectedIndex--;
                    }
                }
                GrouptextBox.Text = gradegrouplist[GrouplistBox.SelectedIndex].GroupText;
            }
        }

     
        private void GrouplistBox_DoubleClick(object sender, EventArgs e)
        {
            GroupSel.Text = GrouplistBox.Text;
           
        }

        private void GroupscheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (GroupscheckBox.Checked)
            {
                showGroups(true);
            }
            else
            {
                showGroups(false);
            }
        }
        private void showGroups(bool b)
        {
            label98.Visible = b;
            label99.Visible = b;
            label100.Visible = b;
            GrouptextBox.Visible = b;
            vScrollBar1.Visible = b;
            GrouplistBox.Visible = b;
            GroupSel.Visible = b;
            gbutton.Visible = b;
        }

        private void templatebutton_Click(object sender, EventArgs e)
        {

            GenFileDialog.InitialDirectory = UnitFilePath;
            GenFileDialog.FileName = "";
            GenFileDialog.DefaultExt = "rtf";
            GenFileDialog.Filter = "Rich text files (*.rtf) |*.rtf";

            GenFileDialog.ShowDialog();
        }

        private void GenFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            templatetextBox.Text = GenFileDialog.FileName;
        }

        private void importFilebutton_Click(object sender, EventArgs e)
        {
            if (EditStudent && startMark)
            {
                importFromFile(importFileBox.Text);
            }
            else
            {
                MessageBox.Show("Need to start marking first");
            }
        }
        private void importFromFile(string filename)    //import reults from an external file
        { //starts with marked file: and ends with conatins percent:
            string str = "";
            string str3 = "";
            string str2 = "";
            double PCent = 0.0;
            double pc = 0.0;
            bool ok = true;
            int cr = 0;
            int Lc = 0;
            int Lf = 0;
            bool found1 = false;
            bool found2 = false;
            bool found3 = false;
            const int maximportableCriteria = 10;
            int[] criterias = new int[maximportableCriteria];
            double[] sumpc = new double[maximportableCriteria];
            int[] numpc = new int[maximportableCriteria];
            int[] linesC = new int[maximportableCriteria];
            int[] linesT = new int[maximportableCriteria];
            int totallines = 0;
            for (int a = 0; a < maximportableCriteria; a++)    //null the criteria counter array
            {
                sumpc[a] = 0.0;
                numpc[a] = 0;
                criterias[a] = 0;
                linesC[a] = 0;
                linesT[a] = 0;
            }
            if (File.Exists(filename))
            {
                try
                {
                    using (StreamReader rw = new StreamReader(filename))
                    {
                        while (!rw.EndOfStream)
                        {
                            str = rw.ReadLine();
                            if (str.StartsWith("Overall lines:"))
                            {
                                str3 = str.Substring(0, "Overall lines:".Length).Trim();
                                str2 = str.Substring(str3.Length, str.Length - str3.Length).Trim();
                                totallines = ExtractfromSlash(str2, false); //find total (X/totallines)
                            }
                        }
                        rw.Close();
                    }
                }
                catch { MessageBox.Show("Problem reading total lines"); }
                try
                {
                    using (StreamReader rw = new StreamReader(filename))
                    {
                        while (!rw.EndOfStream)
                        {

                            str = rw.ReadLine();
                            if (str.StartsWith("Criteria:")) //select criteria to import to
                            {
                                str3 = str.Substring(0, "Criteria:".Length).Trim();
                                str2 = str.Substring(str3.Length, str.Length - str3.Length).Trim();
                                cr = Convert.ToInt32(str2);
                                cr--;
                                if (cr < 0 || cr > CritZ) //if criteria read from file exceeds number of criteria in Ultramarker
                                {
                                    MessageBox.Show("Criteria value does not match criteria in Ultramarker");
                                    ok = false;
                                }
                                else
                                {
                                    ok = true;
                                    found1 = true;
                                    //treeView2.SelectedNode = treeView2.Nodes[0].Nodes[cr];
                                }

                            }
                            else if (str.Contains("Lines correct:") &&ok) //select criteria to import to
                            {
                                str3 = str.Substring(str.IndexOf("Lines correct:") + "Lines correct:".Length).Trim();
                                Lc = ExtractfromSlash(str3, true); //get lines correct
                                Lf = ExtractfromSlash(str3, false); //get lines total from this file
                                found2 = true;
                            }
                            else if (str.Contains("Percentage:") && ok) //select criteria to import to
                            {
                                try
                                {
                                    str3 = str.Substring(str.IndexOf("Percentage:") + "Percentage:".Length).Trim();
                                    pc = Convert.ToDouble(str3);
                                    //PCent = Convert.ToInt32(pc);
                                    for (int a = 0; a < maximportableCriteria; a++)
                                    {
                                        if (criterias[a] == cr)
                                        {
                                            sumpc[a] = sumpc[a] + pc; //sum of percents for this criteria
                                            linesC[a] = linesC[a] + Lc; //total lines correct this criteria
                                            linesT[a] = linesT[a] + Lf; //out of total lines in files for this criteria
                                            numpc[a]++; //number of percents this criteria
                                            break;
                                        }
                                        else
                                        {
                                            if (numpc[a] == 0)
                                            {
                                                criterias[a] = cr;
                                                sumpc[a] = pc;
                                                linesC[a] = Lc;
                                                linesT[a] = Lf;
                                                numpc[a]++;
                                                break;
                                            }
                                        }
                                        found3 = true;
                                    }
                                    //overrideBox.Text = PCent;
                                    //overridegrade(); // put percentage reult in the override box and update mark
                                }
                                catch
                                {
                                    MessageBox.Show("Percentage value error");
                                }
                            }
                            else if (str.StartsWith("Overall lines".Trim()))
                            {
                                //don't save o/a lines here
                            }
                            else if (str.StartsWith("Overall Result".Trim()))
                            {
                                //don't save result here
                            }
                            else if (str.StartsWith("Result:".Trim()))
                            {
                                //ignore this line
                            }
                            else
                            {
                                if (AllowImpComment)
                                {
                                    crComment[cr, MaxSub, 0] = crComment[cr, MaxSub, 0] + str + Environment.NewLine; //comment for criteria, no subcriteria and one session only allowed
                                }
                            }
                        }
                        rw.Close();
                    }
                    if (!found1 || !found2 || !found3)
                    {
                        MessageBox.Show("Input file not in correct format");
                        return;
                    }
                    ok = false;
                    for (int a = 0; a < maximportableCriteria; a++)
                    {
                        cr = criterias[a];
                        if (cr > -1 && cr < maximportableCriteria)
                        {
                            try
                            {
                                if (CalculateImportbyLines) //weight according to lines in files for this criteria - this is the default!
                                { //eg. if file 1 has 10 lines and file 2 has 20 and file 1 scores 5/10 and file 2 15/20 then overall score is 20/30 = 67%
                                    pc = (Convert.ToDouble(linesC[a]) / Convert.ToDouble(linesT[a])) * 100;
                                    PCent = Convert.ToInt32(pc);
                                }
                                else //weight evenly between percentages for each file, eg. if file 1 has 30% and file 2 60% then 90/2 = 45% overall
                                {
                                    pc = Convert.ToDouble(sumpc[a]) / Convert.ToDouble(numpc[a]); //calculate overall percent
                                    PCent = Convert.ToInt32(pc);
                                }
                                treeView2.SelectedNode = treeView2.Nodes[0].Nodes[cr]; //select the node for this criteria
                                overrideBox.Text = PCent.ToString(); //put new percentage in override box
                                overridegrade(); //overide the grade for this criteria
                                //crComment[cr, MaxSub, 0]
                            }
                            catch { }
                        }
                    }
                    MessageBox.Show("Import appears succesful");
                }
                catch
                {
                    MessageBox.Show("Error occurred");
                }
            }
            else
            {
                MessageBox.Show("File does not exist");
            }
        }

        private int ExtractfromSlash(string str,  bool leftof)
        {
            
            string[] str2 = new string[2];
            if (str.Contains('/'))
            {
                str2 = str.Split('/');
                if (leftof) //if left of /
                {
                    if (str2.Count() > 0 && str2[0] != null)
                    {
                        return Convert.ToInt32(str2[0]);
                    }                    
                }
                else
                {
                    //if right of /
                }
                {
                    if (str2.Count() > 1 && str2[1] != null)
                    {
                        return Convert.ToInt32(str2[1]);
                    }
                }
            }            
                return -1;  //if all else fails                       
        }
        private void ImportcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ImportcheckBox.Checked && ImportcheckBox.Visible)
            {
                importGroupBox.Visible = true;
                importCalcLabel.Visible = true;
            }
            else
            {
                importGroupBox.Visible = false;
                importCalcLabel.Visible = false;
            }
        }
     

        private void ImpFilebutton_Click(object sender, EventArgs e)
        {
            ImportFileDialog.InitialDirectory = UnitFilePath;
            ImportFileDialog.FileName = "";
            //ImportFileDialog.DefaultExt = "rtf";
            //ImportFileDialog.Filter = "Rich text files (*.rtf) |*.rtf";

            ImportFileDialog.ShowDialog();
        }

        private void ImportFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            importFileBox.Text = ImportFileDialog.FileName;
        }

       

        private void commentsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Allow comments from imported files Yes/No?", "Imported Comments", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                AllowImpComment = true;
            }
            else
            {
                AllowImpComment = false;
            }
        }

        private void calculateLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Allow calculation of marked import by lines instead of percentages Yes/No?", "Imported Lines/Percentages?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                CalculateImportbyLines = true;
                importCalcLabel.Text = "Calculate by lines";
            }
            else
            {
                CalculateImportbyLines = false;
                importCalcLabel.Text = "Calculate by %ages";
            }
        }

        private void CopyGradesbutton_Click(object sender, EventArgs e)
        {
            DialogResult ret = MessageBox.Show("Do you want to copy grades from Grades tab to Assess tab - yes/no?", "Copy grades", MessageBoxButtons.YesNo);
            if (ret == DialogResult.Yes)
            {
                Copy_GradestoAssess();
            }
        }
        private void Copy_GradestoAssess()
        {
            try
            {
                //copy grades to grades list box in assessment tab
                int A = 0;
                listBox1.Items.Clear();
                while (gradelist[A].grtitle != null)
                {
                    listBox1.Items.Add(gradelist[A].grtitle);
                    A++;
                }
                listBox1.SelectedIndex = 0;
            }
            catch {
                MessageBox.Show("Error copying grades"); 
            }
        }

        private void showGenAssessToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (!showGenAssessToolStripMenuItem.Checked)
            {
                if (button3.Text.StartsWith("Gen"))
                {
                    button3_Click(sender, e);
                }
            }
        }

        private void showGenAssessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!showGenAssessToolStripMenuItem.Checked)
            {
                showGenAssessToolStripMenuItem.Checked = true;
            }
            else
            {
                showGenAssessToolStripMenuItem.Checked = false;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            WebSend();
        }
        private bool WebSend()
        {
            label104.Text = "";
            try
            {
                // URI to post data to 
                String uriString = textBox5.Text;

                // Create a new WebClient instance.
                WebClient myWebClient = new WebClient();

               // fully qualified path of the file to be uploaded to the URI
                string fileName = textBox6.Text;
               
                // Upload the file to the URI.
                // The 'UploadFile(uriString,fileName)' method implicitly uses HTTP POST method.
                byte[] responseArray = myWebClient.UploadFile(uriString, fileName);

                // Decode and display the response.
                label104.Text = "Response Received.The contents of the file uploaded are: {0}" + System.Text.Encoding.ASCII.GetString(responseArray);
                return true;
            }
            catch
            {
                MessageBox.Show("Failed to post file");
                return false;
            }
        }

        private TextBox EditSpecific(TextBox box)
        {
            int s = 0;
            if (CriteriaSelected)//criteria or sub-criteria selected?
            {
                s = MaxSub;
            }
            else
            {
                s = SSub;
            }

            addcommentForm2.Passvalue[0] = "General Comments";
            addcommentForm2.Passvalue[1] = box.Text;
            //addcommentForm.CForm = CommentsForm;
            addcommentForm2.ComFile = CommentFile;
            addcommentForm2.ShowDialog();

            box.Text = addcommentForm2.Passvalue[1];
            if (addcommentForm2.ComFile != CommentFile && addcommentForm2.ComFile != null)
            {
                CommentFile = addcommentForm2.ComFile;
            }
            return box;
        }

        private void specificEditbutton_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox2Comments_DoubleClick(object sender, EventArgs e)
        {
            textBox2Comments = EditSpecific(textBox2Comments); //edit comments text box if double click on it
        }

        private void Clicklabel2_Click(object sender, EventArgs e)
        {
            if (startMark)
            {
                textBox2Comments = EditSpecific(textBox2Comments); //edit comments text box if double click on it        }
            }
        }
        private void Clicklabel1_Click(object sender, EventArgs e)
        {
            if (startMark)
            {
                textBox10 = EditSpecific(textBox10); //edit comments text box if double click on it
            }
        }

        private void templatelabel_Click(object sender, EventArgs e)
        {

        }


        private void highlightButton_Click(object sender, EventArgs e)
        {
            if (highlightButton.Text == "tick")
            {
                highlightButton.Text = "highlight";
            }
            else if (highlightButton.Text == "highlight")
            {
                highlightButton.Text = "tick+high";
            }
            else if (highlightButton.Text == "tick+high")
            {
                highlightButton.Text = "tick+high+bold";
            }
            else if (highlightButton.Text == "tick+high+bold")
            {
                highlightButton.Text = "high+bold";
            }
            else if (highlightButton.Text == "high+bold")
            {
                highlightButton.Text = "no tick";
            }
            else if (highlightButton.Text == "no tick")
            {
                highlightButton.Text = "append tick";
            }
            else if (highlightButton.Text == "append tick")
            {
                highlightButton.Text = "tick";
            }
        }

        private void fine05ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fineWeight = 0.5;
            Set_Weights();
        }

        private void normal1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fineWeight = 1;
            Set_Weights();
        }

        private void coarse5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fineWeight = 5;
            Set_Weights();
        }
        private void Set_Weights()
        {
            if (fineWeight == 1)
            {
                normal1ToolStripMenuItem.Checked = true;
                fine05ToolStripMenuItem.Checked = false;
                coarse5ToolStripMenuItem.Checked = false;
            }
            else if (fineWeight == 5)
            {
                coarse5ToolStripMenuItem.Checked = true;
                normal1ToolStripMenuItem.Checked = false;
                fine05ToolStripMenuItem.Checked = false;
            }
            else //0.5
            {
                fine05ToolStripMenuItem.Checked = true;
                normal1ToolStripMenuItem.Checked = false;
                coarse5ToolStripMenuItem.Checked = false;
            }
            Build_Criteria_List();
        }

        private void repCancelbutton4_Click(object sender, EventArgs e)
        {
            replicate_Description = false;
            treeView2.SelectedNode = SelNode;
            Show_Label("Don't forget to save changes!", 1500);
            repCancelbutton4.Visible = false;
        }
    }

       
    }




