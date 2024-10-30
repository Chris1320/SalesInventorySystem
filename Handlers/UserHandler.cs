using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;
using SalesInventorySystem_WAM1.Models;

namespace SalesInventorySystem_WAM1.Handlers
{
    internal class UserHandler : DatabaseHandler
    {
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
            foreach (byte b in hpw)
                h.Append(b.ToString("x2"));
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
                    var hashed_pass = EncryptPassword(password);
                    command.CommandText =
                        "SELECT * FROM users WHERE username = @username AND userpass = @password";
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", hashed_pass);
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null; // Return null if the user does not exist.
                        UpdateUserLastLoginTimestamp(reader.GetInt32("id")); // Update the last login timestamp.
                        return new User(
                            reader.GetInt32("id"),
                            reader.GetString("username"),
                            reader.GetString("userpass"),
                            reader.IsDBNull(reader.GetOrdinal("name"))
                                ? string.Empty
                                : reader.GetString("name"),
                            reader.GetString("role"),
                            reader.IsDBNull(reader.GetOrdinal("last_login"))
                                ? DateTime.Now
                                : reader.GetDateTime("last_login")
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Update the last login timestamp of a user.
        /// </summary>
        /// <param name="user_id">The user to be updated.</param>
        public void UpdateUserLastLoginTimestamp(int user_id)
        {
            using (var connection = GetNewConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE users SET last_login = NOW() WHERE id = @user_id";
                    command.Parameters.AddWithValue("@user_id", user_id);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Get all users in the database.
        /// </summary>
        /// <returns>A list of users in the database.</returns>
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM users";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(
                                new User(
                                    reader.GetInt32("id"),
                                    reader.GetString("username"),
                                    reader.GetString("userpass"),
                                    reader.IsDBNull(reader.GetOrdinal("name"))
                                        ? string.Empty
                                        : reader.GetString("name"),
                                    reader.GetString("role"),
                                    reader.IsDBNull(reader.GetOrdinal("last_login"))
                                        ? DateTime.Now
                                        : reader.GetDateTime("last_login")
                                )
                            );
                        }
                    }
                }
            }
            return users;
        }

        /// <summary>
        /// Get a user by their ID.
        /// </summary>
        /// <param name="user_id">The ID of the user.</param>
        /// <returns>A User object, or null if the user does not exist.</returns>
        public User GetUser(int user_id)
        {
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM users WHERE id = @user_id";
                    command.Parameters.AddWithValue("@user_id", user_id);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User(
                                reader.GetInt32("id"),
                                reader.GetString("username"),
                                reader.GetString("userpass"),
                                reader.IsDBNull(reader.GetOrdinal("name"))
                                    ? string.Empty
                                    : reader.GetString("name"),
                                reader.GetString("role"),
                                reader.IsDBNull(reader.GetOrdinal("last_login"))
                                    ? DateTime.Now
                                    : reader.GetDateTime("last_login")
                            );
                        }
                        return null; // Return null if the user does not exist.
                    }
                }
            }
        }

        /// <summary>
        /// Delete a user from the database.
        /// </summary>
        /// <param name="user_id">The ID of the user to be removed.</param>
        public void DeleteUser(int user_id)
        {
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM users WHERE id = @user_id";
                    command.Parameters.AddWithValue("@user_id", user_id);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Add a new user to the database.
        /// </summary>
        /// <param name="userdata">The details of the new user.</param>
        public void AddUser(User userdata)
        {
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        "INSERT INTO users (username, userpass, name, role) VALUES (@username, @userpass, @name, @role)";
                    command.Parameters.AddWithValue("@username", userdata.Username);
                    command.Parameters.AddWithValue("@userpass", userdata.Password);
                    command.Parameters.AddWithValue("@name", userdata.Name);
                    command.Parameters.AddWithValue("@role", userdata.Role);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Update a user's details in the database.
        /// </summary>
        /// <param name="userdata">The modified user data</param>
        public void UpdateUser(User userdata)
        {
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        "UPDATE users SET username = @username, userpass = @userpass, name = @name, role = @role WHERE id = @id";
                    command.Parameters.AddWithValue("@username", userdata.Username);
                    command.Parameters.AddWithValue("@userpass", userdata.Password);
                    command.Parameters.AddWithValue("@name", userdata.Name);
                    command.Parameters.AddWithValue("@role", userdata.Role);
                    command.Parameters.AddWithValue("@id", userdata.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Search for users in the database.
        /// </summary>
        /// <param name="query">The user's query.</param>
        /// <returns>A list of Users that match the user's query.</returns>
        public List<User> SearchUsers(string query)
        {
            List<User> users = new List<User>();
            using (MySqlConnection connection = GetNewConnection())
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT * FROM users WHERE CONCAT_WS('', username, name, role) LIKE @search";
                    command.Parameters.AddWithValue("@search", $"%{query}%");
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(
                                new User(
                                    reader.GetInt32("id"),
                                    reader.GetString("username"),
                                    reader.GetString("userpass"),
                                    reader.IsDBNull(reader.GetOrdinal("name"))
                                        ? string.Empty
                                        : reader.GetString("name"),
                                    reader.GetString("role"),
                                    reader.IsDBNull(reader.GetOrdinal("last_login"))
                                        ? DateTime.Now
                                        : reader.GetDateTime("last_login")
                                )
                            );
                        }
                    }
                }
            }
            return users;
        }
    }
}
