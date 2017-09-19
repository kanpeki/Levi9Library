using Levi9LibraryDomain;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Levi9Library.Infrastructure
{
	public class Levi9LibraryDbContext : IdentityDbContext<ApplicationUser>
	{
		public Levi9LibraryDbContext() : base("Levi9LibraryConnection", throwIfV1Schema: false)
		{

		}

		public static Levi9LibraryDbContext Create()
		{
			return new Levi9LibraryDbContext();
		}

		public DbSet<Book> Books { get; set; }

		public DbSet<UserBook> UserBooks { get; set; }

	}
}