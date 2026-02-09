using SalesDesktopApp.Data;
using SalesDesktopApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SalesDesktopApp.Services
{
    public class CustomerService
    {
        // Add new Customer - everyone can add
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

        // Get all Customers with User info - only Manager and Owner can see all
        public List<Customer> GetAllCustomers()
        {
            if (!AuthService.CanEdit()) return new List<Customer>();

            using var db = new AppDbContext();
            return db.Customers
                .Include(c => c.CreatedByUser)
                .ToList();
        }

        // Get Customers created by current user - everyone can see their own
        public List<Customer> GetMyCustomers()
        {
            if (AuthService.CurrentUser == null) return new List<Customer>();

            using var db = new AppDbContext();
            return db.Customers
                .Include(c => c.CreatedByUser)
                .Where(c => c.CreatedByUserId == AuthService.CurrentUser.Id)
                .ToList();
        }

        // Edit a Customer - only Manager and Owner can edit
        public bool UpdateCustomer(int customerId, string firstName, string lastName, string serviceType)
        {
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

        // Delete a Customer - only Manager and Owner can delete
        public bool DeleteCustomer(int customerId)
        {
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
