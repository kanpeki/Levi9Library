using System.Collections.Generic;

namespace Levi9LibraryDomain
{
	public interface IBookRepository
	{
		IList<Book> GetBooks();
		IList<Book> GetAvailableBooks();
		IList<UserBook> GetUserBooks();
		Book GetBook(int bookId);
		void AddBook(Book book);
		void UpdateBook(Book book);
		void DeleteBook(Book book);
		void BorrowBook(UserBook borrowedBook);
		bool IsCurrentlyBorrowed(string userId, int bookId);
		void ReturnBook(ApplicationUser user, Book book, UserBook bookToBeReturned);
		UserBook GetBookToBeReturned(string userId, int bookId);
	}
}
