using AniSpace.Data;
using AniSpace.Infructuctre.Commands.Base;
using AniSpace.Infructuctre.LinqExtensions;
using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using AniSpace.Infructuctre.UserControls.AnimeGaner;
using AniSpace.Infructuctre.UserControls.AnimeMoreButtonControl;
using AniSpace.Models;
using AniSpace.ViewModels.Base;
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

namespace AniSpace.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Propertys

        #region MenuTitelPropertys

        private ComboBoxItem? _SelectedItem;
        public ComboBoxItem? SelectedItem
        {
            get => _SelectedItem;
            set
            {
                if (Equals(_SelectedItem, value)) return;
                _SelectedItem = value;
                OnPropertyChanged();
            }
        }

        private string _SearchAnime;
        public string SearchAnime
        {
            get => _SearchAnime;
            set
            {
                if (Equals(_SearchAnime, value)) return;
                _SearchAnime = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region MenuContentPropertys
        public ObservableCollection<UserControl> AnimeListBoxItems { get; set; }
        public int AnimeCunter { get; set; } = 1;
        public AnimeDbContext AnimeDb { get; set; }
        public ObservableCollection<Models.AnimeBase> Animes { get; set; }
        public ObservableCollection<UserControl> SavedAnimeBoxItems { get; set; }

        private ImageSource? _LoadingImage;
        public ImageSource? LoadingImage
        {
            get => _LoadingImage;
            set
            {
                if (Equals(_LoadingImage, value)) return;
                _LoadingImage = value;
                OnPropertyChanged();
            }
        }

        #endregion
        #region MenuSortingPropertys

        public ObservableCollection<AnimeGaner> Ganers { get; set; }

        private ComboBoxItem _Years;
        public ComboBoxItem Years
        {
            get => _Years;
            set
            {
                if (Equals(_Years, value)) return;
                _Years = value;
                OnPropertyChanged();
            }
        }

        private ComboBoxItem _Genre;
        public ComboBoxItem Genre
        {
            get => _Genre;
            set
            {
                if (Equals(_Genre, value)) return;
                _Genre = value;
                OnPropertyChanged();
            }
        }
        private ComboBoxItem _Tip;
        public ComboBoxItem Tip
        {
            get => _Tip;
            set
            {
                if (Equals(_Tip, value)) return;
                _Tip = value;
                OnPropertyChanged();
            }
        }
        private ComboBoxItem _Age;
        public ComboBoxItem Age
        {
            get => _Age;
            set
            {
                if (Equals(_Age, value)) return;
                _Age = value;
                OnPropertyChanged();
            }
        }

        private ComboBoxItem _Version;
        public ComboBoxItem Version
        {
            get => _Version;
            set
            {
                if (Equals(_Version, value)) return;
                _Version = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Commands

        #region SearchApplicationCommand

        public ICommand SearchApplicationCommand { get; }
        private bool CanSearchApplicationCommandExecuted(object p)
        {
            if (SelectedItem is null) return false;
            if (SelectedItem.Content.ToString() == "новинки") return true;
            else return false;
        }
        private async Task OnSearchApplicationCommandExecuted()
        {
            List<string> str = new List<string>();
            foreach (AnimeGaner item in Ganers)
            {
                if (item.GanerSelected is false) continue;
                str.Add(item.GanerToString(""));
            }
            var ganers = string.Join(", ", str);

            LoadingImage = (ImageSource)new ImageSourceConverter().ConvertFrom(@"D:\Програмирование\Visual Studio\AniSpace\AniSpace\Resources\Img\Background (1).png");
            AnimeListBoxItems.Clear();
            AnimeCunter = 1;
            if (_SearchAnime == "") _SearchAnime = null;
            if(_SearchAnime != null)
            {
                await AnimeControler.SearchAsync(_SearchAnime, _Years.ConvertToString(""), _Version.ConvertToString("shikimori"), ganers);
                LoadingImage = null;
                return;
            }
            await OnMoreApplicationCommandExecuted();
            LoadingImage = null;
        }

        #endregion
        #region MoreApplicationCommand
        public ICommand MoreApplicationCommand { get; set; }
        private bool CanMoreApplicationCommandExecuted(object p) => true;
        private async Task OnMoreApplicationCommandExecuted()
        {
            List<string> str = new List<string>();
            foreach (AnimeGaner item in Ganers)
            {
                if (item.GanerSelected is false) continue;
                str.Add(item.GanerToString(""));
            }
            var ganers = string.Join(", ", str);

            if (AnimeListBoxItems.Count != 0)
            AnimeListBoxItems.Remove(AnimeListBoxItems[AnimeListBoxItems.Count - 1]);

            await AnimeControler.SearchMoreAsync(_Version.ConvertToString("shikimori"),AnimeCunter.ToString(),
                  Years.ConvertToString("2022"), ganers, Age.ConvertToString("pg_13"),MoreApplicationCommand);
           AnimeCunter++;
        }
        #endregion
        #endregion

        public MainWindowViewModel()
        {
            #region Propertys
            AnimeListBoxItems = new ObservableCollection<UserControl>();
            AnimeControler._AnimeListBoxItems = AnimeListBoxItems;
            AnimeDb = new AnimeDbContext();
            Animes = new ObservableCollection<AnimeBase>();
            SavedAnimeBoxItems = new ObservableCollection<UserControl>();
            #endregion
            #region CommandsInition
            SearchApplicationCommand = new RelayCommand(OnSearchApplicationCommandExecuted, CanSearchApplicationCommandExecuted);
            MoreApplicationCommand = new RelayCommand(OnMoreApplicationCommandExecuted, CanMoreApplicationCommandExecuted);
            #endregion
            #region Db
              AnimeDbControler.LoadAnime(Animes, AnimeDb, SavedAnimeBoxItems);
            #endregion
            #region Ganers
            Ganers = new ObservableCollection<AnimeGaner>();
            Ganers.Add(new AnimeGaner("Драма"));
            Ganers.Add(new AnimeGaner("Романтика"));
            Ganers.Add(new AnimeGaner("Этти"));
            Ganers.Add(new AnimeGaner("Экшен"));
            Ganers.Add(new AnimeGaner("Детектив"));
            Ganers.Add(new AnimeGaner("Комедия"));
            Ganers.Add(new AnimeGaner("Меха"));
            Ganers.Add(new AnimeGaner("Мистика"));
            Ganers.Add(new AnimeGaner("Музыка"));
            Ganers.Add(new AnimeGaner("Приключения"));
            Ganers.Add(new AnimeGaner("Повседневность"));
            Ganers.Add(new AnimeGaner("Школа"));
            Ganers.Add(new AnimeGaner("Спрорт"));
            Ganers.Add(new AnimeGaner("Фэнтези"));
            Ganers.Add(new AnimeGaner("Фантастика"));
            Ganers.Add(new AnimeGaner("Боевые искусства"));
            Ganers.Add(new AnimeGaner("Ужасы"));
            #endregion
        }
    }
}