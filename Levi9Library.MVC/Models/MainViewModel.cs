using PagedList;

namespace Levi9Library.MVC.Models
{
	public class MainViewModel
	{
		public IPagedList<BookViewModel> AvailableBooks;
		public int UserScore;
	}
}