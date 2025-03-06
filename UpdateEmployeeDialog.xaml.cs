// UpdateEmployeeDialog.xaml.cs
using System;
using System.Windows;
using Supermarket.Models;

namespace Supermarket
{
    public partial class UpdateEmployeeDialog : Window
    {
        public Employee UpdatedEmployee { get; set; } // Property to hold the updated employee data

        public UpdateEmployeeDialog(Employee employee)
        {
            InitializeComponent();
            UpdatedEmployee = employee;

            // Pre-populate the input fields with the employee's data
            NameTextBox.Text = employee.Name;
            RoleTextBox.Text = employee.Role;
            HireDatePicker.SelectedDate = employee.HireDate;
            SalaryTextBox.Text = employee.Salary.ToString();
            UsernameTextBox.Text = employee.Username;
            // Password should NOT be pre-populated for security reasons
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Update the employee's properties with the data from the text boxes
            UpdatedEmployee.Name = NameTextBox.Text;
            UpdatedEmployee.Role = RoleTextBox.Text;
            UpdatedEmployee.HireDate = HireDatePicker.SelectedDate ?? DateTime.Now;
            UpdatedEmployee.Salary = decimal.TryParse(SalaryTextBox.Text, out decimal salary) ? salary : 0;
            UpdatedEmployee.Username = UsernameTextBox.Text;

            DialogResult = true; // Signal that the dialog was accepted
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Signal that the dialog was canceled
            Close();
        }
    }
}