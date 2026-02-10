using System;
using System.Collections.Generic;
using System.Text;

namespace SalesDesktopApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string ServiceType { get; set; }
            public DateTime DateCreated { get; set; }

        // Foreign key to the user who created this customer
        public int CreatedByUserId { get; set; }
        // Navigation property to the user who created this customer
        public User CreatedByUser { get; set; } = null!;


    }
}
