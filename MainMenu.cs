using SerpentModding;

namespace DiscordEmojiDownloader
{
    public partial class MainMenu : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenu"/> class.
        /// Sets up the UI components and initializes the <see cref="EmojiDownloader"/> with a progress update callback.
        /// </summary>
        public MainMenu()
        {
            Logger.Instance.Trace("MainMenu constructor called");
            InitializeComponent();
            downloader = new EmojiDownloader(UpdateProgress);
            kitt = new KITTScanner(ProgressBarMainMenu);
        }

        private readonly EmojiDownloader downloader;
        private readonly KITTScanner kitt;
        internal static string DownloadFolderRoot { get; private set; } = string.Empty;

        /// <summary>
        /// Handles the selection of the root folder for downloads by displaying a <see cref="FolderBrowserDialog"/>.
        /// Sets the <see cref="DownloadFolderRoot"/> property to the selected path if the user confirms the dialog.
        /// If the user cancels the dialog, the application exits.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        private void SelectDownloadFolderRoot(object sender, EventArgs e)
        {
            Logger.Instance.Trace("MainMenu.SelectDownloadFolderRoot() called");
            if (DesignMode) return;

            using var folderBrowser = new FolderBrowserDialog
            {
                Description = "Select the folder for downloads. A subfolder is automatically created.",
                UseDescriptionForTitle = true
            };

            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                Logger.Instance.Info($"MainMenu: Download folder selected: {folderBrowser.SelectedPath}");
                DownloadFolderRoot = folderBrowser.SelectedPath;
            }
            else
            {
                Logger.Instance.Warn("MainMenu: Download folder selection cancelled, exiting application.");
                Application.Exit();
            }
        }

        /// <summary>
        /// Determines whether the specified input string is likely a valid Discord Guild ID (Snowflake).
        /// Checks if the input is a valid unsigned long, decodes the Discord snowflake timestamp,
        /// and verifies that the creation date falls within a plausible range for Discord guilds.
        /// </summary>
        /// <param name="input">The input string to validate as a Discord Guild ID.</param>
        /// <returns>
        /// <c>true</c> if the input is a valid snowflake and the decoded creation date is between January 1, 2015 and now; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsLikelyGuildId(string input)
        {
            Logger.Instance.Trace("MainMenu.IsLikelyGuildId() called");
            const long discordEpoch = 1420070400000L;
            if (!ulong.TryParse(input, out var snowflake))
            {
                Logger.Instance.Debug("MainMenu: Guild ID is not a valid ulong.");
                return false;
            }

            long timestamp = (long)(snowflake >> 22) + discordEpoch;
            var created = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;

            bool plausible = created >= new DateTime(2015, 1, 1) && created <= DateTime.Now;
            Logger.Instance.Debug($"MainMenu: Guild ID plausible={plausible}, created={created}");
            return plausible;
        }

        /// <summary>
        /// Updates the progress bar on the main menu UI to reflect the current download progress.
        /// Ensures thread-safe updates by invoking on the UI thread if required.
        /// </summary>
        /// <param name="current">The current number of items downloaded.</param>
        /// <param name="total">The total number of items to download.</param>
        private void UpdateProgress(int current, int total)
        {
            Logger.Instance.Trace($"MainMenu.UpdateProgress() called: {current}/{total}");
            void UpdateUI()
            {
                ProgressBarMainMenu.Maximum = total;

                if (current == total)
                {
                    ProgressBarMainMenu.Value = 0;
                    ProgressBarMainMenu.Text = $"Downloaded {total} items.";
                }
                else
                {
                    ProgressBarMainMenu.Value = current;
                    ProgressBarMainMenu.Text = $"Downloading... {current}/{total}";
                }
            }

            if (ProgressBarMainMenu.InvokeRequired)
                ProgressBarMainMenu.Invoke((Action)UpdateUI);
            else
                UpdateUI();
        }

        /// <summary>
        /// Sets the foreground color and text of the main menu progress bar,
        /// ensuring the update is performed on the UI thread.
        /// </summary>
        /// <param name="color">The color to set for the progress bar's foreground.</param>
        /// <param name="text">The text to display on the progress bar.</param>
        private void SetProgressBarState(Color color, string text)
        {
            Logger.Instance.Trace($"MainMenu.SetProgressBarState() called: color={color}, text={text}");
            void Update()
            {
                ProgressBarMainMenu.ForeColor = color;
                ProgressBarMainMenu.Text = text;
            }

            if (ProgressBarMainMenu.InvokeRequired)
                ProgressBarMainMenu.Invoke((Action)Update);
            else
                Update();
        }

        /// <summary>
        /// Handles the click event for the Download button.
        /// Validates the entered Guild ID, disables relevant UI controls, and initiates the download process.
        /// If the download is successful and compression is selected, compresses the downloaded files.
        /// Updates the progress bar and UI state throughout the process, and handles any errors that occur.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> containing event data.</param>
        private async void ButtonDownload_Click(object sender, EventArgs e)
        {
            Logger.Instance.Trace("MainMenu.ButtonDownload_Click() called");
            string guildId = TextboxGuildID.Text.Trim();

            if (!IsLikelyGuildId(guildId))
            {
                Logger.Instance.Warn("MainMenu: Invalid Guild ID entered.");
                SetProgressBarState(Color.Red, "Invalid Guild ID. Please enter a valid ID.");
                return;
            }

            TextboxGuildID.Enabled = false;
            ButtonDownload.Enabled = false;
            CheckBoxCompress.Enabled = false;
            SetProgressBarState(Color.FromArgb(230, 230, 230), string.Empty);
            ProgressIndicatorMainMenu.Visible = true;
            UIController.SetTemporaryTitle(EmojiDownloader.GuildName);

            int downloadedFiles = 0;
            bool compressChecked = CheckBoxCompress.Checked;
            string folderPath = string.Empty;

            try
            {
                Logger.Instance.Info($"MainMenu: Starting download for guild {guildId}");
                int count = await downloader.DownloadContentAsync(guildId).ConfigureAwait(true);
                UpdateProgress(count, count);
                downloadedFiles = EmojiDownloader.TotalFiles;
                if (downloadedFiles != 0 && compressChecked)
                {
                    ProgressBarMainMenu.Text = "Compressing downloaded files...";
                    kitt.Start();
                    folderPath = Path.Combine(DownloadFolderRoot, EmojiDownloader.GuildName);
                    try
                    {
                        Logger.Instance.Info($"MainMenu: Compressing folder {folderPath}");
                        Compressor.CompressFolder(folderPath, true);
                        SetProgressBarState(Color.Green, "Download and compression finished.");
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.Error($"MainMenu: Compression failed: {ex.Message}");
                        SetProgressBarState(Color.Red, $"Compression failed: {ex.Message}");
                    }
                    finally
                    {
                        kitt.Stop();
                    }
                }
                else if (downloadedFiles != 0)
                {
                    SetProgressBarState(Color.Green, $"Downloaded {downloadedFiles} items.");
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error($"MainMenu: Download failed: {ex.Message}");
                SetProgressBarState(Color.Red, $"Error: {ex.Message}");
            }
            finally
            {
                ButtonDownload.Enabled = true;
                ProgressBarMainMenu.Value = 0;
                ProgressIndicatorMainMenu.Visible = false;
                CheckBoxCompress.Enabled = true;
                TextboxGuildID.Enabled = true;
                UIController.ResetTitle();
            }
        }

        /// <summary>
        /// Handles the KeyPress event for the guild ID textbox,
        /// allowing only control characters and digits to be entered.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The KeyPressEventArgs containing event data.</param>
        private void TextboxGuildID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                Logger.Instance.Debug("MainMenu: Non-digit key pressed in GuildID textbox.");
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the guild ID textbox.
        /// Filters the input to keep only digit characters,
        /// and moves the cursor to the end of the text.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The EventArgs containing event data.</param>
        private void TextboxGuildID_TextChanged(object sender, EventArgs e)
        {
            TextboxGuildID.Text = new string(TextboxGuildID.Text.Where(char.IsDigit).ToArray());
            TextboxGuildID.SelectionStart = TextboxGuildID.Text.Length;
        }
    }
}
