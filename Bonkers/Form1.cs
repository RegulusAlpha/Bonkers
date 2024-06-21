using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Threading;

namespace Bonkers
{
    public partial class Form1 : Form
    {
        private string currentDirectory;
        private bool editingMultipleFiles = false;
        private string selectedImageTextFile;
        private int currentIndex = 0;
        private CancellationTokenSource cancellationTokenSource;
        private string localAPI;
        private string externalAPI;
        private int configFlag = 0;
        private int newWidth, newHeight;
        public Form1()
        {
            InitializeComponent();
            LoadConfig();
            LoadDirectories();
        }
        public class Config
        {
            public string LocalAPI { get; set; }
            public string ExternalAPI { get; set; }
        }
        private void LoadConfig()
        {
            string configPath = "bonkers.cfg";

            if (!File.Exists(configPath))
            {
                Config defaultConfig = new Config
                {
                    LocalAPI = "192.168.2.200",
                    ExternalAPI = ""
                };

                string json = System.Text.Json.JsonSerializer.Serialize(defaultConfig);
                File.WriteAllText(configPath, json);
            }

            string configContent = File.ReadAllText(configPath);
            Config config = System.Text.Json.JsonSerializer.Deserialize<Config>(configContent);

            localAPI = config.LocalAPI;
            externalAPI = config.ExternalAPI;

            // Use localAPI and externalAPI as needed
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
            e.Node.EnsureVisible();
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

        private async void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            e.Node.EnsureVisible();
            // Cancel the previous task and clear the lists
            CancelTaskAndClearLists();
            await Task.Delay(1000);
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

            // Show the progress bar and set its maximum value
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Maximum = imageFiles.Length;
            toolStripProgressBar1.Value = 0;

            // Create a CancellationTokenSource for canceling the task
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                foreach (string file in imageFiles)
                {
                    // Check if cancellation is requested
                    if (cancellationTokenSource.Token.IsCancellationRequested)
                        break;

                    ListViewItem item = new ListViewItem(new FileInfo(file).Name);
                    item.ImageIndex = imageList1.Images.Count;

                    // Load image asynchronously and resize it
                    Bitmap resizedImage = await Task.Run(() =>
                    {
                        using (Image originalImage = Image.FromFile(file))
                        {
                            int originalWidth = originalImage.Width;
                            int originalHeight = originalImage.Height;
                            float ratio = Math.Min((float)255 / originalWidth, (float)255 / originalHeight);

                            int newWidth = (int)(originalWidth * ratio);
                            int newHeight = (int)(originalHeight * ratio);

                            Bitmap resized = new Bitmap(newWidth, newHeight);
                            using (Graphics g = Graphics.FromImage(resized))
                            {
                                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                            }

                            return resized;
                        }
                    }, cancellationTokenSource.Token);

                    imageList1.Images.Add(resizedImage);
                    listView1.Items.Add(item);

                    // Update the progress bar value
                    toolStripProgressBar1.Value++;
                }
            }
            catch (OperationCanceledException)
            {
                // Task was canceled
                MessageBox.Show("Task canceled.");
            }
            finally
            {
                // Hide the progress bar after the task is done or canceled
                toolStripProgressBar1.Visible = false;
            }
        }
        
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                CancelTaskAndClearLists();
                treeView1.SelectedNode = e.Node;
                contextMenuStrip1.Show(treeView1, e.Location);
            }
        }

        private async void generateTxtFilesToolStripMenuItem_Click(object sender, EventArgs e)
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

                await Task.Run(() =>
                {
                    foreach (string file in imageFiles)
                    {
                        string txtFileName = Path.Combine(selectedPath, Path.GetFileNameWithoutExtension(file) + ".txt");

                        if (!File.Exists(txtFileName))
                        {
                            File.WriteAllText(txtFileName, ""); // Create an empty .txt file
                        }
                    }
                });

                MessageBox.Show("Text files generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
            //if (listView1.SelectedItems.Count > 0)
            //{
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
            //}
        }

        private void editAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (listView1.SelectedItems.Count > 0)
            //{
            string selectedPath = treeView1.SelectedNode.Tag.ToString();
            string[] txtFiles = Directory.GetFiles(selectedPath, "*.txt");

            foreach (string txtFile in txtFiles)
            {
                File.WriteAllText(txtFile, ""); // Clear the content of the text file
            }

            MessageBox.Show("Text files cleared successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
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
                // Save the current cursor position
                int currentCursorPosition = richTextBox1.SelectionStart;

                // Save the content of the RichTextBox (assuming SaveRichTextBoxContent is a method that does this)
                SaveRichTextBoxContent();

                // Select all text and change its color
                richTextBox1.SelectAll();
                richTextBox1.SelectionColor = Color.Green;
                richTextBox1.DeselectAll();

                // Restore the cursor position
                richTextBox1.SelectionStart = currentCursorPosition;
                richTextBox1.SelectionLength = 0; // Ensure nothing is selected
                richTextBox1.ScrollToCaret(); // Scroll to the caret position
            }
            else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down ||
                     e.KeyCode == Keys.Enter || e.KeyCode == Keys.Back || e.KeyCode == Keys.Shift || e.KeyCode == Keys.Space)
            {
                // Handle other keys if necessary
            }
            else
            {
                // Get the current selection
                int selectionStart = richTextBox1.SelectionStart;
                int selectionLength = richTextBox1.SelectionLength;

                // Delete the selected text
                if (selectionLength > 0)
                {
                    richTextBox1.Text = richTextBox1.Text.Remove(selectionStart, selectionLength);
                    richTextBox1.SelectionStart = selectionStart;
                }

                // Change the color of the text
                int currentCursorPosition = richTextBox1.SelectionStart;
                richTextBox1.SelectAll();
                richTextBox1.SelectionColor = Color.Red;
                richTextBox1.DeselectAll();
                richTextBox1.SelectionStart = currentCursorPosition;
                richTextBox1.SelectionLength = 0; // Ensure nothing is selected
                richTextBox1.ScrollToCaret(); // Scroll to the caret position
            }
        }
        private void SaveRichTextBoxContent()
        {
            if (configFlag == 0) { 

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
            else
            {
                saveConfig();
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

        private void appendAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (listView1.SelectedItems.Count > 0)
            // {
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
            //}
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

        private async void deepboruToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel5.Text = "";
            if (File.Exists(toolStripStatusLabel1.Text))
            {
                string filePath = toolStripStatusLabel1.Text;

                // Load image from file path
                using (Image image = Image.FromFile(filePath))
                {
                    //toolStripStatusLabel5.Text = "working";
                    string base64String = ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Png);
                    //richTextBox1.Text = base64String.ToString();

                    // Send the API request
                    await SendApiRequest(base64String);
                }
            }
            else
            {
                toolStripStatusLabel5.Text = "Invalid file path";
            }
        }

        private string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                return Convert.ToBase64String(imageBytes);
            }
        }

        private async Task SendApiRequest(string base64Image)
        {
            using (var client = new HttpClient())
            {
                string ipAdd = localAPI;
                var request = new HttpRequestMessage(HttpMethod.Post, "http://" + ipAdd + ":7860/sdapi/v1/interrogate");

                var content = new StringContent($"{{\n    \"model\": \"deepdanbooru\",\n    \"image\": \"{base64Image}\"\n}}", Encoding.UTF8, "application/json");
                request.Content = content;

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);

                // Parse the JSON response and extract the caption
                var jsonDocument = JsonDocument.Parse(responseContent);
                string caption = jsonDocument.RootElement.GetProperty("caption").GetString();

                // Update richTextBox1 with the caption
                richTextBox1.Text = caption;

                // Update status label with success message
                // toolStripStatusLabel4.Text = "Request successful";
            }
        }

        private async Task SendApiRequestNormal(string base64Image)
        {
            using (var client = new HttpClient())
            {
                string ipAdd = localAPI;
                var request = new HttpRequestMessage(HttpMethod.Post, "http://" + ipAdd + ":7860/sdapi/v1/interrogate");

                var content = new StringContent($"{{\n    \"image\": \"{base64Image}\"\n}}", Encoding.UTF8, "application/json");
                request.Content = content;

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);

                // Parse the JSON response and extract the caption
                var jsonDocument = JsonDocument.Parse(responseContent);
                string caption = jsonDocument.RootElement.GetProperty("caption").GetString();

                // Update richTextBox1 with the caption
                richTextBox1.Text = caption;

                // Update status label with success message
                //toolStripStatusLabel4.Text = "Request successful";
            }
        }

        private async void blipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel5.Text = "";
            if (File.Exists(toolStripStatusLabel1.Text))
            {
                string filePath = toolStripStatusLabel1.Text;

                // Load image from file path
                using (Image image = Image.FromFile(filePath))
                {
                    //toolStripStatusLabel5.Text = "working";
                    string base64String = ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Png);
                    //richTextBox1.Text = base64String.ToString();

                    // Send the API request
                    await SendApiRequestNormal(base64String);
                }
            }
            else
            {
                toolStripStatusLabel5.Text = "Invalid file path";
            }
        }

        private void deselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.SelectedItems.Clear();
            listView1.Focus();
            toolStripStatusLabel1.Text = "";
            toolStripStatusLabel2.Text = "";
            toolStripStatusLabel3.Text = "";
            toolStripStatusLabel4.Text = "";
            toolStripStatusLabel5.Text = "";
        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {
            // Cancel the task if it's running
            if (cancellationTokenSource != null && !cancellationTokenSource.Token.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
                //MessageBox.Show("Task cancellation requested.");
            }
        }

        private async void CancelTaskAndClearLists()
        {
            // Cancel the task if it's running
            if (cancellationTokenSource != null && !cancellationTokenSource.Token.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
                //MessageBox.Show("Task canceled.");
            }

            // Clear imageList1 and listView1
            imageList1.Images.Clear();
            listView1.Items.Clear();
            listView1.Clear();
            listView1.Update();
            toolStripProgressBar1.Visible = false;
            toolStripProgressBar1.Value = 0;
            await Task.Delay(2000);
        }


        private void openConfigToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string configPath = "bonkers.cfg";

            if (File.Exists(configPath))
            {
                string configContent = File.ReadAllText(configPath);
                richTextBox1.Text = configContent;
                configFlag = 1;
            }
            else
            {
                MessageBox.Show("Config file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveConfig()
        {
            string configPath = "bonkers.cfg";
            if (File.Exists(configPath))
            {
                string textContent = richTextBox1.Text;
                File.WriteAllText(configPath, textContent);
            }
            configFlag = 0;
            LoadConfig();

        }

        private void reloadConfigToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LoadConfig();
        }
    }

}
