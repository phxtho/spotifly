using System;

namespace Spotifly.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string SpotifyId { get; set; }
        public string SpotifyHref { get; set; }
        public string Name { get; set; }
    }
}