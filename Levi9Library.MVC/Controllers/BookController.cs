using Levi9Library.Core;
using Levi9Library.MVC.Models;
using Levi9LibraryDomain;
using Levi9LibraryServices;
using Microsoft.AspNet.Identity;
using Omu.ValueInjecter;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Levi9Library.Services.DTOs;

namespace Levi9Library.MVC.Controllers
{
	[Authorize]
	public class BookController : Controller
	{
		private readonly IUserService _userService;
		private readonly IBookService _bookService;


		public BookController(IUserService userService, IBookService bookService)
		{
			_userService = userService;
			_bookService = bookService;
		}


		//
		// Index
		public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
		{
			if (System.Web.HttpContext.Current.User.IsInRole("Admin"))
			{
				return RedirectToAction("Manage", "Book");
			}

			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;

			var availableBooks = _bookService.GetAvailableBooks();

			if (!String.IsNullOrEmpty(searchString))
			{
				availableBooks = availableBooks.Where(book => book.Author.ToLower().Contains(searchString.ToLower())
														   || book.Title.ToLower().Contains(searchString.ToLower()));
			}

			ViewBag.CurrentSort = sortOrder;
			ViewBag.AuthorSort = String.IsNullOrEmpty(sortOrder) ? "author_desc" : "";
			ViewBag.TitleSort = sortOrder == "Title" ? "title_desc" : "Title";
			ViewBag.BookScoreSort = sortOrder == "BookScore" ? "bookscore_desc" : "BookScore";

			switch (sortOrder)
			{
				case "author_desc":
					availableBooks = availableBooks.OrderByDescending(ab => ab.Author);
					break;
				case "Title":
					availableBooks = availableBooks.OrderBy(ab => ab.Title);
					break;
				case "title_desc":
					availableBooks = availableBooks.OrderByDescending(ab => ab.Title);
					break;
				case "BookScore":
					availableBooks = availableBooks.OrderBy(ab => ab.BookScore);
					break;
				case "bookscore_desc":
					availableBooks = availableBooks.OrderByDescending(ab => ab.BookScore);
					break;
				default:
					availableBooks = availableBooks.OrderBy(ab => ab.Author);
					break;
			}

			int pageSize = 3;
			int pageNumber = page ?? 1;
			var userId = User.Identity.GetUserId();
			var user = _userService.GetUser(userId);

			var model = new MainViewModel
			{
				AvailableBooks = availableBooks.ToPagedList(pageNumber, pageSize),
				UserScore = user.UserScore,
				IsBanned = user.IsBanned
			};

			return View(model);
		}


		//
		// Manage
		public ActionResult Manage(string currentFilter, string searchString, int? page, bool? oldInventoryIsShown)
		{
			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;

			var inventory = _bookService.GetBooksIncludingDisabled();

			oldInventoryIsShown = oldInventoryIsShown ?? false;
			if (oldInventoryIsShown == false)
			{
				inventory = inventory.Where(b => !b.IsArchived);
			}

			if (!String.IsNullOrEmpty(searchString))
			{
				inventory = inventory.Where(book => book.Author.ToLower().Contains(searchString.ToLower())
												 || book.Title.ToLower().Contains(searchString.ToLower()));
			}

			int pageSize = 3;
			int pageNumber = page ?? 1;

			var model = new ManageViewModel
			{
				Inventory = inventory.OrderBy(book => book.Author).ToPagedList(pageNumber, pageSize),
				SearchString = searchString,
				OldInventoryIsShown = (bool)oldInventoryIsShown
			};

			return View(model);
		}


