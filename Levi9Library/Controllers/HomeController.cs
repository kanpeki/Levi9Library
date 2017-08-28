using Levi9Library.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Levi9Library.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly Levi9LibraryDb _dbContext;

		public HomeController()
		{
			_dbContext = new Levi9LibraryDb();
		}

		public HomeController (Levi9LibraryDb db)
		{
			_dbContext = db;
		}

		public ActionResult Index()
		{
			var availableBooks = _dbContext.Books
				.Where(book => book.Stock > 0)
				.ToList();
			return View(availableBooks);
		}

		public ActionResult History()
		{
			var userId = User.Identity.GetUserId();
			var userBooks = from ub in _dbContext.UserBooks
							join b in _dbContext.Books on ub.BookId equals b.BookId
							where ub.ApplicationUser.Id == userId
							select new LibraryViewModels.LendingHistoryViewModel
							{
								BookId = b.BookId,
								Author = b.Author,
								Title = b.Title,
								BookScore = b.BookScore,
								DateBorrowed = ub.DateBorrowed,
								DateReturned = ub.DateReturned ?? default(DateTime) //if DateReturned is null assign default(DateTime)
							};
			return View(userBooks.ToList());
		}

		public ActionResult Borrow(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var book = (from b in _dbContext.Books
				where b.BookId == id
				select b).First();
			if (book == null)
			{
				return HttpNotFound();
			}
			var userId = User.Identity.GetUserId();
			var bookInList = (from ub in _dbContext.UserBooks
							where ub.Id.Equals(userId) && ub.BookId.Equals(id) && ub.DateReturned.Equals(null)
							select ub)
							.ToList();
			
			if (book.Stock > 0 && bookInList.Count == 0)
			{
				TempData["Success"] = "Hurray! I knew Levi9-ers were starving for knowledge :)";
				book.Stock--;
				var borrowedBook = new UserBook
				{
					Id = userId,
					BookId = (int) id,
					DateBorrowed = DateTime.Now
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


		public ActionResult Return(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var userId = User.Identity.GetUserId();
			var returnedBook = (from ub in _dbContext.UserBooks
								where ub.Id.Equals(userId) && ub.BookId.Equals(id) && ub.DateReturned.Equals(null)
								select ub)
								.FirstOrDefault();
			if (returnedBook == null)
			{
				return HttpNotFound();
			}

			var bookToUpdate = (from b in _dbContext.Books
								where b.BookId.Equals(id)
								select b)
								.First();
			bookToUpdate.Stock++;

			var user = (from u in _dbContext.Users
						where u.Id.Equals(userId)
						select u)
						.First();
			user.UserScore += bookToUpdate.BookScore;

			TempData["NewUserScore"] = user.UserScore.ToString();
			returnedBook.DateReturned = DateTime.Now;

			_dbContext.Entry(bookToUpdate).State = EntityState.Modified;
			_dbContext.Entry(user).State = EntityState.Modified;
			_dbContext.Entry(returnedBook).State = EntityState.Modified;

			_dbContext.SaveChanges();

			return RedirectToAction("History");
		}
	}
}