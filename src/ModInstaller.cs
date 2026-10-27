/* rev-v8rm9t-20261027 */
using System;
using System.IO;
using System.IO.Compression;

namespace ThunderstoreModManager
{
    public static class ModInstaller
    {
        public static void Install(string zipPath, string modsDir)
        {
            Directory.CreateDirectory(modsDir);
            using var archive = ZipFile.OpenRead(zipPath);
            foreach (var entry in archive.Entries)
            {
                if (string.IsNullOrEmpty(entry.Name)) continue;
                var dest = Path.Combine(modsDir, entry.FullName);
                Directory.CreateDirectory(Path.GetDirectoryName(dest)!);
                entry.ExtractToFile(dest, overwrite: true);
            }
        }

        public static void Uninstall(string modName, string modsDir)
        {
            var dir = Path.Combine(modsDir, modName);
            if (Directory.Exists(dir)) Directory.Delete(dir, true);
            var dll = Path.Combine(modsDir, modName + ".dll");
            if (File.Exists(dll)) File.Delete(dll);
        }
    }
}
