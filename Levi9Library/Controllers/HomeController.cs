using Levi9Library.Contracts;
using Levi9Library.DataAccess;
using Levi9Library.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Levi9Library.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly IBookService _bookService;
		private readonly ApplicationDbContext _dbContext;

		public HomeController(IBookService bookService)
		{
			this._bookService = bookService;
			this._dbContext = new ApplicationDbContext();

		}
		public ActionResult Index()
		{
			var availableBooks = _bookService.GetAvailableBooks();
			return View(availableBooks);
		}

		public ActionResult History()
		{
			var bookHistory = _bookService.GetLendingHistory();
			return View(bookHistory.ToList());
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
				where (ub.Id.Equals(userId)) && (ub.BookId == id) && (ub.DateReturned == null)
				select ub).ToList();
			
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
			using (var context = new ApplicationDbContext())
			{
				var userId = User.Identity.GetUserId();
				var returnedBook = (from ub in _dbContext.UserBooks
					where (ub.Id.Equals(userId)) && (ub.BookId == id) && (ub.DateReturned == null)
					select ub).First();
				if (returnedBook == null)
				{
					return HttpNotFound();
				}
				var bookToUpdate = (from b in _dbContext.Books
					where b.BookId == id
					select b).First();
				bookToUpdate.Stock++;
				
				//UpdateBookStock(book);
				var user = (from u in _dbContext.Users
					where u.Id.Equals(userId)
					select u).First();
				user.UserScore += bookToUpdate.BookScore;
				TempData["NewUserScore"] = user.UserScore.ToString();
				//returnedBook.DateReturned = DateTime.Now;
				var state = context.Entry(bookToUpdate).State;
				context.SaveChanges();

			}
			/*			var userId = User.Identity.GetUserId();
						var returnedBook = (from ub in _dbContext.UserBooks
							where (ub.Id.Equals(userId)) && (ub.BookId == id) && (ub.DateReturned == null)
							select ub).First();
						if (returnedBook == null)
						{
							return HttpNotFound();
						}
						var book = (from b in _dbContext.Books
							where b.BookId == id
							select b).First();
						book.Stock++;
						//UpdateBookStock(book);
						var user = (from u in _dbContext.Users
							where u.Id.Equals(userId)
							select u).First();
						user.UserScore += book.BookScore;
						//UpdateUserScore(user, book.BookScore);
						TempData["NewUserScore"] = user.UserScore.ToString();
						returnedBook.DateReturned = DateTime.Now;*/

			/*using (var cntxt = new ApplicationDbContext())
			{
				bool saveFailed;
				do
				{
					saveFailed = false;
					try
					{
						cntxt.SaveChanges();
					}
					catch (DbUpdateConcurrencyException ex)
					{
						saveFailed = true;

						// Update original values from the database 
						var entry = ex.Entries.Single();
						entry.OriginalValues.SetValues(entry.GetDatabaseValues());
					}

				} while (saveFailed);
			}*/
			//_dbContext.SaveChanges();
			return RedirectToAction("History");
		}

/*		
 		private void UpdateBookStock(Book book)
		{
			book.Stock += 1;
			_dbContext.Entry(book).State = System.Data.Entity.EntityState.Modified;
			_dbContext.SaveChanges();
		}

		pruivate void UpdateUserScore(ApplicationUser user, int bookScore)
		{
			user.UserScore += bookScore;
			_dbContext.Entry(user).State = System.Data.Entity.EntityState.Modified;
			_dbContext.SaveChanges();
		}
*/
	}
}