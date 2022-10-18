using AniSpace.Data;
using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using AniSpace.Infructuctre.UserControls.AnimeMoreButtonControl;
using Microsoft.EntityFrameworkCore;
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
    internal static class AnimeDbControler
    {
        private static ObservableCollection<AnimeBase>? _Animes;
        private static ObservableCollection<UserControl>? _SavedAnimeBoxItems;
        private static AnimeDbContext? _AnimeDb;
        internal static async Task SaveAsync(string AnimeRaiting, string AnimeName, string AnimeOrigName, string AnimeImage, string AnimeAge, string Tegs)
        {
           AnimeControler.Create(AnimeName,AnimeOrigName, AnimeRaiting, AnimeImage, AnimeAge, Tegs, _SavedAnimeBoxItems);
            await _AnimeDb.AddAsync(ConvertListBoxItemToDbItem(AnimeRaiting,AnimeName,AnimeImage, AnimeOrigName, AnimeAge, Tegs));
            await _AnimeDb.SaveChangesAsync();
            LoadAnime(_AnimeDb);
        }
        internal static async Task DelteByNameAsync(string AnimeName)
        {
            _AnimeDb.AnimeBoxItemControls.Remove(_AnimeDb.AnimeBoxItemControls.Where(x => x.AnimeName == AnimeName).FirstOrDefault());
            await _AnimeDb.SaveChangesAsync();
            LoadAnime(_AnimeDb);
        }
        internal static void LoadAnime(ObservableCollection<AnimeBase> Animes, AnimeDbContext AnimeDb, ObservableCollection<UserControl>? SavedAnimeBoxItems)
        {
            _SavedAnimeBoxItems = SavedAnimeBoxItems;
            _Animes = Animes;
            _AnimeDb = AnimeDb; 
            _AnimeDb.Database.EnsureCreated();
            _AnimeDb.AnimeBoxItemControls.Load();
            foreach (AnimeBase item in _AnimeDb.AnimeBoxItemControls)
            {
                AnimeControler.Create(item.AnimeName,item.AnimeOrigName, item.AnimeRating, item.AnimeImage, item.AnimeAge, item.AnimeTegs, _SavedAnimeBoxItems);
                _Animes.Add(item);
            }
                
        }
        internal static void LoadAnime(AnimeDbContext AnimeDb)
        {  
            _Animes.Clear();
            _SavedAnimeBoxItems.Clear();
            AnimeDb.AnimeBoxItemControls.Load();
            foreach (AnimeBase item in AnimeDb.AnimeBoxItemControls)
            {
                AnimeControler.Create(item.AnimeName, item.AnimeOrigName, item.AnimeRating, item.AnimeImage, item.AnimeAge, item.AnimeTegs, _SavedAnimeBoxItems);
                _Animes.Add(item);
            }
        }
        internal static AnimeBase ConvertListBoxItemToDbItem(string AnimeRaiting, string AnimeName, string AnimeImage, string AnimeOrigName, string Age, string Tegs)
        {
            AnimeBase animeDbItem = new AnimeBase();
            animeDbItem.AnimeImage = AnimeImage.ToString();
            animeDbItem.AnimeName = AnimeName;
            animeDbItem.AnimeRating = AnimeRaiting;
            animeDbItem.AnimeOrigName = AnimeOrigName;
            animeDbItem.AnimeTegs = Tegs;
            animeDbItem.AnimeAge = Age;
            return animeDbItem;
        }
    }
}
