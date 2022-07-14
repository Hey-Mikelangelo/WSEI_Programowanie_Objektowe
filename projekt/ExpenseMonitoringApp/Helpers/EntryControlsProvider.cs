using System.Collections.Generic;
using System.Linq;

namespace ExpenseMonitoringApp.Helpers
{
    public static class EntryControlsProvider
    {
        /// <summary>
        /// Use to get <see cref="ExpenseEntryControl"/> instances wrapping entries (<see cref="Entry"/>).
        /// </summary>
        /// <param name="entries">entries that will be wrappen in <see cref="ExpenseEntryControl"/></param>
        /// <param name="withDeleteButton">flag to create  <see cref="ExpenseEntryControl"/>
        /// with or without delete button</param>
        /// <returns></returns>
        public static List<ExpenseEntryControl> GetEntryControls(IEnumerable<Entry> entries, 
            bool withDeleteButton = true)
        {
            List<ExpenseEntryControl> entriesControls = new List<ExpenseEntryControl>(entries.Count());
            foreach (var entry in entries)
            {
                ExpenseEntryControl expenseEntryControl = 
                    new ExpenseEntryControl(entry.Id, withDeleteButton);
                expenseEntryControl.Category = entry.Category.Name;
                expenseEntryControl.Amount = entry.MoneyCount.ToString();
                expenseEntryControl.MoneyOwner = entry.MoneyOwner.Name;
                expenseEntryControl.Date = entry.CreationTime;
                expenseEntryControl.Comment = entry.Comment.Text;
                entriesControls.Add(expenseEntryControl);
            }
            entriesControls.Sort((x1, x2) => x2.Date.CompareTo(x1.Date));
            return entriesControls;
        }
    }
}
