using System;

namespace Levi9LibraryDomain
{
	public class BookWithDatesNoStockDto
	{
		public int BookId { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public int BookScore { get; set; }
		public DateTime DateBorrowed { get; set; }
		public DateTime DateReturned { get; set; }
	}
}
