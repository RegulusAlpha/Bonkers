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
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.InteropServices;
using System.Drawing.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Icc;
using MetadataExtractor.Formats.Iptc;
using MetadataExtractor.Formats.Xmp;
using MetadataExtractor.Formats.Jpeg;
using SysDirectory = System.IO.Directory;
using MetaDirectory = MetadataExtractor.Directory;
using SkiaSharp;

#pragma warning disable CS8618
#pragma warning disable CS8600
#pragma warning disable CS8604
#pragma warning disable CS8602
#pragma warning disable CS8603
#pragma warning disable CS8622
#pragma warning disable CS8604
#pragma warning disable CS8601


namespace Bonkers
{
    public partial class Form1 : Form
    {
        //private string currentDirectory;
        //private bool editingMultipleFiles = false;
        //private string selectedImageTextFile;
        private int currentIndex = 0;
        private CancellationTokenSource cancellationTokenSource;
        private string localAPI;
        private string externalAPI;
        private int MaxPboxH;
        private int MaxPboxW;
        private int configFlag = 0;
        private int fontSize;
        private string fontName;
        //private int newWidth, newHeight;
        private bool isDragging = false;
        private Point dragStartMousePosition;
        private Point dragStartPictureBoxPosition;
        private string pathCheck;
        private string defaultPath;
        private bool blip = true;
        private bool deepboru = true;
        private bool CogVLM = true;
        private string CogVLMprompt = "whats in this image? give a description?";
        private bool deselect = false;
        private int CogVLMmax_tokens = 2048;
        private double CogVLMtemperature = 0.8;
        private double CogVLMtop_p = 0.8;
        private int consoleTrack = 0;
        private string hint;
        private string ollamaModel;
        private string ollamaSystem;
        private string OllamaPrompt;
        private string ollamaAddress = "localhost";
        private bool ollamaSave = false;

        //private string apiURL;
        private int tabTag = 1;
        private int currentTextboxTag = 1;
        //private StringWriter consoleOutput;
        private int consoleMode = 0;
        //private int tabControlExpand = 200;
        private int imgTabTag = 1;
        private int currentImageBoxTag = 1;
        //private string[] bookmarks;
        Dictionary<string, ImageList> imageListDictionary = new Dictionary<string, ImageList>();
        private FileSystemWatcher fileSystemWatcher;
        private int originalTreeViewWidth;
        private int originalTabControlWidth;
        private int originalTabControlLeft;
        private int originalTabControl1Height;
        private int originalTabControl1Top;
        private int originalTabControl2Height;
        private int originalTreeView1Height;
        private int minTreeViewWidth = 168;
        private int maxTabControlWidth = 1094;
        public Form1()
        {
            consoleStart();
            InitializeComponent();
            LoadConfig();
            AddNewTab();
            AddNewImageTab();
            LoadDirectories();
            Clipboard.Clear();
            this.Resize += Form1_Resize;
            UpdateDynamicConstants();
            originalTreeViewWidth = treeView1.Width;
            originalTabControlWidth = tabControl2.Width;
            originalTabControlLeft = tabControl2.Left;
            originalTabControl1Height = tabControl1.Height;
            originalTabControl1Top = tabControl1.Top;
            originalTabControl2Height = tabControl2.Height;
            originalTreeView1Height = treeView1.Height;
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine("      ___           ___           ___           ___           ___           ___           ___     \r\n     /  /\\         /  /\\         /  /\\         /  /\\         /  /\\         /  /\\         /  /\\    \r\n    /  /::\\       /  /::\\       /  /::|       /  /:/        /  /::\\       /  /::\\       /  /::\\   \r\n   /  /:/\\:\\     /  /:/\\:\\     /  /:|:|      /  /:/        /  /:/\\:\\     /  /:/\\:\\     /__/:/\\:\\  \r\n  /  /::\\ \\:\\   /  /:/  \\:\\   /  /:/|:|__   /  /::\\____   /  /::\\ \\:\\   /  /::\\ \\:\\   _\\_ \\:\\ \\:\\ \r\n /__/:/\\:\\_\\:| /__/:/ \\__\\:\\ /__/:/ |:| /\\ /__/:/\\:::::\\ /__/:/\\:\\ \\:\\ /__/:/\\:\\_\\:\\ /__/\\ \\:\\ \\:\\\r\n \\  \\:\\ \\:\\/:/ \\  \\:\\ /  /:/ \\__\\/  |:|/:/ \\__\\/~|:|~~~~ \\  \\:\\ \\:\\_\\/ \\__\\/~|::\\/:/ \\  \\:\\ \\:\\_\\/\r\n  \\  \\:\\_\\::/   \\  \\:\\  /:/      |  |:/:/     |  |:|      \\  \\:\\ \\:\\      |  |:|::/   \\  \\:\\_\\:\\  \r\n   \\  \\:\\/:/     \\  \\:\\/:/       |__|::/      |  |:|       \\  \\:\\_\\/      |  |:|\\/     \\  \\:\\/:/  \r\n    \\__\\::/       \\  \\::/        /__/:/       |__|:|        \\  \\:\\        |__|:|~       \\  \\::/   \r\n        ~~         \\__\\/         \\__\\/         \\__\\|         \\__\\/         \\__\\|         \\__\\/    \r\n\r\n");




            Console.ForegroundColor = consoleTrack % 2 == 0 ? ConsoleColor.Green : ConsoleColor.White;

        }
        private void UpdateDynamicConstants()
        {
            // Calculate minimum treeView1 width based on form size
            minTreeViewWidth = Math.Max(treeView1.MinimumSize.Width, this.ClientSize.Width / 10); // Adjust as needed

            // Calculate maximum tabControl2 width based on form size
            maxTabControlWidth = Math.Min(tabControl2.MaximumSize.Width, this.ClientSize.Width - 10); // Adjust as needed

            // Adjust other controls as needed based on form size
        }
        private void consoleStart()
        {

            string asciiArt =
@"      ___           ___           ___           ___           ___           ___           ___     
     /  /\         /  /\         /  /\         /  /\         /  /\         /  /\         /  /\    
    /  /::\       /  /::\       /  /::|       /  /:/        /  /::\       /  /::\       /  /::\   
   /  /:/\:\     /  /:/\:\     /  /:|:|      /  /:/        /  /:/\:\     /  /:/\:\     /__/:/\:\  
  /  /::\ \:\   /  /:/  \:\   /  /:/|:|__   /  /::\____   /  /::\ \:\   /  /::\ \:\   _\_ \:\ \:\ 
 /__/:/\:\_\:| /__/:/ \__\:\ /__/:/ |:| /\ /__/:/\:::::\ /__/:/\:\ \:\ /__/:/\:\_\:\ /__/\ \:\ \:\
 \  \:\ \:\/:/ \  \:\ /  /:/ \__\/  |:|/:/ \__\/~|:|~~~~ \  \:\ \:\_\/ \__\/~|::\/:/ \  \:\ \:\_\/
  \  \:\_\::/   \  \:\  /:/      |  |:/:/     |  |:|      \  \:\ \:\      |  |:|::/   \  \:\_\:\  
   \  \:\/:/     \  \:\/:/       |__|::/      |  |:|       \  \:\_\/      |  |:|\/     \  \:\/:/  
    \__\::/       \  \::/        /__/:/       |__|:|        \  \:\        |__|:|~       \  \::/   
        ~~         \__\/         \__\/         \__\|         \__\/         \__\|         \__\/    
v0.1.6
B: Bitmap
O: Organizer &
N: Notation
K: Knowledge-based
E: Editor 4
R: Recognizing &
S: Sorting
";

            string[] lines = asciiArt.Split('\n');

            foreach (string line in lines)
            {
                foreach (char c in line)
                {
                    // Choose a color based on the character's position
                    ConsoleColor color = GetRainbowColor(line.IndexOf(c));

                    Console.ForegroundColor = color;
                    Console.Write(c);

                    // Adjust delay for effect
                    Thread.Sleep(964 / 1000);
                }
                Console.WriteLine();
            }

            // Reset console color
            Console.ResetColor();

        }
        static ConsoleColor GetRainbowColor(int position)
        {
            // Define an array of rainbow colors
            ConsoleColor[] colors = {
            ConsoleColor.Red,
            ConsoleColor.Yellow,
            ConsoleColor.Green,
            ConsoleColor.Cyan,
            ConsoleColor.Blue,
            ConsoleColor.Magenta
        };
            //int index = position % colors.Length;
            int length = colors.Length;
            Random Random = new Random();
            int index = Random.Next(0, length);
            return colors[index];
        }
        public class Config
        {
            // Define a public property named LocalAPI of type string
            public string LocalAPI { get; set; }

            // Define a public property for all the remaning config items:
            public string ExternalAPI { get; set; }

            public int maxPboxH { get; set; }
            public int maxPboxW { get; set; }
            public int fontSize { get; set; }
            public string fontName { get; set; }
            public string defaultPath { get; set; }
            public string CogVLMprompt { get; set; }
            public bool CogVLM { get; set; }
            public bool blip { get; set; }
            public bool deepboru { get; set; }
            public bool deselect { get; set; }
            public double CogVLMtemperature { get; set; }
            public double CogVLMtop_p { get; set; }
            public int CogVLMmax_tokens { get; set; }
            public string hint { get; set; }
            public string ollamaModel { get; set; }
            public string ollamaSystem { get; set; }
            public string ollamaPrompt { get; set; }
            public string ollamaAddress { get; set; }
            public bool ollamaSave { get; set; }
            public int tabControlExpand { get; set; }
            public string[] Bookmarks { get; set; }
        }

        private void LoadConfig()
        {
            string configPath = "Bonkers.cfg";

            if (!File.Exists(configPath))
            {
                Config defaultConfig = new Config
                {
                    LocalAPI = "192.168.2.200",
                    ExternalAPI = "",
                    maxPboxH = 420,
                    maxPboxW = 420,
                    fontSize = 24,
                    fontName = "Arial",
                    defaultPath = "",
                    CogVLM = true,
                    CogVLMprompt = "whats in this image? give a description?",
                    hint = " current tags: ",
                    blip = true,
                    deepboru = true,
                    deselect = false,
                    CogVLMtemperature = 0.8,
                    CogVLMtop_p = 0.8,
                    CogVLMmax_tokens = 2048,
                    ollamaModel = "llava",
                    ollamaPrompt = "Whats in this photo",
                    ollamaSystem = "The user will send an image, make short descriptive image tags",
                    ollamaAddress = "localhost",
                    ollamaSave = false,
                    tabControlExpand = 200,
                    Bookmarks = new string[] { }
                };
                SaveConfig(defaultConfig);
                //SaveBookmarks(defaultConfig.Bookmarks);
            }

            string configContent = File.ReadAllText(configPath);
            Config config = JsonSerializer.Deserialize<Config>(configContent);

            // Assign values from the deserialized configuration object to variables
            localAPI = config.LocalAPI;
            externalAPI = config.ExternalAPI;
            MaxPboxH = config.maxPboxH;
            MaxPboxW = config.maxPboxW;
            fontSize = config.fontSize;
            fontName = config.fontName;
            defaultPath = config.defaultPath;
            CogVLM = config.CogVLM;
            CogVLMprompt = config.CogVLMprompt;
            blip = config.blip;
            deepboru = config.deepboru;
            deselect = config.deselect;
            CogVLMmax_tokens = config.CogVLMmax_tokens;
            CogVLMtop_p = config.CogVLMtop_p;
            CogVLMtemperature = config.CogVLMtemperature;
            hint = config.hint;
            ollamaSystem = config.ollamaSystem;
            ollamaModel = config.ollamaModel;
            OllamaPrompt = config.ollamaPrompt;
            ollamaAddress = config.ollamaAddress;
            ollamaSave = config.ollamaSave;
            string[] bookmarks = config.Bookmarks;

            // Use localAPI, externalAPI, bookmarks, etc. as needed
            // richTextBox1.Font = new Font(fontName, fontSize, FontStyle.Regular);
            deselectToolStripMenuItem.Visible = deselect;
            blipToolStripMenuItem.Visible = blip;
            deepboruToolStripMenuItem.Visible = deepboru;
            cogVLMToolStripMenuItem.Visible = CogVLM;
        }

