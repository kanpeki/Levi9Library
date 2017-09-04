using System;
using System.Linq;
using Levi9Library.Core;
using Levi9LibraryDomain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Levi9Library.Infrastructure.Migrations
{
	using System.Data.Entity.Migrations;

	internal sealed class Configuration : DbMigrationsConfiguration<Levi9LibraryDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(Levi9LibraryDbContext context)
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

			var userStore = new UserStore<ApplicationUser>(context);
			var userManager = new UserManager<ApplicationUser>(userStore);
			var roleStore = new RoleStore<IdentityRole>(context);
			var roleManager = new RoleManager<IdentityRole>(roleStore);

			/*if (!roleManager.RoleExists("Admin"))
			{
				roleManager.Create(new IdentityRole("Admin"));
			}

			if (!context.Users.Any(u => u.Email == "admin@mailinator.com"))
			{
				// Create User
				var user = new ApplicationUser
				{
					Email = "admin@mailinator.com",
					UserName = "admin@mailinator.com",
				};
				userManager.Create(user, "Pa$$W0rD!");

				// Add User To Role
				if (!userManager.IsInRole(user.Id, "Admin"))
				{
					userManager.AddToRole(user.Id, "Admin");
				}
			}*/

			/*			context.Users.AddOrUpdate(
							u => u.UserName,
							new ApplicationUser
							{
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

			/*			context.Books.AddOrUpdate(
							b => b.Title,
							new Book { Title = "Introduction to algorithms", Author = "Thomas H. Cormen", Stock = 3, BookScore = 10 },
							new Book { Title = "The Art of Computer Programming", Author = "Donald Knuth", Stock = 1, BookScore = 30 },
							new Book { Title = "Introduction to the Theory of Computation", Author = "Michael Sipser", Stock = 2, BookScore = 15 },
							new Book { Title = "Code Complete", Author = "Steve McConnell", Stock = 2, BookScore = 20 },
							new Book { Title = "Modern Operating Systems", Author = "Andrew S. Tanenbaum", Stock = 2, BookScore = 10 },
							new Book { Title = "Domain-driven Design", Author = "Eric J. Evans", Stock = 1, BookScore = 15 },
							new Book { Title = "The Clean Coder", Author = "John Martin", Stock = 1, BookScore = 20 },
							new Book { Title = "Hacking: The Art of Exploitation", Author = "Jon Erickson", Stock = 1, BookScore = 10 }
						);
			*/

			/*			context.UserBooks.AddOrUpdate(
							ub => new { ub.Id, ub.BookId, ub.DateBorrowed },
							new UserBook { Id = userManager.FindByEmail("test@mailinator.com").Id, BookId = 1, DateBorrowed = new DateTime(2017, 04, 30), DateReturned = new DateTime(2017, 06, 02) },
							new UserBook { Id = userManager.FindByEmail("test@mailinator.com").Id, BookId = 2, DateBorrowed = new DateTime(2017, 06, 10), DateReturned = null },
							new UserBook { Id = userManager.FindByEmail("test2@mailinator.com").Id, BookId = 1, DateBorrowed = new DateTime(2017, 06, 20), DateReturned = null },
							new UserBook { Id = userManager.FindByEmail("test2@mailinator.com").Id, BookId = 3, DateBorrowed = new DateTime(2017, 06, 27), DateReturned = null }
						);
			*/

		}
	}
}