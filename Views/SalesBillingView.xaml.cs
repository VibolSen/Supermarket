using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Supermarket.Data;
using Supermarket.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Collections.Generic;

namespace Supermarket
{
    public partial class SalesBillingView : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>();
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        public ObservableCollection<CartItem> CartItems { get; set; } = new ObservableCollection<CartItem>();

        public SalesBillingView()
        {
            InitializeComponent();

            // Load Data
            LoadCustomersAsync();
            LoadProductsAsync();

            // Set DataContext
            DataContext = this;

            CartItems.CollectionChanged += CartItems_CollectionChanged;
        }

        private void CartItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateTotal();
        }

        private async System.Threading.Tasks.Task LoadCustomersAsync()
        {
            using (var scope = App.Current.ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SupermarketContext>();
                try
                {
                    Customers.Clear();
                    var customerList = await context.Customers.ToListAsync();
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
        }

        private async System.Threading.Tasks.Task LoadProductsAsync()
        {
            using (var scope = App.Current.ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SupermarketContext>();
                try
                {
                    Products.Clear();
                    var productList = await context.Products.ToListAsync();
                    foreach (var product in productList)
                    {
                        Products.Add(product);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            if (FindName("ProductComboBox") is ComboBox productComboBox &&
                FindName("QuantityTextBox") is TextBox quantityTextBox)
            {
                if (productComboBox.SelectedItem is Product selectedProduct &&
                    int.TryParse(quantityTextBox.Text, out int quantity) &&
                    quantity > 0)
                {
                    if (selectedProduct.StockQuantity < quantity)
                    {
                        MessageBox.Show($"Not enough stock for {selectedProduct.ProductName}.  Only {selectedProduct.StockQuantity} available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Check if the product is already in the cart
                    var existingCartItem = CartItems.FirstOrDefault(item => item.ProductID == selectedProduct.ProductID);

                    if (existingCartItem != null)
                    {
                        // If the product is already in the cart, update the quantity
                        existingCartItem.Quantity += quantity;
                    }
                    else
                    {
                        // If the product is not in the cart, add a new cart item
                        CartItems.Add(new CartItem
                        {
                            ProductID = selectedProduct.ProductID,
                            ProductName = selectedProduct.ProductName,
                            Quantity = quantity,
                            Price = selectedProduct.UnitPrice
                        });
                    }

                    // Clear the quantity textbox
                    quantityTextBox.Text = "";
                    UpdateTotal();
                }
                else
                {
                    MessageBox.Show("Please select a product and enter a valid quantity.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (FindName("CustomerComboBox") is ComboBox customerComboBox)
            {
                if (customerComboBox.SelectedItem is Customer selectedCustomer)
                {
                    if (CartItems.Count == 0)
                    {
                        MessageBox.Show("The cart is empty. Please add items to the cart before checking out.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    using (var scope = App.Current.ServiceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<SupermarketContext>();
                        using (var transaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                // Get the employee.
                                // Important: Replace this with your ACTUAL way of getting the EmployeeID
                                int employeeID = 1; // Hardcoded

                                // *** FETCH THE EMPLOYEE NAME FROM THE DATABASE (or wherever it's stored)! ***
                                // Example (assuming you have an Employees table and can get the name by ID):
                                var employee = await context.Employees.FindAsync(employeeID);
                                string employeeName = (employee != null) ? employee.Name : "Unknown";

                                // Create the Sale record
                                var sale = new Sale
                                {
                                    CustomerID = selectedCustomer.CustomerID,
                                    SaleDate = DateTime.Now,
                                    TotalAmount = Total,
                                    EmployeeID = employeeID  // *** SET THE EMPLOYEE ID HERE! ***
                                };

                                // Check for a valid employee ID
                                if (employeeID <= 0)
                                {
                                    MessageBox.Show("Invalid Employee ID. Cannot complete sale.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    transaction.Rollback();
                                    return;
                                }

                                context.Sales.Add(sale);
                                await context.SaveChangesAsync(); // Save to get the SaleID

                                // 2. Create SaleItem records and update stock
                                foreach (var cartItem in CartItems)
                                {
                                    var product = await context.Products.FindAsync(cartItem.ProductID);
                                    if (product == null)
                                    {
                                        throw new Exception($"Product with ID {cartItem.ProductID} not found.");
                                    }

                                    if (product.StockQuantity < cartItem.Quantity)
                                    {
                                        throw new Exception($"Not enough stock for {product.ProductName}.");
                                    }

                                    var saleItem = new SaleItem
                                    {
                                        SaleID = sale.SaleID,
                                        ProductID = cartItem.ProductID,
                                        Quantity = cartItem.Quantity,
                                        UnitPrice = cartItem.Price
                                    };

                                    context.SaleItems.Add(saleItem);

                                    product.StockQuantity -= cartItem.Quantity;
                                    context.Entry(product).State = EntityState.Modified;
                                }

                                await context.SaveChangesAsync();

                                transaction.Commit();

                                // Generate Receipt Data
                                List<Tuple<string, int, decimal>> receiptItems = GenerateReceiptData();

                                // *** PASS EMPLOYEE AND CUSTOMER NAMES TO RECEIPT WINDOW! ***
                                ShowReceiptWindow(receiptItems, employeeName, selectedCustomer.Name);

                                // Clear the cart after successful checkout
                                CartItems.Clear();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                string errorMessage = $"Error during checkout: {ex.Message}";
                                if (ex.InnerException != null)
                                {
                                    errorMessage += $"\nInner Exception: {ex.InnerException.Message}";
                                }
                                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                Console.WriteLine(ex);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a customer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ShowReceiptWindow(List<Tuple<string, int, decimal>> receiptItems, string employeeName, string customerName)
        {
            var receiptWindow = new ReceiptWindow(receiptItems, employeeName, customerName);
            receiptWindow.ShowDialog();
        }

        private List<Tuple<string, int, decimal>> GenerateReceiptData()
        {
            List<Tuple<string, int, decimal>> receiptItems = new List<Tuple<string, int, decimal>>();

            foreach (var cartItem in CartItems)
            {
                receiptItems.Add(new Tuple<string, int, decimal>(cartItem.ProductName, cartItem.Quantity, cartItem.Price));
            }

            return receiptItems;
        }

        private void ShowReceiptWindow(List<Tuple<string, int, decimal>> receiptItems)
        {
            var receiptWindow = new ReceiptWindow(receiptItems);
            receiptWindow.ShowDialog();
        }

        private void UpdateTotal()
        {
            Total = CartItems.Sum(item => item.Subtotal);
            OnPropertyChanged(nameof(Total)); // Use nameof operator
        }

        private decimal _total;

        public decimal Total
        {
            get { return _total; }
            set
            {
                if (_total != value)
                {
                    _total = value;
                    OnPropertyChanged(nameof(Total)); // Use nameof operator
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public decimal Subtotal
        {
            get { return Quantity * Price; }
        }
    }
}