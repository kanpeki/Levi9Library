using Levi9Library.Core;
using Levi9Library.Services.DTOs;
using Levi9LibraryDomain;
using System;
using System.Collections.Generic;
using System.Linq;

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

		public IQueryable<Book> GetBooks()
		{
			return _bookRepository.GetBooks();
		}

		public IQueryable<Book> GetBooksIncludingDisabled()
		{
			return _bookRepository.GetBooksIncludingDisabled();
		}

		public IQueryable<Book> GetAvailableBooks()
		{
			return _bookRepository.GetAvailableBooks();
		}

		public IList<BookWithDatesNoStockDto> GetBooksCurrentlyBorrowing(string userId)
		{
			var userLendingHistory = GetBorrowedBooks(userId);
			var booksCurrentlyBorrowing = userLendingHistory?.Where(book => book.DateReturned == null);
			return booksCurrentlyBorrowing?.ToList();
		}

		public IQueryable<BookWithDatesNoStockDto> GetBooksPreviouslyBorrowed(string userId)
		{
			var userLendingHistory = GetBorrowedBooks(userId);
			var booksPreviouslyBorrowed = userLendingHistory?.Where(book => book.DateReturned != null);
			return booksPreviouslyBorrowed as IQueryable<BookWithDatesNoStockDto>;
		}

		private IEnumerable<BookWithDatesNoStockDto> GetBorrowedBooks(string userId)
		{
			var books = _bookRepository.GetBooksIncludingDisabled();
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
			return userLendingHistory;
		}

		public Book GetBook(int bookId)
		{
			return _bookRepository.GetBook(bookId);
		}

		public int AddBook(string title, string author, int bookScore, int stock)
		{
			var newBook = new Book
			{
				Title = title,
				Author = author,
				BookScore = bookScore,
				Stock = stock
			};
			var newBookId = _bookRepository.AddBook(newBook);
			return newBookId;
		}

		public Result UpdateBook(int bookId, string title, string author, int bookScore, int stock, int borrowedCount)
		{
			var bookToUpdate = _bookRepository.GetBook(bookId);
			if (stock < bookToUpdate.BorrowedCount)
			{
				return Result.Fail($"Stock can't be less than the number of books currently borrowed: {bookToUpdate.BorrowedCount}.");
			}
			var updatedBook = new Book
			{
				BookId = bookId,
				Title = title,
				Author = author,
				BookScore = bookScore,
				Stock = stock,
				BorrowedCount = borrowedCount
			};
			_bookRepository.UpdateBook(updatedBook);
			return Result.Ok();
		}

		public void ToggleEnabled(int bookId)
		{
			var bookToModify = GetBook(bookId);
			bookToModify.IsArchived = !bookToModify.IsArchived;
			_bookRepository.ToggleEnabled(bookToModify);
		}

		public Result BorrowBook(string userId, Book book)
		{
			if (!(book.BorrowedCount < book.Stock) || book.IsArchived)
			{
				return Result.Fail("Not Available");
			}
			var user = _userService.GetUser(userId);
			if (user.IsBanned)
			{
				var result = _userService.RemoveBan(user);
				if (result.IsFailure)
				{
					return Result.Fail("Still Banned");
				}
			}
			var booksCurrentlyBorrowing = GetBooksCurrentlyBorrowing(userId).ToList();
			if (booksCurrentlyBorrowing.Count >= LibraryManager.MaxBooksPerUser)
			{
				return Result.Fail("Over Limit");
			}
			var isBorrowed = _bookRepository.GetBookToBeReturned(userId, book.BookId) != null;
			if (isBorrowed)
			{
				return Result.Fail("Already Borrowed");
			}
			book.BorrowedCount++;
			var borrowedBook = new UserBook
			{
				Id = userId,
				BookId = book.BookId,
				DateBorrowed = DateTime.UtcNow,
				DateReturned = null
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
				return Result.Fail("Nothing to return");
			}
			book.BorrowedCount--;
			user.UserScore += book.BookScore;
			returnedBook.DateReturned = DateTime.UtcNow;
			if (returnedBook.DateReturned - returnedBook.DateBorrowed > LibraryManager.BorrowDuration)
			{
				// OverdueCount will be larger than MaxOverDueCount when a user still has unreturned books the moment he is banned
				// in this case his ban might be longer than the BanDuration, as the ban reinstates itself each time a late book is returned
				user.OverdueCount++;
			}
			if (user.OverdueCount >= LibraryManager.MaxOverdueCount)
			{
				_userService.AddBan(user);
			}
			_bookRepository.ReturnBook(user, book, returnedBook);
			return Result.Ok();
		}
	}
}
