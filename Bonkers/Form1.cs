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
        private bool isDragging = false;
        private Point dragStartMousePosition;
        private Point dragStartPictureBoxPosition;
        public Form1()
        {
            InitializeComponent();
            LoadConfig();
            LoadDirectories();
           
        }
        public class Config
        {
            // Define a public property named LocalAPI of type string
            public string LocalAPI { get; set; }

            // Define a public property named ExternalAPI of type string
            public string ExternalAPI { get; set; }
        }
        private void LoadConfig()
        {
            // Specify the path to the configuration file
            string configPath = "bonkers.cfg";

            // Check if the configuration file exists
            if (!File.Exists(configPath))
            {
                // Create a default configuration object
                Config defaultConfig = new Config
                {
                    LocalAPI = "192.168.2.200",
                    ExternalAPI = ""
                };

                // Serialize the default configuration object to JSON
                string json = System.Text.Json.JsonSerializer.Serialize(defaultConfig);

                // Write the JSON string to the configuration file
                File.WriteAllText(configPath, json);
            }

            // Read the content of the configuration file
            string configContent = File.ReadAllText(configPath);

            // Deserialize the JSON content into a Config object
            Config config = System.Text.Json.JsonSerializer.Deserialize<Config>(configContent);

            // Assign values from the deserialized configuration object to variables
            localAPI = config.LocalAPI;
            externalAPI = config.ExternalAPI;

            // Use localAPI and externalAPI as needed
        }

        private void LoadDirectories()
        {
            // Clear existing nodes in the TreeView
            treeView1.Nodes.Clear();

            // Get all drives on the system
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            // Iterate through each drive
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

                // Add the drive node to the TreeView
                treeView1.Nodes.Add(driveNode);
            }

            // Attach event handlers for further interactions with the TreeView nodes
            treeView1.BeforeExpand += treeView1_BeforeExpand;
            treeView1.AfterSelect += treeView1_AfterSelect;
        }


        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // Ensure that the expanded node is visible in the TreeView
            e.Node.EnsureVisible();

            try
            {
                // Check if the first child node is a placeholder ("Loading...")
                if (e.Node.Nodes[0].Text == "Loading...")
                {
                    // Remove all child nodes
                    e.Node.Nodes.Clear();

                    // Load directories for the expanded node
                    LoadDirectories(e.Node);
                }
            }
            catch { }
        }


        private void LoadDirectories(TreeNode node)
        {
            // Get the path from the node's tag
            string path = (string)node.Tag;

            try
            {
                // Get all directories in the specified path
                string[] directories = Directory.GetDirectories(path);

                // Iterate through each directory
                foreach (string directory in directories)
                {
                    // Get directory information
                    DirectoryInfo dirInfo = new DirectoryInfo(directory);

                    // Create a new TreeNode for the directory
                    TreeNode dirNode = new TreeNode(dirInfo.Name);
                    dirNode.Tag = dirInfo.FullName;

                    // Add a placeholder node to indicate that the directory can be expanded
                    dirNode.Nodes.Add("Loading...");

                    // Add the directory node to the parent node
                    node.Nodes.Add(dirNode);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Handle unauthorized access exceptions if needed
            }
        }


        private async void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Ensure that the selected node in the TreeView is visible
            e.Node.EnsureVisible();

            // Cancel any previous task and clear associated lists
            CancelTaskAndClearLists();

            // Wait for 1 second before continuing execution
            await Task.Delay(1000);

            // Get the path of the selected node in the TreeView
            string selectedPath = e.Node.Tag.ToString();

            // Get all image files (*.jpg, *.png, *.bmp, *.gif) in the selected directory
            string[] imageFiles = Directory.GetFiles(selectedPath, "*.*")
                .Where(s => s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                            s.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                            s.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                            s.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            // Clear the items in the ListView
            listView1.Items.Clear();

            // Set the LargeImageList property of the ListView to imageList1
            listView1.LargeImageList = imageList1;

            // Show the progress bar and set its maximum value to the number of image files
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Maximum = imageFiles.Length;
            toolStripProgressBar1.Value = 0;

            // Create a CancellationTokenSource for canceling the asynchronous task
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                // Iterate through each image file
                foreach (string file in imageFiles)
                {
                    // Check if cancellation is requested before processing each file
                    if (cancellationTokenSource.Token.IsCancellationRequested)
                        break;

                    // Create a new ListViewItem for the image file
                    ListViewItem item = new ListViewItem(new FileInfo(file).Name);
                    item.ImageIndex = imageList1.Images.Count;

                    // Load the image asynchronously and resize it
                    Bitmap resizedImage = await Task.Run(() =>
                    {
                        // Open the original image file
                        using (Image originalImage = Image.FromFile(file))
                        {
                            // Calculate the new dimensions for resizing
                            int originalWidth = originalImage.Width;
                            int originalHeight = originalImage.Height;
                            float ratio = Math.Min((float)255 / originalWidth, (float)255 / originalHeight);
                            int newWidth = (int)(originalWidth * ratio);
                            int newHeight = (int)(originalHeight * ratio);

                            // Create a new bitmap with the resized dimensions
                            Bitmap resized = new Bitmap(newWidth, newHeight);

                            // Draw the resized image
                            using (Graphics g = Graphics.FromImage(resized))
                            {
                                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                            }

                            return resized;
                        }
                    }, cancellationTokenSource.Token);

                    // Add the resized image to imageList1 and the corresponding ListViewItem to listView1
                    imageList1.Images.Add(resizedImage);
                    listView1.Items.Add(item);

                    // Update the progress bar value
                    toolStripProgressBar1.Value++;
                }
            }
            catch (OperationCanceledException)
            {
                // Display a message if the task was canceled
                MessageBox.Show("Task canceled.");
            }
            finally
            {
                // Hide the progress bar after the task is completed or canceled
                toolStripProgressBar1.Visible = false;
            }
        }


        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Check if the right mouse button is clicked
            if (e.Button == MouseButtons.Right)
            {
                // Call the CancelTaskAndClearLists method to cancel any ongoing task and clear lists
                CancelTaskAndClearLists();

                // Set the selected node in the TreeView to the node that was clicked
                treeView1.SelectedNode = e.Node;

                // Show the context menu strip at the location of the mouse click relative to the TreeView
                contextMenuStrip1.Show(treeView1, e.Location);
            }
        }


        private async void generateTxtFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if a node is selected in the TreeView
            if (treeView1.SelectedNode != null)
            {
                // Get the path of the selected node in the TreeView
                string selectedPath = treeView1.SelectedNode.Tag.ToString();

                // Get all image files (*.jpg, *.png, *.bmp, *.gif) in the selected directory
                string[] imageFiles = Directory.GetFiles(selectedPath, "*.*")
                    .Where(s => s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                s.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                s.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                                s.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                // Perform the file generation asynchronously using Task.Run
                await Task.Run(() =>
                {
                    // Iterate through each image file in the array
                    foreach (string file in imageFiles)
                    {
                        // Create the path for the corresponding text file by changing the extension of the image file to .txt
                        string txtFileName = Path.Combine(selectedPath, Path.GetFileNameWithoutExtension(file) + ".txt");

                        // Check if the text file does not exist
                        if (!File.Exists(txtFileName))
                        {
                            // Create an empty .txt file
                            File.WriteAllText(txtFileName, "");
                        }
                    }
                });

                // Display a message box to inform the user that the text files have been generated successfully
                MessageBox.Show("Text files generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if any items are selected in the ListView
            if (listView1.SelectedItems.Count > 0)
            {
                // Get the name of the selected image
                string selectedImage = listView1.SelectedItems[0].Text;

                // Combine the path of the selected node in the TreeView with the selected image name to get the full image path
                string imagePath = Path.Combine(treeView1.SelectedNode.Tag.ToString(), selectedImage);

                // Create the path for the corresponding text file by changing the extension of the image file to .txt
                string txtFilePath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + ".txt");

                // Check if the text file exists
                if (File.Exists(txtFilePath))
                {
                    // Read the text content from the text file
                    string textContent = File.ReadAllText(txtFilePath);

                    // Set the text content to richTextBox1
                    richTextBox1.Text = textContent;
                }
                else
                {
                    // Display an error message if the text file does not exist
                    MessageBox.Show("Text file not found for the selected image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if any items are selected in the ListView
            if (listView1.SelectedItems.Count > 0)
            {
                // Get the name of the selected image
                string selectedImage = listView1.SelectedItems[0].Text;

                // Combine the path of the selected node in the TreeView with the selected image name to get the full image path
                string imagePath = Path.Combine(treeView1.SelectedNode.Tag.ToString(), selectedImage);

                // Create the path for the corresponding text file by changing the extension of the image file to .txt
                string txtFilePath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + ".txt");

                // Check if the text file exists
                if (File.Exists(txtFilePath))
                {
                    // Get the text content from richTextBox1
                    string textContent = richTextBox1.Text;

                    // Write the text content to the text file, overwriting its current content
                    File.WriteAllText(txtFilePath, textContent);

                    // Display a message box to inform the user that the text file has been saved successfully
                    MessageBox.Show("Text file saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Display an error message if the text file does not exist
                    MessageBox.Show("Text file not found for the selected image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the path of the selected node in the TreeView
            string selectedPath = treeView1.SelectedNode.Tag.ToString();

            // Get all text files (*.txt) in the selected directory
            string[] imageFiles = Directory.GetFiles(selectedPath, "*.txt");

            // Iterate through each text file in the array
            foreach (string txtFile in imageFiles)
            {
                // Get the text content from richTextBox1
                string textContent = richTextBox1.Text; // Append text from richTextBox1

                // Write the text content to the text file, overwriting its current content
                File.WriteAllText(txtFile, textContent);
            }

            // Select all text in richTextBox1
            richTextBox1.SelectAll();
            // Change the color of the selected text to green
            richTextBox1.SelectionColor = Color.Green;
            // Deselect all text
            richTextBox1.DeselectAll();
            // Set the cursor position to the end of the text
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            // Scroll to the caret position (end of text)
            richTextBox1.ScrollToCaret();

            // Display a message box to inform the user that the text files have been saved successfully
            MessageBox.Show("Text files saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void editAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the path of the selected node in the TreeView
            string selectedPath = treeView1.SelectedNode.Tag.ToString();

            // Get all text files (*.txt) in the selected directory
            string[] txtFiles = Directory.GetFiles(selectedPath, "*.txt");

            // Iterate through each text file in the array
            foreach (string txtFile in txtFiles)
            {
                // Clear the content of the text file by writing an empty string to it
                File.WriteAllText(txtFile, "");
            }

            // Display a message box to inform the user that the text files have been cleared successfully
            MessageBox.Show("Text files cleared successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            // Check if configFlag is 0
            if (configFlag == 0)
            {
                // Check if an item is selected in listView1
                if (listView1.SelectedItems.Count > 0)
                {
                    // Get the selected image name from the first selected item in listView1
                    string selectedImage = listView1.SelectedItems[0].Text;

                    // Combine the image path using the selected node in treeView1 and the selected image name
                    string imagePath = Path.Combine(treeView1.SelectedNode.Tag.ToString(), selectedImage);

                    // Create the path for the text file associated with the image
                    string txtFilePath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + ".txt");

                    // Check if the text file already exists
                    if (File.Exists(txtFilePath))
                    {
                        // Get the text content from richTextBox1
                        string textContent = richTextBox1.Text;

                        // Write the text content to the text file
                        File.WriteAllText(txtFilePath, textContent);
                    }
                }
            }
            else
            {
                // Call the saveConfig() method if configFlag is not 0
                saveConfig();
            }
        }

        private void OpenTextFileOfSelectedPhoto()
        {
            // Check if an item is selected in listView1
            if (listView1.SelectedItems.Count > 0)
            {
                // Get the selected image name from the first selected item in listView1
                string selectedImage = listView1.SelectedItems[0].Text;

                // Combine the image path using the selected node in treeView1 and the selected image name
                string imagePath = Path.Combine(treeView1.SelectedNode.Tag.ToString(), selectedImage);

                // Update the text of toolStripStatusLabel1 with the image path
                toolStripStatusLabel1.Text = imagePath.ToString();

                // Create the path for the associated text file with the image
                string txtFilePath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + ".txt");

                // Check if the text file exists
                if (File.Exists(txtFilePath))
                {
                    // Read the text content from the text file
                    string textContent = File.ReadAllText(txtFilePath);

                    // Set the text content to richTextBox1
                    richTextBox1.Text = textContent;

                    // Update toolStripStatusLabel2 with the path of the text file
                    toolStripStatusLabel2.Text = txtFilePath.ToString();

                    // Update toolStripStatusLabel3 to indicate that the text file exists
                    toolStripStatusLabel3.Text = "TXT File Exists";
                }
                else
                {
                    // Clear toolStripStatusLabel2
                    toolStripStatusLabel2.Text = "";

                    // Update toolStripStatusLabel3 to indicate that no text file is loaded
                    toolStripStatusLabel3.Text = "No TXT File Loaded";

                    // Clear the text in richTextBox1
                    richTextBox1.Clear();
                }
            }
        }

        // Define an event handler for the refreshToolStripMenuItem click event
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Call the RefreshTreeView method to refresh the TreeView
            RefreshTreeView();
        }

        // Define the RefreshTreeView method to clear nodes in the TreeView and reload directories
        private void RefreshTreeView()
        {
            // Clear all nodes in the TreeView
            treeView1.Nodes.Clear();

            // Load directories to populate the TreeView
            LoadDirectories();
        }

        // Define an event handler for the appendAllToolStripMenuItem click event
        private void appendAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the selected path from the TreeView's selected node
            string selectedPath = treeView1.SelectedNode.Tag.ToString();

            // Get all text files in the selected directory
            string[] imageFiles = Directory.GetFiles(selectedPath, "*.txt");

            // Iterate through each text file
            foreach (string txtFile in imageFiles)
            {
                // Read the existing text content from the text file
                string textContent = File.ReadAllText(txtFile);

                // Append text from richTextBox1 to the existing text content
                textContent += richTextBox1.Text;

                // Write the updated text content back to the text file
                File.WriteAllText(txtFile, textContent);
            }

            // Select all text in richTextBox1
            richTextBox1.SelectAll();

            // Set the selection color to green in richTextBox1
            richTextBox1.SelectionColor = Color.Green;

            // Deselect all text in richTextBox1
            richTextBox1.DeselectAll();

            // Set the selection start to the end of the text in richTextBox1
            richTextBox1.SelectionStart = richTextBox1.Text.Length;

            // Scroll to the caret position (end of text) in richTextBox1
            richTextBox1.ScrollToCaret();

            // Show a success message box
            MessageBox.Show("Text files saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        // Define an event handler for the copyConvertToolStripMenuItem click event
        private void copyConvertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the selected node from the TreeView
            TreeNode selectedNode = treeView1.SelectedNode;

            // Check if a node is selected
            if (selectedNode != null)
            {
                // Define the root directory
                string rootDir = @"C:\Your\Root\Directory"; // Update this with your actual root directory

                // Combine the selected node's full path with the root directory to get the selected folder path
                string selectedFolderPath = Path.Combine(rootDir, selectedNode.FullPath);

                // Check if the selected folder exists
                if (Directory.Exists(selectedFolderPath))
                {
                    // Create a new directory for converted files
                    string destDir = Path.Combine(Path.GetDirectoryName(selectedFolderPath),
                        Path.GetFileNameWithoutExtension(selectedFolderPath) + "-conv");
                    Directory.CreateDirectory(destDir);

                    // Copy files from the selected folder to the destination directory
                    foreach (string file in Directory.GetFiles(selectedFolderPath))
                    {
                        File.Copy(file, Path.Combine(destDir, Path.GetFileName(file)));
                    }

                    // Convert images to PNG format
                    ConvertImagesToPng(selectedFolderPath, destDir);

                    // Delete non-PNG files from the destination directory
                    DeleteNonPngFiles(destDir);

                    // Show a message box indicating successful copy and conversion
                    MessageBox.Show("Copy and conversion completed.");
                }
            }
        }

        // Define a method to convert images to PNG format
        private void ConvertImagesToPng(string sourceDir, string destDir)
        {
            // Get all files (including subdirectories) from the source directory
            string[] imageFiles = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);

            // Iterate through each image file
            foreach (string imageFile in imageFiles)
            {
                // Get the file extension and convert to lowercase
                string ext = Path.GetExtension(imageFile).ToLower();

                // Check if the file is an image file (JPEG, GIF, BMP, WebP, etc.)
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".gif" || ext == ".bmp" || ext == ".webp")
                {
                    // Load the image file
                    using (Image image = Image.FromFile(imageFile))
                    {
                        // Define the destination file path with a PNG extension
                        string destFile = Path.Combine(destDir, Path.GetFileNameWithoutExtension(imageFile) + ".png");

                        // Save the image in PNG format to the destination directory
                        image.Save(destFile, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
        }


        // Define a method to delete non-PNG files from a specified folder
        private void DeleteNonPngFiles(string folderPath)
        {
            // Get all files in the specified folder
            string[] files = Directory.GetFiles(folderPath);

            // Iterate through each file
            foreach (string file in files)
            {
                // Check if the file extension is not ".png"
                if (Path.GetExtension(file).ToLower() != ".png")
                {
                    // Delete the file
                    File.Delete(file);
                }
            }
        }

        // Define an asynchronous event handler for the deepboruToolStripMenuItem click event
        private async void deepboruToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clear toolStripStatusLabel5 text
            toolStripStatusLabel5.Text = "";

            // Check if the file path in toolStripStatusLabel1 exists
            if (File.Exists(toolStripStatusLabel1.Text))
            {
                // Get the file path
                string filePath = toolStripStatusLabel1.Text;

                // Load the image from the file path
                using (Image image = Image.FromFile(filePath))
                {
                    // Convert the image to base64 string (PNG format)
                    string base64String = ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Png);

                    // Send the API request asynchronously
                    await SendApiRequest(base64String);
                }
            }
            else
            {
                // Update toolStripStatusLabel5 with an error message
                toolStripStatusLabel5.Text = "Invalid file path";
            }
        }

        // Define a method to convert an Image to Base64 string
        private string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            // Use a MemoryStream to temporarily store the image data
            using (MemoryStream ms = new MemoryStream())
            {
                // Save the Image to the MemoryStream in the specified format
                image.Save(ms, format);

                // Convert the image data (byte[]) to a Base64 string
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        // Define an asynchronous task to send an API request with a Base64 image string
        private async Task SendApiRequest(string base64Image)
        {
            // Create a new HttpClient for making HTTP requests
            using (var client = new HttpClient())
            {
                // Get the local API address from the localAPI variable
                string ipAdd = localAPI;

                // Create a new HttpRequestMessage for the API endpoint
                var request = new HttpRequestMessage(HttpMethod.Post, "http://" + ipAdd + ":7860/sdapi/v1/interrogate");

                // Set the request content to JSON format with the Base64 image string
                var content = new StringContent($"{{\n    \"model\": \"deepdanbooru\",\n    \"image\": \"{base64Image}\"\n}}", Encoding.UTF8, "application/json");
                request.Content = content;

                // Send the HTTP request asynchronously
                var response = await client.SendAsync(request);

                // Ensure the response is successful (status code 200-299)
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);

                // Parse the JSON response to extract the caption
                var jsonDocument = JsonDocument.Parse(responseContent);
                string caption = jsonDocument.RootElement.GetProperty("caption").GetString();

                // Update the richTextBox1 with the extracted caption
                richTextBox1.Text = caption;

                // Optionally update a status label with a success message
                // toolStripStatusLabel4.Text = "Request successful";
            }
        }

        // Define an asynchronous task to send an API request with a Base64 image string (without specifying the model)
        private async Task SendApiRequestNormal(string base64Image)
        {
            // Create a new HttpClient for making HTTP requests
            using (var client = new HttpClient())
            {
                // Get the local API address from the localAPI variable
                string ipAdd = localAPI;

                // Create a new HttpRequestMessage for the API endpoint without specifying the model
                var request = new HttpRequestMessage(HttpMethod.Post, "http://" + ipAdd + ":7860/sdapi/v1/interrogate");

                // Set the request content to JSON format with the Base64 image string
                var content = new StringContent($"{{\n    \"image\": \"{base64Image}\"\n}}", Encoding.UTF8, "application/json");
                request.Content = content;

                // Send the HTTP request asynchronously
                var response = await client.SendAsync(request);

                // Ensure the response is successful (status code 200-299)
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);

                // Parse the JSON response to extract the caption
                var jsonDocument = JsonDocument.Parse(responseContent);
                string caption = jsonDocument.RootElement.GetProperty("caption").GetString();

                // Update the richTextBox1 with the extracted caption
                richTextBox1.Text = caption;

                // Optionally update a status label with a success message
                //toolStripStatusLabel4.Text = "Request successful";
            }
        }

        // Define an asynchronous event handler for the blipToolStripMenuItem click event
        private async void blipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clear toolStripStatusLabel5 text
            toolStripStatusLabel5.Text = "";

            // Check if the file path in toolStripStatusLabel1 exists
            if (File.Exists(toolStripStatusLabel1.Text))
            {
                // Get the file path
                string filePath = toolStripStatusLabel1.Text;

                // Load the image from the file path
                using (Image image = Image.FromFile(filePath))
                {
                    // Convert the image to base64 string (PNG format)
                    string base64String = ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Png);

                    // Send the API request (without specifying the model)
                    await SendApiRequestNormal(base64String);
                }
            }
            else
            {
                // Update toolStripStatusLabel5 with an error message
                toolStripStatusLabel5.Text = "Invalid file path";
            }
        }


        // Define an event handler for the deselectToolStripMenuItem click event
        private void deselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clear the selected items in listView1
            listView1.SelectedItems.Clear();

            // Set focus to listView1
            listView1.Focus();

            // Clear status labels
            toolStripStatusLabel1.Text = "";
            toolStripStatusLabel2.Text = "";
            toolStripStatusLabel3.Text = "";
            toolStripStatusLabel4.Text = "";
            toolStripStatusLabel5.Text = "";
        }

        // Define an event handler for the toolStripProgressBar1 click event
        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {
            // Cancel the task if it's running
            if (cancellationTokenSource != null && !cancellationTokenSource.Token.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
                //MessageBox.Show("Task cancellation requested.");
            }
        }

        // Define an asynchronous method to cancel a task and clear lists
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

            // Hide the progress bar and reset its value
            toolStripProgressBar1.Visible = false;
            toolStripProgressBar1.Value = 0;

            // Delay for 2000 milliseconds (2 seconds)
            await Task.Delay(2000);
        }



        // Define an event handler for opening the config file
        private void openConfigToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Define the path to the config file
            string configPath = "bonkers.cfg";

            // Check if the config file exists
            if (File.Exists(configPath))
            {
                // Read the content of the config file and display it in richTextBox1
                string configContent = File.ReadAllText(configPath);
                richTextBox1.Text = configContent;

                // Set the configFlag to 1 to indicate that config is loaded
                configFlag = 1;
            }
            else
            {
                // Display an error message if the config file is not found
                MessageBox.Show("Config file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Define a method for saving the config
        private void saveConfig()
        {
            // Define the path to the config file
            string configPath = "bonkers.cfg";

            // Check if the config file exists
            if (File.Exists(configPath))
            {
                // Get the text content from richTextBox1 and write it to the config file
                string textContent = richTextBox1.Text;
                File.WriteAllText(configPath, textContent);
            }

            // Reset the configFlag to 0 and reload the config
            configFlag = 0;
            LoadConfig();
        }

        // Define an event handler for reloading the config
        private void reloadConfigToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Reload the config
            LoadConfig();
        }


        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            string imagePath = toolStripStatusLabel1.Text; // Assuming toolStripStatusLabel1 contains the image file path

            if (!string.IsNullOrEmpty(imagePath))
            {
                try
                {
                    // Load the original image from the file path
                    Image originalImage = Image.FromFile(imagePath);

                    // Calculate the aspect ratio
                    float aspectRatio = (float)originalImage.Width / (float)originalImage.Height;

                    // Set the maximum width and height for the resized image
                    int maxWidth = 420;
                    int maxHeight = 420;

                    // Calculate the new dimensions while maintaining aspect ratio
                    int newWidth = Math.Min(originalImage.Width, maxWidth);
                    int newHeight = (int)(newWidth / aspectRatio);

                    // Check if the height exceeds maxHeight, then adjust dimensions
                    if (newHeight > maxHeight)
                    {
                        newHeight = maxHeight;
                        newWidth = (int)(newHeight * aspectRatio);
                    }

                    // Create a new Bitmap with the resized dimensions
                    Bitmap resizedImage = new Bitmap(originalImage, newWidth, newHeight);

                    // Set the size of the PictureBox to match the resized image
                    pictureBox1.Size = new Size(newWidth, newHeight);

                    // Calculate the initial location for PictureBox placement
                    int edgeOffset = (int)(0.1 * this.ClientSize.Width); // 10% from the edge
                    int pictureBoxX = (this.ClientSize.Width - pictureBox1.Width) / 2; // Center horizontally initially
                    int pictureBoxY = (this.ClientSize.Height - pictureBox1.Height) / 2; // Center vertically initially

                    // Determine the mouse position relative to the form
                    Point mousePos = this.PointToClient(Control.MousePosition);

                    // Determine if the click was on the left or right side of the form
                    bool clickedFromRight = mousePos.X > this.ClientSize.Width / 2;

                    // Adjust initial PictureBox placement based on click position
                    if (clickedFromRight)
                    {
                        pictureBoxX = edgeOffset; // Place on the left
                    }
                    else
                    {
                        pictureBoxX = this.ClientSize.Width - pictureBox1.Width - edgeOffset; // Place on the right
                    }

                    // Set the location of the PictureBox
                    pictureBox1.Location = new Point(pictureBoxX, pictureBoxY);

                    // Display the resized image in the PictureBox
                    pictureBox1.Image = resizedImage;
                    pictureBox1.BackColor = Color.Transparent;
                    pictureBox1.Visible = true;

                    // Attach event handlers for drag-and-drop
                    pictureBox1.MouseDown += PictureBox_MouseDown;
                    pictureBox1.MouseMove += PictureBox_MouseMove;
                    pictureBox1.MouseUp += PictureBox_MouseUp;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading/resizing image: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Image file path is empty.");
            }
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                pictureBox1.Visible = false;
            }
            else if (e.Button == MouseButtons.Right)
            {
                isDragging = true;
                dragStartMousePosition = this.PointToClient(Control.MousePosition); // Mouse position relative to the form
                dragStartPictureBoxPosition = pictureBox1.Location;
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentMousePosition = this.PointToClient(Control.MousePosition);

                int offsetX = currentMousePosition.X - dragStartMousePosition.X;
                int offsetY = currentMousePosition.Y - dragStartMousePosition.Y;

                Point newLocation = new Point(
                    dragStartPictureBoxPosition.X + offsetX,
                    dragStartPictureBoxPosition.Y + offsetY);

                // Ensure the PictureBox stays within the form's client area
                newLocation.X = Math.Max(0, Math.Min(this.ClientSize.Width - pictureBox1.Width, newLocation.X));
                newLocation.Y = Math.Max(0, Math.Min(this.ClientSize.Height - pictureBox1.Height, newLocation.Y));

                // Only update the location if it has changed
                if (newLocation != pictureBox1.Location)
                {
                    pictureBox1.Location = newLocation;
                }
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                isDragging = false;
            }
        }


        //EXPERIMENTAL

        public class TransparentPictureBox : PictureBox
        {
            public TransparentPictureBox()
            {
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                BackColor = Color.Transparent;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                // Fill the control's background with a transparent color
                using (SolidBrush brush = new SolidBrush(Color.Transparent))
                {
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                }
            }
        }









    }

}
