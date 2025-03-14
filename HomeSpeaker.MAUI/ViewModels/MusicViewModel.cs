﻿
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeSpeaker.MAUI.Models;
using HomeSpeaker.MAUI.Services;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;


namespace HomeSpeaker.MAUI.ViewModels;

public partial class MusicViewModel : ObservableObject
{
    private readonly IHomeSpeakerMauiService homeSpeakerService;

    [ObservableProperty]
    private ObservableCollection<SongModel> songs;

    [ObservableProperty]
    private string statusMessage = "";

    public MusicViewModel(IHomeSpeakerMauiService homeSpeakerService)
    {
        this.homeSpeakerService = homeSpeakerService;
    }

    [RelayCommand]
    public async Task NavigateToMusicPage()
    {
        await Shell.Current.GoToAsync("//MusicPage");
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

    [RelayCommand]
    public async Task PlaySong(SongModel selectedSong)
    {
        if (selectedSong == null)
        {
            StatusMessage = "No song selected.";
            return;
        }

        try
        {
            await homeSpeakerService.PlaySongAsync(selectedSong.SongId);
            StatusMessage = $"Playing: {selectedSong.Name}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error playing song: {ex.Message}";
        }
    }

    [RelayCommand]
    public async Task StopPlay()
    {
        try 
        {
            await homeSpeakerService.StopPlayingAsync();
            StatusMessage = GetStatus().ToString();
        }
        catch(Exception e)
        {
            StatusMessage = $"Error playing song: {e.Message}";
        }
    }
    [RelayCommand]
    public async Task ResumePlay()
    {
        try
        {
            await homeSpeakerService.ResumePlayAsync();
            StatusMessage =  GetStatus().ToString();
        }
        catch(Exception e)
        {
            StatusMessage = $"Error playing song: {e.Message}";
        }
    }
    [RelayCommand]
    public async Task NextSong()
    {
        try 
        { 
            await homeSpeakerService.SkipToNextAsync();
            StatusMessage =  GetStatus().ToString();
        }
        catch(Exception e)
        {
            StatusMessage = $"Error playing song: {e.Message}";
        }
    }
    [RelayCommand]
    public async Task ClearQueue()
    {
        try
        {
            await homeSpeakerService.ClearQueueAsync();
            StatusMessage = "Queue Cleared";
        }
        catch(Exception e)
        {
            StatusMessage = $"Error playing song: {e.Message}";
        }
    }
    [RelayCommand]
    public async Task ShuffleQueue()
    {
        try
        {
            await homeSpeakerService.ShuffleQueueAsync();
            StatusMessage = StatusMessage =  GetStatus().ToString();
        }
        catch(Exception e)
        {
            StatusMessage = $"Error playing song: {e.Message}";
        }
    }
}