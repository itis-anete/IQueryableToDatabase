using Microsoft.EntityFrameworkCore;
using System;

namespace Tests
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<MajorPayne> MajorPayne { get; set; }
        public DbSet<Boba> Boba { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(AppSettings.LocalConnectionString);
        }
    }
}