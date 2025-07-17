# DiscordEmojiDownloader

DiscordEmojiDownloader is a Windows application that allows you to download all emojis and stickers from a Discord server (guild) using your Discord user token. The app provides a simple graphical interface and supports optional compression of the downloaded files.

<blockquote style="border-left: 5px solid red; padding: 1em; background-color: #ffe5e5;">
<b>⚠️ Disclaimer</b><br><br>
This tool requires your Discord user token. <b>Never share your token with anyone.</b><br>
Use this tool responsibly and only for servers you have permission to access.<br>
This project is not affiliated with Discord.
</blockquote>

## Features
- Download all custom emojis and stickers from any Discord server you have access to
- Supports both static and animated emojis and stickers
- Automatically scans your PC for all available Discord accounts/tokens
- Lets you select from all detected accounts, or enter a token manually if none are found
- Optionally compress the downloaded files into a ZIP archive
- Modern Windows Forms UI
- Logging and progress indication

## Requirements
- Windows 10 or later
- .NET 9.0 Desktop Runtime (if not bundled)

## Dependencies
This project uses the [SerpentModding.CustomCSharpModules](https://www.nuget.org/packages/SerpentModding.CustomCSharpModules) library, available directly from NuGet.

## Building from Source

This project is developed using **Visual Studio 2022 or later** and targets **.NET 9.0**. The required dependency is available via NuGet:

### Prerequisites
- Windows 10 or later
- Visual Studio 2022+ with the **.NET 9.0 Desktop Development** workload
- Optionally: Git

### Steps to Build

1. **Clone this repository**:`git clone https://github.com/Serpensin/DiscordEmojiDownloader.git`
2. **Restore NuGet packages**
   - Visual Studio will automatically restore the required NuGet package `SerpentModding.CustomCSharpModules` on build.
   - If not, right-click the solution and select **Restore NuGet Packages**.

3. **Open the solution in Visual Studio**

4. **Publish the application**  
   You can publish the app in two modes:

   ### 🔹 Option A: Self-contained (default)
   - Includes the .NET runtime
   - Can run on systems **without .NET 9.0 installed**
   - Larger file size (~50–120 MB - Depending if compression is enabled)

   **Steps:**
   - Right-click the project in Solution Explorer → **Publish**
   - Target: **Folder**
   - In **Settings**:
     - Deployment mode: `Self-contained`
     - Target runtime: `win-x64`
     - Check **Produce single file**
   - Click **Publish**

   Output: a single `.exe` that works standalone on any compatible Windows system.

   ### 🔹 Option B: Framework-dependent
   - Requires .NET 9.0 Desktop Runtime to be installed on the target system
   - Much smaller file size (~7MB)
   - Easier to distribute if .NET is already available

   **Steps:**
   - Same as above, but in **Settings**:
     - Deployment mode: `Framework-dependent`
     - Target runtime: `win-x64`
     - Check **Produce single file** if desired

   Output: lightweight executable, but **.NET 9.0 must be installed** on the target PC.

   **Summary:**

   | Mode                  | Requires .NET Installed | File Size     | Portable? |
   |-----------------------|--------------------------|----------------|-----------|
   | Self-contained        | ❌ No                    | Large (~50–120MB) | ✅ Yes    |
   | Framework-dependent   | ✅ Yes                   | Small (~7MB)  | ⚠️ Only on systems with .NET 9 |

## How to Use

1. **Start the application**
   - Double-click the executable or run it from the command line.

2. **Token detection**
   - The app will automatically scan your PC for all available Discord tokens/accounts.
   - If multiple accounts are found, you can select which one to use.
   - If no token is found, you can enter your Discord user token manually.

3. **Select a download folder**
   - Choose the root folder where the server's emojis and stickers will be saved.

4. **Enter the Guild (Server) ID**
   - Enter the numeric ID of the Discord server you want to download from. The app checks if the ID is plausible.

5. **Start the download**
   - Click the download button. The app will fetch all emojis and stickers and save them in subfolders.
   - Optionally, enable the compression checkbox to create a ZIP archive of the downloaded files.

6. **Find your files**
   - The files will be saved in the selected folder, organized by server name and type (Emojis/Stickers).

## Command Line Options

- `--help` : Show help and exit  
- `--log-level=LEVEL` : Set the log level (`Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`)

## License

Copyright © 2025 SerpentModding. All rights reserved.
