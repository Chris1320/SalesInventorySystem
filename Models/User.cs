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

        public User(int id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }
    }
}
