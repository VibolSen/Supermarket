using System;
using System.Windows;
using Supermarket.Models;
using Supermarket.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Supermarket
{
    public partial class CustomerEditWindow : Window
    {
        private SupermarketContext _context;
        public Customer CustomerToEdit { get; set; }

        public CustomerEditWindow(Customer customer)
        {
            InitializeComponent();
            _context = App.Current.ServiceProvider.GetRequiredService<SupermarketContext>();

            CustomerToEdit = customer; // Receive customer to edit
            DataContext = CustomerToEdit; // Bind to existing customer
        }

        private async Task SaveCustomerAsync()
        {
            //moved to async method, because async operations should be in async methods
            try
            {
                //Validate data before saving
                if (string.IsNullOrWhiteSpace(NameTextBox.Text))
                {
                    MessageBox.Show("Name is required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    NameTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(PhoneTextBox.Text))
                {
                    MessageBox.Show("Phone is required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    PhoneTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
                {
                    MessageBox.Show("Email is required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    EmailTextBox.Focus();
                    return;
                }

                if (!int.TryParse(LoyaltyPointsTextBox.Text, out int loyaltyPoints))
                {
                    MessageBox.Show("Invalid loyalty points. Please enter a valid number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    LoyaltyPointsTextBox.Focus();
                    return;
                }

                CustomerToEdit.LoyaltyPoints = loyaltyPoints;

                _context.Customers.Update(CustomerToEdit); //Use Update method here instead of Add
                await _context.SaveChangesAsync();

                DialogResult = true;
                Close();
            }
            catch (DbUpdateException ex)
            {
                string errorMessage = $"Error saving customer: {ex.Message}\n";
                if (ex.InnerException != null)
                {
                    errorMessage += $"Inner Exception: {ex.InnerException.Message}\n";
                }

                if (ex.InnerException?.Message.Contains("UNIQUE constraint") == true)
                {
                    errorMessage = "This email or phone number already exists. Please provide unique data.";
                }

                errorMessage += $"Stack Trace: {ex.StackTrace}";
                MessageBox.Show(errorMessage, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unexpected error occurred: {ex.Message}\nStack Trace: {ex.StackTrace}";
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await SaveCustomerAsync();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}