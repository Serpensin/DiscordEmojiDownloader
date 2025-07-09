using SerpentModding;
using System.Runtime.InteropServices;

namespace DiscordEmojiDownloader
{
    internal static class Program
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Attach to parent console if not already attached
            EnsureConsoleAttached();

            // Check for --help flag
            if (args.Any(a => a.Equals("--help", StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("DiscordEmojiDownloader - Options:");
                Console.WriteLine("  --log-level=LEVEL   Sets the LogLevel (Trace, Debug, Info, Warn, Error, Fatal)");
                Console.WriteLine("  --help              Shows this help");
                Console.WriteLine();
                Console.WriteLine("Note: If you started this app from a console, close the window yourself after reading this help.");
                Environment.Exit(0);
            }

            // Default log level
            LogLevel logLevel = LogLevel.Info;

            // Parse --log-level= argument if present
            foreach (var arg in args)
            {
                if (arg.StartsWith("--log-level=", StringComparison.OrdinalIgnoreCase))
                {
                    var value = arg[12..];
                    if (Enum.TryParse<LogLevel>(value, true, out var parsedLevel))
                    {
                        logLevel = parsedLevel;
                    }
                }
            }

            Logger.Instance.Initialize(logLevel, logToConsole: true);
            Logger.Instance.Trace($"Program.Main() called with logLevel={logLevel}");
            Logger.Instance.Info("Initializing Logger and application configuration.");
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            ApplicationConfiguration.Initialize();
            Logger.Instance.Info("Starting main application window.");
            Application.Run(new Main());
            Logger.Instance.Info("Application exited.");
        }

        private static void EnsureConsoleAttached()
        {
            if (GetConsoleWindow() == IntPtr.Zero)
            {
                AttachConsole(ATTACH_PARENT_PROCESS);
            }
        }
    }
}