namespace Bonkers
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            imageList1 = new ImageList(components);
            richTextBox1 = new RichTextBox();
            treeView1 = new TreeView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            generateTxtFilesToolStripMenuItem = new ToolStripMenuItem();
            refreshToolStripMenuItem = new ToolStripMenuItem();
            listView1 = new ListView();
            contextMenuStrip2 = new ContextMenuStrip(components);
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAllToolStripMenuItem = new ToolStripMenuItem();
            editAllToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            toolStripStatusLabel2 = new ToolStripStatusLabel();
            toolStripStatusLabel3 = new ToolStripStatusLabel();
            contextMenuStrip1.SuspendLayout();
            contextMenuStrip2.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(192, 255);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // richTextBox1
            // 
            richTextBox1.Dock = DockStyle.Bottom;
            richTextBox1.Location = new Point(0, 506);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(1268, 137);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            richTextBox1.KeyDown += richTextBox1_KeyDown;
            // 
            // treeView1
            // 
            treeView1.ContextMenuStrip = contextMenuStrip1;
            treeView1.Dock = DockStyle.Left;
            treeView1.Location = new Point(0, 0);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(156, 506);
            treeView1.TabIndex = 3;
            treeView1.AfterSelect += treeView1_AfterSelect_1;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { generateTxtFilesToolStripMenuItem, refreshToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(163, 48);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            // 
            // generateTxtFilesToolStripMenuItem
            // 
            generateTxtFilesToolStripMenuItem.Name = "generateTxtFilesToolStripMenuItem";
            generateTxtFilesToolStripMenuItem.Size = new Size(162, 22);
            generateTxtFilesToolStripMenuItem.Text = "Generate txt files";
            generateTxtFilesToolStripMenuItem.Click += generateTxtFilesToolStripMenuItem_Click;
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.Size = new Size(162, 22);
            refreshToolStripMenuItem.Text = "Refresh";
            refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;
            // 
            // listView1
            // 
            listView1.ContextMenuStrip = contextMenuStrip2;
            listView1.Dock = DockStyle.Fill;
            listView1.Location = new Point(156, 0);
            listView1.Name = "listView1";
            listView1.Size = new Size(1112, 506);
            listView1.TabIndex = 4;
            listView1.TileSize = new Size(255, 255);
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.ItemSelectionChanged += listView1_ItemSelectionChanged;
            // 
            // contextMenuStrip2
            // 
            contextMenuStrip2.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, saveAllToolStripMenuItem, editAllToolStripMenuItem });
            contextMenuStrip2.Name = "contextMenuStrip2";
            contextMenuStrip2.Size = new Size(115, 92);
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(114, 22);
            openToolStripMenuItem.Text = "open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(114, 22);
            saveToolStripMenuItem.Text = "save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAllToolStripMenuItem
            // 
            saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            saveAllToolStripMenuItem.Size = new Size(114, 22);
            saveAllToolStripMenuItem.Text = "save all";
            saveAllToolStripMenuItem.Click += saveAllToolStripMenuItem_Click;
            // 
            // editAllToolStripMenuItem
            // 
            editAllToolStripMenuItem.Name = "editAllToolStripMenuItem";
            editAllToolStripMenuItem.Size = new Size(114, 22);
            editAllToolStripMenuItem.Text = "clear all";
            editAllToolStripMenuItem.Click += editAllToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, toolStripStatusLabel2, toolStripStatusLabel3 });
            statusStrip1.Location = new Point(156, 484);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1112, 22);
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            statusStrip1.ItemClicked += statusStrip1_ItemClicked;
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new Size(0, 17);
            // 
            // toolStripStatusLabel3
            // 
            toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            toolStripStatusLabel3.Size = new Size(0, 17);
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1268, 643);
            Controls.Add(statusStrip1);
            Controls.Add(listView1);
            Controls.Add(treeView1);
            Controls.Add(richTextBox1);
            Name = "Form1";
            Text = "Bonkers";
            contextMenuStrip1.ResumeLayout(false);
            contextMenuStrip2.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void RichTextBox1_KeyDown1(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private ImageList imageList1;
        private RichTextBox richTextBox1;
        private TreeView treeView1;
        private ListView listView1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem generateTxtFilesToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAllToolStripMenuItem;
        private ToolStripMenuItem editAllToolStripMenuItem;
        private ToolStripMenuItem refreshToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripStatusLabel toolStripStatusLabel3;
    }
}
