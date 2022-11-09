using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AniSpace.Infructuctre.UserControls.AnimeNewsControl;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using AniSpace.Infructuctre.UserControls.AnimeMoreButtonControl;
using System.Windows.Input;

namespace AniSpace.Models
{
    internal static class News
    {
        public static HtmlDocument Html { get; set; }
        public static HtmlNodeCollection Names { get; set; }
        public static HtmlNodeCollection Texts { get; set; }
        public static HtmlNodeCollection imgs { get; set; }
        public static ObservableCollection<UserControl> NewsList { get; set; }

        internal static async Task CreateAsync(ObservableCollection<UserControl> news, int Counter, AnimeMoreButtonControl more)
        {
                    NewsList = news;
                    using (var client = new HttpClient())
                    {
                        object looker = new();
                   
                        Html = new HtmlDocument();
                        var message = await client.GetAsync($"https://kg-portal.ru/news/anime/{Counter}");
                        Html.LoadHtml(await message.Content.ReadAsStringAsync());
                        foreach (HtmlNode item in Html.DocumentNode.SelectNodes("//div[@class='video']"))
                            item.Remove();
                   
                        Names = Html.DocumentNode.SelectNodes("//a[@class='news_card_link']");
                        Texts = Html.DocumentNode.SelectNodes("//div[@class='news_text']");
                        imgs = Html.DocumentNode.SelectNodes("//img[@title]");
                   
                        if (NewsList.Count != 0) NewsList.Remove(NewsList.Last());
                        for (int i = 0; i < 19; i++)
                            NewsList.Add(new AnimeNewsControl("https://kg-portal.ru" + imgs[i].Attributes["src"].Value, Names[i].InnerText, Texts[i].InnerText));
                        NewsList.Add(more);
                    }
        }
        internal static void Create(ObservableCollection<UserControl> news, int Counter, AnimeMoreButtonControl more)
        {
            NewsList = news;
            using (var client = new HttpClient())
            {
                object looker = new();

                        Html = new HtmlDocument();
                    var message = client.GetAsync($"https://kg-portal.ru/news/anime/{Counter}").Result;
                    Html.LoadHtml(message.Content.ReadAsStringAsync().Result);
                    foreach (HtmlNode item in Html.DocumentNode.SelectNodes("//div[@class='video']"))
                        item.Remove();
                    
                    Names = Html.DocumentNode.SelectNodes("//a[@class='news_card_link']");
                    Texts = Html.DocumentNode.SelectNodes("//div[@class='news_text']");
                    imgs = Html.DocumentNode.SelectNodes("//img[@title]");
                    
                    if (NewsList.Count != 0) NewsList.Remove(NewsList.Last());
                    for (int i = 0; i < 19; i++)
                        NewsList.Add(new AnimeNewsControl("https://kg-portal.ru" + imgs[i].Attributes["src"].Value, Names[i].InnerText, Texts[i].InnerText));
                    NewsList.Add(more);
            }
        }
    }
}
