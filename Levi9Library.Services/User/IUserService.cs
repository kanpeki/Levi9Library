using System;
using Levi9LibraryDomain;

namespace Levi9LibraryServices
{
	public interface IUserService
	{
		ApplicationUser GetUser(string userId);
		bool UpdateBan(ApplicationUser user, DateTime currentTime);
		int GetLateCount(ApplicationUser user, DateTime currentTime);
		void Dispose();
	}
}
