namespace Levi9Library.Models
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public class UserBook
	{
		[Key, ForeignKey("ApplicationUser"), Column(Order = 0)]
		public string Id { get; set; } //nvarchar(128)

		[Key, ForeignKey("Book"), Column(Order = 1)]
		public int BookId { get; set; }

		[Key, Column(Order = 2, TypeName = "datetime2")]
		[Display(Name = "Date Borrowed")]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime DateBorrowed { get; set; }

		[Column(TypeName = "datetime2")]
		[Display(Name = "Date Returned")]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
		public DateTime DateReturned { get; set; }

		public virtual ApplicationUser ApplicationUser { get; set; }
		public virtual Book Book { get; set; }
	}

	// generated

	//public partial class UserBook
	//{
	//	[Key]
	//	[Column(Order = 0)]
	//	public string Id { get; set; }

	//	[Key]
	//	[Column(Order = 1)]
	//	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	//	public int BookId { get; set; }

	//	[Key]
	//	[Column(Order = 2)]
	//	public DateTime DateBorrowed { get; set; }

	//	public DateTime? DateReturned { get; set; }

	//	public virtual AspNetUser AspNetUser { get; set; }

	//	public virtual Book Book { get; set; }
	//}
}