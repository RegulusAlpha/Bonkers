using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Bonkers
{
    public partial class Form1 : Form
    {
        private string currentDirectory;
        private bool editingMultipleFiles = false;
        private string selectedImageTextFile;
        private int currentIndex = 0;
        public Form1()
        {
            InitializeComponent();
            LoadDirectories();
        }
        private void LoadDirectories()
        {
            // Clear existing nodes
            treeView1.Nodes.Clear();

            // Get all drives on the system
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in allDrives)
            {
                // Create the root node for each drive
                TreeNode driveNode = new TreeNode(drive.Name);
                driveNode.Tag = drive.RootDirectory.FullName;

                // Add a placeholder node to indicate that the drive can be expanded
                if (drive.IsReady)
                {
                    driveNode.Nodes.Add("Loading...");
                }

                // Add the drive node to the tree view
                treeView1.Nodes.Add(driveNode);
            }

            // Attach event handlers
            treeView1.BeforeExpand += treeView1_BeforeExpand;
            treeView1.AfterSelect += treeView1_AfterSelect;
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                // Remove the placeholder node
                if (e.Node.Nodes[0].Text == "Loading...")
                {
                    e.Node.Nodes.Clear();

                    // Load directories for the expanded node
                    LoadDirectories(e.Node);
                }
            }
            catch { }
        }

        private void LoadDirectories(TreeNode node)
        {
            string path = (string)node.Tag;

            try
            {
                string[] directories = Directory.GetDirectories(path);
                foreach (string directory in directories)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(directory);
                    TreeNode dirNode = new TreeNode(dirInfo.Name);
                    dirNode.Tag = dirInfo.FullName;

                    // Add a placeholder node to indicate that the directory can be expanded
                    dirNode.Nodes.Add("Loading...");
                    node.Nodes.Add(dirNode);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Handle access exceptions if needed
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Display images in the selected directory
            string selectedPath = e.Node.Tag.ToString();
            string[] imageFiles = Directory.GetFiles(selectedPath, "*.*")
                .Where(s => s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                            s.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                            s.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                            s.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            listView1.Items.Clear();
            listView1.LargeImageList = imageList1;

            foreach (string file in imageFiles)
            {
                ListViewItem item = new ListViewItem(new FileInfo(file).Name);
                item.ImageIndex = imageList1.Images.Count;

                // Load image into ImageList
                imageList1.Images.Add(System.Drawing.Image.FromFile(file));
                listView1.Items.Add(item);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeView1.SelectedNode = e.Node;
                contextMenuStrip1.Show(treeView1, e.Location);
            }
        }

        private void GenerateTxtFilesItem_Click(object sender, EventArgs e)
        {


        }

        private void generateTxtFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                string selectedPath = treeView1.SelectedNode.Tag.ToString();
                string[] imageFiles = Directory.GetFiles(selectedPath, "*.*")
                    .Where(s => s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                s.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                s.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                                s.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                foreach (string file in imageFiles)
                {
                    string txtFileName = Path.Combine(selectedPath, Path.GetFileNameWithoutExtension(file) + ".txt");

                    if (!File.Exists(txtFileName))
                    {
                        File.WriteAllText(txtFileName, ""); // Create an empty .txt file
                    }
                }

                MessageBox.Show("Text files generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedImage = listView1.SelectedItems[0].Text;
                string imagePath = Path.Combine(treeView1.SelectedNode.Tag.ToString(), selectedImage);
                string txtFilePath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + ".txt");

                if (File.Exists(txtFilePath))
                {
                    string textContent = File.ReadAllText(txtFilePath);
                    richTextBox1.Text = textContent;
                }
                else
                {
                    MessageBox.Show("Text file not found for the selected image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedImage = listView1.SelectedItems[0].Text;
                string imagePath = Path.Combine(treeView1.SelectedNode.Tag.ToString(), selectedImage);
                string txtFilePath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + ".txt");

                if (File.Exists(txtFilePath))
                {
                    string textContent = richTextBox1.Text;
                    File.WriteAllText(txtFilePath, textContent);
                    MessageBox.Show("Text file saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Text file not found for the selected image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedPath = treeView1.SelectedNode.Tag.ToString();
                string[] imageFiles = Directory.GetFiles(selectedPath, "*.txt");

                foreach (string txtFile in imageFiles)
                {
                    //string textContent = File.ReadAllText(txtFile);
                    String textContent = richTextBox1.Text; // Append text from richTextBox1

                    File.WriteAllText(txtFile, textContent);
                }
                richTextBox1.SelectAll();
                richTextBox1.SelectionColor = Color.Green;
                richTextBox1.DeselectAll();
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret(); // Scroll to the caret position (end of text)
                MessageBox.Show("Text files saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void editAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedPath = treeView1.SelectedNode.Tag.ToString();
                string[] txtFiles = Directory.GetFiles(selectedPath, "*.txt");

                foreach (string txtFile in txtFiles)
                {
                    File.WriteAllText(txtFile, ""); // Clear the content of the text file
                }

                MessageBox.Show("Text files cleared successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void listView1_ItemSelectionChanged(Object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // Check if an item is selected
            if (listView1.SelectedItems.Count > 0)
            {
                currentIndex = e.ItemIndex;
                //SaveRichTextBoxContent();
                OpenTextFileOfSelectedPhoto();

            }
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if Ctrl+S is pressed
            if (e.Control && e.KeyCode == Keys.S)
            {
                // Save the content of richTextBox1
                SaveRichTextBoxContent();
                richTextBox1.SelectAll();
                richTextBox1.SelectionColor = Color.Green;
                richTextBox1.DeselectAll();
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret(); // Scroll to the caret position (end of text)
            }
            else if (e.Control)
            {

            }
            else if (e.KeyCode == Keys.Left) { }
            else if (e.KeyCode == Keys.Right) { }
            else if (e.KeyCode == Keys.Up) { }
            else if (e.KeyCode == Keys.Down) { }
            else if (e.KeyCode == Keys.Enter) { }
            else if (e.KeyCode == Keys.Back) { }
            else if (e.KeyCode == Keys.Shift) { }
            else
            {
                richTextBox1.SelectAll();
                richTextBox1.SelectionColor = Color.Red;
                richTextBox1.DeselectAll();
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret(); // Scroll to the caret position (end of text)
            }
        }
        private void SaveRichTextBoxContent()
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedImage = listView1.SelectedItems[0].Text;
                string imagePath = Path.Combine(treeView1.SelectedNode.Tag.ToString(), selectedImage);


                string txtFilePath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + ".txt");

                if (File.Exists(txtFilePath))
                {
                    string textContent = richTextBox1.Text;
                    File.WriteAllText(txtFilePath, textContent);
                }
            }
        }

        private void OpenTextFileOfSelectedPhoto()
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedImage = listView1.SelectedItems[0].Text;
                string imagePath = Path.Combine(treeView1.SelectedNode.Tag.ToString(), selectedImage);
                toolStripStatusLabel1.Text = imagePath.ToString();
                string txtFilePath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + ".txt");

                if (File.Exists(txtFilePath))
                {
                    string textContent = File.ReadAllText(txtFilePath);
                    richTextBox1.Text = textContent;
                    toolStripStatusLabel2.Text = txtFilePath.ToString();
                    toolStripStatusLabel3.Text = "TXT File Exists";
                }
                else
                {
                    toolStripStatusLabel2.Text = "";
                    toolStripStatusLabel3.Text = "No TXT File Loaded";
                    richTextBox1.Clear();
                }
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshTreeView();
        }

        private void RefreshTreeView()
        {
            treeView1.Nodes.Clear();
            LoadDirectories();
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void appendAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedPath = treeView1.SelectedNode.Tag.ToString();
                string[] imageFiles = Directory.GetFiles(selectedPath, "*.txt");

                foreach (string txtFile in imageFiles)
                {
                    string textContent = File.ReadAllText(txtFile);
                    textContent += richTextBox1.Text; // Append text from richTextBox1

                    File.WriteAllText(txtFile, textContent);
                }
                richTextBox1.SelectAll();
                richTextBox1.SelectionColor = Color.Green;
                richTextBox1.DeselectAll();
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret(); // Scroll to the caret position (end of text)
                MessageBox.Show("Text files saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void copyConvertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeView1.SelectedNode;
            if (selectedNode != null)
            {
                string rootDir = @"C:\Your\Root\Directory"; // Update this with your actual root directory
                string selectedFolderPath = Path.Combine(rootDir, selectedNode.FullPath);

                if (Directory.Exists(selectedFolderPath))
                {
                    string destDir = Path.Combine(Path.GetDirectoryName(selectedFolderPath),
                        Path.GetFileNameWithoutExtension(selectedFolderPath) + "-conv");
                    Directory.CreateDirectory(destDir);

                    // Copy files from selectedFolderPath to destDir
                    foreach (string file in Directory.GetFiles(selectedFolderPath))
                    {
                        File.Copy(file, Path.Combine(destDir, Path.GetFileName(file)));
                    }

                    // Convert images to PNG
                    ConvertImagesToPng(selectedFolderPath, destDir);
                    DeleteNonPngFiles(destDir);

                    MessageBox.Show("Copy and conversion completed.");
                }
            }
        }

        private void ConvertImagesToPng(string sourceDir, string destDir)
        {
            string[] imageFiles = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);
            foreach (string imageFile in imageFiles)
            {
                string ext = Path.GetExtension(imageFile).ToLower();
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".gif" || ext == ".bmp" || ext == ".webp")
                {
                    using (Image image = Image.FromFile(imageFile))
                    {
                        string destFile = Path.Combine(destDir, Path.GetFileNameWithoutExtension(imageFile) + ".png");
                        image.Save(destFile, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
        }

        private void DeleteNonPngFiles(string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                if (Path.GetExtension(file).ToLower() != ".png")
                {
                    File.Delete(file);
                }
            }
        }

    }
}
