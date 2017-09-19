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

		public bool UpdateBan(ApplicationUser user, DateTime currentTime)
		{
			var lateCount = GetLateCount(user, currentTime);
			var isBanned = true;
			if (user.IsBanned)
			{
				if (lateCount == 0)
				{
					if (user.LastBannedDate + LibraryManager.BanDuration < currentTime)
					{
						isBanned = false;
						user.IsBanned = false;
						user.OverdueCount = 0;
						_userRepository.Update(user);
					}
				}
				else
				{
					user.LastBannedDate = currentTime;
					_userRepository.Update(user);
				}
			}
			else
			{
				if (lateCount + user.OverdueCount >= LibraryManager.MaxOverdueCount)
				{
					user.IsBanned = true;
					user.LastBannedDate = currentTime;
					_userRepository.Update(user);
				}
				else
				{
					isBanned = false;
				}
			}

			return isBanned;
		}

		public int GetLateCount(ApplicationUser user, DateTime currentTime)
		{
			var lateCount = user.UserBooks.Count(book => book.DateReturned == null &&
														 book.DateBorrowed + LibraryManager.BorrowDuration < currentTime);
			return lateCount;
		}

		public void Dispose()
		{
			_userRepository.Dispose();
		}
	}
}
