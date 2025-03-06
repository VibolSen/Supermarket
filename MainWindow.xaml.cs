using System.Windows;
using Supermarket.Views; // Ensure WelcomePage is inside Views namespace

namespace Supermarket
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Ensure WelcomePage is properly loaded on startup
            MainContent.Content = new WelcomePage();
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new InventoryView();
        }

        private void SalesBillingButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SalesBillingView();
        }

        private void CustomerManagementButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new CustomerManagementView();
        }

        private void EmployeeManagementButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new EmployeeManagementView();
        }

        private void SupplierManagementButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SupplierManagementView();
        }

        private void DiscountsPromotionsButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new DiscountsPromotionsView();
        }

        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ReportsView();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SettingsView();
        }
    }
}
