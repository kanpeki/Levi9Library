﻿using Levi9Library.MVC.Models;
using Levi9LibraryServices;
using Microsoft.AspNet.Identity;
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

		public ActionResult Index()
		{
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
								})
								.ToList();
			var model = new MainViewModel
			{
				AvailableBooks = availableBooks,
				UserScore = user.UserScore
			};
			return View(model);
		}

		public ActionResult History()
		{
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