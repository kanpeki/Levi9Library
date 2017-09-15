using Levi9LibraryDomain;
using PagedList;

namespace Levi9Library.MVC.Models
{
	public class MainViewModel
	{
		public IPagedList<Book> AvailableBooks;
		public int UserScore;
		public bool IsBanned;
	}
}