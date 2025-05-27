using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShelfMate.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace ShelfMate.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
                options.UseNpgsql("Host=localhost;Port=5432;Database=LibraryDB;Username=post;Password=post");
         
        }

    }
}
