using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using AniSpace.Infructuctre.UserControls.AnimeMoreButtonControl;
using AniSpace.Models.Factory;
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
    internal static class AnimeControler
    {
        private static AnimeFactory GetFactory(string animeTipe) =>
        animeTipe switch
        {
            "AniMang" => new GetAniMangFactory(),
            "AniDB" => new GetAniDBFactory()
        };
        private static AnimeFactory animeFactory;
        public static ObservableCollection<UserControl> _AnimeListBoxItems { get; set; }
        public static string? Limit { get; set; } = "15";
        private static List<Root>? DeserializeInString;
        private static HttpRequestMessage? Request;
        private static HttpResponseMessage? Response;
        internal enum AnimeStudio
        {
            Shikimori = 1,
            AniDB = 2,
            Kinopoisk = 3,
            Jut = 4,
            AniMang = 5
        }
        internal static void CreateAnime(string Name, string NameOrig, string Raiting, string image, string seson, ObservableCollection<UserControl> AnimeListBoxItems)
        {
            AnimeBoxItemControl item = new AnimeBoxItemControl();
            AnimeListBoxItems.Add(item);
            item.AnimeName = $"{Name}";
            item.AnimeOrigName = NameOrig;
            item.AnimeRaiting = $"{Raiting}";
            item.AnimeAge = seson;
            item.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(image);
        }
        internal static void CreateMoreButten(ObservableCollection<UserControl> AnimeListBoxItems, ICommand MoreApplicationCommand)
        {
            AnimeMoreButtonControl control = new AnimeMoreButtonControl();
            control.Command = MoreApplicationCommand;
            AnimeListBoxItems.Add(control);
        }
        internal static void GetAnime(string StudioName, AnimeBoxItemControl anime)
        {
            animeFactory = GetFactory(StudioName);
            animeFactory.GetAnime(anime);
        }
        public static async Task SelectDisplay(AnimeStudio AnimeStudio, ObservableCollection<UserControl> AnimeListBoxItems, string AnimeName = null, string AnimeAge = null, string AnimeRaiting = null, string AnimeImage = null)
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
                case AnimeStudio.AniMang:
                    await AnimeDisplay.AniMangDisplay(Response, AnimeName, AnimeAge, AnimeRaiting, AnimeImage);
                    break;
                default:
                    break;
            }
        }
        public static async Task SelectStudio(AnimeStudio AnimeStudio, string page = null, string limit = null, string season = null, string rating = null, string AnimeName = null)
        {
            switch (AnimeStudio)
            {
                case AnimeStudio.Shikimori:
                    Request = AnimeRequests.GetShikimoriRequest(page, limit, season, rating);
                    break;
                default:
                    break;
            }
        }
        public static async Task GetAnimeAsync(AnimeStudio AnimeStudio, string page, string limit, string season, string rating, ObservableCollection<UserControl> AnimeListBoxItems, ICommand MoreApplicationCommand)
        {
            _AnimeListBoxItems = AnimeListBoxItems;
            using (HttpClient client = new HttpClient())
            {
                await SelectStudio(AnimeStudio, page, limit, season, rating);
                Response = await client.SendAsync(Request);
                if (Response is null) return;
                await SelectDisplay(AnimeStudio, AnimeListBoxItems);
            }
            CreateMoreButten(AnimeListBoxItems, MoreApplicationCommand);
        }
    }
}
