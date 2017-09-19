using System.Data.Entity;
using Levi9LibraryDomain;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Levi9Library.Infrastructure.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly Levi9LibraryDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public UserRepository(Levi9LibraryDbContext context)
		{
			_context = context;
			var userStore = new UserStore<ApplicationUser>(context);
			_userManager = new UserManager<ApplicationUser>(userStore);
		}

		public ApplicationUser GetUser(string id)
		{
			return _context.Users
				.FirstOrDefault(user => user.Id.Equals(id));
		}

		public bool IsAdmin(string userId)
		{
			return _userManager.IsInRole(userId, "Admin");
		}

		public void Update(ApplicationUser user)
		{
			_context.Entry(user).State = EntityState.Modified;
			_context.SaveChanges();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
