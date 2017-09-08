using System.ComponentModel.DataAnnotations;

namespace Levi9Library.MVC.Models
{
	public class ManageBookViewModel
	{
		public int BookId { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Enter the book title")]
		public string Title { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Enter the book author.")]
		public string Author { get; set; }

		[Required(ErrorMessage = "Enter the book score")]
		[Range(1, int.MaxValue, ErrorMessage = "Book score must be greater than 0")]
		public int BookScore { get; set; }

		[Required(ErrorMessage = "Enter the book stock")]
		[Range(1, int.MaxValue, ErrorMessage = "Enter a stock greater than 0")]
		public int Stock { get; set; }

		public int BorrowedCount { get; set; }

		public bool IsDisabled { get; set; }
	}
}