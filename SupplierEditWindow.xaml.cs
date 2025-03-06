using System;
using System.Windows;
using Supermarket.Models;
using Supermarket.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Supermarket
{
    public partial class SupplierEditWindow : Window
    {
        private SupermarketContext _context;
        public Supplier SelectedSupplier { get; set; }

        public SupplierEditWindow(Supplier supplier)
        {
            InitializeComponent();
            _context = App.Current.ServiceProvider.GetRequiredService<SupermarketContext>();
            SelectedSupplier = supplier;
            DataContext = SelectedSupplier;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(SelectedSupplier.SupplierName))
            {
                MessageBox.Show("Supplier Name is required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Update the supplier in the database
                _context.Suppliers.Update(SelectedSupplier);
                _context.SaveChanges();

                DialogResult = true; // Indicate success
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating supplier: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Indicate cancellation
            Close();
        }
    }
}