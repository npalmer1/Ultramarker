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

namespace UltraMarker
{
    public partial class loForm : Form
    {

        //public string[] LOList = new string[50];
        public List<string> LODesc = new List<string>();
        public List<string> LOTitle = new List<string>();

       

        public int LOSelected;
        public string SelectedLO;
        public string LOList;
        public string LOFilePath;
        //public string LOPath;
        public bool FirstOpen = true;

        bool editing = false;
        int oldindex = 0;
        bool saved = true;

        

        public bool selectLO;
        bool editmode = false;
        bool addmode = false;
        int MaxLOs = 50;
       

        public loForm()
        {
            InitializeComponent();

        }

         private void loForm_Activated(object sender, EventArgs e)
        {
           
         }

        private void loForm_Load_1(object sender, EventArgs e)
        {
            string str;           
            try
            {
                    saved = true;
                    listBox1.Items.Clear();
                    if (FirstOpen)
                    {
                        if (LOFilePath != null)
                        {
                            if (LOFilePath.Length > 0)
                            {
                                ReadLOFromFile(LOFilePath); //read LOs from file if one exists
                            }
                        }
                        FirstOpen = false;                     
                    }
                    else
                    {
                        for (int i = 0; i < LOTitle.Count; i++)
                        {

                            if (LOTitle[i] != null)
                            {

                                listBox1.Items.Add(LOTitle[i]);
                            }
                        }
                    }
                    if (selectLO)
                    {
                            //saved = true;
                            listBox1.SelectionMode = SelectionMode.MultiSimple;
                            button3.Text = "Cancel";
                            button3.Visible = true;
                           
                            OKbutton.Visible = true;
                            contextMenuStrip1.Enabled = false;
                            menuStrip1.Visible = false;
                          
                           
                            if (SelectedLO != null)
                            {
                                int f = listBox1.FindString(SelectedLO.Trim());
                                if (f > 0)
                                {
                                    listBox1.SelectedIndex = f;
                                }
                            }
                     }
                     else
                     {
                            //saved = false;
                            listBox1.SelectionMode = SelectionMode.One;
                            contextMenuStrip1.Enabled = true;
                            menuStrip1.Visible = true;
                            button3.Visible = true;
                            button3.Text = "Close";
                            OKbutton.Visible = false;
                     }
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        void Add_LO(bool insert)
        {
            //Add or insert new LO
            string str, str2;
            str = textBox1.Text;
            str2 = textBox2.Text;
            int insI = 0;
           
            if (listBox1.Items.Count > 0)
            {
                insI = listBox1.SelectedIndex;
            }
            if (str.Length > 0)
            {
                if (insert)
                {
                    LOTitle.Insert(insI+1, str);
                    LODesc.Insert(insI + 1, str2);
                    listBox1.Items.Insert(insI + 1, str2);
                    listBox1.SelectedIndex = listBox1.SelectedIndex + 1;
                  
                }
                else
                {
                    LOTitle.Add(str);
                    LODesc.Add(str2);
                    listBox1.Items.Add(str);
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                   
                }
               
            }
        }

        

        private void button1_Click(object sender, EventArgs e)
        {   //save button
            
                if (editmode)
                {
                    
                    try
                    {
                        int i = listBox1.SelectedIndex;
                        //listBox1.SelectedItem. = textBox1.Text;
                        listBox1.Items.RemoveAt(i);
                        listBox1.Items.Insert(i, textBox1.Text);
                        listBox1.SelectedIndex = oldindex;
                        LOTitle[i] = textBox1.Text;
                        LODesc[i] = textBox2.Text;
                    }
                    catch
                    {
                    }
                  
                    
                }
                else
                {
                    if (addmode)
                    {
                        Add_LO(false);
                        addmode = false;
                    }
                    else //insertmode
                    {
                        Add_LO(true);
                        addmode = false;

                    }
                   
                }
                button1.Visible = false;
                button2.Visible = false;
                OKbutton.Visible = false;
                button3.Visible = true;
                textBox1.ReadOnly  = true;
                textBox2.ReadOnly = true;
                editing = false;
                saved = false;
            
                listBox1.EndUpdate();
              
                MessageBox.Show("Don't forget to save changes to LO File!");   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //cancel
            editing = false;
            button1.Visible = false;
            button2.Visible = false;
            OKbutton.Visible = false;
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            listBox1.EndUpdate();
            //label1.Visible = false;
        }

       

        private void contextMenuStrip1_Click_1(object sender, EventArgs e)
        {
            string str;
            try
            {
                
                textBox1.Text = "";
              
                //edit, insert or delete LO
                if (contextMenuStrip1.Items[0].Selected)
                {
                    //EDIT LO
                    listBox1.BeginUpdate();
                    if (listBox1.SelectedIndex >= 0)
                    {
                        oldindex = listBox1.SelectedIndex;
                        editing = true;
                        editmode = true;
                        button1.Visible = true;
                        button2.Visible = true;
                        button3.Visible = false;
                        textBox1.ReadOnly = false;
                        textBox2.ReadOnly = false;

                        //label1.Visible = true;

                        try
                        {

                            //str = listBox1.Items[listBox1.SelectedIndex].ToString();
                            str = listBox1.SelectedItem.ToString();
                            textBox1.Text = str.Trim();
                            textBox2.Text = LODesc[listBox1.SelectedIndex];

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
                else if (contextMenuStrip1.Items[1].Selected)
                {
                    //remove the LO node (gets selected node from treeview1_NodeMouseClick):
                    DialogResult dialogResult = MessageBox.Show("Delete Yes/No?", "Remove LO", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                     
                        int i = listBox1.SelectedIndex;
                        if (i > -1)
                        {
                            str = listBox1.Items[i].ToString();
                            LOTitle.Remove(str);
                            LODesc.RemoveAt(i);
                            listBox1.Items.Remove(str);
                        }
                        else
                        {
                            MessageBox.Show("Select an LO to delete");
                        }
                                
                    }
                }
                else if (contextMenuStrip1.Items[2].Selected) //insert LO below
                {
                    listBox1.BeginUpdate();
                    oldindex = listBox1.SelectedIndex;
                    editing = true;
                    addmode = false;
                    editmode = false;
                    button1.Visible = true;
                    button2.Visible = true;
                    button3.Visible = false;
                    textBox1.ReadOnly = false;
                    textBox2.ReadOnly = false;
                   
                    //label1.Visible = true;
                }

            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

       

        private void Put_in_LO_List(string s)
        {
            if (listBox1.FindString(s) > 0)
            {
                //already in listbox
            }
            else
            {
                listBox1.Items.Add(s);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //close button       
            try
            {
                if (!selectLO)
                {
                    if (!saved) //force file save
                    {
                        DialogResult dialogResult = MessageBox.Show("Warning you haven't saved the LOs to file - save now Yes/No?", "Save LO", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            saveFileDialog1.ShowDialog();
                        }
                        saved = true;
                    }

                    if (listBox1.SelectedIndex > -1)
                    {
                        LOSelected = listBox1.SelectedIndex;
                        //SelectedLO = listBox1.Items[listBox1.SelectedIndex].ToString();
                        SelectedLO = listBox1.SelectedItem.ToString();
                    }

                    //label1.Visible = false;
                }
                else
                {
                  
                }
                button1.Visible = false;
                button2.Visible = false;
                OKbutton.Visible = false;
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
            this.Hide();
                
        }

        private void addLOtoolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            addmode = true;
            editmode = false;
            editing = true;
            oldindex = listBox1.SelectedIndex;

            //alladd LOs
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = false;
            textBox1.Text = "";
            textBox2.Text = "";
            
            //label1.Visible = true;   
        }

      
        private void filetoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveToolStripMenuItem.Selected)
            {  //Save LOs to file
            }
            else if (loadToolStripMenuItem.Selected)
            {  //load LOs from file
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            SaveLOToFile(saveFileDialog1.FileName);
            LOFilePath = saveFileDialog1.FileName;
        }

       

        private void SaveLOToFile(string filename)
        {
            // write grades to file:
            try
            {
                //int i =0;
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    
                   
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        sw.WriteLine("Learning Outcome: ");
                        sw.Write(LOTitle[i] + "||");
                        sw.WriteLine(LODesc[i]);
                        sw.WriteLine("EndLO:");                       
                    }
                    sw.Close();
                    LOFilePath = filename;
                    saved = true;
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
            string[] str2;
            string str = "";
            string line = "";
            
            try
            {
                Remove_LOs();

                LOFilePath = filename;
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
                                str2 = line.Split('|');
                                LOTitle.Add(str2[0]);
                                LODesc.Add(str2[2]);
                               
                                listBox1.Items.Add(str2[0]);
                                line = "";
                        }                        
                        else
                        {
                            line = line + str;
                           
                        }                                   
                    }
                    sw.Close();
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
           
            listBox1.Items.Clear();
            textBox1.Text = "";

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            ReadLOFromFile(openFileDialog1.FileName);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        
        private void OKbutton_Click(object sender, EventArgs e)
        {
            
            string str;
            //if selecting LOs and saving selection to main form
            try
            {
               
                LOList = "";
                foreach (int index in listBox1.SelectedIndices)
                {
                    str = listBox1.Items[index].ToString ();                   
                    LOList = LOList + str + Environment.NewLine;
                }
                this.Hide();
            }
            catch
            {
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!editing)
                {
                    int i = listBox1.SelectedIndex;
                    textBox1.Text = LOTitle[i];
                    textBox2.Text = LODesc[i];
                }
                else
                {

                   /* if (listBox1.SelectedIndex != oldindex)
                    {
                        MessageBox.Show("Save LO first");
                    }
                    listBox1.SelectedIndex = oldindex;
                    * */
                }
            }
            catch
            {
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clear all current LOs to start a new series of LOs
             DialogResult dialogResult = MessageBox.Show("Clear all current Learning Outcomes Yes/No?", "Clear LO", MessageBoxButtons.YesNo);
             if (dialogResult == DialogResult.Yes)
             {
                 listBox1.Items.Clear();
                 textBox1.Text = "";
                 textBox2.Text = "";
                 LOTitle.Clear();
                 LODesc.Clear();
                 saved = false;
             }

        }

        
     
    }
}
