using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AniSpace.Models
{
    internal static class AnimeRequests
    {
        private static HttpRequestMessage? Request;
        internal static HttpRequestMessage GetShikimoriRequest(string page, string limit, string season, string rating)
        {
            Dictionary<string, string> ShikimoriContent = new Dictionary<string, string>
            {
                {"page", page},
                {"limit",limit },
                { "order","popularity" },
                { "season",season },
                {"rating", rating }
            };
            Request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Content = new FormUrlEncodedContent(ShikimoriContent),
                RequestUri = new Uri("https://shikimori.one/api/animes")
            };
            return Request;
        }
        internal static HttpRequestMessage GetAniDBRequest()
        {
            return Request;
        }
        internal static HttpRequestMessage GetKinopoiskRequest()
        {
            return Request;
        }
        internal static HttpRequestMessage GetJutRequest()
        {
            return Request;
        }
    }
}
