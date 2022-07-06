using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseMonitoringApp.Models
{
    internal class Entry
    {

        public string Category { get; }
        public float MoneyCount { get; }
        public string MoneyType { get; }
        public DateTime DateTime { get; }
        public Entry(string category, float moneyCount, string moneyType, DateTime dateTime)
        {
            Category = category;
            MoneyCount = moneyCount;
            MoneyType = moneyType;
            DateTime = dateTime;
        }
    }

    internal class Entries
    {
        public List<Entry> entries;

        public void LoadEntries()
        {
            
        }
    }
}
