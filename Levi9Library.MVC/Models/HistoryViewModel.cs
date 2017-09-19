using System.Collections.Generic;
using Levi9Library.Services.DTOs;
using PagedList;

namespace Levi9Library.MVC.Models
{
	public class HistoryViewModel
	{
		public IList<BookWithDatesNoStockDto> CurrentlyBorrowing { get; set; }
		public IPagedList<BookWithDatesNoStockDto> BorrowedBooks { get; set; }
		public int UserScore { get; set; }
		public bool IsBanned { get; set; }
	}
}