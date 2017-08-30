using System;
using Levi9Library.Core;
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
				.ToList();
		}

		public IList<Book> GetAvailableBooks()
		{
			return _context
				.Books
				.Where(book => book.Stock > 0)
				.ToList();
		}

		public IQueryable<BookWithDates> GetLendingHistory(string userId)
		{

			/*		var userLendingHistory = _context.UserBooks
											.Join(_context.Books,
												ub => ub.BookId,
												b => b.BookId,
												(ub, b) => new
												{
													Book = b,
													UserBook = ub
												})
											.Where(bUb => bUb.UserBook.ApplicationUser.Id.Equals(userId))
											.Select(bUb => new BookWithDates
											{
												Book = new Book
												{
													BookId = bUb.Book.BookId,
													Title = bUb.Book.Title,
													Author = bUb.Book.Author,
													Stock = bUb.Book.Stock,
													BookScore = bUb.Book.BookScore
												},
												DateBorrowed = bUb.UserBook.DateBorrowed,
												DateReturned = bUb.UserBook.DateReturned
											});*/
			var userLendingHistory = from ub in _context.UserBooks
									 join b in _context.Books on ub.BookId equals b.BookId
									 where ub.ApplicationUser.Id == userId
									 select new BookWithDates
									 {
										 Book = new Book
										 {
											 BookId = b.BookId,
											 Title = b.Title,
											 Author = b.Author,
											 Stock = b.Stock,
											 BookScore = b.BookScore
										 },
										 DateBorrowed = ub.DateBorrowed,
										 DateReturned = ub.DateReturned
									 };
			return userLendingHistory;
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
