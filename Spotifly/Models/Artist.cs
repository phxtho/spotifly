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
        public string Id { get; set; }
        public string SpotifyHref { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
    }
}