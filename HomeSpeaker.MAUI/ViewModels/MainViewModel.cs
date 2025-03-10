using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSpeaker.MAUI.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [RelayCommand]
        public async Task NavigateToMusicPage()
        {
            await Shell.Current.GoToAsync("//MusicPage");
        }

        [RelayCommand]
        public async Task NavigateToServerSettingsPage()
        {
            await Shell.Current.GoToAsync("//ServerSettingsPage");
        }
    }
}
