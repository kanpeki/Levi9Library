using Levi9Library.DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Levi9Library.Models
{
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser
    {
		// custom property with default value
	    public int UserScore { get; set; } = 0;
		public virtual ICollection<UserBook> UserBooks { get; set; }


		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
	        userIdentity.AddClaim(new Claim("UserScore", UserScore.ToString()));

	        return userIdentity;
		}
	}

	public static class IdentityExtensions
	{
		public static string GetUserScore(this IIdentity identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			var ci = identity as ClaimsIdentity;
			return ci?.FindFirstValue("UserScore");
		}
	}

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
	    public DbSet<Book> Books { get; set; }
	    public DbSet<UserBook> UserBooks { get; set; }


		public ApplicationDbContext()
			: base("Levi9LibraryConnection", throwIfV1Schema: false)
		{
		}

		public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

/*	    
	    public int SaveChanges(bool refreshOnConcurrencyException, RefreshMode refreshMode = RefreshMode.ClientWins)
	    {
		    try
		    {
			    return SaveChanges();
		    }
		    catch (DbUpdateConcurrencyException ex)
		    {
			    foreach (DbEntityEntry entry in ex.Entries)
			    {
				    if (refreshMode == RefreshMode.ClientWins)
					    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
				    else
					    entry.Reload();
			    }
			    return SaveChanges();
		    }
	    }
*/
	}
}