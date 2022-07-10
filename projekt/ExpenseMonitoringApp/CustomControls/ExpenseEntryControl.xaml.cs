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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExpenseMonitoringApp
{
    /// <summary>
    /// Interaction logic for ExpenseEntry.xaml
    /// </summary>
    public partial class ExpenseEntryControl : UserControl
    {
        public event System.Action<long>? OnEntryDeleteClicked;
        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("Categorya", typeof(string), typeof(ExpenseEntryControl), 
                new PropertyMetadata("Empty category"));



        public string Amount
        {
            get { return (string)GetValue(AmountProperty); }
            set { SetValue(AmountProperty, value); }
        }

        public static readonly DependencyProperty AmountProperty =
            DependencyProperty.Register("Amount", typeof(string), typeof(ExpenseEntryControl), new PropertyMetadata("0"));



        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTime), typeof(ExpenseEntryControl), new PropertyMetadata(DateTime.Now));


        public string Comment
        {
            get { return (string)GetValue(CommentProperty); }
            set { SetValue(CommentProperty, value); }
        }

        public static readonly DependencyProperty CommentProperty =
            DependencyProperty.Register("Comment", typeof(string), typeof(ExpenseEntryControl), new PropertyMetadata(string.Empty));


        public string MoneyOwner
        {
            get { return (string)GetValue(MoneyOwnerProperty); }
            set { SetValue(MoneyOwnerProperty, value); }
        }

        public static readonly DependencyProperty MoneyOwnerProperty =
            DependencyProperty.Register("MoneyOwner", typeof(string), typeof(ExpenseEntryControl), new PropertyMetadata("Money"));

        public long EntryIndex { get; set; }
        public ExpenseEntryControl(long entryIndex, bool withDeleteButton)
        {
            InitializeComponent();
            this.DataContext = this;
            EntryIndex = entryIndex;
            if(withDeleteButton == false)
            {
                FieldsContainer.Children.Remove(DeleteButton);
                FieldsContainer.ColumnDefinitions.RemoveAt(FieldsContainer.ColumnDefinitions.Count - 1);
            }
            else
            {
                DeleteButton.Click += DeleteButton_Click;
            }
        }
        ~ExpenseEntryControl()
        {
            DeleteButton.Click -= DeleteButton_Click;
            OnEntryDeleteClicked = null;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Delete?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if(messageBoxResult == MessageBoxResult.Yes)
            {
                OnEntryDeleteClicked?.Invoke(EntryIndex);
            }
        }
    }
}
