using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;

namespace Spotifly.Models
{
    public class Playlist
    {
        public string Id { get; set; }
        public Int64 OwnerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SpotifyHref { get; set; }
        public bool Public { get; set; }
    }
}