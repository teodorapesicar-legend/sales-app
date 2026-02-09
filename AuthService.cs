using SalesDesktopApp.Data;
using SalesDesktopApp.Models;
using System.Linq;
using System;

namespace SalesDesktopApp.Services
{
    public class AuthService
	{
        // Current logged-in user
        public static User? CurrentUser { get; private set; }

        // Login method
        public bool Login(string username, string password)
        {
            using var db = new AppDbContext();
            var user = db.Users.FirstOrDefault(u => u.Username == username);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                CurrentUser = user;
                return true;
            }
            return false;
        }

        // Logout method
        public void Logout()
        {
            CurrentUser = null;
        }

        //Check if User can edit (Owner or Manager)
        public static bool CanEdit()
        {
            return CurentUser?.Role == "Owner" ||
                   CurrentUser?.Role == "Manager";
        }

        // Check if User can delete (Owner or Manager)
        public static bool CanDelete()
        {
            return CurrentUser?.Role == "Owner" ||
                   CurrentUser?.Role == "Manager";
        }

        // Check if User is Logged In
        public static bool IsLoggedIn()
        {
            return CurrentUser != null;
        }

    }
