# B.O.N.K.E.R.S

B: Bitmap<br>
O: Organizer<br>
N: Notation<br>
K: Knowledge-based<br>
E: Editor for<br>
R: Recognizing and<br>
S: Sorting<br>

Is a simple manual image captioning program designed to streamline the process of adding captions to your images. This application is built using C# and offers a mostly user-friendly interface for organizing and captioning images effectively.

## Features

      Configuration Management: Loads and saves API configuration settings from a configuration file (bonkers.cfg), including local and external API endpoints.
      Directory and File Browser: Allows users to browse through directories and display the directory structure in a TreeView component. The TreeView dynamically loads directories as nodes are expanded.
      Image Loading and Display: Displays images from the selected directory in a ListView component. Images are asynchronously loaded and resized for display, with progress indicated by a progress bar.
      Text File Association: Associates each image with a corresponding text file (with the same name but a .txt extension). The program can create these text files, load their contents into a RichTextBox, and save changes back to the text files.
      Text Editing: Allows users to edit the content of the associated text files in a RichTextBox. Supports saving the edited text with visual indicators for changes.
      Batch Operations: Provides options to generate text files for all images in a directory, save all text files, clear the content of all text files, and append text from the RichTextBox to all text files in the directory.
      Image Conversion and Copy: Copies the contents of a selected directory to a new directory with "-conv" appended to the name. Converts all images in the copied directory to PNG format and deletes non-PNG files.
      API Integration: Integrates with an external API to analyze images. Sends base64-encoded images to the API and receives responses containing tags or captions, which are then displayed in the RichTextBox.
      Context Menu: Provides a context menu for TreeView nodes, enabling operations like copying and converting directories or generating text files.
      Progress and Status Indication: Displays progress and status updates using a progress bar and status labels, providing feedback on ongoing operations and API requests.


## running on visual studio
- Windows operating system
- .NET Framework 8 LTS or later
- Visual Studio 2019 or later

**Clone the repository:**

   ```sh
    git clone https://github.com/RegulusAlpha/bonkers.git

    Open the solution:

    Open the Bonkers.sln file in Visual Studio.

    Build the solution:

    Build the solution to restore the necessary NuGet packages and compile the project.

    Run the application:

    Start the application by pressing F5 or selecting Debug > Start Debugging.
   ```
## Usage

    Initialize Application: On application start, the form is initialized, configuration is loaded, and directories are loaded into the TreeView.
    Directory Navigation: Expand and select directories in the TreeView to display images in the ListView.
    Context Menu: Right-click on directories or images to access various actions like generating text files, saving content, and converting images.
    RichTextBox Editing: Use the RichTextBox for editing text files associated with images. Use Ctrl+S to save content.
    API Integration: Convert images to Base64 and send API requests for image processing.

## Contributing

### Contributions are welcome! If you would like to contribute to Bonkers, please follow these steps:

    Fork the repository
    Create a new branch: git checkout -b feature/YourFeature
    Make your changes
    Commit your changes: git commit -m 'Add some feature'
    Push to the branch: git push origin feature/YourFeature
    Submit a pull request

## Documentation



### Class: Form1
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

### Constructor

    Form1(): Initializes the form, loads the configuration, and loads the directories.

### Nested Class: Config

    Represents the configuration settings.
    Properties:
        LocalAPI (string): Local API endpoint.
        ExternalAPI (string): External API endpoint.

### Methods
Configuration Methods

    LoadConfig(): Loads the configuration from a file (bonkers.cfg). If the file doesn't exist, it creates a default configuration.

Directory Loading Methods

    LoadDirectories(): Loads all drives on the system into the TreeView.
    treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e): Handles the BeforeExpand event for the TreeView, loading directories as needed.
    LoadDirectories(TreeNode node): Loads the directories for a given TreeNode.
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

### Event Handlers

    treeView1_AfterSelect(object sender, TreeViewEventArgs e): Handles the AfterSelect event for the TreeView, displaying images in the selected directory.
    treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e): Handles the NodeMouseClick event for the TreeView, showing the context menu.
    listView1_ItemSelectionChanged(Object sender, ListViewItemSelectionChangedEventArgs e): Handles the ItemSelectionChanged event for the ListView, opening the text file of the selected photo.
    richTextBox1_KeyDown(object sender, KeyEventArgs e): Handles the KeyDown event for the RichTextBox, managing shortcut keys for saving and formatting text.

### Context Menu Actions

    generateTxtFilesToolStripMenuItem_Click(object sender, EventArgs e): Generates text files for images in the selected directory.
    openToolStripMenuItem_Click(object sender, EventArgs e): Opens the text file associated with the selected image.
    saveToolStripMenuItem_Click(object sender, EventArgs e): Saves the content of the RichTextBox to the associated text file.
    saveAllToolStripMenuItem_Click(object sender, EventArgs e): Saves the RichTextBox content to all text files in the selected directory.
    editAllToolStripMenuItem_Click(object sender, EventArgs e): Clears the content of all text files in the selected directory.
    refreshToolStripMenuItem_Click(object sender, EventArgs e): Refreshes the TreeView.
    appendAllToolStripMenuItem_Click(object sender, EventArgs e): Appends the RichTextBox content to all text files in the selected directory.
    copyConvertToolStripMenuItem_Click(object sender, EventArgs e): Copies and converts images in the selected directory to PNG format.


### License

This project is licensed under the MIT License https://en.wikipedia.org/wiki/MIT_License
