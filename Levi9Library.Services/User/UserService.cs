using System;
using System.Linq;
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

		public bool UpdateBan(ApplicationUser user)
		{
			var currentTime = DateTime.UtcNow;
			var lateCount = user.UserBooks.Count(book => book.DateReturned == null &&
														 book.DateBorrowed + LibraryManager.BorrowDuration < currentTime);
			if (lateCount == 0)
			{
				if (user.IsBanned && user.LastBannedDate + LibraryManager.BanDuration < currentTime)
				{
					user.IsBanned = false;
					user.OverdueCount = 0;
					_userRepository.Update(user);
				}
			}
			if (lateCount > 0 && lateCount + user.OverdueCount >= LibraryManager.MaxOverdueCount)
			{
				user.IsBanned = true;
				user.LastBannedDate = DateTime.UtcNow;
				_userRepository.Update(user);
			}
			return user.IsBanned;
		}
	}
}