		//
		// History
		public ActionResult History(string sortOrder, int? page)
		{
			ViewBag.DateReturnedSort = String.IsNullOrEmpty(sortOrder) ? "DateReturned" : "";
			ViewBag.AuthorSort = sortOrder == "Author" ? "author_desc" : "Author";
			ViewBag.TitleSort = sortOrder == "Title" ? "title_desc" : "Title";
			ViewBag.BookScoreSort = sortOrder == "BookScore" ? "bookscore_desc" : "BookScore";
			ViewBag.DateBorrowedSort = sortOrder == "DateBorrowed" ? "dateborrowed_desc" : "DateBorrowed";

			var userId = User.Identity.GetUserId();
			var user = _userService.GetUser(userId);
			var currentTime = DateTime.UtcNow;

			var booksCurrentlyBorrowing = _bookService.GetBooksCurrentlyBorrowing(userId);
			var previouslyBorrowed = _bookService.GetBooksPreviouslyBorrowed(userId);
			IEnumerable<BookWithDatesNoStockDto> previouslyBorrowedEnum;

			var isBanned = _userService.UpdateBan(user, currentTime);

			switch (sortOrder)
			{
				case "DateReturned":
					previouslyBorrowedEnum = previouslyBorrowed.OrderBy(ab => ab.DateReturned);
					break;
				case "Author":
					previouslyBorrowedEnum = previouslyBorrowed.OrderBy(ab => ab.Author);
					break;
				case "author_desc":
					previouslyBorrowedEnum = previouslyBorrowed.OrderByDescending(ab => ab.Author);
					break;
				case "Title":
					previouslyBorrowedEnum = previouslyBorrowed.OrderBy(ab => ab.Title);
					break;
				case "title_desc":
					previouslyBorrowedEnum = previouslyBorrowed.OrderByDescending(ab => ab.Title);
					break;
				case "BookScore":
					previouslyBorrowedEnum = previouslyBorrowed.OrderBy(ab => ab.BookScore);
					break;
				case "bookscore_desc":
					previouslyBorrowedEnum = previouslyBorrowed.OrderByDescending(ab => ab.BookScore);
					break;
				case "DateBorrowed":
					previouslyBorrowedEnum = previouslyBorrowed.OrderBy(ab => ab.DateBorrowed);
					break;
				case "dateborrowed_desc":
					previouslyBorrowedEnum = previouslyBorrowed.OrderByDescending(ab => ab.DateBorrowed);
					break;
				default:
					previouslyBorrowedEnum = previouslyBorrowed.OrderByDescending(ab => ab.DateReturned);
					break;
			}

			int pageSize = 3;
			int pageNumber = page ?? 1;

			ViewBag.Times = LibraryManager.MaxOverdueCount;
			ViewBag.Duration = GetBanDurationDisplay(LibraryManager.BanDuration);

			var model = new HistoryViewModel
			{
				CurrentlyBorrowing = booksCurrentlyBorrowing.ToList(),
				BorrowedBooks = previouslyBorrowedEnum.ToPagedList(pageNumber, pageSize),
				UserScore = user.UserScore,
				IsBanned = isBanned
			};
			return View(model);
		}


		//
		// GET: /Book/Details
		public ActionResult Details(int bookId)
		{
			var book = _bookService.GetBook(bookId);
			if (book == null)
			{
				return RedirectToAction("Manage");
			}
			return View(Mapper.Map<Book, BookViewModel>(book));
		}


		//
		// GET: /Book/Create
		[Authorize(Roles = "Admin")]
		public ActionResult Create()
		{
			return View();
		}

		// POST: /Book/Create
		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public ActionResult Create(BookViewModel newBook)
		{
			if (ModelState.IsValid)
			{
				var newBookId = _bookService.AddBook(newBook.Title, newBook.Author, newBook.BookScore, newBook.Stock);
				TempData["Success"] = "The book was succesfully added.";
				return RedirectToAction("Details", new { bookId = newBookId });
			}

			return View(newBook);
		}


		//
		// GET: /Book/Edit/5
		[Authorize(Roles = "Admin")]
		public ActionResult Edit(int bookId = 0)
		{
			var editedBook = _bookService.GetBook(bookId);
			if (editedBook == null)
			{
				return HttpNotFound();
			}
			return View(Mapper.Map<Book, BookViewModel>(editedBook));
		}

		// POST: /Book/Edit/5
		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(BookViewModel editedBook)
		{
			if (ModelState.IsValid)
			{
				var result = _bookService.UpdateBook(editedBook.BookId, editedBook.Title, editedBook.Author, editedBook.BookScore,
					editedBook.Stock, editedBook.BorrowedCount);
				if (result.IsSuccess)
				{
					return RedirectToAction("Details", new { bookId = editedBook.BookId });
				}
				if (result.IsFailure)
				{
					TempData["StockLessThanBorrowed"] = result.Error;
				}
			}
			return View(editedBook);
		}


