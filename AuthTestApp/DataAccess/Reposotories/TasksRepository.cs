using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthTestApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthTestApp.DataAccess.Reposotories
{
    public class TasksRepository
    {
        private readonly AuthDBContext _dbContext;

        public TasksRepository(AuthDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ToDoItem> Get()
        {
            return _dbContext.Tasks
                .AsNoTracking()
                .OrderBy(task => task.Description)
                .ToList();
        }

        public List<ToDoItem> GetByUser(Guid userId)
        {
            var user = _dbContext.Users.FirstOrDefault(user => user.Id == userId) 
                ?? throw new Exception("User was not found");
            return _dbContext.Tasks
                .AsNoTracking()
                .Where(task => task.User.Id == userId)
                .ToList();
        }

        public List<ToDoItem> GetByUsername(string username)
        {
            var user = _dbContext.Users.FirstOrDefault(user => user.Name == username)
                ?? throw new Exception("User was not found");
            return _dbContext.Tasks
                .AsNoTracking()
                .Where(task => task.User.Name == username)
                .ToList();
        }

        public void Add(string description, Guid userId)
        {
            var task = new ToDoItem(description, userId);
            _dbContext.Add(task);
            _dbContext.SaveChanges();
        }

        public void Delete(Guid taskId)
        {
            _dbContext.Tasks
                .Where(task => task.Id == taskId)
                .ExecuteDelete();
        }

        public void Update(Guid taskId, string description, Guid userId)
        {
            var task = _dbContext.Tasks.FirstOrDefault(task => task.Id == taskId)
                ?? throw new Exception("Task was not found");
            var user = _dbContext.Users.FirstOrDefault(user => user.Id == userId)
                ?? throw new Exception("User was not found");
            _dbContext.Tasks
                .Where(t => t.Id == taskId)
                .ExecuteUpdate(t => t
                .SetProperty(task => task.Description, description)
                .SetProperty(task => task.UserId, userId));
        }
    }
}
