using System.Web.Mvc;
using Levi9Library.Contracts;
using Levi9Library.Services;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Microsoft.Owin.Security;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Levi9Library.Models;
using System.Data.Entity;

namespace Levi9Library
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
	        container.RegisterType<DbContext, ApplicationDbContext>(
				new HierarchicalLifetimeManager());
			container.RegisterType<SignInManager<ApplicationUser, string>>(
				new HierarchicalLifetimeManager());
	        container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
	        container.RegisterType<IAuthenticationManager>(
		        new InjectionFactory(x => HttpContext.Current.GetOwinContext().Authentication));
	

			container.RegisterType<IBookService, BookService>();

			DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}