using System.Windows;
using System.Windows.Controls;

namespace AniSpace.Infructuctre.UserControls.AnimeGaner
{
    public partial class AnimeGaner : UserControl
    {
        #region GanerName
        public string GanerName
        {
            get { return (string)GetValue(GanerNameProperty); }
            set { SetValue(GanerNameProperty, value); }
        }
        public static readonly DependencyProperty GanerNameProperty =
            DependencyProperty.Register("GanerName", typeof(string), typeof(AnimeGaner), new PropertyMetadata(""));
        #endregion
        #region GanerSelected
        public bool GanerSelected
        {
            get { return (bool)GetValue(GanerSelectedProperty); }
            set { SetValue(GanerSelectedProperty, value); }
        }
        public static readonly DependencyProperty GanerSelectedProperty =
            DependencyProperty.Register("GanerSelected", typeof(bool), typeof(AnimeGaner), new PropertyMetadata(false));
        #endregion

        public AnimeGaner() => InitializeComponent();
        public AnimeGaner(string GanerName)
        {
            this.GanerName = GanerName;
            InitializeComponent();
        }
    }
}
