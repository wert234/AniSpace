using AniSpace.Data;
using AniSpace.Infructuctre.Commands.Base;
using AniSpace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace AniSpace.Infructuctre.UserControls.AnimeBoxItemControl
{
    public partial class AnimeBoxItemControl : UserControl
    {
        public bool isAdded { get; set; } = false;

        #region StudioNames
        public ObservableCollection<MenuItem> StudioNames
        {
            get { return (ObservableCollection<MenuItem>)GetValue(StudioNameProperty); }
            set { SetValue(StudioNameProperty, value); }
        }
        public static readonly DependencyProperty StudioNameProperty =
            DependencyProperty.Register("StudioNames", typeof(ObservableCollection<MenuItem>), typeof(AnimeBoxItemControl));
        #endregion

        #region AnimeAge
        public string AnimeAge
        {
            get { return (string)GetValue(AnimeAgeProperty); }
            set { SetValue(AnimeAgeProperty, value); }
        }
        public static readonly DependencyProperty AnimeAgeProperty =
            DependencyProperty.Register("AnimeAge", typeof(string), typeof(AnimeBoxItemControl));
        #endregion

        #region AnimeName
        public string AnimeName
        {
            get { return (string)GetValue(AnimeNameProperty); }
            set { SetValue(AnimeNameProperty, value); }
        }
        public static readonly DependencyProperty AnimeNameProperty =
            DependencyProperty.Register("AnimeName", typeof(string), typeof(AnimeBoxItemControl), new PropertyMetadata(""));
        #endregion
        #region AnimeOrigName
        public string AnimeOrigName
        {
            get { return (string)GetValue(AnimeOrigNameProperty); }
            set { SetValue(AnimeOrigNameProperty, value); }
        }
        public static readonly DependencyProperty AnimeOrigNameProperty =
            DependencyProperty.Register("AnimeOrigName", typeof(string), typeof(AnimeBoxItemControl), new PropertyMetadata(""));
        #endregion

        #region AnimeTegs
        public string AnimeTegs
        {
            get { return (string)GetValue(AnimeTegsProperty); }
            set { SetValue(AnimeTegsProperty, value); }
        }
        public static readonly DependencyProperty AnimeTegsProperty =
            DependencyProperty.Register("AnimeTegs", typeof(string), typeof(AnimeBoxItemControl), new PropertyMetadata(""));
        #endregion

        #region AnimeRaiting

        private string OldRaiting;
        public Brush RaitingColor
        {
            get { return (Brush)GetValue(RaitingColorProperty); }
            set { SetValue(RaitingColorProperty, value); }
        }
        public static readonly DependencyProperty RaitingColorProperty =
            DependencyProperty.Register("RaitingColor", typeof(Brush), typeof(AnimeBoxItemControl));


        public string AnimeRaiting
        {
            get { return (string)GetValue(RaitingProperty); }
            set { SetValue(RaitingProperty, value); }
        }
        public static readonly DependencyProperty RaitingProperty =
            DependencyProperty.Register("AnimeRaiting", typeof(string), typeof(AnimeBoxItemControl), new PropertyMetadata(""));

        private void RaitingCompare()
        {
            if (OldRaiting is null || OldRaiting is "") OldRaiting = AnimeRaiting;
            if (AnimeRaiting is null || AnimeRaiting is "") { RaitingColor = Brushes.Black; return; }
            if (float.Parse(OldRaiting.Replace('.', ',')) < float.Parse(AnimeRaiting.Replace('.', ',')))
                RaitingColor = Brushes.Green;
            if (float.Parse(OldRaiting.Replace('.', ',')) > float.Parse(AnimeRaiting.Replace('.', ',')))
                RaitingColor = Brushes.Red;
            OldRaiting = AnimeRaiting;
        }
        #endregion

        #region AnimeImage
        public ImageSource AnimeImage
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("AnimeImage", typeof(ImageSource), typeof(AnimeBoxItemControl), new PropertyMetadata());
        #endregion

        #region Commands
        #region AddAplicationCommand
        public ICommand AddApplicationCommand { get; set; }
        private bool CanAddApplicationCommandExecuted(object p) => !isAdded;
        private async Task OnAddApplicationCommandExecuted()
        {
            await AnimeDbControler.SaveAsync(AnimeRaiting, AnimeName, AnimeOrigName, AnimeImage.ToString(), AnimeAge, AnimeTegs);
        }

        #endregion
        #region RemuveApplicationCommand
        public ICommand RemoveApplicationCommand { get; set; }
        private bool CanRemoveApplicationCommand(object p) => isAdded;
        private async Task OnRemoveApplicationCommand()
        {
            await AnimeDbControler.DelteByNameAsync(AnimeName);
        }

        #endregion
        #region ChangeStudioApplicationCommands

        public ICommand ChangeStudioCommand { get; set; }
        private bool CanChangeStudioCommand(object p) => true;
        private async void OnChangeStudioCommand(object p)
        {
            RaitingCompare();
            await AnimeControler.GetAsync(p.ToString(), this);
            RaitingCompare();
        }

        #endregion

        #endregion
        public AnimeBoxItemControl()
        {
           
            #region CommandInit

            AddApplicationCommand = new RelayCommand(OnAddApplicationCommandExecuted, CanAddApplicationCommandExecuted);
            RemoveApplicationCommand = new RelayCommand(OnRemoveApplicationCommand, CanRemoveApplicationCommand);  
            ChangeStudioCommand = new RelayCommand(OnChangeStudioCommand, CanChangeStudioCommand);

            #endregion

            StudioNames = new ObservableCollection<MenuItem>();
            foreach (var item in AnimeControler.StudioNemse)
                StudioNames.Add(new MenuItem() { Header = item.Key, CommandParameter = item.Key, Command = ChangeStudioCommand } );
      
            RaitingCompare();
            InitializeComponent();
        }
    }
}
