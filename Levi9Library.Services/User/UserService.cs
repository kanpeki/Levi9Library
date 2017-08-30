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

		public ApplicationUser GetUser(string id)
		{
			return _userRepository.GetUser(id);
		}
	}
}
