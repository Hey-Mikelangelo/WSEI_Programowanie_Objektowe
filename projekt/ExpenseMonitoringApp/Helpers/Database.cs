using ExpenseMonitoringApp.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ExpenseMonitoringApp.Helpers
{
    public static class Database
    {
        /// <returns><see cref="ExpensesDbContext"/> database context</returns>
        public static ExpensesDbContext GetNewDbContext() => new ExpensesDbContext();

        /// <returns>List of strings representing money owners names in the database</returns>
        public static List<string> GetMoneyOwnersNames()
        {
            using (var db = GetNewDbContext())
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

        /// <returns>List of strings representing entries categories names in the database</returns>
        public static List<string> GetCategoryNames()
        {
            List<string> categoriesNames = new List<string>();
            using (var db = GetNewDbContext())
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
