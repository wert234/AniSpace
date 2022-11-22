using AniSpace.Data;
using AniSpace.Infructuctre.Commands.Base;
using AniSpace.Infructuctre.LinqExtensions;
using AniSpace.Infructuctre.UserControls.AnimeGaner;
using AniSpace.Infructuctre.UserControls.AnimeMoreButtonControl;
using AniSpace.Models;
using AniSpace.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace AniSpace.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Propertys
        #region MenuTitelPropertys

        public ObservableCollection<ComboBoxItem> AnimeGaners { get; set; }

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
        public ObservableCollection<UserControl> AnimeNews { get; set; }
        public ObservableCollection<UserControl> AnimeListBoxItems { get; set; }
        public int AnimeCunter { get; set; } = 1;
        public AnimeDbContext AnimeDb { get; set; }
        public ObservableCollection<Models.AnimeBase> Animes { get; set; }
        public ObservableCollection<UserControl> SavedAnimeBoxItems { get; set; }

        private bool? _isLoading;
        public bool? isLoading
        {
            get => _isLoading;
            set
            {
                if (Equals(_isLoading, value)) return;
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        private int _AnimeTitelsOpasity = 1;
        public int AnimeTitelsOpasity
        {
            get => _AnimeTitelsOpasity;
            set
            {
                if(_AnimeTitelsOpasity == value) return;
                _AnimeTitelsOpasity = value;
                OnPropertyChanged();
            }
        }
        private int _AnimeNewsOpasity = 0;
        public int AnimeNewsOpasity
        {
            get => _AnimeNewsOpasity;
            set
            {
                if(_AnimeNewsOpasity == value) return;
                _AnimeNewsOpasity = value;
                OnPropertyChanged();
            }
        }

        #endregion
        #region MenuSortingPropertys

        private bool _isOpenSortingMenu = true;
        public bool isOpenSortingMenu
        {
            get => _isOpenSortingMenu;
            set
            {
                if(_isOpenSortingMenu == value) return;
                _isOpenSortingMenu = value;
                OnPropertyChanged();
            }
        }
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

        public string GenresList
        {
            get
            {
                List<string> str = new List<string>();
                foreach (AnimeGaner item in Ganers)
                {
                    if (item.GanerSelected is false) continue;
                    str.Add(item.GanerToString(""));
                }
                return string.Join(", ", str);
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
            if (SelectedItem.Content.ToString() == "новинки")
            {
                isOpenSortingMenu = true;
                AnimeNewsOpasity = 0;
                AnimeTitelsOpasity = 1;
                return true;
            }

                isOpenSortingMenu = false;
                AnimeNewsOpasity = 1;
                AnimeTitelsOpasity = 0;
                return false;
        }
        private async Task OnSearchApplicationCommandExecuted()
        {
            isLoading = true;
            AnimeListBoxItems.Clear();
            AnimeCunter = 1;
            if (_SearchAnime == "") _SearchAnime = null;
            if(_SearchAnime != null)
            {
                await AnimeControler.SearchAsync(_SearchAnime, _Years.ConvertToString(""), _Version.ConvertToString("shikimori"), GenresList);
                isLoading = false;
                return;
            }
            await OnMoreApplicationCommandExecuted();
        }

        #endregion
        #region MoreApplicationCommand
        public ICommand MoreApplicationCommand { get; set; }
        private bool CanMoreApplicationCommandExecuted(object p) => true;
        private async Task OnMoreApplicationCommandExecuted()
        {
            if (AnimeListBoxItems.Count != 0)
            AnimeListBoxItems.Remove(AnimeListBoxItems[AnimeListBoxItems.Count - 1]);

            await AnimeControler.SearchMoreAsync(_Version.ConvertToString("shikimori"),AnimeCunter.ToString(),
                  Years.ConvertToString("2022"), GenresList, Age.ConvertToString("pg_13"),MoreApplicationCommand);
           AnimeCunter++;
            isLoading = false;
        }
        #endregion

        #region MoreNewsApplicationCommand
        public int NewsCounter { get; set; } = 10;
        public ICommand MoreNewsApplicationCommand { get; set; }
        private bool CanMoreNewsApplicationCommandExecuted(object p) => true;
        private async Task OnMoreNewsApplicationCommandExecuted()
        {
            NewsCounter = NewsCounter + 10;
           await News.CreateAsync(AnimeNews, NewsCounter, new AnimeMoreButtonControl(MoreNewsApplicationCommand));
        }
        #endregion
        #endregion

        public MainWindowViewModel()
        {
            #region Propertys

            AnimeGaners = new ObservableCollection<ComboBoxItem>();
            foreach (var item in AnimeControler.StudioNemse)
                AnimeGaners.Add(new ComboBoxItem() { Content = item.Key });

            AnimeListBoxItems = new ObservableCollection<UserControl>();
            AnimeControler._AnimeListBoxItems = AnimeListBoxItems;
            AnimeDb = new AnimeDbContext();
            Animes = new ObservableCollection<AnimeBase>();
            SavedAnimeBoxItems = new ObservableCollection<UserControl>();
            AnimeNews = new ObservableCollection<UserControl>();
            #endregion
            #region CommandsInition
            SearchApplicationCommand = new RelayCommand(OnSearchApplicationCommandExecuted, CanSearchApplicationCommandExecuted);
            MoreApplicationCommand = new RelayCommand(OnMoreApplicationCommandExecuted, CanMoreApplicationCommandExecuted);
            MoreNewsApplicationCommand = new RelayCommand(OnMoreNewsApplicationCommandExecuted, CanMoreNewsApplicationCommandExecuted);
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
            #region News
            News.Create(AnimeNews, NewsCounter, new AnimeMoreButtonControl(MoreNewsApplicationCommand));
            #endregion
        }
    }
}