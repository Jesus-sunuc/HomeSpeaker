using System.ComponentModel;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Converter;
using YoutubeExplode.Search;
using YoutubeExplode.Videos.Streams;

namespace HomeSpeaker.MAUI.Services
{
    //public class YouTubeService
    //{
    //    private readonly YoutubeClient _youtubeClient;

    //    public YouTubeService()
    //    {
    //        _youtubeClient = new YoutubeClient();
    //    }

    //    public async Task<List<string>> SearchYouTubeVideosAsync(string query)
    //    {
    //        var results = new List<string>();
    //        await foreach (var video in _youtubeClient.Search.GetVideosAsync(query))
    //        {
    //            results.Add($"{video.Title} ({video.Url})");
    //        }
    //        return results;
    //    }

    //    public async Task<string> GetYouTubeVideoUrlAsync(string videoId)
    //    {
    //        var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(videoId);
    //        var streamInfo = streamManifest
    //                .GetMuxedStreams()
    //                .OrderByDescending(s => s.VideoQuality.Label)
    //                .FirstOrDefault();
    //        return streamInfo?.Url ?? string.Empty;
    //    }

    //    public async Task<string> DownloadYouTubeVideoAsync(string videoId)
    //    {
    //        var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(videoId);
    //        var streamInfo = streamManifest
    //                .GetMuxedStreams()
    //                .OrderByDescending(s => s.VideoQuality.Label)
    //                .FirstOrDefault();

    //        if (streamInfo == null) return "No video found";

    //        string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), $"{videoId}.mp4");

    //        using (var stream = await _youtubeClient.Videos.Streams.GetAsync(streamInfo))
    //        using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
    //        {
    //            await stream.CopyToAsync(fileStream);
    //        }

    //        return filePath;
    //    }
    //}
    public class YouTubeService
    {
        private readonly YoutubeClient _youtubeClient;
        private readonly string _downloadPath;

        public YouTubeService()
        {
            _youtubeClient = new YoutubeClient();
            _downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "HomeSpeakerMedia", "YouTube Cache");

            if (!Directory.Exists(_downloadPath))
            {
                Directory.CreateDirectory(_downloadPath);
            }
        }

        public async Task<List<VideoDto>> SearchYouTubeVideosAsync(string query, int maxResults = 20)
        {
            var results = new List<VideoDto>();
            await foreach (var batch in _youtubeClient.Search.GetResultBatchesAsync(query))
            {
                foreach (var result in batch.Items)
                {
                    if (result is VideoSearchResult video)
                    {
                        results.Add(new VideoDto(video.Title, video.Id, video.Url, video.Thumbnails.FirstOrDefault()?.Url, video.Author?.ChannelTitle, video.Duration));
                        if (results.Count >= maxResults) return results;
                    }
                }
            }
            return results;
        }

        public async Task<string> GetYouTubeVideoUrlAsync(string videoId)
        {
            var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(videoId);
            var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();
            return streamInfo?.Url ?? string.Empty;
        }

        public async Task<string> DownloadYouTubeVideoAsync(string videoId, IProgress<double> progress)
        {
            var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(videoId);
            var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

            if (streamInfo == null) return "No video found";

            string fileName = $"{videoId}.mp3";
            string filePath = Path.Combine(_downloadPath, fileName);

            if (File.Exists(filePath))
            {
                return filePath; // Return the existing file path if already downloaded
            }

            using var stream = await _youtubeClient.Videos.Streams.GetAsync(streamInfo);
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            byte[] buffer = new byte[81920];
            int bytesRead;
            long totalBytes = streamInfo.Size.Bytes;
            long bytesWritten = 0;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await fileStream.WriteAsync(buffer, 0, bytesRead);
                bytesWritten += bytesRead;
                progress.Report((double)bytesWritten / totalBytes * 100);
            }

            return filePath;
        }
    }

    public record VideoDto(string Title, string Id, string Url, string? Thumbnail, string? Author, TimeSpan? Duration);
}
