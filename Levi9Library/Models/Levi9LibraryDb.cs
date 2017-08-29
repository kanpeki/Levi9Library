namespace Levi9Library.Models
{
	using Microsoft.AspNet.Identity.EntityFramework;
	using System.Data.Entity;

	public class Levi9LibraryDb : IdentityDbContext<ApplicationUser> //, ILevi9LibraryDb
	{
		public Levi9LibraryDb() : base("Levi9LibraryConnection", throwIfV1Schema: false)
		{

		}

		public static Levi9LibraryDb Create()
		{
			return new Levi9LibraryDb();
		}

		public DbSet<Book> Books { get; set; }

		public DbSet<UserBook> UserBooks { get; set; }

	}
}