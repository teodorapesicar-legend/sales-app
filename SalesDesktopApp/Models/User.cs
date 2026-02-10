using System.Collections.Generic;
using System;

namespace SalesDesktopApp.Models 
{
	// Represents a user in the system with authentication and role-based access control
	public class User
	{
			public int Id { get; set; }
		public required string Username { get; set; }
		// BCrypt hashed password (never store plain text!)
		public required string PasswordHash { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public UserRole Role { get; set; }

		// Navigation property: All sales created by this user
		public ICollection<Sale> Sales { get; set; } = new List<Sale>();
	}

	// Defines access levels:
	// Employee - Can only add, view own data
	// Owner - Can view all, edit/delete
	// Manager - Full access, including user management (can create new users)
	public enum UserRole
	{
		Employee = 1,
		Owner = 2,
		Manager = 3
	}

}
