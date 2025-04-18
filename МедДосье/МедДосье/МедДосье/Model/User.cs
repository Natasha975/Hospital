using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace МедДосье.Model
{
	public class User
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }	
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public DateTime BirthDate { get; set; }
		public string Email { get; set; }
		public string Gender { get; set; }
		public string Password { get; set; }

		public bool IsCurrent { get; set; }

		public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
	}
}