using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Levi9Library.Core
{
	public static class LabelExtensions
	{
		public static MvcHtmlString LabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
																Expression<Func<TModel, TProperty>> ex,
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
