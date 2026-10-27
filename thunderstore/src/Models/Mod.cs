using System;

namespace Thunderstore.ModManager
{
    public class Mod
    {
        public string Name        { get; set; }
        public string FullName    { get; set; }
        public string Owner       { get; set; }
        public string Description { get; set; }
        public string IconUrl     { get; set; }
        public string WebsiteUrl  { get; set; }
        public string[] Categories { get; set; }
        public int    TotalDownloads { get; set; }
        public int    RatingScore    { get; set; }
        public bool   IsPinned       { get; set; }
        public bool   IsDeprecated   { get; set; }
        public Package[] Versions    { get; set; }

        public class Package
        {
            public string VersionNumber { get; set; }
            public string DownloadUrl   { get; set; }
            public string[] Dependencies { get; set; }
        }
    }
}