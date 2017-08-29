using System.Collections.Generic;

namespace Levi9Library.Models.ViewModels
{
	public class HistoryViewModel
	{
		public ICollection<LendingHistoryViewModel> BorrowedBooks;
		public int UserScore;

	}
}