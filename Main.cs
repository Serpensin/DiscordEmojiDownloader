using SerpentModding;

namespace DiscordEmojiDownloader
{
    public partial class Main : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Main"/> form.
        /// Sets up UI controls, window title, and icon, and displays the initial menu.
        /// </summary>
        public Main()
        {
            Logger.Instance.Trace("Main constructor called");
            InitializeComponent();

            Logger.Instance.Debug("Initializing UIController and main window properties.");
            UIController.Init(this);
            UIController.SetWindowTitle("Discord Emoji Downloader", remember: true);
            UIController.SetWindowIcon(Properties.Resources.icon_ico);

            UIController.RegisterControl("TokenMenu", tokenMenu);
            UIController.RegisterControl("MainMenu", mainMenu);

            Logger.Instance.Info("Showing TokenMenu as initial control.");
            UIController.ShowControl("TokenMenu");
        }
    }
}
