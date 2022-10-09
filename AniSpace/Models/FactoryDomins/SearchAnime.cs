using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AniSpace.Models.FactoryDomins
{
    internal class SearchAnime : AnimeBase
    {
        private readonly Dictionary<string, string>? _ShikimoriContent;
        private AnimeRequest _Request;
        private List<Root> _DeserializeInString;
        private ObservableCollection<UserControl> _AnimeListBoxItems;
        internal SearchAnime(ObservableCollection<UserControl> AnimeListBoxItems, string page, string limit, string season, string rating)
        {
            _ShikimoriContent = new Dictionary<string, string>
            {
                {"page", page},
                {"limit",limit },
                { "order","popularity" },
                { "season",season },
                {"rating", rating }
            };
            _Request = new AnimeRequest(new Uri("https://shikimori.one/api/animes"), _ShikimoriContent);
            _DeserializeInString = new List<Root>();
            _AnimeListBoxItems = AnimeListBoxItems;
        }
        private async Task<HttpResponseMessage> GetRespons()
        {
            using(AnimeClient client = new AnimeClient())
            {
               return await client.SendAsync(_Request);
            }
        }
        internal async Task Display()
        {
            var response = await GetRespons();
            _DeserializeInString = JsonConvert.DeserializeObject<List<Root>>(await response.Content.ReadAsStringAsync());
            foreach (Root? item in _DeserializeInString)
                AnimeControler.CreateAnime(item.russian, item.name, item.score, $"https://shikimori.one/{item.image.preview}", item.released_on, "Жанр", _AnimeListBoxItems);
        }
    }
}
