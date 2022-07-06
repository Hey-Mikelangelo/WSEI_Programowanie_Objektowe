using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public static class DatatableExtensions
{
    public static DataTable ToDataTable<T>(this IEnumerable<T> data)
    {
        PropertyDescriptorCollection properties =
            TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new DataTable();
        foreach (PropertyDescriptor prop in properties)
            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        foreach (T item in data)
        {
            DataRow row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            table.Rows.Add(row);
        }
        return table;
    }

}

namespace ExpenseMonitoringApp
{
    /// <summary>
    /// Interaction logic for AddEntriesWindow.xaml
    /// </summary>
    public partial class AddEntriesWindow : Window
    {
        private AddEntriesViewModel model = new AddEntriesViewModel();
        public AddEntriesWindow()
        {
            InitializeComponent();
            this.DataContext = model;
            RefreshCategoryNames();
            RefreshMoneyTypes();
            RefreshEntries();
            AddEntryButton.Click += AddEntryButton_Click;
        }

        private void RefreshMoneyTypes()
        {
            var items = MoneyTypeComboBox.Items;
            items.Clear();
            foreach (var item in model.GetMoneyTypesNames())
            {
                if (items.Contains(item) == false)
                {
                    items.Add(item);
                }
            }
        }

        private void RefreshCategoryNames()
        {
            var items = CategoryComboBox.Items;
            items.Clear();
            foreach (var item in model.GetCategoryNames())
            {
                items.Add(item);
            }
        }

        private void RefreshEntries()
        {
            var stackChildren = EntriesStack.Children;
            stackChildren.Clear();
            foreach (var item in model.GetEntryControls())
            {
                if (stackChildren.Contains(item) == false)
                {
                    stackChildren.Add(item);
                }
            }
        }

        private void AddEntryButton_Click(object sender, RoutedEventArgs e)
        {
            var categoryName = CategoryComboBox.SelectedItem as string;
            string moneyCount = AmountTextBox.Text;
            var moneyTypeName = MoneyTypeComboBox.SelectedItem as string;
            model.AddEntry(categoryName, moneyCount, moneyTypeName);
            RefreshEntries();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(sender.ToString());
        }
    }


    public class AddEntriesViewModel
    {
        public ObservableCollection<string> Categories { get; private set; } = new ObservableCollection<string>();
        public DataTable EntriesDatatable { get; private set; }
        public AddEntriesViewModel()
        {
            using(var db = Database.GetNewDbContext())
            {
                foreach (Category category in db.Categories)
                {
                    this.Categories.Add(category.Name);
                }
                
                var datatable = db.Entries.ToDataTable();
                EntriesDatatable = datatable;
                DataColumn removeButtonColumn = new DataColumn();
            }
        }

        /*private void GenerateButton()
        {
            if (e.PropertyName == "#")
            {
                DataGridTemplateColumn buttonColumn = new DataGridTemplateColumn();
                DataTemplate buttonTemplate = new DataTemplate();
                FrameworkElementFactory buttonFactory = new FrameworkElementFactory(typeof(Button));
                buttonTemplate.VisualTree = buttonFactory;
                //add handler or you can add binding to command if you want to handle click
                buttonFactory.AddHandler(Button.ClickEvent, new RoutedEventHandler(button1_Click));
                buttonFactory.SetBinding(Button.ContentProperty, new Binding("#"));
                buttonColumn.CellTemplate = buttonTemplate;
                e.Column = buttonColumn;
            }
        }*/

        public List<ExpenseEntryControl> GetEntryControls()
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
                    ExpenseEntryControl expenseEntryControl = new ExpenseEntryControl(entry.Id);
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

        public void RemoveEntry(int index)
        {
            using(var db = Database.GetNewDbContext())
            {
                if(index < 0 || index >= db.Entries.Count())
                {
                    throw new Exception($"index {index} is out of bounds for Entries list");
                }
                var entryAtIndex = db.Entries.OrderBy(x => x.Id).ToList()[index];
                db.Entries.Remove(entryAtIndex);
            }
        }

        public void AddEntry(string categoryName, string moneyCountString, string moneyTypeName)
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
                var entry = new Entry(categoryId, moneyAmount, moneyTypeId, DateTime.Now);
                EntityEntry<Entry> addedEntry = db.Entries.Add(entry);
                db.SaveChanges();
            }
        }
    }
}
