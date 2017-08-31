using System.Collections.Generic;
using PagedList;

namespace Levi9Library.MVC.Models
{
	public class HistoryViewModel
	{
		public IList<LendingHistoryViewModel> CurrentlyBorrowing;
		public IPagedList<LendingHistoryViewModel> BorrowedBooks;
		public int UserScore;

	}
}