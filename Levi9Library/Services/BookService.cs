using Levi9Library.Contracts;
using Levi9Library.DataAccess;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using static Levi9Library.Models.LibraryViewModels;

namespace Levi9Library.Services
{
	public class BookService : IBookService
	{
		public ICollection<Book> GetAvailableBooks()
		{
			string connectionString = ConfigurationManager.ConnectionStrings["Levi9LibraryConnection"].ConnectionString;
			string sql = "SELECT * FROM books";

			var model = new Collection<Book>();
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(sql, conn);
				SqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					var book = new Book()
					{
						BookId = (int)rdr["BookId"],
						Author = (string)rdr["Author"],
						Title = (string)rdr["Title"],
						BookScore = (int)rdr["BookScore"],
						Stock = (int)rdr["Stock"]
					};

					if (book.Stock > 0)
						model.Add(book);
				}
			}
			return model;
		}

		public ICollection<LendingHistoryViewModel> GetLendingHistory()
		{
			string connectionString = ConfigurationManager.ConnectionStrings["Levi9LibraryConnection"].ConnectionString;
			string sql = "select Books.BookId, Title, Author, BookScore, DateBorrowed, DateReturned "
				+ "from Books left join UserBooks on Books.BookId = UserBooks.BookId "
				+"where Id LIKE '"+ System.Web.HttpContext.Current.User.Identity.GetUserId() +"'";

			var model = new Collection<LendingHistoryViewModel>();
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(sql, conn);
				SqlDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					var book = new LendingHistoryViewModel
					{
						BookId = (int)rdr["BookId"],
						Author = (string)rdr["Author"],
						Title = (string)rdr["Title"],
						BookScore = (int)rdr["BookScore"],
						DateBorrowed = Convert.ToDateTime(rdr["DateBorrowed"]),
						DateReturned = rdr["DateReturned"] == DBNull.Value ? default(DateTime) : Convert.ToDateTime(rdr["DateReturned"])
				};
					model.Add(book);
				}
			}
			return model;
		}
	}
}