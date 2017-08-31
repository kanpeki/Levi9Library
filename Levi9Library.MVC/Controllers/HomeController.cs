using Levi9Library.MVC.Models;
using Levi9LibraryServices;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

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

		public ActionResult Index(string sortOrder)
		{
			ViewBag.AuthorSort = String.IsNullOrEmpty(sortOrder) ? "author_desc" : "";
			ViewBag.TitleSort = sortOrder == "Title" ? "title_desc" : "Title";
			ViewBag.BookScoreSort = sortOrder == "BookScore" ? "bookscore_desc" : "BookScore";
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
			var model = new MainViewModel
			{
				AvailableBooks = availableBooks.ToList(),
				UserScore = user.UserScore
			};

			return View(model);
		}

		public ActionResult History(string sortOrder)
		{
			ViewBag.DateReturnedSort = String.IsNullOrEmpty(sortOrder) ? "DateReturned" : "";
			ViewBag.AuthorSort = sortOrder == "Author" ? "author_desc" : "Author";
			ViewBag.TitleSort = sortOrder == "Title" ? "title_desc" : "Title";
			ViewBag.BookScoreSort = sortOrder == "BookScore" ? "bookscore_desc" : "BookScore";
			ViewBag.DateBorrowedSort = sortOrder == "DateBorrowed" ? "dateborrowed_desc" : "DateBorrowed";

			var userId = User.Identity.GetUserId();
			var user = _userService.GetUser(userId);
			var userBooksQuery = _bookService.GetBorrowedBooks(userId);
			var userBooks = from bUb in userBooksQuery
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
					userBooks = userBooks.OrderBy(ab => ab.DateReturned);
					break;
				case "Author":
					userBooks = userBooks.OrderBy(ab => ab.Author);
					break;
				case "author_desc":
					userBooks = userBooks.OrderByDescending(ab => ab.Author);
					break;
				case "Title":
					userBooks = userBooks.OrderBy(ab => ab.Title);
					break;
				case "title_desc":
					userBooks = userBooks.OrderByDescending(ab => ab.Title);
					break;
				case "BookScore":
					userBooks = userBooks.OrderBy(ab => ab.BookScore);
					break;
				case "bookscore_desc":
					userBooks = userBooks.OrderByDescending(ab => ab.BookScore);
					break;
				case "DateBorrowed":
					userBooks = userBooks.OrderBy(ab => ab.DateBorrowed);
					break;
				case "dateborrowed_desc":
					userBooks = userBooks.OrderByDescending(ab => ab.DateBorrowed);
					break;
				default:
					userBooks = userBooks.OrderByDescending(ab => ab.DateReturned);
					break;
			}
			var model = new HistoryViewModel
			{
				BorrowedBooks = userBooks.ToList(),
				UserScore = user.UserScore
			};
			return View(model);
		}

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