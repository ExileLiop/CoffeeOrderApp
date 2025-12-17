using MySql.Data.MySqlClient;

namespace CoffeeOrderApp
{
    public static class Database
    {
        private static string connectionString = "server=localhost;user=root;password=1122;database=coffee_db;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}