using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;


namespace Spotifly.Models
{
    public class Track
    {
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public string SpotifyId { get; set; }
        public string SpotifyHref { get; set; }
        public string Name { get; set; }
        public bool Explicit { get; set; }
        public int DurationMs { get; set; }
        public int Popularity { get; set; }
        public int TrackNumber { get; set; }
    }
}