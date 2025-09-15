using _02_CRUD_Interface;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _03_SportShopUA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DbHelper dbHelper;

        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["SportShopDbConnection"].ConnectionString;
            dbHelper = new DbHelper(connectionString);
            

        }

        private void ShowProducts_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = dbHelper.GetProducts().DefaultView;
        }

        private void ShowEmployees_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = dbHelper.GetEmployees().DefaultView;
        }

        private void ShowClients_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = dbHelper.GetClients().DefaultView;
        }

        private void ShowSalles_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = dbHelper.GetSalles().DefaultView;
        }


    }
}