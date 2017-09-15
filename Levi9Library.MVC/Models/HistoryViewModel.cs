using System.Collections.Generic;
using Levi9Library.Services.DTOs;
using PagedList;

namespace Levi9Library.MVC.Models
{
	public class HistoryViewModel
	{
		public IList<BookWithDatesNoStockDto> CurrentlyBorrowing;
		public IPagedList<BookWithDatesNoStockDto> BorrowedBooks;
		public int UserScore;
		public bool IsBanned;
	}
}