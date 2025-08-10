using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AuthTestApp.DataAccess
{
    public class UsersRepository
    {
        private readonly AuthDBContext _dbContext;

        public UsersRepository(AuthDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<User> Get()
        {
            return _dbContext.Users
                .AsNoTracking()
                .OrderBy(user => user.Role)
                .ThenBy(user => user.Name)
                .ToList();
        }

        public User GetByUsername(string username)
        {
            return _dbContext.Users
                .AsNoTracking()
                .FirstOrDefault(user => user.Name == username);
        }

        public void Add(string name, string password, string role)
        {
            var user = new User(name, password, role);
            _dbContext.Add(user);
            _dbContext.SaveChanges();
        }

        public void Delete(string name)
        {
            _dbContext.Users
                .Where(user => user.Name == name)
                .ExecuteDelete();
        }

        public bool UsernameUsed(string username)
        {
            return _dbContext.Users
                .Any(user => user.Name == username);
        }

        public void Update(string username, string password, string role)
        {
            _dbContext.Users
                .Where(user => user.Name == username)
                .ExecuteUpdate(s => s
                .SetProperty(user => user.Password, password)
                .SetProperty(user => user.Role, role));
        }
    }
}
