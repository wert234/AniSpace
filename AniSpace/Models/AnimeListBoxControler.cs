using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AniSpace.Models
{
    internal static class AnimeListBoxControler
    {
        internal static void Create(string Name, string Raiting, string image, ObservableCollection<AnimeBoxItemControl> AnimeListBoxItems)
        {
            AnimeBoxItemControl item = new AnimeBoxItemControl();
            AnimeListBoxItems.Add(item);
            item.AnimeName = $"Название: {Name}";
            item.AnimeRaiting = $"Рейтинг: {Raiting}";
            item.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(image);
        }
        internal static void Save()
        {

        }
        internal static void Delte()
        {

        }
    }
}
