using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthTestApp.Models
{
    public class ToDoItem
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public ToDoItem(string description, Guid userId)
        {
            Description = description;
            UserId = userId;
        }
    }
}
