using Newtonsoft.Json.Linq;
using SerpentModding;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;

namespace DiscordEmojiDownloader
{
    public class EmojiDownloader(Action<int, int> updateProgress)
    {
        public static int TotalFiles { get; private set; } = 0;
        public static string GuildName { get; private set; } = string.Empty;

        private static readonly string userToken = TokenMenu.TokenValue!;
        private static readonly string folder = MainMenu.DownloadFolderRoot;

        private readonly Action<int, int> updateProgress = updateProgress;
        private string serverName = string.Empty;
        private static readonly HttpClient _httpClient = new();
        private JArray emojis = [];
        private JArray stickers = [];
        private static readonly char[] DisallowedCharacters = ['<', '>', ':', '"', '/', '\\', '|', '?', '*'];

        /// <summary>
        /// Downloads all emojis and stickers for the specified Discord guild (server) and saves them to the local file system.
        /// </summary>
        /// <param name="guildId">The unique identifier of the Discord guild (server) to download content from.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains the total number of emojis and stickers downloaded.
        /// </returns>
        /// <remarks>
        /// This method fetches the guild's emojis and stickers, creates the necessary folders, and downloads each item.
        /// The <see cref="updateProgress"/> callback is invoked after each download to report progress.
        /// </remarks>
        public async Task<int> DownloadContentAsync(string guildId)
        {
            Logger.Instance.Trace($"EmojiDownloader.DownloadContentAsync() called for guildId={guildId}");
            await FetchGuildDataAsync(guildId);

            int total = (emojis?.Count ?? 0) + (stickers?.Count ?? 0);
            int count = 0;

            if (emojis?.Count > 0) CreateFolder("Emojis");
            if (stickers?.Count > 0) CreateFolder("Stickers");

            if (emojis != null)
            {
                foreach (var emoji in emojis)
                {
                    await DownloadEmojiAsync(emoji);
                    count++;
                    updateProgress?.Invoke(count, total);
                }
            }

            if (stickers != null)
            {
                foreach (var sticker in stickers)
                {
                    await DownloadStickerAsync(sticker);
                    count++;
                    updateProgress?.Invoke(count, total);
                }
            }

            Logger.Instance.Info($"EmojiDownloader: Downloaded {count} items for guild {guildId}.");
            return count;
        }

        /// <summary>
        /// Fetches guild data, including emojis and stickers, from the Discord API for the specified guild ID.
        /// </summary>
        /// <param name="guildId">The unique identifier of the Discord guild (server) to fetch data for.</param>
        /// <exception cref="Exception">
        /// Thrown if the guild cannot be accessed (e.g., due to missing access or the guild not being found),
        /// or if no emojis or stickers are found in the guild.
        /// </exception>
        /// <remarks>
        /// This method updates the <see cref="serverName"/>, <see cref="GuildName"/>, <see cref="emojis"/>, <see cref="stickers"/>, and <see cref="TotalFiles"/> fields.
        /// It uses a new <see cref="HttpClient"/> instance for each call and sets the required authorization header.
        /// </remarks>
        private async Task FetchGuildDataAsync(string guildId)
        {
            Logger.Instance.Trace($"EmojiDownloader.FetchGuildDataAsync() called for guildId={guildId}");
            string baseUrl = $"https://discord.com/api/v10/guilds/{guildId}";
            string emojiUrl = $"{baseUrl}/emojis";
            string stickerUrl = $"{baseUrl}/stickers";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", userToken);

            var guildResponse = await client.GetAsync(baseUrl);
            if (!guildResponse.IsSuccessStatusCode)
            {
                Logger.Instance.Error($"EmojiDownloader: Guild not found or missing access for {guildId}");
                throw new Exception("Missing Access or Guild not found");
            }

            var guildJson = JObject.Parse(await guildResponse.Content.ReadAsStringAsync());
            serverName = guildJson.Value<string>("name") ?? "UnknownServer";
            GuildName = serverName;

            var emojiResponse = await client.GetAsync(emojiUrl);
            emojis = emojiResponse.IsSuccessStatusCode
                ? JArray.Parse(await emojiResponse.Content.ReadAsStringAsync())
                : [];

            var stickerResponse = await client.GetAsync(stickerUrl);
            stickers = stickerResponse.IsSuccessStatusCode
                ? JArray.Parse(await stickerResponse.Content.ReadAsStringAsync())
                : [];
            TotalFiles = emojis.Count + stickers.Count;

            if (emojis.Count == 0 && stickers.Count == 0)
            {
                Logger.Instance.Warn($"EmojiDownloader: No emojis or stickers found for guild {guildId}");
                throw new Exception("No Emojis or Stickers found");
            }
            Logger.Instance.Info($"EmojiDownloader: Found {emojis.Count} emojis and {stickers.Count} stickers for guild {guildId}");
        }

