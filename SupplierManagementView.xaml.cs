using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Supermarket.Data;
using Supermarket.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Supermarket
{
    public partial class SupplierManagementView : UserControl
    {
        private SupermarketContext _context;
        public ObservableCollection<Supplier> Suppliers { get; set; } = new ObservableCollection<Supplier>();

        public SupplierManagementView()
        {
            InitializeComponent();

            // Get the DbContext from DI container
            _context = App.Current.ServiceProvider.GetRequiredService<SupermarketContext>();

            // Load the data
            LoadSuppliers();

            // Set the DataContext to the current view
            DataContext = this;
        }

        private void LoadSuppliers()
        {
            try
            {
                // Load the data from the database
                var supplierList = _context.Suppliers.ToList();

                // Clear the existing collection
                Suppliers.Clear();

                // Add the new data to the ObservableCollection
                foreach (var supplier in supplierList)
                {
                    Suppliers.Add(supplier);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading suppliers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadSuppliers();
        }

        private void AddSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the Add Supplier window
            SupplierAddWindow addSupplierWindow = new SupplierAddWindow();
            bool? result = addSupplierWindow.ShowDialog();

            if (result == true) // If the user clicked "Save" in the Add window
            {
                // Refresh the supplier list
                LoadSuppliers();
            }
        }

        private void UpdateSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected supplier from the DataGrid
            Supplier selectedSupplier = SupplierDataGrid.SelectedItem as Supplier;

            if (selectedSupplier == null)
            {
                MessageBox.Show("Please select a supplier to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Open the Edit Supplier window, passing in the selected supplier
            SupplierEditWindow editSupplierWindow = new SupplierEditWindow(selectedSupplier);
            bool? result = editSupplierWindow.ShowDialog();

            if (result == true) // If the user clicked "Save" in the Edit window
            {
                // Refresh the supplier list
                LoadSuppliers();
            }
        }

        private void RemoveSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected supplier from the DataGrid
            Supplier selectedSupplier = SupplierDataGrid.SelectedItem as Supplier;

            if (selectedSupplier == null)
            {
                MessageBox.Show("Please select a supplier to remove.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Confirm deletion with the user
            MessageBoxResult confirmResult = MessageBox.Show(
                $"Are you sure you want to remove {selectedSupplier.SupplierName}?",
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
                // Remove the supplier from the database
                _context.Suppliers.Remove(selectedSupplier);
                _context.SaveChanges();

                // Remove the supplier from the ObservableCollection
                Suppliers.Remove(selectedSupplier);

                MessageBox.Show("Supplier removed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing supplier: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                // If the search box is empty, reload all suppliers
                LoadSuppliers();
                return;
            }

            try
            {
                // Clear the existing collection
                Suppliers.Clear();

                // Perform the search in the database
                var searchResults = _context.Suppliers
                    .Where(s => s.SupplierName.ToLower().Contains(searchText) || s.SupplierID.ToString() == searchText)
                    .ToList();

                // Add the search results to the ObservableCollection
                foreach (var supplier in searchResults)
                {
                    Suppliers.Add(supplier);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching suppliers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}