        private void LoadDirectories()
        {
            var listView = FindListViewByTag(currentImageBoxTag);
            var imageList = GetImageListByTag(currentImageBoxTag.ToString());
            // Clear existing nodes in the TreeView
            treeView1.Nodes.Clear();
            listView.Items.Clear();
            imageList.Images.Clear();

            // Get all drives on the system
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            // Check if defaultPath is set and exists
            if (defaultPath is not null && SysDirectory.Exists(defaultPath))
            {
                // Create the root node for the defaultPath
                TreeNode defaultNode = new TreeNode(defaultPath)
                {
                    Tag = defaultPath
                };

                // Add the default node to the TreeView
                treeView1.Nodes.Add(defaultNode);

                // Populate the default node with subdirectories and files
                LoadDirectories(defaultNode);
            }
            else
            {
                foreach (DriveInfo drive in allDrives)
                {
                    // Create the root node for each drive
                    TreeNode driveNode = new TreeNode(drive.Name)
                    {
                        Tag = drive.RootDirectory.FullName
                    };

                    // Add a placeholder node to indicate that the drive can be expanded
                    if (drive.IsReady)
                    {
                        driveNode.Nodes.Add("Loading...");
                    }

                    // Add the drive node to the TreeView
                    treeView1.Nodes.Add(driveNode);
                }
            }

            // Load bookmarks into the TreeView
            LoadBookmarks();

            // Attach event handlers for further interactions with the TreeView nodes
            treeView1.BeforeExpand += treeView1_BeforeExpand;
            treeView1.AfterSelect += treeView1_AfterSelect;
        }

        private void LoadBookmarks()
        {
            // Load bookmarks from the configuration file
            string configPath = "Bonkers.cfg";
            string configContent = File.ReadAllText(configPath);
            Config config = JsonSerializer.Deserialize<Config>(configContent);

            if (config.Bookmarks != null && config.Bookmarks.Length > 0)
            {
                TreeNode bookmarksNode = new TreeNode("Bookmarks");
                treeView1.Nodes.Add(bookmarksNode);

                foreach (string bookmark in config.Bookmarks)
                {
                    // Create a node for each bookmark
                    TreeNode bookmarkNode = new TreeNode(bookmark)
                    {
                        Tag = bookmark
                    };

                    // Add a placeholder node to indicate that the bookmark can be expanded
                    bookmarkNode.Nodes.Add("Loading...");

                    // Add the bookmark node under the bookmarks node
                    bookmarksNode.Nodes.Add(bookmarkNode);
                }
            }
        }
        private void LoadDirectories(TreeNode node)
        {
            // Get the path from the node's tag
            string path = (string)node.Tag;

            try
            {
                // Get all directories in the specified path
                string[] directories = SysDirectory.GetDirectories(path);

                // Iterate through each directory
                foreach (string directory in directories)
                {
                    // Get directory information
                    DirectoryInfo dirInfo = new DirectoryInfo(directory);

                    // Create a new TreeNode for the directory
                    TreeNode dirNode = new TreeNode(dirInfo.Name)
                    {
                        Tag = dirInfo.FullName
                    };

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


        private async void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CancelTaskAndClearLists();
            string listBoxTab = currentImageBoxTag.ToString();
            configFlag = 0;

            // Check if the node's tag is null (e.g., after a refresh)
            if (e.Node.Tag == null)
            {
                return;
            }

            // Clear status labels
            toolStripStatusLabel1.Text = "";
            toolStripStatusLabel2.Text = "";
            toolStripStatusLabel3.Text = "";
            toolStripStatusLabel4.Text = "";
            toolStripStatusLabel5.Text = "";

            // Ensure that the selected node in the TreeView is visible
            e.Node.EnsureVisible();

            // Cancel any previous task and clear associated lists


            // Wait for 1 second before continuing execution
            await Task.Delay(1000);

            // Get the path of the selected node in the TreeView
            string selectedPath = e.Node.Tag.ToString();

            // Check if this path has already been processed to prevent duplicate loading
            if (pathCheck == selectedPath)
            {
                LogToConsole("pathCheck prevented loading of " + selectedPath);
                return;
            }

            // Update pathCheck to the current selected path
            pathCheck = selectedPath;

            // Output debug information
            if (consoleTrack % 2 == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("node tag: " + e.Node.Tag.ToString());
                Console.Out.WriteLine("selected path " + selectedPath);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White; // Default color
                Console.Out.WriteLine("node tag: " + e.Node.Tag.ToString());
                Console.Out.WriteLine("selected path " + selectedPath);
            }

            consoleTrack++;

            // Get the tag of the currently selected tab
            //string currentImageBoxTag = tabControl2.SelectedTab.Tag.ToString();

            // Get the ImageList from the dictionary based on currentImageBoxTag
            var imageList = GetImageListByTag(currentImageBoxTag.ToString());
            var listView = FindListViewByTag(currentImageBoxTag);
            if (imageList == null)
            {
                LogToConsole($"ImageList not found for tag: {currentImageBoxTag}");
                return;
            }

            // Get all image files (*.jpg, *.png, *.bmp, *.gif) in the selected directory
            string[] imageFiles = SysDirectory.GetFiles(selectedPath, "*.*")
                .Where(s => s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                            s.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                            s.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                            s.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            // Clear the images in the found ImageList
            imageList.Images.Clear();
            listView.LargeImageList = imageList;
            // Show the progress bar and set its maximum value to the number of image files
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Maximum = imageFiles.Length;
            toolStripProgressBar1.Value = 0;

            // Create a CancellationTokenSource for canceling the asynchronous task
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                int i = 0;
                // Iterate through each image file
                foreach (string file in imageFiles)
                //for (int i = 0; i < imageList.Images.Count && i < imageFiles.Length; i++)

                {
                    // Check if cancellation is requested before processing each file
                    if (cancellationTokenSource.Token.IsCancellationRequested)
                        break;

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

                    // Add the resized image to the ImageList
                    imageList.Images.Add(resizedImage);
                    string filename = Path.GetFileName(file); // Get the filename from the path

                    // Create a new ListViewItem
                    System.Windows.Forms.ListViewItem newItem = new System.Windows.Forms.ListViewItem();

                    // Assign the image from ImageList to the ListViewItem
                    newItem.ImageIndex = i; // Set the index of the image in the ImageList

                    // Set the text of the ListViewItem to the filename
                    newItem.Text = filename;

                    // Add the ListViewItem to the ListView
                    listView.Items.Add(newItem);
                    //UpdateListViewWithImages(imageList);
                    //UpdateListViewWithImages(listBoxTab, imageList, imageFiles);
                    // Update the progress bar value
                    toolStripProgressBar1.Value++;
                    i++;
                }

                // Once all images are added, update the ListView
                int imageCount = imageList.Images.Count;
                //UpdateListViewWithImages(currentImageBoxTag.ToString(), imageList, imageFiles);
                LogToConsole($"Number of images in ImageList: {imageCount}");

                int itemCount = listView.Items.Count;
                LogToConsole($"Number of items in ListView: {itemCount}");
            }
            catch (OperationCanceledException)
            {
                // Display a message if the task was canceled
               LogToConsole("Task canceled.");
            }
            finally
            {
                // Hide the progress bar after the task is completed or canceled
                toolStripProgressBar1.Visible = false;
            }
        }

        //NODE MOUSE CLICK

        private async void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var imageList = GetImageListByTag(currentImageBoxTag.ToString());
            if (imageList == null)
            {
                return;
            }
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
            else
            {


                string listBoxTab = currentImageBoxTag.ToString();
                configFlag = 0;

                // Check if the node's tag is null (e.g., after a refresh)
                if (e.Node.Tag == null)
                {
                    LogToConsole("node tag is null");
                    return;

                }

                // Clear status labels
                toolStripStatusLabel1.Text = "";
                toolStripStatusLabel2.Text = "";
                toolStripStatusLabel3.Text = "";
                toolStripStatusLabel4.Text = "";
                toolStripStatusLabel5.Text = "";

                // Ensure that the selected node in the TreeView is visible
                e.Node.EnsureVisible();

                // Cancel any previous task and clear associated lists
                CancelTaskAndClearLists();

                // Wait for 1 second before continuing execution
                await Task.Delay(1000);

                // Get the path of the selected node in the TreeView
                string selectedPath = e.Node.Tag.ToString();

                // Check if this path has already been processed to prevent duplicate loading
                //if (pathCheck == selectedPath)
                // {
                //     LogToConsole("pathCheck prevented processing");
                //     return;
                // }

                // Update pathCheck to the current selected path
                pathCheck = selectedPath;

                // Output debug information
                if (consoleTrack % 2 == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Out.WriteLine("node tag: " + e.Node.Tag.ToString());
                    Console.Out.WriteLine("selected path " + selectedPath);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White; // Default color
                    Console.Out.WriteLine("node tag: " + e.Node.Tag.ToString());
                    Console.Out.WriteLine("selected path " + selectedPath);
                }

                consoleTrack++;

                // Get the tag of the currently selected tab
                //string currentImageBoxTag = tabControl2.SelectedTab.Tag.ToString();

                // Get the ImageList from the dictionary based on currentImageBoxTag
                //var imageList = GetImageListByTag(currentImageBoxTag.ToString());
                var listView = FindListViewByTag(currentImageBoxTag);
                if (imageList == null)
                {
                    LogToConsole($"ImageList not found for tag: {currentImageBoxTag}");
                    return;
                }

                // Get all image files (*.jpg, *.png, *.bmp, *.gif) in the selected directory
                string[] imageFiles = SysDirectory.GetFiles(selectedPath, "*.*")
                    .Where(s => s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                s.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                s.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                                s.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                // Clear the images in the found ImageList
                imageList.Images.Clear();
                listView.LargeImageList = imageList;
                // Show the progress bar and set its maximum value to the number of image files
                toolStripProgressBar1.Visible = true;
                toolStripProgressBar1.Maximum = imageFiles.Length;
                toolStripProgressBar1.Value = 0;

                // Create a CancellationTokenSource for canceling the asynchronous task
                cancellationTokenSource = new CancellationTokenSource();

                try
                {
                    int i = 0;
                    // Iterate through each image file
                    foreach (string file in imageFiles)
                    //for (int i = 0; i < imageList.Images.Count && i < imageFiles.Length; i++)

                    {
                        // Check if cancellation is requested before processing each file
                        if (cancellationTokenSource.Token.IsCancellationRequested)
                            break;

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

                        // Add the resized image to the ImageList
                        imageList.Images.Add(resizedImage);
                        string filename = Path.GetFileName(file); // Get the filename from the path

                        // Create a new ListViewItem
                        System.Windows.Forms.ListViewItem newItem = new System.Windows.Forms.ListViewItem();

                        // Assign the image from ImageList to the ListViewItem
                        newItem.ImageIndex = i; // Set the index of the image in the ImageList

                        // Set the text of the ListViewItem to the filename
                        newItem.Text = filename;

                        // Add the ListViewItem to the ListView
                        listView.Items.Add(newItem);
                        //UpdateListViewWithImages(imageList);
                        //UpdateListViewWithImages(listBoxTab, imageList, imageFiles);
                        // Update the progress bar value
                        toolStripProgressBar1.Value++;
                        i++;
                    }

                    // Once all images are added, update the ListView
                    int imageCount = imageList.Images.Count;
                    //UpdateListViewWithImages(currentImageBoxTag.ToString(), imageList, imageFiles);
                    LogToConsole($"Number of images in ImageList: {imageCount}");

                    int itemCount = listView.Items.Count;
                    LogToConsole($"Number of items in ListView: {itemCount}");
                }
                catch (OperationCanceledException)
                {
                    // Display a message if the task was canceled
                    LogToConsole("Task canceled.");
                }
                finally
                {
                    // Hide the progress bar after the task is completed or canceled
                    toolStripProgressBar1.Visible = false;
                }
            }
        }


        //END NODE MOUSE CLICK
        private void UpdateListViewWithImages(string currentImageBoxTag, System.Windows.Forms.ImageList imageList, string[] imageFiles)
        {
            // Find the ListView with the matching tag in the selected TabPage
            foreach (Control control in tabControl2.SelectedTab.Controls)
            {
                if (control is System.Windows.Forms.ListView listView && listView.Tag.ToString() == currentImageBoxTag)
                {
                    LogToConsole("UpdateListViewWithImages got passed control");

                    // Clear existing items in the ListView
                    listView.Items.Clear();

                    // Iterate through each image in the ImageList and corresponding filename in imageFiles
                    for (int i = 0; i < imageList.Images.Count && i < imageFiles.Length; i++)
                    {
                        string filename = Path.GetFileName(imageFiles[i]); // Get the filename from the path

                        // Create a new ListViewItem
                        System.Windows.Forms.ListViewItem newItem = new System.Windows.Forms.ListViewItem();

                        // Assign the image from ImageList to the ListViewItem
                        newItem.ImageIndex = i; // Set the index of the image in the ImageList

                        // Set the text of the ListViewItem to the filename
                        newItem.Text = filename;

                        // Add the ListViewItem to the ListView
                        listView.Items.Add(newItem);
                    }

                    // Assign the ImageList to the ListView's LargeImageList
                    listView.LargeImageList = imageList;

                    // Set the view to display large icons
                    listView.View = System.Windows.Forms.View.LargeIcon;

                    // Refresh the ListView to display the images
                    listView.Refresh();

                    // Exit the loop since we've found and updated the ListView
                    break;
                }
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
                string[] imageFiles = SysDirectory.GetFiles(selectedPath, "*.*")
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
                LogToConsole("Text files generated successfully!");
            }
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var listView = FindListViewByTag(currentImageBoxTag);
            // Check if any items are selected in the ListView
            if (listView.SelectedItems.Count > 0)
            {
                // Get the name of the selected image
                string selectedImage = listView.SelectedItems[0].Text;

                // Combine the path of the selected node in the TreeView with the selected image name to get the full image path
                string imagePath = Path.Combine(treeView1.SelectedNode.Tag.ToString(), selectedImage);

                // Create the path for the corresponding text file by changing the extension of the image file to .txt
                string txtFilePath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + ".txt");

                // Find the RichTextBox with the corresponding tag
                RichTextBox selectedRichTextBox = FindRichTextBoxByTag(currentTextboxTag);

                if (selectedRichTextBox != null)
                {
                    // Check if the text file exists
                    if (File.Exists(txtFilePath))
                    {
                        // Read the text content from the text file
                        string textContent = File.ReadAllText(txtFilePath);

                        // Set the text content to the selected RichTextBox
                        AddTextToSelectedRichTextBox(selectedRichTextBox, textContent);

                        // Display a message box to inform the user that the text file has been loaded successfully
                        LogToConsole("Text file loaded successfully!");
                    }
                    else
                    {
                        // Display an error message if the text file does not exist
                        LogToConsole("Text file not found for the selected image.");
                    }
                }
            }
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var listView = FindListViewByTag(currentImageBoxTag);

            // Check if any items are selected in the ListView
            if (listView.SelectedItems.Count > 0)
            {
                // Get the name of the selected image
                string selectedImage = listView.SelectedItems[0].Text;

                // Combine the path of the selected node in the TreeView with the selected image name to get the full image path
                string imagePath = Path.Combine(treeView1.SelectedNode.Tag.ToString(), selectedImage);

                // Create the path for the corresponding text file by changing the extension of the image file to .txt
                string txtFilePath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + ".txt");

                // Find the RichTextBox with the corresponding tag
                RichTextBox selectedRichTextBox = FindRichTextBoxByTag(currentTextboxTag);

                if (selectedRichTextBox != null)
                {
                    // Check if the text file exists
                    if (File.Exists(txtFilePath))
                    {
                        // Get the text content from the selected RichTextBox
                        string textContent = selectedRichTextBox.Text;

                        // Write the text content to the text file, overwriting its current content
                        File.WriteAllText(txtFilePath, textContent);

                        // Display a message box to inform the user that the text file has been saved successfully
                        LogToConsole("Text file saved successfully!");
                    }
                    else
                    {
                        // Display an error message if the text file does not exist
                        LogToConsole("Text file not found for the selected image.");
                    }
                }
            }
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the path of the selected node in the TreeView
            string selectedPath = treeView1.SelectedNode.Tag.ToString();

            // Get all text files (*.txt) in the selected directory
            string[] textFiles = SysDirectory.GetFiles(selectedPath, "*.txt");

            // Find the RichTextBox with the corresponding tag
            RichTextBox selectedRichTextBox = FindRichTextBoxByTag(currentTextboxTag);

            if (selectedRichTextBox != null)
            {
                // Iterate through each text file in the array
                foreach (string txtFile in textFiles)
                {
                    // Read the existing text content from the text file
                    string textContent = File.ReadAllText(txtFile);

                    // Append text from the selected RichTextBox to the existing text content
                    textContent += selectedRichTextBox.Text;

                    // Write the updated text content back to the text file, overwriting its current content
                    File.WriteAllText(txtFile, textContent);
                }

                // Select all text in the selected RichTextBox
                selectedRichTextBox.SelectAll();

                // Change the color of the selected text to green
                selectedRichTextBox.SelectionColor = Color.Green;

                // Deselect all text
                selectedRichTextBox.DeselectAll();

                // Set the cursor position to the end of the text
                selectedRichTextBox.SelectionStart = selectedRichTextBox.Text.Length;

                // Scroll to the caret position (end of text)
                selectedRichTextBox.ScrollToCaret();

                // Display a message box to inform the user that the text files have been saved successfully
                LogToConsole("Text files saved successfully!");
            }
        }

