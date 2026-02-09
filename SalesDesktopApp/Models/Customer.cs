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

        //Created by User
        public int CreatedByUserId { get; set; }
            public User CreatedByUser { get; set; } = null!;


    }
}
