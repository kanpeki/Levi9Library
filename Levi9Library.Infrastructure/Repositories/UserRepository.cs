using Levi9LibraryDomain;
using System.Linq;

namespace Levi9Library.Infrastructure.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly Levi9LibraryDbContext _context;

		public UserRepository(Levi9LibraryDbContext context)
		{
			_context = context;
		}

		public ApplicationUser GetUser(string id)
		{
			return _context.Users
				.FirstOrDefault(user => user.Id.Equals(id));
		}
	}
}
