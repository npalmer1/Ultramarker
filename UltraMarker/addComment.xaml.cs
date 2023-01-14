using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
//using System.Windows.Forms;

namespace UltraMarker
{
    /// <summary>
    /// Interaction logic for addComment.xaml
    /// </summary>
    

    public partial class addComment : UserControl
    {
    
        CommentForm CForm = new CommentForm();
        public string ComFile;
        public string BoxText;
        public bool View = false;

        public addComment()
        {
            InitializeComponent();
            TextBox1.SpellCheck.IsEnabled = true;
            TextBox1.Text = BoxText;          
        }

        private void TextBox1_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            BoxText = TextBox1.Text;
        }

        private void TextBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string CommentStr;
            if (View)
            {
                return;
            }
            try
            {
                if (File.Exists(ComFile))
                {
                    CForm.selectComment = true; //loads form anyway
                    CForm.CFile = ComFile;
                    CForm.ShowDialog();
                    CommentStr = CForm.Passvalue;
                    if (CommentStr != null && CommentStr != "")
                    {
                        TextBox1.Text = TextBox1.Text.Insert(TextBox1.SelectionStart + TextBox1.SelectionLength, CommentStr);

                    }
                    ComFile = CForm.CFile;
                }
                else
                {

                    //MessageBox.Show("No comments file selected - you need to select or create one from File menu");
                    CForm.selectComment = true; //loads form anyway
                    //CForm.CFile = ComFile;
                    CForm.ShowDialog();
                    CommentStr = CForm.Passvalue;
                    if (CommentStr != null && CommentStr != "")
                    {
                        TextBox1.Text = TextBox1.Text.Insert(TextBox1.SelectionStart + TextBox1.SelectionLength, CommentStr);

                    }
                    ComFile = CForm.CFile;
                }

            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /*private void TextBox1_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {

        }*/





        /* private void TextBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
         {
             string CommentStr;

             try
             {
                 if (File.Exists(ComFile))
                 {
                     CForm.selectComment = true; //loads form anyway
                     CForm.CFile = ComFile;
                     CForm.ShowDialog();
                     CommentStr = CForm.Passvalue;
                     if (CommentStr != null && CommentStr != "")
                     {
                         textBox1.Text = textBox1.Text.Insert(textBox1.SelectionStart + textBox1.SelectionLength, CommentStr);

                     }
                     ComFile = CForm.CFile;
                 }
                 else
                 {

                     MessageBox.Show("No comments file selected - you need to select or create one from Comments menu");
                 }

             }
             catch
             {
                 MessageBox.Show("Error");
             }
         }*/


    }
}
