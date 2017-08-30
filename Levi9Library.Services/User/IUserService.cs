using Levi9LibraryDomain;

namespace Levi9LibraryServices
{
	public interface IUserService
	{
		ApplicationUser GetUser(string userId);
	}
}
