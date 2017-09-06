using Levi9Library.Core;
using Levi9Library.Services.DTOs;
using Levi9LibraryDomain;
using System;
using System.Collections.Generic;

namespace Levi9LibraryServices
{
	public interface IBookService
	{
		IList<Book> GetBooks();
		IList<Book> GetAvailableBooks();
		Tuple<IList<BookWithDatesNoStockDto>, IList<BookWithDatesNoStockDto>> GetBorrowedBooks(string userId);
		Book GetBook(int bookId);
		int AddBook(string title, string author, int bookScore, int stock);
		Result UpdateBook(int bookId, string title, string author, int bookScore, int stock, int borrowedCount);
		void DeleteBook(int bookId);
		Result BorrowBook(string userId, Book book);
		Result ReturnBook(string userId, Book book);
	}
}
