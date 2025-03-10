using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using HomeSpeaker.Shared;
using static HomeSpeaker.Shared.HomeSpeaker;
using Grpc.Core;
using HomeSpeaker.MAUI.Models;


namespace HomeSpeaker.MAUI.Services;
public class HomeSpeakerMauiService : IHomeSpeakerMauiService
{
    private HomeSpeakerClient client;
    private string baseUrl;
    private readonly List<string> availableServers = new();


    private readonly List<SongMessage> _songs = new();
    public IEnumerable<SongMessage> Songs => _songs;



    public event EventHandler QueueChanged;
    public event EventHandler<string>? StatusChanged;

    public HomeSpeakerMauiService(string initialBaseUrl)
    {
        baseUrl = initialBaseUrl;
        availableServers.Add(initialBaseUrl);
        InitializeClient();
    }

    private void InitializeClient()
    {
        var httpHandler = new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(5),
            EnableMultipleHttp2Connections = true
        };

        var grpcWebHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, httpHandler);

        var channelOptions = new GrpcChannelOptions
        {
            HttpHandler = grpcWebHandler
        };

        if (baseUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
        {
            channelOptions.Credentials = ChannelCredentials.SecureSsl;
        }
        else
        {
            channelOptions.Credentials = ChannelCredentials.Insecure;
        }

        var channel = GrpcChannel.ForAddress(baseUrl, channelOptions);
        client = new HomeSpeakerClient(channel);

        Console.WriteLine($"[INFO] Initialized gRPC client with server: {baseUrl}");
    }


    public void ChangeServer(string newBaseUrl)
    {
        if (availableServers.Contains(newBaseUrl))
        {
            baseUrl = newBaseUrl;
            InitializeClient();
            Console.WriteLine($"[INFO] Switched to new server: {newBaseUrl}");
        }
        else
        {
            Console.WriteLine($"[WARNING] Server {newBaseUrl} not found in available servers.");
        }
    }

    public void AddServer(string newServer)
    {
        if (!availableServers.Contains(newServer))
        {
            availableServers.Add(newServer);
            Console.WriteLine($"[INFO] Added new server: {newServer}");
        }
    }

    public void RemoveServer(string serverToRemove)
    {
        if (availableServers.Contains(serverToRemove) && availableServers.Count > 1)
        {
            availableServers.Remove(serverToRemove);
            Console.WriteLine($"[INFO] Removed server: {serverToRemove}");

            if (serverToRemove == baseUrl)
            {
                ChangeServer(availableServers[0]);
            }
        }
    }

    public List<string> GetAvailableServers()
    {
        return new List<string>(availableServers);
    }



    public async Task<IEnumerable<SongModel>> GetAllSongsAsync()
    {
        var result = new List<SongModel>();

        using var call = client.GetSongs(new GetSongsRequest());
        while (await call.ResponseStream.MoveNext())
        {
            var reply = call.ResponseStream.Current;
            result.AddRange(reply.Songs.Select(songMsg => songMsg.ToSongModel()));
        }
        return result;
    }


    public async Task<GetStatusReply> GetStatusAsync()
    {
        var statusReply = await client.GetPlayerStatusAsync(new GetStatusRequest());
        StatusChanged?.Invoke(this, $"Now playing: {statusReply.CurrentSong?.Name}");
        return statusReply;
    }

    public async Task PlaySongAsync(int songId)
    {
        await client.PlaySongAsync(new PlaySongRequest { SongId = songId });
    }

    public async Task PlayStreamAsync(string streamUri)
    {
        await client.PlayStreamAsync(new PlayStreamRequest { StreamUrl = streamUri });
    }

    public async Task<IEnumerable<SongModel>> GetPlayQueueAsync()
    {
        var queue = new List<SongModel>();
        var queueResponse = client.GetPlayQueue(new GetSongsRequest());

        await foreach (var reply in queueResponse.ResponseStream.ReadAllAsync())
        {
            queue.AddRange(reply.Songs.Select(s => s.ToSongModel()));
        }

        return queue;
    }

    public async Task ClearQueueAsync()
    {
        await client.PlayerControlAsync(new PlayerControlRequest { ClearQueue = true });
        QueueChanged?.Invoke(this, EventArgs.Empty);
    }
    public async Task SkipToNextAsync()
    {
        await client.PlayerControlAsync(new PlayerControlRequest { SkipToNext = true });
        QueueChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task ResumePlayAsync()
    {
        await client.PlayerControlAsync(new PlayerControlRequest { Play = true });
        QueueChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task StopPlayingAsync()
    {
        await client.PlayerControlAsync(new PlayerControlRequest { Stop = true });
        QueueChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task SetVolumeAsync(int volume)
    {
        await client.PlayerControlAsync(new PlayerControlRequest { SetVolume = true, VolumeLevel = volume });
    }

    public async Task<int> GetVolumeAsync()
    {
        var status = await GetStatusAsync();
        return status.Volume;
    }

    public async Task UpdateQueueAsync(List<SongModel> songs)
    {
        var request = new UpdateQueueRequest();
        request.Songs.AddRange(songs.Select(s => s.Path));

        await client.UpdateQueueAsync(request);
        QueueChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task PlayPlaylistAsync(string playlistName)
    {
        await client.PlayPlaylistAsync(new PlayPlaylistRequest { PlaylistName = playlistName });
    }

    public async Task<IEnumerable<Playlist>> GetPlaylistsAsync()
    {
        var response = await client.GetPlaylistsAsync(new GetPlaylistsRequest());
        return response.Playlists.Select(p => new Playlist(p.PlaylistName, p.Songs.Select(s => s.ToSong())));
    }

    public async Task AddToPlaylistAsync(string playlistName, string songPath)
    {
        await client.AddSongToPlaylistAsync(new AddSongToPlaylistRequest { PlaylistName = playlistName, SongPath = songPath });
    }

    public async Task RemoveFromPlaylistAsync(string playlistName, string songPath)
    {
        await client.RemoveSongFromPlaylistAsync(new RemoveSongFromPlaylistRequest { PlaylistName = playlistName, SongPath = songPath });
    }

    public async Task ShuffleQueueAsync()
    {
        await client.ShuffleQueueAsync(new ShuffleQueueRequest());
        QueueChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task PlayFolderAsync(string folder)
    {
        await client.PlayFolderAsync(new PlayFolderRequest { FolderPath = folder });
    }

    public async Task<IEnumerable<string>> GetFolders()
    {
        var folders = new List<string>();
        var getSongsReply = client.GetSongs(new GetSongsRequest());

        await foreach (var reply in getSongsReply.ResponseStream.ReadAllAsync())
        {
            foreach (var song in reply.Songs)
            {
                var folder = System.IO.Path.GetDirectoryName(song.Path);
                if (!folders.Contains(folder))
                {
                    folders.Add(folder);
                }
            }
        }

        return folders;
    }

    public async Task ToggleBrightness()
    {
        await client.ToggleBacklightAsync(new Google.Protobuf.WellKnownTypes.Empty());
    }

    public async Task UpdateMetadataAsync(SongMessage song)
    {
        Console.WriteLine($"[INFO] Updating metadata for {song.Name}");
    }
    public async Task EnqueueFolderAsync(SongGroup songs)
    {
        if (songs == null || !songs.Any()) return;

        await client.EnqueueFolderAsync(new EnqueueFolderRequest { FolderPath = songs.First().Folder });
    }

    public async Task EnqueueFolderAsync(string folder)
    {
        if (string.IsNullOrWhiteSpace(folder)) return;

        await client.EnqueueFolderAsync(new EnqueueFolderRequest { FolderPath = folder });
    }

    public async Task EnqueueSongAsync(int songId)
    {
        if (songId <= 0) return;

        await client.EnqueueSongAsync(new PlaySongRequest { SongId = songId });
    }


    public async Task<Dictionary<string, List<SongModel>>> GetSongGroups()
    {
        var groups = new Dictionary<string, List<SongModel>>();
        var getSongsReply = client.GetSongs(new GetSongsRequest());

        await foreach (var reply in getSongsReply.ResponseStream.ReadAllAsync())
        {
            foreach (var song in reply.Songs)
            {
                var songModel = song.ToSongModel();
                if (songModel.Folder == null) continue;

                if (!groups.ContainsKey(songModel.Folder))
                    groups[songModel.Folder] = new List<SongModel>();

                groups[songModel.Folder].Add(songModel);
            }
        }

        return groups;
    }

    public async Task<IEnumerable<SongModel>> GetSongsInFolder(string folder)
    {
        var songs = new List<SongModel>();
        var getSongsReply = client.GetSongs(new GetSongsRequest { Folder = folder });

        await foreach (var reply in getSongsReply.ResponseStream.ReadAllAsync())
        {
            songs.AddRange(reply.Songs.Select(s => s.ToSongModel()));
        }

        return songs;
    }

}
