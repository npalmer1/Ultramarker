namespace UltraMarker
{
    partial class GradeGroup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.printbutton = new System.Windows.Forms.Button();
            this.savebutton = new System.Windows.Forms.Button();
            this.editbutton = new System.Windows.Forms.Button();
            this.cancelbutton = new System.Windows.Forms.Button();
            this.closebutton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(38, 10);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(968, 529);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // printbutton
            // 
            this.printbutton.Location = new System.Drawing.Point(154, 551);
            this.printbutton.Margin = new System.Windows.Forms.Padding(2);
            this.printbutton.Name = "printbutton";
            this.printbutton.Size = new System.Drawing.Size(56, 27);
            this.printbutton.TabIndex = 1;
            this.printbutton.Text = "Print";
            this.printbutton.UseVisualStyleBackColor = true;
            this.printbutton.Click += new System.EventHandler(this.printbutton_Click);
            // 
            // savebutton
            // 
            this.savebutton.Cursor = System.Windows.Forms.Cursors.Default;
            this.savebutton.Location = new System.Drawing.Point(230, 551);
            this.savebutton.Margin = new System.Windows.Forms.Padding(2);
            this.savebutton.Name = "savebutton";
            this.savebutton.Size = new System.Drawing.Size(59, 27);
            this.savebutton.TabIndex = 2;
            this.savebutton.Text = "Save";
            this.savebutton.UseVisualStyleBackColor = true;
            this.savebutton.Click += new System.EventHandler(this.savebutton_Click);
            // 
            // editbutton
            // 
            this.editbutton.Location = new System.Drawing.Point(309, 551);
            this.editbutton.Margin = new System.Windows.Forms.Padding(2);
            this.editbutton.Name = "editbutton";
            this.editbutton.Size = new System.Drawing.Size(56, 27);
            this.editbutton.TabIndex = 3;
            this.editbutton.Text = "Edit";
            this.editbutton.UseVisualStyleBackColor = true;
            this.editbutton.Visible = false;
            this.editbutton.Click += new System.EventHandler(this.editbutton_Click);
            // 
            // cancelbutton
            // 
            this.cancelbutton.Location = new System.Drawing.Point(386, 551);
            this.cancelbutton.Margin = new System.Windows.Forms.Padding(2);
            this.cancelbutton.Name = "cancelbutton";
            this.cancelbutton.Size = new System.Drawing.Size(56, 27);
            this.cancelbutton.TabIndex = 4;
            this.cancelbutton.Text = "Cancel";
            this.cancelbutton.UseVisualStyleBackColor = true;
            this.cancelbutton.Visible = false;
            this.cancelbutton.Click += new System.EventHandler(this.cancelbutton_Click);
            // 
            // closebutton
            // 
            this.closebutton.Location = new System.Drawing.Point(462, 551);
            this.closebutton.Margin = new System.Windows.Forms.Padding(2);
            this.closebutton.Name = "closebutton";
            this.closebutton.Size = new System.Drawing.Size(56, 27);
            this.closebutton.TabIndex = 5;
            this.closebutton.Text = "Close";
            this.closebutton.UseVisualStyleBackColor = true;
            this.closebutton.Click += new System.EventHandler(this.closebutton_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // GradeGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 587);
            this.Controls.Add(this.closebutton);
            this.Controls.Add(this.cancelbutton);
            this.Controls.Add(this.editbutton);
            this.Controls.Add(this.savebutton);
            this.Controls.Add(this.printbutton);
            this.Controls.Add(this.richTextBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GradeGroup";
            this.Text = "GradeGroup";
            this.Load += new System.EventHandler(this.GradeGroup_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button printbutton;
        private System.Windows.Forms.Button savebutton;
        private System.Windows.Forms.Button editbutton;
        private System.Windows.Forms.Button cancelbutton;
        private System.Windows.Forms.Button closebutton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}