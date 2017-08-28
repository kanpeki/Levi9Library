using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Levi9Library.DataAccess
{
	public class Book
	{
		public int BookId { get; set; }

		[Required]
		[MinLength(1)]
		public string Title { get; set; }

		[Required]
		[MinLength(1)]
		public string Author { get; set; }

		[Required]
		public int Stock { get; set; }

		[Required]
		[Display(Name = "Book Score")]
		public int BookScore { get; set; }

		public virtual ICollection<UserBook> UserBooks { get; set; }
	}
}