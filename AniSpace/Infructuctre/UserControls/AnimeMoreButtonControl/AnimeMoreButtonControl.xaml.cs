﻿using System;
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

namespace AniSpace.Infructuctre.UserControls.AnimeMoreButtonControl
{
    /// <summary>
    /// Логика взаимодействия для AnimeMoreButtonControl.xaml
    /// </summary>
    public partial class AnimeMoreButtonControl : UserControl
    {
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(AnimeMoreButtonControl), new PropertyMetadata());

        public AnimeMoreButtonControl()
        {
            InitializeComponent();
        }
    }
}
