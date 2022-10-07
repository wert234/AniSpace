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
    internal class GetAniMang : AnimeBase
    {
        private AnimeBoxItemControl _Anime;
        private List<string> _Content;
        private AnimeRequest _Request;
        private HtmlDocument _Document;
        internal GetAniMang(AnimeBoxItemControl anime)
        {
            _Anime = anime;
            _Request = new AnimeRequest(new Uri($"https://animang.ru/?s={Regex.Replace(Regex.Replace(_Anime.AnimeName, "[!\"#$%&'()*+,-./:;<=>?@\\[\\]^_`{|}~]", " ")," ", "+")}"));
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
            _Document.LoadHtml(GetRespons().ToString());
            List<HtmlNode>? str = new List<HtmlNode>();

            if(_Anime.AnimeAge is null)
                str = _Document.DocumentNode?.SelectNodes("//section")?.ToList();
            else
                str = _Document.DocumentNode?.SelectNodes("//section")?.Where(x => x.InnerText.Contains(_Anime.AnimeAge?.Remove(4))).ToList();

            if (str?.Count is null || str.Count == 0)
            {
              _Request = new AnimeRequest(new Uri($"https://animang.ru/?s={Regex.Replace(Regex.Replace(_Anime.AnimeOrigName, "[!\"#$%&'()*+,-./:;<=>?@\\[\\]^_`{|}~]", " "), " ", "+")}"));
            _Document.LoadHtml(GetRespons().ToString());

            }
            if (_Anime.AnimeAge is null || str is null || str.Count == 0)
                str = _Document.DocumentNode.SelectNodes("//section").ToList();
            else
                str = _Document.DocumentNode?.SelectNodes("//section")?.Where(x => x.InnerText.Contains(_Anime.AnimeAge?.Remove(4))).ToList();


            _Content.Add(str[0].SelectSingleNode("//span").InnerText);
            _Document.LoadHtml(str[0].InnerHtml);
            _Content.Add(str[0].SelectSingleNode("//u").InnerText);
            _Content.Add(str[0].SelectSingleNode($"//img").Attributes["src"].Value);
            _Request = new AnimeRequest(new Uri(_Document.DocumentNode.SelectSingleNode("//a").Attributes["href"].Value));
            _Document.LoadHtml(GetRespons().ToString());
            _Content.Add(_Document.DocumentNode.SelectSingleNode("//span[@class='rt-opis']").InnerText);
        }
        internal void Display()
        {
            GetHtml();
            _Anime.AnimeTegs = _Content[1];
            _Anime.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(_Content[2]);
            _Anime.AnimeRaiting = _Content[3];
        }

    }
}
