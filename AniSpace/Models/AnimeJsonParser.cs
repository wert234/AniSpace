using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AniSpace.Models
{
    internal static class AnimeJsonParser
    {
        private static List<Root>? DeserializeInString;
        public static async Task Parse(string limit, string season, string rating, ObservableCollection<AnimeBoxItemControl> listBox)
        {
            Dictionary<string, string> content = new Dictionary<string, string>
            {
                {"limit","2" },
                { "order","popularity" },
                { "season","2019" },
                {"rating", "r" }
            };

            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://shikimori.one/api/animes"),
                Method = HttpMethod.Get,
                Content = new FormUrlEncodedContent(content)
            };

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage message = await client.SendAsync(request);
                if (message is null) return;
                DeserializeInString = JsonConvert.DeserializeObject<List<Root>>
                    (await message.Content.ReadAsStringAsync());
            }

            foreach (Root item in DeserializeInString)
                    AnimeListBoxControler.Create(item.name, item.score, $"https://shikimori.one/{item.image.preview}", listBox);
         }
    }
}
