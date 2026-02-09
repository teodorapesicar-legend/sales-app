using System;

namespace SalesDesktopApp.Models 
{

	public class User
	{
		
			public int Id { get; set; }
		public string Username { get; set; }
		public string PasswordHash { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public UserRole Role { get; set; }

		// Navigation property - Link to Sales table
		public ICollection<Sale> Sales { get; set; }
	}

	public enum UserRole
	{
		Employee = 0,
		Owner = 1,
		Manager = 2
	}

}
