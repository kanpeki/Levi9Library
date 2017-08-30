using System.Collections.Generic;

namespace Levi9Library.MVC.Models
{
	public class HistoryViewModel
	{
		public ICollection<LendingHistoryViewModel> BorrowedBooks;
		public int UserScore;

	}
}