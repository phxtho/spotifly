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

        private static void AdjustIdOrder(ref int user1Id, ref int user2Id)
        {
            if (user1Id < user2Id)
            {
                int temp = user1Id;
                user1Id = user2Id;
                user2Id = temp;
            }
        }
        public static bool AddFriend(int user1Id, int user2Id)
        {
            string sql = "INSERT INTO friend (user1_id, user2_id) VALUES (@User1Id, @User2Id)";
            bool success = false;

            AdjustIdOrder(ref user1Id, ref user2Id);
            using (MySqlConnection conn = SpotiflyDB.NewConnection())
            {
                var affectedRows = conn.Execute(sql, new { User1Id = user1Id, User2Id = user2Id});
                success = (1 == affectedRows);
            }
            return success;
        }

        public static bool RemoveFriend(int user1Id, int user2Id)
        {
            string sql = "DELETE FROM friend WHERE user1_id = @User1Id AND user2_id = @User2Id";
            bool success = false;

            AdjustIdOrder(ref user1Id, ref user2Id);
            using (MySqlConnection conn = SpotiflyDB.NewConnection())
            {
                var affectedRows = conn.Execute(sql, new { User1Id = user1Id, User2Id = user2Id});
                success = (1 == affectedRows);
            }
            return success;
        }
    }
}