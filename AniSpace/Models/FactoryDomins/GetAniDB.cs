using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using AniSpace.Infructuctre.LinqExtensions;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AniSpace.Models.FactoryDomins
{
    internal class GetAniDB : AnimeBase
    {
        private AnimeBoxItemControl _Anime;
        private List<string> _Tegs;
        private List<string> _Content;
        private AnimeRequest _Request;
        private HtmlDocument _Document;
        private string _Age;
        internal GetAniDB(AnimeBoxItemControl anime)
        {
            _Anime = anime;
            _Tegs = new List<string>();
            _Request = new AnimeRequest(new Uri($"https://anidb.net/anime/?adb.search={_Anime.AnimeName.ConvertToSearchName(':')}&h=1&noalias=1&orderby.name=0.1&view=grid"));
            _Content = new List<string>();
            _Document = new HtmlDocument();
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
            _Content.Add(_Document.DocumentNode.SelectSingleNode("//a[@class='name-colored']").InnerText);
            _Content.Add(_Document.DocumentNode.SelectSingleNode("//img").Attributes["src"].Value);
            foreach (HtmlNode item in _Document.DocumentNode.SelectSingleNode("//div[@class='tags']").SelectNodes("//span[@class='tagname']"))
                _Tegs.Add($"{item.InnerText}");

            _Content.Add(string.Join(" ", _Tegs));
            _Content.Add(_Document.DocumentNode.SelectSingleNode("//div[@class='votes rating']").InnerText);
        }
        private async Task GetDiractionHtml()
        {
            _Content.Add(_Document.DocumentNode.SelectSingleNode("//span[@itemprop='name']").InnerText);
            _Content.Add(_Document.DocumentNode.SelectSingleNode("//meta").Attributes["content"].Value);
            foreach (HtmlNode item in _Document.DocumentNode.SelectSingleNode("//td[@class='value']").SelectNodes("//span[@class='tagname']"))
                _Tegs.Add($"{item.InnerText}");

             _Content.Add(string.Join(" ", _Tegs));
             _Content.Add(_Document.DocumentNode.SelectSingleNode("//span[@itemprop='ratingValue']").InnerText);
        }
        private async Task SelectHtml()
        {
            _Document.LoadHtml(await GetRespons());
            if (_Anime.AnimeAge is null) _Age = ""; else _Age = _Anime.AnimeAge.Remove(4);
            if (_Document.DocumentNode?.SelectNodes($"//div[@class='g_definitionlist']")?.Where(x => x.InnerText.Contains(_Age))?.ToList() != null)
            {
                _Document.LoadHtml(_Document.DocumentNode.SelectNodes($"//div[@class='g_definitionlist']")
                  ?.Where(x => x.InnerText.Contains(_Age)).ToList()[0].InnerHtml);
                await GetDiractionHtml();
                return;
            }
            if (_Document.DocumentNode.SelectSingleNode($"//div[@class='g_bubblewrap nowrap g_section']").InnerText.Contains("No results.") && _Request.RequestUri.ToString().Contains(_Anime.AnimeName.ConvertToSearchName(':')))
            {
                _Request = new AnimeRequest(new Uri($"https://anidb.net/anime/?adb.search={_Anime.AnimeOrigName.ConvertToSearchName(':')}&h=1&noalias=1&orderby.name=0.1&view=grid"));
              await SelectHtml();
                return;
            }
            if (_Document.DocumentNode?.SelectNodes($"//div[@class='g_bubble box']")?.Where(x => x.InnerText.Contains(_Age))?.ToList().Count != 0)
            {
                _Document.LoadHtml(_Document.DocumentNode?.SelectNodes($"//div[@class='g_bubble box']")
                    ?.Where(x => x.InnerText.Contains(_Age))?.ToList()[0].InnerHtml);
                await GetHtml();
                return;
            }
            if (_Document.DocumentNode?.SelectNodes($"//div[@class='g_odd g_bubble box']")?.Where(x => x.InnerText.Contains(_Age))?.ToList().Count != 0)
            {
                _Document.LoadHtml(_Document.DocumentNode?.SelectNodes($"//div[@class='g_odd g_bubble box']")
                    ?.Where(x => x.InnerText.Contains(_Age))?.ToList()[0].InnerHtml);
                await GetHtml();
                return;
            }
            DisplayDefault();
        }
        internal async Task Display()
        {
            await SelectHtml();
            _Anime.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(_Content[1]);
            _Anime.AnimeTegs = _Content[2];
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