		// GET: /Book/Delete/5
		[Authorize(Roles = "Admin")]
		public ActionResult Delete(int bookId = 0)
		{
			var bookToBeDeleted = _bookService.GetBook(bookId);
			if (bookToBeDeleted == null)
			{
				return HttpNotFound();
			}
			return View(Mapper.Map<Book, BookViewModel>(bookToBeDeleted));
		}


		// POST: /Book/Delete/5
		[HttpPost, ActionName("Delete")]
		[Authorize(Roles = "Admin")]
		public ActionResult DeleteConfirmed(int bookId)
		{
			_bookService.ToggleIsArchived(bookId);
			TempData["Deleted"] = "The book was successfully deleted.";
			return RedirectToAction("Manage");
		}

		// POST: /Book/Enable
		public ActionResult Enable(int bookId = 0)
		{
			var bookToBeEnabled = _bookService.GetBook(bookId);
			if (bookToBeEnabled == null)
			{
				return HttpNotFound();
			}
			_bookService.ToggleIsArchived(bookToBeEnabled.BookId);
			TempData["Readded"] = $"You have successfully readded {bookToBeEnabled.Title} by {bookToBeEnabled.Author}.";
			return RedirectToAction("Manage");
		}

		//
		// Borrow
		public ActionResult Borrow(int bookId)
		{
			var userId = User.Identity.GetUserId();
			var user = _userService.GetUser(userId);
			var book = _bookService.GetBook(bookId);
			if (book == null || user == null)
			{
				return HttpNotFound();
			}
			var result = _bookService.BorrowBook(userId, book);

			if (result.IsSuccess)
			{
				TempData["Success"] = "You successfully borrowed ";
				TempData["BorrowedBook"] = new object[] { book.Title, book.Author };
			}
			else if (result.IsFailure)
			{
				if (result.Error.Equals("Not Available"))
				{
					TempData["NotAvailable"] = "This book is currently not available.";
				}
				else if (result.Error.Equals("Already Borrowed"))
				{
					TempData["AlreadyBorrowed"] = "You are already borrowing ";
					TempData["BorrowedBook"] = new object[] { book.Title, book.Author };
				}
				else if (result.Error.Equals("Banned") && user.LastBannedDate.HasValue)
				{
					TempData["Banned"] = $"You were late {LibraryManager.MaxOverdueCount} times.\n" +
										"You are banned for " +
										GetBanDurationDisplay(user.LastBannedDate.Value + LibraryManager.BanDuration - DateTime.UtcNow) + ".\n " +
										"You can only return books during this time.";
				}
				else if (result.Error.Equals("Still Banned"))
				{
					TempData["StillBanned"] = $"You have {LibraryManager.MaxOverdueCount} or more overdue books.\n" +
											 "You are banned until you return them and for " +
											 GetBanDurationDisplay(LibraryManager.BanDuration) + " after that.\n" +
											 "You can only return books during this time.";
				}
				else if (result.Error.Equals("Over Limit"))
				{
					TempData["OverLimit"] = $"You are already borrowing the maximum number of books: {LibraryManager.MaxBooksPerUser}.";
				}
			}

			return RedirectToAction("Index");
		}


		//
		// Return
		public ActionResult Return(int bookId)
		{
			var book = _bookService.GetBook(bookId);
			var userId = User.Identity.GetUserId();
			var result = _bookService.ReturnBook(userId, book);
			if (result.IsFailure)
			{
				return HttpNotFound();
			}

			return RedirectToAction("History");
		}

		private string GetBanDurationDisplay(TimeSpan duration)
		{
			string days = duration.Days != 0 ? duration.Days + " days " : "";
			string hours = duration.Hours != 0 ? duration.Hours + " hours " : "";
			string minutes = duration.Minutes != 0 ? duration.Minutes + " minutes " : "";
			string seconds = duration.Seconds != 0 ? duration.Seconds + " seconds" : "";

			return days + hours + minutes + seconds;
		}

		[HttpPost]
		public ActionResult CheckBan()
		{
			var userId = User.Identity.GetUserId();
			var user = _userService.GetUser(userId);
			var currentTime = DateTime.UtcNow;
			var status = _userService.UpdateBan(user, currentTime);
			return Json(new
			{
				isBanned = status
			});
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_userService?.Dispose(); //null propagation
				_bookService?.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}