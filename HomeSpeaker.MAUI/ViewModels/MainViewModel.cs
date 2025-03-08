using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeSpeaker.MAUI.Models;
using HomeSpeaker.MAUI.Services;
using HomeSpeaker.Shared;
using System.Collections.ObjectModel;


namespace HomeSpeaker.MAUI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IHomeSpeakerMauiService homeSpeakerService;

    [ObservableProperty]
    private ObservableCollection<SongModel> songs;

    [ObservableProperty]
    private string statusMessage = "Welcome to HomeSpeaker!";

    public MainViewModel(IHomeSpeakerMauiService homeSpeakerService)
    {
        this.homeSpeakerService = homeSpeakerService;
    }


    [RelayCommand]
    public async Task LoadSongs()
    {
        try
        {
            Console.WriteLine("[DEBUG] Calling GetAllSongsAsync()...");
            var songList = await homeSpeakerService.GetAllSongsAsync();
            Console.WriteLine($"[DEBUG] Received {songList.Count()} songs from API.");

            if (!songList.Any())
            {
                StatusMessage = "No songs found.";
                return;
            }

            Songs = new ObservableCollection<SongModel>(songList);
            StatusMessage = $"Loaded {Songs.Count} songs.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] LoadSongsAsync Exception: {ex.Message}");
            StatusMessage = $"Error: {ex.Message}";
        }
    }


    [RelayCommand]
    public async Task GetStatus()
    {
        try
        {
            StatusMessage = "Fetching status...";
            var statusReply = await homeSpeakerService.GetStatusAsync();
            StatusMessage = statusReply != null && statusReply.CurrentSong != null
                ? $"Now playing: {statusReply.CurrentSong.Name}"
                : "No song currently playing.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error fetching status: {ex.Message}";
        }
    }
}