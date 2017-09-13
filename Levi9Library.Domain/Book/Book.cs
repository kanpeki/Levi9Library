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
		public string Title { get; set; }

		[Required]
		public string Author { get; set; }

		public int BookScore { get; set; }

		public int Stock { get; set; }

		public int BorrowedCount { get; set; } = 0;

		public bool IsDisabled { get; set; } = false;

		public virtual ICollection<UserBook> UserBooks { get; set; }
	}
}