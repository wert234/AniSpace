using AniSpace.Infructuctre.Commands.Base;
using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
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

        public ObservableCollection<AnimeBoxItemControl> AnimeListBoxItems { get; set; }

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
            await Models.AnimeJsonParser.Parse("2", "2019", "r", AnimeListBoxItems);
        }

        #endregion

        #endregion

        public MainWindowViewModel()
        {
            #region Propertys
            AnimeListBoxItems = new ObservableCollection<AnimeBoxItemControl>();
            #endregion
            #region CommandsInition

            SearchApplicationCommand = new RelayCommand(OnSearchApplicationCommandExecuted, CanSearchApplicationCommandExecuted);
            
            #endregion
        }
    }
}
