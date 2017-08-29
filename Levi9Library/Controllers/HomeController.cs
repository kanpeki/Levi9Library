using Levi9Library.Models;
using Levi9Library.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Levi9Library.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly Levi9LibraryDb _dbContext;
		public static DateTime DefaultDateTime { get; } = default(DateTime);

		public HomeController()
		{
			_dbContext = new Levi9LibraryDb();
		}

		public HomeController(Levi9LibraryDb db)
		{
			_dbContext = db;
		}

		public ActionResult Index()
		{
			var userId = User.Identity.GetUserId();
			var user = _dbContext.Users.FirstOrDefault(u => u.Id.Equals(userId));
			var availableBooks = _dbContext.Books
								.Where(book => book.Stock > 0)
								.Select(b => new BookViewModel
								{
									BookId = b.BookId,
									Title = b.Title,
									Author = b.Author,
									Stock = b.Stock,
									BookScore = b.BookScore
								});
			var model = new MainViewModel
			{
				AvailableBooks = availableBooks.ToList(),
				UserScore = user == null ? 0 : user.UserScore
			};
			return View(model);
		}

		public ActionResult History()
		{
			var userId = User.Identity.GetUserId();
			var user = _dbContext.Users.FirstOrDefault(u => u.Id.Equals(userId));
			var userBooks = from ub in _dbContext.UserBooks
							join b in _dbContext.Books on ub.BookId equals b.BookId
							where ub.ApplicationUser.Id == userId
							select new LendingHistoryViewModel
							{
								BookId = b.BookId,
								Author = b.Author,
								Title = b.Title,
								BookScore = b.BookScore,
								DateBorrowed = ub.DateBorrowed,
								DateReturned = ub.DateReturned
							};
			var model = new HistoryViewModel
			{
				BorrowedBooks = userBooks.ToList(),
				UserScore = user == null ? 0 : user.UserScore
			};
			return View(model);
		}

		public ActionResult Borrow(int id = 0)
		{
			var book = _dbContext.Books.Single(b => b.BookId == id);
			if (book == null)
			{
				return HttpNotFound();
			}
			var userId = User.Identity.GetUserId();

			/*var bookInList = (from ub in _dbContext.UserBooks
							  where ub.Id.Equals(userId) && ub.BookId == id && ub.DateReturned.Equals(DefaultDateTime)
							  select ub)
							  .ToList();*/

			var bookInList = _dbContext
							.UserBooks
							.Where(ub => ub.Id.Equals(userId) && ub.BookId == id && ub.DateReturned.Equals(DefaultDateTime))
							.ToList();

			if (book.Stock > 0 && bookInList.Count == 0)
			{
				TempData["Success"] = "Hurray! I knew Levi9-ers were starving for knowledge :)";
				book.Stock--;
				var borrowedBook = new UserBook
				{
					Id = userId,
					BookId = id,
					DateBorrowed = DateTime.UtcNow,
					DateReturned = DefaultDateTime
				};
				_dbContext.UserBooks.Add(borrowedBook);
				_dbContext.SaveChanges();
			}
			else if (book.Stock <= 0)
			{
				TempData["NotAvailable"] = "This book is currently not available.";

			}
			else if (bookInList.Count != 0)
			{
				TempData["AlreadyBorrowed"] = "You are already borrowing this book.Please return that copy before requesting another one.";
			}
			return RedirectToAction("Index");
		}


		public ActionResult Return(int id = 0)
		{
			var userId = User.Identity.GetUserId();
			var returnedBook = _dbContext.UserBooks.Single(ub => ub.Id.Equals(userId) && ub.BookId == id && ub.DateReturned.Equals(DefaultDateTime));
			var bookToUpdate = _dbContext.Books.Single(b => b.BookId == id);
			var user = _dbContext.Users.Single(u => u.Id.Equals(userId));

			if (returnedBook == null || bookToUpdate == null || user == null)
			{
				return HttpNotFound();
			}

			returnedBook.DateReturned = DateTime.UtcNow;
			bookToUpdate.Stock++;
			user.UserScore += bookToUpdate.BookScore;
			TempData["NewUserScore"] = user.UserScore.ToString();
			_dbContext.SaveChanges();
			return RedirectToAction("History");
		}

	}
}