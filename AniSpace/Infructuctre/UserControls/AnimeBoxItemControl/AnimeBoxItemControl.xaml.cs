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

namespace AniSpace.Infructuctre.UserControls.AnimeBoxItemControl
{
    public partial class AnimeBoxItemControl : UserControl
    {
        #region AnimeAge
        public string AnimeAge
        {
            get { return (string)GetValue(AnimeAgeProperty); }
            set { SetValue(AnimeAgeProperty, value); }
        }
        public static readonly DependencyProperty AnimeAgeProperty =
            DependencyProperty.Register("AnimeAge", typeof(string), typeof(AnimeBoxItemControl), new PropertyMetadata(""));
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
            if (float.Parse(OldRaiting.Replace('.',',')) < float.Parse(AnimeRaiting.Replace('.', ',')))
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
        private bool CanAddApplicationCommandExecuted(object p) => true;
        private async Task OnAddApplicationCommandExecuted()
        {
            await AnimeDbControler.SaveAsync(AnimeRaiting, AnimeName, AnimeOrigName, AnimeImage.ToString(), AnimeAge, AnimeTegs);
        }

        #endregion
        #region RemuveApplicationCommand
        public ICommand RemoveApplicationCommand { get; set; }
        private bool CanRemoveApplicationCommand(object p) => true;
        private async Task OnRemoveApplicationCommand()
        {
            await AnimeDbControler.DelteByNameAsync(AnimeName);
        }

        #endregion

        #region ChangeStudioApplicationCommands

        public ICommand ChangeOnAniMangCommand { get; set; }
        private bool CanChangeOnAniMangCommand(object p) => true;
        private async Task OnChangeOnAniMangCommand()
        {
            RaitingCompare();
            await AnimeControler.Get("AniMang", this);
            RaitingCompare();
        }

        public ICommand ChangeOnAniDBCommand { get; set; }
        private bool CanChangeOnAniDBCommand(object p) => true;
        private async Task OnChangeOnAniDBCommand()
        {
            RaitingCompare();
            await AnimeControler.Get("AniDB", this);
            RaitingCompare();
        }

        public ICommand ChangeOnShikimoriCommand { get; set; }
        private bool CanChangeOnShikimoriCommand(object p) => true;
        private async Task OnChangeOnShikimoriCommand()
        {
            RaitingCompare();
            await AnimeControler.Get("Shikimori", this);
            RaitingCompare();
        }

        #endregion

        #endregion
        public AnimeBoxItemControl()
        {
            #region CommandInit

            AddApplicationCommand = new RelayCommand(OnAddApplicationCommandExecuted, CanAddApplicationCommandExecuted);
            RemoveApplicationCommand = new RelayCommand(OnRemoveApplicationCommand, CanRemoveApplicationCommand);
            ChangeOnShikimoriCommand = new RelayCommand(OnChangeOnShikimoriCommand, CanChangeOnShikimoriCommand);

            #region ChangeStudioApplicationCommands     
            ChangeOnAniMangCommand = new RelayCommand(OnChangeOnAniMangCommand, CanChangeOnAniMangCommand);
            ChangeOnAniDBCommand = new RelayCommand(OnChangeOnAniDBCommand, CanChangeOnAniDBCommand);
            #endregion

            #endregion
            RaitingCompare();
            InitializeComponent();
        }
    }
}
