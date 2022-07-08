using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ExpenseMonitoringApp
{
    public static class EntryControlsProvider
    {
        public static List<ExpenseEntryControl> GetEntryControls(bool withDeleteButton = true)
        {
            using (var db = Database.GetNewDbContext())
            {
                db.Entries
                    .Include(x => x.Category)
                    .Include(x => x.MoneyType)
                    .Load();

                List<ExpenseEntryControl> entriesControls = new List<ExpenseEntryControl>(db.Entries.Count());
                foreach (var entry in db.Entries)
                {
                    ExpenseEntryControl expenseEntryControl = new ExpenseEntryControl(entry.Id, withDeleteButton);
                    expenseEntryControl.Category = entry.Category.Name;
                    expenseEntryControl.Amount = entry.MoneyCount.ToString();
                    expenseEntryControl.MoneyType = entry.MoneyType.Name;
                    expenseEntryControl.Date = entry.CreationTime;
                    entriesControls.Add(expenseEntryControl);
                }
                entriesControls.Sort((x1, x2) => x2.Date.CompareTo(x1.Date));
                return entriesControls;

                db.MoneyTypes.Load();

            }
        }
    }
    public class AddEntriesViewModel
    {
        public event System.Action OnEntriesChanged;
        public AddEntriesViewModel()
        {
            
        }

        public List<ExpenseEntryControl> GetEntryControls(bool withDeleteButton = true)
        {
            List<ExpenseEntryControl> entriesControls = EntryControlsProvider.GetEntryControls(true);
            foreach (var entryControl in entriesControls)
            {
                entryControl.OnEntryDeleteClicked += DeleteEntry;
            }
            return entriesControls;
        }

        private void DeleteEntry(long entryId)
        {
            using (var db = Database.GetNewDbContext())
            {
                db.Entries.Include(x => x.Comment).Load();
                var entryToRemove = db.Entries.First(x => x.Id == entryId);
                db.Entries.Remove(entryToRemove);
                db.Comments.Remove(entryToRemove.Comment);
                db.SaveChanges();
                OnEntriesChanged?.Invoke();
            }
        }

        public List<string> GetMoneyTypesNames()
        {
            using (var db = Database.GetNewDbContext())
            {
                List<string> moneyTypesNames = new List<string>();

                var moneyTypes = db.MoneyTypes;
                foreach (var moneyType in moneyTypes)
                {
                    moneyTypesNames.Add(moneyType.Name);
                }
                return moneyTypesNames;
            }
           
        }

        public List<string> GetCategoryNames()
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

        public void AddEntry(string categoryName, string moneyCountString, string moneyTypeName, string commentText)
        {
            using (var db = Database.GetNewDbContext())
            {
                db.Categories.Load();
                db.MoneyTypes.Load();
                var selectedCategory = db.Categories.FirstOrDefault(x => x.Name == categoryName);
                var selectedMoneyType = db.MoneyTypes.FirstOrDefault(x => x.Name == moneyTypeName);
                if (selectedCategory == null)
                {
                    MessageBox.Show($"Category {categoryName} not found in db");
                    return;
                }
                if (selectedMoneyType == null)
                {
                    MessageBox.Show($"Money type {moneyTypeName} not found in db");
                    return;
                }

                decimal moneyAmount;
                if (decimal.TryParse(moneyCountString, out moneyAmount) == false)
                {
                    MessageBox.Show($"{moneyCountString} is not a decimal number");
                    return;
                }
                long categoryId = selectedCategory.Id;
                long moneyTypeId = selectedMoneyType.Id;
                Comment comment = new Comment(commentText);
                db.Comments.Add(comment);
                db.SaveChanges();
                var entry = new Entry(categoryId, moneyAmount, moneyTypeId, DateTime.Now, comment.Id);
                EntityEntry<Entry> addedEntry = db.Entries.Add(entry);
                db.SaveChanges();
            }
        }
    }
}
