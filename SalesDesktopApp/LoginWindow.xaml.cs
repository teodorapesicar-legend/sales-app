using SalesDesktopApp.Services;
using System.Windows;
using System.Windows.Documents;

namespace SalesDesktopApp
{
    public partial class LoginWindow : Window
    {
        private AuthService _authService = new AuthService();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ErrorTextBlock.Text = "Enter username and password!";
                return;
            }

            if (_authService.Login(username, password))
            {
                // Login successful - open main window
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ErrorTextBlock.Text = "Invalid username or password!";
            }
        }

        // Opens registration window for new user sign-up
        private void RegisterLink_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.ShowDialog(); // Open as modal dialog
        }
    }
}