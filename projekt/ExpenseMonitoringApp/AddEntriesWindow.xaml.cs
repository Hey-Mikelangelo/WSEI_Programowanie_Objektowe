using ExpenseMonitoringApp.Helpers;
using ExpenseMonitoringApp.ViewModels;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ExpenseMonitoringApp
{
    public partial class AddEntriesWindow : Window
    {
        private AddEntriesViewModel model = new AddEntriesViewModel();
        public AddEntriesWindow()
        {
            InitializeComponent();
            DataContext = model;
            RefreshCategoryNames();
            RefreshMoneyOwners();
            RefreshEntries();
            ResetAllFields();
            AddEntryButton.Click += AddEntryButton_Click;
            model.OnEntriesChanged += RefreshEntries;
            ButtonGoToSummaryWindow.Click += ButtonGoToSummaryWindow_Click;
        }

        private void ButtonGoToSummaryWindow_Click(object sender, RoutedEventArgs e)
        {
            SpendingsSummaryWindow spendingsSummaryWindow = new SpendingsSummaryWindow();
            spendingsSummaryWindow.CopySizeAndPosition(this);
            spendingsSummaryWindow.Show();
            Close();
        }

        private void RefreshMoneyOwners()
        {
            var items = MoneyOwnerComboBox.Items;
            items.Clear();
            foreach (var item in Database.GetMoneyOwnersNames())
            {
                if (items.Contains(item) == false)
                {
                    items.Add(item);
                }
            }
            Button newOwnerButton = new Button();
            newOwnerButton.Content = "New Owner";
            newOwnerButton.Click += NewOwnerButton_Click;
            items.Add(newOwnerButton);
        }

        private void RefreshCategoryNames()
        {
            var items = CategoryComboBox.Items;
            items.Clear();
            foreach (var item in Database.GetCategoryNames())
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

        private void NewOwnerButton_Click(object sender, RoutedEventArgs e)
        {
            AddOwnerWindow addOwnerWindow = new AddOwnerWindow();
            addOwnerWindow.ShowDialog();
            RefreshMoneyOwners();
            MoneyOwnerComboBox.SelectedIndex = MoneyOwnerComboBox.Items.Count - 2;
        }

        private void RefreshEntries()
        {

            var stackChildren = EntriesStack.Children;
            stackChildren.Clear();
            foreach (var item in model.GetEntryControls())
            {
                stackChildren.Add(item);
            }
        }

        private void AddEntryButton_Click(object sender, RoutedEventArgs e)
        {
            var categoryName = CategoryComboBox.SelectedItem as string;
            char decimalSeparator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            string moneyCount = AmountTextBox.Text;
            char dotChar = '.';
            char commaChar = ',';
            if (decimalSeparator != commaChar)
            {
                moneyCount = moneyCount.Replace(commaChar, decimalSeparator);
            }
            if (decimalSeparator != dotChar)
            {
                moneyCount = moneyCount.Replace(dotChar, decimalSeparator);
            }
            var moneyOwnerName = MoneyOwnerComboBox.SelectedItem as string;
            string comment = CommentTextBox.Text;
            model.AddEntry(categoryName, moneyCount, moneyOwnerName, comment);
            RefreshEntries();
            ResetMoneyCount();
            ResetComment();
        }

        private void ResetAllFields()
        {
            ResetCategoryComboBox();
            ResetMoneyCount();
            ResetComment();
            ResetOwner();
        }

        private void ResetOwner()
        {
            if (MoneyOwnerComboBox.Items.Count > 0)
            {
                MoneyOwnerComboBox.SelectedItem = MoneyOwnerComboBox.Items[0];
            }
        }

        private void ResetMoneyCount()
        {
            AmountTextBox.Text = string.Empty;
        }

        private void ResetComment()
        {
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
