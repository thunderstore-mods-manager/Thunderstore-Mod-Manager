/* rev-v8rm9t-20261027 */
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ThunderstoreModManager
{
    public class ModDownloader
    {
        private static readonly HttpClient Http = new HttpClient();

        public async Task DownloadAsync(string url, string dest, IProgress<double> progress = null)
        {
            using var res = await Http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            res.EnsureSuccessStatusCode();
            var total = res.Content.Headers.ContentLength ?? -1L;
            using var src = await res.Content.ReadAsStreamAsync();
            using var dst = File.Create(dest);
            var buf = new byte[65536];
            long got = 0; int n;
            while ((n = await src.ReadAsync(buf, 0, buf.Length)) > 0)
            {
                await dst.WriteAsync(buf, 0, n);
                got += n;
                if (total > 0) progress?.Report((double)got / total);
            }
        }
    }
}
