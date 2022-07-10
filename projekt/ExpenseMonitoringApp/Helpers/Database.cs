using ExpenseMonitoringApp.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ExpenseMonitoringApp.Helpers
{
    public static class Database
    {
        public static ExpensesDbContext GetNewDbContext() => new ExpensesDbContext();

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
