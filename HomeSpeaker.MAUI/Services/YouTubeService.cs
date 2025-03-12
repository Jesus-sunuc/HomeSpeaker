using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;

namespace HomeSpeaker.MAUI.Services
{
    public class YouTubeService
    {
        private readonly YoutubeClient _youtubeClient;

        public YouTubeService()
        {
            _youtubeClient = new YoutubeClient();
        }

        public async Task<List<string>> SearchYouTubeVideosAsync(string query)
        {
            var results = new List<string>();
            await foreach (var video in _youtubeClient.Search.GetVideosAsync(query))
            {
                results.Add($"{video.Title} ({video.Url})");
            }
            return results;
        }

        public async Task<string> GetYouTubeVideoUrlAsync(string videoId)
        {
            var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(videoId);
            var streamInfo = streamManifest
                    .GetMuxedStreams()
                    .OrderByDescending(s => s.VideoQuality.Label)
                    .FirstOrDefault();
            return streamInfo?.Url ?? string.Empty;
        }

        public async Task<string> DownloadYouTubeVideoAsync(string videoId)
        {
            var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(videoId);
            var streamInfo = streamManifest
                    .GetMuxedStreams()
                    .OrderByDescending(s => s.VideoQuality.Label)
                    .FirstOrDefault();

            if (streamInfo == null) return "No video found";

            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), $"{videoId}.mp4");

            using (var stream = await _youtubeClient.Videos.Streams.GetAsync(streamInfo))
            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }

            return filePath;
        }
    }
}
