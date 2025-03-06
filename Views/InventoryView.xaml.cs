using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Supermarket.Models;
using Supermarket.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Supermarket
{
    public partial class InventoryView : UserControl
    {
        private readonly InventoryService _inventoryService;
        private List<Product> _allProducts; // Store all products for searching

        public InventoryView()
        {
            InitializeComponent();
            _inventoryService = App.Current.ServiceProvider.GetRequiredService<InventoryService>();
            LoadInventoryData();
        }

        private void LoadInventoryData()
        {
            try
            {
                _allProducts = _inventoryService.GetProducts();
                InventoryDataGrid.ItemsSource = _allProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading inventory data: {ex.Message}");
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadInventoryData();
            SearchTextBox.Text = ""; // Clear search field on refresh
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            ProductEntryDialog dialog = new ProductEntryDialog();
            if (dialog.ShowDialog() == true)
            {
                Product newProduct = dialog.Product;
                bool success = _inventoryService.AddProduct(newProduct);

                if (success)
                {
                    MessageBox.Show("Product added successfully!");
                    LoadInventoryData();
                }
                else
                {
                    MessageBox.Show("Failed to add product.");
                }
            }
        }

        private void UpdateProductButton_Click(object sender, RoutedEventArgs e)
        {
            Product selectedProduct = (Product)InventoryDataGrid.SelectedItem;
            if (selectedProduct == null)
            {
                MessageBox.Show("Please select a product to update.");
                return;
            }

            ProductEntryDialog dialog = new ProductEntryDialog(selectedProduct);
            if (dialog.ShowDialog() == true)
            {
                Product updatedProduct = dialog.Product;
                bool success = _inventoryService.UpdateProduct(updatedProduct);

                if (success)
                {
                    MessageBox.Show("Product updated successfully!");
                    LoadInventoryData();
                }
                else
                {
                    MessageBox.Show("Failed to update product.");
                }
            }
        }

        private void RemoveProductButton_Click(object sender, RoutedEventArgs e)
        {
            Product selectedProduct = (Product)InventoryDataGrid.SelectedItem;
            if (selectedProduct == null)
            {
                MessageBox.Show("Please select a product to remove.");
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {selectedProduct.ProductName}?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                bool success = _inventoryService.DeleteProduct(selectedProduct.ProductID);
                if (success)
                {
                    MessageBox.Show("Product removed successfully.");
                    LoadInventoryData();
                }
                else
                {
                    MessageBox.Show("Failed to remove product.");
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                // If search text is empty, reload all products
                InventoryDataGrid.ItemsSource = _allProducts;
                return;
            }

            try
            {
                // Filter products by name or ID
                var searchResults = _allProducts.Where(p =>
                    p.ProductName.ToLower().Contains(searchText) ||
                    p.ProductID.ToString().Contains(searchText)).ToList();

                // Set the filtered results as the data source for the DataGrid
                InventoryDataGrid.ItemsSource = searchResults;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
