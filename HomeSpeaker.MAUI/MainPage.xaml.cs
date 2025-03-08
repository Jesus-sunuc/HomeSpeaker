﻿using HomeSpeaker.MAUI.ViewModels;
using HomeSpeaker.Shared;
using System.Collections.ObjectModel;

namespace HomeSpeaker.MAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }

}
