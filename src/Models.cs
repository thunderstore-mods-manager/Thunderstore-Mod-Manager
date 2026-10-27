/* rev-v8rm9t-20261027 */
using System.Collections.Generic;

namespace ThunderstoreModManager
{
    public class ThunderstoreMod
    {
        public string Name        { get; set; }
        public string Author      { get; set; }
        public string Version     { get; set; }
        public string Description { get; set; }
        public string DownloadUrl { get; set; }
        public long   Downloads   { get; set; }
        public string[] Dependencies { get; set; }
        public bool   IsDeprecated { get; set; }
    }

    public class ModIndex
    {
        public string GameId          { get; set; }
        public long   LastUpdated     { get; set; }
        public List<ThunderstoreMod> Mods { get; set; } = new();
    }
}
