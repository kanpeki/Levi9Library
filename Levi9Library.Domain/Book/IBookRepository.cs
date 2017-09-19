using System.Collections.Generic;
using System.Linq;

namespace Levi9LibraryDomain
{
	public interface IBookRepository
	{
		IQueryable<Book> GetBooks();
		IQueryable<Book> GetBooksIncludingDisabled();
		IQueryable<Book> GetAvailableBooks();
		Book GetBook(int bookId);
		int AddBook(Book book);
		void UpdateBook(Book book);
		void ToggleIsArchived(Book book);
		void BorrowBook(UserBook borrowedBook);
		void ReturnBook(ApplicationUser user, Book book, UserBook bookToBeReturned);
		UserBook GetBookToBeReturned(string userId, int bookId);
		void Dispose();
	}
}
