using System.Collections.Generic;
using Levi9Library.Core;
using Levi9Library.Services.DTOs;
using Levi9LibraryDomain;
using System.Linq;

namespace Levi9LibraryServices
{
	public interface IBookService
	{
		IQueryable<Book> GetBooks();
		IQueryable<Book> GetBooksIncludingDisabled();
		IQueryable<Book> GetAvailableBooks();
		IList<BookWithDatesNoStockDto> GetBooksCurrentlyBorrowing(string userId);
		IList<BookWithDatesNoStockDto> GetBooksPreviouslyBorrowed(string userId);
		Book GetBook(int bookId);
		int AddBook(string title, string author, int bookScore, int stock);
		Result UpdateBook(int bookId, string title, string author, int bookScore, int stock, int borrowedCount);
		void ToggleIsArchived(int bookId);
		Result BorrowBook(string userId, Book book);
		Result ReturnBook(string userId, Book book);
		void Dispose();
	}
}
