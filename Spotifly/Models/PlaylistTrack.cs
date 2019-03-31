using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;

namespace Spotifly.Models
{
    public class PlaylistTrack
    {
        public string playlist_id { get; set; }
        public string track_id { get; set; }
    }
}