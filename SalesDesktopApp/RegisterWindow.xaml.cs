using SalesDesktopApp.Data;
using SalesDesktopApp.Models;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Linq.Expressions;

namespace SalesDesktopApp
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            RoleComboBox.SelectedIndex = 0; // Default Employee
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTextBox.Text))
                return; // All fields are required

            // Check if username already exists
            using (var db = new AppDbContext())
            {
                if (db.User.Any(u => u.Username == UsernameTextBox.Text))
                {
                    ErrorTextBlock.Text = "Username already exists!";
                    return;
                }

                // Create new user
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