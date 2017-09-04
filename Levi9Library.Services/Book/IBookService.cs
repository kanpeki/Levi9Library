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
		void AddBook(string title, string author, int bookScore, int stock);
		void UpdateBook(int id, string title, string author, int bookScore, int stock);
		void DeleteBook(int bookId);
		Result BorrowBook(string userId, Book book);
		Result ReturnBook(string userId, Book book);
	}
}
