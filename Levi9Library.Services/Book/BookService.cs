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

		public IList<Book> GetBooks()
		{
			return _bookRepository.GetBooks();
		}

		public IQueryable<Book> GetBooksIncludingDisabled()
		{
			return _bookRepository.GetBooksIncludingDisabled();
		}

		public IList<Book> GetAvailableBooks()
		{
			return _bookRepository.GetAvailableBooks();
		}

		public Tuple<IList<BookWithDatesNoStockDto>, IList<BookWithDatesNoStockDto>> GetBorrowedBooks(string userId)
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
			var currentlyBorrowing = userLendingHistory.Where(b => b.DateReturned == null).ToList();
			var previouslyBorrowed = userLendingHistory.Where(b => b.DateReturned != null).ToList();
			return new Tuple<IList<BookWithDatesNoStockDto>, IList<BookWithDatesNoStockDto>>(currentlyBorrowing, previouslyBorrowed);
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
			bookToModify.IsDisabled = !bookToModify.IsDisabled;
			_bookRepository.ToggleEnabled(bookToModify);
		}

		public Result BorrowBook(string userId, Book book)
		{
			if (!(book.BorrowedCount < book.Stock) || book.IsDisabled)
			{
				return Result.Fail("Not Available");
			}
			var user = _userService.GetUser(userId);
			if (user.IsBanned)
			{
				if (DateTime.UtcNow - user.LastBannedDate < LibraryManager.BanDuration)
				{
					return Result.Fail("Still Banned");
				}
				user.IsBanned = false;
				user.OverdueCount = 0;
			}
			if (GetBorrowedBooks(userId).Item1.Count >= LibraryManager.MaxBooksPerUser)
			{
				return Result.Fail("Over Limit");
			}
			var isBorrowed = _bookRepository.IsCurrentlyBorrowed(userId, book.BookId);
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
				user.IsBanned = true;
				user.LastBannedDate = DateTime.UtcNow;
			}
			_bookRepository.ReturnBook(user, book, returnedBook);
			return Result.Ok();
		}
	}
}
