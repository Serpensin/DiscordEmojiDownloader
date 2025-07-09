using SerpentModding;
using System.IO.Compression;

namespace DiscordEmojiDownloader
{
    /// <summary>
    /// Provides functionality to compress a folder into a ZIP archive.
    /// </summary>
    public class Compressor
    {
        /// <summary>
        /// Compresses the specified folder into a ZIP archive.
        /// </summary>
        /// <param name="folderPath">The full path of the folder to compress.</param>
        /// <param name="deleteSourceAfterSuccess">
        /// If set to <c>true</c>, deletes the source folder after successful compression.
        /// </param>
        /// <exception cref="DirectoryNotFoundException">
        /// Thrown if the specified folder does not exist.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown if compression fails for any reason.
        /// </exception>
        public static void CompressFolder(string folderPath, bool deleteSourceAfterSuccess)
        {
            Logger.Instance.Trace($"Compressor.CompressFolder() called for {folderPath}, deleteSourceAfterSuccess={deleteSourceAfterSuccess}");
            if (!Directory.Exists(folderPath))
            {
                Logger.Instance.Error($"Compressor: Folder not found: {folderPath}");
                throw new DirectoryNotFoundException($"Folder not found: {folderPath}");
            }

            string parentDir = Path.GetDirectoryName(folderPath)!;
            string folderName = Path.GetFileName(folderPath);
            string archivePath = Path.Combine(parentDir, folderName + ".zip");

            try
            {
                Logger.Instance.Info($"Compressor: Creating archive {archivePath} from {folderPath}");
                ZipFile.CreateFromDirectory(folderPath, archivePath, CompressionLevel.Optimal, includeBaseDirectory: true);

                if (deleteSourceAfterSuccess)
                {
                    Logger.Instance.Debug($"Compressor: Deleting source folder {folderPath}");
                    Directory.Delete(folderPath, true);
                }
                Logger.Instance.Info($"Compressor: Compression successful: {archivePath}");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error($"Compressor: Compression failed: {ex.Message}");
                throw new Exception("Compression failed: " + ex.Message, ex);
            }
        }
    }
}
