using ExpenseMonitoringApp.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

namespace ExpenseMonitoringApp
{
    public partial class AddOwnerWindow : Window
    {
        public AddOwnerWindow()
        {
            InitializeComponent();
            AddButton.Click += AddButton_Click;


        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = Database.GetNewDbContext())
            {
                db.MoneyOwners.Load();
                string newOwnerName = OwnerNameTextBox.Text;
                if (newOwnerName == string.Empty)
                {
                    MessageBox.Show($"Owner name canot be empty");
                    return;
                }
                if (db.Categories.Any(x => x.Name == newOwnerName))
                {
                    MessageBox.Show($"Owner with name \"{newOwnerName}\" already exists");
                    return;
                }
                Owner owner = new Owner(newOwnerName);
                db.MoneyOwners.Add(owner);
                db.SaveChanges();
            }
            Close();
        }
    }
}
