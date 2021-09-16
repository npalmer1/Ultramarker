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

namespace UltraMarker
{
    public partial class summaryForm : Form
    {
        string[] Nm = new string[6];
        public int sort_type;
        public bool industrial;
        string percent_grade = "11";

        public string marksDirectory = "";
        public string assessmentFilePath;
        public string assessmentTitle = "";
        public string assessmentCode = "";
        public string UnitPath;
        public string moduleName;
        string marksCodeDir = "";

        struct st_struct
        {
            public string firstname;
            public string surname;
            public string percent;
            public string grade;
            public string mod;
        }
        List<st_struct> list1 = new List<st_struct>();
        


        public string[] Passvalue
        {
            get { return Nm; }
            set { Nm = value; }
        }
        string summaryFilename = "";

        public summaryForm()
        {
            InitializeComponent();
        }

        private void summaryForm_Load(object sender, EventArgs e)
        {
            radioButton2.Checked = true;
            marksCodeDir = marksDirectory + "\\" + assessmentCode;
            ModuletextBox.Text = moduleName;
            textBox1.Text = assessmentFilePath;
            
            textBox3.Text = marksCodeDir;
            textBox2.Text = assessmentTitle;
            textBox4.Text = assessmentCode;
            if (industrial)
            {
                modBox.Visible = false;
                radioButton2.Text = "Name";
                radioButton1.Visible = false;

            }
            try
            {
                
                sort_type = Convert.ToInt32(Passvalue[1]);
                percent_grade = Passvalue[2];
            }
            catch
            {
            }
            switch (sort_type)
            {
                case 0: radioButton1.Checked = true; break;
                case 1: radioButton2.Checked = true; break;
                case 2: radioButton3.Checked = true; break;
                case 3: radioButton4.Checked = true; break;
            }
            try
            {
                if (percent_grade.Length > 1)
                {
                    if (percent_grade[0] == '1')
                    {
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        checkBox1.Checked = false;
                    }
                    if (percent_grade[1] == '1')
                    {
                        checkBox2.Checked = true;
                    }
                    else
                    {
                        checkBox2.Checked = false;
                    }
                }
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            openFileDialog1.FileName = Path.GetDirectoryName(assessmentFilePath);
            openFileDialog1.InitialDirectory = assessmentFilePath; //assessnent criteria dir
            openFileDialog1.Filter = "Criteria file (*.cri)| *.cri";
            openFileDialog1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            folderBrowserDialog1.SelectedPath = marksCodeDir;// marks directory
            folderBrowserDialog1.ShowDialog();
            textBox3.Text = folderBrowserDialog1.SelectedPath;
            marksCodeDir = textBox3.Text;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string f = "";
            f = openFileDialog1.FileName;
            textBox1.Text = f;
            assessmentFilePath = f;
            GetAssessmentInfo(openFileDialog1.FileName);
        }
        private void GetAssessmentInfo(string filename)
        {
            string str = "";
            string str3 = "";
            try
            {
                using (StreamReader sw = new StreamReader(filename))
                {
                    while (!sw.EndOfStream)
                    {
                        str = sw.ReadLine();
                        str.Trim();
                        if (str.Length > 0)
                        {
                            if (str.Contains("Unit/Module:"))
                            {
                                int a = str.IndexOf(":");
                                str3 = str.Substring(a + 1).Trim();
                                ModuletextBox.Text = str3;
                                moduleName = str3;
                                //return;
                            }
                            if (str.Contains("Assessment title:"))
                            {
                                int a = str.IndexOf(":");
                                str3 = str.Substring(a+1).Trim();
                                textBox2.Text = str3;
                                assessmentTitle = str3;
                                //return;

                            }
                            if (str.Contains("Assessment code:"))
                            {
                                int a = str.IndexOf(":");
                                str3 = str.Substring(a + 1).Trim();
                                textBox4.Text = str3;
                                assessmentCode = str3;
                                //return;

                            }
                            
                        }
                      
                    }
                    sw.Close();
                    marksCodeDir = marksDirectory + "\\" + assessmentCode;
                    textBox3.Text = marksCodeDir;
                }
              
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

       
        private void exitButton_Click(object sender, EventArgs e)
        {
            Passvalue[1] = Convert.ToString(sort_type);
            Passvalue[2] = percent_grade;
            this.Close();
        }

        private void summaryButton_Click(object sender, EventArgs e)
        {
            try
            {
                if ((textBox1.Text.Trim().Length <1) || (textBox3.Text.Trim().Length <1))
                {
                    MessageBox.Show("Assessment filename and location of marks are not specified");
                    return;
                }

                string str = textBox1.Text;
                int i = str.IndexOf(".cri");
                str = str.Substring(0, i) + "_summary.rtf";
                //saveFileDialog1.FileName = System.IO.Path.ChangeExtension(textBox1.Text, ".sum");
                saveFileDialog1.FileName = str;
                saveFileDialog1.InitialDirectory = UnitPath;
                saveFileDialog1.Filter = "Summary file (*_summary.rtf)| *_summary.rtf";
                saveFileDialog1.ShowDialog();
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }


        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            GenerateSummary(saveFileDialog1.FileName);
        }

        private void GenerateSummary(string filename)
        {
            string str = "";
            string str3 = "";
            
            st_struct sts;
            sts.firstname = "";
            sts.surname = "";
            sts.percent = "";
            sts.grade = "";
            sts.mod = "";
            float group_total = 0;
            string[] name = new string[3];
            list1.Clear();
            summaryFilename = filename;
            try
            {
                using (StreamWriter sw2 = new StreamWriter(filename))
                {
                    sw2.WriteLine("Unit/module: " + moduleName);
                    sw2.WriteLine("-------");
                    sw2.WriteLine("Summary report for assessment: " + assessmentTitle +" , Code: " + assessmentCode);
                    sw2.WriteLine("-----------------------------");
                    string pc = "";
                    string gr = "";
                    if (checkBox1.Checked) pc = "%";
                    if (checkBox2.Checked) gr = "Grade";
                    sw2.WriteLine("Student".PadRight(30) + "\t\t "+ pc + " \t "+ gr);
                    sw2.WriteLine();
                    string[] files = Directory.GetFiles(marksCodeDir);
                    if (files.Count() <1)
                    {
                        MessageBox.Show("Folder contains no marked files");
                        sw2.Close();
                        return;
                    }
                    foreach (string file in files)
                    {
                        sts.firstname = "";
                        sts.surname = "";
                        sts.percent = "";
                        sts.grade = "";
                        sts.mod = "";
                        if (file.Contains(".mrk") || (file.Contains(".mrm")))
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
                                                if (name.Count() == 4)
                                                {
                                                    sts.surname = name[3];
                                                }
                                                else if (name.Count() == 3)
                                                {
                                                    sts.surname = name[2];
                                                }
                                                else if (name.Count() == 2)
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
                                        else if (str.Contains("Overall mark:") && checkBox1.Checked)
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
                                        else if (str.Contains("Equivalent grade:") && checkBox2.Checked)
                                        {
                                            sts.grade = str3;
                                        }
                                        else if (str.Contains("Moderated:"))
                                        {
                                            if (str3.Contains("Y") && modBox.Checked)
                                            {
                                                sts.mod = "*";
                                            }
                                        }

                                    }
                                  
                                }
                                list1.Add(sts);
                                sw.Close();
                            }

                        }

                    } //for each
                    //list1.Sort(sts.surname);
                     SortList();
                     foreach (st_struct st in list1)
                     {
                         if (sort_type == 1)
                         {                        
                                sw2.Write((st.surname + " " + st.firstname).PadRight(35,'-') + " \t" + st.percent + " \t" + st.grade + " \t" + st.mod);
                                sw2.WriteLine();
                         }
                         else 
                         {
                             sw2.Write((st.firstname + " " + st.surname).PadRight(39,'-') + " \t" + st.percent + " \t" + st.grade + " \t" + st.mod);
                             sw2.WriteLine();
                         }
                           
                     }
                     sw2.WriteLine();
                    try
                    {
                        if (checkBox1.Checked) sw2.WriteLine("Average percentage: " + Convert.ToString(Math.Round(group_total/list1.Count)) + " %");
                        sw2.WriteLine("Number of students marked: " + Convert.ToString(list1.Count));
                        if (modBox.Checked)
                        { sw2.WriteLine(" \t" + "* moderated"); }
                    }
                    catch{
                    }
                    sw2.Close();
                }//streamwriter
                button3.Visible = true;

            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void SortList()
        {
            try
            {
                if (sort_type == 0)
                {
                    list1.Sort(delegate(st_struct x, st_struct y)
                    {
                        return x.firstname.CompareTo(y.firstname);
                    });
                }
                else if (sort_type == 1)
                {
                    list1.Sort(delegate(st_struct x, st_struct y)
                    {
                        return x.surname.CompareTo(y.surname);
                    });
                }
                else if (sort_type == 2)
                {
                    list1.Sort(delegate(st_struct x, st_struct y)
                    {
                        return y.percent.CompareTo(x.percent);
                    });
                }
                else if (sort_type == 3)
                {
                    list1.Sort(delegate(st_struct x, st_struct y)
                   {
                       return y.grade.CompareTo(x.grade);
                   });
                }
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
           
        }

        private void GenerateSummaryFile(string filename) //not used anymore
        {
            string str = "";
            string str3 = "";
            summaryFilename = filename;
            try
            {
                using (StreamWriter sw2 = new StreamWriter(filename))
                {
                    sw2.WriteLine("Unit/module: " + moduleName);
                    sw2.WriteLine("-------");
                    sw2.WriteLine("Summary report for assessment: " + textBox2.Text + " Code: " + assessmentCode);
                    sw2.WriteLine("-----------------------------");
                    sw2.WriteLine("Student \t\t % \t Grade");
                    string[] files = Directory.GetFiles(textBox3.Text);
                    foreach (string file in files)
                    {
                        if (file.Contains(".mrk"))
                        {
                            sw2.WriteLine();
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
                                            sw2.Write(str3 + "\t");
                                        }
                                        else if (str.Contains("Overall mark:"))
                                        {
                                            sw2.Write(str3 + "\t"); ;
                                        }
                                        else if (str.Contains("Equivalent grade:"))
                                        {
                                            sw2.Write(str3 + " ");
                                        }

                                    }
                                }
                                sw.Close();
                            }
                        }

                    } //for each
                    sw2.Close();
                }//streamwriter
                button3.Visible = true;
            
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

      

        private void button3_Click(object sender, EventArgs e)
        {
            show_report();
        }
        private void show_report()
        {
            string str = "";
            try
            {
                summaryShowForm hForm = new summaryShowForm();
                using (StreamReader sw = new StreamReader(summaryFilename))
                {
                    while (!sw.EndOfStream)
                    {
                        str = str + sw.ReadLine() + System.Environment.NewLine;
                    }
                    sw.Close();
                }
                hForm.Passvalue = str;
                hForm.filePath = summaryFilename;
                hForm.module = moduleName;
                hForm.Show();
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button3.Visible = false;
            sort_type = 0;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button3.Visible = false;
            sort_type = 1;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            button3.Visible = false;
            sort_type = 2;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            button3.Visible = false;
            sort_type = 3;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            string str = "";
            if (checkBox1.Checked)
            {
                str = "1";
            }
            else
            {
                str= "0";
            }
            try
            {
                percent_grade = str + percent_grade[1];
            }
            catch
            {
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            string str = "";
            if (checkBox2.Checked)
            {
                str = "1";
            }
            else
            {
                str = "0";
            }
            try
            {
                percent_grade = percent_grade[0] + str;
            }
            catch
            {
            }
        }
    }
}
