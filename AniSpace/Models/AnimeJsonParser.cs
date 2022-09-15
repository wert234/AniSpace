﻿using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AniSpace.Models
{
    internal static class AnimeJsonParser
    {
        private static List<Root>? DeserializeInString;
        public static async Task Parse(string page, string limit, string season, string rating, ObservableCollection<UserControl> AnimeListBoxItems)
        {
            Dictionary<string, string> content = new Dictionary<string, string>
            {
                {"page", page},
                {"limit",limit },
                { "order","popularity" },
                { "season",season },
                {"rating", rating }
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

            foreach (Root? item in DeserializeInString)
                    AnimeListBoxControler.Create(item.russian, item.score, $"https://shikimori.one/{item.image.preview}", AnimeListBoxItems);
         }
    }
}
