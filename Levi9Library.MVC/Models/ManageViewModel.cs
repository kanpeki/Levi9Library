using System.ComponentModel.DataAnnotations;
using Levi9LibraryDomain;
using PagedList;

namespace Levi9Library.MVC.Models
{
	public class ManageViewModel
	{
		public IPagedList<Book> Inventory;

		public string SearchString { get; set; }

		[Display(Name = " ")]
		public bool OldInventoryIsShown { get; set; }
	}
}