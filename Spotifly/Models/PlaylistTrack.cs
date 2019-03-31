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
        public int PlaylistId;
        public int TrackId;
    }
}