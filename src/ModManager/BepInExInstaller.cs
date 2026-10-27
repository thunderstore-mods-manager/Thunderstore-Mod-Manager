// thunderstore mod manager — BepInEx auto-install and management
// thunderstore mod manager lethal company, valheim, peak, ultrakill
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Thunderstore.ModManager
{
    /// <summary>
    /// Handles BepInEx download and installation for each supported game.
    /// thunderstore mod manager without overwolf — BepInEx management.
    /// </summary>
    public class BepInExInstaller
    {
        private const string BepInExApiUrl =
            "https://api.github.com/repos/BepInEx/BepInEx/releases/latest";

        public static async Task<string> GetLatestBepInExUrlAsync(bool x64)
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.UserAgent.ParseAdd("ThunderstoreModManager");
            var json = await http.GetStringAsync(BepInExApiUrl);
            // Parse and return the appropriate asset URL
            var arch = x64 ? "x64" : "x86";
            return $"https://github.com/BepInEx/BepInEx/releases/latest/download/BepInEx_win_{arch}_latest.zip";
        }

        public static async Task InstallAsync(string gameDirectory, IProgress<int> progress)
        {
            var is64Bit = Is64BitGame(gameDirectory);
            var url = await GetLatestBepInExUrlAsync(is64Bit);
            var tmp = Path.GetTempFileName();

            await DownloadAsync(url, tmp, progress);
            System.IO.Compression.ZipFile.ExtractToDirectory(tmp, gameDirectory, overwriteFiles: true);
            File.Delete(tmp);

            // Create BepInEx plugin folder if it doesn't exist
            Directory.CreateDirectory(Path.Combine(gameDirectory, "BepInEx", "plugins"));
            Directory.CreateDirectory(Path.Combine(gameDirectory, "BepInEx", "config"));
        }

        public static bool IsInstalled(string gameDirectory)
            => File.Exists(Path.Combine(gameDirectory, "winhttp.dll"))
               || File.Exists(Path.Combine(gameDirectory, "BepInEx", "core", "BepInEx.Core.dll"));

        private static bool Is64BitGame(string dir)
        {
            foreach (var exe in Directory.GetFiles(dir, "*.exe"))
            {
                try
                {
                    using var fs = File.OpenRead(exe);
                    using var br = new BinaryReader(fs);
                    br.BaseStream.Seek(0x3C, SeekOrigin.Begin);
                    var peOffset = br.ReadInt32();
                    br.BaseStream.Seek(peOffset + 4, SeekOrigin.Begin);
                    var machine = br.ReadUInt16();
                    return machine == 0x8664; // AMD64
                }
                catch { }
            }
            return true;
        }

        private static async Task DownloadAsync(string url, string dest, IProgress<int> progress)
        {
            using var http = new HttpClient();
            var response = await http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            var total = response.Content.Headers.ContentLength ?? 1L;
            await using var stream = await response.Content.ReadAsStreamAsync();
            await using var file = File.Create(dest);
            var buf = new byte[65536];
            long downloaded = 0;
            int read;
            while ((read = await stream.ReadAsync(buf)) > 0)
            {
                await file.WriteAsync(buf.AsMemory(0, read));
                downloaded += read;
                progress.Report((int)(downloaded * 100 / total));
            }
        }
    }
}
