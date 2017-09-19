using System;
using System.ComponentModel.DataAnnotations;

namespace Levi9Library.Services.DTOs
{
	public class BookWithDatesNoStockDto
	{
		public int BookId { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public int BookScore { get; set; }

		[Display(Name = "Date Borrowed")]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime DateBorrowed { get; set; }

		[Display(Name = "Date Returned")]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? DateReturned { get; set; }
	}
}
