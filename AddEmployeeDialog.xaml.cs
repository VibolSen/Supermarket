using System;
using System.Windows;
using Supermarket.Models;
using BCrypt.Net; // Add this using statement

namespace Supermarket
{
    public partial class AddEmployeeDialog : Window
    {
        public Employee NewEmployee { get; set; }

        public AddEmployeeDialog()
        {
            InitializeComponent();  // ADD THIS LINE (Very Important!)
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            NewEmployee = new Employee
            {
                Name = NameTextBox.Text,
                Role = RoleTextBox.Text,
                HireDate = HireDatePicker.SelectedDate ?? DateTime.Now,
                Salary = decimal.TryParse(SalaryTextBox.Text, out decimal salary) ? salary : 0,
                Username = UsernameTextBox.Text,
                Attendance = string.Empty // Set a default value for Attendance
            };

            // Hash the password
            string password = PasswordBox.Password;
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            NewEmployee.PasswordHash = hashedPassword;

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}