using System;
using Levi9Library.Core;
using Levi9LibraryDomain;

namespace Levi9LibraryServices
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public ApplicationUser GetUser(string userId)
		{
			return _userRepository.GetUser(userId);
		}

		public void AddBan(ApplicationUser user)
		{
			user.IsBanned = true;
			user.LastBannedDate = DateTime.UtcNow;
			_userRepository.Update(user);
		}

		public Result RemoveBan(ApplicationUser user)
		{
			if (user.IsBanned && user.LastBannedDate + LibraryManager.BanDuration < DateTime.UtcNow)
			{
				user.IsBanned = false;
				user.OverdueCount = 0;
				_userRepository.Update(user);
				return Result.Ok();
			}
			return Result.Fail("Nothing to update");
		}
	}
}
