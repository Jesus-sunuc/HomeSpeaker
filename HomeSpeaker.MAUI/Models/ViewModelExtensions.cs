using HomeSpeaker.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSpeaker.MAUI.Models
{
    public static class ViewModelExtensions
    {
        public static SongModel ToSongModel(this SongMessage song)
        {
            return new SongModel
            {
                SongId = song?.SongId ?? -1,
                Name = song?.Name?.Trim() ?? "[ Null Song Response ??? ]",
                Album = song?.Album?.Trim() ?? "[ No Album ]",
                Artist = song?.Artist?.Trim() ?? "[ No Artist ]",
                Path = song?.Path?.Trim()
            };
        }
    }
}
