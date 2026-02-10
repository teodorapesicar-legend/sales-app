using SalesDesktopApp.Data;
using SalesDesktopApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

#pragma warning disable CS8602

namespace SalesDesktopApp.Services
{
    // Manages Sales operations with role-based data access
    // Employees can add sales and view their own sales, Managers and Owners can view and manage all sales
    public class SalesService
	{
        // Add a new sale for currently logged-in user
        public void AddSale(string articleName, decimal price)
		{
			if (AuthService.CurrentUser == null) return;

			using var db = new AppDbContext();
			var sale = new Sale
			{
				ArticleName = articleName,
				Price = price,
				SaleDate = DateTime.Now,
				UserId = AuthService.CurrentUser.Id
			};

			db.Sales.Add(sale);
			db.SaveChanges();
		}

        // Retrives all sales from the database
        // Used for Owner and Manager roles
        // Returns  list of all sales with associated user information
        public List<Sale> GetAllSales()
		{
			using var db = new AppDbContext();
			return db.Sales
				.Include(s => s.User) // Load user data in same query for efficiency
                .ToList();
		}

        // Retrives sales created by the currently logged-in user
        // Used for Employee role
		// Returns list of curent user's sales
        public List<Sale> GetMySales()
		{
			using var db = new AppDbContext();
			return db.Sales
				.Include(s => s.User)
				.Where(s => s.UserId == AuthService.CurrentUser.Id)
				.ToList();
		}

        // Updates an existing sale
        // Requires Owner or Manager permissions
        // Returns true if update successful, false if sale not found or user lacks permissions
        public bool UpdateSale(int saleId, string ArticleName, decimal Price)
		{
            // Check permissions (Owner or Manager can edit)
            if (!AuthService.CanEdit()) return false;

            using var db = new AppDbContext();
			var sale = db.Sales.Find(saleId);
			if (sale == null) return false;

			sale.ArticleName = ArticleName;
			sale.Price = Price;
			db.SaveChanges();
			return true;
		}

        // Deletes a sale
        // Requires Owner or Manager permissions
        // Returns true if deletion successful, false if sale not found or user lacks permissions
        public bool DeleteSale(int saleId)
		{
            // Check permissions (Owner or Manager can delete)
            if (!AuthService.CanDelete()) return false;

            using var db = new AppDbContext();
			var sale = db.Sales.Find(saleId);
			if (sale == null) return false;

			db.Sales.Remove(sale);
			db.SaveChanges();
			return true;
		}
	}
}
