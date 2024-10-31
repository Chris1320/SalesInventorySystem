using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SalesInventorySystem_WAM1.Models;

namespace SalesInventorySystem_WAM1.Handlers
{
    internal class SalesHandler : DatabaseHandler
    {
        /// <summary>
        /// Add a transaction to the database.
        /// </summary>
        /// <param name="transaction">The transaction to be added.</param>
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

        /// <summary>
        /// Retrieve a transaction from the database.
        /// </summary>
        /// <param name="id">The ID of the transaction to be retrieved.</param>
        /// <returns>A Transaction object, or null if it does not exist.</returns>
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
            return null; // Return null if the transaction does not exist.
        }

        /// <summary>
        /// Get all transactions from the database.
        /// </summary>
        /// <returns>A list of Transactions.</returns>
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

        /// <summary>
        /// Remove a transaction from the database.
        /// </summary>
        /// <param name="id">The ID of the transacation to be deleted.</param>
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

        /// <summary>
        /// Update a transaction in the database.
        /// </summary>
        /// <param name="transaction">The modified Transaction object.</param>
        public void UpdateTransaction(Transaction transaction)
        {
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        "UPDATE sales SET item_id = @item_id, category = @category, price = @price, quantity = @quantity, status = @status, notes = @notes WHERE id = @id";
                    command.Parameters.AddWithValue("@item_id", transaction.ItemId);
                    command.Parameters.AddWithValue("@category", transaction.Category);
                    command.Parameters.AddWithValue("@price", transaction.Price);
                    command.Parameters.AddWithValue("@quantity", transaction.Quantity);
                    command.Parameters.AddWithValue("@status", transaction.Status);
                    command.Parameters.AddWithValue("@notes", transaction.Notes);
                    command.Parameters.AddWithValue("@id", transaction.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Search for transactions in the database that match the user's query.
        /// </summary>
        /// <param name="query">The user's query.</param>
        /// <returns>A list of Transactions that match the user's query.</returns>
        public List<Transaction> SearchTransactions(string query)
        {
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    // Join the sales and items tables to get the item name.
                    command.CommandText =
                        "SELECT sales.*, items.name AS item_name FROM sales JOIN items ON sales.item_id = items.id WHERE CONCAT_WS('', sales.id, sales.item_id, items.name, sales.category, sales.price, sales.quantity, sales.status, sales.notes) LIKE @query";
                    command.Parameters.AddWithValue("@query", $"%{query}%");
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
    }
}
