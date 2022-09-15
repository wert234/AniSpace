using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using AniSpace.Infructuctre.UserControls.AnimeMoreButtonControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AniSpace.Models
{
    internal static class AnimeListBoxControler
    {
        public static string? Limit { get; set; } = "15";
        internal static void CreateAnime(string Name, string Raiting, string image, ObservableCollection<UserControl> AnimeListBoxItems)
        {
            AnimeBoxItemControl item = new AnimeBoxItemControl();
            AnimeListBoxItems.Add(item);
            item.AnimeName = $"Название: {Name}";
            item.AnimeRaiting = $"Рейтинг: {Raiting}";
            item.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(image);
        }
        internal static void CreateMoreButten(ObservableCollection<UserControl> AnimeListBoxItems, ICommand MoreApplicationCommand)
        {
            AnimeMoreButtonControl control = new AnimeMoreButtonControl();
            control.Command = MoreApplicationCommand;
            AnimeListBoxItems.Add(control);
        }
        internal static void Save()
        {

        }
        internal static void Delte()
        {

        }
    }
}
