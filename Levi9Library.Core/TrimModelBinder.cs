﻿using System.ComponentModel;
using System.Web.Mvc;

namespace Levi9Library.Core
{
	public class TrimModelBinder : DefaultModelBinder
	{
		protected override void SetProperty(ControllerContext controllerContext,
											ModelBindingContext bindingContext,
											PropertyDescriptor propertyDescriptor,
											object value)
		{
			if (propertyDescriptor.PropertyType == typeof(string))
			{
				var stringValue = (string)value;
				value = !string.IsNullOrWhiteSpace(stringValue) ? stringValue.Trim() : null;
			}
			base.SetProperty(controllerContext, bindingContext,
				propertyDescriptor, value);
		}
	}
}
