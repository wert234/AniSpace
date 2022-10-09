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
    internal class GetAniMang : AnimeBase
    {
        private AnimeBoxItemControl _Anime;
        private List<string> _Content;
        private AnimeRequest _Request;
        private HtmlDocument _Document;
        private List<HtmlNode>? _AnimeList;
        private string _Age;
        internal GetAniMang(AnimeBoxItemControl anime)
        {
            _Anime = anime;
            _Request = new AnimeRequest(new Uri($"https://animang.ru/?s={_Anime.AnimeName.ConvertToSearchName(':')}"));
            _Content = new List<string>();
            _Document = new HtmlDocument();
            _AnimeList = new List<HtmlNode>();
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
            if (_Anime.AnimeAge is null) _Age = ""; else _Age = _Anime.AnimeAge.Remove(4);
            _AnimeList = _Document.DocumentNode?.SelectNodes("//section")?.Where(x => x.InnerText.Contains(_Age)).ToList();
            if ((_AnimeList?.Count is null || _AnimeList.Count == 0) && _Request.RequestUri.ToString().Contains(_Anime.AnimeName.ConvertToSearchName(':')))
            {
                _Request = new AnimeRequest(new Uri($"https://animang.ru/?s={_Anime.AnimeOrigName.ConvertToSearchName(':')}"));
                _Document.LoadHtml(await GetRespons());
                _AnimeList = _Document.DocumentNode?.SelectNodes("//section")?.Where(x => x.InnerText.Contains(_Age)).ToList();
            }
            if (_Document.DocumentNode?.SelectNodes("//div[@class='s-navi']")?.ToList()?.Count is null)
            {
                if(_AnimeList is null || _AnimeList.Count == 0)
                    _AnimeList = _Document.DocumentNode?.SelectNodes("//section").ToList();
                _Content.Add(_AnimeList[0].SelectSingleNode("//span").InnerText);
                _Document.LoadHtml(_AnimeList[0].InnerHtml);
                _Content.Add(_AnimeList[0].SelectSingleNode("//u").InnerText);
                _Content.Add(_AnimeList[0].SelectSingleNode($"//img").Attributes["src"].Value);
                _Request = new AnimeRequest(new Uri(_Document.DocumentNode.SelectSingleNode("//a").Attributes["href"].Value));
                _Document.LoadHtml(await GetRespons());
                _Content.Add(_Document.DocumentNode.SelectSingleNode("//span[@class='rt-opis']").InnerText);
                return;
            }
            DisplayDefault();
        }
        internal async Task Display()
        {
             await GetHtml();
            _Anime.AnimeTegs = _Content[1];
            _Anime.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(_Content[2]);
            _Anime.AnimeRaiting = _Content[3];
        }
        private void DisplayDefault()
        {
            _Anime.AnimeTegs = "Такого аниме нет на этом сайте";
            _Anime.AnimeRaiting = "Ошибка 404";
            _Anime.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(@"D:\Програмирование\Visual Studio\AniSpace\AniSpace\Resources\Img\ErrorImage.png");
        }

    }
}
