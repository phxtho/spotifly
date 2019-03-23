using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;


namespace Spotifly.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }

        public static List<User> SelectAll()
        {
            string sql = "SELECT * FROM user";
            List<User> users = null;
            try
            {
                using (MySqlConnection conn = SpotiflyDB.NewConnection())
                {
                    users = conn.Query<User>(sql).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("FAILED");
                Console.WriteLine(e.Message);
            }
            return users;
        }
    }
}