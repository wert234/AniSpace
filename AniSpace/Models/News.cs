using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AniSpace.Infructuctre.UserControls.AnimeNewsControl;
using System.Collections.ObjectModel;

namespace AniSpace.Models
{
    internal static class News
    {
        public static HtmlDocument Html { get; set; }
        public static HtmlNodeCollection Names { get; set; }
        public static HtmlNodeCollection Texts { get; set; }
        public static HtmlNodeCollection imgs { get; set; }
        internal static void Create(ObservableCollection<AnimeNewsControl> news)
        {
            using (var client = new HttpClient())
            {
                Html = new HtmlDocument();
                var message = client.GetAsync("https://kg-portal.ru/news/anime/").Result;
                Html.LoadHtml(message.Content.ReadAsStringAsync().Result);
                foreach (HtmlNode item in Html.DocumentNode.SelectNodes("//div[@class='video']"))
                    item.Remove();

                 Names = Html.DocumentNode.SelectNodes("//a[@class='news_card_link']");
                 Texts = Html.DocumentNode.SelectNodes("//div[@class='news_text']");
                 imgs = Html.DocumentNode.SelectNodes("//img[@title]");

                for (int i = 0; i < 19; i++)
                    news.Add(new AnimeNewsControl("https://kg-portal.ru" + imgs[i].Attributes["src"].Value, Names[i].InnerText, Texts[i].InnerText));
            }
        }
    }
}
