// thunderstore mod manager — profile system (import/export, r2modman compatibility)
// thunderstore mod manager r2modman profile import
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using YamlDotNet.Serialization;

namespace Thunderstore.Models
{
    public class Profile
    {
        public string Name { get; set; } = string.Empty;
        public string GameId { get; set; } = string.Empty;
        public List<InstalledMod> Mods { get; set; } = new();
    }

    public class InstalledMod
    {
        public string PackageName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
    }

    /// <summary>
    /// Profile serialization — YAML export for modpack sharing, r2modman import.
    /// thunderstore mod manager r2modman compatible profile format.
    /// </summary>
    public static class ProfileSerializer
    {
        public static string ExportYaml(Profile profile)
        {
            var serializer = new SerializerBuilder().Build();
            return serializer.Serialize(profile);
        }

        public static Profile ImportYaml(string yaml)
        {
            var deserializer = new DeserializerBuilder().Build();
            return deserializer.Deserialize<Profile>(yaml);
        }

        public static Profile ImportR2Modman(string profileDir)
        {
            // Reads r2modman's mods.yml format and converts to Thunderstore profile
            var modsFile = Path.Combine(profileDir, "mods.yml");
            if (!File.Exists(modsFile)) throw new FileNotFoundException("r2modman mods.yml not found.");
            return ImportYaml(File.ReadAllText(modsFile));
        }

        public static void SaveProfile(Profile profile, string dir)
        {
            var path = Path.Combine(dir, $"{profile.Name}.yml");
            File.WriteAllText(path, ExportYaml(profile));
        }
    }
}