        /// <summary>
        /// Downloads an emoji from Discord and saves it to the local file system.
        /// </summary>
        /// <param name="emoji">A <see cref="JToken"/> representing the emoji object from the Discord API.</param>
        /// <remarks>
        /// The method determines if the emoji is animated and downloads it as either a GIF or PNG.
        /// Any <see cref="HttpRequestException"/> encountered during the download is silently ignored.
        /// </remarks>
        private async Task DownloadEmojiAsync(JToken emoji)
        {
            string emojiId = emoji.Value<string>("id")!;
            string name = SanitizeFileName(emoji.Value<string>("name") ?? emojiId);
            bool animated = emoji.Value<bool?>("animated") ?? false;
            string ext = animated ? "gif" : "png";
            string url = $"https://cdn.discordapp.com/emojis/{emojiId}.{ext}";
            string savePath = Path.Combine(folder, serverName, "Emojis", $"{name}.{ext}");

            try
            {
                Logger.Instance.Debug($"EmojiDownloader: Downloading emoji {name} ({emojiId})");
                await DownloadFileAsync(url, savePath);
            }
            catch (HttpRequestException ex)
            {
                Logger.Instance.Warn($"EmojiDownloader: Failed to download emoji {name}: {ex.Message}");
            }
        }

        /// <summary>
        /// Downloads a sticker from Discord and saves it to the local file system.
        /// </summary>
        /// <param name="sticker">A <see cref="JToken"/> representing the sticker object from the Discord API.</param>
        /// <remarks>
        /// The method determines the sticker's format and downloads it as either PNG, GIF, or BIN.
        /// If the sticker is in APNG format (format_type == 2), it will be converted to GIF after download.
        /// Any exceptions during download or conversion are silently ignored.
        /// </remarks>
        private async Task DownloadStickerAsync(JToken sticker)
        {
            string id = sticker.Value<string>("id")!;
            int formatType = sticker.Value<int>("format_type");
            string name = SanitizeFileName(sticker.Value<string>("name") ?? id);

            string ext = formatType switch
            {
                1 or 2 => "png",
                4 => "gif",
                _ => "bin"
            };

            string baseUrl = formatType == 4 ? "https://media.discordapp.net" : "https://cdn.discordapp.com";
            string url = $"{baseUrl}/stickers/{id}.{ext}";

            string savePath = Path.Combine(folder, serverName, "Stickers", $"{name}.{ext}");

            try
            {
                Logger.Instance.Debug($"EmojiDownloader: Downloading sticker {name} ({id})");
                await DownloadFileAsync(url, savePath);

                if (formatType == 2)
                {
                    Logger.Instance.Debug($"EmojiDownloader: Converting APNG sticker {name} to GIF");
                    await ConvertApngToGifAsync(savePath);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Warn($"EmojiDownloader: Failed to download or convert sticker {name}: {ex.Message}");
            }
        }

        /// <summary>
        /// Downloads a file asynchronously from the specified URL and saves it to the given path.
        /// </summary>
        /// <param name="url">The URL of the file to download.</param>
        /// <param name="path">The local file path where the downloaded file will be saved.</param>
        /// <remarks>
        /// The method creates the directory for the file if it does not exist.
        /// Any <see cref="HttpRequestException"/> or <see cref="IOException"/> encountered during the download is silently ignored.
        /// </remarks>
        private static async Task DownloadFileAsync(string url, string path)
        {
            try
            {
                Logger.Instance.Debug($"EmojiDownloader: Downloading file from {url} to {path}");
                using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                await using var stream = await response.Content.ReadAsStreamAsync();
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                await using var file = File.Create(path);
                await stream.CopyToAsync(file);
                Logger.Instance.Debug($"EmojiDownloader: File saved to {path}");
            }
            catch (HttpRequestException ex)
            {
                Logger.Instance.Warn($"EmojiDownloader: HTTP error downloading {url}: {ex.Message}");
            }
            catch (IOException ex)
            {
                Logger.Instance.Warn($"EmojiDownloader: IO error saving {path}: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a directory for the specified subfolder within the current server's folder.
        /// </summary>
        /// <param name="subfolder">The name of the subfolder to create (e.g., "Emojis" or "Stickers").</param>
        private void CreateFolder(string subfolder)
        {
            Logger.Instance.Debug($"EmojiDownloader: Creating folder {subfolder}");
            Directory.CreateDirectory(Path.Combine(folder, serverName, subfolder));
        }

        /// <summary>
        /// Removes disallowed characters from a file name to ensure it is valid for use in the file system.
        /// </summary>
        /// <param name="name">The original file name to sanitize.</param>
        /// <returns>A sanitized file name with disallowed characters removed.</returns>
        private static string SanitizeFileName(string name)
        {
            foreach (char c in DisallowedCharacters)
                name = name.Replace(c.ToString(), "");
            return name;
        }

        /// <summary>
        /// Converts an APNG file to a GIF file asynchronously.
        /// </summary>
        /// <param name="path">The file path of the APNG image to convert. The resulting GIF will be saved with the same name and a .gif extension.</param>
        /// <remarks>
        /// If the conversion is successful, the original APNG file is deleted. Any exceptions during conversion are silently ignored.
        /// </remarks>
        private static async Task ConvertApngToGifAsync(string path)
        {
            string gifPath = Path.ChangeExtension(path, ".gif");
            try
            {
                Logger.Instance.Debug($"EmojiDownloader: Converting {path} to GIF");
                using var image = await Image.LoadAsync<Rgba32>(path);

                var meta = image.Metadata.GetGifMetadata();
                meta.RepeatCount = 0;

                using var gif = File.Create(gifPath);
                await image.SaveAsGifAsync(gif);

                File.Delete(path);
                Logger.Instance.Debug($"EmojiDownloader: Conversion to GIF successful, deleted original APNG {path}");
            }
            catch (Exception ex)
            {
                Logger.Instance.Warn($"EmojiDownloader: Failed to convert {path} to GIF: {ex.Message}");
            }
        }
    }
}
