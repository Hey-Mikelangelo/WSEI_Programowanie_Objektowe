using ExpenseMonitoringApp.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ExpenseMonitoringApp
{
    public partial class SpendingsSummaryWindow : Window
    {
        public SpendingsSummaryWindow()
        {
            InitializeComponent();
            SetupMoneyOwnersComboBox();
            DatePickerFrom.SelectedDate = DateTime.Now.AddDays(-30);
            DatePickerTo.SelectedDate = DateTime.Now;

            DatePickerFrom.CalendarClosed += DatePickerFrom_CalendarClosed;
            DatePickerTo.CalendarClosed += DatePickerTo_CalendarClosed;
            MoneyOwnerComboBox.SelectionChanged += MoneyOwnerComboBox_SelectionChanged;

            ButtonGoToAddingEntriesWindow.Click += ButtonGoToAddingEntriesWindow_Click;

            UpdateEntries();
        }

        private void ButtonGoToAddingEntriesWindow_Click(object sender, RoutedEventArgs e)
        {
            AddEntriesWindow addEntriesWindow = new AddEntriesWindow();
            addEntriesWindow.Show();
            addEntriesWindow.CopySizeAndPosition(this);
            Close();
        }

        private void MoneyOwnerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateEntries();
        }

        private void DatePickerTo_CalendarClosed(object sender, RoutedEventArgs e)
        {
            UpdateEntries();
        }

        private void DatePickerFrom_CalendarClosed(object sender, RoutedEventArgs e)
        {
            UpdateEntries();
        }

        private void UpdateEntries()
        {
            if (DatePickerFrom.SelectedDate.HasValue == false || DatePickerTo.SelectedDate.HasValue == false)
            {
                return;
            }
            List<ExpenseEntryControl> entriesControls;
            using (var db = Database.GetNewDbContext())
            {
                db.Entries
                    .Include(x => x.Category)
                    .Include(x => x.MoneyOwner)
                    .Include(x => x.Comment)
                    .Load();

                DateTime fromDate = DatePickerFrom.SelectedDate.Value;
                DateTime toDate = DatePickerTo.SelectedDate.Value.AddDays(1);

                var entries = db.Entries.Where(x => x.CreationTime <= toDate && x.CreationTime >= fromDate);
                if (MoneyOwnerComboBox.SelectedIndex != 0)
                {
                    string ownerName = MoneyOwnerComboBox.SelectedItem as string;
                    entries = entries.Where(x => x.MoneyOwner.Name == ownerName);
                }
                entriesControls = EntryControlsProvider.GetEntryControls(entries, false);
                SpendingSummaryDataGrid.ItemsSource = GetSpendingsSummaryEntries(entries);
            }

            var stackChildren = EntriesStack.Children;
            stackChildren.Clear();
            foreach (var item in entriesControls)
            {
                stackChildren.Add(item);
            }
        }
        private void SetupMoneyOwnersComboBox()
        {
            var items = MoneyOwnerComboBox.Items;
            items.Clear();
            TextBlock allOwnersVariantTextBlock = new TextBlock();
            allOwnersVariantTextBlock.Text = "All";
            items.Add(allOwnersVariantTextBlock);
            foreach (var item in Database.GetMoneyOwnersNames())
            {
                items.Add(item);
            }
            MoneyOwnerComboBox.SelectedIndex = 0;
        }

        private IEnumerable<SpendingsSummaryEntry> GetSpendingsSummaryEntries(IEnumerable<Entry> entries)
        {
            var moneyCountByCategory = entries.GroupBy(entry => entry.Category, entry => entry.MoneyCount);
            var spendingsSummaryEntries = new List<SpendingsSummaryEntry>(moneyCountByCategory.Count());
            foreach (var item in moneyCountByCategory)
            {
                var spendingsSummaryEntry = new SpendingsSummaryEntry(item.Key.Name, item.Sum());
                spendingsSummaryEntries.Add(spendingsSummaryEntry);
            }
            spendingsSummaryEntries.Add(new SpendingsSummaryEntry("All", moneyCountByCategory.SelectMany(x => x).Sum()));
            return spendingsSummaryEntries;
        }
        private class SpendingsSummaryEntry
        {
            public string CategoryName { get; set; }
            public decimal Amount { get; set; }
            public SpendingsSummaryEntry(string categoryName, decimal amount)
            {
                CategoryName = categoryName;
                Amount = amount;
            }
        }
    }

    
}
