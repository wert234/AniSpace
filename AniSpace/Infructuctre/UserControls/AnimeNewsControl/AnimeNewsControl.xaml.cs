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

namespace AniSpace.Infructuctre.UserControls.AnimeNewsControl
{
    public partial class AnimeNewsControl : UserControl
    {
        #region AnimeImage
        public ImageSource AnimeImage
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("AnimeImage", typeof(ImageSource), typeof(AnimeNewsControl));
        #endregion
        #region AnimeName
        public string AnimeName
        {
            get { return (string)GetValue(AnimeNameProperty); }
            set { SetValue(AnimeNameProperty, value); }
        }
        public static readonly DependencyProperty AnimeNameProperty =
            DependencyProperty.Register("AnimeName", typeof(string), typeof(AnimeNewsControl));
        #endregion
        #region AnimeText
        public string AnimeText
        {
            get { return (string)GetValue(AnimeTextProperty); }
            set { SetValue(AnimeTextProperty, value); }
        }
        public static readonly DependencyProperty AnimeTextProperty =
            DependencyProperty.Register("AnimeText", typeof(string), typeof(AnimeNewsControl));
        #endregion

        public AnimeNewsControl() => InitializeComponent();
        public AnimeNewsControl(string AnimeImage, string AnimeName, string AnimeText)
        {   this.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom(AnimeImage);
            this.AnimeName = AnimeName;
            this.AnimeText = AnimeText;
            InitializeComponent();
        }

    }
}
