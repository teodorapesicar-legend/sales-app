using System;

namespace SalesDesktopApp.Models
{
	public class Sale
	{
		public int Id { get; set; }
		public required string ArticleName { get; set; }
		public decimal Price { get; set; }
		public DateTime SaleDate { get; set; }

        // Foreign key to the user who created this sale
        public int UserId { get; set; }
        // Navigation property to the user 
        public User User { get; set; } = null!;
    }
}
