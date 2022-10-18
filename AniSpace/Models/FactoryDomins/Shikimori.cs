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
    internal class Shikimori : AnimeBase
    {
        private AnimeBoxItemControl _Anime;
        private List<string> _Content;
        private AnimeRequest _Request;
        private HtmlDocument _Document;
        private List<string> _Tegs;
        private Dictionary<string, string>? _ShikimoriContent;
        private List<Root> _DeserializeInString;
        internal Shikimori(AnimeBoxItemControl anime)
        {
            _Anime = anime;
            _Request = new AnimeRequest(new Uri($"https://shikimori.one/animes/?search={_Anime.AnimeName.ConvertToSearchName(':')}"));
            if(_Anime.AnimeAge != null)
            if (_Anime.AnimeAge != "")
                _Request = new AnimeRequest(new Uri($"https://shikimori.one/animes/season/{_Anime.AnimeAge.Remove(4)}?search={_Anime.AnimeName.ConvertToSearchName(':')}"));
            _Content = new List<string>();
            _Document = new HtmlDocument();
            _Tegs = new List<string>();
        }
        internal Shikimori(string page, string limit, string season, string rating)
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
        }
        private async Task<string> GetRespons()
        {
            using (var client = new AnimeClient())
            {
                var respons = await client.SendAsync(_Request);
                return await respons.Content.ReadAsStringAsync();
            }
        }
        internal async Task HowToGet()
        {
            if (_Document.Text is null) _Document.LoadHtml(await GetRespons());
            if (_Document.DocumentNode.SelectNodes("//div[@class='b-db_entry']")?.Count != null || _Document.DocumentNode.SelectNodes("//div[@class='entry-tooltip_container']")?.Count != null)
            {
                GetDirectAcync();
                return;
            }
            if (_Document.DocumentNode.SelectSingleNode("//div[@class='cc-entries']").InnerText.Contains("Ничего нет") && _Request.RequestUri.ToString().Contains(_Anime.AnimeName.ConvertToSearchName(':')))
            {
                _Request = new AnimeRequest(new Uri($"https://shikimori.one/animes/season/{_Anime.AnimeAge}?search={_Anime.AnimeOrigName.ConvertToSearchName(':')}"));
                await HowToGet();
            }
            if (_Document.DocumentNode.SelectSingleNode("//div[@class='cc-entries']").InnerText.Contains("Ничего нет")) GetDefault();
            await GetAsync();
        }
        private async Task HowToSearch()
        {
            if (_Document.Text is null) _Document.LoadHtml(await GetRespons());
            if (_Document.DocumentNode.SelectNodes("//div[@class='b-db_entry']")?.Count != null
                || _Document.DocumentNode.SelectNodes("//div[@class='entry-tooltip_container']")?.Count != null)
            {
                await GetDirectAcync();
                AnimeControler._AnimeListBoxItems[0].Opacity = 1;
                return;
            }
            if (_Document.DocumentNode.SelectSingleNode("//div[@class='cc-entries']").InnerText.Contains("Ничего нет"))
            {
                GetDefault();
                AnimeControler._AnimeListBoxItems[0].Opacity = 1;
                return;
            }
        }
        private async Task GetDirectAcync()
        {
            var collection = _Document.DocumentNode.SelectNodes("//span[@class='genre-ru']");
            foreach (HtmlNode item in collection)
                _Tegs.Add(item.InnerText);

           _Anime.AnimeImage = (ImageSource) new ImageSourceConverter().ConvertFrom(_Document.DocumentNode.SelectSingleNode("//img").Attributes["src"].Value);
            _Anime.AnimeTegs = string.Join(", ", _Tegs);
            _Anime.AnimeRaiting = _Document.DocumentNode.SelectSingleNode("//meta[@itemprop='ratingValue']").Attributes["content"].Value;
            _Anime.Name = _Document.DocumentNode.SelectSingleNode("//meta[@itemprop='alternativeHeadline']").Attributes["content"].Value;
            if (_Anime.AnimeOrigName == "") _Anime.AnimeOrigName = _Document.DocumentNode.SelectSingleNode("//meta[@itemprop='name']").Attributes["content"].Value;
        }
        private async Task GetAsync()
        {
            if (_Document.DocumentNode?.SelectNodes("//div[@class='cover linkeable anime-tooltip']")?.Count != null)
                _Request = new AnimeRequest(new Uri(_Document.DocumentNode.SelectSingleNode("//div[@class='cover linkeable anime-tooltip']").Attributes["data-tooltip_url"].Value));
            else if(_Document.DocumentNode.SelectNodes("//a[@class='cover anime-tooltip']")?.Count != null)
                _Request = new AnimeRequest(new Uri(_Document.DocumentNode.SelectSingleNode("//a[@class='cover anime-tooltip']").Attributes["data-tooltip_url"].Value));
            else _Request = new AnimeRequest(new Uri(_Document.DocumentNode.SelectSingleNode("//a").Attributes["data-tooltip_url"].Value));
            _Document.LoadHtml(await GetRespons());
            AnimeImage = _Document.DocumentNode.SelectSingleNode("//img").Attributes["src"].Value;
            AnimeName = _Document.DocumentNode.SelectSingleNode("//span[@class='name-ru']").InnerText;

            var collection = _Document.DocumentNode.SelectNodes("//span[@class='genre-ru']");
            if(collection is null) return;
            foreach (HtmlNode item in collection)
                _Tegs.Add(item.InnerText);
                _Anime.AnimeTegs = string.Join(", ", _Tegs);

            _Anime.AnimeRaiting = _Document.DocumentNode?.SelectSingleNode("//div[@class='rating']")?.InnerText?.Remove(4);
            if (_Anime.AnimeOrigName == "") _Anime.AnimeOrigName = _Document.DocumentNode.SelectSingleNode("//span[@class='name-en']").InnerText;
        }
        internal async Task Search()
        {
            await HowToSearch();
            var tooltips = _Document.DocumentNode.SelectNodes("//article");
            if(tooltips != null)
            {
                foreach (HtmlNode node in tooltips)
                {
                    _Request = new AnimeRequest(new Uri($"https://shikimori.one/animes/season/{_Anime.AnimeAge}?search={_Anime.AnimeName.ConvertToSearchName(':')}"));
                    if (_Anime.AnimeAge == "") _Request = new AnimeRequest(new Uri($"https://shikimori.one/animes/?search={_Anime.AnimeName.ConvertToSearchName(':')}"));
                    _Document.LoadHtml(await GetRespons());
                    _Document.LoadHtml(node.InnerHtml);
                    await GetAsync();
                    AnimeControler.Create(AnimeName, _Anime.AnimeOrigName, _Anime.AnimeRaiting, AnimeImage, "", _Anime.AnimeTegs);
                    _Content.Clear();
                }
                AnimeControler._AnimeListBoxItems.Remove(AnimeControler._AnimeListBoxItems[0]);
            }
        }
        internal async Task GetList()
        {
            _DeserializeInString = JsonConvert.DeserializeObject<List<Root>>(await GetRespons());
            foreach (Root? item in _DeserializeInString)
                AnimeControler.Create(item.russian, item.name, item.score, $"https://shikimori.one/{item.image.preview}", item.released_on, "Жанр");
        }
        private void GetDefault()
        {
            _Anime.AnimeTegs = "Такого аниме нет на этом сайте";
            _Anime.AnimeRaiting = "Ошибка 404";
            _Anime.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(@"D:\Програмирование\Visual Studio\AniSpace\AniSpace\Resources\Img\ErrorImage.png");
        }
    }
}
