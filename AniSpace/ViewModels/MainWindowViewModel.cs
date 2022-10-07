using AniSpace.Data;
using AniSpace.Infructuctre.Commands.Base;
using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
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
        #endregion
        #region MenuContentPropertys
        public ObservableCollection<UserControl> AnimeListBoxItems { get; set; }
        public int AnimeCunter { get; set; } = 1;
        public AnimeDbContext AnimeDb { get; set; }
        public ObservableCollection<Models.AnimeBase> Animes { get; set; }
        public ObservableCollection<UserControl> SavedAnimeBoxItems { get; set; }

        #endregion
        #region MenuSortingPropertys

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
            await AnimeControler.SearchAnimeAsync(
                AnimeCunter.ToString(),
                ((TextBlock)Years.Content).Text,
                ((TextBlock)Age.Content).Text,
                AnimeListBoxItems,
                MoreApplicationCommand);
            AnimeCunter++;
        }

        #endregion
        #region MoreApplicationCommand
        public ICommand MoreApplicationCommand { get; set; }
        private bool CanMoreApplicationCommandExecuted(object p) => true;
        private async Task OnMoreApplicationCommandExecuted()
        {
            AnimeListBoxItems.Remove(AnimeListBoxItems[AnimeListBoxItems.Count - 1]);
            await OnSearchApplicationCommandExecuted();
        }
        #endregion
        #endregion

        public MainWindowViewModel()
        {
            #region Propertys
            AnimeListBoxItems = new ObservableCollection<UserControl>();
            AnimeDb = new AnimeDbContext();
            Animes = new ObservableCollection<Models.AnimeBase>();
            SavedAnimeBoxItems = new ObservableCollection<UserControl>();
            #endregion
            #region CommandsInition

            SearchApplicationCommand = new RelayCommand(OnSearchApplicationCommandExecuted, CanSearchApplicationCommandExecuted);
            MoreApplicationCommand = new RelayCommand(OnMoreApplicationCommandExecuted, CanMoreApplicationCommandExecuted);
            #endregion
            #region Db
              AnimeDbControler.LoadAnime(Animes, AnimeDb, SavedAnimeBoxItems);
            #endregion
        }
    }
}