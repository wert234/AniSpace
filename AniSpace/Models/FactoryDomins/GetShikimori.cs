using AniSpace.Infructuctre.LinqExtensions;
using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace AniSpace.Models.FactoryDomins
{
    internal class GetShikimori : AnimeBase
    {
        private AnimeBoxItemControl _Anime;
        private List<string> _Content;
        private AnimeRequest _Request;
        private HtmlDocument _Document;
        private List<string> _Tegs;
        private Dictionary<string, string>? _ShikimoriContent;
        private List<Root> _DeserializeInString;
        private ObservableCollection<UserControl> _AnimeListBoxItems;
        internal GetShikimori(AnimeBoxItemControl anime)
        {
            _Anime = anime;
            _Request = new AnimeRequest(new Uri($"https://shikimori.one/animes/season/{_Anime.AnimeAge}?search={_Anime.AnimeName.ConvertToSearchName(':')}"));
            if (AnimeAge is null) _Request = new AnimeRequest(new Uri($"https://shikimori.one/animes/?search={_Anime.AnimeName.ConvertToSearchName(':')}"));
            _Content = new List<string>();
            _Document = new HtmlDocument();
            _Tegs = new List<string>();
        }
        internal GetShikimori(ObservableCollection<UserControl> AnimeListBoxItems, string page, string limit, string season, string rating)
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
        private async Task<string> GetRespons()
        {
            using (var client = new AnimeClient())
            {
                var respons = await client.SendAsync(_Request);
                return await respons.Content.ReadAsStringAsync();
            }
        }
        private async Task GetHtml()
        {
            _Document.LoadHtml(await GetRespons());
            if(_Document.DocumentNode.SelectSingleNode("//div[@class='cc-entries']").InnerText.Contains("Ничего нет") && _Request.RequestUri.ToString().Contains(_Anime.AnimeName.ConvertToSearchName(':')))
            {
                _Request = new AnimeRequest(new Uri($"https://shikimori.one/animes/season/{_Anime.AnimeAge}?search={_Anime.AnimeOrigName.ConvertToSearchName(':')}"));
                this._Document.LoadHtml(await GetRespons());
            }
            if(_Document.DocumentNode.SelectSingleNode("//div[@class='cc-entries']").InnerText.Contains("Ничего нет")) DisplayDefault();

            _Request = new AnimeRequest(new Uri(_Document.DocumentNode.SelectSingleNode("//a[@class='cover anime-tooltip']").Attributes["data-tooltip_url"].Value));
            _Document.LoadHtml(await GetRespons());
            _Content.Add(_Document.DocumentNode.SelectSingleNode("//img").Attributes["src"].Value);

            var collection = _Document.DocumentNode.SelectNodes("//span[@class='genre-ru']");
            foreach (HtmlNode item in collection)
                _Tegs.Add(item.InnerText);
            _Content.Add(string.Join(", ", _Tegs));

            _Content.Add(_Document.DocumentNode.SelectSingleNode("//div[@class='rating']").InnerText.Remove(4));

        }
        internal async Task SearchDisplay()
        {
            _DeserializeInString = JsonConvert.DeserializeObject<List<Root>>(await GetRespons());
            foreach (Root? item in _DeserializeInString)
                AnimeControler.CreateAnime(item.russian, item.name, item.score, $"https://shikimori.one/{item.image.preview}", item.released_on, "Жанр", _AnimeListBoxItems);
        }
        internal async Task Display()
        {
            await GetHtml();
            _Anime.AnimeTegs = _Content[1];
            _Anime.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(_Content[0]);
            _Anime.AnimeRaiting = _Content[2];
        }
        private void DisplayDefault()
        {
            _Anime.AnimeTegs = "Такого аниме нет на этом сайте";
            _Anime.AnimeRaiting = "Ошибка 404";
            _Anime.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(@"D:\Програмирование\Visual Studio\AniSpace\AniSpace\Resources\Img\ErrorImage.png");
        }
    }
}
