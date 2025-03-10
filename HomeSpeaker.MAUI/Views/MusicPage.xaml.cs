using HomeSpeaker.MAUI.ViewModels;
using HomeSpeaker.Shared;
using System.Collections.ObjectModel;

namespace HomeSpeaker.MAUI
{
    public partial class MusicPage : ContentPage
    {
        public MusicPage(MusicViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }

}