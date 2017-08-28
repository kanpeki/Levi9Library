using Levi9Library.DataAccess;
using Levi9Library.Models;
using System.Collections.Generic;

namespace Levi9Library.Contracts
{
	public interface IBookService
	{
		ICollection<Book> GetAvailableBooks();
		ICollection<LibraryViewModels.LendingHistoryViewModel> GetLendingHistory();
	}
}
