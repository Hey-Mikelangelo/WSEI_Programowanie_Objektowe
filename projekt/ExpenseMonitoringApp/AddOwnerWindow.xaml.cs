using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ExpenseMonitoringApp
{
    /// <summary>
    /// Interaction logic for AddOwnerWindow.xaml
    /// </summary>
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
                if (newOwnerName == String.Empty)
                {
                    MessageBox.Show($"Owner name canot be empty");
                    return;
                }
                if (db.Categories.Any(x => x.Name == newOwnerName))
                {
                    MessageBox.Show($"Owner with name \"{newOwnerName}\" already exists");
                    return;
                }
                MoneyType owner = new MoneyType(newOwnerName);
                db.MoneyOwners.Add(owner);
                db.SaveChanges();
            }
            this.Close();
        }
    }
}
