using MySql.Data.MySqlClient;

namespace SalesInventorySystem_WAM1.Handlers
{
    internal class DatabaseHandler
    {
        private readonly string connection_string;

        /// <summary>
        /// A base class that handles the connection to the database.
        /// </summary>
        /// <param name="db_name">The name of the database to connect to.</param>
        /// <param name="host">The hostname of the database server.</param>
        /// <param name="port">The port of the database server.</param>
        /// <param name="username">The username to be used.</param>
        /// <param name="password">The password to be used.</param>
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
    }
}