        private void editAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the path of the selected node in the TreeView
            string selectedPath = treeView1.SelectedNode.Tag.ToString();

            // Get all text files (*.txt) in the selected directory
            string[] txtFiles = SysDirectory.GetFiles(selectedPath, "*.txt");

            // Iterate through each text file in the array
            foreach (string txtFile in txtFiles)
            {
                // Clear the content of the text file by writing an empty string to it
                File.WriteAllText(txtFile, "");
            }

            // Display a message box to inform the user that the text files have been cleared successfully
            LogToConsole("Text files cleared successfully!");
        }

        private void richTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                RichTextBox richTextBox = sender as RichTextBox;
                if (richTextBox != null && richTextBox.Focused)
                {
                    richTextBox.Size = new System.Drawing.Size(1268, 300);
                }
            }
        }

        private void richTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            RichTextBox richTextBox = sender as RichTextBox;
            if (richTextBox == null) return;

            // Check if Ctrl+S is pressed to save content
            if (e.Control && e.KeyCode == Keys.S)
            {
                // Save the content of the RichTextBox
                //SaveRichTextBoxContent(richTextBox);
                SaveRichTextBoxContent();
                // Save the current cursor position
                int currentCursorPosition = richTextBox.SelectionStart;

                // Select all text and change its color to green
                richTextBox.SelectAll();
                richTextBox.SelectionColor = Color.Green;
                richTextBox.DeselectAll();

                // Restore the cursor position
                richTextBox.SelectionStart = currentCursorPosition;
                richTextBox.SelectionLength = 0; // Ensure nothing is selected
                richTextBox.ScrollToCaret(); // Scroll to the caret position
            }
            else if (e.Control && e.KeyCode == Keys.T)
            {
                // Handle Ctrl+T (open new tab)
                AddNewTab(); // Prevent default handling if needed
                             // Your logic to open a new tab
            }
            else if (e.Control && e.KeyCode == Keys.W)
            {
                if (tabControl1.TabCount > 1)
                {
                    // Close the currently selected tab
                    tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
                }
                else
                {
                    // Optionally handle the case where only one tab is remaining
                    LogToConsole("Cannot close the last tab.");
                }

            }
            else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down ||
                     e.KeyCode == Keys.Enter || e.KeyCode == Keys.Back || e.KeyCode == Keys.Shift || e.KeyCode == Keys.Space || e.Control)
            {
                // Handle other keys if necessary

                // Example: Allow default behavior for arrow keys, Enter, Backspace, Shift, Space, and Ctrl
                // (No specific action needed here for these keys, so no additional code is added)
            }
            else
            {
                // For any other key press (not specifically handled above)

                // Get the current selection
                int selectionStart = richTextBox.SelectionStart;
                int selectionLength = richTextBox.SelectionLength;

                // Delete the selected text if Ctrl is not pressed
                if (selectionLength > 0 && !e.Control)
                {
                    richTextBox.Text = richTextBox.Text.Remove(selectionStart, selectionLength);
                    richTextBox.SelectionStart = selectionStart;
                }

                // Change the color of the text to red
                int currentCursorPosition = richTextBox.SelectionStart;
                richTextBox.SelectAll();
                richTextBox.SelectionColor = Color.Red;
                richTextBox.DeselectAll();
                richTextBox.SelectionStart = currentCursorPosition;
                richTextBox.SelectionLength = 0; // Ensure nothing is selected
                richTextBox.ScrollToCaret(); // Scroll to the caret position
            }
        }


        private void SaveRichTextBoxContent()
        {
            var listView = FindListViewByTag(currentImageBoxTag);
            var imageList = GetImageListByTag(currentImageBoxTag.ToString());
            // Check if configFlag is 0
            if (configFlag == 0)
            {
                // Check if an item is selected in listView1
                if (listView.SelectedItems.Count > 0)
                {
                    // Get the selected image name from the first selected item in listView1
                    string selectedImage = listView.SelectedItems[0].Text;

                    // Combine the image path using the selected node in treeView1 and the selected image name
                    string imagePath = Path.Combine(treeView1.SelectedNode.Tag.ToString(), selectedImage);

                    // Create the path for the text file associated with the image
                    string txtFilePath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + ".txt");

                    // Get the current selected tab in tabControl1
                    TabPage selectedTab = tabControl1.SelectedTab;

                    // Find the RichTextBox within the selected tab using the currentTextboxTag
                    RichTextBox selectedRichTextBox = FindRichTextBoxByTag(currentTextboxTag);

                    if (selectedRichTextBox != null) // && File.Exists(txtFilePath)
                    {
                        // Get the text content from the selected RichTextBox
                        string textContent = selectedRichTextBox.Text;

                        // Write the text content to the text file
                        File.WriteAllText(txtFilePath, textContent);

                        // Optionally update a status label with a success message
                        // toolStripStatusLabel4.Text = "Text content saved successfully";
                    }
                    else
                    {
                        LogToConsole("RichTextBox not found or text file does not exist.");
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
            var listView = FindListViewByTag(currentImageBoxTag);
            // Check if an item is selected in listView1
            if (listView.SelectedItems.Count > 0)
            {
                // Get the selected image name from the first selected item in listView1
                string selectedImage = listView.SelectedItems[0].Text;

                // Combine the image path using the selected node in treeView1 and the selected image name
                string imagePath = Path.Combine(treeView1.SelectedNode?.Tag?.ToString(), selectedImage); // Added null checks

                // Update the text of toolStripStatusLabel1 with the image path
                toolStripStatusLabel1.Text = imagePath?.ToString(); // Added null check

                // Create the path for the associated text file with the image
                string txtFilePath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + ".txt");

                // Check if tabControl1.SelectedTab is not null before accessing its properties
                if (tabControl1.SelectedTab != null)
                {
                    // Find the RichTextBox with the corresponding tag
                    RichTextBox selectedRichTextBox = FindRichTextBoxByTag(currentTextboxTag);

                    if (selectedRichTextBox != null)
                    {
                        // Check if the text file exists
                        if (File.Exists(txtFilePath))
                        {
                            // Read the text content from the text file
                            string textContent = File.ReadAllText(txtFilePath);

                            // Set the text content to the selected RichTextBox
                            AddTextToSelectedRichTextBox(selectedRichTextBox, textContent);

                            // Update toolStripStatusLabel2 with the path of the text file
                            toolStripStatusLabel2.Text = txtFilePath?.ToString(); // Added null check

                            // Update toolStripStatusLabel3 to indicate that the text file exists
                            toolStripStatusLabel3.Text = "TXT File Exists";
                        }
                        else
                        {
                            // Clear toolStripStatusLabel2
                            toolStripStatusLabel2.Text = "";

                            // Update toolStripStatusLabel3 to indicate that no text file is loaded
                            toolStripStatusLabel3.Text = "No TXT File Loaded";

                            // Clear the text in the selected RichTextBox
                            selectedRichTextBox.Clear();
                        }
                    }
                    else
                    {
                        LogToConsole("No RichTextBox found with the specified tag.");
                    }
                }
                else
                {
                   LogToConsole("No tab selected in tabControl1.");
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
            CancelTaskAndClearLists();
            // Store the state of each node
            var nodeInfos = new Dictionary<string, TreeNodeInfo>();
            StoreNodesState(treeView1.Nodes, nodeInfos);

            // Clear all nodes in the TreeView
            treeView1.Nodes.Clear();

            // Load directories to populate the TreeView
            LoadDirectories();

            // Restore the state of each node
            RestoreNodesState(treeView1.Nodes, nodeInfos);
        }
        private class TreeNodeInfo
        {
            public string Path { get; set; }
            public object Tag { get; set; }
            public bool IsExpanded { get; set; }

            public TreeNodeInfo(string path, object tag, bool isExpanded)
            {
                Path = path;
                Tag = tag;
                IsExpanded = isExpanded;
            }
        }

        // Method to store the state of each node
        private void StoreNodesState(TreeNodeCollection nodes, Dictionary<string, TreeNodeInfo> nodeInfos)
        {
            foreach (TreeNode node in nodes)
            {
                var nodeInfo = new TreeNodeInfo(node.FullPath, node.Tag, node.IsExpanded);
                nodeInfos[node.FullPath] = nodeInfo;

                // Recursive call for child nodes
                StoreNodesState(node.Nodes, nodeInfos);
            }
        }

        // Method to restore the state of each node
        private void RestoreNodesState(TreeNodeCollection nodes, Dictionary<string, TreeNodeInfo> nodeInfos)
        {
            foreach (TreeNode node in nodes)
            {
                if (nodeInfos.TryGetValue(node.FullPath, out var nodeInfo))
                {
                    node.Tag = nodeInfo.Tag;

                    if (nodeInfo.IsExpanded)
                    {
                        node.Expand();
                    }
                }

                // Recursive call for child nodes
                RestoreNodesState(node.Nodes, nodeInfos);
            }
        }

        // Define an event handler for the appendAllToolStripMenuItem click event
        private void appendAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the selected path from the TreeView's selected node
            string selectedPath = treeView1.SelectedNode.Tag.ToString();

            // Get all text files in the selected directory
            string[] textFiles = SysDirectory.GetFiles(selectedPath, "*.txt");

            // Find the RichTextBox with the corresponding tag
            RichTextBox selectedRichTextBox = FindRichTextBoxByTag(currentTextboxTag);

            if (selectedRichTextBox != null)
            {
                // Iterate through each text file
                foreach (string txtFile in textFiles)
                {
                    // Read the existing text content from the text file
                    string textContent = File.ReadAllText(txtFile);

                    // Append text from the selected RichTextBox to the existing text content
                    textContent += selectedRichTextBox.Text;

                    // Write the updated text content back to the text file
                    File.WriteAllText(txtFile, textContent);
                }

                // Select all text in the selected RichTextBox
                selectedRichTextBox.SelectAll();

                // Set the selection color to green in the selected RichTextBox
                selectedRichTextBox.SelectionColor = Color.Green;

                // Deselect all text in the selected RichTextBox
                selectedRichTextBox.DeselectAll();

                // Set the selection start to the end of the text in the selected RichTextBox
                selectedRichTextBox.SelectionStart = selectedRichTextBox.Text.Length;

                // Scroll to the caret position (end of text) in the selected RichTextBox
                selectedRichTextBox.ScrollToCaret();

                // Show a success message box
                LogToConsole("Text files saved successfully!");
            }
        }


        // Define an event handler for the copyConvertToolStripMenuItem click event
        private void copyConvertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the selected node from the TreeView
            TreeNode selectedNode = treeView1.SelectedNode;

            // Check if a node is selected
            if (selectedNode != null)
            {
                // Split the FullPath by the tree view's path separator (usually a backslash \)
                // and remove the first part (like "Bookmarks")
                string[] pathParts = selectedNode.FullPath.Split(treeView1.PathSeparator.ToCharArray());

                // Recombine the path, excluding the first node (e.g., "Bookmarks")
                string selectedFolderPath = string.Join(Path.DirectorySeparatorChar.ToString(), pathParts.Skip(1));

                // Check if the selected folder exists
                if (SysDirectory.Exists(selectedFolderPath))
                {
                    try
                    {
                        // Create a new directory for converted files
                        string destDir = Path.Combine(Path.GetDirectoryName(selectedFolderPath),
                            Path.GetFileNameWithoutExtension(selectedFolderPath) + "-conv");
                        SysDirectory.CreateDirectory(destDir);

                        // Copy files from the selected folder to the destination directory
                        foreach (string file in SysDirectory.GetFiles(selectedFolderPath))
                        {
                            string destFilePath = Path.Combine(destDir, Path.GetFileName(file));
                            File.Copy(file, destFilePath, true);  // 'true' to overwrite existing files
                        }

                        // Convert images to PNG format
                        ConvertImagesToPngNew(selectedFolderPath, destDir);

                        // Delete non-PNG files from the destination directory
                        DeleteNonPngFiles(destDir);

                        // Log success message
                        LogToConsole("Copy and conversion completed.");
                    }
                    catch (Exception ex)
                    {
                        // Log any errors encountered
                        LogToConsole($"Error during copy/convert: {ex.Message}");
                    }
                }
                else
                {
                    // Log if the selected folder does not exist
                    LogToConsole("Selected folder does not exist.");
                }
            }
            else
            {
                // Log if no node is selected
                LogToConsole("No folder selected.");
            }
        }

        // Define a method to convert images to PNG format
        private void ConvertImagesToPng(string sourceDir, string destDir)
        {
            // Get all files (including subdirectories) from the source directory
            string[] imageFiles = SysDirectory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);

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
        private void ConvertImagesToPngNew(string sourceDir, string destDir)
        {
            // Get all files (including subdirectories) from the source directory
            string[] imageFiles = SysDirectory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);

            // Iterate through each image file
            foreach (string imageFile in imageFiles)
            {
                // Get the file extension and convert to lowercase
                string ext = Path.GetExtension(imageFile).ToLower();

                // Check if the file is an image file (JPEG, GIF, BMP, WebP, etc.)
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".gif" || ext == ".bmp" || ext == ".webp")
                {
                    try
                    {
                        // Load the image using SkiaSharp (supports WebP natively)
                        using (var input = File.OpenRead(imageFile))
                        {
                            using (var bitmap = SKBitmap.Decode(input))
                            {
                                // Define the destination file path with a PNG extension
                                string destFile = Path.Combine(destDir, Path.GetFileNameWithoutExtension(imageFile) + ".png");

                                // Convert the image to PNG and save it
                                using (var output = File.OpenWrite(destFile))
                                {
                                    var image = SKImage.FromBitmap(bitmap);
                                    image.Encode(SKEncodedImageFormat.Png, 100).SaveTo(output);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions (e.g., unsupported formats, file access issues)
                        Console.WriteLine($"Error processing {imageFile}: {ex.Message}");
                    }
                }
            }
        }

        // Define a method to delete non-PNG files from a specified folder
        private void DeleteNonPngFiles(string folderPath)
        {
            // Get all files in the specified folder
            string[] files = SysDirectory.GetFiles(folderPath);

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
                try
                {
                    // Get the file path
                    string filePath = toolStripStatusLabel1.Text;

                    // Load the image from the file path
                    using (Image image = Image.FromFile(filePath))
                    {
                        // Convert the image to base64 string (PNG format)
                        string base64String = ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Png);

                        // Get the selected RichTextBox based on the current textbox tag
                        RichTextBox selectedRichTextBox = FindRichTextBoxByTag(currentTextboxTag);

                        if (selectedRichTextBox != null)
                        {
                            // Send the API request asynchronously
                            await SendApiRequest(base64String, selectedRichTextBox);
                        }
                        else
                        {
                            // Handle case where no RichTextBox is found
                            toolStripStatusLabel5.Text = "No RichTextBox found for selected tab";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions related to file loading or API request
                    toolStripStatusLabel5.Text = $"Error: {ex.Message}";
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
        private async Task SendApiRequest(string base64Image, RichTextBox selectedRichTextBox)
        {
            // Create a new HttpClient for making HTTP requests
            using (var client = new HttpClient())
            {
                // Get the local API address from the localAPI variable
                string ipAdd = localAPI; // Ensure localAPI is defined and accessible

                try
                {
                    // Create a new HttpRequestMessage for the API endpoint
                    var request = new HttpRequestMessage(HttpMethod.Post, $"http://{ipAdd}:7860/sdapi/v1/interrogate");

                    // Set the request content to JSON format with the Base64 image string
                    var content = new StringContent($"{{\n    \"model\": \"deepdanbooru\",\n    \"image\": \"{base64Image}\"\n}}", Encoding.UTF8, "application/json");
                    request.Content = content;

                    // Send the HTTP request asynchronously
                    var response = await client.SendAsync(request);

                    // Ensure the response is successful (status code 200-299)
                    response.EnsureSuccessStatusCode();

                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Log the response content
                    LogToConsole(responseContent);

                    // Parse the JSON response to extract the caption
                    var jsonDocument = JsonDocument.Parse(responseContent);
                    string caption = jsonDocument.RootElement.GetProperty("caption").GetString();

                    // Update the selected RichTextBox with the extracted caption
                    UpdateRichTextBox(selectedRichTextBox, caption);
                }
                catch (HttpRequestException ex)
                {
                    // Log HTTP request exceptions
                    LogToConsole($"HTTP request failed: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    // Log JSON parsing exceptions
                    LogToConsole($"JSON parsing failed: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Log any other exceptions
                    LogToConsole($"An error occurred: {ex.Message}");
                }
            }
        }

        // Define an asynchronous task to send an API request with a Base64 image string (without specifying the model)
        private async Task SendApiRequestNormal(string base64Image, RichTextBox selectedRichTextBox)
        {
            // Create a new HttpClient for making HTTP requests
            using (var client = new HttpClient())
            {
                // Get the local API address from the localAPI variable
                string ipAdd = localAPI;
                try
                {
                    // Create a new HttpRequestMessage for the API endpoint without specifying the model
                    var request = new HttpRequestMessage(HttpMethod.Post, $"http://{ipAdd}:7860/sdapi/v1/interrogate");

                    // Set the request content to JSON format with the Base64 image string
                    var content = new StringContent($"{{\n    \"image\": \"{base64Image}\"\n}}", Encoding.UTF8, "application/json");
                    request.Content = content;

                    // Send the HTTP request asynchronously
                    var response = await client.SendAsync(request);

                    // Ensure the response is successful (status code 200-299)
                    response.EnsureSuccessStatusCode();

                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Log the response content
                    LogToConsole(responseContent);

                    // Parse the JSON response to extract the caption
                    var jsonDocument = JsonDocument.Parse(responseContent);
                    string caption = jsonDocument.RootElement.GetProperty("caption").GetString();

                    // Update the selected RichTextBox with the extracted caption
                    UpdateRichTextBox(selectedRichTextBox, caption);
                }
                catch (Exception e)
                {
                    // Log the exception message
                    LogToConsole(e.Message);
                }
            }
        }
        private async Task SendApiRequestChatCompletions(string base64Image, RichTextBox selectedRichTextBox)
        {
            string thinkTags = "";
            string hintTag = hint;
            if (hintTag != "" && selectedRichTextBox.Text != "")
            {
                thinkTags = selectedRichTextBox.Text;
            }
            else
            {
                thinkTags = "";
                hintTag = "";
            }

            // Create a new HttpClient for making HTTP requests
            using (var client = new HttpClient())
            {
                // Get the local API address for chat completions from the localAPIChat variable
                string ipAdd = localAPI;
                try
                {
                    // Config item
                    string nonsense = "you are a AI artwork tagging assistant, you primarily tag images in detail. Tag appearance, tag clothing, tag background, tag expression, tag position, tag pose, tag camera angle:  ";

                    // Create a new HttpRequestMessage for the chat completions API endpoint
                    var request = new HttpRequestMessage(HttpMethod.Post, $"http://{ipAdd}:8000/v1/chat/completions");

                    // Set the request content to JSON format with the specified JSON payload
                    var jsonPayload = $@"
    {{
        ""model"": ""cogview-3"",
        ""messages"": [
            {{
                ""role"": ""user"",
                ""content"": [
                    {{
                        ""type"": ""text"",
                        ""text"": ""{nonsense}{CogVLMprompt}{hintTag}{thinkTags}""
                    }},
                    {{
                        ""type"": ""image_url"",
                        ""image_url"": {{
                            ""url"": ""data:image/jpeg;base64,{base64Image}""
                        }}
                    }}
                ]
            }}
        ],
        ""stream"": false,
        ""max_tokens"": {CogVLMmax_tokens},
        ""temperature"": {CogVLMtemperature},
        ""top_p"": {CogVLMtop_p}
    }}";

                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                    request.Content = content;

                    // Send the HTTP request asynchronously
                    var response = await client.SendAsync(request);

                    // Ensure the response is successful (status code 200-299)
                    response.EnsureSuccessStatusCode();

                    // Read the response content as a stream
                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        // Parse the JSON response using JsonDocument
                        using (var jsonDocument = await JsonDocument.ParseAsync(responseStream))
                        {
                            var root = jsonDocument.RootElement;

                            // Extract the content string
                            var choices = root.GetProperty("choices");
                            if (choices.GetArrayLength() > 0)
                            {
                                var firstChoice = choices[0];
                                var message = firstChoice.GetProperty("message");
                                string chatContent = message.GetProperty("content").GetString();

                                // Update the selected RichTextBox with the extracted content
                                UpdateRichTextBox(selectedRichTextBox, chatContent);


                                LogToConsole(chatContent);

                            }
                        }
                    }
                }
                catch (Exception e)
                {

                    LogToConsole(e.Message);

                }
                // Optionally update a status label with a success message
                // toolStripStatusLabel4.Text = "Request successful";
            }
        }

        // Define an asynchronous event handler for the blipToolStripMenuItem click event
        private async void blipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clear toolStripStatusLabel5 text
            toolStripStatusLabel5.Text = "";

            // Get the selected RichTextBox based on the current textbox tag
            RichTextBox selectedRichTextBox = FindRichTextBoxByTag(currentTextboxTag);

            if (selectedRichTextBox != null)
            {
                // Check if the file path in toolStripStatusLabel1 exists
                string filePath = toolStripStatusLabel1.Text;
                if (File.Exists(filePath))
                {
                    try
                    {
                        // Load the image from the file path
                        using (Image image = Image.FromFile(filePath))
                        {
                            // Convert the image to base64 string (PNG format)
                            string base64String = ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Png);

                            // Send the API request asynchronously
                            await SendApiRequestNormal(base64String, selectedRichTextBox);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions when loading the image
                        toolStripStatusLabel5.Text = $"Error loading image: {ex.Message}";
                    }
                }
                else
                {
                    // Update toolStripStatusLabel5 with an error message
                    toolStripStatusLabel5.Text = "Invalid file path";
                }
            }
            else
            {
                // Handle case where no RichTextBox is found
                toolStripStatusLabel5.Text = "No RichTextBox found for selected tab";
            }
        }


        // Define an event handler for the deselectToolStripMenuItem click event
        private void deselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var listView = FindListViewByTag(currentImageBoxTag);
            // Clear the selected items in listView1
            listView.SelectedItems.Clear();

            // Set focus to listView1
            listView.Focus();

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
            var listView = FindListViewByTag(currentImageBoxTag);
            var imageList = GetImageListByTag(currentImageBoxTag.ToString());
            // Cancel the task if it's running
            if (cancellationTokenSource != null && !cancellationTokenSource.Token.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
                //MessageBox.Show("Task canceled.");
            }

            // Clear imageList1 and listView1

            LogToConsole("Items count before clearing: " + listView.Items.Count);
            LogToConsole("Images count before clearing: " + imageList.Images.Count);



            // Clear items and images

            listView.Items.Clear();
            imageList.Images.Clear();

            // After clearing

            LogToConsole("Items count after clearing: " + listView.Items.Count);
            LogToConsole("Images count after clearing: " + imageList.Images.Count);



            toolStripStatusLabel1.Text = "";
            toolStripStatusLabel2.Text = "";
            toolStripStatusLabel3.Text = "";
            toolStripStatusLabel4.Text = "";
            toolStripStatusLabel5.Text = "";
            //richTextBox1.Text = "";

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
            string configPath = "Bonkers.cfg";
            toolStripStatusLabel1.Text = "";
            toolStripStatusLabel2.Text = "";
            toolStripStatusLabel3.Text = "";
            toolStripStatusLabel4.Text = "";
            toolStripStatusLabel5.Text = "";

            // Get the current RichTextBox based on its tag
            int tabTag = tabControl1.SelectedIndex + 1; // Assuming tabTag starts from 1
            RichTextBox selectedRichTextBox = FindRichTextBoxByTag(tabTag);

            if (selectedRichTextBox != null)
            {
                // Check if the config file exists
                if (File.Exists(configPath))
                {
                    // Read the content of the config file and display it in the selected RichTextBox
                    string configContent = File.ReadAllText(configPath);
                    UpdateRichTextBox(selectedRichTextBox, configContent);

                    // Set the configFlag to 1 to indicate that config is loaded
                    configFlag = 1;
                }
                else
                {
                    // Display an error message if the config file is not found
                    LogToConsole("Config file not found!");
                }
            }
            else
            {
               LogToConsole("No RichTextBox found for the current tab!");
            }
        }
        // save bookmarks 

        private void AddBookmark(string path)
        {
            string configPath = "Bonkers.cfg";

            // Load the existing configuration
            string configContent = File.ReadAllText(configPath);
            Config config = JsonSerializer.Deserialize<Config>(configContent);

            // Add the new bookmark if it doesn't already exist
            List<string> bookmarks = config.Bookmarks?.ToList() ?? new List<string>();
            if (!bookmarks.Contains(path))
            {
                bookmarks.Add(path);
                config.Bookmarks = bookmarks.ToArray();
                SaveBookmarks(config.Bookmarks);
                LoadConfig();
            }
            else
            {
                LogToConsole("Bookmark already exists!");
            }
        }

        private void SaveBookmarks(string[] bookmarks)
        {
            string configPath = "Bonkers.cfg";

            // Load the existing configuration
            string configContent = File.ReadAllText(configPath);
            Config config = JsonSerializer.Deserialize<Config>(configContent);

            // Update only the bookmarks
            config.Bookmarks = bookmarks;

            // Save the updated configuration
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(config, options);
            File.WriteAllText(configPath, json);

            LogToConsole("Bookmarks saved successfully!");
        }

        // Define a method for saving the config
        //fsdgfhfsg
        private void SaveConfig(Config config)
        {
            string configPath = "Bonkers.cfg";
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(config, options);
            File.WriteAllText(configPath, json);

            LogToConsole("Config file saved successfully!");
        }


        private void saveConfig()
        {
            // Define the path to the config file
            string configPath = "Bonkers.cfg";

            // Get the current RichTextBox based on its tag
            RichTextBox selectedRichTextBox = FindRichTextBoxByTag(currentTextboxTag);

            if (selectedRichTextBox != null)
            {
                // Get the text content from the selected RichTextBox
                string textContent = selectedRichTextBox.Text;

                // Write the content to the config file
                File.WriteAllText(configPath, textContent);

                // Optionally display a message to indicate successful save
                LogToConsole("Config file saved successfully!");
            }
            else
            {
                LogToConsole("No RichTextBox found for the current tab!");
            }
        }

        // Define an event handler for reloading the config
        private void reloadConfigToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Reload the config
            LoadConfig();
        }



        private void listView_DoubleClick(object sender, EventArgs e)
        {
            string imagePath = toolStripStatusLabel1.Text; // Assuming toolStripStatusLabel1 contains the image file path
            OpenImageInNewForm();
            int lock101 = 0;
            if (!string.IsNullOrEmpty(imagePath) & lock101 == 1)
            {
                try
                {
                    // Load the original image from the file path
                    Image originalImage = Image.FromFile(imagePath);

                    // Calculate the aspect ratio
                    float aspectRatio = (float)originalImage.Width / (float)originalImage.Height;

                    // Set the maximum width and height for the resized image
                    int maxWidth = MaxPboxW;
                    int maxHeight = MaxPboxH;

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
                    LogToConsole($"Error loading/resizing image: {ex.Message}");
                }
            }
            else
            {
                LogToConsole("Image file path is empty.");
            }
        }

        private void OpenImageInNewForm()
        {
            // Get the image path from the ToolStripStatusLabel
            string imagePath = toolStripStatusLabel1.Text;

            // Check if the file exists
            if (!System.IO.File.Exists(imagePath))
            {
                LogToConsole("Image file not found.");
                return;
            }

            // Get the directory and all image files in the directory
            string directory = System.IO.Path.GetDirectoryName(imagePath);
            string[] imageFiles = System.IO.Directory.GetFiles(directory, "*.jpg")
                                    .Concat(System.IO.Directory.GetFiles(directory, "*.png"))
                                    .Concat(System.IO.Directory.GetFiles(directory, "*.bmp"))
                                    .Concat(System.IO.Directory.GetFiles(directory, "*.gif"))
                                    .OrderBy(f => f).ToArray();

            // Find the index of the current image
            int currentIndex = Array.IndexOf(imageFiles, imagePath);

            // Create a new Form
            Form imageForm = new Form();
            imageForm.Text = "Image Viewer";
            imageForm.Size = new System.Drawing.Size(800, 600);

            // Create a PictureBox
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = Image.FromFile(imageFiles[currentIndex]);
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom; // Auto size image to form

            // Add the PictureBox to the form
            imageForm.Controls.Add(pictureBox);

            // Enable the form to be resized and keep the pictureBox autosized
            imageForm.FormBorderStyle = FormBorderStyle.Sizable;

            // Event handler for the mouse wheel scroll event
            imageForm.MouseWheel += (sender, e) =>
            {
                // Scroll up (next image) or down (previous image)
                if (e.Delta > 0) // Mouse scroll up
                {
                    currentIndex = (currentIndex + 1) % imageFiles.Length;
                }
                else if (e.Delta < 0) // Mouse scroll down
                {
                    currentIndex = (currentIndex - 1 + imageFiles.Length) % imageFiles.Length;
                }

                // Update the image in the PictureBox
                pictureBox.Image = Image.FromFile(imageFiles[currentIndex]);
            };

            // Show the new form
            imageForm.Show();
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
            else if (e.Button == MouseButtons.Middle)
            {
                contextMenuStrip6.Show(this, this.PointToClient(MousePosition));
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
        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            Image originalImage = Image.FromFile(toolStripStatusLabel1.Text);
            const double scaleFactor = 1.1;
            int newWidth, newHeight;

            if (e.Delta > 0)
            {
                // Mouse wheel scrolled up (Zoom in)
                newWidth = (int)(pictureBox1.Width * scaleFactor);
                newHeight = (int)(pictureBox1.Height * scaleFactor);
            }
            else
            {
                // Mouse wheel scrolled down (Zoom out)
                newWidth = (int)(pictureBox1.Width / scaleFactor);
                newHeight = (int)(pictureBox1.Height / scaleFactor);
            }

            // Ensure the new size fits within the form's client area
            if (newWidth <= this.ClientSize.Width && newHeight <= this.ClientSize.Height)
            {
                pictureBox1.Width = newWidth;
                pictureBox1.Height = newHeight;
                Bitmap resizedImage = new Bitmap(originalImage, newWidth, newHeight);
                pictureBox1.Image = resizedImage;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }


        private void PictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (pictureBox1.Visible == true)
            {
                pictureBox1.Focus();
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

        private async void cogVLMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clear toolStripStatusLabel5 text
            toolStripStatusLabel5.Text = "";

            // Get the selected RichTextBox based on the current textbox tag
            RichTextBox selectedRichTextBox = FindRichTextBoxByTag(currentTextboxTag);

            if (selectedRichTextBox != null)
            {
                // Check if the file path in toolStripStatusLabel1 exists
                string filePath = toolStripStatusLabel1.Text;
                if (File.Exists(filePath))
                {
                    try
                    {
                        // Load the image from the file path
                        using (Image image = Image.FromFile(filePath))
                        {
                            // Convert the image to base64 string (PNG format)
                            string base64String = ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Png);

                            // Send the API request asynchronously
                            await SendApiRequestChatCompletions(base64String, selectedRichTextBox);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions when loading the image
                        toolStripStatusLabel5.Text = $"Error loading image: {ex.Message}";
                    }
                }
                else
                {
                    // Update toolStripStatusLabel5 with an error message
                    toolStripStatusLabel5.Text = "Invalid file path";
                }
            }
            else
            {
                // Handle case where no RichTextBox is found
                toolStripStatusLabel5.Text = "No RichTextBox found for selected tab";
            }
        }

        private void contextMenuStrip3_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //IDK what I want to do with this yet
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the currently selected RichTextBox based on the active TabPage
                RichTextBox selectedRichTextBox = FindRichTextBoxByTag(currentTextboxTag);

                if (selectedRichTextBox != null && !string.IsNullOrEmpty(selectedRichTextBox.SelectedText))
                {
                    Clipboard.Clear();
                    string clip = selectedRichTextBox.SelectedText;

                    // Output to console for debugging
                    LogToConsole("copied text: " + clip);

                    // Copy the selected text to clipboard
                    Clipboard.SetText(clip);
                }
                else
                {
                    // Output to console for debugging
                    LogToConsole("No text selected to copy.");
                }
            }
            catch (ExternalException ex)
            {
                // Handle clipboard exceptions if necessary
                LogToConsole("Clipboard operation failed: " + ex.Message);
            }
        }

        private void LogToConsole(string message)
        {
            // Toggle console text color for readability
            Console.ForegroundColor = consoleTrack % 2 == 0 ? ConsoleColor.Green : ConsoleColor.White;
            Console.Out.WriteLine(message);
            consoleTrack++;
        }


        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the currently selected RichTextBox based on the active TabPage
            RichTextBox selectedRichTextBox = FindRichTextBoxByTag(currentTextboxTag);

            if (selectedRichTextBox != null && Clipboard.ContainsText())
            {
                string clipboardText = Clipboard.GetText();

                if (selectedRichTextBox.SelectedText != string.Empty)
                {
                    // If text is selected, replace it with clipboard content
                    selectedRichTextBox.SelectedText = clipboardText;
                }
                else
                {
                    // Insert clipboard content at current cursor position
                    int selectionStart = selectedRichTextBox.SelectionStart;
                    selectedRichTextBox.Text = selectedRichTextBox.Text.Insert(selectionStart, clipboardText);
                    selectedRichTextBox.SelectionStart = selectionStart + clipboardText.Length;
                }

                // Output to console for debugging
                LogToConsole("pasted text: " + clipboardText);
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the currently selected RichTextBox based on the active TabPage
                RichTextBox selectedRichTextBox = FindRichTextBoxByTag(currentTextboxTag);

                if (selectedRichTextBox != null && !string.IsNullOrEmpty(selectedRichTextBox.SelectedText))
                {
                    Clipboard.Clear();
                    string cut = selectedRichTextBox.SelectedText;

                    // Output to console for debugging
                    LogToConsole("cut text: " + cut);

                    // Cut the selected text and copy to clipboard
                    Clipboard.SetText(cut);
                    selectedRichTextBox.SelectedText = "";
                }
                else
                {
                    // Output to console for debugging
                    LogToConsole("No text selected to cut.");
                }
            }
            catch (ExternalException ex)
            {
                // Handle clipboard exceptions if necessary
                LogToConsole("Clipboard operation failed: " + ex.Message);
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the currently selected RichTextBox based on the active TabPage
            RichTextBox selectedRichTextBox = FindRichTextBoxByTag(currentTextboxTag);

            if (selectedRichTextBox != null)
            {
                // Focus on the RichTextBox and select all text
                selectedRichTextBox.Focus();
                selectedRichTextBox.SelectAll();
            }
        }
        private async void ollamaAPIBulkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var listView = FindListViewByTag(currentImageBoxTag);

            // Initialize the cancellation token source
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            // Make the progress bar visible
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Maximum = listView.Items.Count;
            toolStripProgressBar1.Value = 0;

            try
            {
                for (int i = 0; i < listView.Items.Count; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        // Handle the cancellation request
                        LogToConsole("Task was cancelled.");
                        break;
                    }

                    ListViewItem item = listView.Items[i];

                    // Select the item
                    item.Selected = true;

                    // Manually trigger the ItemSelectionChanged event
                    ListViewItemSelectionChangedEventArgs eventArgs =
                        new ListViewItemSelectionChangedEventArgs(item, i, true);
                    ListView_ItemSelectionChanged(listView, eventArgs);

                    // Perform your API request here
                    if (File.Exists(toolStripStatusLabel1.Text))
                    {
                        // Get the file path
                        string filePath = toolStripStatusLabel1.Text;

                        // Load the image from the file path
                        using (Image image = Image.FromFile(filePath))
                        {
                            // Convert the image to a base64 string (PNG format)
                            string base64String = ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Png);

                            // Send the API request
                            await OllamaApiCall(base64String);
                        }
                    }
                    else
                    {
                        // Update toolStripStatusLabel5 with an error message
                        toolStripStatusLabel5.Text = "Invalid file path";
                    }

                    // Deselect the item after processing
                    item.Selected = false;

                    // Update the progress bar
                    toolStripProgressBar1.Value = i + 1;
                }
            }
            catch (Exception ex)
            {
                LogToConsole($"An error occurred: {ex.Message}");
            }
            finally
            {
                // Make the progress bar invisible
                toolStripProgressBar1.Visible = false;
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }
        }
        private async void ollamaAPIToolStripMenuItem_Click(object sender, EventArgs e)
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
                    await OllamaApiCall(base64String);
                }
            }
            else
            {
                // Update toolStripStatusLabel5 with an error message
                toolStripStatusLabel5.Text = "Invalid file path";
            }
        }

        private static readonly HttpClient client = new HttpClient();
        //private const string url = "http://" + ollamaAddress + ":11434/api/generate";

        public async Task OllamaApiCall(string base64Image)
        {
            string jsonPayload = $@"
    {{
        ""model"": ""{ollamaModel}"",
        ""system"": ""{ollamaSystem}"",
        ""prompt"": ""{OllamaPrompt}"",
        ""images"": [""{base64Image}""],
        ""stream"": false
    }}";

            var jsonContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            string apiURL = ollamaAddress is not null
                ? $"http://{ollamaAddress}:11434/api/generate"
                : "http://localhost:11434/api/generate";

            try
            {
                var response = await client.PostAsync(apiURL, jsonContent);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                HandleResponse(responseBody);
            }
            catch (HttpRequestException e)
            {
                LogToConsole($"Request error: {e.Message}");
            }
        }

        private void HandleResponse(string responseBody)
        {
            // Find the RichTextBox associated with the current TabPage
            RichTextBox selectedRichTextBox = FindRichTextBoxByTag(currentTextboxTag);

            if (selectedRichTextBox != null)
            {
                // Parse the JSON response body
                using (JsonDocument doc = JsonDocument.Parse(responseBody))
                {
                    JsonElement root = doc.RootElement;

                    // Extract the response text from JSON
                    if (root.TryGetProperty("response", out JsonElement responseElement))
                    {
                        string responseText = responseElement.GetString();

                        // Print response to console with alternating colors for tracking

                        LogToConsole("Response: " + responseText);
                        //Console.ResetColor(); // Reset to default color after print
                        //consoleTrack++;

                        // Update the selected RichTextBox with the response text
                        selectedRichTextBox.Text = responseText;

                        if (ollamaSave == true)
                        {
                            SaveRichTextBoxContent();
                        }

                    }
                    else
                    {
                        LogToConsole("Error: Response property not found in JSON.");
                    }
                }
            }
            else
            {
                LogToConsole("Error: RichTextBox not found for the current tag.");
            }
        }

        private void reloadConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }



        //EXTREMELY EXPERIMENTAL
        private void AddNewTab()
        {

            // Create a new TabPage
            TabPage newTabPage = new TabPage("Tab: " + tabTag);

            // Create a new RichTextBox
            RichTextBox newRichTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill, // Fill the TabPage with the RichTextBox
                ContextMenuStrip = contextMenuStrip3, // Attach contextMenuStrip4 to the RichTextBox
                Tag = tabTag.ToString(), // Set the tag as needed
                BackColor = Color.LightGray, // Set the background color to black
                ForeColor = Color.Black // Set the text color to green
            };

            // Attach the KeyDown event handler
            newRichTextBox.KeyDown += richTextBox_KeyDown;

            // Add the RichTextBox to the TabPage
            newTabPage.Controls.Add(newRichTextBox);

            // Add the TabPage to the TabControl
            tabControl1.TabPages.Add(newTabPage);

            // Set the newly added tab as the selected tab
            tabControl1.SelectedTab = newTabPage;
            newRichTextBox.Enter += RichTextBox_Enter;
            newRichTextBox.MouseWheel += tabControl1_MouseWheelResize;
            newRichTextBox.MouseDown += tabControl1_MouseDownResize;
            tabTag++;
        }

        private void RichTextBox_Enter(object sender, EventArgs e)
        {
            //if (tabControl1.Height != tabControlExpand)
            // {
            //     int kl = tabControlExpand - 148;
            //     // Resize the TabControl to a height of 500
            //     tabControl1.Height = tabControlExpand = 200;
            //     tabControl1.Location = new Point(tabControl1.Location.X, tabControl1.Location.Y - kl);
            //     // Resize the parent container of the TabControl if necessary
            // }
        }
        private void newTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewTab();
        }
        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if middle mouse button (mouse wheel click) is clicked
            if (e.Button == MouseButtons.Middle)
            {
                // Ensure there is more than one tab before attempting to close
                if (tabControl1.TabCount > 1)
                {
                    // Get the tab at the clicked position
                    for (int i = 0; i < tabControl1.TabCount; i++)
                    {
                        Rectangle tabRect = tabControl1.GetTabRect(i);
                        if (tabRect.Contains(e.Location))
                        {
                            // Close the tab
                            tabControl1.TabPages.RemoveAt(i);
                            break;
                        }
                    }
                }
                else
                {
                    // Optionally handle the case where only one tab is remaining
                    LogToConsole("Cannot close the last tab.");
                }
            }
        }
        private void tabControl2_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Check if middle mouse button (mouse wheel click) is clicked
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                // Ensure there is more than one tab before attempting to close
                if (tabControl2.TabCount > 1)
                {
                    // Get the tab at the clicked position
                    for (int i = 0; i < tabControl2.TabCount; i++)
                    {
                        System.Drawing.Rectangle tabRect = tabControl2.GetTabRect(i);
                        if (tabRect.Contains(e.Location))
                        {
                            // Close the tab
                            tabControl2.TabPages.RemoveAt(i);
                            break;
                        }
                    }
                }
                else
                {
                    // Optionally handle the case where only one tab is remaining
                    LogToConsole("Cannot close the last tab.");
                }
            }
        }
        //HYPER EXPERIMENTAL
        private void AddTextToSelectedRichTextBox(RichTextBox richTextBox, string text)
        {
            // Ensure we are operating on the UI thread
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke(new Action(() => richTextBox.Text = text));
            }
            else
            {
                richTextBox.Text = text;
            }
        }
        private RichTextBox FindRichTextBoxByTag(int tag)
        {
            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                foreach (Control control in tabPage.Controls)
                {
                    if (control is RichTextBox richTextBox && richTextBox.Tag is string rtbTagString && int.TryParse(rtbTagString, out int rtbTag) && rtbTag == tag)
                    {
                        return richTextBox;
                    }
                }
            }
            return null;
        }
        private System.Windows.Forms.ListView FindListViewByTag(int tag)
        {
            foreach (System.Windows.Forms.TabPage tabPage in tabControl2.TabPages)
            {
                foreach (System.Windows.Forms.Control control in tabPage.Controls)
                {
                    if (control is System.Windows.Forms.ListView listView && listView.Tag is string lvTagString && int.TryParse(lvTagString, out int lvTag) && lvTag == tag)
                    {
                        LogToConsole("FindListViewByTag returned: " + listView);
                        return listView;

                    }
                }
            }
            return null;
        }

        private void UpdateRichTextBox(RichTextBox richTextBox, string text)
        {
            // Ensure we are operating on the UI thread
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke(new Action(() => richTextBox.Text = text));
            }
            else
            {
                richTextBox.Text = text;
            }
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the currently selected tab
            TabPage selectedTab = tabControl1.SelectedTab;

            if (selectedTab != null)
            {
                // Iterate through the controls in the selected tab page
                foreach (Control control in selectedTab.Controls)
                {
                    // Check if the control is a RichTextBox
                    if (control is RichTextBox richTextBox)
                    {
                        // Get the tag of the RichTextBox and log it to the console
                        object rtbTag = richTextBox.Tag;
                        LogToConsole($"Tag of RichTextBox in selected tab: {rtbTag}");

                        // Convert the tag to int
                        if (rtbTag != null && int.TryParse(rtbTag.ToString(), out int tagValue))
                        {
                            currentTextboxTag = tagValue;
                            LogToConsole($"Converted Tag to int: {currentTextboxTag}");
                        }
                        else
                        {
                            LogToConsole("Failed to parse Tag to int.");
                        }

                        break; // Assuming there's only one RichTextBox per tab, break after finding it
                    }
                }
            }
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //RichTextBox targetRichTextBox = FindRichTextBoxByTag(currentTextboxTag);
            //if (targetRichTextBox != null)
            // {
            //     targetRichTextBox.AppendText("this is a test");
            // }
            // else
            // {
            //    MessageBox.Show("RichTextBox not found for the current tag.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // }
            FindListViewByTag(currentImageBoxTag);


        }

        ///EXTRA HYPER SUPER EXPERIMENTAL
        ///

        private async void consoleModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            consoleMode = 1;
            await StartConsoleMode();
            consoleMode = 0;
        }
        static Command ParseArguments(string input)
        {
            var args = Regex.Matches(input, @"[\""].+?[\""]|[^ ]+")
                            .Cast<Match>()
                            .Select(m => m.Value.Replace("\"", ""))
                            .ToArray();

            if (args.Length == 0)
            {
                return null;
            }

            var command = new Command();

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "send":
                        command.Name = "send";
                        break;
                    case "--apiCall":
                        if (i + 1 < args.Length)
                        {
                            command.ApiCall = args[i + 1];
                            i++;
                        }
                        break;
                    case "--prompt":
                        if (i + 1 < args.Length)
                        {
                            command.Prompt = args[i + 1];
                            i++;
                        }
                        break;
                    case "--system":
                        if (i + 1 < args.Length)
                        {
                            command.System = args[i + 1];
                            i++;
                        }
                        break;
                    case "--help":
                        Console.WriteLine("*******************************************************************************\r\n\r\n                             BONKERS 0.1.4 HELP MENU\r\n\r\nWelcome to the BONKERS 0.1.4 help screen! Below you'll find a list of available\r\ncommands and their descriptions. Use these commands to navigate and utilize the\r\nvarious functions of BONKERS 0.1.4. Type the command followed by any necessary\r\nparameters and hit ENTER.\r\n\r\n COMMAND      DESCRIPTION\r\n --------     ----------------------------------------------------------\r\n --help         Display this help menu\r\n\r\n\r\nFor detailed information on a specific command, look at the source code :)\r\n*******************************************************************************\r\n\r\n                           PRESS ANY KEY TO RETURN TO MAIN MENU\r\n\r\n*******************************************************************************");
                        break;
                    default:
                        // Handle unknown argument
                        break;
                }
            }

            return command;
        }
        class Command
        {
            public string Name { get; set; }
            public string ApiCall { get; set; }
            public string Prompt { get; set; }
            public string System { get; set; }
        }
