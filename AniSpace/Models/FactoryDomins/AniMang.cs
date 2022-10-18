using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using AniSpace.Infructuctre.LinqExtensions;
using System.IO;

namespace AniSpace.Models.FactoryDomins
{
    internal class AniMang : AnimeBase
    {
        private AnimeBoxItemControl _Anime;
        private List<string> _Content;
        private AnimeRequest _Request;
        private HtmlDocument _Document;
        private List<HtmlNode>? _AnimeList;
        private string _Age;
        private string _Page;
        internal AniMang(AnimeBoxItemControl anime)
        {
            _Anime = anime;
            _Request = new AnimeRequest(new Uri($"https://animang.ru/?s={_Anime.AnimeName.ConvertToSearchName(':')}"));
            _Content = new List<string>();
            _Document = new HtmlDocument();
            _AnimeList = new List<HtmlNode>();
        }
        internal AniMang(string page, string limit, string season, string rating)
        {
            _Age = season;
            _Page = page;
            _Anime = new AnimeBoxItemControl();
            _Content = new List<string>();
            _Document = new HtmlDocument();
            _AnimeList = new List<HtmlNode>();
            if (season is null || season == "")
                _Request = new AnimeRequest(new Uri($"https://animang.one/navi/page/{page}?razdel&sort_rai=ratings_average&sort_stat=status#038;sort_rai=ratings_average&sort_stat=status"));
            else _Request = new AnimeRequest(new Uri($"https://animang.one/navi/page/{page}?razdel&tax_god%5B0%5D={season}&sort_rai=ratings_average&sort_stat=status#038;&sort_rai=ratings_average&sort_stat=status"));
        }
        private async Task<string> GetRespons()
        {
            using (var client = new AnimeClient())
            {
              var respons = await client.SendAsync(_Request);
                return await respons.Content.ReadAsStringAsync();
            }    
        }
        private async Task HowToSearch()
        {
            if (_Document.Text is null) _Document.LoadHtml(await GetRespons());
            _AnimeList?.Add(_Document.DocumentNode);
            await GetAsync();
        }
        internal async Task HowToGet()
        {
            if (_Document.Text is null) _Document.LoadHtml(await GetRespons());
            if (_Anime.AnimeAge is null || _Anime.AnimeAge == "") _Age = ""; if (_Anime.AnimeAge != "") _Age = _Anime.AnimeAge.Remove(4);
                _AnimeList = _Document.DocumentNode?.SelectNodes("//section")?.Where(x => x.InnerText.Contains(_Age)).ToList();
            await GetAsync();
        }
        private async Task GetAsync()
        {
            if ((_AnimeList?.Count is null || _AnimeList.Count == 0) && _Request.RequestUri.ToString().Contains(_Anime.AnimeName.ConvertToSearchName(':')))
            {
                _Request = new AnimeRequest(new Uri($"https://animang.ru/?s={_Anime.AnimeOrigName.ConvertToSearchName(':')}"));
                _Document.LoadHtml(await GetRespons());
                _AnimeList = _Document.DocumentNode?.SelectNodes("//section")?.Where(x => x.InnerText.Contains(_Age)).ToList();
            }
            if (_Document.DocumentNode?.SelectNodes("//div[@class='s-navi']")?.ToList()?.Count is null)
            {
                _Content.Add(_AnimeList[0].SelectSingleNode("//span").InnerText);
                _Document.LoadHtml(_AnimeList[0].InnerHtml);
                _Anime.AnimeTegs = _AnimeList[0].SelectSingleNode("//u").InnerText;
                AnimeImage = _AnimeList[0].SelectSingleNode($"//img").Attributes["src"].Value;
                if (_AnimeList[0]?.SelectNodes("//div[@class='average']")?.ToList().Count > 0) _Anime.AnimeRaiting = _Document.DocumentNode.SelectSingleNode("//div[@class='average']").InnerText;
                else
                {
                    _Request = new AnimeRequest(new Uri(_Document.DocumentNode.SelectSingleNode("//a").Attributes["href"].Value));
                    _Document.LoadHtml(await GetRespons());
                    _Anime.AnimeRaiting = _Document.DocumentNode.SelectSingleNode("//span[@class='rt-opis']").InnerText;
                }
                if (_Anime.AnimeOrigName == "") _Anime.AnimeOrigName = _Document.DocumentNode.SelectSingleNode("//em").InnerText;
                AnimeName = _Document.DocumentNode?.SelectSingleNode("//h1[@itemprop='name']")?.InnerText;
                if (AnimeName is null) AnimeName = _Document.DocumentNode.SelectSingleNode("//span").InnerText;
                return;
            }
            Default();
        }
        internal async Task GetList()
        {
            _Document.LoadHtml(await GetRespons());
            var tooltips = _Document.DocumentNode?.SelectNodes("//section[@class='post-list']")?.ToList();
            if (tooltips != null)
            {
                foreach (HtmlNode node in tooltips)
                {
                    if (_Age is null || _Age == "")
                        _Request = new AnimeRequest(new Uri($"https://animang.one/navi/page/{_Page}?razdel&sort_rai=ratings_average&sort_stat=status#038;sort_rai=ratings_average&sort_stat=status"));
                    else _Request = new AnimeRequest(new Uri($"https://animang.one/navi/page/{_Page}?razdel&tax_god%5B0%5D={_Age}&sort_rai=ratings_average&sort_stat=status#038;&sort_rai=ratings_average&sort_stat=status"));
                    _Document.LoadHtml(await GetRespons());
                    _Document.LoadHtml(node.InnerHtml);
                    await HowToSearch();
                    AnimeControler.Create(AnimeName, _Anime.AnimeOrigName, _Anime.AnimeRaiting, AnimeImage, "", _Anime.AnimeTegs);
                    _AnimeList.Clear();
                    _Content.Clear();
                }
             //   AnimeControler._AnimeListBoxItems.Remove(AnimeControler._AnimeListBoxItems[0]);
                return;
            }
            Default();
            AnimeControler._AnimeListBoxItems[0].Opacity = 1;
        }
        internal async Task Search()
        {
            _Document.LoadHtml(await GetRespons());
            if (_Anime.AnimeAge is null || _Anime.AnimeAge == "") _Age = ""; else if (_Anime.AnimeAge != "") _Age = _Anime.AnimeAge.Remove(4);
            var tooltips = _Document.DocumentNode?.SelectNodes("//section[@class='post-list']")?.Where(x => x.InnerText.Contains(_Age))?.ToList();
            if (tooltips != null)
            {
                foreach (HtmlNode node in tooltips)
                {
                    _Request = new AnimeRequest(new Uri($"https://animang.ru/?s={_Anime.AnimeName.ConvertToSearchName(':')}"));
                    _Document.LoadHtml(await GetRespons());
                    _Document.LoadHtml(node.InnerHtml);
                    await HowToSearch();
                    AnimeControler.Create(AnimeName, _Anime.AnimeOrigName, _Anime.AnimeRaiting, AnimeImage, "", _Anime.AnimeTegs);
                    _AnimeList.Clear();
                    _Content.Clear();
                }
                AnimeControler._AnimeListBoxItems.Remove(AnimeControler._AnimeListBoxItems[0]);
                return;
            }
            Default();
            AnimeControler._AnimeListBoxItems[0].Opacity = 1;
        }
        private void Default()
        {
            _Anime.AnimeTegs = "Такого аниме нет на этом сайте";
            _Anime.AnimeRaiting = "Ошибка 404";
            _Anime.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(@"D:\Програмирование\Visual Studio\AniSpace\AniSpace\Resources\Img\ErrorImage.png");
        }

    }
}
