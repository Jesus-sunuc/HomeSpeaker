using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSpeaker.MAUI.Models
{
    public class SongModel
    {
        public int SongId { get; set; }
        public required string Name { get; init; }
        private string? path;
        public string? Path
        {
            get => path;
            set
            {
                path = value;
                if (path?.Contains('\\') ?? false)
                    Folder = System.IO.Path.GetDirectoryName(path.Replace('\\', '/'));
                else
                    Folder = System.IO.Path.GetDirectoryName(path);
            }
        }
        public required string Album { get; init; }
        public required string Artist { get; init; }
        public string? Folder { get; private set; }
    }
}
