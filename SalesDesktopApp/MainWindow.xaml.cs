using System;
using System.Windows;
using System.Windows.Controls;
using SalesDesktopApp.Models;
using SalesDesktopApp.Services;

namespace SalesDesktopApp
{
    public partial class MainWindow : Window
    {
        private SalesService _salesService = new SalesService();
        private CustomerService _customerService = new CustomerService();
        private AuthService _authService = new AuthService();

        // Track selected sale and customer IDs for edit/delete operations
        private int _selectedSaleId = 0;
        private int _selectedCustomerId = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Display current user info in header
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (AuthService.CurrentUser != null)
            {
                UserInfoTextBlock.Text = $"User: {AuthService.CurrentUser.Username} ({AuthService.CurrentUser.Role})";
            }

            LoadSales();
            LoadCustomers();

            // Hide "Add User" button for non-managers
            AddUserButton.Visibility = AuthService.CurrentUser?.Role == UserRole.Manager
                ? Visibility.Visible
                : Visibility.Collapsed;

        }
        // Load sales based on user role:
        // - Employee: only their own sales
        // - Owner/Manager: all sales
        private void LoadSales()
        {
            if (AuthService.CurrentUser?.Role == UserRole.Employee)
            {
                SalesDataGrid.ItemsSource = _salesService.GetMySales();
            }
            else
            {
                SalesDataGrid.ItemsSource = _salesService.GetAllSales();
            }
        }

