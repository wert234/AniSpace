using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AniSpace.Models
{
    internal class AnimeRequest : HttpRequestMessage
    {
        internal AnimeRequest(Uri AnimeUri)
        {
            Method = HttpMethod.Get;
            RequestUri = AnimeUri;
        }
    }
}
