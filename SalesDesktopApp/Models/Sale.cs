using System;

namespace SalesDesktopApp.Models
{
	public class Sale
	{
		public int Id { get; set; }
		public required string ArticleName { get; set; }
		public decimal Price { get; set; }
		public DateTime SaleDate { get; set; }

		// Foreign key - Link to Sales table
		public int UserId { get; set; }
		public User User { get; set; } = null!;
    }
}
