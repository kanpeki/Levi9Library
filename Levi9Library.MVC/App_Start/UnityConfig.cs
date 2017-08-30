using Levi9Library.Infrastructure;
using Levi9LibraryDomain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Levi9Library.Infrastructure.Repositories;
using Levi9LibraryServices;
using Unity.Mvc5;

namespace Levi9Library.MVC
{
	public static class UnityConfig
	{
		public static void RegisterComponents()
		{
			var container = new UnityContainer();

			// register all your components with the container here
			// it is NOT necessary to register your controllers

			// e.g. container.RegisterType<ITestService, TestService>();

			// Identity
			container.RegisterType<UserManager<ApplicationUser>>(
				new HierarchicalLifetimeManager());
			container.RegisterType<DbContext, Levi9LibraryDbContext>(
				new HierarchicalLifetimeManager());
			container.RegisterType<SignInManager<ApplicationUser, string>>(
				new HierarchicalLifetimeManager());
			container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
			container.RegisterType<IAuthenticationManager>(
				new InjectionFactory(x => HttpContext.Current.GetOwinContext().Authentication));

			// interfaces
			container.RegisterType<IUserRepository, UserRepository>();
			container.RegisterType<IUserService, UserService>();
			container.RegisterType<IBookRepository, BookRepository>();
			container.RegisterType<IBookService, BookService>();

			DependencyResolver.SetResolver(new UnityDependencyResolver(container));
		}
	}
}