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
    public partial class CommentForm : Form
    {
        bool Category = true;
        bool editing = false;
        bool makingChanges = false;
       
        public string CFile = "";
        public string CPath = "";
        bool unsaved = false;
        bool moveItems = false;
        string oldCategory = "";

        private string Nm;

        public string Passvalue
        {
            get { return Nm; }
            set { Nm = value; }
        }

        public bool selectComment = true;

        struct comments
        {
            public string comment;
            public string category;
            public int index;
        }
        List<comments> commentlist = new List<comments>();

        public CommentForm()
        {
            InitializeComponent();
        }

        private void addHeadingToolStripMenuItem_Click(object sender, EventArgs e)
        { //add category to listbox1
            if (Movebutton.Text == "Move Selected")
            {
                MessageBox.Show("Invalid - moving comments");
                return;
            }
            editing = false;
            Category = true;
            textBox1.Text = "";
            listBox2.Items.Clear(); //clear comments box to make way for new category
            textBox2.Text = ""; //clear current comment
            unsaved = true;
            showCategory(true);

        }

        private void showCategory(bool b)
        {
            savebutton.Visible = b;
            cancelbutton.Visible = b;
            //label3.Visible = b;
            //textBox1.Visible = b;
            textBox1.ReadOnly = !b;
            Movebutton.Visible = !b;

            contextMenuStrip2.Enabled = !b;
            contextMenuStrip1.Enabled = !b;
            menuStrip1.Enabled = !b;
        }

        private void setMenus(bool b)
        {
            //menuStrip1.Visible = !b;
            contextMenuStrip1.Visible = !b;
            contextMenuStrip2.Visible = !b;
            label5.Visible = b;
            label6.Visible = !b;
        }

        private void CommentForm_Load(object sender, EventArgs e)
        {
            LoadCommentsFromFile(CFile);    
            setMenus(selectComment);
            if (listBox2.Items.Count > 0)
            {
                listBox2.ContextMenuStrip = contextMenuStrip2;
                listBox2.SelectedIndex = 0;
            }
            if (listBox1.Items.Count > 0)
            {
                listBox1.SelectedIndex = 0;
            }
        }

        private void savebutton_Click(object sender, EventArgs e)
        {
            if (Category)
            {
                saveCategory();
                Category = false;
            }
            else
            {
                saveComment();
            }
            MessageBox.Show("Now save these comments from the File menu");
            makingChanges = false;
        }
        private void saveCategory()
        {
            
            if (!editing) //if adding category
            {
                listBox1.Items.Add(textBox1.Text);
                listBox1.SelectedIndex = listBox1.Items.Count-1;
                editing = false;
                
            }
            else //if editing
            {
                if (listBox1.SelectedIndex > -1)
                {
                    listBox1.Items[listBox1.SelectedIndex] = textBox1.Text;
                    updateCategoryComments(textBox1.Text);
                  
                }
            }
            showCategory(false);
        }

        private void updateCategoryComments(string cat)
        {
            comments c;
            for (int i =0; i < commentlist.Count; i++)
            {
                try
                {
                    if (commentlist[i].category == oldCategory)
                    {
                        c.comment = commentlist[i].comment;
                        c.index = commentlist[i].index;
                        c.category = cat;
                        commentlist.Insert(i, c);
                        commentlist.RemoveAt(i + 1);
                    }
                }
                catch { }
            }
           
        }
        

        private void cancelbutton_Click(object sender, EventArgs e)
        {
             DialogResult dialogResult = MessageBox.Show("Cancel edit Yes/No?", "Cancel", MessageBoxButtons.YesNo);
             if (dialogResult == DialogResult.Yes)
             {
                 editing = false;
                 showCategory(false);
                 showComment(false);
                 Category = false;                
             }
             makingChanges = false;
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                textBox1.Text = listBox1.SelectedItem.ToString();
            }
        }

        private void deleteCategory()
        {
            if (Movebutton.Text == "Move Selected")
            {
                MessageBox.Show("Invalid - moving comments");
                return;
            }
            Movebutton.Visible = false;
            comments c;
            c.category = listBox1.SelectedItem.ToString();
            try
            {
                DialogResult dialogResult = MessageBox.Show("Delete category and all associated comments Yes/No?", "Delete category and comments", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (listBox1.Items.Count > 0)
                    {
                        if (listBox1.SelectedIndex > -1)
                        {
                            
                            if (commentlist.Count > 0)
                            {
                                for (int i = 0; i < commentlist.Count; i++)
                                {
                                    if (commentlist[i].category.Trim() == listBox1.SelectedItem.ToString())
                                    {
                                        try
                                        {
                                            commentlist.RemoveAt(i);
                                            listBox2.Items.RemoveAt(i);
                                        }
                                        catch { }
                                    }
                                }
                            }
                            int s = listBox1.SelectedIndex;
                            if (listBox1.Items.Count > 1)
                            {
                                listBox1.SelectedIndex = listBox1.Items.Count - 2;
                            }
                        
                            listBox1.Items.RemoveAt(s);
                        }
                    }
                    textBox1.Text = "";
                }
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                //MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
            Movebutton.Visible = true;
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {
            if (Movebutton.Text == "Move Selected")
            {
                MessageBox.Show("Invalid - moving comments");
                return;
            }
            if (contextMenuStrip1.Items[0].Selected) //edit category
            {
                if (listBox1.SelectedIndex > -1)
                {
                    oldCategory = listBox1.SelectedItem.ToString();
                    editing = true;
                    Category = true;
                    makingChanges = true;
                    showCategory(true);
                }
                else
                {
                    MessageBox.Show("Select an item to edit");
                }
            }
            else if (contextMenuStrip1.Items[1].Selected) //delete category
            {
                deleteCategory();
            }
            else if (contextMenuStrip1.Items[2].Selected) //add comment
            {
                if (listBox1.SelectedIndex > -1)
                {
                    textBox1.Text = listBox1.SelectedItem.ToString();
                }
                Category = false;
                editing = false;
                showComment(true);
                makingChanges = true;
                textBox2.Text = "";
            }
            unsaved = true;
        }

        private void showComment(bool b)
        {
            savebutton.Visible = b;
            cancelbutton.Visible = b;
            //label3.Visible = b;
            //textBox1.Visible = b;
            textBox1.ReadOnly = true;
            //textBox2.Visible = b;
            //label4.Visible = b;
            textBox2.ReadOnly = !b;
            contextMenuStrip2.Enabled = !b;
            contextMenuStrip1.Enabled = !b;
            menuStrip1.Enabled = !b;
            Movebutton.Visible = !b;
        }

        private void saveComment()
        {
            comments c;
            if (!editing) //adding comment
            {
                c.comment = textBox2.Text;
                c.category = textBox1.Text;
               
                listBox2.Items.Add(textBox2.Text);
                listBox2.SelectedIndex = listBox2.Items.Count-1;
                c.index = listBox1.SelectedIndex;
                commentlist.Add(c);
                if (listBox2.Items.Count > 0)
                {
                    listBox2.ContextMenuStrip = contextMenuStrip2;
                }
            }
            else //edit comment
            {
                if (listBox2.SelectedIndex > -1)
                {
                    listBox2.Items[listBox2.SelectedIndex] = textBox2.Text;
                    int i = listBox2.SelectedIndex;
                    c = commentlist[i];
                    c.comment = textBox2.Text;
                    commentlist[i] = c;                   
                }
            }
            showComment(false);
            makingChanges = false;
        }

        private void contextMenuStrip2_Click(object sender, EventArgs e)
        {
            if (Movebutton.Text == "Move Selected")
            {
                MessageBox.Show("Invalid - moving comments");
                return;
            }
            Category = false;
            if (contextMenuStrip2.Items[0].Selected) //edit comment
            {
                if (listBox2.SelectedIndex > -1)
                {
                    textBox2.Text = listBox2.SelectedItem.ToString();
                    editing = true;
                    Category = false;
                    showComment(true);
                }
                else
                {
                    MessageBox.Show("Select an item to edit");
                }
            }
            else if (contextMenuStrip2.Items[1].Selected) //delete comment
            {
                deleteComment();
            }
            unsaved = true;
           
        }

        private void deleteComment()
        {
            if (Movebutton.Text == "Move Selected")
            {
                MessageBox.Show("Invalid - moving comments");
                return;
            }
            Movebutton.Visible = false;
            comments c;
            c.category = listBox1.SelectedItem.ToString();
            try
            {
                DialogResult dialogResult = MessageBox.Show("Delete commentsYes/No?", "Delete comment", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (listBox2.Items.Count > 0)
                    {
                        if (listBox2.SelectedIndex > -1)
                        {
                           
                            if (commentlist.Count > 0)
                            {
                                commentlist.RemoveAt(listBox2.SelectedIndex);                              
                            }
                            listBox2.Items.RemoveAt(listBox2.SelectedIndex);
                            listBox2.SelectedIndex = listBox2.Items.Count - 1;
                            MessageBox.Show("Now save these comments from the File menu");
                        }
                    }
                   // textBox2.Text = "";
                }
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
            Movebutton.Visible = true;
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (selectComment)
                {
                    string str = listBox2.SelectedItem.ToString();
                    if (str.Length > 0)
                    {
                        Passvalue = listBox2.SelectedItem.ToString();
                    }
                    else
                    {
                        MessageBox.Show("No comment highlighted");
                        return;
                    }
                }
            }
            catch {
                
            }
            if (unsaved)
            {
                DialogResult dialogResult = MessageBox.Show("You have unsaved comments - exit Yes/No?", "Unsaved comments", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Save comments from the File menu");
                }
            }
            else
            {
                this.Hide();
            }
                
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) //when select categories listbox
        {
            updateCategoryListBox();
        }
        private void updateCategoryListBox()
        {
            if (!makingChanges)
            {
                if (!moveItems)
                {
                    listBox2.Items.Clear();
                    if (commentlist.Count > 0)
                    {
                        for (int i = 0; i < commentlist.Count; i++)
                        {
                            if (commentlist[i].category == listBox1.SelectedItem.ToString())
                            {
                                listBox2.Items.Add(commentlist[i].comment);
                            }
                        }

                    }
                }
            }

            if (listBox1.Items.Count > 0)
            {
                textBox1.Text = listBox1.Text; //update comment
            }
        }

        private void saveCommentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = CPath;
            saveFileDialog1.ShowDialog();
        }

        private void SaveCommentsToFile(string file)
        {          
            // write comments to file:
            try
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    for (int i =0; i < listBox1.Items.Count; i++)
                    {
                        sw.WriteLine("Category: " + listBox1.Items[i].ToString());
                        for (int j = 0; j < commentlist.Count ; j++)
                        {
                            if (commentlist[j].category == listBox1.Items[i].ToString())
                            {
                                sw.WriteLine("Comment: " + commentlist[j].comment);
                            }
                        }
                        sw.WriteLine("EndCategory: ");
                    }
                    sw.Close();
                    //unsaved = false;
                 }
                this.CFile = file;
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (saveFileDialog1.FileName.Length > 0)
            {
                CFile = saveFileDialog1.FileName;
                SaveCommentsToFile(saveFileDialog1.FileName);
                unsaved = false;
            }
        }

        private void loadCommentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            CFile = openFileDialog1.FileName;
            CPath = Path.GetDirectoryName(CFile);
            LoadCommentsFromFile(openFileDialog1.FileName);
        }

        private void LoadCommentsFromFile(string file)
        {
            string str;
            string str2;
            string category = "";
            int i = 0;
            comments c;
           
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            commentlist.Clear();
            makingChanges = true;
            // read comments from file:
            if (!File.Exists(file))
            {
                if (selectComment)
                {
                    MessageBox.Show("No comments file specified");
                }
                else
                {
                    MessageBox.Show("Note: need to create a new comments file");
                }
                return;
            }
            try
            {
                using (StreamReader sw = new StreamReader(file))
                {
                    while (!sw.EndOfStream)
                    {
                        str = sw.ReadLine();
                        if (str.StartsWith("Category:"))
                        {
                            str2 = str.Substring("Category: ".Length, str.Length - "Category: ".Length);
                            category = str2;
                            listBox1.Items.Add(str2);
                        }
                        if (str.StartsWith("Comment:"))
                        {
                            str2 = str.Substring("Comment: ".Length, str.Length - "Comment: ".Length);
                            //listBox2.Items.Add(str2);
                            c.category = category;
                            c.comment = str2;
                            c.index = i;
                            commentlist.Add(c);
                        }
                        if (str.StartsWith("EndCategory"))
                        {
                            i++;
                        }
                    }

                    sw.Close();
                }                
                makingChanges = false;
                if (listBox1.Items.Count > 0)
                {
                    listBox1.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Comments file appears to be empty");
                }
                if (listBox2.Items.Count > 0)
                {
                    listBox2.ContextMenuStrip = contextMenuStrip2;
                }
                this.CFile = file;
               
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
                
            }
        }

        private void Closebutton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult;
            if (!selectComment)
            {
                if (unsaved)
                {
                    dialogResult = MessageBox.Show("Exit without saving comments?", "Exit", MessageBoxButtons.YesNo);
                }
                else
                {
                    dialogResult = MessageBox.Show("Exit comment editor?", "Exit", MessageBoxButtons.YesNo);
                }
                if (dialogResult == DialogResult.No)
                {
                    Passvalue = "";
                    MessageBox.Show("Save comments from File menu");
                    return;
                }
                else
                { Passvalue = CFile; }//filename

            }
            else
            {
                Passvalue = ""; //closing 
            }
            cancelbutton.Visible = false;
            savebutton.Visible = false;
            //textBox1.Visible = false;
            //label3.Visible = false;
            editing = false;
            showCategory(false);
            showComment(false);
            Category = false;
            if (unsaved)
            {
                if (MessageBox.Show("Exit without saving comments?", "Exit", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    MessageBox.Show("Save comments from File menu");
                    return;
                }                
            }
            this.Hide();
        }

        private void newCommentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Create new comments file yes/no?", "New comments", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                listBox1.Items.Clear();
                listBox2.Items.Clear();
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = listBox2.Text;
        }

        private void Movebutton_Click(object sender, EventArgs e)
        {
            if (Movebutton.Text.Trim() == "Move Selected")
            {
               if (oldCategory == listBox1.SelectedItem.ToString())
                {
                    MessageBox.Show("Cannot move comments to same category");
                    return;
                }
                MoveSelected();
                Movebutton.Text = " Move Comments";
                moveItems = false;
                listBox2.SelectionMode = SelectionMode.One;
                cancelMovebutton.Visible = false;
            }
            else
            {
                listBox2.SelectionMode = SelectionMode.MultiSimple;
                oldCategory = listBox1.SelectedItem.ToString();
                MessageBox.Show("Select Comments to move then select new category and hit Move Selected");
                Movebutton.Text = "Move Selected";
                moveItems = true; //prevent category listbox from being updated whilest moving
                cancelMovebutton.Visible = true;
            }
        }
        private void MoveSelected()
        {
            comments c;
            string str = "";
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                if (listBox2.SelectedIndices.Contains(i))
                {
                  
                    str = listBox1.SelectedItem.ToString();
                    c.category = str;
                    c.comment = listBox2.Items[i].ToString();
                    c.index = listBox1.SelectedIndex;
                    commentlist.Add(c);

                }
            }
           
            for (int i = listBox2.Items.Count -1; i >=0;  i--)
            {
                if (listBox2.SelectedIndices.Contains(i))
                {
                    deleteMovedComment(str, i);
                }
            }
            moveItems = false;  //allow category listbox to be updated now
            updateCategoryListBox();
            listBox2.SelectedIndex = listBox2.Items.Count - 1;
            MessageBox.Show("Now save these comments from the File menu");
        }
        private void deleteMovedComment(string str, int i)
        {
            comments c;
            c.category = str;
            try
            {
               
                    if (listBox2.Items.Count > 0)
                    {                        
                            if (commentlist.Count > 0)
                            {
                                commentlist.RemoveAt(i);
                            }
                            listBox2.Items.RemoveAt(i);                           
                        
                    }
                    // textBox2.Text = "";
                
            }
            catch (System.Exception excep)
            {
                StackTrace stackTrace = new StackTrace();
                MessageBox.Show("In: " + stackTrace.GetFrame(0).GetMethod().Name + ", " + excep.Message);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void cancelMovebutton_Click(object sender, EventArgs e)
        {
            Movebutton.Text = " Move Comments";
            moveItems = false;
            listBox2.SelectionMode = SelectionMode.One;
            cancelMovebutton.Visible = false;
        }
    }
}
