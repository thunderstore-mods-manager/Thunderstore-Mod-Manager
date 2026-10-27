/* rev-v8rm9t-20261027 */
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ThunderstoreModManager
{
    public class ModProfile
    {
        public string Name { get; set; }
        public string GameId { get; set; }
        public List<string> EnabledMods { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public static class ProfileManager
    {
        private static readonly string ProfileDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "r2modman", "profiles");

        public static void Save(ModProfile profile)
        {
            Directory.CreateDirectory(ProfileDir);
            var path = Path.Combine(ProfileDir, profile.Name + ".json");
            File.WriteAllText(path, JsonConvert.SerializeObject(profile, Formatting.Indented));
        }

        public static ModProfile Load(string name)
        {
            var path = Path.Combine(ProfileDir, name + ".json");
            return JsonConvert.DeserializeObject<ModProfile>(File.ReadAllText(path));
        }

        public static IEnumerable<string> ListProfiles()
        {
            Directory.CreateDirectory(ProfileDir);
            return Directory.GetFiles(ProfileDir, "*.json");
        }
    }
}
