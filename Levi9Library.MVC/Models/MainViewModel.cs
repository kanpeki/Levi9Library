using System.Collections.Generic;

namespace Levi9Library.MVC.Models
{
	public class MainViewModel
	{
		public ICollection<BookViewModel> AvailableBooks;
		public int UserScore;
	}
}