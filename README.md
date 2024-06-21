# Bonkers
Bonkers is a simple manual image captioning program designed to streamline the process of adding captions to your images. This application is built using C# and offers a mostly user-friendly interface for organizing and captioning images effectively.
<br>Its called Bonkers because its Bonkers I had to make this.

[Watch the video](https://files.catbox.moe/44btc6.webm)

## Features

- **Load Directories and Drives:** Easily load and navigate through directories and drives to manage your image files.
- **Manual Captioning:** Add custom captions to your images manually.
- **Tree View Navigation:** Use the tree view to explore your file system and find the images you want to caption.
- **Dynamic Loading:** Drives and directories are loaded dynamically, improving performance and usability.

## Getting Started

### Prerequisites

## running on visual studio
- Windows operating system
- .NET Framework 8 LTS or later
- Visual Studio 2019 or later

### Installation Options

1. **Clone the repository:**

   ```sh
   git clone https://github.com/yourusername/bonkers.git

    Open the solution:

    Open the Bonkers.sln file in Visual Studio.

    Build the solution:

    Build the solution to restore the necessary NuGet packages and compile the project.

    Run the application:

    Start the application by pressing F5 or selecting Debug > Start Debugging.

2. **download from releases**
3. **run on linux with wine**    

### Usage

    Load Drives:

    Upon starting the application, all available drives on your system will be listed in the tree view, unexpanded. Click on a drive to expand it and view its directories.

    Navigate Directories:

    Expand the directories to navigate through your file system and locate the images you want to caption.

    Add Captions:

    Select an image and add your custom caption using the provided interface. Save the caption by using CTRL+S, the text will change from red to green to indicate it was saved.
    You can right click on a image and click open to open its text file.
    

### Contributing

### Contributions are welcome! If you would like to contribute to Bonkers, please follow these steps:

    Fork the repository
    Create a new branch: git checkout -b feature/YourFeature
    Make your changes
    Commit your changes: git commit -m 'Add some feature'
    Push to the branch: git push origin feature/YourFeature
    Submit a pull request

### Documentation

Class: Form1
Fields

    currentDirectory (string): Stores the current directory path.
    editingMultipleFiles (bool): Indicates if multiple files are being edited.
    selectedImageTextFile (string): Stores the path of the selected image text file.
    currentIndex (int): Tracks the current index in the ListView.
    cancellationTokenSource (CancellationTokenSource): Manages cancellation tokens for asynchronous tasks.
    localAPI (string): Stores the local API endpoint.
    externalAPI (string): Stores the external API endpoint.
    configFlag (int): Configuration flag for determining specific actions.
    newWidth (int): Stores the new width for image resizing.
    newHeight (int): Stores the new height for image resizing.

Constructor

    Form1(): Initializes the form, loads the configuration, and loads the directories.

Nested Class: Config

    Represents the configuration settings.
    Properties:
        LocalAPI (string): Local API endpoint.
        ExternalAPI (string): External API endpoint.

Methods
Configuration Methods

    LoadConfig(): Loads the configuration from a file (bonkers.cfg). If the file doesn't exist, it creates a default configuration.

Directory Loading Methods

    LoadDirectories(): Loads all drives on the system into the TreeView.
    treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e): Handles the BeforeExpand event for the TreeView, loading directories as needed.
    LoadDirectories(TreeNode node): Loads the directories for a given TreeNode.

Event Handlers

    treeView1_AfterSelect(object sender, TreeViewEventArgs e): Handles the AfterSelect event for the TreeView, displaying images in the selected directory.
    treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e): Handles the NodeMouseClick event for the TreeView, showing the context menu.
    listView1_ItemSelectionChanged(Object sender, ListViewItemSelectionChangedEventArgs e): Handles the ItemSelectionChanged event for the ListView, opening the text file of the selected photo.
    richTextBox1_KeyDown(object sender, KeyEventArgs e): Handles the KeyDown event for the RichTextBox, managing shortcut keys for saving and formatting text.

Context Menu Actions

    generateTxtFilesToolStripMenuItem_Click(object sender, EventArgs e): Generates text files for images in the selected directory.
    openToolStripMenuItem_Click(object sender, EventArgs e): Opens the text file associated with the selected image.
    saveToolStripMenuItem_Click(object sender, EventArgs e): Saves the content of the RichTextBox to the associated text file.
    saveAllToolStripMenuItem_Click(object sender, EventArgs e): Saves the RichTextBox content to all text files in the selected directory.
    editAllToolStripMenuItem_Click(object sender, EventArgs e): Clears the content of all text files in the selected directory.
    refreshToolStripMenuItem_Click(object sender, EventArgs e): Refreshes the TreeView.
    appendAllToolStripMenuItem_Click(object sender, EventArgs e): Appends the RichTextBox content to all text files in the selected directory.
    copyConvertToolStripMenuItem_Click(object sender, EventArgs e): Copies and converts images in the selected directory to PNG format.

Image Handling Methods

    ConvertImagesToPng(string sourceDir, string destDir): Converts images in the source directory to PNG format and saves them in the destination directory.
    DeleteNonPngFiles(string folderPath): Deletes non-PNG files in the specified folder.
    ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format): Converts an image to a Base64 string.

API Methods

    SendApiRequest(string base64Image): Sends an API request with the Base64 image to the specified endpoint.
    SendApiRequestNormal(string base64Image): Sends a normal API request with the Base64 image.

Helper Methods

    SaveRichTextBoxContent(): Saves the content of the RichTextBox to the associated text file.
    OpenTextFileOfSelectedPhoto(): Opens the text file of the selected photo and loads its content into the RichTextBox.
    CancelTaskAndClearLists(): Cancels the current task and clears the lists (not fully implemented in the provided code).

Usage

    Initialize Application: On application start, the form is initialized, configuration is loaded, and directories are loaded into the TreeView.
    Directory Navigation: Expand and select directories in the TreeView to display images in the ListView.
    Context Menu: Right-click on directories or images to access various actions like generating text files, saving content, and converting images.
    RichTextBox Editing: Use the RichTextBox for editing text files associated with images. Use Ctrl+S to save content.
    API Integration: Convert images to Base64 and send API requests for image processing.


### License

This project is licensed under the MIT License https://en.wikipedia.org/wiki/MIT_License
