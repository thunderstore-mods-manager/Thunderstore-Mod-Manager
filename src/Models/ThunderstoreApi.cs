using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Thunderstore.Models
{
    /// <summary>
    /// Thunderstore API client for searching and downloading mods.
    /// thunderstore mod manager download — Thunderstore repo API.
    /// </summary>
    public class ThunderstoreApi
    {
        private const string BaseUrl = "https://thunderstore.io/api/v1";
        private static readonly HttpClient _http = new();

        public record ThunderstoreMod(
            string Name,
            string Owner,
            string PackageName,
            string VersionNumber,
            string Description,
            string DownloadUrl,
            int Downloads,
            string Icon,
            bool IsDeprecated);

        public static async Task<List<ThunderstoreMod>> GetPackagesAsync(string communitySlug)
        {
            var url = $"{BaseUrl}/package/?community={communitySlug}";
            var json = await _http.GetStringAsync(url);
            using var doc = JsonDocument.Parse(json);
            var results = new List<ThunderstoreMod>();
            foreach (var pkg in doc.RootElement.EnumerateArray())
            {
                var latest = pkg.GetProperty("versions")[0];
                results.Add(new ThunderstoreMod(
                    pkg.GetProperty("name").GetString() ?? string.Empty,
                    pkg.GetProperty("owner").GetString() ?? string.Empty,
                    pkg.GetProperty("package_url").GetString() ?? string.Empty,
                    latest.GetProperty("version_number").GetString() ?? string.Empty,
                    latest.GetProperty("description").GetString() ?? string.Empty,
                    latest.GetProperty("download_url").GetString() ?? string.Empty,
                    pkg.GetProperty("total_downloads").GetInt32(),
                    latest.GetProperty("icon").GetString() ?? string.Empty,
                    pkg.GetProperty("is_deprecated").GetBoolean()
                ));
            }
            return results;
        }
    }
}
