using SalesDesktopApp.Data;
using SalesDesktopApp.Models;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Linq.Expressions;

namespace SalesDesktopApp
{
    // User registration window.
    // Used for:
    // 1. Initial user sign-up (accessed from LoginWindow)
    // 2. Adding new users by Manager (accessed from MainWindow "Add User" button)
    // Note: Permission check for manager-only access is enforced in MainWindow.AddUserButton_Click
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            RoleComboBox.SelectedIndex = 0; // Default to Employee role
        }

        // Handles user registration with validation and BCrypt password hashing.
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate all required fields are filled
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                ErrorTextBlock.Text = "All fields are required!";
                return; 
            }

            // Check if username already exists
            using (var db = new AppDbContext())
            {
                if (db.User.Any(u => u.Username == UsernameTextBox.Text))
                {
                    ErrorTextBlock.Text = "Username already exists!";
                    return;
                }

                // Create new user with hashed password
                var newUser = new User
                {
                    Username = UsernameTextBox.Text,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(PasswordBox.Password),
                    FirstName = FirstNameTextBox.Text,
                    LastName = LastNameTextBox.Text,
                    Role = (UserRole)int.Parse(((ComboBoxItem)RoleComboBox.SelectedItem).Tag!.ToString()!)
                };

                try
                {

                    db.User.Add(newUser);
                    db.SaveChanges();

                    MessageBox.Show("User added successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding user: " + ex.Message, "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}