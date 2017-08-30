using System.Collections.Generic;
using System.Linq;
using Levi9Library.Core;
using Levi9LibraryDomain;

namespace Levi9LibraryServices
{
	public interface IBookService
	{
		IList<Book> GetBooks();
		IList<Book> GetAvailableBooks();
		IQueryable<BookWithDates> GetBorrowedBooks(string userId);
		Book GetBook(int bookId);
		void AddBook(string title, string author, int stock, int bookScore);
		void UpdateBook(int id, string title, string author, int stock, int bookScore);
		void DeleteBook(int bookId);
		Result BorrowBook(string userId, Book book);
		Result ReturnBook(string userId, Book book);
	}
}
