using SalesDesktopApp.Data;
using SalesDesktopApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SalesDesktopApp.Services
{
    // Manages customer operations with role-based data access
    // Employees see only their own customers, Managers and Owners see all customers
    public class CustomerService
    {
        // Adds a new customer created by the current user - everyone can add
        public void AddCustomer(string firstName, string lastName, string serviceType)
        {
            if (AuthService.CurrentUser == null) return;

            using var db = new AppDbContext();
            var customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                ServiceType = serviceType,
                DateCreated = DateTime.Now,
                CreatedByUserId = AuthService.CurrentUser.Id
            };

            db.Customers.Add(customer);
            db.SaveChanges();
        }

        // Retrieves all customers from database
        // Used for Owner and Manager roles
        // Returns list of all customers with creator information
        public List<Customer> GetAllCustomers()
        {
            // Only Owner and Manager can see all customers
            if (!AuthService.CanEdit()) return new List<Customer>();

            using var db = new AppDbContext();
            return db.Customers
                .Include(c => c.CreatedByUser)
                .ToList();
        }

        // Retrieves only customers created by the current user
        // Used for Employee role
        // Returns list of current user's customers
        public List<Customer> GetMyCustomers()
        {
            if (AuthService.CurrentUser == null) return new List<Customer>();

            using var db = new AppDbContext();
            return db.Customers
                .Include(c => c.CreatedByUser)
                .Where(c => c.CreatedByUserId == AuthService.CurrentUser.Id)
                .ToList();
        }

        // Updates an existing customer
        // Requires Owner or Manager permissions
        // Returns true if update successful, false if no permission or customer not found
        public bool UpdateCustomer(int customerId, string firstName, string lastName, string serviceType)
        {
            // Check permissions (Owner or Manager can edit)
            if (!AuthService.CanEdit()) return false;

            using var db = new AppDbContext();
            var customer = db.Customers.Find(customerId);
            if (customer == null) return false;

            customer.FirstName = firstName;
            customer.LastName = lastName;
            customer.ServiceType = serviceType;
            db.SaveChanges();
            return true;
        }

        // Deletes a customer
        // Requires Owner or Manager permissions
        // Returns true if deletion successful, false if no permission or customer not found
        public bool DeleteCustomer(int customerId)
        {
            // Check permissions (Owner or Manager can delete customers)
            if (!AuthService.CanDelete()) return false;

            using var db = new AppDbContext();
            var customer = db.Customers.Find(customerId);
            if (customer == null) return false;

            db.Customers.Remove(customer);
            db.SaveChanges();
            return true;
        }
    }
}
