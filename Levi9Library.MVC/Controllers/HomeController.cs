using Levi9Library.MVC.Models;
using Levi9LibraryServices;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;

namespace Levi9Library.MVC.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly IUserService _userService;
		private readonly IBookService _bookService;

		public HomeController(IUserService userService, IBookService bookService)
		{
			_userService = userService;
			_bookService = bookService;
		}

		//
		// Index

		public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
		{
			ViewBag.CurrentSort = sortOrder;
			ViewBag.AuthorSort = String.IsNullOrEmpty(sortOrder) ? "author_desc" : "";
			ViewBag.TitleSort = sortOrder == "Title" ? "title_desc" : "Title";
			ViewBag.BookScoreSort = sortOrder == "BookScore" ? "bookscore_desc" : "BookScore";

			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;

			var user = _userService.GetUser(User.Identity.GetUserId());
			var availableBooks = _bookService
								.GetAvailableBooks()
								.Select(b => new BookViewModel
								{
									BookId = b.BookId,
									Title = b.Title,
									Author = b.Author,
									Stock = b.Stock,
									BookScore = b.BookScore
								});
			if (!String.IsNullOrEmpty(searchString))
			{
				availableBooks = availableBooks.Where(ab => ab.Author.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
														 || ab.Title.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0);
			}
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
			var currentlyBorrowing = from bUb in userBooks.Item1
									 select new LendingHistoryViewModel
									 {
										 BookId = bUb.BookId,
										 Author = bUb.Author,
										 Title = bUb.Title,
										 BookScore = bUb.BookScore,
										 DateBorrowed = bUb.DateBorrowed,
										 DateReturned = bUb.DateReturned
									 };
			var previouslyBorrowed = from bUb in userBooks.Item2
									 select new LendingHistoryViewModel
									 {
										 BookId = bUb.BookId,
										 Author = bUb.Author,
										 Title = bUb.Title,
										 BookScore = bUb.BookScore,
										 DateBorrowed = bUb.DateBorrowed,
										 DateReturned = bUb.DateReturned
									 };
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

			var model = new HistoryViewModel
			{
				CurrentlyBorrowing = currentlyBorrowing.ToList(),
				BorrowedBooks = previouslyBorrowed.ToPagedList(pageNumber, pageSize),
				UserScore = user.UserScore
			};
			return View(model);
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
				TempData["Success"] = "Hurray! I knew Levi9-ers were starving for knowledge :)";
			}
			else if (result.IsFailure)
			{
				if (result.Error.Equals("Not Available"))
				{
					TempData["NotAvailable"] = "This book is currently not available.";
				}
				else if (result.Error.Equals("Already Borrowed"))
				{
					TempData["AlreadyBorrowed"] = "You are currently borrowing this book.";
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

	}
}