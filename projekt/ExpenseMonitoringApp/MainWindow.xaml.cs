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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private void ButtonAddName_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtName.Text) && !lstNames.Items.Contains(txtName.Text))
            {
                lstNames.Items.Add(txtName.Text);
                txtName.Clear();
            }
        }

        private void Start()
        {
            string connectionString = @$"Data Source={Database.DatabasePath};";

            using (var db = Database.GetNewDbContext())
            {
                Console.WriteLine($"Database ConnectionString: {db.ConnectionString}.");

                // Read
                Console.WriteLine("Querying for a money types");
                
                var moneyTypes = db.MoneyTypes;
              
                var entries = db.Entries.Include(x => x.MoneyType).Include(x => x.Category);
                foreach (var item in entries)
                {
                    var moneyType = item.MoneyType;
                }
                foreach (var item in moneyTypes)
                {
                    var type = item;
                }

                // Update
                /* Console.WriteLine("Updating the blog and adding a post");

                 blog.Url = "https://devblogs.microsoft.com/dotnet";s
                 blog.Posts.Add(new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
                 db.SaveChanges();

                 // Delete
                 Console.WriteLine("Delete the blog");

                 db.Remove(blog);
                 db.SaveChanges();*/
            }

        }
    }

    public class Expenses
    {
        public ExpenseEntryControl[] ExpenseEntries => GetExpenseEntries();
        public ExpenseEntryControl[] ExpenseEntries2 => GetExpenseEntries();

        private ExpenseEntryControl[] entries = new ExpenseEntryControl[3];
        public ExpenseEntryControl[] GetExpenseEntries()
        {
            entries[0] ??= new ExpenseEntryControl() { Category = "Food", Amount = "100" };
            entries[1] ??= new ExpenseEntryControl() { Category = "Rent", Amount = 500.ToString() };
            entries[2] ??= new ExpenseEntryControl() { Category = "Car", Amount = 200.ToString() };

            return entries;
        }
    }
}
