using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;

namespace Spotifly.Models
{
    public class Friend
    {
        public int User1Id { get; set; }
        public int User2Id { get; set; }
    }
}