namespace StableCompanion
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
            copyConvertToolStripMenuItem = new ToolStripMenuItem();
            listView1 = new ListView();
            contextMenuStrip2 = new ContextMenuStrip(components);
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            appendAllToolStripMenuItem = new ToolStripMenuItem();
            saveAllToolStripMenuItem = new ToolStripMenuItem();
            editAllToolStripMenuItem = new ToolStripMenuItem();
            deepboruToolStripMenuItem = new ToolStripMenuItem();
            blipToolStripMenuItem = new ToolStripMenuItem();
            deselectToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripProgressBar1 = new ToolStripProgressBar();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            toolStripStatusLabel2 = new ToolStripStatusLabel();
            toolStripStatusLabel3 = new ToolStripStatusLabel();
            toolStripStatusLabel4 = new ToolStripStatusLabel();
            toolStripStatusLabel5 = new ToolStripStatusLabel();
            menuStrip1 = new MenuStrip();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            openConfigToolStripMenuItem = new ToolStripMenuItem();
            openConfigToolStripMenuItem1 = new ToolStripMenuItem();
            reloadConfigToolStripMenuItem = new ToolStripMenuItem();
            reloadConfigToolStripMenuItem1 = new ToolStripMenuItem();
            pictureBox1 = new PictureBox();
            cogVLMToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            contextMenuStrip2.SuspendLayout();
            statusStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(255, 255);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = SystemColors.ScrollBar;
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
            treeView1.BackColor = SystemColors.ScrollBar;
            treeView1.ContextMenuStrip = contextMenuStrip1;
            treeView1.Dock = DockStyle.Left;
            treeView1.Location = new Point(0, 0);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(156, 506);
            treeView1.TabIndex = 3;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { generateTxtFilesToolStripMenuItem, refreshToolStripMenuItem, copyConvertToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(163, 70);
            // 
            // generateTxtFilesToolStripMenuItem
            // 
            generateTxtFilesToolStripMenuItem.Name = "generateTxtFilesToolStripMenuItem";
            generateTxtFilesToolStripMenuItem.Size = new Size(162, 22);
            generateTxtFilesToolStripMenuItem.Text = "Generate txt files";
            generateTxtFilesToolStripMenuItem.ToolTipText = "This will generate txt files for all the images if they do not already exist";
            generateTxtFilesToolStripMenuItem.Click += generateTxtFilesToolStripMenuItem_Click;
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.Size = new Size(162, 22);
            refreshToolStripMenuItem.Text = "Refresh";
            refreshToolStripMenuItem.ToolTipText = "refreshes the directory tree";
            refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;
            // 
            // copyConvertToolStripMenuItem
            // 
            copyConvertToolStripMenuItem.Name = "copyConvertToolStripMenuItem";
            copyConvertToolStripMenuItem.Size = new Size(162, 22);
            copyConvertToolStripMenuItem.Text = "copy convert";
            copyConvertToolStripMenuItem.Click += copyConvertToolStripMenuItem_Click;
            // 
            // listView1
            // 
            listView1.BackColor = SystemColors.ScrollBar;
            listView1.ContextMenuStrip = contextMenuStrip2;
            listView1.Dock = DockStyle.Fill;
            listView1.Location = new Point(156, 0);
            listView1.Name = "listView1";
            listView1.Size = new Size(1112, 506);
            listView1.TabIndex = 4;
            listView1.TileSize = new Size(255, 255);
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.ItemSelectionChanged += listView1_ItemSelectionChanged;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            listView1.DoubleClick += listView1_DoubleClick;
            // 
            // contextMenuStrip2
            // 
            contextMenuStrip2.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, appendAllToolStripMenuItem, saveAllToolStripMenuItem, editAllToolStripMenuItem, deepboruToolStripMenuItem, blipToolStripMenuItem, deselectToolStripMenuItem, cogVLMToolStripMenuItem });
            contextMenuStrip2.Name = "contextMenuStrip2";
            contextMenuStrip2.Size = new Size(181, 224);
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(180, 22);
            openToolStripMenuItem.Text = "open";
            openToolStripMenuItem.ToolTipText = "will open the text file of the currently selected image";
            openToolStripMenuItem.Visible = false;
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(180, 22);
            saveToolStripMenuItem.Text = "save";
            saveToolStripMenuItem.ToolTipText = "will save the current text file only";
            saveToolStripMenuItem.Visible = false;
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // appendAllToolStripMenuItem
            // 
            appendAllToolStripMenuItem.Name = "appendAllToolStripMenuItem";
            appendAllToolStripMenuItem.Size = new Size(180, 22);
            appendAllToolStripMenuItem.Text = "append all";
            appendAllToolStripMenuItem.ToolTipText = "will append the current text in the editor to all the text files in the directory";
            appendAllToolStripMenuItem.Click += appendAllToolStripMenuItem_Click;
            // 
            // saveAllToolStripMenuItem
            // 
            saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            saveAllToolStripMenuItem.Size = new Size(180, 22);
            saveAllToolStripMenuItem.Text = "overwrite all";
            saveAllToolStripMenuItem.ToolTipText = "will overwrite all textfiles in the directory with the current text in the editor";
            saveAllToolStripMenuItem.Click += saveAllToolStripMenuItem_Click;
            // 
            // editAllToolStripMenuItem
            // 
            editAllToolStripMenuItem.Name = "editAllToolStripMenuItem";
            editAllToolStripMenuItem.Size = new Size(180, 22);
            editAllToolStripMenuItem.Text = "clear all";
            editAllToolStripMenuItem.ToolTipText = "will clear the contents from all the text files in the directory";
            editAllToolStripMenuItem.Click += editAllToolStripMenuItem_Click;
            // 
            // deepboruToolStripMenuItem
            // 
            deepboruToolStripMenuItem.Name = "deepboruToolStripMenuItem";
            deepboruToolStripMenuItem.Size = new Size(180, 22);
            deepboruToolStripMenuItem.Text = "deepboru";
            deepboruToolStripMenuItem.Click += deepboruToolStripMenuItem_Click;
            // 
            // blipToolStripMenuItem
            // 
            blipToolStripMenuItem.Name = "blipToolStripMenuItem";
            blipToolStripMenuItem.Size = new Size(180, 22);
            blipToolStripMenuItem.Text = "blip";
            blipToolStripMenuItem.Click += blipToolStripMenuItem_Click;
            // 
            // deselectToolStripMenuItem
            // 
            deselectToolStripMenuItem.Name = "deselectToolStripMenuItem";
            deselectToolStripMenuItem.Size = new Size(180, 22);
            deselectToolStripMenuItem.Text = "deselect";
            deselectToolStripMenuItem.Click += deselectToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = SystemColors.Control;
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripProgressBar1, toolStripStatusLabel1, toolStripStatusLabel2, toolStripStatusLabel3, toolStripStatusLabel4, toolStripStatusLabel5 });
            statusStrip1.Location = new Point(156, 484);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1112, 22);
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            statusStrip1.ItemClicked += statusStrip1_ItemClicked;
            // 
            // toolStripProgressBar1
            // 
            toolStripProgressBar1.Name = "toolStripProgressBar1";
            toolStripProgressBar1.Size = new Size(100, 16);
            toolStripProgressBar1.ToolTipText = "Click to Cancel Task";
            toolStripProgressBar1.Visible = false;
            toolStripProgressBar1.Click += toolStripProgressBar1_Click;
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
            // toolStripStatusLabel4
            // 
            toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            toolStripStatusLabel4.Size = new Size(0, 17);
            // 
            // toolStripStatusLabel5
            // 
            toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            toolStripStatusLabel5.Size = new Size(0, 17);
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { optionsToolStripMenuItem });
            menuStrip1.Location = new Point(156, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1112, 24);
            menuStrip1.TabIndex = 6;
            menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openConfigToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(61, 20);
            optionsToolStripMenuItem.Text = "Options";
            // 
            // openConfigToolStripMenuItem
            // 
            openConfigToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openConfigToolStripMenuItem1, reloadConfigToolStripMenuItem, reloadConfigToolStripMenuItem1 });
            openConfigToolStripMenuItem.Name = "openConfigToolStripMenuItem";
            openConfigToolStripMenuItem.Size = new Size(110, 22);
            openConfigToolStripMenuItem.Text = "Config";
            // 
            // openConfigToolStripMenuItem1
            // 
            openConfigToolStripMenuItem1.Name = "openConfigToolStripMenuItem1";
            openConfigToolStripMenuItem1.Size = new Size(149, 22);
            openConfigToolStripMenuItem1.Text = "Open Config";
            openConfigToolStripMenuItem1.Click += openConfigToolStripMenuItem1_Click;
            // 
            // reloadConfigToolStripMenuItem
            // 
            reloadConfigToolStripMenuItem.Name = "reloadConfigToolStripMenuItem";
            reloadConfigToolStripMenuItem.Size = new Size(149, 22);
            reloadConfigToolStripMenuItem.Text = "Save Config";
            reloadConfigToolStripMenuItem.Visible = false;
            // 
            // reloadConfigToolStripMenuItem1
            // 
            reloadConfigToolStripMenuItem1.Name = "reloadConfigToolStripMenuItem1";
            reloadConfigToolStripMenuItem1.Size = new Size(149, 22);
            reloadConfigToolStripMenuItem1.Text = "Reload Config";
            reloadConfigToolStripMenuItem1.Click += reloadConfigToolStripMenuItem1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(813, 47);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(420, 420);
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            pictureBox1.Visible = false;
            pictureBox1.MouseDown += PictureBox_MouseDown;
            pictureBox1.MouseMove += PictureBox_MouseMove;
            pictureBox1.MouseUp += PictureBox_MouseUp;
            // 
            // cogVLMToolStripMenuItem
            // 
            cogVLMToolStripMenuItem.Name = "cogVLMToolStripMenuItem";
            cogVLMToolStripMenuItem.Size = new Size(180, 22);
            cogVLMToolStripMenuItem.Text = "CogVLM";
            cogVLMToolStripMenuItem.Click += cogVLMToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1268, 643);
            Controls.Add(pictureBox1);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Controls.Add(listView1);
            Controls.Add(treeView1);
            Controls.Add(richTextBox1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "StableCompanion";
            contextMenuStrip1.ResumeLayout(false);
            contextMenuStrip2.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
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
        private ToolStripMenuItem appendAllToolStripMenuItem;
        private ToolStripMenuItem copyConvertToolStripMenuItem;
        private ToolStripStatusLabel toolStripStatusLabel4;
        private ToolStripMenuItem deepboruToolStripMenuItem;
        private ToolStripStatusLabel toolStripStatusLabel5;
        private ToolStripMenuItem blipToolStripMenuItem;
        private ToolStripMenuItem deselectToolStripMenuItem;
        private ToolStripProgressBar toolStripProgressBar1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem openConfigToolStripMenuItem;
        private ToolStripMenuItem openConfigToolStripMenuItem1;
        private ToolStripMenuItem reloadConfigToolStripMenuItem;
        private ToolStripMenuItem reloadConfigToolStripMenuItem1;
        private PictureBox pictureBox1;
        private ToolStripMenuItem cogVLMToolStripMenuItem;
    }
}
