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
    /// Interaction logic for AddNewCategoryWindow.xaml
    /// </summary>
    public partial class AddNewCategoryWindow : Window
    {
        public AddNewCategoryWindow()
        {
            InitializeComponent();
            AddButton.Click += AddButton_Click;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            using(var db = Database.GetNewDbContext())
            {
                db.Categories.Load();
                string newCategoryName = CategorynameTextBox.Text;
                if(newCategoryName == String.Empty)
                {
                    MessageBox.Show($"Category name canot be empty");
                    return;
                }
                if(db.Categories.Any(x => x.Name == newCategoryName))
                {
                    MessageBox.Show($"Category with name \"{newCategoryName}\" already exists");
                    return;
                }
                Category category = new Category(newCategoryName);
                db.Categories.Add(category);
                db.SaveChanges();
            }
            this.Close();
        }
    }
}
