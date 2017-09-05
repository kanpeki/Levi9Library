using System.ComponentModel.DataAnnotations;

namespace Levi9Library.MVC.Models
{
	public class BookViewModel
	{
		public int BookId { get; set; }

		private string _title;
		[Required(AllowEmptyStrings = false, ErrorMessage = "Enter the book title")]
		public string Title
		{
			get => _title;
			set => _title = value.Trim();
		}

		private string _author;
		[Required(AllowEmptyStrings = false, ErrorMessage = "Enter the book author.")]
		public string Author
		{
			get => _author;
			set => _author = value.Trim();
		}

		[Required(ErrorMessage = "Enter the book score")]
		[Range(1, int.MaxValue, ErrorMessage = "Book score must be greater than 0")]
		public int BookScore { get; set; }

		[Required(ErrorMessage = "Enter the book stock")]
		[Range(1, int.MaxValue, ErrorMessage = "Enter a stock greater than 0")]
		public int Stock { get; set; }
	}
}