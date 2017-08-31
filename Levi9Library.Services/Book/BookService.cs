using Levi9Library.Core;
using Levi9LibraryDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using Levi9Library.Services.DTOs;

namespace Levi9LibraryServices
{
	public class BookService : IBookService
	{
		private readonly IUserService _userService;
		private readonly IBookRepository _bookRepository;

		public BookService(IUserService userService, IBookRepository bookRepository)
		{
			_userService = userService;
			_bookRepository = bookRepository;
		}

		public IList<Book> GetBooks()
		{
			return _bookRepository.GetBooks();
		}

		public IList<Book> GetAvailableBooks()
		{
			return _bookRepository.GetAvailableBooks();
		}

		public IList<BookWithDatesNoStockDto> GetBorrowedBooks(string userId)
		{
			var books = _bookRepository.GetBooks();
			var userBooks = _bookRepository.GetUserBooks();
			var userLendingHistory = from ub in userBooks
									 join b in books on ub.BookId equals b.BookId
									 where ub.ApplicationUser.Id == userId
									 select new BookWithDatesNoStockDto
									 {
										 BookId = b.BookId,
										 Title = b.Title,
										 Author = b.Author,
										 BookScore = b.BookScore,
										 DateBorrowed = ub.DateBorrowed,
										 DateReturned = ub.DateReturned
									 };
			return userLendingHistory.ToList();
		}

		public Book GetBook(int bookId)
		{
			return _bookRepository.GetBook(bookId);
		}

		public void AddBook(string title, string author, int stock, int bookScore)
		{
			var newBook = new Book
			{
				Title = title,
				Author = author,
				Stock = stock,
				BookScore = bookScore
			};
			_bookRepository.AddBook(newBook);
		}

		public void UpdateBook(int id, string title, string author, int stock, int bookScore)
		{
			var updatedBook = new Book
			{
				BookId = id,
				Title = title,
				Author = author,
				Stock = stock,
				BookScore = bookScore
			};
			_bookRepository.UpdateBook(updatedBook);
		}

		public void DeleteBook(int bookId)
		{
			var bookToBeDeleted = GetBook(bookId);
			_bookRepository.DeleteBook(bookToBeDeleted);
		}

		public Result BorrowBook(string userId, Book book)
		{
			if (book.Stock <= 0)
			{
				return Result.Fail("Not Available");
			}
			var isBorrowed = _bookRepository.IsCurrentlyBorrowed(userId, book.BookId);
			if (isBorrowed)
			{
				return Result.Fail("Already Borrowed");
			}
			book.Stock--;
			var borrowedBook = new UserBook
			{
				Id = userId,
				BookId = book.BookId,
				DateBorrowed = DateTime.UtcNow,
				DateReturned = StaticValues.DefaultDateTime
			};
			_bookRepository.BorrowBook(borrowedBook);
			return Result.Ok();
		}

		public Result ReturnBook(string userId, Book book)
		{
			if (book == null)
			{
				return Result.Fail("Book not found");
			}
			var user = _userService.GetUser(userId);
			var returnedBook = _bookRepository.GetBookToBeReturned(userId, book.BookId);
			if (returnedBook == null)
			{
				return Result.Fail("Nothing To Return");
			}
			_bookRepository.ReturnBook(user, book, returnedBook);
			return Result.Ok();
		}
	}
}
