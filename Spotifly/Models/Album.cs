using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;

namespace Spotifly.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string SpotifyId { get; set; }
        public string SpotifyHref { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int DurationMs { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}