#pragma warning disable CS1998
        private async Task StartConsoleMode()
        {
            while (consoleMode == 1)
            {
                //Console.ForegroundColor = consoleTrack % 2 == 0 ? ConsoleColor.Green : ConsoleColor.White;
                //Console.WriteLine("Enter some text:");
                //consoleTrack++;

                //string userInput = await Task.Run(() => Console.ReadLine());

                //Console.ForegroundColor = consoleTrack % 2 == 0 ? ConsoleColor.Green : ConsoleColor.White;
                //Console.WriteLine($"You entered: {userInput}");
                //consoleTrack++;
                consoleWriter("**************************************************************************\r\n*                                                                        *\r\n*                          WELCOME TO MY APP                             *\r\n*                                                                        *\r\n*               The Ultimate Solution for Your Daily Tasks!              *\r\n*                                                                        *\r\n*               Developed by [Your Name], Version 1.0, 1985              *\r\n*                                                                        *\r\n**************************************************************************\r\n\r\nGreetings, User!\r\n\r\nPrepare yourself for an unparalleled computing experience with MY APP, \r\nthe cutting-edge software designed to simplify and enhance your workflow.\r\n\r\nInstructions:\r\n1. Follow the on-screen prompts.\r\n2. Use the arrow keys to navigate through options.\r\n3. Press ENTER to select.\r\n\r\nNeed Help? Contact our support team at 1-800-MYAPP-HELP\r\n\r\nThank you for choosing MY APP!\r\nPress any key to begin...\r\n");
                Console.WriteLine("Enter commands (type 'exit' to quit):");
                while (true)
                {
                    Console.Write("> ");
                    var input = Console.ReadLine();
                    if (input == "exit")
                    {
                        consoleMode = 0;
                        break;
                    }

                    var command = ParseArguments(input);

                    if (command != null)
                    {
                        // Handle the command here
                        Console.WriteLine($"API Call: {command.ApiCall}");
                        Console.WriteLine($"Prompt: {command.Prompt}");
                        Console.WriteLine($"System: {command.System}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid command or arguments.");
                    }
                }


            }
        }
