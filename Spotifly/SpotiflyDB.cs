using MySql.Data.MySqlClient;

namespace Spotifly
{
    public class SpotiflyDB
    {
        public static string _ConnectionString;
        public static MySqlConnection NewConnection()
        {
            return new MySqlConnection(_ConnectionString);
        }
    }
}