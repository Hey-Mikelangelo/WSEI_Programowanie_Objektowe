using ExpenseMonitoringApp.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

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
            using (var db = Database.GetNewDbContext())
            {
                db.Categories.Load();
                string newCategoryName = CategorynameTextBox.Text;
                if (newCategoryName == string.Empty)
                {
                    MessageBox.Show($"Category name canot be empty");
                    return;
                }
                if (db.Categories.Any(x => x.Name == newCategoryName))
                {
                    MessageBox.Show($"Category with name \"{newCategoryName}\" already exists");
                    return;
                }
                Category category = new Category(newCategoryName);
                db.Categories.Add(category);
                db.SaveChanges();
            }
            Close();
        }
    }
}
