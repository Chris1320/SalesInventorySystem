using MySql.Data.MySqlClient;

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
    }
}
