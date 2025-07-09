using SerpentModding;

namespace DiscordEmojiDownloader
{
    public partial class TokenMenu : UserControl
    {
        public TokenMenu()
        {
            Logger.Instance.Trace("TokenMenu constructor called");
            InitializeComponent();
        }
        internal static string? TokenValue { get; private set; }
        private Dictionary<string, Dictionary<string, string>> DetectedAccounts { get; set; } = [];

        /// <summary>
        /// Handles the click event for the token button. Depending on the current UI state, this method will:
        /// <list type="bullet">
        /// <item>Validate and set a manually entered token if the manual token textbox is visible.</item>
        /// <item>Set the token from a selected detected account if the account combo box is visible.</item>
        /// <item>Attempt to automatically extract tokens, update the detected accounts, and update the UI accordingly if neither input is visible.</item>
        /// </list>
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private async void ButtonToken_Click(object sender, EventArgs e)
        {
            Logger.Instance.Trace("TokenMenu.ButtonToken_Click() called");
            if (TextboxManualToken.Visible)
            {
                labelInvalidToken.Visible = false;
                var token = TextboxManualToken.Text.Trim();
                Logger.Instance.Debug("TokenMenu: Manual token entry detected.");
                if (TokenFunctions.ValidateToken(token, out _))
                {
                    Logger.Instance.Info("TokenMenu: Manual token validated successfully.");
                    ButtonGetToken.Enabled = false;
                    TextboxManualToken.Enabled = false;
                    SetToken(token);
                    return;
                }
                else
                {
                    Logger.Instance.Warn("TokenMenu: Invalid manual token entered.");
                    labelInvalidToken.Visible = true;
                    labelInvalidToken.Text = "Invalid token. Please try again.";
                    return;
                }
            }
            if (ComboDetectedAccounts.Visible)
            {
                string selectedText = ComboDetectedAccounts.SelectedItem!.ToString()!;
                string uid = selectedText.Split('(')[1].TrimEnd(')');
                Logger.Instance.Info($"TokenMenu: Account selected from ComboBox: {uid}");
                ButtonGetToken.Enabled = false;
                ComboDetectedAccounts.Enabled = false;
                SetToken(DetectedAccounts[uid]["token"]);
                return;
            }

            ButtonGetToken.Enabled = false;
            ProgressIndicatorTokenMenu.Visible = true;
            Logger.Instance.Info("TokenMenu: Attempting to extract tokens automatically.");
            var tokens = await TokenExtractor.GetTokenAsync();
            DetectedAccounts = (Dictionary<string, Dictionary<string, string>>)tokens["accounts"];

            switch (DetectedAccounts.Count)
            {
                case 0:
                    Logger.Instance.Warn("TokenMenu: No tokens found during extraction.");
                    labelInvalidToken.Visible = true;
                    labelInvalidToken.Text = "No tokens found. Please enter a token manually.";
                    ZeroTokens();
                    break;
                case 1:
                    Logger.Instance.Info("TokenMenu: One token found, proceeding automatically.");
                    SetToken(DetectedAccounts.First().Value["token"]);
                    break;
                case > 1:
                    Logger.Instance.Info($"TokenMenu: Multiple tokens found: {DetectedAccounts.Count}");
                    MultipleTokens();
                    break;
            }
            ProgressIndicatorTokenMenu.Visible = false;
        }

        /// <summary>
        /// Updates the UI to prompt the user to manually enter a token when no tokens are detected.
        /// </summary>
        private void ZeroTokens()
        {
            Logger.Instance.Debug("TokenMenu: Switching to manual token entry UI.");
            TextboxManualToken.Visible = true;
            ButtonGetToken.Enabled = true;
            ButtonGetToken.Text = "Confirm";
        }

        /// <summary>
        /// Updates the UI to allow the user to select from multiple detected accounts.
        /// Populates the ComboDetectedAccounts combo box with the display names and user IDs
        /// of all detected accounts, sets the default selection, updates the button text,
        /// and enables the token selection button.
        /// </summary>
        private void MultipleTokens()
        {
            Logger.Instance.Debug("TokenMenu: Populating ComboBox with multiple detected accounts.");
            ComboDetectedAccounts.Visible = true;
            ComboDetectedAccounts.Items.Clear();
            foreach (var account in DetectedAccounts)
            {
                var displayName = account.Value["display_name"];
                var userId = account.Key;
                ComboDetectedAccounts.Items.Add($"{displayName} ({userId})");
            }
            ComboDetectedAccounts.SelectedIndex = 0;
            ButtonGetToken.Text = "Select Account";
            ButtonGetToken.Enabled = true;
        }

        /// <summary>
        /// Sets the provided token as the current token value and navigates to the MainMenu UI control.
        /// </summary>
        /// <param name="token">The Discord token to set as the current value.</param>
        private static void SetToken(string token)
        {
            Logger.Instance.Info("TokenMenu: Token set and switching to MainMenu.");
            TokenValue = token;
            UIController.ShowControl("MainMenu", UIController.TransitionDirection.Right, 690, UIController.EasingMode.EaseInOut);
        }
    }
}
