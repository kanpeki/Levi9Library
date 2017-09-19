using Levi9LibraryDomain;
using PagedList;

namespace Levi9Library.MVC.Models
{
	public class MainViewModel
	{
		public IPagedList<Book> AvailableBooks { get; set; }
		public int UserScore { get; set; }
		public bool IsBanned { get; set; }
	}
}