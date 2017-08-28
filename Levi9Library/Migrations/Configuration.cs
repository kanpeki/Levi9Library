namespace Levi9Library.Migrations
{
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.EntityFramework;
	using Models;
	using System.Data.Entity.Migrations;

	internal sealed class Configuration : DbMigrationsConfiguration<Levi9LibraryDb>
    {
        public Configuration()
        {
			AutomaticMigrationsEnabled = false;
			//AutomaticMigrationsEnabled = true;
		}

        protected override void Seed(Levi9LibraryDb context)
        {
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data. E.g.
			//
			//    context.People.AddOrUpdate(
			//      p => p.FullName,
			//      new Person { FullName = "Andrew Peters" },
			//      new Person { FullName = "Brice Lambson" },
			//      new Person { FullName = "Rowan Miller" }
			//    );
			//

			var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
/*

			context.Users.AddOrUpdate(
				u => u.UserName,
				new ApplicationUser {
					Email = "test@mailinator.com",
					EmailConfirmed = false,
					PasswordHash = new PasswordHasher().HashPassword("Test1@3"),
					SecurityStamp = Guid.NewGuid().ToString(),
					PhoneNumberConfirmed = false,
					TwoFactorEnabled = false,
					LockoutEnabled = true,
					AccessFailedCount = 0, 
					UserName = "test@mailinator.com",
					UserScore = 10
				},
				new ApplicationUser
				{
					Email = "test2@mailinator.com",
					EmailConfirmed = false,
					PasswordHash = new PasswordHasher().HashPassword("Test21@3"),
					SecurityStamp = Guid.NewGuid().ToString(),
					PhoneNumberConfirmed = false,
					TwoFactorEnabled = false,
					LockoutEnabled = true,
					AccessFailedCount = 0,
					UserName = "test2@mailinator.com",
					UserScore = 0
				},
				new ApplicationUser
				{
					Email = "test3@mailinator.com",
					EmailConfirmed = false,
					PasswordHash = new PasswordHasher().HashPassword("Test31@3"),
					SecurityStamp = Guid.NewGuid().ToString(),
					PhoneNumberConfirmed = false,
					TwoFactorEnabled = false,
					LockoutEnabled = true,
					AccessFailedCount = 0,
					UserName = "test3@mailinator.com",
					UserScore = 0
				}
			);
*/

/*

		    context.Books.AddOrUpdate(
				b => b.Title,
				new Book { Title = "Introduction to algorithms", Author = "Thomas H. Cormen", Stock = 3, BookScore = 10 },
				new Book { Title = "The Art of Computer Programming", Author = "Donald Knuth", Stock = 1, BookScore = 30 },
				new Book { Title = "Introduction to the Theory of Computation", Author = "Michael Sipser", Stock = 2, BookScore = 15 }
			);

*/
/*

			context.UserBooks.AddOrUpdate(
				ub => new { ub.Id, ub.BookId, ub.DateBorrowed },
				new UserBook { Id = um.FindByEmail("test@mailinator.com").Id, BookId = 1, DateBorrowed = new DateTime(2017,04,30), DateReturned = new DateTime(2017,06,02) },
				new UserBook { Id = um.FindByEmail("test@mailinator.com").Id, BookId = 2, DateBorrowed = new DateTime(2017,06,10) },
				new UserBook { Id = um.FindByEmail("test2@mailinator.com").Id, BookId = 1, DateBorrowed = new DateTime(2017,06,20) },
				new UserBook { Id = um.FindByEmail("test2@mailinator.com").Id, BookId = 3, DateBorrowed = new DateTime(2017,06,27) }
			);

*/
		}
	}
}
