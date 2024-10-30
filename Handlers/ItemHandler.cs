using MySql.Data.MySqlClient;
using SalesInventorySystem_WAM1.Models;
using System;
using System.Collections.Generic;

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
                        if (reader.Read()) return GenerateItemId();
                        return new_id;
                    }
                }
            }
        }

        /// <summary>
        /// Add an item to the database.
        /// </summary>
        /// <param name="itemdata"></param>
        public void AddItem(Item itemdata)
        {
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO items (id, name, category, unit_price, stock, date_added) VALUES (@id, @name, @category, @unit_price, @stock, @date_added)";
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
                                DateAdded = reader.GetDateTime("date_added")
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public List<Item> GetAllItems()
        {
            var items = new List<Item>();
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM items";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(
                                new Item
                                {
                                    Id = reader.GetInt32("id"),
                                    Name = reader.GetString("name"),
                                    Category = reader.GetString("category"),
                                    UnitPrice = reader.GetDouble("unit_price"),
                                    Stock = reader.GetInt32("stock"),
                                    DateAdded = reader.GetDateTime("date_added")
                                }
                            );
                        }
                    }
                }
            }
            return items;
        }

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
    }
}
