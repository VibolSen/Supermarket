using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Supermarket.Data;
using Supermarket.Models;

namespace Supermarket
{
    public partial class AddDiscountWindow : Window
    {
        public AddDiscountWindow()
        {
            InitializeComponent();
            LoadProductsAsync();
            LoadCategoriesAsync();
        }

        private async void LoadProductsAsync()
        {
            using (var scope = App.Current.ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SupermarketContext>();
                try
                {
                    var products = await context.Products.ToListAsync();
                    ProductComboBox.ItemsSource = products;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void LoadCategoriesAsync()
        {
            using (var scope = App.Current.ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SupermarketContext>();
                try
                {
                    var categories = await context.Categories.ToListAsync();
                    CategoryComboBox.ItemsSource = categories;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading categories: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var scope = App.Current.ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SupermarketContext>();
                try
                {
                    // Validate inputs
                    if (string.IsNullOrEmpty(DiscountNameTextBox.Text))
                    {
                        MessageBox.Show("Discount Name is required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (DiscountTypeComboBox.SelectedItem == null)
                    {
                        MessageBox.Show("Discount Type is required.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (!decimal.TryParse(DiscountValueTextBox.Text, out decimal discountValue))
                    {
                        MessageBox.Show("Invalid Discount Value.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Create the discount
                    var discount = new Discount
                    {
                        DiscountName = DiscountNameTextBox.Text,
                        DiscountType = (DiscountTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                        DiscountValue = discountValue,
                        StartDate = StartDatePicker.SelectedDate,
                        EndDate = EndDatePicker.SelectedDate,
                        ProductID = (int?)ProductComboBox.SelectedValue,
                        CategoryID = (int?)CategoryComboBox.SelectedValue
                    };

                    // Save the discount
                    context.Discounts.Add(discount);
                    context.SaveChanges();

                    // Close the window
                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving discount: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}