# DiscordEmojiDownloader

DiscordEmojiDownloader is a Windows application that allows you to download all emojis and stickers from a Discord server (guild) using your Discord user token. The app provides a simple graphical interface and supports optional compression of the downloaded files.

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
- `--log-level=LEVEL` : Set the log level (Trace, Debug, Info, Warn, Error, Fatal)

## Disclaimer
- This tool requires your Discord user token. **Never share your token with anyone.**
- Use this tool responsibly and only for servers you have permission to access.
- This project is not affiliated with Discord.

## License
Copyright © 2025 SerpentModding. All rights reserved.
