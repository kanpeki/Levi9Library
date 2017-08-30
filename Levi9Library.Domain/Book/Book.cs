using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Levi9LibraryDomain
{
	public class Book
	{
		public Book()
		{
			UserBooks = new HashSet<UserBook>();
		}

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