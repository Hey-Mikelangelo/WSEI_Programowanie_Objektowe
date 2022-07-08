using Microsoft.EntityFrameworkCore;

namespace ExpenseMonitoringApp
{
    public class ExpensesDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<MoneyType> MoneyTypes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public string ConnectionString { get; }

        public ExpensesDbContext(string connectionString)
        {
            this.ConnectionString = connectionString;
            
            bool createdNewDatabase = Database.EnsureCreated();
            if (createdNewDatabase)
            {
                Categories.Add(new Category("Food"));
                MoneyTypes.Add(new MoneyType("PLN"));
                SaveChanges();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(this.ConnectionString);
        }
    }

}
