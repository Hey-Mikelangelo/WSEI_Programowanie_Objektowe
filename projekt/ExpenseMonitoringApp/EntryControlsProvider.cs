using System.Collections.Generic;
using System.Linq;

namespace ExpenseMonitoringApp
{
    public static class EntryControlsProvider
    {
        public static List<ExpenseEntryControl> GetEntryControls(IEnumerable<Entry> entries, bool withDeleteButton = true)
        {
            List<ExpenseEntryControl> entriesControls = new List<ExpenseEntryControl>(entries.Count());
            foreach (var entry in entries)
            {
                ExpenseEntryControl expenseEntryControl = new ExpenseEntryControl(entry.Id, withDeleteButton);
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
