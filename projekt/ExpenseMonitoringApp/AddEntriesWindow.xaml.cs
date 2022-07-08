using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            ResetAllFields();
            AddEntryButton.Click += AddEntryButton_Click;
            model.OnEntriesChanged += RefreshEntries;
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
            Button newCategoryButton = new Button();
            newCategoryButton.Content = "New Category";
            newCategoryButton.Click += NewCategoryButton_Click;
            items.Add(newCategoryButton);
        }

        private void NewCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewCategoryWindow addNewCategoryWindow = new AddNewCategoryWindow();
            addNewCategoryWindow.ShowDialog();
            RefreshCategoryNames();
            CategoryComboBox.SelectedIndex = CategoryComboBox.Items.Count - 2;
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
            string comment = CommentTextBox.Text;
            model.AddEntry(categoryName, moneyCount, moneyTypeName, comment);
            RefreshEntries();
            ResetAllFields();
        }

        private void ResetAllFields()
        {
            ResetCategoryComboBox();
            AmountTextBox.Text = string.Empty;
            if (MoneyTypeComboBox.Items.Count > 0)
            {
                MoneyTypeComboBox.SelectedItem = MoneyTypeComboBox.Items[0];
            }
            CommentTextBox.Text = string.Empty;
        }

        private void ResetCategoryComboBox()
        {
            if (CategoryComboBox.Items.Count > 0)
            {
                CategoryComboBox.SelectedItem = CategoryComboBox.Items[0];
            }
        }
    }
}
