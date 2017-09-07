using System;

namespace Levi9Library.Core
{
	public static class LibraryManager
	{
		public static TimeSpan BorrowDuration { get; set; } = new TimeSpan(0, 0, 5);
		public static TimeSpan BanDuration { get; set; } = new TimeSpan(0, 0, 30);
		public static int MaxOverdueCount { get; set; } = 3;
		public static int MaxBooksPerUser { get; set; } = 5;
	}
}
