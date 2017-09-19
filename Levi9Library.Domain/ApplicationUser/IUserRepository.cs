namespace Levi9LibraryDomain
{
	public interface IUserRepository
	{
		ApplicationUser GetUser(string id);
		void Update(ApplicationUser user);
		void Dispose();
	}
}
