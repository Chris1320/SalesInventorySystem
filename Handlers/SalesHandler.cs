using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SalesInventorySystem_WAM1.Models;

namespace SalesInventorySystem_WAM1.Handlers
{
    internal class SalesHandler : DatabaseHandler
    {
        public void AddTransaction(Transaction transaction)
        {
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        "INSERT INTO sales (item_id, category, price, quantity, status, notes) VALUES (@item_id, @category, @price, @quantity, @status, @notes)";
                    command.Parameters.AddWithValue("@item_id", transaction.ItemId);
                    command.Parameters.AddWithValue("@category", transaction.Category);
                    command.Parameters.AddWithValue("@price", transaction.Price);
                    command.Parameters.AddWithValue("@quantity", transaction.Quantity);
                    command.Parameters.AddWithValue("@status", transaction.Status);
                    command.Parameters.AddWithValue("@notes", transaction.Notes);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Transaction GetTransaction(DateTime id)
        {
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM sales WHERE id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return new Transaction
                            {
                                Id = reader.GetDateTime("id"),
                                ItemId = reader.GetInt32("item_id"),
                                Category = reader.GetString("category"),
                                Price = reader.GetDouble("price"),
                                Quantity = reader.GetInt32("quantity"),
                                Status = reader.GetString("status"),
                                Notes = reader.GetString("notes"),
                            };
                    }
                }
            }
            return null;
        }

        public List<Transaction> GetAllTransactions()
        {
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT id FROM sales";
                    List<Transaction> transactions = new List<Transaction>();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            transactions.Add(GetTransaction(reader.GetDateTime("id")));

                        return transactions;
                    }
                }
            }
        }

        public void DeleteTransaction(DateTime id)
        {
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM sales WHERE id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