#pragma warning restore CS1998
        private void AddNewImageTab()
        {
            // Create a new TabPage
            System.Windows.Forms.TabPage newTabPage = new System.Windows.Forms.TabPage("Tab: " + imgTabTag);



            // Create a new ImageList and attach it to the ListView
            System.Windows.Forms.ImageList imageList = new System.Windows.Forms.ImageList
            {
                Tag = imgTabTag.ToString(), // Set the ImageList tag as the current imgTabTag
                ImageSize = new Size(256, 256) // Set the image size to 256x256 pixels
            };
            // Create a new ListView
            System.Windows.Forms.ListView newListView = new System.Windows.Forms.ListView
            {
                Dock = System.Windows.Forms.DockStyle.Fill, // Fill the TabPage with the ListView
                ContextMenuStrip = contextMenuStrip2, // Attach contextMenuStrip2 to the ListView
                Tag = imgTabTag.ToString(), // Set the ListView tag as the current imgTabTag
                //BackColor = System.Drawing.Color.LightGray, // Set the background color to light gray
                //ForeColor = System.Drawing.Color.Black, // Set the text color to black
                //View = System.Windows.Forms.View.LargeIcon, // Set the view to large icon
                LargeImageList = imageList,
                Visible = true,
                Sorting = SortOrder.Ascending


            };
            //newListView.LargeImageList = imageList;
            newListView.LargeImageList = imageList;
            // Add the ListView to the TabPage
            newTabPage.Controls.Add(newListView);

            // Add the TabPage to the TabControl
            tabControl2.TabPages.Add(newTabPage);

            // Store the ImageList in the dictionary with its tag for future reference
            imageListDictionary.Add(imgTabTag.ToString(), imageList);
            newListView.ItemSelectionChanged += ListView_ItemSelectionChanged;
            newListView.MouseDown += NewListView_MouseDown;
            newListView.MouseClick += listView_MouseClick;
            newListView.MouseDoubleClick += listView_DoubleClick;
            newListView.MouseDown += treeView1_MouseDown;
            // Set the newly added tab as the selected tab
            tabControl2.SelectedTab = newTabPage;

            // Increment imgTabTag for the next tab
            imgTabTag++;



        }
        private void consoleWriter(string message)
        {
            string[] lines = message.Split('\n');

            foreach (string line in lines)
            {
                foreach (char c in line)
                {
                    // Choose a color based on the character's position
                    ConsoleColor color = ConsoleColor.Green;

                    Console.ForegroundColor = color;
                    Console.Write(c);

                    // Adjust delay for effect
                    Thread.Sleep(964 / 1000);
                }
                Console.WriteLine();
            }
            return;
        }
        private void consoleWriterRGB(string message)
        {
            string[] lines = message.Split('\n');

            foreach (string line in lines)
            {
                foreach (char c in line)
                {
                    // Choose a color based on the character's position
                    ConsoleColor color = GetRainbowColor(line.IndexOf(c));

                    Console.ForegroundColor = color;
                    Console.Write(c);

                    // Adjust delay for effect
                    Thread.Sleep(900 / 1000);
                }
                Console.WriteLine();
            }
            return;
        }
        private void NewListView_MouseDown(object? sender, MouseEventArgs e)
        {
            // if (tabControl1.Height != 148)
            // {
            //     // Resize the TabControl to a height of 500
            //     int kl = tabControlExpand - 148;
            //     tabControl1.Height = 148;
            //     tabControl1.Location = new Point(tabControl1.Location.X, tabControl1.Location.Y + kl);
            //     // Resize the parent container of the TabControl if necessary
            // }
        }
        private void listView_MouseClick(object sender, MouseEventArgs e)
        {
            var listView = FindListViewByTag(currentImageBoxTag);
            if (e.Button == MouseButtons.Left)
            {
                ListViewHitTestInfo hit = listView.HitTest(e.Location);
                if (hit.Item != null)
                {
                    // Handle the click on the specific item
                    LogToConsole($"Item clicked: {hit.Item.Text}");
                }
            }
        }

        private void ListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var listView = FindListViewByTag(currentImageBoxTag);
            var imageList = GetImageListByTag(currentImageBoxTag.ToString());

            configFlag = 0;
            // Check if an item is selected
            if (listView.SelectedItems.Count > 0)
            {
                currentIndex = e.ItemIndex;
                //SaveRichTextBoxContent(); // Ensure to save content when item selection changes
                OpenTextFileOfSelectedPhoto(); // Load text file content for the selected photo

                if (pictureBox1.Visible == true)
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
                            int maxWidth = MaxPboxW;
                            int maxHeight = MaxPboxH;

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
                            LogToConsole($"Error loading/resizing image: {ex.Message}");
                        }
                    }
                    else
                    {
                        LogToConsole("Image file path is empty.");
                    }
                }


            }
            // if (tabControl1.Height != 148)
            // {
            //     // Resize the TabControl to a height of 500
            //     int kl = tabControlExpand - 148;
            //     tabControl1.Height = 148;
            //     tabControl1.Location = new Point(tabControl1.Location.X, tabControl1.Location.Y + kl);
            //     // Resize the parent container of the TabControl if necessary
            // }
        }


        private ImageList GetImageListByTag(string tag)
        {
            if (imageListDictionary.ContainsKey(tag))
            {
                LogToConsole("image list " + tag);
                return imageListDictionary[tag];
            }
            return null; // Handle case where ImageList with specified tag is not found
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

            // Get the currently selected tab
            System.Windows.Forms.TabPage selectedTab = tabControl2.SelectedTab;

            if (selectedTab != null)
            {
                // Iterate through the controls in the selected tab page
                foreach (System.Windows.Forms.Control control in selectedTab.Controls)
                {
                    // Check if the control is a ListView
                    if (control is System.Windows.Forms.ListView listView)
                    {
                        // Get the tag of the ListView and log it to the console
                        object lvTag = listView.Tag;
                        LogToConsole($"Tag of ListView in selected tab: {lvTag}");

                        // Convert the tag to int
                        if (lvTag != null && int.TryParse(lvTag.ToString(), out int tagValue))
                        {
                            currentImageBoxTag = tagValue;
                            LogToConsole($"Converted Tag to int: {currentImageBoxTag}");
                        }
                        else
                        {
                            LogToConsole("Failed to parse Tag to int.");
                        }

                        break; // Assuming there's only one ListView per tab, break after finding it
                    }
                }
            }
        }

        private void newTabToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddNewImageTab();
        }

        private void addBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                string path = treeView1.SelectedNode.Tag as string;

                if (!string.IsNullOrEmpty(path))
                {
                    AddBookmark(path);
                }
            }
        }

        private void metadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var listView = FindListViewByTag(currentImageBoxTag);
            if (listView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView.SelectedItems[0];
                int imageIndex = selectedItem.ImageIndex;

                // Retrieve the corresponding image from the ImageList
                var imageList = GetImageListByTag(currentImageBoxTag.ToString());
                if (imageList != null && imageIndex >= 0 && imageIndex < imageList.Images.Count)
                {
                    //Image selectedImage = imageList.Images[imageIndex];
                    string imagePath = toolStripStatusLabel1.Text; // Assuming toolStripStatusLabel1 contains the image file path

                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        try
                        {
                            // Load the original image from the file path
                            //Image originalImage = Image.FromFile(imagePath);

                            // Print metadata to the console
                            PrintImageMetadata(imagePath);
                        }
                        catch { }

                    }
                }
                else
                {
                    Console.WriteLine("Image not found in ImageList.");
                }
            }
            else
            {
                Console.WriteLine("No image selected.");
            }
        }

        private void PrintImageMetadata(string filePath)
        {
            // Load the image
            using (Image image = Image.FromFile(filePath))
            {
                // Print basic metadata
                Console.WriteLine($"Image Size: {image.Width}x{image.Height}");
                Console.WriteLine($"Pixel Format: {image.PixelFormat}");

                // EXIF Metadata
                Console.WriteLine("EXIF Metadata:");
                foreach (var prop in image.PropertyItems)
                {
                    string propValue = GetPropertyItemValueAsString(prop);
                    Console.WriteLine($"Property ID: {prop.Id}, Type: {prop.Type}, Length: {prop.Len}, Value: {propValue}");
                }

                // Using MetadataExtractor for ICC, IPTC, XMP, and PNG-tEXt
                Console.WriteLine("Additional Metadata:");
                var directories = MetadataExtractor.ImageMetadataReader.ReadMetadata(filePath);

                foreach (MetaDirectory directory in directories)
                {
                    foreach (var tag in directory.Tags)
                    {
                        Console.WriteLine($"{directory.Name} - {tag.Name} = {tag.Description}");

                        // Handle PNG-tEXt metadata
                        if (tag.Name == "PNG-tEXt")
                        {
                            string[] lines = tag.Description.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string line in lines)
                            {
                                // Print each line that separates with a semicolon
                                Console.WriteLine(line.Trim());
                            }
                        }
                    }

                    if (directory.HasError)
                    {
                        foreach (var error in directory.Errors)
                        {
                            Console.WriteLine($"ERROR: {error}");
                        }
                    }
                }
            }
        }

        private string GetPropertyItemValueAsString(PropertyItem prop)
        {
            try
            {
                switch (prop.Type)
                {
                    case 1: // PropertyTagTypeByte
                        return BitConverter.ToString(prop.Value);
                    case 2: // PropertyTagTypeASCII
                        return System.Text.Encoding.ASCII.GetString(prop.Value);
                    case 3: // PropertyTagTypeShort
                        return BitConverter.ToUInt16(prop.Value, 0).ToString();
                    case 4: // PropertyTagTypeLong
                        return BitConverter.ToUInt32(prop.Value, 0).ToString();
                    case 5: // PropertyTagTypeRational
                        uint numerator = BitConverter.ToUInt32(prop.Value, 0);
                        uint denominator = BitConverter.ToUInt32(prop.Value, 4);
                        return $"{numerator}/{denominator}";
                    default:
                        return BitConverter.ToString(prop.Value);
                }
            }
            catch (Exception ex)
            {
                return "Error reading property item: " + ex.Message;
            }
        }

        private void metadataToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string imagePath = toolStripStatusLabel1.Text; // Assuming toolStripStatusLabel1 contains the image file path

            if (!string.IsNullOrEmpty(imagePath))
            {
                try
                {
                    // Load the original image from the file path
                    //Image originalImage = Image.FromFile(imagePath);

                    // Print metadata to the console
                    PrintImageMetadata(imagePath);
                }
                catch { }

            }
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            // Calculate the maximum width for tabControl2 based on the form's client width
            int maxTabControl2Width = this.ClientSize.Width - 10; // Adjust margin as needed

            // Ensure tabControl2 doesn't exceed the maximum width
            if (tabControl2.Width > maxTabControl2Width)
            {
                tabControl2.Width = maxTabControl2Width;
            }

            // Adjust treeView1 width based on tabControl2's current width
            treeView1.Width = tabControl2.Left - treeView1.Left - 10; // Adjust margin as needed
        }

        private void treeView1_MouseWheel(object sender, MouseEventArgs e)
        {
            // Define the maximum allowable width for treeView1 (50% of the form width)
            int maxTreeViewWidth = this.ClientSize.Width / 2;

            if (Control.ModifierKeys == Keys.Control)
            {
                if (e.Delta > 0) // Scrolled up
                {
                    // Check if increasing treeView1's width would exceed 50% of the form width
                    if (treeView1.Width + 10 <= maxTreeViewWidth && tabControl2.Width >= 168)
                    {
                        // Increase treeView1 width and decrease tabControl2 width
                        treeView1.Width += 10;
                        tabControl2.Width -= 10;

                        // Move tabControl2 to the right if within bounds
                        if (tabControl2.Left + tabControl2.Width <= this.ClientSize.Width - 10)
                        {
                            tabControl2.Left += 10;
                        }
                    }
                }
                else if (e.Delta < 0) // Scrolled down
                {
                    // Check if decreasing treeView1's width does not go below its minimum allowed width
                    if (treeView1.Width > minTreeViewWidth)
                    {
                        // Decrease treeView1 width and increase tabControl2 width
                        treeView1.Width -= 10;
                        tabControl2.Width += 10;

                        // Move tabControl2 to the left if within bounds
                        if (tabControl2.Left >= 10)
                        {
                            tabControl2.Left -= 10;
                        }
                    }
                }
            }
        }
        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle) // Middle mouse button (wheel button) clicked
            {
                // Reset treeView1 width to 168
                treeView1.Width = 168;

                // Calculate tabControl2 width to fit between treeView1 and the right edge of the form
                tabControl2.Width = this.ClientSize.Width - treeView1.Width - treeView1.Left - 10; // Adjust margin as needed

                // Position tabControl2 just to the right of treeView1
                tabControl2.Left = treeView1.Right + 10; // Adjust margin as needed
            }
        }



        private void tabControl1_MouseWheelResize(object sender, MouseEventArgs e)
        {
            int formHeight = this.ClientSize.Height; // Get the height of the form's client area
            int maxTabControl1Height = formHeight / 2; // Maximum allowed height for tabControl1 (50% of form height)

            if (Control.ModifierKeys.HasFlag(Keys.Control)) // Check for Control key pressed
            {
                if (e.Delta > 0) // Scrolled up
                {
                    if (tabControl1.Height < maxTabControl1Height)
                    {
                        tabControl1.Height += 10;
                        tabControl1.Top -= 10;

                        tabControl2.Height -= 10;
                        treeView1.Height -= 10;
                    }
                }
                else if (e.Delta < 0) // Scrolled down
                {
                    if (tabControl1.Height > originalTabControl1Height) // Adjust based on your original height if needed
                    {
                        tabControl1.Height -= 10;
                        tabControl1.Top += 10;

                        tabControl2.Height += 10;
                        treeView1.Height += 10;
                    }
                }
            }
        }

        private void tabControl1_MouseDownResize(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle) // Middle mouse button (wheel button) clicked
            {
                // Reset to original sizes and positions
                tabControl1.Height = originalTabControl1Height;

                // Move tabControl1 to the bottom of the form
                tabControl1.Top = this.ClientSize.Height - tabControl1.Height;

                // Resize tabControl2 and treeView1 just above tabControl1
                tabControl2.Height = tabControl1.Top - tabControl2.Top;
                treeView1.Height = tabControl1.Top - treeView1.Top;
            }
        }

        private void appendAllFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the selected path from the TreeView's selected node
            string selectedPath = treeView1.SelectedNode.Tag.ToString();

            // Get all text files in the selected directory
            string[] textFiles = SysDirectory.GetFiles(selectedPath, "*.txt");

            // Find the RichTextBox with the corresponding tag
            RichTextBox selectedRichTextBox = FindRichTextBoxByTag(currentTextboxTag);

            if (selectedRichTextBox != null)
            {
                // Iterate through each text file
                foreach (string txtFile in textFiles)
                {
                    // Read the existing text content from the text file
                    string textContent = File.ReadAllText(txtFile);

                    // Append text from the selected RichTextBox to the front of the existing text content
                    textContent = selectedRichTextBox.Text + textContent;

                    // Write the updated text content back to the text file
                    File.WriteAllText(txtFile, textContent);
                }

                // Select all text in the selected RichTextBox
                selectedRichTextBox.SelectAll();

                // Set the selection color to green in the selected RichTextBox
                selectedRichTextBox.SelectionColor = Color.Green;

                // Deselect all text in the selected RichTextBox
                selectedRichTextBox.DeselectAll();

                // Set the selection start to the end of the text in the selectedRichTextBox
                selectedRichTextBox.SelectionStart = selectedRichTextBox.Text.Length;

                // Scroll to the caret position (end of text) in the selectedRichTextBox
                selectedRichTextBox.ScrollToCaret();

                // Show a success message box
                LogToConsole("Text files saved successfully!");
            }
        }

        private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count > 1)
            {
                tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
            }
        }

        private void closeTabToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tabControl2.TabPages.Count > 1)
            {
                tabControl2.TabPages.RemoveAt(tabControl2.SelectedIndex);
            }
        }

        private void undoAppendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the selected path from the TreeView's selected node
            string selectedPath = treeView1.SelectedNode.Tag.ToString();

            // Get all text files in the selected directory
            string[] textFiles = SysDirectory.GetFiles(selectedPath, "*.txt");

            // Find the RichTextBox with the corresponding tag
            RichTextBox selectedRichTextBox = FindRichTextBoxByTag(currentTextboxTag);

            if (selectedRichTextBox != null)
            {
                // Get the text that was appended from the RichTextBox
                string appendedText = selectedRichTextBox.Text;

                // Iterate through each text file
                foreach (string txtFile in textFiles)
                {
                    // Read the existing text content from the text file
                    string textContent = File.ReadAllText(txtFile);

                    // Check if the text from the RichTextBox exists at the end of the file
                    if (textContent.EndsWith(appendedText))
                    {
                        // Remove the appended text from the end of the text content
                        textContent = textContent.Substring(0, textContent.Length - appendedText.Length);

                        // Write the updated text content back to the text file
                        File.WriteAllText(txtFile, textContent);
                    }
                }

                // Select all text in the selected RichTextBox
                selectedRichTextBox.SelectAll();

                // Set the selection color to red in the selected RichTextBox to indicate undo
                selectedRichTextBox.SelectionColor = Color.Red;

                // Deselect all text in the selected RichTextBox
                selectedRichTextBox.DeselectAll();

                // Set the selection start to the end of the text in the selected RichTextBox
                selectedRichTextBox.SelectionStart = selectedRichTextBox.Text.Length;

                // Scroll to the caret position (end of text) in the selected RichTextBox
                selectedRichTextBox.ScrollToCaret();

                // Show a success message box
                LogToConsole("Undo operation completed successfully!");
            }
        }
    }
}
