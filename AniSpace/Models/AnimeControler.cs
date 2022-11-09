using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using AniSpace.Infructuctre.UserControls.AnimeMoreButtonControl;
using AniSpace.Models.Factory;
using System.Collections.ObjectModel;
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
            "AniMang" => new AniMangFactory(),
            "Shikimori" => new ShikimoriFactory(),
        };
        private static AnimeFactory animeFactory;
        public static ObservableCollection<UserControl> _AnimeListBoxItems { get; set; }
        public static string Limit { get; set; } = "10";
        internal static void Create(string Name, string NameOrig, string Raiting, string image, string seson, string tegs, ObservableCollection<UserControl> SavedBoxItems = null)
        {
            AnimeBoxItemControl item = new AnimeBoxItemControl();

            if(SavedBoxItems is null)
                _AnimeListBoxItems.Add(item);
            else SavedBoxItems.Add(item);
         
            item.AnimeName = $"{Name}";
            item.AnimeOrigName = NameOrig;
            item.AnimeRaiting = $"{Raiting}";
            item.AnimeAge = $"{seson}";
            item.AnimeTegs = tegs;
            item.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(image);
        }
        internal static void CreateMore(ICommand MoreApplicationCommand)
        {
            AnimeMoreButtonControl control = new AnimeMoreButtonControl();
            control.Command = MoreApplicationCommand;
            _AnimeListBoxItems.Add(control);
        }
        internal static async Task Get(string StudioName, AnimeBoxItemControl anime)
        {
            animeFactory = GetFactory(StudioName);
            await animeFactory.GetAnime(anime);
        }
        public static async Task SearchMoreAsync(string Studio,string page, string season, string ganers, string rating, ICommand MoreApplicationCommand)
        {
            animeFactory = GetFactory(Studio);
            await animeFactory.GetListAnime(page, Limit, season, ganers, rating);
            CreateMore(MoreApplicationCommand);
        }
        public static async Task SearchAsync(string AnimeName, string season, string Studio, string Ganers)
        {
            Create(AnimeName, "", "", @"D:\Програмирование\Visual Studio\AniSpace\AniSpace\Resources\Img\ErrorImage.png", season, Ganers);
            _AnimeListBoxItems[0].Opacity = 0;
            animeFactory = GetFactory(Studio);
            await animeFactory.SearchAnime((AnimeBoxItemControl)_AnimeListBoxItems[0]);
        }
    }
}
