using MySql.Data.MySqlClient;
using SalesInventorySystem_WAM1.Models;
using System.Security.Cryptography;
using System.Text;

namespace SalesInventorySystem_WAM1.Handlers
{
    internal class DatabaseHandler
    {
        private readonly string connection_string;

        public DatabaseHandler(
            string db_name = "sales_inventory_system",
            string host = "localhost",
            int port = 3306,
            string username = "root",
            string password = ""
        )
        {
            this.connection_string =
                $"server={host};port={port};uid={username};pwd={password};database={db_name}";
        }

        /// <summary>
        /// Get a new connection to the database.
        /// </summary>
        /// <returns>A new MySQL connection.</returns>
        public MySqlConnection GetNewConnection() => new MySqlConnection(connection_string);

        /// <summary>
        /// Encrypt a password using SHA-256.
        /// </summary>
        /// <param name="password">The plaintext version of the password.</param>
        /// <returns>The SHA-256 hash of the password.</returns>
        public static string EncryptPassword(string password)
        {
            var c = new SHA256Managed();
            var h = new StringBuilder();
            byte[] hpw = c.ComputeHash(Encoding.UTF8.GetBytes(password));
            foreach (byte b in hpw) h.Append(b.ToString("x2"));
            return h.ToString();
        }

        /// <summary>
        /// Log in a user using their given username and password.
        /// </summary>
        /// <param name="username">The name of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A usermodel.</returns>
        public User Login(string username, string password)
        {
            using (var connection = GetNewConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM users WHERE username = @username AND userpass = @password";
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", EncryptPassword(password));
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read()) return null;
                        return new User(
                            reader.GetInt32("id"),
                            reader.GetString("username"),
                            reader.GetString("userpass")
                        );
                    }
                }
            }
        }
    }
}
