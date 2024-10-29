using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInventorySystem_WAM1.Models
{
    public class User
    {
        public int Id { get; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public DateTime LastLogin { get; set; }

        public User(int id, string username, string password, string name, string role, DateTime last_login)
        {
            Id = id;
            Username = username;
            Password = password;
            Name = name == null ? null : name;
            Role = role;
            LastLogin = last_login;
        }
    }
}
