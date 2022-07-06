using System;



namespace ExpenseMonitoringApp
{
    public static class Database
    {
        public static ExpensesDbContext GetNewDbContext() => new ExpensesDbContext(ConnectionString);

        public static readonly string DatabasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"database\ExpensesDB.db");
        public static readonly string ConnectionString = @$"Data Source={Database.DatabasePath};";
    }
}
