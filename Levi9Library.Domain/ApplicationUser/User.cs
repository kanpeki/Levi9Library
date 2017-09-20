using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Levi9LibraryDomain
{
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser
	{
		// custom property with default value
		public int UserScore { get; set; } = 0;
		public int OverdueCount { get; set; } = 0;
		public bool IsBanned { get; set; } = false;
		public DateTime? LastBannedDate { get; set; }
		public virtual ICollection<UserBook> UserBooks { get; set; }


		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

			// Add custom user claims here
			userIdentity.AddClaim(new Claim("UserScore", UserScore.ToString()));
			userIdentity.AddClaim(new Claim("OverdueCount", UserScore.ToString()));
			userIdentity.AddClaim(new Claim("IsBanned", IsBanned.ToString()));
			userIdentity.AddClaim(new Claim("LastBannedDate", IsBanned.ToString()));

			return userIdentity;
		}
	}

	public static class IdentityExtensions
	{
		public static string GetUserScore(this IIdentity identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException(nameof(identity));
			}
			var ci = identity as ClaimsIdentity;
			return ci?.FindFirstValue("UserScore");
		}

		public static string GetOverdueCount(this IIdentity identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException(nameof(identity));
			}
			var ci = identity as ClaimsIdentity;
			return ci?.FindFirstValue("OverdueCount");
		}

		public static string GetIsBanned(this IIdentity identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException(nameof(identity));
			}
			var ci = identity as ClaimsIdentity;
			return ci?.FindFirstValue("IsBanned");
		}

		public static string GetLastBannedDate(this IIdentity identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException(nameof(identity));
			}
			var ci = identity as ClaimsIdentity;
			return ci?.FindFirstValue("LastBannedDate");
		}
	}
}