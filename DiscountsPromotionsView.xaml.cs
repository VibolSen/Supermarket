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
using System.Collections.Generic;

namespace Supermarket
{
    /// <summary>
    /// Interaction logic for DiscountsPromotionsView.xaml
    /// </summary>
    public partial class DiscountsPromotionsView : UserControl
    {
        public ObservableCollection<Discount> Discounts { get; set; } = new ObservableCollection<Discount>();

        // Declare _allDiscounts as a FIELD of the class
        private List<Discount> _allDiscounts = new List<Discount>();

        public DiscountsPromotionsView()
        {
            InitializeComponent();

            // Load Discounts
            LoadDiscountsAsync();

            // Set the DataContext.
            DataContext = this;
        }

        private async Task LoadDiscountsAsync()
        {
            using (var scope = App.Current.ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SupermarketContext>();
                try
                {
                    Discounts.Clear(); // Clear existing data before loading

                    var discountList = await context.Discounts.ToListAsync();
                    _allDiscounts = discountList.ToList(); // Store all discounts

                    foreach (var discount in discountList)
                    {
                        Discounts.Add(discount);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading discounts: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Log the full exception details to the Output window for debugging
                    System.Diagnostics.Debug.WriteLine(ex);
                    if (ex.InnerException != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    }
                }
            }
        }

        private void AddDiscountButton_Click(object sender, RoutedEventArgs e)
        {
            var addDiscountWindow = new AddDiscountWindow();
            if (addDiscountWindow.ShowDialog() == true)
            {
                // Refresh the discounts list ONLY if DialogResult is true (Save button was clicked)
                LoadDiscountsAsync();
            }
        }

        private void UpdateDiscountButton_Click(object sender, RoutedEventArgs e)
        {
            if (DiscountDataGrid.SelectedItem is Discount selectedDiscount)
            {
                var updateDiscountWindow = new UpdateDiscountWindow(selectedDiscount); // Pass the selected discount
                if (updateDiscountWindow.ShowDialog() == true)
                {
                    // Refresh the discounts list after a successful update
                    LoadDiscountsAsync();
                }
            }
            else
            {
                MessageBox.Show("Please select a discount to update.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void RemoveDiscountButton_Click(object sender, RoutedEventArgs e)
        {
            if (DiscountDataGrid.SelectedItem is Discount selectedDiscount)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to remove this discount?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (var scope = App.Current.ServiceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<SupermarketContext>();
                        try
                        {
                            context.Discounts.Remove(selectedDiscount);
                            await context.SaveChangesAsync();
                            LoadDiscountsAsync(); // Refresh the DataGrid
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error removing discount: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a discount to remove.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadDiscountsAsync(); // Reload the discounts from the database
            SearchTextBox.Text = ""; // Clear search box
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                // If search text is empty, show all discounts
                Discounts.Clear();
                foreach (var discount in _allDiscounts)
                {
                    Discounts.Add(discount);
                }
            }
            else
            {
                // Filter discounts based on search text
                var filteredDiscounts = _allDiscounts.Where(d =>
                    d.DiscountName.ToLower().Contains(searchText) ||
                    d.DiscountType.ToLower().Contains(searchText) ||
                    d.DiscountValue.ToString().Contains(searchText)
                ).ToList();

                Discounts.Clear();
                foreach (var discount in filteredDiscounts)
                {
                    Discounts.Add(discount);
                }
            }
        }
    }
}