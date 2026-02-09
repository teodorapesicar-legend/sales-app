using SalesDesktopApp.Data;
using SalesDesktopApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SalesDesktopApp.Services
{
	public class SalesService
	{
		// Add a new Sale
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

		// Get all Sales with User info
		public List<Sale> GetAllSales()
		{
			using var db = new AppDbContext();
			return db.Sales
				.Include(s => s.User)
				.ToList();
		}

		// Get Sales by Current User
		public List<Sale> GetSalesByUser(int userId)
		{
			using var db = new AppDbContext();
			return db.Sales
				.Include(s => s.User)
				.Where(s => s.UserId == AuthService.CurrentUser.Id)
				.ToList();
		}

		// Edit a Sale
		public bool UpdateSale(int saleId, string ArticleName, decimal Price)
		{
			if (!AuthService.CanEdit()) return false;

			using var db = new AppDbContext();
			var sale = db.Sales.Find(saleId);
			if (sale == null) return false;

			sale.ArticleName = ArticleName;
			sale.Price = Price;
			db.SaveChanges();
			return true;
		}

		// Delete a Sale
		public bool DeleteSale(int saleId)
		{
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
