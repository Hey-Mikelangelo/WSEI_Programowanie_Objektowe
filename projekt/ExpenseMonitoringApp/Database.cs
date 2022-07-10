using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ExpenseMonitoringApp
{
    public static class Database
    {
        public static ExpensesDbContext GetNewDbContext() => new ExpensesDbContext(ConnectionString);

        public static readonly string DatabasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"database\ExpensesDB.db");
        public static readonly string ConnectionString = @$"Data Source={Database.DatabasePath};";

        public static List<string> GetMoneyOwnersNames()
        {
            using (var db = Database.GetNewDbContext())
            {
                List<string> moneyOwnersNames = new List<string>();

                var moneyOwners = db.MoneyOwners;
                foreach (var moneyOwner in moneyOwners)
                {
                    moneyOwnersNames.Add(moneyOwner.Name);
                }
                return moneyOwnersNames;
            }

        }

        public static List<string> GetCategoryNames()
        {
            List<string> categoriesNames = new List<string>();
            using (var db = Database.GetNewDbContext())
            {
                db.Categories.Load();
                var categories = db.Categories;
                foreach (var category in categories)
                {
                    categoriesNames.Add(category.Name);
                }
            }
            return categoriesNames;
        }
    }
}
