using System.Collections.Generic;

namespace Levi9Library.Models.ViewModels
{
	public class MainViewModel
	{
		public ICollection<BookViewModel> AvailableBooks;
		public int UserScore;
	}
}