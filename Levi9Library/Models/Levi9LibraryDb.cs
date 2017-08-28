namespace Levi9Library.Models
{
	using Microsoft.AspNet.Identity.EntityFramework;
	using System;
	using System.Data.Entity;
	using System.Linq;

	//public partial class Levi9LibraryDb : DbContext
	//{
	//	public Levi9LibraryDb()
	//		: base("Levi9LibraryConnection") //connection string name
	//	{
	//	}

	//	public static Levi9LibraryDb Create()
	//	{
	//		return new Levi9LibraryDb();
	//	}

	//	public virtual DbSet<MigrationHistory> MigrationHistory { get; set; }
	//	public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
	//	public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
	//	public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
	//	public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
	//	public virtual DbSet<Book> Books { get; set; }
	//	public virtual DbSet<UserBook> UserBooks { get; set; }

	//	protected override void OnModelCreating(DbModelBuilder modelBuilder)
	//	{
	//		modelBuilder.Entity<AspNetRole>()
	//			.HasMany(e => e.AspNetUsers)
	//			.WithMany(e => e.AspNetRoles)
	//			.Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

	//		modelBuilder.Entity<AspNetUser>()
	//			.HasMany(e => e.AspNetUserClaims)
	//			.WithRequired(e => e.AspNetUser)
	//			.HasForeignKey(e => e.UserId);

	//		modelBuilder.Entity<AspNetUser>()
	//			.HasMany(e => e.AspNetUserLogins)
	//			.WithRequired(e => e.AspNetUser)
	//			.HasForeignKey(e => e.UserId);
	//	}
	//}

/*	public interface ILevi9LibraryDb : IDisposable
	{
		IQueryable<T> Query<T>() where T : class;
		void Add<T>(T entity) where T : class;
		void Update<T>(T entity) where T : class;
		void Remove<T>(T entity) where T : class;
		void SaveChanges();
	}*/


	public class Levi9LibraryDb : IdentityDbContext<ApplicationUser> //, ILevi9LibraryDb
	{
		public Levi9LibraryDb() : base("Levi9LibraryConnection", throwIfV1Schema: false)
		{

		}

		public static Levi9LibraryDb Create()
		{
			return new Levi9LibraryDb();
		}

		public DbSet<Book> Books { get; set; }

		public DbSet<UserBook> UserBooks { get; set; }

		//IQueryable<T> ILevi9LibraryDb.Query<T>()
		//{
		//	return Set<T>();
		//}

		//void ILevi9LibraryDb.Add<T>(T entity)
		//{
		//	Set<T>().Add(entity);
		//}

		//void ILevi9LibraryDb.Update<T>(T entity)
		//{
		//	Entry(entity).State = EntityState.Modified;
		//}

		//void ILevi9LibraryDb.Remove<T>(T entity)
		//{
		//	Set<T>().Remove(entity);
		//}

		//void ILevi9LibraryDb.SaveChanges()
		//{
		//	SaveChanges();
		//}
	}
}