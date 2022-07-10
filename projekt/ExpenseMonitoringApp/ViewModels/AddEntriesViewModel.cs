using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ExpenseMonitoringApp.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ExpenseMonitoringApp.ViewModels
{
    public class AddEntriesViewModel
    {
        public event Action OnEntriesChanged;

        public List<ExpenseEntryControl> GetEntryControls(bool withDeleteButton = true)
        {
            using (var db = Database.GetNewDbContext())
            {
                db.Entries
                    .Include(x => x.Category)
                    .Include(x => x.MoneyOwner)
                    .Include(x => x.Comment)
                    .Load();

                List<ExpenseEntryControl> entriesControls = EntryControlsProvider.GetEntryControls(db.Entries, true);
                foreach (var entryControl in entriesControls)
                {
                    entryControl.OnEntryDeleteClicked += DeleteEntry;
                }
                return entriesControls;
            }

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

        public void AddEntry(string categoryName, string moneyCountString, string moneyOwnerName, string commentText)
        {
            using (var db = Database.GetNewDbContext())
            {
                db.Categories.Load();
                db.MoneyOwners.Load();
                var selectedCategory = db.Categories.FirstOrDefault(x => x.Name == categoryName);
                var selectedMoneyOwner = db.MoneyOwners.FirstOrDefault(x => x.Name == moneyOwnerName);
                if (selectedCategory == null)
                {
                    MessageBox.Show($"Category {categoryName} not found in db");
                    return;
                }
                if (selectedMoneyOwner == null)
                {
                    MessageBox.Show($"Money Owner {moneyOwnerName} not found in db");
                    return;
                }

                decimal moneyAmount;
                if(moneyCountString.Length == 0)
                {
                    return;
                }
                if (decimal.TryParse(moneyCountString, out moneyAmount) == false)
                {
                    MessageBox.Show($"{moneyCountString} is not a decimal number");
                    return;
                }
                if(moneyAmount <= 0)
                {
                    MessageBox.Show($"Money count cannot be less than 0");
                    return;
                }
                long categoryId = selectedCategory.Id;
                long moneyOwnerId = selectedMoneyOwner.Id;
                Comment comment = new Comment(commentText);
                db.Comments.Add(comment);
                db.SaveChanges();
                var entry = new Entry(categoryId, moneyAmount, moneyOwnerId, DateTime.Now, comment.Id);
                EntityEntry<Entry> addedEntry = db.Entries.Add(entry);
                db.SaveChanges();
            }
        }
    }
}
