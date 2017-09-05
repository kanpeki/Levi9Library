using System.Web.Mvc;

namespace Levi9Library.MVC.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return RedirectToAction("Index", "Book");
		}
	}
}