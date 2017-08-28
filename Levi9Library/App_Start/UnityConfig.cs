using Levi9Library.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Unity.Mvc5;

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
	        container.RegisterType<DbContext, Levi9LibraryDb>(
				new HierarchicalLifetimeManager());
			container.RegisterType<SignInManager<ApplicationUser, string>>(
				new HierarchicalLifetimeManager());
	        container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
	        container.RegisterType<IAuthenticationManager>(
		        new InjectionFactory(x => HttpContext.Current.GetOwinContext().Authentication));

			DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}