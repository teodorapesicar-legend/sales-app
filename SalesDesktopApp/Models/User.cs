using System.Collections.Generic;
using System;

namespace SalesDesktopApp.Models 
{

	public class User
	{
			public int Id { get; set; }
		public required string Username { get; set; }
		public required string PasswordHash { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public UserRole Role { get; set; }

		// Navigation property - Link to Sales table
		public ICollection<Sale> Sales { get; set; } = new List<Sale>();
	}

	public enum UserRole
	{
		Employee = 1,
		Owner = 2,
		Manager = 3
	}

}
