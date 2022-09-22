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
               AnimeControler.CreateAnime(item.russian, item.score, $"https://shikimori.one/{item.image.preview}", AnimeListBoxItems);
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
