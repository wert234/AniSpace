using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using AniSpace.Infructuctre.UserControls.AnimeMoreButtonControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AniSpace.Models
{
    //ренейм в AnimeControler
    internal static class AnimeControler
    { 
        public static string? Limit { get; set; } = "15";
        private static List<Root>? DeserializeInString;
        private static HttpRequestMessage? Request;
        private static HttpResponseMessage? Response;
        internal enum AnimeStudio
        {
            Shikimori = 1,
            AniDB = 2,
            Kinopoisk = 3,
            Jut = 4
        }
        internal static void CreateAnime(string Name, string Raiting, string image, ObservableCollection<UserControl> AnimeListBoxItems)
        {
            AnimeBoxItemControl item = new AnimeBoxItemControl();
            AnimeListBoxItems.Add(item);
            item.AnimeName = $"{Name}";
            item.AnimeRaiting = $"{Raiting}";
            item.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(image);
        }
        internal static void CreateMoreButten(ObservableCollection<UserControl> AnimeListBoxItems, ICommand MoreApplicationCommand)
        {
            AnimeMoreButtonControl control = new AnimeMoreButtonControl();
            control.Command = MoreApplicationCommand;
            AnimeListBoxItems.Add(control);
        }
        public static async Task SelectDisplay(AnimeStudio AnimeStudio, ObservableCollection<UserControl> AnimeListBoxItems)
        {
            switch (AnimeStudio)
            {
                case AnimeStudio.Shikimori:
                    await  AnimeDisplay.ShikimoriDisplay(Response, DeserializeInString, AnimeListBoxItems);
                    break;
                case AnimeStudio.AniDB:
                    await AnimeDisplay.AniDBDisplay();
                    break;
                case AnimeStudio.Kinopoisk:
                    await AnimeDisplay.KinopoiskDisplay();
                    break;
                case AnimeStudio.Jut:
                    await AnimeDisplay.JutDisplay();
                    break;
                default:
                    break;
            }
        }
        public static async Task SelectStudio(AnimeStudio AnimeStudio, string page, string limit, string season, string rating)
        {
            switch (AnimeStudio)
            {
                case AnimeStudio.Shikimori:
                    Request = AnimeRequests.GetShikimoriRequest(page, limit, season, rating);  
                    break;
                case AnimeStudio.AniDB:
                    Request = AnimeRequests.GetAniDBRequest();
                    break;
                case AnimeStudio.Kinopoisk:
                    Request = AnimeRequests.GetKinopoiskRequest();
                    break;
                case AnimeStudio.Jut:
                    Request = AnimeRequests.GetJutRequest();
                    break;
                default:
                    break;
            }
        }
        public static async Task GetAnimeAsync(AnimeStudio AnimeStudio, string page, string limit, string season, string rating, ObservableCollection<UserControl> AnimeListBoxItems, ICommand MoreApplicationCommand)
        {
            using (HttpClient client = new HttpClient())
            {
                await SelectStudio(AnimeStudio, page, limit, season, rating);
                Response = await client.SendAsync(Request);
                if (Response is null) return; 
                await SelectDisplay(AnimeStudio, AnimeListBoxItems);
            }
         }
    }
}
