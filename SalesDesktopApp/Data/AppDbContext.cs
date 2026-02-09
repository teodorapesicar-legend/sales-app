using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SalesDesktopApp.Models;
using System;
using System.IO;

namespace SalesDesktopApp.Data
{ 
	public class AppDbContext : DbContext
	{
		public DbSet<User> User { get; set; }
		public DbSet<Sale> Sales { get; set; }
		public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
            // Get Azure SQL connection string from appsettings.json
            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false)
               .Build();

            var connectionString = config.GetConnectionString("AzureSQL");
            optionsBuilder.UseSqlServer(connectionString);
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Define Sale-User relationship
			modelBuilder.Entity<Sale>()
				.HasOne(s => s.User)
				.WithMany(u => u.Sales)
				.HasForeignKey(s => s.UserId);

            //Define Customer-Sale relationship
			modelBuilder.Entity<Customer>()
				.HasOne(s => s.CreatedByUser)
				.WithMany()
				.HasForeignKey(c => c.CreatedByUserId);
        }
    }
}
