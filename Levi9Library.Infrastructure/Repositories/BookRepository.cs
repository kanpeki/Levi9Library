using Levi9LibraryDomain;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Levi9Library.Infrastructure.Repositories
{
	public class BookRepository : IBookRepository
	{
		private readonly Levi9LibraryDbContext _context;

		public BookRepository(Levi9LibraryDbContext context)
		{
			_context = context;
		}

		public IList<Book> GetBooks()
		{
			return _context
				.Books
				.Where(book => !book.IsDisabled)
				.ToList();
		}

		public IQueryable<Book> GetBooksIncludingDisabled()
		{
			return _context
				.Books;
		}

		public IList<Book> GetAvailableBooks()
		{
			return _context
				.Books
				.Where(book => !book.IsDisabled && book.BorrowedCount < book.Stock)
				.ToList();
		}

		public IList<UserBook> GetUserBooks()
		{
			return _context
				.UserBooks
				.Include(ub => ub.ApplicationUser)
				.ToList();
		}

		public Book GetBook(int bookId)
		{
			return _context
				.Books
				.FirstOrDefault(book => book.BookId == bookId);
		}

		public int AddBook(Book book)
		{
			_context.Books.Add(book);
			_context.SaveChanges();
			return book.BookId;
		}

		public void UpdateBook(Book book)
		{
			var bookToUpdate = GetBook(book.BookId);
			bookToUpdate.Title = book.Title;
			bookToUpdate.Author = book.Author;
			bookToUpdate.BookScore = book.BookScore;
			bookToUpdate.Stock = book.Stock;

			_context.SaveChanges();
		}

		public void ToggleEnabled(Book book)
		{
			_context.Entry(book).State = EntityState.Modified;
			_context.SaveChanges();
		}

		public void BorrowBook(UserBook borrowedBook)
		{
			_context.UserBooks.Add(borrowedBook);
			_context.SaveChanges();
		}

		public bool IsCurrentlyBorrowed(string userId, int bookId)
		{
			var borrowedBook = GetBookToBeReturned(userId, bookId);
			if (borrowedBook == null)
				return false;
			return true;
		}

		public void ReturnBook(ApplicationUser user, Book book, UserBook bookToBeReturned)
		{
			_context.Entry(user).State = EntityState.Modified;
			_context.Entry(book).State = EntityState.Modified;
			_context.Entry(bookToBeReturned).State = EntityState.Modified;
			_context.SaveChanges();
		}

		public UserBook GetBookToBeReturned(string userId, int bookId)
		{
			return _context
				.UserBooks
				.FirstOrDefault(ub => ub.Id.Equals(userId) && ub.BookId == bookId && ub.DateReturned == null);
		}
	}
}