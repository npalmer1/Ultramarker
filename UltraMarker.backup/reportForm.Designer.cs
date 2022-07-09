namespace UltraMarker
{
    partial class reportForm
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
            this.closeButton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.editButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.saveButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.printButton = new System.Windows.Forms.Button();
            this.Emailbutton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(763, 650);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click_1);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 39);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(929, 605);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // editButton
            // 
            this.editButton.Location = new System.Drawing.Point(364, 650);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(75, 23);
            this.editButton.TabIndex = 3;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.editButton_Click_1);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(528, 650);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Visible = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Rich Text Format (*.rtf) |*.rtf";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(217, 650);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 5;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(153, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(344, 20);
            this.textBox1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(98, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Name:";
            // 
            // printButton
            // 
            this.printButton.Location = new System.Drawing.Point(49, 650);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(75, 23);
            this.printButton.TabIndex = 9;
            this.printButton.Text = "Print";
            this.printButton.UseVisualStyleBackColor = true;
            this.printButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // Emailbutton
            // 
            this.Emailbutton.Location = new System.Drawing.Point(655, 650);
            this.Emailbutton.Name = "Emailbutton";
            this.Emailbutton.Size = new System.Drawing.Size(75, 23);
            this.Emailbutton.TabIndex = 10;
            this.Emailbutton.Text = "Email";
            this.Emailbutton.UseVisualStyleBackColor = true;
            this.Emailbutton.Visible = false;
            this.Emailbutton.Click += new System.EventHandler(this.Emailbutton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label2.Location = new System.Drawing.Point(217, 692);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Don\'t forget to save the report!";
            // 
            // reportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(953, 710);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Emailbutton);
            this.Controls.Add(this.printButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.closeButton);
            this.Name = "reportForm";
            this.Text = "Feedback Report";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.reportForm_FormClosed);
            this.Load += new System.EventHandler(this.reportForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.Button Emailbutton;
        private System.Windows.Forms.Label label2;
    }
}