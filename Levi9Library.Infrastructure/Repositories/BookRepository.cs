using Levi9Library.Core;
using Levi9LibraryDomain;
using System;
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
				.ToList();
		}

		public IList<Book> GetAvailableBooks()
		{
			return _context
				.Books
				.Where(book => book.Stock > 0)
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

		public void AddBook(Book book)
		{
			_context.Books
					.Add(book);
			_context.SaveChanges();
		}

		public void UpdateBook(Book book)
		{
			var bookToBeUpdated = GetBook(book.BookId);

			bookToBeUpdated.Title = book.Title;
			bookToBeUpdated.Author = book.Author;
			bookToBeUpdated.Stock = book.Stock;
			bookToBeUpdated.BookScore = book.BookScore;

			_context.SaveChanges();
		}

		public void DeleteBook(Book book)
		{
			_context.Books.Remove(book);
			_context.SaveChanges();
		}

		public void BorrowBook(UserBook borrowedBook)
		{
			_context.UserBooks.Add(borrowedBook);
			_context.SaveChanges();
		}

		public bool IsCurrentlyBorrowed(string userId, int bookId)
		{
			var bookInList = _context
				.UserBooks
				.FirstOrDefault(ub => ub.Id.Equals(userId) && ub.BookId == bookId && ub.DateReturned.Equals(StaticValues.DefaultDateTime));
			if (bookInList == null)
				return false;
			return true;
		}

		public void ReturnBook(ApplicationUser user, Book book, UserBook bookToBeReturned)
		{
			bookToBeReturned.DateReturned = DateTime.UtcNow;
			book.Stock++;
			user.UserScore += book.BookScore;
			_context.SaveChanges();
		}

		public UserBook GetBookToBeReturned(string userId, int bookId)
		{
			return _context
				.UserBooks
				.FirstOrDefault(ub => ub.Id.Equals(userId) && ub.BookId == bookId && ub.DateReturned.Equals(StaticValues.DefaultDateTime));
		}
	}
}
