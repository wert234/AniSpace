using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        internal GetAniDB(AnimeBoxItemControl anime)
        {
            _Anime = anime;
            _Tegs = new List<string>();
            _Request = new AnimeRequest(new Uri($"https://anidb.net/anime/?adb.search={_Anime.AnimeName}&h=1&noalias=1&orderby.name=0.1&view=grid"));
            _Content = new List<string>();
            _Document = new HtmlDocument();
        }
        private string GetRespons()
        {
            if (_Request is null)
                return string.Empty;

            using (var client = new AnimeClient())
                return client.Send(_Request).Content.ReadAsStringAsync().Result.ToString();
        }
        private void GetHtml()
        {
            _Content.Add(_Document.DocumentNode.SelectSingleNode("//a[@class='name-colored']").InnerText);
            _Content.Add(_Document.DocumentNode.SelectSingleNode("//img").Attributes["src"].Value);
            foreach (HtmlNode item in _Document.DocumentNode.SelectSingleNode("//div[@class='tags']").SelectNodes("//span[@class='tagname']"))
                _Tegs.Add($"{item.InnerText}");

            _Content.Add(string.Join(" ", _Tegs));
            _Content.Add(_Document.DocumentNode.SelectSingleNode("//div[@class='votes rating']").InnerText);
        }
        private void GetDiractionHtml()
        {
            _Content.Add(_Document.DocumentNode.SelectSingleNode("//span[@itemprop='name']").InnerText);
            _Content.Add(_Document.DocumentNode.SelectSingleNode("//meta").Attributes["content"].Value);
            foreach (HtmlNode item in _Document.DocumentNode.SelectSingleNode("//td[@class='value']").SelectNodes("//span[@class='tagname']"))
                _Tegs.Add($"{item.InnerText}");

             _Content.Add(string.Join(" ", _Tegs));
             _Content.Add(_Document.DocumentNode.SelectSingleNode("//span[@itemprop='ratingValue']").InnerText);
        }
        private void SelectHtml()
        {
            _Document.LoadHtml(GetRespons());

            string age = _Anime.AnimeAge?.Remove(4);
            if (age is null)
                age = "";

            if (_Document.DocumentNode?.SelectSingleNode($"//div[@class='g_bubblewrap nowrap g_section']") != null)
            {
                _Request = new AnimeRequest(new Uri($"https://anidb.net/anime/?adb.search={_Anime.AnimeOrigName}&h=1&noalias=1&orderby.name=0.1&view=grid"));
                _Document.LoadHtml(GetRespons());
            }

            if (_Document.DocumentNode?.SelectNodes($"//div[@class='g_odd g_bubble box']")?.Where(x => x.InnerText.Contains(age))?.ToList() is null)
            {
                if(_Document.DocumentNode?.SelectNodes($"//div[@class='g_bubble box']")?.Where(x => x.InnerText.Contains(age))?.ToList() is null)
                {
                    if(_Document.DocumentNode?.SelectNodes($"//tr[@class='g_odd year']")?.Where(x => x.InnerText.Contains(age)).ToList() is null)
                    {
                        _Request = new AnimeRequest(new Uri($"https://anidb.net/anime/?adb.search={_Anime.AnimeOrigName}&h=1&noalias=1&orderby.name=0.1&view=grid"));
                        SelectHtml();
                    }
                    _Document.LoadHtml(_Document.DocumentNode?.SelectNodes($"//div[@class='block']")
                        .Where(x => x.InnerText.Contains(age)).ToList()[0].InnerHtml);
                    GetDiractionHtml();
                    return;
                }
                _Document.LoadHtml(_Document.DocumentNode?.SelectNodes($"//div[@class='g_bubble box']")
                    ?.Where(x => x.InnerText.Contains(age))?.ToList()[0].InnerHtml);
                GetHtml();
                return;
            }
            _Document.LoadHtml(_Document.DocumentNode?.SelectNodes($"//div[@class='g_odd g_bubble box']")?
                .Where(x => x.InnerText.Contains(_Anime.AnimeAge.Remove(4)))?.ToList()[0].InnerHtml);
            GetHtml();
        }
        internal async Task Display()
        {
            SelectHtml();
            _Anime.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(_Content[1]);
            _Anime.AnimeTegs = _Content[2];
            _Anime.AnimeRaiting = _Content[3];
        }

    }
}
