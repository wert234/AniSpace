using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using AniSpace.Infructuctre.UserControls.AnimeMoreButtonControl;
using AniSpace.Models.Factory;
using AniSpace.Models.FactoryDomins;
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
        public static string Limit { get; set; } = "10";
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
        internal static async Task GetAnime(string StudioName, AnimeBoxItemControl anime)
        {
            animeFactory = GetFactory(StudioName);
            await animeFactory.GetAnime(anime);
        }
        public static async Task SearchAnimeAsync(string page, string season, string rating, ObservableCollection<UserControl> AnimeListBoxItems, ICommand MoreApplicationCommand)
        {
            SearchAnime searchAnime = new SearchAnime(AnimeListBoxItems, page, Limit, season, rating);
            await searchAnime.Display();
            CreateMoreButten(AnimeListBoxItems, MoreApplicationCommand);
        }
    }
}