        private void SetupButtonsByRole()
        {
            // Everyone can Add sales
            AddButton.IsEnabled = true;

            // Only Owner and Manager can Edit
            UpdateButton.IsEnabled = AuthService.CanEdit();

            // Only Owner and Manager can Delete
            DeleteButton.IsEnabled = AuthService.CanDelete();
        }
        
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ArticleNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text))
            {
                MessageBox.Show("Enter article name and price!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price))
            {
                MessageBox.Show("Price needs to be a number!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _salesService.AddSale(ArticleNameTextBox.Text, price);

            StatusTextBlock.Text = "Sale added successsfully!";
            ClearInputFields();
            LoadSales();
        }

        // Edit button click handler - only for Owner and Manager
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedSaleId == 0)
            {
                MessageBox.Show("Choose a sale to change!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(ArticleNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text))
            {
                MessageBox.Show("Enter article name and price!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price))
            {
                MessageBox.Show("Price needs to be a number!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_salesService.UpdateSale(_selectedSaleId, ArticleNameTextBox.Text, price))
            {
                StatusTextBlock.Text = "Sale edited successfully!";
                ClearInputFields();
                LoadSales();
            }
            else
            {
                MessageBox.Show("You don't have the permission to edit!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Delete button click handler - only for Owner and Manager
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedSaleId == 0)
            {
                MessageBox.Show("Choose a sale to delete!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this sale?",
                "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (_salesService.DeleteSale(_selectedSaleId))
                {
                    StatusTextBlock.Text = "Sale deleted successfully!";
                    ClearInputFields();
                    LoadSales();
                }
                else
                {
                    MessageBox.Show("You don't have the permission to delete!", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SalesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SalesDataGrid.SelectedItem is Sale selectedSale)
            {
                _selectedSaleId = selectedSale.Id;
                ArticleNameTextBox.Text = selectedSale.ArticleName;
                PriceTextBox.Text = selectedSale.Price.ToString();

                bool canEdit = AuthService.CanEdit();
                bool canDelete = AuthService.CanDelete();

                UpdateButton.IsEnabled = canEdit;
                DeleteButton.IsEnabled = canDelete;
            }
        }

        private void ClearSalesFeilds()
        {
            ArticleNameTextBox.Text = "";
            PriceTextBox.Text = "";
            _selectedSaleId = 0;
            SalesDataGrid.SelectedItem = null;
        }

        // Load customers based on user role:
        // - Employee: only customers they created
        // - Owner/Manager: all customers
        private void LoadCustomers()
        {
            if (AuthService.CurrentUser?.Role == UserRole.Employee)
            {
                CustomersDataGrid.ItemsSource = _customerService.GetMyCustomers();
            }
            else
            {
                CustomersDataGrid.ItemsSource = _customerService.GetAllCustomers();
            }
        }

        // Add customer button click handler - everyone can add customers
        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CustomerFirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(CustomerLastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(ServiceTypeTextBox.Text))
            {
                MessageBox.Show("Enter all customer details!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _customerService.AddCustomer(
                CustomerFirstNameTextBox.Text, 
                CustomerLastNameTextBox.Text, 
                ServiceTypeTextBox.Text
                );

            StatusTextBlock.Text = "Customer added successsfully!";
            ClearCustomerFields();
            LoadCustomers();
        }

        // Edit customer button click handler - only for Owner and Manager
        private void UpdateCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCustomerId == 0)
            {
                MessageBox.Show("Choose a customer to update!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CustomerFirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(CustomerLastNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(ServiceTypeTextBox.Text))
            {
                MessageBox.Show("Enter all customer details!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_customerService.UpdateCustomer(
                _selectedCustomerId,
                CustomerFirstNameTextBox.Text,
                CustomerLastNameTextBox.Text,
                ServiceTypeTextBox.Text))
            {
                StatusTextBlock.Text = "Customer updated successfully!";
                ClearCustomerFields();
                LoadCustomers();
            }
            else
            {
                MessageBox.Show("You don't have the permission to edit customers!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Delete customer button click handler - only for Owner and Manager
        private void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCustomerId == 0)
            {
                MessageBox.Show("Choose a customer to delete!", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this customer?",
                "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (_customerService.DeleteCustomer(_selectedCustomerId))
                {
                    StatusTextBlock.Text = "Customer deleted successfully!";
                    ClearCustomerFields();
                    LoadCustomers();
                }
                else
                {
                    MessageBox.Show("You don't have the permission to delete customers!", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CustomersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustomersDataGrid.SelectedItem is Customer selectedCustomer)
            {
                _selectedCustomerId = selectedCustomer.Id;
                CustomerFirstNameTextBox.Text = selectedCustomer.FirstName;
                CustomerLastNameTextBox.Text = selectedCustomer.LastName;
                ServiceTypeTextBox.Text = selectedCustomer.ServiceType;

                bool canEdit = AuthService.CanEdit();
                bool canDelete = AuthService.CanDelete();
                UpdateCustomerButton.IsEnabled = canEdit;
                DeleteCustomerButton.IsEnabled = canDelete;
            }
            else
            {
                UpdateCustomerButton.IsEnabled = false;
                DeleteCustomerButton.IsEnabled = false;
            }
        }

        private void ClearCustomerFields()
        {
            CustomerFirstNameTextBox.Text = "";
            CustomerLastNameTextBox.Text = "";
            ServiceTypeTextBox.Text = "";
            _selectedCustomerId = 0;
            CustomersDataGrid.SelectedItem = null;
        }
     
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _authService.Logout();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void ClearInputFields()
        {
            ArticleNameTextBox.Text = "";
            PriceTextBox.Text = "";
            _selectedSaleId = 0;
            SalesDataGrid.SelectedItem = null;
        }

        // Add User button click handler - only for Manager
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthService.CurrentUser?.Role != UserRole.Manager)
            {
                MessageBox.Show("Only Manager can add new users!",
                    "You don't have the permission", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }

        // Event handler to capitalize the first letter of the article name as the user types
        private void ArticleNameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ArticleNameTextBox.Text))
            {
                // Get current cursor position before changing text
                int selectionStart = ArticleNameTextBox.SelectionStart;

                // Capitalizing the first letter of the article name
                string text = ArticleNameTextBox.Text;
                string capitalized = char.ToUpper(text[0]) + text.Substring(1);

                // Updating if the text has changed after capitalization to prevent infinite loop
                if (ArticleNameTextBox.Text != capitalized)
                {
                    ArticleNameTextBox.Text = capitalized;
                    // Restoring the cursor position after text change
                    ArticleNameTextBox.SelectionStart = selectionStart;
                }
            }
        }
    }
}