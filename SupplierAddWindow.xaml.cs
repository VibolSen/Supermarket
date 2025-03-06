using System;
using System.Windows;
using Supermarket.Models;
using Supermarket.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Supermarket
{
    public partial class SupplierAddWindow : Window
    {
        private SupermarketContext _context;
        public Supplier NewSupplier { get; set; }

        public SupplierAddWindow()
        {
            InitializeComponent();
            _context = App.Current.ServiceProvider.GetRequiredService<SupermarketContext>();
            NewSupplier = new Supplier();
            DataContext = NewSupplier;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(NewSupplier.SupplierName))
            {
                MessageBox.Show("Supplier Name is required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Add the new supplier to the database
                _context.Suppliers.Add(NewSupplier);
                _context.SaveChanges();

                DialogResult = true; // Indicate success
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving supplier: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Indicate cancellation
            Close();
        }
    }
}