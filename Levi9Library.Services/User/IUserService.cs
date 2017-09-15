using Levi9Library.Core;
using Levi9LibraryDomain;

namespace Levi9LibraryServices
{
	public interface IUserService
	{
		ApplicationUser GetUser(string userId);
		void AddBan(ApplicationUser user);
		Result RemoveBan(ApplicationUser user);
	}
}
