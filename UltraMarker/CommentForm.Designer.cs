namespace UltraMarker
{
    partial class CommentForm
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
            this.components = new System.ComponentModel.Container();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addCommentStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCommentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadCommentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newCommentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.headingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addHeadingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.savebutton = new System.Windows.Forms.Button();
            this.cancelbutton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editCommentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteCommentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Closebutton = new System.Windows.Forms.Button();
            this.Movebutton = new System.Windows.Forms.Button();
            this.cancelMovebutton = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(8, 51);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(142, 424);
            this.listBox1.TabIndex = 0;
            this.listBox1.Click += new System.EventHandler(this.listBox1_Click);
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editStripMenuItem1,
            this.deleteStripMenuItem1,
            this.addCommentStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(159, 70);
            this.contextMenuStrip1.Click += new System.EventHandler(this.contextMenuStrip1_Click);
            // 
            // editStripMenuItem1
            // 
            this.editStripMenuItem1.Name = "editStripMenuItem1";
            this.editStripMenuItem1.Size = new System.Drawing.Size(158, 22);
            this.editStripMenuItem1.Text = "Edit Category";
            // 
            // deleteStripMenuItem1
            // 
            this.deleteStripMenuItem1.Name = "deleteStripMenuItem1";
            this.deleteStripMenuItem1.Size = new System.Drawing.Size(158, 22);
            this.deleteStripMenuItem1.Text = "Delete Category";
            // 
            // addCommentStripMenuItem
            // 
            this.addCommentStripMenuItem.Name = "addCommentStripMenuItem";
            this.addCommentStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.addCommentStripMenuItem.Text = "Add Comment";
            // 
            // listBox2
            // 
            this.listBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 17;
            this.listBox2.Location = new System.Drawing.Point(166, 51);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(818, 327);
            this.listBox2.TabIndex = 1;
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            this.listBox2.DoubleClick += new System.EventHandler(this.listBox2_DoubleClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.headingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(992, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveCommentsToolStripMenuItem,
            this.loadCommentsToolStripMenuItem,
            this.newCommentsToolStripMenuItem});
            this.fileToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveCommentsToolStripMenuItem
            // 
            this.saveCommentsToolStripMenuItem.Name = "saveCommentsToolStripMenuItem";
            this.saveCommentsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.saveCommentsToolStripMenuItem.Text = "Save Comments";
            this.saveCommentsToolStripMenuItem.Click += new System.EventHandler(this.saveCommentsToolStripMenuItem_Click);
            // 
            // loadCommentsToolStripMenuItem
            // 
            this.loadCommentsToolStripMenuItem.Name = "loadCommentsToolStripMenuItem";
            this.loadCommentsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.loadCommentsToolStripMenuItem.Text = "Load Comments";
            this.loadCommentsToolStripMenuItem.Click += new System.EventHandler(this.loadCommentsToolStripMenuItem_Click);
            // 
            // newCommentsToolStripMenuItem
            // 
            this.newCommentsToolStripMenuItem.Name = "newCommentsToolStripMenuItem";
            this.newCommentsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.newCommentsToolStripMenuItem.Text = "New Comments";
            this.newCommentsToolStripMenuItem.Click += new System.EventHandler(this.newCommentsToolStripMenuItem_Click);
            // 
            // headingsToolStripMenuItem
            // 
            this.headingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addHeadingToolStripMenuItem});
            this.headingsToolStripMenuItem.Name = "headingsToolStripMenuItem";
            this.headingsToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.headingsToolStripMenuItem.Text = "Category";
            // 
            // addHeadingToolStripMenuItem
            // 
            this.addHeadingToolStripMenuItem.Name = "addHeadingToolStripMenuItem";
            this.addHeadingToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.addHeadingToolStripMenuItem.Text = "Add Category";
            this.addHeadingToolStripMenuItem.Click += new System.EventHandler(this.addHeadingToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(164, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Comments:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Category:";
            // 
            // savebutton
            // 
            this.savebutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.savebutton.Location = new System.Drawing.Point(258, 522);
            this.savebutton.Name = "savebutton";
            this.savebutton.Size = new System.Drawing.Size(75, 23);
            this.savebutton.TabIndex = 6;
            this.savebutton.Text = "Save";
            this.savebutton.UseVisualStyleBackColor = true;
            this.savebutton.Visible = false;
            this.savebutton.Click += new System.EventHandler(this.savebutton_Click);
            // 
            // cancelbutton
            // 
            this.cancelbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelbutton.Location = new System.Drawing.Point(412, 521);
            this.cancelbutton.Name = "cancelbutton";
            this.cancelbutton.Size = new System.Drawing.Size(75, 23);
            this.cancelbutton.TabIndex = 7;
            this.cancelbutton.Text = "Cancel";
            this.cancelbutton.UseVisualStyleBackColor = true;
            this.cancelbutton.Visible = false;
            this.cancelbutton.Click += new System.EventHandler(this.cancelbutton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(166, 431);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(191, 21);
            this.textBox1.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(170, 414);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "Category:";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(8, 488);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(975, 21);
            this.textBox2.TabIndex = 11;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(170, 470);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "Comment:";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editCommentMenuItem,
            this.deleteCommentMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(165, 48);
            this.contextMenuStrip2.Click += new System.EventHandler(this.contextMenuStrip2_Click);
            // 
            // editCommentMenuItem
            // 
            this.editCommentMenuItem.Name = "editCommentMenuItem";
            this.editCommentMenuItem.Size = new System.Drawing.Size(164, 22);
            this.editCommentMenuItem.Text = "Edit Comment";
            // 
            // deleteCommentMenuItem
            // 
            this.deleteCommentMenuItem.Name = "deleteCommentMenuItem";
            this.deleteCommentMenuItem.Size = new System.Drawing.Size(164, 22);
            this.deleteCommentMenuItem.Text = "Delete Comment";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(383, 392);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(321, 15);
            this.label5.TabIndex = 13;
            this.label5.Text = "Select category and double click on a comment to select it";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(483, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 15);
            this.label6.TabIndex = 14;
            this.label6.Text = "Comment Editor";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "cts";
            this.saveFileDialog1.Filter = "Comment file (.cts) |*.cts";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "cts";
            this.openFileDialog1.Filter = "Comment file (.cts)| *.cts";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // Closebutton
            // 
            this.Closebutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Closebutton.Location = new System.Drawing.Point(748, 521);
            this.Closebutton.Name = "Closebutton";
            this.Closebutton.Size = new System.Drawing.Size(75, 23);
            this.Closebutton.TabIndex = 15;
            this.Closebutton.Text = "Close";
            this.Closebutton.UseVisualStyleBackColor = true;
            this.Closebutton.Click += new System.EventHandler(this.Closebutton_Click);
            // 
            // Movebutton
            // 
            this.Movebutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Movebutton.Location = new System.Drawing.Point(8, 516);
            this.Movebutton.Margin = new System.Windows.Forms.Padding(2);
            this.Movebutton.Name = "Movebutton";
            this.Movebutton.Size = new System.Drawing.Size(112, 29);
            this.Movebutton.TabIndex = 16;
            this.Movebutton.Text = "Move Comments";
            this.Movebutton.UseVisualStyleBackColor = true;
            this.Movebutton.Click += new System.EventHandler(this.Movebutton_Click);
            // 
            // cancelMovebutton
            // 
            this.cancelMovebutton.Location = new System.Drawing.Point(148, 519);
            this.cancelMovebutton.Margin = new System.Windows.Forms.Padding(2);
            this.cancelMovebutton.Name = "cancelMovebutton";
            this.cancelMovebutton.Size = new System.Drawing.Size(75, 24);
            this.cancelMovebutton.TabIndex = 17;
            this.cancelMovebutton.Text = "Cancel";
            this.cancelMovebutton.UseVisualStyleBackColor = true;
            this.cancelMovebutton.Visible = false;
            this.cancelMovebutton.Click += new System.EventHandler(this.cancelMovebutton_Click);
            // 
            // CommentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(992, 577);
            this.Controls.Add(this.cancelMovebutton);
            this.Controls.Add(this.Movebutton);
            this.Controls.Add(this.Closebutton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cancelbutton);
            this.Controls.Add(this.savebutton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.listBox1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "CommentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Comments";
            this.Load += new System.EventHandler(this.CommentForm_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveCommentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadCommentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem headingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addHeadingToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button savebutton;
        private System.Windows.Forms.Button cancelbutton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem editStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem addCommentStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem editCommentMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteCommentMenuItem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button Closebutton;
        private System.Windows.Forms.ToolStripMenuItem newCommentsToolStripMenuItem;
        private System.Windows.Forms.Button Movebutton;
        private System.Windows.Forms.Button cancelMovebutton;
    }
}