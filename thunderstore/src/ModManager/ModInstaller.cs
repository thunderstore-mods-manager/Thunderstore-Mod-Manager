using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Thunderstore.ModManager
{
    public class ModInstaller
    {
        private readonly string _profileDir;

        public ModInstaller(string profileDir)
        {
            _profileDir = profileDir;
            Directory.CreateDirectory(_profileDir);
        }

        public async Task InstallAsync(Mod mod, byte[] zipBytes, IProgress<string> log = null)
        {
            string dest = Path.Combine(_profileDir, mod.FullName);
            log?.Report($"Installing {mod.FullName}...");

            await Task.Run(() =>
            {
                using var ms      = new MemoryStream(zipBytes);
                using var archive = new ZipArchive(ms, ZipArchiveMode.Read);
                foreach (var entry in archive.Entries)
                {
                    if (string.IsNullOrEmpty(entry.Name)) continue;
                    string entryDest = Path.Combine(dest, entry.FullName);
                    Directory.CreateDirectory(Path.GetDirectoryName(entryDest)!);
                    entry.ExtractToFile(entryDest, overwrite: true);
                }
            });

            log?.Report($"{mod.FullName} installed to {dest}");
        }

        public void Uninstall(string modFullName)
        {
            string path = Path.Combine(_profileDir, modFullName);
            if (Directory.Exists(path))
                Directory.Delete(path, recursive: true);
        }

        public bool IsInstalled(string modFullName) =>
            Directory.Exists(Path.Combine(_profileDir, modFullName));
    }
}