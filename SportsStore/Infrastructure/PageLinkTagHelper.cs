﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsStore.Models.ViewModels;

namespace SportsStore.Infrastructure
{
	[HtmlTargetElement("div", Attributes = "page-model")]
	public class PageLinkTagHelper(IUrlHelperFactory urlHelperFactory) : TagHelper
	{
		private IUrlHelperFactory urlHelperFactory = urlHelperFactory;

		[ViewContext]
		[HtmlAttributeNotBound]
		public ViewContext? ViewContext { get; set; }

		public PagingInfo? PageModel { get; set; }

		public string? PageAction { get; set; }

		public bool PageClassesEnabled { get; set; } = false;

		public string PageClass { get; set; } = String.Empty;

		public string PageClassNormal { get; set; } = String.Empty;

		public string PageClassSelected { get; set; } = String.Empty;

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (ViewContext != null && PageModel != null)
			{
				IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
				TagBuilder result = new("div");
				for (int i = 1; i <= PageModel.TotalPages; i++)
				{
					TagBuilder tag = new("a");
					tag.Attributes["href"] = urlHelper.Action(PageAction, new { productPage = i });
					tag.InnerHtml.Append(i.ToString());
					result.InnerHtml.AppendHtml(tag);
				}
				output.Content.AppendHtml(result.InnerHtml);
			}
		}
	}
}