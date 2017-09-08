using PagedList;

namespace Levi9Library.MVC.Models
{
	public class ManageViewModel
	{
		public IPagedList<ManageBookViewModel> WholeInventory;
		public IPagedList<ManageBookViewModel> CurrentInventory;
	}
}