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
    //public partial class YouTubeViewModel : ObservableObject
    //{
    //    private readonly YouTubeService _youTubeService;

    //    [ObservableProperty]
    //    private ObservableCollection<string> searchResults = new();

    //    [ObservableProperty]
    //    private string searchQuery;

    //    [ObservableProperty]
    //    private string videoUrl;

    //    public YouTubeViewModel(YouTubeService youTubeService)
    //    {
    //        _youTubeService = youTubeService;
    //    }

    //    [RelayCommand]
    //    public async Task SearchYouTube()
    //    {
    //        if (string.IsNullOrWhiteSpace(SearchQuery)) return;

    //        var results = await _youTubeService.SearchYouTubeVideosAsync(SearchQuery);
    //        SearchResults = new ObservableCollection<string>(results);
    //    }

    //    [RelayCommand]
    //    public async Task PlayVideo(string videoId)
    //    {
    //        VideoUrl = await _youTubeService.GetYouTubeVideoUrlAsync(videoId);
    //    }

    //    [RelayCommand]
    //    public async Task DownloadVideo(string videoId)
    //    {
    //        string filePath = await _youTubeService.DownloadYouTubeVideoAsync(videoId);
    //        VideoUrl = filePath;
    //    }
    //}

    public partial class YouTubeViewModel : ObservableObject
    {
        private readonly YouTubeService _youTubeService;

        [ObservableProperty]
        private ObservableCollection<VideoDto> searchResults = new();

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private string videoUrl;

        [ObservableProperty]
        private bool isDownloading;

        [ObservableProperty]
        private double downloadProgress;

        public YouTubeViewModel(YouTubeService youTubeService)
        {
            _youTubeService = youTubeService;
        }

        [RelayCommand]
        public async Task SearchYouTube()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery)) return;
            var results = await _youTubeService.SearchYouTubeVideosAsync(SearchQuery);
            SearchResults = new ObservableCollection<VideoDto>(results);
        }

        [RelayCommand]
        public async Task PlayVideo(string videoId)
        {
            VideoUrl = await _youTubeService.GetYouTubeVideoUrlAsync(videoId);
        }

        //[RelayCommand]
        //public async Task DownloadVideo(string videoId)
        //{
        //    IsDownloading = true;
        //    DownloadProgress = 0;

        //    var progress = new Progress<double>(p => DownloadProgress = p);
        //    string filePath = await _youTubeService.DownloadYouTubeVideoAsync(videoId, progress);

        //    IsDownloading = false;
        //    VideoUrl = filePath; // Set to downloaded file path if needed
        //}
    }
}
