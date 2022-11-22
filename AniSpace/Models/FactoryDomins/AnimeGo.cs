using AniSpace.Infructuctre.LinqExtensions;
using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AniSpace.Models.FactoryDomins
{
    internal class AnimeGo : AnimeBase
    {
        private AnimeBoxItemControl _Anime;
        private List<string> _Animes;
        private List<string> _Content;
        private AnimeRequest _Request;
        private HtmlDocument _Document;
        private List<string> _Tegs;
        private string _Uri;
        private string _Limit;
        private string _Age;
        internal AnimeGo(AnimeBoxItemControl anime)
        {
            _Anime = anime;
            _Age = _Anime.AnimeAge;
            _Animes = new List<string>();
            _Request = new AnimeRequest(new Uri($"https://animego.org/search/anime?q={_Anime.AnimeName}"));
            _Content = new List<string>();
            _Document = new HtmlDocument();
            _Tegs = new List<string>();
        }
        internal AnimeGo(string page, string limit, string season, string ganers, string rating)
        {
            _Limit = limit;
            _Age = season;
            _Anime = new AnimeBoxItemControl();
            _Animes = new List<string>();
            _Document = new HtmlDocument();
            _Content = new List<string>();
            _Tegs = new List<string>();
            _Uri = "https://animego.org/anime/filter";
            if (season != "") _Uri = _Uri + $"/year-from-{season}-to-{season}";
            if (ganers != "") _Uri = _Uri + "/genres-is-" + ganers.GanerToAnimeGoGaner("Драма");
            _Uri = _Uri + $"/apply?sort=createdAt&direction=desc&type=animes&page={page}";
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
        internal async Task HowToGetAsync()
        {
            if (_Document.Text is null) _Document.LoadHtml(await GetRespons());
            if (_Document.DocumentNode.SelectNodes("//div[@class='animes-grid-item col-6 col-sm-6 col-md-4 col-lg-3 col-xl-2 col-ul-2']") is null && _Request.RequestUri.ToString().Contains(_Anime.AnimeName))
            {
                _Request = new AnimeRequest(new Uri($"https://animego.org/search/anime?q={_Anime.AnimeOrigName}"));
                await HowToGetAsync();
            }
            if (_Document.DocumentNode.SelectNodes("//div[@class='animes-grid-item col-6 col-sm-6 col-md-4 col-lg-3 col-xl-2 col-ul-2']") is null) GetDefault();
            await GetAsync();
        }
        private async Task HowToSearchAsync()
        {
            if (_Document.Text is null) _Document.LoadHtml(await GetRespons());
            if (_Document.DocumentNode.InnerText.Contains("ничего не найдено"))
            {
                GetDefault();
                AnimeControler._AnimeListBoxItems[0].Opacity = 1;
                return;
            }
        }
        private async Task GetDirectAcync()
        {
            _Anime.AnimeOrigName = _Document.DocumentNode.SelectSingleNode("//div[@class='text-gray-dark-6 small mb-2']").InnerText;
            AnimeImage = _Document.DocumentNode.SelectSingleNode("//div[@class='anime-list-lazy lazy']").Attributes["data-original"].Value;
            _Anime.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(AnimeImage);
            AnimeName = _Document.DocumentNode.SelectSingleNode("//div[@class='h5 font-weight-normal mb-1']").InnerText;
            if(_Document?.DocumentNode?.SelectSingleNode("//span[@class='anime-genre d-none d-sm-inline']")?.InnerText != null)
               _Anime.AnimeTegs = _Document.DocumentNode.SelectSingleNode("//span[@class='anime-genre d-none d-sm-inline']").InnerText.Replace(" ", "").Replace(",", ", ");
            _Anime.AnimeAge = _Document.DocumentNode.SelectSingleNode("//span[@class='anime-year mb-2']").InnerText;
            if(_Document?.DocumentNode?.SelectSingleNode("//div[@class='p-rate-flag__text']")?.InnerText != null)
            _Anime.AnimeRaiting = _Document.DocumentNode.SelectSingleNode("//div[@class='p-rate-flag__text']").InnerText;
        }
        private async Task GetAsync()
        {
            _Anime.AnimeOrigName = _Document.DocumentNode.SelectSingleNode("//div[@class='text-gray-dark-6 small mb-1 d-none d-sm-block']").InnerText;
            AnimeImage = _Document.DocumentNode.SelectSingleNode("//div[@class='anime-grid-lazy lazy']").Attributes["data-original"].Value;
            _Anime.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(AnimeImage);
            AnimeName = _Document.DocumentNode.SelectSingleNode("//div[@class='h5 font-weight-normal mb-2 card-title text-truncate']").InnerText;

            _Request = new AnimeRequest(new Uri(_Document.DocumentNode.SelectSingleNode("//a[@class='d-block']").Attributes["href"].Value));
            _Document.LoadHtml(await GetRespons());

            if (_Document?.DocumentNode?.SelectSingleNode("//span[@class='rating-value']")?.InnerText != null)
                _Anime.AnimeRaiting = _Document.DocumentNode.SelectSingleNode("//span[@class='rating-value']").InnerText;
            _Anime.AnimeTegs = _Document.DocumentNode.SelectSingleNode("//dd[@class='col-6 col-sm-8 mb-1 overflow-h']").InnerText.Replace(" ","").Replace(",", ", ");
        }
        internal async Task SearchAsync()
        {
            await HowToSearchAsync();
            if (_Document.DocumentNode.SelectNodes("//div[@class='animes-grid-item col-6 col-sm-6 col-md-4 col-lg-3 col-xl-2 col-ul-2']") is null) return;
            foreach (var item in _Document.DocumentNode.SelectNodes("//div[@class='animes-grid-item col-6 col-sm-6 col-md-4 col-lg-3 col-xl-2 col-ul-2']"))
            {
                if (item.InnerText.Contains(_Age))
                    _Animes.Add(item.InnerHtml);
            }

            if (_Animes.Count != 0)
            {
                foreach (string node in _Animes)
                {
                    _Document.LoadHtml(node);
                    await GetAsync();
                    AnimeControler.Create(AnimeName, _Anime.AnimeOrigName, _Anime.AnimeRaiting, AnimeImage, _Anime.AnimeAge, _Anime.AnimeTegs);
                    _Content.Clear();
                    _Tegs.Clear();
                }
                AnimeControler._AnimeListBoxItems.Remove(AnimeControler._AnimeListBoxItems[0]);
                _Animes.Clear();
            }
        }
        internal async Task GetListAsync()
        {
            await HowToSearchAsync();
            if (_Document.DocumentNode.SelectNodes("//div[@class='col-12']") is null) return;

            var counter = Convert.ToInt32(_Limit);

            foreach (var item in _Document.DocumentNode.SelectNodes("//div[@class='col-12']"))
            {
                if (item.InnerText.Contains(_Age))
                    _Animes.Add(item.InnerHtml);
            }

            if (_Animes.Count != 0)
            {
                if (_Animes.Count < Convert.ToInt32(_Limit)) counter = _Animes.Count;
                for (int i = 0; i < counter; i++)
                {
                    _Document.LoadHtml(_Animes[i]);
                    await GetDirectAcync();
                    AnimeControler.Create(AnimeName, _Anime.AnimeOrigName, _Anime.AnimeRaiting, AnimeImage, _Anime.AnimeAge, _Anime.AnimeTegs);
                    _Content.Clear();
                    _Tegs.Clear();
                }
                _Animes.Clear();
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
