using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace ExpenseMonitoringApp.Db
{
    public class ExpensesDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<MoneyType> MoneyOwners { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public static readonly string DatabasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"database\ExpensesDB.db");
        public static readonly string ConnectionString = @$"Data Source={DatabasePath};";
        public ExpensesDbContext()
        {
            string directoryPath = Path.GetDirectoryName(DatabasePath);
            if(Directory.Exists(directoryPath) == false)
            {
                Directory.CreateDirectory(directoryPath);
            }
            bool createdNewDatabase = Database.EnsureCreated();
            if (createdNewDatabase)
            {
                SaveChanges();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(ConnectionString);
        }
    }

}
