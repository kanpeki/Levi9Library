using System.ComponentModel.DataAnnotations;

namespace Levi9Library.MVC.Models
{
	public class BookViewModel
	{
		public int BookId { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Author { get; set; }

		[Required]
		public int BookScore { get; set; }

		[Required]
		public int Stock { get; set; }
	}
}