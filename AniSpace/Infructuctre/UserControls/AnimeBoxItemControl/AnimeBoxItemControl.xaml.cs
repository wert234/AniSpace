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
    /// <summary>
    /// Логика взаимодействия для AnimeBoxItemControl.xaml
    /// </summary>
    public partial class AnimeBoxItemControl : UserControl
    {

        #region AnimeName
        public string AnimeName
        {
            get { return (string)GetValue(AnimeNameProperty); }
            set { SetValue(AnimeNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimeNameProperty =
            DependencyProperty.Register("AnimeName", typeof(string), typeof(AnimeBoxItemControl), new PropertyMetadata(""));
        #endregion

        #region AnimeRaiting
        public string AnimeRaiting
        {
            get { return (string)GetValue(RaitingProperty); }
            set { SetValue(RaitingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RaitingProperty =
            DependencyProperty.Register("AnimeRaiting", typeof(string), typeof(AnimeBoxItemControl), new PropertyMetadata(""));
        #endregion

        #region AnimeImage
        public ImageSource AnimeImage
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("AnimeImage", typeof(ImageSource), typeof(AnimeBoxItemControl), new PropertyMetadata());
        #endregion

        #region Commands
        #region AddAplicationCommand
        public ICommand AddAplicationCommand { get; set; }
        private bool CanAddAplicationCommandExecuted(object p) => true;
        private async Task OnAddAplicationCommandExecuted()
        {
            await AnimeListBoxControler.SaveAsync(AnimeRaiting,AnimeName,AnimeImage.ToString());
        }

        #endregion
        #region RemuveAplicationCommand
        public ICommand RemoveAplicationCommand { get; set; }
        private bool CanRemoveAplicationCommand(object p) => true;
        private async Task OnRemoveAplicationCommand()
        {
           await AnimeListBoxControler.DelteByNameAsync(AnimeName);
        }

        #endregion

        #endregion
        public AnimeBoxItemControl()
        { 
            AddAplicationCommand = new RelayCommand(OnAddAplicationCommandExecuted, CanAddAplicationCommandExecuted);
            RemoveAplicationCommand = new RelayCommand(OnRemoveAplicationCommand, CanRemoveAplicationCommand);
            InitializeComponent();

        }
    }
}
