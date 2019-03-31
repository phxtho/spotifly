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
        public Int64 User1Id { get; set; }
        public Int64 User2Id { get; set; }
        public DateTime DateCreated { get; set; }

        private static void AdjustIdOrder(ref Int64 user1Id, ref Int64 user2Id)
        {
            if (user1Id < user2Id)
            {
                Int64 temp = user1Id;
                user1Id = user2Id;
                user2Id = temp;
            }
        }

        public static List<User> SelectFriends(string userId)
        {
            List<User> friends = null;
            string sql = @"SELECT user.id, user.name FROM user
                JOIN friend ON (friend.user1_id = user.id OR friend.user2_id = user.id) WHERE
                (friend.user1_id = @UserId OR friend.user2_id = @UserId) and user.id != @UserId";

            using (MySqlConnection conn = SpotiflyDB.NewConnection())
            {
                friends = conn.Query<User>(sql, new {UserId = userId}).ToList();
            }
            return friends;
        }

        public static bool IsFriend(Int64 user1Id, Int64 user2Id)
        {
            Friend friend = null;
            string sql = "SELECT user1_id User1Id, user2_id User2Id FROM friend WHERE user1_id = @User1Id AND user2_id = @User2Id";

            AdjustIdOrder(ref user1Id, ref user2Id);
            using (MySqlConnection conn = SpotiflyDB.NewConnection())
            {
                friend = conn.QuerySingle<Friend>(sql, new { User1Id = user1Id, User2Id = user2Id});
            }
            return friend != null;
        }
        
        public static bool AddFriend(Int64 user1Id, Int64 user2Id)
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

        public static bool RemoveFriend(Int64 user1Id, Int64 user2Id)
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