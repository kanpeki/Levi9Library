﻿using System.Web.Mvc;

namespace Levi9Library.MVC
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new ErrorHandler.AiHandleErrorAttribute());
		}
	}
}
