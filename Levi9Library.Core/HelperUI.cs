using System;

namespace Levi9Library.Core
{
	using System.Web.Mvc;
	using System.Web.Mvc.Html;

	public static class HelperUI
	{
		public static MvcHtmlString CheckBoxSimple(this HtmlHelper htmlHelper, string name, object htmlAttributes)
		{
			string checkBoxWithHidden = htmlHelper.CheckBox(name, htmlAttributes).ToHtmlString().Trim();
			string pureCheckBox = checkBoxWithHidden.Substring(0, checkBoxWithHidden.IndexOf("<input", 1, StringComparison.Ordinal));
			return new MvcHtmlString(pureCheckBox);
		}
	}
}
