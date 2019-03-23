using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;

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