using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Thunderstore.ModManager
{
    public class Profile
    {
        public string Name   { get; set; }
        public string Game   { get; set; }
        public List<string> EnabledMods { get; set; } = new();

        public static Profile Load(string path)
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Profile>(json)
                   ?? throw new InvalidOperationException("Invalid profile file");
        }

        public void Save(string path)
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}