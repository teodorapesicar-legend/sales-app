using SalesDesktopApp.Data;
using SalesDesktopApp.Models;
using System.Linq;
using System;

namespace SalesDesktopApp.Services
{
    // Handles user authentication and authorization
    public class AuthService
    {
        // Currently logged-in user (null if not authenticated)
        public static User? CurrentUser { get; private set; }

        // Authenticates user with username and password.
        // Uses BCrypt to verify password hash stored in the database.
        // Returns true if authentication is successful, false otherwise.
        public bool Login(string username, string password)
        {
            using var db = new AppDbContext();
            var user = db.User.FirstOrDefault(u => u.Username == username);

            // Verify the password using BCrypt (constant time comparison to prevent timing attacks)
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                CurrentUser = user;
                return true;
            }
            return false;
        }

        // Clears current user session, effectively logging out the user.
        public void Logout()
        {
            CurrentUser = null;
        }

        //Check if current user can edit data (Owner or Manager)
        public static bool CanEdit()
        {
            return CurrentUser?.Role == UserRole.Owner ||
                   CurrentUser?.Role == UserRole.Manager;
        }

        // Check if current user can delete data (Owner or Manager)
        public static bool CanDelete()
        {
            return CurrentUser?.Role == UserRole.Owner ||
                   CurrentUser?.Role == UserRole.Manager;
        }

        // Check if User is Logged In
        public static bool IsLoggedIn()
        {
            return CurrentUser != null;
        }

    }
}
