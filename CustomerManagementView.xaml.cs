using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Supermarket.Data;
using Supermarket.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Supermarket
{
    public partial class CustomerManagementView : UserControl
    {
        private SupermarketContext _context;
        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>();

        public CustomerManagementView()
        {
            InitializeComponent();

            // Get the DbContext from DI
            _context = App.Current.ServiceProvider.GetRequiredService<SupermarketContext>();

            // Load Customers
            LoadCustomersAsync();

            // Set the DataContext.
            DataContext = this;
        }

        private async Task LoadCustomersAsync()
        {
            try
            {
                Customers.Clear(); // Clear existing data before loading

                var customerList = await _context.Customers.ToListAsync();

                foreach (var customer in customerList)
                {
                    Customers.Add(customer);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                await LoadCustomersAsync(); // Reload all customers if search is empty
                return;
            }

            try
            {
                Customers.Clear();

                // Perform search in the database, or use in-memory collection if appropriate
                var searchResults = await _context.Customers
                    .Where(c => c.Name.ToLower().Contains(searchText) || c.CustomerID.ToString() == searchText)
                    .ToListAsync(); // Load the results

                foreach (var customer in searchResults)
                {
                    Customers.Add(customer);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching customers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadCustomersAsync();
        }

        private async void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerAddWindow addCustomerWindow = new CustomerAddWindow();
            bool? result = addCustomerWindow.ShowDialog();

            if (result == true) // If the user clicked "Save" in the Add window
            {
                await LoadCustomersAsync(); // Refresh the customer list in the DataGrid
            }
        }

        private async void UpdateCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected customer from the DataGrid
            Customer selectedCustomer = CustomerDataGrid.SelectedItem as Customer;

            if (selectedCustomer == null)
            {
                MessageBox.Show("Please select a customer to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Open an update window, passing in the selected customer
            CustomerEditWindow updateCustomerWindow = new CustomerEditWindow(selectedCustomer);
            bool? result = updateCustomerWindow.ShowDialog();

            if (result == true) // If the user clicked "Save" in the Update window
            {
                await LoadCustomersAsync(); // Refresh the customer list in the DataGrid
            }
        }

        private async void RemoveCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected customer from the DataGrid
            Customer selectedCustomer = CustomerDataGrid.SelectedItem as Customer;

            if (selectedCustomer == null)
            {
                MessageBox.Show("Please select a customer to remove.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Confirm deletion with the user
            MessageBoxResult confirmResult = MessageBox.Show(
                $"Are you sure you want to remove {selectedCustomer.Name}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (confirmResult != MessageBoxResult.Yes)
            {
                return; // User canceled the deletion
            }

            try
            {
                // Remove the customer from the database
                _context.Customers.Remove(selectedCustomer);
                await _context.SaveChangesAsync();

                // Remove the customer from the ObservableCollection
                Customers.Remove(selectedCustomer);

                MessageBox.Show("Customer removed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing customer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}