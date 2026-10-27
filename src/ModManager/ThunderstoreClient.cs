using System;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Thunderstore.ModManager
{
    public class ThunderstoreClient
    {
        private readonly HttpClient _http = new HttpClient();
        private const string API = "https://thunderstore.io/api/v1";

        public async Task<List<Mod>> SearchAsync(string game, string query = "")
        {
            var url = $"{API}/package/?game={Uri.EscapeDataString(game)}";
            if (!string.IsNullOrEmpty(query))
                url += $"&q={Uri.EscapeDataString(query)}";

            var json = await _http.GetStringAsync(url);
            var results = JsonSerializer.Deserialize<List<Mod>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return results ?? new List<Mod>();
        }

        public async Task<byte[]> DownloadAsync(string downloadUrl)
        {
            return await _http.GetByteArrayAsync(downloadUrl);
        }
    }
}