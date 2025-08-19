using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AuthTestApp.DataAccess.Configurations;
using AuthTestApp.Models;
using Avalonia.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthTestApp.DataAccess
{
    public class AuthDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TaskToDo> Tasks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = @"D:\Programming\C#\AuthTestApp\AuthTestApp\Users.db";
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
