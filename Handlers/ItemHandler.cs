using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SalesInventorySystem_WAM1.Models;

namespace SalesInventorySystem_WAM1.Handlers
{
    internal class ItemHandler : DatabaseHandler
    {
        /// <summary>
        /// Generate a new item ID.
        /// </summary>
        /// <returns>A new item ID</returns>
        public int GenerateItemId()
        {
            var rand = new Random();
            var new_id = rand.Next(100000000, 999999999);
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT id FROM items WHERE id = @id";
                    command.Parameters.AddWithValue("@id", new_id);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // If the ID already exists, generate a new one.
                        if (reader.Read())
                            return GenerateItemId();
                        return new_id;
                    }
                }
            }
        }

        /// <summary>
        /// Add an item to the database.
        /// </summary>
        /// <param name="itemdata">The item to be added to the database.</param>
        public void AddItem(Item itemdata)
        {
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        "INSERT INTO items (id, name, category, unit_price, stock, date_added) VALUES (@id, @name, @category, @unit_price, @stock, @date_added)";
                    command.Parameters.AddWithValue("@id", itemdata.Id);
                    command.Parameters.AddWithValue("@name", itemdata.Name);
                    command.Parameters.AddWithValue("@category", itemdata.Category);
                    command.Parameters.AddWithValue("@unit_price", itemdata.UnitPrice);
                    command.Parameters.AddWithValue("@stock", itemdata.Stock);
                    command.Parameters.AddWithValue("@date_added", itemdata.DateAdded);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Retrieve an item from the database.
        /// </summary>
        /// <param name="id">The ID of the item to be retrieved.</param>
        /// <returns>An Item object or null if it does not exist.</returns>
        public Item GetItem(int id)
        {
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM items WHERE id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Item
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                Category = reader.GetString("category"),
                                UnitPrice = reader.GetDouble("unit_price"),
                                Stock = reader.GetInt32("stock"),
                                DateAdded = reader.GetDateTime("date_added"),
                            };
                        }
                        return null; // Return null if the item does not exist.
                    }
                }
            }
        }

        /// <summary>
        /// Get all items from the database.
        /// </summary>
        /// <returns>A list of Items</returns>
        public List<Item> GetAllItems()
        {
            var items = new List<Item>();
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT id FROM items";
                    using (MySqlDataReader reader = command.ExecuteReader())
                        while (reader.Read())
                            items.Add(GetItem(reader.GetInt32("id")));
                }
            }
            return items;
        }

        /// <summary>
        /// Delete an item from the database.
        /// </summary>
        /// <param name="id">The ID of the item to be removed.</param>
        public void DeleteItem(int id)
        {
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM items WHERE id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Update an item's information in the database.
        /// </summary>
        /// <param name="itemdata">The modified data of the item.</param>
        public void UpdateItem(Item itemdata)
        {
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        "UPDATE items SET name = @name, category = @category, unit_price = @unit_price, stock = @stock WHERE id = @id";
                    command.Parameters.AddWithValue("@id", itemdata.Id);
                    command.Parameters.AddWithValue("@name", itemdata.Name);
                    command.Parameters.AddWithValue("@category", itemdata.Category);
                    command.Parameters.AddWithValue("@unit_price", itemdata.UnitPrice);
                    command.Parameters.AddWithValue("@stock", itemdata.Stock);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Search for items in the database.
        /// </summary>
        /// <param name="query">The user's search query.</param>
        /// <returns>A list of Items that match the user's query.</returns>
        public List<Item> SearchItems(string query)
        {
            var items = new List<Item>();
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT * FROM items WHERE CONCAT_WS('', id, name, category, unit_price, stock) LIKE @query";
                    command.Parameters.AddWithValue("@query", $"%{query}%");
                    using (MySqlDataReader reader = command.ExecuteReader())
                        while (reader.Read())
                            items.Add(GetItem(reader.GetInt32("id")));
                }
            }
            return items;
        }
    }
}
