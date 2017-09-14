using System.Collections.Generic;
using PagedList;

namespace Levi9Library.MVC.Models
{
	public class HistoryViewModel
	{
		public IList<BorrowedBookViewModel> CurrentlyBorrowing;
		public IPagedList<BorrowedBookViewModel> BorrowedBooks;
		public int UserScore;
		public int OverdueCount;
	}
}