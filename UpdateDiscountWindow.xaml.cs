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
    public partial class UpdateDiscountWindow : Window
    {
        private Discount _discount;

        public UpdateDiscountWindow(Discount discount)
        {
            InitializeComponent();
            _discount = discount;
            DataContext = _discount; // Set the DataContext to the discount object

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

                    // Update the discount properties
                    _discount.DiscountName = DiscountNameTextBox.Text;
                    _discount.DiscountType = (DiscountTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                    _discount.DiscountValue = discountValue;
                    _discount.StartDate = StartDatePicker.SelectedDate;
                    _discount.EndDate = EndDatePicker.SelectedDate;
                    _discount.ProductID = (int?)ProductComboBox.SelectedValue;
                    _discount.CategoryID = (int?)CategoryComboBox.SelectedValue;

                    //Attach discount to context if not tracked already
                    if (!context.Discounts.Local.Any(d => d.DiscountID == _discount.DiscountID))
                    {
                        context.Discounts.Attach(_discount);
                    }

                    context.Entry(_discount).State = EntityState.Modified;
                    context.SaveChanges();

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