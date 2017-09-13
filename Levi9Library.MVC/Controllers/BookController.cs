using Levi9Library.Core;
using Levi9Library.MVC.Models;
using Levi9Library.Services.DTOs;
using Levi9LibraryDomain;
using Levi9LibraryServices;
using Microsoft.AspNet.Identity;
using Omu.ValueInjecter;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;

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

			var user = _userService.GetUser(User.Identity.GetUserId());
			var availableBooks = _bookService
				.GetAvailableBooks()
				.Select(b => Mapper.Map<Book, BookViewModel>(b));

			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.SearchString = searchString;
			ViewBag.CurrentFilter = searchString;

			if (!String.IsNullOrEmpty(searchString))
			{
				availableBooks = availableBooks.Where(ab => ab.Author.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 ||
															ab.Title.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0);
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

			var model = new MainViewModel
			{
				AvailableBooks = availableBooks.ToPagedList(pageNumber, pageSize),
				UserScore = user.UserScore
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
				inventory = inventory.Where(b => !b.IsDisabled);
			}

			if (!String.IsNullOrEmpty(searchString))
			{
				inventory = inventory.Where(b => b.Author.ToLower().Contains(searchString.ToLower())
												 || b.Title.ToLower().Contains(searchString.ToLower()));
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


		//public ActionResult Manage(string currentFilter, string searchString, int? page, bool? oldInventoryIsShown)
		//{
		//	ViewBag.CurrentFilter = searchString;

		//	var inventory = _bookService.GetBooksIncludingDisabled();

		//	if (oldInventoryIsShown.HasValue && oldInventoryIsShown == false)
		//	{
		//		inventory = inventory.Where(b => !b.IsDisabled);
		//	}
		//	if (!String.IsNullOrEmpty(searchString))
		//	{
		//		inventory = inventory.Where(b => b.Author.ToLower().Contains(searchString.ToLower())
		//								 || b.Title.ToLower().Contains(searchString.ToLower()));
		//	}
		//	int pageNumber = page ?? 1;
		//	int pageSize = 3;
		//	var model = new ManageViewModel
		//	{
		//		Inventory = inventory.OrderBy(book => book.Author).ToPagedList(pageNumber, pageSize),
		//		SearchString = searchString,
		//		OldInventoryIsShown = oldInventoryIsShown ?? false
		//	};

		//	return View(model);
		//}


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
			var userBooks = _bookService.GetBorrowedBooks(userId);
			var currentlyBorrowing = userBooks.Item1.Select(b => Mapper.Map<BookWithDatesNoStockDto, BorrowedBookViewModel>(b));
			var previouslyBorrowed = userBooks.Item2.Select(b => Mapper.Map<BookWithDatesNoStockDto, BorrowedBookViewModel>(b));

			switch (sortOrder)
			{
				case "DateReturned":
					previouslyBorrowed = previouslyBorrowed.OrderBy(ab => ab.DateReturned);
					break;
				case "Author":
					previouslyBorrowed = previouslyBorrowed.OrderBy(ab => ab.Author);
					break;
				case "author_desc":
					previouslyBorrowed = previouslyBorrowed.OrderByDescending(ab => ab.Author);
					break;
				case "Title":
					previouslyBorrowed = previouslyBorrowed.OrderBy(ab => ab.Title);
					break;
				case "title_desc":
					previouslyBorrowed = previouslyBorrowed.OrderByDescending(ab => ab.Title);
					break;
				case "BookScore":
					previouslyBorrowed = previouslyBorrowed.OrderBy(ab => ab.BookScore);
					break;
				case "bookscore_desc":
					previouslyBorrowed = previouslyBorrowed.OrderByDescending(ab => ab.BookScore);
					break;
				case "DateBorrowed":
					previouslyBorrowed = previouslyBorrowed.OrderBy(ab => ab.DateBorrowed);
					break;
				case "dateborrowed_desc":
					previouslyBorrowed = previouslyBorrowed.OrderByDescending(ab => ab.DateBorrowed);
					break;
				default:
					previouslyBorrowed = previouslyBorrowed.OrderByDescending(ab => ab.DateReturned);
					break;
			}

			int pageSize = 3;
			int pageNumber = page ?? 1;

			ViewBag.Times = LibraryManager.MaxOverdueCount;
			ViewBag.Duration = GetBanDurationDisplay(LibraryManager.BanDuration);

			var model = new HistoryViewModel
			{
				CurrentlyBorrowing = currentlyBorrowing.ToList(),
				BorrowedBooks = previouslyBorrowed.ToPagedList(pageNumber, pageSize),
				UserScore = user.UserScore,
				OverdueCount = user.OverdueCount
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
			_bookService.ToggleEnabled(bookId);
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
			_bookService.ToggleEnabled(bookToBeEnabled.BookId);
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
				else if (result.Error.Equals("Still Banned") && user.LastBannedDate.HasValue)
				{
					TempData["StillBanned"] = $"You were late {LibraryManager.MaxOverdueCount} times.\n You are banned for "
											+ GetBanDurationDisplay(user.LastBannedDate.Value + LibraryManager.BanDuration - DateTime.UtcNow)
											+ ".\n You can only return books during this time.";
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
			string display = $"{duration.Days} days and {duration:hh\\:mm\\:ss}";
			return display;
		}
	}
}