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
        public string Id { get; set; }
        public string AlbumId { get; set; }
        public string SpotifyHref { get; set; }
        public string Name { get; set; }
        public bool Explicit { get; set; }
        public Int64 DurationMs { get; set; }
        public Int64 Popularity { get; set; }
        public Int64 TrackNumber { get; set; }
    }
}