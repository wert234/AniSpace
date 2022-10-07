using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AniSpace.Models
{
    internal class AnimeDisplay
    {
        internal static async Task ShikimoriDisplay(HttpResponseMessage? Response, List<Root>? DeserializeInString, ObservableCollection<UserControl> AnimeListBoxItems)
        {
            DeserializeInString = JsonConvert.DeserializeObject<List<Root>>(await Response.Content.ReadAsStringAsync());
            foreach (Root? item in DeserializeInString)
               AnimeControler.CreateAnime(item.russian, item.name, item.score, $"https://shikimori.one/{item.image.preview}", item.released_on, AnimeListBoxItems);
        }
        internal static async Task AniMangDisplay(HttpResponseMessage? Response, string AnimeName,string AnimeAge, string AnimeRaiting, string AnimeImage)
        {
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(await Response.Content.ReadAsStringAsync());
            var str = html.DocumentNode.SelectNodes("//section").Where(x => x.InnerText.Contains(AnimeAge)).ToList();
            AnimeImage = str[0].SelectSingleNode($"//img[@alt='{AnimeName}']").Attributes["src"].Value;
            html.LoadHtml(str[0].InnerHtml);
            AnimeName = (html.DocumentNode.SelectSingleNode("//span").InnerText);

        }
        internal static async Task AniDBDisplay()
        {
         
        }
        internal static async Task KinopoiskDisplay()
        {

        }
        internal static async Task JutDisplay()
        {

        }
    }
}
