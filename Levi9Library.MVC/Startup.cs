using Levi9Library.Infrastructure;
using Levi9Library.MVC;
using Levi9LibraryDomain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace Levi9Library.MVC
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureAuth(app);
			CreateRolesAndUsers();
		}

		// Create default User roles and Admin user
		private void CreateRolesAndUsers()
		{
			var context = new Levi9LibraryDbContext();

			var userStore = new UserStore<ApplicationUser>(context);
			var userManager = new UserManager<ApplicationUser>(userStore);
			var roleStore = new RoleStore<IdentityRole>(context);
			var roleManager = new RoleManager<IdentityRole>(roleStore);


			if (!roleManager.RoleExists("Admin"))
			{
				var role = new IdentityRole { Name = "Admin" };
				roleManager.Create(role);

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
			}
		}
	}
}
