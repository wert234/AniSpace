using AniSpace.Infructuctre.Commands.Base;
using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using AniSpace.Infructuctre.UserControls.AnimeMoreButtonControl;
using AniSpace.ViewModels.Base;
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

        //private int _AnimeHeight;
        //public int AnimeHeight
        //{
        //    get => _AnimeHeight;
        //    set
        //    {
        //        if(Equals(_AnimeHeight, value)) return;
        //        _AnimeHeight = value;
        //        OnPropertyChanged();
        //    }
        //}
        //private int _NiewsHeight;
        //public int NiewsHeight
        //{
        //    get => _NiewsHeight;
        //    set
        //    {
        //        if(_NiewsHeight == value) return;
        //        _NiewsHeight = value;
        //        OnPropertyChanged();
        //    }
        //}

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
            if(SelectedItem is null) return false;
            if(SelectedItem.Content.ToString() == "новинки") return true;
            else return false; 
        }
        private async Task OnSearchApplicationCommandExecuted()
        {
           TextBlock years = (TextBlock)Years.Content;
           TextBlock age = (TextBlock)Age.Content;
           await Models.AnimeJsonParser.Parse("1","10", years.Text, age.Text, AnimeListBoxItems);
            AnimeMoreButtonControl control = new AnimeMoreButtonControl();
            control.Command = MoreApplicationCommand;
            AnimeListBoxItems.Add(control);
        }

        #endregion
        #region MoreApplicationCommand
        public ICommand MoreApplicationCommand { get; set; }
        private bool CanMoreApplicationCommandExecuted(object p) => true;
        private async Task OnMoreApplicationCommandExecuted()
        {
           await Task.Run(() => AnimeListBoxItems.Remove(AnimeListBoxItems[AnimeListBoxItems.Count-1]));
        }
        #endregion

        #endregion

        public MainWindowViewModel()
        {
            #region Propertys
            AnimeListBoxItems = new ObservableCollection<UserControl>();
            #endregion
            #region CommandsInition

            SearchApplicationCommand = new RelayCommand(OnSearchApplicationCommandExecuted, CanSearchApplicationCommandExecuted);

            #endregion
        }
    }
}
