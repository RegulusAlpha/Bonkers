﻿namespace Bonkers
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
            contextMenuStrip3 = new ContextMenuStrip(components);
            cutToolStripMenuItem = new ToolStripMenuItem();
            copyToolStripMenuItem = new ToolStripMenuItem();
            pasteToolStripMenuItem = new ToolStripMenuItem();
            selectAllToolStripMenuItem = new ToolStripMenuItem();
            clearToolStripMenuItem = new ToolStripMenuItem();
            treeView1 = new TreeView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            generateTxtFilesToolStripMenuItem = new ToolStripMenuItem();
            refreshToolStripMenuItem = new ToolStripMenuItem();
            copyConvertToolStripMenuItem = new ToolStripMenuItem();
            addBookmarkToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip2 = new ContextMenuStrip(components);
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            appendAllToolStripMenuItem = new ToolStripMenuItem();
            appendAllFrontToolStripMenuItem = new ToolStripMenuItem();
            saveAllToolStripMenuItem = new ToolStripMenuItem();
            editAllToolStripMenuItem = new ToolStripMenuItem();
            deepboruToolStripMenuItem = new ToolStripMenuItem();
            blipToolStripMenuItem = new ToolStripMenuItem();
            deselectToolStripMenuItem = new ToolStripMenuItem();
            cogVLMToolStripMenuItem = new ToolStripMenuItem();
            ollamaAPIToolStripMenuItem = new ToolStripMenuItem();
            testToolStripMenuItem = new ToolStripMenuItem();
            metadataToolStripMenuItem = new ToolStripMenuItem();
            ollamaAPIBulkToolStripMenuItem = new ToolStripMenuItem();
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
            consoleModeToolStripMenuItem = new ToolStripMenuItem();
            tabControl1 = new TabControl();
            contextMenuStrip4 = new ContextMenuStrip(components);
            newTabToolStripMenuItem = new ToolStripMenuItem();
            closeTabToolStripMenuItem = new ToolStripMenuItem();
            tabControl2 = new TabControl();
            contextMenuStrip5 = new ContextMenuStrip(components);
            newTabToolStripMenuItem1 = new ToolStripMenuItem();
            pictureBox1 = new PictureBox();
            contextMenuStrip6 = new ContextMenuStrip(components);
            metadataToolStripMenuItem1 = new ToolStripMenuItem();
            closeTabToolStripMenuItem1 = new ToolStripMenuItem();
            contextMenuStrip3.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            contextMenuStrip2.SuspendLayout();
            statusStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            contextMenuStrip4.SuspendLayout();
            contextMenuStrip5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            contextMenuStrip6.SuspendLayout();
            SuspendLayout();
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(255, 255);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // contextMenuStrip3
            // 
            contextMenuStrip3.Items.AddRange(new ToolStripItem[] { cutToolStripMenuItem, copyToolStripMenuItem, pasteToolStripMenuItem, selectAllToolStripMenuItem, clearToolStripMenuItem });
            contextMenuStrip3.Name = "contextMenuStrip3";
            contextMenuStrip3.Size = new Size(123, 114);
            contextMenuStrip3.Opening += contextMenuStrip3_Opening;
            // 
            // cutToolStripMenuItem
            // 
            cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            cutToolStripMenuItem.Size = new Size(122, 22);
            cutToolStripMenuItem.Text = "Cut";
            cutToolStripMenuItem.Click += cutToolStripMenuItem_Click;
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new Size(122, 22);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.Size = new Size(122, 22);
            pasteToolStripMenuItem.Text = "Paste";
            pasteToolStripMenuItem.Click += pasteToolStripMenuItem_Click;
            // 
            // selectAllToolStripMenuItem
            // 
            selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            selectAllToolStripMenuItem.Size = new Size(122, 22);
            selectAllToolStripMenuItem.Text = "Select All";
            selectAllToolStripMenuItem.Click += selectAllToolStripMenuItem_Click;
            // 
            // clearToolStripMenuItem
            // 
            clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            clearToolStripMenuItem.Size = new Size(122, 22);
            clearToolStripMenuItem.Text = "Clear All";
            clearToolStripMenuItem.Click += clearToolStripMenuItem_Click;
            // 
            // treeView1
            // 
            treeView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            treeView1.BackColor = SystemColors.ScrollBar;
            treeView1.ContextMenuStrip = contextMenuStrip1;
            treeView1.Location = new Point(0, 27);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(168, 440);
            treeView1.TabIndex = 3;
            treeView1.MouseDown += treeView1_MouseDown;
            treeView1.MouseWheel += treeView1_MouseWheel;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { generateTxtFilesToolStripMenuItem, refreshToolStripMenuItem, copyConvertToolStripMenuItem, addBookmarkToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(170, 92);
            // 
            // generateTxtFilesToolStripMenuItem
            // 
            generateTxtFilesToolStripMenuItem.Name = "generateTxtFilesToolStripMenuItem";
            generateTxtFilesToolStripMenuItem.Size = new Size(169, 22);
            generateTxtFilesToolStripMenuItem.Text = "Generate TXT Files";
            generateTxtFilesToolStripMenuItem.ToolTipText = "This will generate txt files for all the images if they do not already exist";
            generateTxtFilesToolStripMenuItem.Click += generateTxtFilesToolStripMenuItem_Click;
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.Size = new Size(169, 22);
            refreshToolStripMenuItem.Text = "Refresh";
            refreshToolStripMenuItem.ToolTipText = "refreshes the directory tree";
            refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;
            // 
            // copyConvertToolStripMenuItem
            // 
            copyConvertToolStripMenuItem.Name = "copyConvertToolStripMenuItem";
            copyConvertToolStripMenuItem.Size = new Size(169, 22);
            copyConvertToolStripMenuItem.Text = "Copy Convert";
            copyConvertToolStripMenuItem.Click += copyConvertToolStripMenuItem_Click;
            // 
            // addBookmarkToolStripMenuItem
            // 
            addBookmarkToolStripMenuItem.Name = "addBookmarkToolStripMenuItem";
            addBookmarkToolStripMenuItem.Size = new Size(169, 22);
            addBookmarkToolStripMenuItem.Text = "Add Bookmark";
            addBookmarkToolStripMenuItem.Click += addBookmarkToolStripMenuItem_Click;
            // 
            // contextMenuStrip2
            // 
            contextMenuStrip2.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, appendAllToolStripMenuItem, appendAllFrontToolStripMenuItem, saveAllToolStripMenuItem, editAllToolStripMenuItem, deepboruToolStripMenuItem, blipToolStripMenuItem, deselectToolStripMenuItem, cogVLMToolStripMenuItem, ollamaAPIToolStripMenuItem, testToolStripMenuItem, metadataToolStripMenuItem, ollamaAPIBulkToolStripMenuItem });
            contextMenuStrip2.Name = "contextMenuStrip2";
            contextMenuStrip2.Size = new Size(176, 312);
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(175, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.ToolTipText = "will open the text file of the currently selected image";
            openToolStripMenuItem.Visible = false;
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(175, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.ToolTipText = "will save the current text file only";
            saveToolStripMenuItem.Visible = false;
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // appendAllToolStripMenuItem
            // 
            appendAllToolStripMenuItem.Name = "appendAllToolStripMenuItem";
            appendAllToolStripMenuItem.Size = new Size(175, 22);
            appendAllToolStripMenuItem.Text = "Append All";
            appendAllToolStripMenuItem.ToolTipText = "will append the current text in the editor to all the text files in the directory";
            appendAllToolStripMenuItem.Click += appendAllToolStripMenuItem_Click;
            // 
            // appendAllFrontToolStripMenuItem
            // 
            appendAllFrontToolStripMenuItem.Name = "appendAllFrontToolStripMenuItem";
            appendAllFrontToolStripMenuItem.Size = new Size(175, 22);
            appendAllFrontToolStripMenuItem.Text = "Append All - Front";
            appendAllFrontToolStripMenuItem.Click += appendAllFrontToolStripMenuItem_Click;
            // 
            // saveAllToolStripMenuItem
            // 
            saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            saveAllToolStripMenuItem.Size = new Size(175, 22);
            saveAllToolStripMenuItem.Text = "Overwrite All";
            saveAllToolStripMenuItem.ToolTipText = "will overwrite all textfiles in the directory with the current text in the editor";
            saveAllToolStripMenuItem.Click += saveAllToolStripMenuItem_Click;
            // 
            // editAllToolStripMenuItem
            // 
            editAllToolStripMenuItem.Name = "editAllToolStripMenuItem";
            editAllToolStripMenuItem.Size = new Size(175, 22);
            editAllToolStripMenuItem.Text = "Clear All";
            editAllToolStripMenuItem.ToolTipText = "will clear the contents from all the text files in the directory";
            editAllToolStripMenuItem.Click += editAllToolStripMenuItem_Click;
            // 
            // deepboruToolStripMenuItem
            // 
            deepboruToolStripMenuItem.Name = "deepboruToolStripMenuItem";
            deepboruToolStripMenuItem.Size = new Size(175, 22);
            deepboruToolStripMenuItem.Text = "Deepboru  - API";
            deepboruToolStripMenuItem.Click += deepboruToolStripMenuItem_Click;
            // 
            // blipToolStripMenuItem
            // 
            blipToolStripMenuItem.Name = "blipToolStripMenuItem";
            blipToolStripMenuItem.Size = new Size(175, 22);
            blipToolStripMenuItem.Text = "Blip - API";
            blipToolStripMenuItem.Click += blipToolStripMenuItem_Click;
            // 
            // deselectToolStripMenuItem
            // 
            deselectToolStripMenuItem.Name = "deselectToolStripMenuItem";
            deselectToolStripMenuItem.Size = new Size(175, 22);
            deselectToolStripMenuItem.Text = "Deselect";
            deselectToolStripMenuItem.Click += deselectToolStripMenuItem_Click;
            // 
            // cogVLMToolStripMenuItem
            // 
            cogVLMToolStripMenuItem.Name = "cogVLMToolStripMenuItem";
            cogVLMToolStripMenuItem.Size = new Size(175, 22);
            cogVLMToolStripMenuItem.Text = "CogVLM - API";
            cogVLMToolStripMenuItem.Click += cogVLMToolStripMenuItem_Click;
            // 
            // ollamaAPIToolStripMenuItem
            // 
            ollamaAPIToolStripMenuItem.Name = "ollamaAPIToolStripMenuItem";
            ollamaAPIToolStripMenuItem.Size = new Size(175, 22);
            ollamaAPIToolStripMenuItem.Text = "Ollama - API";
            ollamaAPIToolStripMenuItem.Click += ollamaAPIToolStripMenuItem_Click;
            // 
            // testToolStripMenuItem
            // 
            testToolStripMenuItem.Name = "testToolStripMenuItem";
            testToolStripMenuItem.Size = new Size(175, 22);
            testToolStripMenuItem.Text = "test";
            testToolStripMenuItem.Click += testToolStripMenuItem_Click;
            // 
            // metadataToolStripMenuItem
            // 
            metadataToolStripMenuItem.Name = "metadataToolStripMenuItem";
            metadataToolStripMenuItem.Size = new Size(175, 22);
            metadataToolStripMenuItem.Text = "Metadata";
            metadataToolStripMenuItem.Click += metadataToolStripMenuItem_Click;
            // 
            // ollamaAPIBulkToolStripMenuItem
            // 
            ollamaAPIBulkToolStripMenuItem.Name = "ollamaAPIBulkToolStripMenuItem";
            ollamaAPIBulkToolStripMenuItem.Size = new Size(175, 22);
            ollamaAPIBulkToolStripMenuItem.Text = "Ollama - API - Bulk";
            ollamaAPIBulkToolStripMenuItem.Click += ollamaAPIBulkToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = SystemColors.Control;
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripProgressBar1, toolStripStatusLabel1, toolStripStatusLabel2, toolStripStatusLabel3, toolStripStatusLabel4, toolStripStatusLabel5 });
            statusStrip1.Location = new Point(0, 621);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1268, 22);
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            toolStripProgressBar1.Name = "toolStripProgressBar1";
            toolStripProgressBar1.Size = new Size(100, 16);
            toolStripProgressBar1.ToolTipText = "Click me to STOP task";
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
            menuStrip1.Items.AddRange(new ToolStripItem[] { optionsToolStripMenuItem, consoleModeToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1268, 24);
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
            reloadConfigToolStripMenuItem.Click += reloadConfigToolStripMenuItem_Click;
            // 
            // reloadConfigToolStripMenuItem1
            // 
            reloadConfigToolStripMenuItem1.Name = "reloadConfigToolStripMenuItem1";
            reloadConfigToolStripMenuItem1.Size = new Size(149, 22);
            reloadConfigToolStripMenuItem1.Text = "Reload Config";
            reloadConfigToolStripMenuItem1.Click += reloadConfigToolStripMenuItem1_Click;
            // 
            // consoleModeToolStripMenuItem
            // 
            consoleModeToolStripMenuItem.Name = "consoleModeToolStripMenuItem";
            consoleModeToolStripMenuItem.Size = new Size(96, 20);
            consoleModeToolStripMenuItem.Text = "Console Mode";
            consoleModeToolStripMenuItem.Click += consoleModeToolStripMenuItem_Click;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.ContextMenuStrip = contextMenuStrip4;
            tabControl1.Location = new Point(0, 473);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1268, 148);
            tabControl1.TabIndex = 8;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            tabControl1.MouseDown += tabControl1_MouseDownResize;
            tabControl1.MouseWheel += tabControl1_MouseWheelResize;
            // 
            // contextMenuStrip4
            // 
            contextMenuStrip4.Items.AddRange(new ToolStripItem[] { newTabToolStripMenuItem, closeTabToolStripMenuItem });
            contextMenuStrip4.Name = "contextMenuStrip4";
            contextMenuStrip4.Size = new Size(125, 48);
            // 
            // newTabToolStripMenuItem
            // 
            newTabToolStripMenuItem.Name = "newTabToolStripMenuItem";
            newTabToolStripMenuItem.Size = new Size(124, 22);
            newTabToolStripMenuItem.Text = "new tab";
            newTabToolStripMenuItem.Click += newTabToolStripMenuItem_Click;
            // 
            // closeTabToolStripMenuItem
            // 
            closeTabToolStripMenuItem.Name = "closeTabToolStripMenuItem";
            closeTabToolStripMenuItem.Size = new Size(124, 22);
            closeTabToolStripMenuItem.Text = "Close Tab";
            closeTabToolStripMenuItem.Click += closeTabToolStripMenuItem_Click;
            // 
            // tabControl2
            // 
            tabControl2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl2.ContextMenuStrip = contextMenuStrip5;
            tabControl2.Location = new Point(174, 27);
            tabControl2.Name = "tabControl2";
            tabControl2.SelectedIndex = 0;
            tabControl2.Size = new Size(1094, 440);
            tabControl2.TabIndex = 9;
            tabControl2.SelectedIndexChanged += tabControl2_SelectedIndexChanged;
            tabControl2.MouseDown += treeView1_MouseDown;
            tabControl2.MouseWheel += treeView1_MouseWheel;
            // 
            // contextMenuStrip5
            // 
            contextMenuStrip5.Items.AddRange(new ToolStripItem[] { newTabToolStripMenuItem1, closeTabToolStripMenuItem1 });
            contextMenuStrip5.Name = "contextMenuStrip5";
            contextMenuStrip5.Size = new Size(181, 70);
            // 
            // newTabToolStripMenuItem1
            // 
            newTabToolStripMenuItem1.Name = "newTabToolStripMenuItem1";
            newTabToolStripMenuItem1.Size = new Size(180, 22);
            newTabToolStripMenuItem1.Text = "New Tab";
            newTabToolStripMenuItem1.Click += newTabToolStripMenuItem1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 34);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(420, 420);
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            pictureBox1.Visible = false;
            pictureBox1.MouseDown += PictureBox_MouseDown;
            pictureBox1.MouseEnter += PictureBox1_MouseEnter;
            pictureBox1.MouseMove += PictureBox_MouseMove;
            pictureBox1.MouseUp += PictureBox_MouseUp;
            pictureBox1.MouseWheel += PictureBox1_MouseWheel;
            // 
            // contextMenuStrip6
            // 
            contextMenuStrip6.Items.AddRange(new ToolStripItem[] { metadataToolStripMenuItem1 });
            contextMenuStrip6.Name = "contextMenuStrip6";
            contextMenuStrip6.Size = new Size(125, 26);
            // 
            // metadataToolStripMenuItem1
            // 
            metadataToolStripMenuItem1.Name = "metadataToolStripMenuItem1";
            metadataToolStripMenuItem1.Size = new Size(124, 22);
            metadataToolStripMenuItem1.Text = "Metadata";
            metadataToolStripMenuItem1.Click += metadataToolStripMenuItem1_Click;
            // 
            // closeTabToolStripMenuItem1
            // 
            closeTabToolStripMenuItem1.Name = "closeTabToolStripMenuItem1";
            closeTabToolStripMenuItem1.Size = new Size(180, 22);
            closeTabToolStripMenuItem1.Text = "Close Tab";
            closeTabToolStripMenuItem1.Click += closeTabToolStripMenuItem1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1268, 643);
            Controls.Add(pictureBox1);
            Controls.Add(tabControl1);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Controls.Add(treeView1);
            Controls.Add(tabControl2);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Bonkers";
            contextMenuStrip3.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            contextMenuStrip2.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            contextMenuStrip4.ResumeLayout(false);
            contextMenuStrip5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            contextMenuStrip6.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ImageList imageList1;
        private TreeView treeView1;
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
        private ToolStripMenuItem cogVLMToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip3;
        private ToolStripMenuItem clearToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private ToolStripMenuItem ollamaAPIToolStripMenuItem;
        private TabControl tabControl1;
        private ContextMenuStrip contextMenuStrip4;
        private ToolStripMenuItem newTabToolStripMenuItem;
        private ToolStripMenuItem testToolStripMenuItem;
        private ToolStripMenuItem consoleModeToolStripMenuItem;
        private TabControl tabControl2;
        private PictureBox pictureBox1;
        private ContextMenuStrip contextMenuStrip5;
        private ToolStripMenuItem newTabToolStripMenuItem1;
        private ToolStripMenuItem addBookmarkToolStripMenuItem;
        private ToolStripMenuItem metadataToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip6;
        private ToolStripMenuItem metadataToolStripMenuItem1;
        private ToolStripMenuItem appendAllFrontToolStripMenuItem;
        private ToolStripMenuItem ollamaAPIBulkToolStripMenuItem;
        private ToolStripMenuItem closeTabToolStripMenuItem;
        private ToolStripMenuItem closeTabToolStripMenuItem1;
    }
}
