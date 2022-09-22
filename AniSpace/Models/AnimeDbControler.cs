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
        private static ObservableCollection<AnimeDbItem>? _Animes;
        private static ObservableCollection<UserControl>? _SavedAnimeBoxItems;
        private static AnimeDbContext? _AnimeDb;
        internal static async Task SaveAsync(string AnimeRaiting, string AnimeName, string AnimeImage)
        {
           AnimeControler.CreateAnime(AnimeName, AnimeRaiting, AnimeImage, _SavedAnimeBoxItems);
            await _AnimeDb.AddAsync(ConvertListBoxItemToDbItem(AnimeRaiting,AnimeName,AnimeImage));
            await _AnimeDb.SaveChangesAsync();
            LoadAnime(_AnimeDb);
        }
        internal static void Save(string AnimeRaiting, string AnimeName, string AnimeImage)
        {
            AnimeControler.CreateAnime(AnimeName, AnimeRaiting, AnimeImage, _SavedAnimeBoxItems);
            _AnimeDb.Add(ConvertListBoxItemToDbItem(AnimeRaiting, AnimeName, AnimeImage));
            _AnimeDb.SaveChanges();
            LoadAnime(_AnimeDb);
        }
        internal static async Task DelteByNameAsync(string AnimeName)
        {
            _AnimeDb.AnimeBoxItemControls.Remove(_AnimeDb.AnimeBoxItemControls.Where(x => x.AnimeName == AnimeName).FirstOrDefault());
            await _AnimeDb.SaveChangesAsync();
            LoadAnime(_AnimeDb);
        }
        internal static void DelteByName(string AnimeName)
        {
            _AnimeDb.AnimeBoxItemControls.Remove(_AnimeDb.AnimeBoxItemControls.Where(x => x.AnimeName == AnimeName).FirstOrDefault());
            _AnimeDb.SaveChangesAsync();
            LoadAnime(_AnimeDb);
        }
        internal static void LoadAnime(ObservableCollection<AnimeDbItem> Animes, AnimeDbContext AnimeDb, ObservableCollection<UserControl>? SavedAnimeBoxItems)
        {
            _SavedAnimeBoxItems = SavedAnimeBoxItems;
            _Animes = Animes;
            _AnimeDb = AnimeDb; 
            _AnimeDb.Database.EnsureCreated();
            _AnimeDb.AnimeBoxItemControls.Load();
            foreach (AnimeDbItem item in _AnimeDb.AnimeBoxItemControls)
            {
                AnimeControler.CreateAnime(item.AnimeName, item.AnimeRating, item.AnimeImage, _SavedAnimeBoxItems);
                _Animes.Add(item);
            }
                
        }
        internal static void LoadAnime(AnimeDbContext AnimeDb)
        {  
            _Animes.Clear();
            _SavedAnimeBoxItems.Clear();
            AnimeDb.AnimeBoxItemControls.Load();
            foreach (AnimeDbItem item in AnimeDb.AnimeBoxItemControls)
            {
                AnimeControler.CreateAnime(item.AnimeName, item.AnimeRating, item.AnimeImage, _SavedAnimeBoxItems);
                _Animes.Add(item);
            }
        }
        internal static AnimeDbItem ConvertListBoxItemToDbItem(string AnimeRaiting, string AnimeName, string AnimeImage)
        {
            AnimeDbItem animeDbItem = new AnimeDbItem();
            animeDbItem.AnimeImage = AnimeImage.ToString();
            animeDbItem.AnimeName = AnimeName;
            animeDbItem.AnimeRating = AnimeRaiting;
            return animeDbItem;
        }
    }
}
