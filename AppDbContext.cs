using Microsoft.EntityFrameworkCore;
using SalesDesktopApp.Models;
using System;

namespace SalesDesktopApp.Data
{ 
	public class AppDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Sale> Sales { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// Here goes Azure SQL connection string
			optionsBuilder.UseSqlServer(
		/*	"Server=tcp:VAŠ-SERVER.database.windows.net,1433;" +
			"Initial Catalog=VAŠ-BAZA;" +
			"User ID=VAŠ-USERNAME;" +
			"Password=VAŠ-PASSWORD;" +
			*/
			"Encrypt=True;" +
			"TrustServerCertificate=False;"
			);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Define Sale-User relationship
			modelBuilder.Entity<Sale>(
				.HasOne(s => s.User)
				.WithMany(u => u.Sales)
				.HasForeignKey(s => s.UserId);
		}
    }
}
