using System;
using System.Linq.Expressions;
using System.Web.WebPages;

namespace Levi9Library.Core
{
	using System.Web.Mvc;
	using System.Web.Mvc.Html;

	public static class HelperExtensions
	{
		public static MvcHtmlString CheckBoxSimple(this HtmlHelper htmlHelper,
			string name,
			object htmlAttributes)
		{
			string checkBoxWithHidden = htmlHelper.CheckBox(name, htmlAttributes).ToHtmlString().Trim();
			string pureCheckBox = checkBoxWithHidden.Substring(0, checkBoxWithHidden.IndexOf("<input", 1, StringComparison.Ordinal));
			return new MvcHtmlString(pureCheckBox);
		}

		public static MvcHtmlString LabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> ex,
			object htmlAttributes,
			Func<object, HelperResult> template)
		{
			var htmlFieldName = ExpressionHelper.GetExpressionText(ex);
			var forAttr = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);
			var label = new TagBuilder("label");
			label.Attributes["for"] = TagBuilder.CreateSanitizedId(forAttr);
			label.InnerHtml = template(null).ToHtmlString();
			return MvcHtmlString.Create(label.ToString());
		}
	}
}
