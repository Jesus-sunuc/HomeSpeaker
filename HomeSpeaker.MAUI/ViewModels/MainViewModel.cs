using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeSpeaker.MAUI.Services;
using HomeSpeaker.Shared;
using System.Collections.ObjectModel;


namespace HomeSpeaker.MAUI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly HomeSpeakerMauiService homeSpeakerService;

    [ObservableProperty]
    private ObservableCollection<Song> songs;

    [ObservableProperty]
    private string statusMessage;

    public MainViewModel()
    {
        homeSpeakerService = new HomeSpeakerMauiService("http://localhost:5028");
    }

    [RelayCommand]
    public async Task LoadSongsAsync()
    {
        try
        {
            var list = await homeSpeakerService.GetAllSongsAsync();
            Songs = new ObservableCollection<Song>(list);
            StatusMessage = $"Loaded {list.Count} songs from server.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"LoadSongs error: {ex.Message}";
        }
    }

    [RelayCommand]
    public async Task GetStatusAsync()
    {
        try
        {
            var statusReply = await homeSpeakerService.GetStatusAsync();
            StatusMessage = $"Currently playing: {statusReply?.CurrentSong?.Name}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"GetStatus error: {ex.Message}";
        }
    }

    [RelayCommand]
    public async Task PlaySongAsync(Song selectedSong)
    {
        var reply = await homeSpeakerService.PlaySongAsync(selectedSong.SongId);
        if (reply.Ok)
            StatusMessage = $"Playing: {selectedSong.Name}";
        else
            StatusMessage = "Server rejected song play request.";
    }
}