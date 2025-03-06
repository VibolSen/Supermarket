using System;
using System.Linq;
using System.Windows;
using Supermarket.Models;
using Supermarket.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Supermarket
{
    public partial class ProductEntryDialog : Window
    {
        public Product Product { get; set; }

        private readonly SupermarketContext _context;
        private bool _isUpdating = false;

        public ProductEntryDialog()
        {
            InitializeComponent();
            _context = App.Current.ServiceProvider.GetRequiredService<SupermarketContext>();
            Product = new Product();
            DataContext = this;
            LoadCategoriesAndSuppliers();
        }

        public ProductEntryDialog(Product product)
        {
            InitializeComponent();
            _context = App.Current.ServiceProvider.GetRequiredService<SupermarketContext>();
            Product = product;
            DataContext = this;
            LoadCategoriesAndSuppliers();

            ProductNameTextBox.Text = product.ProductName;
            QuantityTextBox.Text = product.StockQuantity.ToString();
            UnitPriceTextBox.Text = product.UnitPrice.ToString();

            if (product.Category != null)
            {
                CategoryComboBox.SelectedItem = product.Category;
            }

            if (product.Supplier != null)
            {
                SupplierComboBox.SelectedItem = product.Supplier;
            }

            _isUpdating = true;
        }

        private void LoadCategoriesAndSuppliers()
        {
            CategoryComboBox.ItemsSource = _context.Categories.ToList();
            SupplierComboBox.ItemsSource = _context.Suppliers.ToList();
        }

        // In ProductEntryDialog.cs, within the SaveButton_Click method:

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validation
            if (string.IsNullOrEmpty(ProductNameTextBox.Text))
            {
                MessageBox.Show("Product name is required.");
                return;
            }

            if (!decimal.TryParse(UnitPriceTextBox.Text, out decimal unitPrice))
            {
                MessageBox.Show("Invalid Unit Price. Please enter a number.");
                return;
            }

            if (!int.TryParse(QuantityTextBox.Text, out int quantity))
            {
                MessageBox.Show("Invalid quantity. Please enter a number.");
                return;
            }

            var selectedCategory = CategoryComboBox.SelectedItem as Category;
            var selectedSupplier = SupplierComboBox.SelectedItem as Supplier;

            if (selectedCategory == null || selectedSupplier == null)
            {
                MessageBox.Show("Please select a valid category and supplier.");
                return;
            }

            // Assign values to the Product
            Product.ProductName = ProductNameTextBox.Text;
            Product.Category = selectedCategory;
            Product.Supplier = selectedSupplier;
            Product.StockQuantity = quantity;
            Product.UnitPrice = unitPrice;

            // Assign CategoryID and SupplierID
            Product.CategoryID = selectedCategory.CategoryID;
            Product.SupplierID = selectedSupplier.SupplierID;

            try
            {
                if (!_isUpdating)
                {
                    _context.Products.Add(Product);
                }

                _context.SaveChanges(); // Save changes to the database

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                // Log or display the inner exception for debugging
                string errorMessage = $"Error saving product: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nInner Exception: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage);

                // Additional debugging: Log the Product object details
                MessageBox.Show($"Product Details:\nName: {Product.ProductName}\nCategory: {Product.Category?.CategoryName}\nSupplier: {Product.Supplier?.SupplierName}\nQuantity: {Product.StockQuantity}\nPrice: {Product.UnitPrice}");
            }
        }
    }
}