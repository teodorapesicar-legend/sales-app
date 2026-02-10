using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SalesDesktopApp.Models;
using System;
using System.IO;

namespace SalesDesktopApp.Data
{
    // Entity Framework Core database context for Azure SQL.
    // Manages Users, Sales, and Customers tables.
    public class AppDbContext : DbContext
	{
		public DbSet<User> User { get; set; }
		public DbSet<Sale> Sales { get; set; }
		public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
            // Read connection string from appsettings.json (keeps secrets out of code)
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = config.GetConnectionString("AzureSQL");
            optionsBuilder.UseSqlServer(connectionString);
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            // Configure relationship: Sales belong to Users (one-to-many)
            modelBuilder.Entity<Sale>()
				.HasOne(s => s.User)
				.WithMany(u => u.Sales)
				.HasForeignKey(s => s.UserId);

            // Configure relationship: Customers created by Users (one-to-many)
            modelBuilder.Entity<Customer>()
				.HasOne(s => s.CreatedByUser)
				.WithMany()
				.HasForeignKey(c => c.CreatedByUserId);
        }
    }
}
