namespace Levi9Library.Models
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

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

	// generated

	//public partial class Book
	//{
	//	public Book()
	//	{
	//		UserBooks = new HashSet<UserBook>();
	//	}

	//	public int BookId { get; set; }

	//	[Required]
	//	public string Title { get; set; }

	//	[Required]
	//	public string Author { get; set; }

	//	public int Stock { get; set; }

	//	public int BookScore { get; set; }


	//	public virtual ICollection<UserBook> UserBooks { get; set; }
	//}
}