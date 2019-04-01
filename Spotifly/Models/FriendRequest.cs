using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;

namespace Spotifly.Models
{
    public class FriendRequest
    {
        public Int64 SenderId { get; set; }
        public Int64 ReceiverId { get; set; }
        public DateTime DateCreated { get; set; }

        public static bool SendRequest(Int64 senderId, Int64 receiverId)
        {
            FriendRequest friendRequest = null;
            // Checking if the receiver hasn't already sent a request to the sender
            string select = "SELECT * FROM friend_request WHERE sender_id = @ReceiverId and receiver_id = @SenderId";
            string insert = "INSERT INTO friend_request (sender_id, receiver_id) VALUES (@SenderId, @ReceiverId)";
            bool success = false;

            using (MySqlConnection conn = SpotiflyDB.NewConnection())
            {
                try
                {
                    friendRequest = conn.QuerySingle(select, new { SenderId = senderId, ReceiverId = receiverId});
                }
                #pragma warning disable 0168
                catch (Exception e)
                #pragma warning restore 0168
                {
                    friendRequest = null;
                }
                if (friendRequest == null)
                {
                    var affectedRows = conn.Execute(insert, new { SenderId = senderId, ReceiverId = receiverId});
                    success = (1 == affectedRows);
                }
            }
            return success;
        }

        public static List<User> SelectReceivedRequests(Int64 userId)
        {
            List<User> requests = null;
            string sql = "SELECT user.id, user.name FROM user JOIN friend_request ON receiver_id = user.id WHERE id = @UserId";

            using (MySqlConnection conn = SpotiflyDB.NewConnection())
            {
                requests = conn.Query<User>(sql, new {UserId = userId}).ToList();
            }
            return requests;
        }

        public static bool AcceptRequest(Int64 senderId, Int64 receiverId)
        {
            bool success = false;
            string sql = "DELETE FROM friend_request WHERE sender_id = @SenderId and receiver_id = @ReceiverId";

            if (Friend.AddFriend(senderId, receiverId))
            {
                using (MySqlConnection conn = SpotiflyDB.NewConnection())
                {
                    var affectedRows = conn.Execute(sql, new { SenderId = senderId, ReceiverId = receiverId});
                    success = (1 == affectedRows);
                }
            }
            return success;
        }

        public static bool DeclineRequest(Int64 senderId, Int64 receiverId)
        {
            bool success = false;
            string sql = "DELETE FROM friend_request WHERE sender_id = @SenderId and receiver_id = @ReceiverId";

            using (MySqlConnection conn = SpotiflyDB.NewConnection())
            {
                var affectedRows = conn.Execute(sql, new { SenderId = senderId, ReceiverId = receiverId});
                success = (1 == affectedRows);
            }
            return success;
        }
    }
}