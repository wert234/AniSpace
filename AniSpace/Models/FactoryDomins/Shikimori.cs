using AniSpace.Infructuctre.LinqExtensions;
using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Net.Http;
using System.Net;

namespace AniSpace.Models.FactoryDomins
{
    internal class Shikimori : AnimeBase
    {
        private AnimeBoxItemControl _Anime;
        private List<string> _Content;
        private AnimeRequest _Request;
        private HtmlDocument _Document;
        private List<string> _Tegs;
        private string _Uri;
        private string _Limit;
        internal Shikimori(AnimeBoxItemControl anime)
        {
            _Anime = anime;
            _Request = new AnimeRequest(new Uri($"https://shikimori.one/animes/?search={_Anime.AnimeName}"));
            if(_Anime.AnimeAge != null)
            if (_Anime.AnimeAge != "")
                _Request = new AnimeRequest(new Uri($"https://shikimori.one/animes/season/{_Anime.AnimeAge.Remove(4)}?search={_Anime.AnimeName}"));
            _Content = new List<string>();
            _Document = new HtmlDocument();
            _Tegs = new List<string>();
        }
        internal Shikimori(string page, string limit, string season, string ganers, string rating)
        {
            _Limit = limit;
            _Anime = new AnimeBoxItemControl();
            _Document = new HtmlDocument();
            _Content = new List<string>();
            _Tegs = new List<string>();
            _Uri = "https://shikimori.one/animes/";
            if (season != "") _Uri = _Uri + $"season/{season}";
            if (ganers != "") _Uri = _Uri + "/genre/" + ganers.GanerToShikiGaner("Драма");
            if (rating != "") _Uri = _Uri + $"/rating/{rating}";
            _Uri = _Uri + $"/page/{page}";
            _Request = new AnimeRequest(new Uri(_Uri));
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
            if (_Document.DocumentNode.SelectSingleNode("//div[@class='cc-entries']").InnerText.Contains("Ничего нет") && _Request.RequestUri.ToString().Contains(_Anime.AnimeName))
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
            _Anime.AnimeName = _Document.DocumentNode.SelectSingleNode("//h1").InnerText;
            if (_Anime.AnimeOrigName == "") _Anime.AnimeOrigName = _Document.DocumentNode.SelectSingleNode("//meta[@itemprop='name']").Attributes["content"].Value;
        }
        private async Task GetAsync()
        {
            if (_Document.DocumentNode?.SelectNodes("//article")?.Count != null)
                _Document.LoadHtml(_Document.DocumentNode.SelectSingleNode("//article").InnerHtml);
            if (_Document.DocumentNode?.SelectNodes("//div[@class='cover linkeable anime-tooltip']")?.Count != null)
                _Request = new AnimeRequest(new Uri(_Document.DocumentNode.SelectSingleNode("//div[@class='cover linkeable anime-tooltip']").Attributes["data-tooltip_url"].Value));
            else if(_Document.DocumentNode.SelectNodes("//a[@class='cover anime-tooltip']")?.Count != null)
                _Request = new AnimeRequest(new Uri(_Document.DocumentNode.SelectSingleNode("//a[@class='cover anime-tooltip']").Attributes["data-tooltip_url"].Value));
            else _Request = new AnimeRequest(new Uri(_Document.DocumentNode.SelectSingleNode("//a").Attributes["data-tooltip_url"].Value));
            _Document.LoadHtml(await GetRespons());
            _Anime.AnimeAge = _Document.DocumentNode.SelectNodes("//div[@class='b-tag linkeable']")[1].InnerText.Remove(4);
            AnimeImage = _Document.DocumentNode.SelectSingleNode("//img").Attributes["src"].Value;
            _Anime.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(AnimeImage);
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
            if (tooltips != null)
            {
                foreach (HtmlNode node in tooltips)
                {
                    await Task.Delay(310);
                    _Request = new AnimeRequest(new Uri($"https://shikimori.one/animes/season/{_Anime.AnimeAge}?search={_Anime.AnimeName}"));
                    if (_Anime.AnimeAge == "") _Request = new AnimeRequest(new Uri($"https://shikimori.one/animes/?search={_Anime.AnimeName}"));
                    _Document.LoadHtml(await GetRespons());
                    _Document.LoadHtml(node.InnerHtml);
                    await GetAsync();
                    AnimeControler.Create(AnimeName, _Anime.AnimeOrigName, _Anime.AnimeRaiting, AnimeImage, _Anime.AnimeAge, _Anime.AnimeTegs);
                    _Content.Clear();
                    _Tegs.Clear();
                }
                AnimeControler._AnimeListBoxItems.Remove(AnimeControler._AnimeListBoxItems[0]);
            }
        }
        internal async Task GetList()
        {
            var counter = Convert.ToInt32(_Limit);
            _Document.LoadHtml(await GetRespons());
            var tooltips = _Document.DocumentNode.SelectNodes("//article");
            if (tooltips != null)
            {
                if (tooltips.Count < Convert.ToInt32(_Limit)) counter = tooltips.Count;
                for (int i = 0; i < counter; i++)
                {
                    await Task.Delay(310);
                    _Request = new AnimeRequest(new Uri(_Uri));
                    _Document.LoadHtml(await GetRespons());
                    _Document.LoadHtml(tooltips[i].InnerHtml);
                    await GetAsync();
                    AnimeControler.Create(AnimeName, _Anime.AnimeOrigName, _Anime.AnimeRaiting, AnimeImage, _Anime.AnimeAge, _Anime.AnimeTegs);
                    _Content.Clear();
                    _Tegs.Clear();
                }
            }
        }
        private void GetDefault()
        {
            _Anime.AnimeTegs = "Такого аниме нет на этом сайте";
            _Anime.AnimeRaiting = "Ошибка 404";
            _Anime.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(@"pack://application:,,,/Resources/Img/ErrorImage.png");
        }
    }
}
