using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthTestApp.Models
{
    public class User
    {
        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public List<TaskToDo> Tasks { get; set; } = [];
        public User(string name, string password, string role)
        {
            Name = name;
            Password = password;
            Role = role;
        }
    }
}
