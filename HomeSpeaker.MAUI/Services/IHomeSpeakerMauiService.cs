using HomeSpeaker.MAUI.Models;
using HomeSpeaker.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSpeaker.MAUI.Services;

/// <summary>
/// Defining the methods outlined in the WASM project's HomeSpeakerService
/// Noting which are directly from the proto file
/// </summary>
    interface IHomeSpeakerMauiService
    {
        IEnumerable<SongMessage> Songs { get; }

        event EventHandler QueueChanged;
        event EventHandler<string>? StatusChanged;

        Task AddToPlaylistAsync(string playlistName, string songPath);//rpc AddSongToPlaylist(AddSongToPlaylistRequest) returns (AddSongToPlaylistReply);
        Task ClearQueueAsync();// from WASM project but not proto file
        Task EnqueueFolderAsync(SongGroup songs);//rpc EnqueueFolder(EnqueueFolderRequest) returns (EnqueueFolderReply);
        Task EnqueueFolderAsync(string folder);//rpc EnqueueFolder(EnqueueFolderRequest) returns (EnqueueFolderReply);
        Task EnqueueSongAsync(int songId);//rpc EnqueueSong(PlaySongRequest) returns (PlaySongReply);
        Task<IEnumerable<SongModel>> GetAllSongsAsync();//rpc GetSongs(GetSongsRequest) returns (stream GetSongsReply);
        Task<IEnumerable<string>> GetFolders();// from WASM project but not proto file
        Task<IEnumerable<Playlist>> GetPlaylistsAsync();//	rpc GetPlaylists(GetPlaylistsRequest) returns (GetPlaylistsReply);
        Task<IEnumerable<SongModel>> GetPlayQueueAsync();//rpc GetPlayQueue(GetSongsRequest) returns (stream GetSongsReply);
        Task<Dictionary<string, List<SongModel>>> GetSongGroups();// from WASM project but not proto file
        Task<IEnumerable<SongModel>> GetSongsInFolder(string folder);// from WASM project but not proto file
        Task<GetStatusReply> GetStatusAsync();//rpc GetPlayerStatus(GetStatusRequest) returns (GetStatusReply);
        Task<int> GetVolumeAsync();// from WASM project but not proto file
        Task PlayFolderAsync(string folder);//rpc PlayFolder(PlayFolderRequest) returns (PlayFolderReply);
        Task PlayPlaylistAsync(string playlistName);//rpc PlayPlaylist(PlayPlaylistRequest) returns (PlayPlaylistReply);
        Task PlaySongAsync(int songId);//rpc PlaySong(PlaySongRequest) returns (PlaySongReply);
        Task PlayStreamAsync(string streamUri);//rpc PlayStream(PlayStreamRequest) returns (PlaySongReply);
        Task RemoveFromPlaylistAsync(string playlistName, string songPath);//rpc RemoveSongFromPlaylist(RemoveSongFromPlaylistRequest) returns (RemoveSongFromPlaylistReply);
        Task ResumePlayAsync();// from WASM project but not proto file
        Task SetVolumeAsync(int volume0to100);// from WASM project but not proto file
        Task ShuffleQueueAsync();//rpc ShuffleQueue(ShuffleQueueRequest) returns (ShuffleQueueReply);
        Task SkipToNextAsync();// from WASM project but not proto file
        Task StopPlayingAsync();// from WASM project but not proto file
        Task ToggleBrightness();// from WASM project but not proto file
        Task UpdateMetadataAsync(SongMessage song);
        Task UpdateQueueAsync(List<SongModel> songs);//rpc UpdateQueue(UpdateQueueRequest) returns (UpdateQueueReply);

        //private async Task listenForEvents() // from WASM project but not proto file

        //rpc SearchViedo(SearchVideoRequest) returns (SearchVideoReply);
        //rpc CacheVideo(CacheVideoRequest) returns (stream CacheVideoReply);
        //rpc DeleteSong(DeleteSongRequest) returns (DeleteSongReply);

        //rpc ToggleBacklight(google.protobuf.Empty) returns (google.protobuf.Empty);
        // => client.ToggleBacklightAsync(. . .

        //rpc PlayerControl(PlayerControlRequest) returns (PlayerControlReply);
        // => client.PlayerControl(. . .

        //rpc SendEvent(google.protobuf.Empty) returns (stream StreamServerEvent);
        // => client.SendEvent(. . . 
    }
