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

Bonkers Namespace
=================

Classes:
--------
- Form1: Main form class for the application.

Namespaces Used:
----------------
- System: Basic .NET system functionality.
- System.Diagnostics.Eventing.Reader: Not used in the provided code.
- System.Drawing: For working with images.
- System.IO: For input-output operations.
- System.Linq: For LINQ queries.
- System.Text: For text-related operations.
- System.Windows.Forms: For creating Windows Forms applications.
- System.Net.Http: For HTTP client functionality.
- System.Threading.Tasks: For asynchronous task handling.
- System.Text.Json: For JSON parsing.

Form1 Class
-----------
This class represents the main form of the application.

Fields:
- currentDirectory: Stores the current directory path.
- editingMultipleFiles: Tracks if multiple files are being edited.
- selectedImageTextFile: Stores the selected image text file.
- currentIndex: Tracks the current index in the ListView.

Constructor:
- Form1(): Initializes the form and calls LoadDirectories() to populate the tree view with drive information.

Methods:
- LoadDirectories(): Populates the tree view with drive information and attaches event handlers for tree view actions.
- treeView1_BeforeExpand(): Handles the tree view's BeforeExpand event to load directories when nodes are expanded.
- LoadDirectories(TreeNode node): Loads directories for a specified tree node.
- treeView1_AfterSelect(): Displays images in the selected directory in the ListView.
- treeView1_NodeMouseClick(): Handles right-click on tree view nodes to show a context menu.
- GenerateTxtFilesItem_Click(): Not implemented in the provided code.
- generateTxtFilesToolStripMenuItem_Click(): Generates empty text files for selected images in the tree view.
- openToolStripMenuItem_Click(): Opens the text file associated with the selected image in the ListView.
- saveToolStripMenuItem_Click(): Saves changes made to the text file associated with the selected image.
- saveAllToolStripMenuItem_Click(): Saves changes made to all text files in the selected directory.
- editAllToolStripMenuItem_Click(): Clears the content of all text files in the selected directory.
- listView1_ItemSelectionChanged(): Handles item selection changes in the ListView.
- richTextBox1_KeyDown(): Handles keyboard input events in the RichTextBox.
- SaveRichTextBoxContent(): Saves the content of the RichTextBox to the associated text file.
- OpenTextFileOfSelectedPhoto(): Opens the text file associated with the selected photo.
- refreshToolStripMenuItem_Click(): Refreshes the tree view.
- RefreshTreeView(): Clears and reloads the tree view.
- appendAllToolStripMenuItem_Click(): Appends content from the RichTextBox to all text files in the selected directory.
- copyConvertToolStripMenuItem_Click(): Copies images from a selected directory, converts them to PNG, and saves them in a new directory.
- ConvertImagesToPng(): Converts images to PNG format in a specified directory.
- DeleteNonPngFiles(): Deletes non-PNG files from a specified directory.
- listView1_SelectedIndexChanged(): Handles item selection changes in the ListView.
- deepboruToolStripMenuItem_Click(): Sends an image to an API for captioning and displays the caption in the RichTextBox.
- ImageToBase64(): Converts an image to a Base64 string.
- SendApiRequest(): Sends an API request with an image and parses the response for caption information.
- SendApiRequestNormal(): Sends an API request with an image (without specifying the model) and parses the response for caption information.
- blipToolStripMenuItem_Click(): Sends an image to an API (without specifying the model) for captioning and displays the caption in the RichTextBox.

Usage:
- The Form1 class handles various functionalities related to managing directories, images, text files, and API interactions within the Windows Forms application.


### License

This project is licensed under the MIT License https://en.wikipedia.org/wiki/MIT_License