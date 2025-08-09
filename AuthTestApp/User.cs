using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthTestApp
{
    internal class User
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public User(string name, string password, string role) 
        {
            Id = Guid.NewGuid();
            Name = name;
            Password = password;
            Role = role;
        }
    }
}
