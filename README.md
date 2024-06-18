# Bonkers
Bonkers is a simple manual image captioning program designed to streamline the process of adding captions to your images. This application is built using C# and offers a mostly user-friendly interface for organizing and captioning images effectively.
<br>Its called Bonkers because its Bonkers I had to make this.

<video width="320" height="240" controls>
  <source src="https://files.catbox.moe/44btc6.webm" type="video/webm">
  Your browser does not support the video tag.
</video>

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

### License

This project is licensed under the MIT License https://en.wikipedia.org/wiki/MIT_License