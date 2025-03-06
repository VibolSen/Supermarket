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

namespace Supermarket
{
    public partial class EmployeeManagementView : UserControl
    {
        private SupermarketContext _context;
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

        public EmployeeManagementView()
        {
            InitializeComponent();

            // Get the DbContext from DI
            _context = App.Current.ServiceProvider.GetRequiredService<SupermarketContext>();

            // Load Employees
            LoadEmployeesAsync();

            // Set the DataContext.
            DataContext = this;
        }

        private async Task LoadEmployeesAsync()
        {
            try
            {
                Employees.Clear();

                var employeeList = await _context.Employees.ToListAsync();

                foreach (var employee in employeeList)
                {
                    Employees.Add(employee);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Reload the employees from the database
                await LoadEmployeesAsync();

                // Optional: You can reset the search text if you want
                SearchTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing employees: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            var addEmployeeDialog = new AddEmployeeDialog();
            if (addEmployeeDialog.ShowDialog() == true)
            {
                // Add the new employee to the database
                var newEmployee = addEmployeeDialog.NewEmployee;
                _context.Employees.Add(newEmployee);
                await _context.SaveChangesAsync();

                // Refresh the DataGrid
                await LoadEmployeesAsync();
            }
        }

        private async void UpdateEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected employee from the DataGrid
            Employee selectedEmployee = EmployeeDataGrid.SelectedItem as Employee;

            if (selectedEmployee == null)
            {
                MessageBox.Show("Please select an employee to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var updateEmployeeDialog = new UpdateEmployeeDialog(selectedEmployee);
            bool? result = updateEmployeeDialog.ShowDialog(); // Fix: Use ShowDialog() method from Window class
            if (result == true)
            {
                // Update the employee in the database
                await _context.SaveChangesAsync();  // EF Core tracks changes; SaveChangesAsync() persists them

                // Refresh the DataGrid
                await LoadEmployeesAsync();
            }
        }
        private async void RemoveEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected employee from the DataGrid
            Employee selectedEmployee = EmployeeDataGrid.SelectedItem as Employee;

            if (selectedEmployee == null)
            {
                MessageBox.Show("Please select an employee to remove.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Are you sure you want to remove employee: {selectedEmployee.Name}?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Remove the employee from the database
                    _context.Employees.Remove(selectedEmployee);
                    await _context.SaveChangesAsync();

                    // Refresh the DataGrid
                    await LoadEmployeesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error removing employee: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                // If search text is empty, reload all employees
                EmployeeDataGrid.ItemsSource = null; // Clear existing ItemsSource
                EmployeeDataGrid.ItemsSource = Employees; // Re-assign
                return;
            }

            try
            {
                // Filter employees by name or ID
                var searchResults = Employees.Where(e =>
                    e.Name.ToLower().Contains(searchText) ||
                    e.EmployeeID.ToString().Contains(searchText)).ToList();

                // Set the filtered results as the data source for the DataGrid
                EmployeeDataGrid.ItemsSource = searchResults;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching employees: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}