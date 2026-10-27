using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Thunderstore.ModManager
{
    /// <summary>
    /// Downloads and installs individual Thunderstore mods into a game's BepInEx plugins folder.
    /// thunderstore mod manager download — one-click mod install.
    /// </summary>
    public class ModInstaller
    {
        private readonly string _gameDirectory;

        public ModInstaller(string gameDirectory)
        {
            _gameDirectory = gameDirectory;
        }

        public async Task InstallModAsync(string downloadUrl, string modName, IProgress<int> progress)
        {
            var tmp = Path.GetTempFileName();
            try
            {
                await DownloadAsync(downloadUrl, tmp, progress);
                var pluginsDir = Path.Combine(_gameDirectory, "BepInEx", "plugins", modName);
                Directory.CreateDirectory(pluginsDir);
                System.IO.Compression.ZipFile.ExtractToDirectory(tmp, pluginsDir, overwriteFiles: true);
            }
            finally
            {
                if (File.Exists(tmp)) File.Delete(tmp);
            }
        }

        public void UninstallMod(string modName)
        {
            var dir = Path.Combine(_gameDirectory, "BepInEx", "plugins", modName);
            if (Directory.Exists(dir)) Directory.Delete(dir, recursive: true);
        }

        public void EnableMod(string modName, bool enable)
        {
            var dir = Path.Combine(_gameDirectory, "BepInEx", "plugins", modName);
            var disabled = dir + ".disabled";
            if (enable && Directory.Exists(disabled))
                Directory.Move(disabled, dir);
            else if (!enable && Directory.Exists(dir))
                Directory.Move(dir, disabled);
        }

        private static async Task DownloadAsync(string url, string dest, IProgress<int> progress)
        {
            using var http = new HttpClient();
            var resp = await http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            var total = resp.Content.Headers.ContentLength ?? 1L;
            await using var stream = await resp.Content.ReadAsStreamAsync();
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
