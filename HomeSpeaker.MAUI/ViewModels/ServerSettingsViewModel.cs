using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeSpeaker.MAUI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSpeaker.MAUI.ViewModels
{
    public partial class ServerSettingsViewModel : ObservableObject
    {
        private readonly IHomeSpeakerMauiService homeSpeakerService;

        [ObservableProperty]
        private string newServerUrl = string.Empty;

        [ObservableProperty]
        private ObservableCollection<string> servers;

        public ServerSettingsViewModel(IHomeSpeakerMauiService homeSpeakerService)
        {
            homeSpeakerService = homeSpeakerService;
            servers = new ObservableCollection<string>(homeSpeakerService.GetAvailableServers());
        }

        [RelayCommand]
        public async Task NavigateToServerSettingsPage()
        {
            await Shell.Current.GoToAsync("//ServerSettingsPage");
        }


        [RelayCommand]
        public void AddServer()
        {
            if (!string.IsNullOrWhiteSpace(NewServerUrl))
            {
                homeSpeakerService.AddServer(NewServerUrl);
                Servers.Add(NewServerUrl);
                NewServerUrl = string.Empty; // Clear input field after adding
            }
        }

        [RelayCommand]
        public void SetActiveServer(string serverUrl)
        {
            homeSpeakerService.ChangeServer(serverUrl);
        }

        [RelayCommand]
        public void RemoveServer(string serverUrl)
        {
            if (Servers.Contains(serverUrl))
            {
                homeSpeakerService.RemoveServer(serverUrl);
                Servers.Remove(serverUrl);
            }
        }
    }
}
