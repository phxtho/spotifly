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

            using (MySqlConnection conn = SpotiflyDB.NewConnection())
            {
                users = conn.Query<User>(sql).ToList();
            }
            return users;
        }

        public static User SelectByEmailForAuth(string email)
        {
            string sql = "SELECT name, email, password, date_created FROM user WHERE email = @Email";
            User user = null;

            using (MySqlConnection conn = SpotiflyDB.NewConnection())
            {
                user = conn.QueryFirst<User>(sql, new { Email = email });
            }
            return user;
        }

        public static User SelectByEmail(string email)
        {
            string sql = "SELECT name, email, date_created FROM user WHERE email = @Email";
            User user = null;

            using (MySqlConnection conn = SpotiflyDB.NewConnection())
            {
                user = conn.QueryFirst<User>(sql, new { Email = email });
            }
            return user;
        }

        public static User InsertUser(string name, string email, string password, DateTime dateCreated)
        {
            string insert = "INSERT INTO user (name, email, password, date_created) VALUES (@Name, @Email, @Password, @DateCreated)";
            string select = "SELECT name, email, date_created FROM user WHERE email = @Email";
            User user = null;

            using (MySqlConnection conn = SpotiflyDB.NewConnection())
            {
                var affectedRows = conn.Execute(insert, new { Name = name, Email = email, Password = password, DateCreated = dateCreated });
                if (1 == affectedRows)
                    user = conn.QueryFirst<User>(select, new { Email = email });
            }
            return user;
        }

        public static bool UpdatePassword(int id, string password)
        {
            string sql = "UPDATE user SET password=@Password where id=@Id";
            bool success = false;

            using (MySqlConnection conn = SpotiflyDB.NewConnection())
            {
                var affectedRows = conn.Execute(sql, new {Id = id, Password = password});
                success = (affectedRows == 1);
            }
            return success;
        }
        public static bool UpdatePasswordUsingEmail(string email, string password)
        {
            string sql = "UPDATE user SET password=@Password where email=@Email";
            bool success = false;

            using (MySqlConnection conn = SpotiflyDB.NewConnection())
            {
                var affectedRows = conn.Execute(sql, new {Email = email, Password = password});
                success = (affectedRows == 1);
            }
            return success;
        }
        public static bool UpdateEmail(int id, string email)
        {
            string sql = "UPDATE user SET email=@Email where id=@Id";
            bool success = false;

            using (MySqlConnection conn = SpotiflyDB.NewConnection())
            {
                var affectedRows = conn.Execute(sql, new {Id = id, Email = email});
                success = (affectedRows == 1);
            }
            return success;
        }
        public static bool UpdateEmailUsingEmail(string oldEmail, string newEmail)
        {
            string sql = "UPDATE user SET email=@NewEmail where email=@OldEmail";
            bool success = false;

            using (MySqlConnection conn = SpotiflyDB.NewConnection())
            {
                var affectedRows = conn.Execute(sql, new { NewEmail = newEmail, OldEmail = oldEmail });
                success = (affectedRows == 1);
            }
            return success;
        }
    }
}