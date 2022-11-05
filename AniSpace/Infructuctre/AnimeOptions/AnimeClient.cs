using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AniSpace.Models
{
    internal class AnimeClient : HttpClient
    {
        internal AnimeClient(HttpClientHandler handler = null)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:77.0) Gecko/20100101 Firefox/77.0");
        }
    }
}
